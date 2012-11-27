using System;
using Machine.Specifications.AutoMocking.Core;
using Rhino.Mocks;

namespace Machine.Specifications.AutoMocking.Rhino
{
	public class RhinoMocksMockFactory : IMockFactory
	{
		public Dependency create_stub<Dependency>() where Dependency : class
		{
			return MockRepository.GenerateStub<Dependency>();
		}

		public object create_stub(Type type)
		{
			return MockRepository.GenerateStub(type);
		}
	}
}