using System;
using System.Text;

namespace FluentBuild.AssemblyInfoBuilding
{
    public class VisualBasicAssemblyInfoBuilder : IAssemblyInfoBuilder
    {
        public string Build(IAssemblyInfoDetails details)
        {
            var sb = new StringBuilder();
            details.Imports.Sort();
            foreach (var import in details.Imports)
            {
                sb.AppendFormat("imports {0}{1}", import, Environment.NewLine);
            }

            foreach (var item in details.LineItems)
            {
                if (item.IsQuotedValue)
                    sb.AppendFormat("<assembly: {0}(\"{1}\")>{2}", item.Name, item.Value, Environment.NewLine);
                else
                    sb.AppendFormat("<assembly: {0}({1})>{2}", item.Name, item.Value, Environment.NewLine);
            }

//            if (details.ComVisibleSet)
//                sb.AppendFormat("<assembly: ComVisible({0})>{1}", details.AssemblyComVisible.ToString().ToLower(), Environment.NewLine);
//
//            if (details.ClsCompliantSet)
//                sb.AppendFormat("<assembly: CLSCompliant({0})>{1}", details.AssemblyClsCompliant.ToString().ToLower(), Environment.NewLine);
//
//            if (!String.IsNullOrEmpty(details.AssemblyVersion.ToString()))
//                sb.AppendFormat("<assembly: AssemblyVersion(\"{0}\")>{1}", details.AssemblyVersion, Environment.NewLine);
//            sb.AppendFormat("<assembly: AssemblyTitle(\"{0}\")>{1}", details.AssemblyTitle, Environment.NewLine);
//            sb.AppendFormat("<assembly: AssemblyDescription(\"{0}\")>{1}", details.AssemblyDescription, Environment.NewLine);
//            sb.AppendFormat("<assembly: AssemblyCopyright(\"{0}\")>{1}", details.AssemblyCopyright, Environment.NewLine);
            //sb.AppendFormat("[assembly: ApplicationNameAttribute(\"{0}\")]{1}", details._applicationName, Environment.NewLine);
//            sb.AppendFormat("<assembly: AssemblyCompany(\"{0}\")>{1}", details.AssemblyCompany, Environment.NewLine);
//            sb.AppendFormat("<assembly: AssemblyProduct(\"{0}\")>{1}", details.AssemblyProduct, Environment.NewLine);
            return sb.ToString();
        }
    }
}