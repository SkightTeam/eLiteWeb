using System;
using System.Text;
using NUnit.Framework;


namespace FluentBuild.AssemblyInfoBuilding
{
    ///<summary>
    ///</summary>
    ///<summary />
	[TestFixture]
    public class CSharpAssemblyInfoBuilderTests
    {
        ///<summary>
        ///</summary>
        ///<summary />
	[Test]
        public void ShouldBuildString()
        {
            var builder = new CSharpAssemblyInfoBuilder();
            var details = new AssemblyInfoDetails(builder).ComVisible(false).ClsCompliant(false).Version("1.0.0.0").Title("asmTitle").Description("asmDesc").Copyright("asmCopyright").Company("Company").Product("My Product");
           
            
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Reflection;");
            sb.AppendLine("using System.Runtime.InteropServices;");

            sb.AppendLine("[assembly: ComVisible(false)]");
            sb.AppendLine("[assembly: CLSCompliant(false)]");
            sb.AppendLine("[assembly: AssemblyVersionAttribute(\"1.0.0.0\")]");
            sb.AppendLine("[assembly: AssemblyTitleAttribute(\"asmTitle\")]");
            sb.AppendLine("[assembly: AssemblyDescriptionAttribute(\"asmDesc\")]");
            sb.AppendLine("[assembly: AssemblyCopyrightAttribute(\"asmCopyright\")]");
            sb.AppendLine("[assembly: AssemblyCompanyAttribute(\"Company\")]");
            sb.AppendLine("[assembly: AssemblyProductAttribute(\"My Product\")]");

            //sb.AppendFormat("[assembly: ApplicationNameAttribute(\"{0}\")]{1}", details._applicationName, Environment.NewLine);
//            sb.AppendFormat("[assembly: AssemblyCompany(\"{0}\")]{1}", details.AssemblyCompany, Environment.NewLine);
//            sb.AppendFormat("[assembly: AssemblyProduct(\"{0}\")]{1}", details.AssemblyProduct, Environment.NewLine);
            Assert.That(builder.Build(details).Trim(), Is.EqualTo(sb.ToString().Trim()));
        }
    }
}