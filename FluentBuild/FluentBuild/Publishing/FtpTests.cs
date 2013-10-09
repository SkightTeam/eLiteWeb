using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace FluentBuild.Publishing
{
    [TestFixture]
    public class FtpTests
    {
        [Test]
        public void TestFtp()
        {
//            Task.Publish(x => x.Ftp.Server("dummy.com").UserName("username").Password("password").RemoteFilePath("/html/Installers/Test/").LocalFilePath("c:\\temp\\stuff.txt"));
        }
    }
}
