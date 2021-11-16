using System;

namespace EmulatorHost.Interpreter
{
	public sealed class VisitorTokenHandlerException : Exception
	{
		public VisitorTokenHandlerException(string message) : base(message)
		{
		}
	}
}