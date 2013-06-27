using System;
using FluentBuild;

namespace Skight.HelpCenter.Build {
    public class Default:BuildFile 
    {
        public Default()
        {
            AddTask("Start",Start);
        }
        void Start()
        {
            Console.WriteLine("Start Step");
        }
    }
}
