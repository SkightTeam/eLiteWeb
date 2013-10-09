using System;
using FluentBuild.ApplicationProperties;
using NUnit.Framework;

namespace FluentBuild
{
    ///<summary />
	[TestFixture]
    public class PropertiesTests
    {
        ///<summary />
	[Test]
        public void TeamCityShouldReturnProperObject()
        {
            Assert.That(Properties.TeamCity, Is.TypeOf<TeamCityProperties>());
        }

        ///<summary />
	[Test]
        public void CruiseControlShouldReturnProperObject()
        {
            Assert.That(Properties.CruiseControl, Is.TypeOf<CruiseControlProperties>());
        }

        ///<summary />
	[Test]
        public void CommandLineShouldReturnProperObject()
        {
            Assert.That(Properties.CommandLineProperties, Is.TypeOf<CommandLineProperties>());
        }

        ///<summary />
	[Test]
        public void CurrentDirectoryShouldBeProper()
        {
            Assert.That(Properties.CurrentDirectory, Is.EqualTo(Environment.CurrentDirectory));
        }
    }
}