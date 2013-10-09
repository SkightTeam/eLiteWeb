using System;
using FluentFs.Core;

namespace FluentFs.Support
{
    internal class FailableActionExecutor
    {
        internal static void DoAction<T1, T2>(OnError onError, Action<T1, T2> action, T1 param1, T2 param2)
        {
            try
            {
                action.Invoke(param1, param2);
            }
            catch (Exception e)
            {
                if (onError == OnError.Fail)
                    throw;
//                Logger.WriteDebugMessage("An error occured but ContinueOnError was set. Error: " + e);
            }
        }

        internal static void DoAction<T>(OnError onError, Action<T> action, T param)
        {
            try
            {
                action.Invoke(param);
            }
            catch (Exception e)
            {
                if (onError == OnError.Fail)
                    throw;
  //              Logger.WriteDebugMessage("An error occured but ContinueOnError was set. Error: " + e);
            }
        }
    }
}