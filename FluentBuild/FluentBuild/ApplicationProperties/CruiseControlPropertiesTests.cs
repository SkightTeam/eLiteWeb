
using System;
using System.Diagnostics.CodeAnalysis;

using NUnit.Framework;

namespace FluentBuild.ApplicationProperties
{
    ///<summary />
	[TestFixture]
    public class CruiseControlPropertiesTests
    {
        ///<summary />
    	[Test]
        public void AllPropertiesShouldMap()
        {
            var subject = new CruiseControlProperties();
            DateTime buildDate = DateTime.Now;
            string projectName = "projectName";
            string lastbuild = "2346234";
            string cctimestamp = "246347";
            string lastSuccessfulBuild = "346346";
            string label = "label";
            string interval="234656";
            string lastbuildsuccessful = "true";
            string logdir = "c:\\temp";
            string logfile = "mylog.txt";

            Properties.CommandLineProperties.Properties.Add("projectname", projectName);
            Properties.CommandLineProperties.Properties.Add("lastbuild", lastbuild);
            Properties.CommandLineProperties.Properties.Add("lastsuccessfulbuild", lastSuccessfulBuild);
            Properties.CommandLineProperties.Properties.Add("builddate", buildDate.ToString());
            Properties.CommandLineProperties.Properties.Add("cctimestamp", cctimestamp);
            Properties.CommandLineProperties.Properties.Add("label", label);
            Properties.CommandLineProperties.Properties.Add("interval", interval);
            Properties.CommandLineProperties.Properties.Add("lastbuildsuccessful", lastbuildsuccessful);
            Properties.CommandLineProperties.Properties.Add("logdir", logdir);
            Properties.CommandLineProperties.Properties.Add("logfile", logfile);


            Assert.That(subject.BuildDate.ToString(), Is.EqualTo(buildDate.ToString()));
            Assert.That(subject.Interval, Is.EqualTo(int.Parse(interval)));
            Assert.That(subject.Label , Is.EqualTo(label));
            Assert.That(subject.LastBuild, Is.EqualTo(lastbuild));
            Assert.That(subject.LastBuildSuccessful, Is.True);
            Assert.That(subject.LastSuccessfulBuild , Is.EqualTo(lastSuccessfulBuild ));
            Assert.That(subject.LogDirectory , Is.EqualTo(logdir));
            Assert.That(subject.LogFile, Is.EqualTo(logfile ));
            Assert.That(subject.ProjectName , Is.EqualTo(projectName ));
            Assert.That(subject.Timestamp, Is.EqualTo(cctimestamp));

        }
    }
}