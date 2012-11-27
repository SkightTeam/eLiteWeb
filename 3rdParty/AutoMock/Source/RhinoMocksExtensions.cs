using System;
using Rhino.Mocks;

namespace Machine.Specifications.AutoMocking.Rhino
{
	public static class RhinoMocksExtensions
	{
		static public void was_told_to<T>(this T mock, Action<T> action) where T : class
		{
			received(mock, action);
		}

		static public void was_never_told_to<T>(this T mock, Action<T> action) where T : class
		{
			never_received(mock, action);
		}

		static public void received<T>(this T mock, Action<T> action) where T : class
		{
			mock.AssertWasCalled(action, o=>o.Repeat.AtLeastOnce());
		}

		static public void never_received<T>(this T mock, Action<T> action) where T : class
		{
			mock.AssertWasNotCalled(action);
		}
	}
}