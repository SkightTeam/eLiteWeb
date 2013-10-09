using System;
using FluentBuild.Utilities;

namespace FluentBuild.Runners.Zip
{
    public interface IZipOptions
    {
        void Compress(Action<ZipCompress> args);
        void Decompress(Action<ZipDecompress> args);
    }

    public class ZipOptions : IZipOptions
    {
        private readonly IActionExcecutor _actionExcecutor;

        public ZipOptions() : this(new ActionExcecutor())
        {
        }

        public ZipOptions(IActionExcecutor actionExcecutor)
        {
            _actionExcecutor = actionExcecutor;
        }

        public void Compress(Action<ZipCompress> args)
        {
            _actionExcecutor.Execute(args);
        }

        public void Decompress(Action<ZipDecompress> args)
        {
            _actionExcecutor.Execute(args);
        }
    }
}