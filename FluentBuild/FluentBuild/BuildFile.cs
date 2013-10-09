using System;
using System.Collections.Generic;

namespace FluentBuild
{
    ///<summary>
    /// Represents a Build file that can be run
    ///</summary>
    public class BuildFile
    {
        //TODO: make this a readonly wrapper. It only needs to be exposed for testing at this point
        public static bool IsInErrorState;
        internal Queue<NamedTask> Tasks;

        ///<summary>
        /// Instantiates a build file and initializes the Tasks queue.
        ///</summary>
        public BuildFile()
        {
            Tasks = new Queue<NamedTask>();
        }

        protected internal static void SetErrorState()
        {
                IsInErrorState = true;
                Environment.ExitCode = 1;
        }

        ///<summary>
        /// Gets the number of tasks in the queue.
        ///</summary>
        ///<returns>The number of tasks in the queue</returns>
        public int TaskCount
        {
            get { return Tasks.Count; }
        }

        ///<summary>
        /// Invokes the next task in the queue
        ///</summary>
        public void InvokeNextTask()
        {
            while (Tasks.Count > 0)
            {
                NamedTask task = Tasks.Dequeue();
                //do not run another task if a previous task has errored
                Defaults.Logger.WriteHeader(task.Name);
                try
                {
                    task.Task.Invoke();
                }
                catch (Exception ex)
                {
                    Defaults.Logger.WriteError("ERROR", ex.ToString());
                    BuildFile.SetErrorState();
                }

                if (IsInErrorState) 
                    return; //stop executing tasks if non-zero error code
            }

            if (IsInErrorState == false)
                Defaults.Logger.WriteHeader("DONE");
        }

        ///<summary>
        /// Clears the task list
        ///</summary>
        public void ClearTasks()
        {
            Tasks.Clear();
        }

        ///<summary>
        /// Adds a task for fb.exe to run in the order that it should be run
        ///</summary>
        ///<param name="task">The method to run</param>
        public void AddTask(Action task)
        {
            Tasks.Enqueue(new NamedTask(task.Method.Name, task));
        }

        ///<summary>
        /// Adds a task for fb.exe to run in the order that it should be run
        ///</summary>
        ///<param name="task">The method to run</param>
        ///<param name="name">The name of the task (will be displayed when the task is run)</param>
        public void AddTask(string name, Action task)
        {
            Tasks.Enqueue(new NamedTask(name, task));
        }

        #region Nested type: NamedTask

        internal class NamedTask
        {
            public NamedTask(string name, Action task)
            {
                Name = name;
                Task = task;
            }

            public string Name { get; set; }
            public Action Task { get; set; }
        }

        #endregion
    }
}