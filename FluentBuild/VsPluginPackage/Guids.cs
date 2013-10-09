// Guids.cs
// MUST match guids.h
using System;

namespace FluentBuild.VsPluginPackage
{
    static class GuidList
    {
        public const string guidVsPluginPackagePkgString = "d625ce10-9354-47a0-a0f9-6097e6f201be";
        public const string guidVsPluginPackageCmdSetString = "b2e0b2dd-9a43-4741-8c25-0e3e88044ceb";
        public const string guidToolWindowPersistanceString = "f0d78530-d9b6-4313-95bd-ca7b19e7d2cd";

        public static readonly Guid guidVsPluginPackageCmdSet = new Guid(guidVsPluginPackageCmdSetString);
    };
}