using System;

namespace PluginInterpreter
{
	public sealed class VisitorTokenHandlerException : Exception
	{
		public VisitorTokenHandlerException(string message) : base(message)
		{
		}
	}
}