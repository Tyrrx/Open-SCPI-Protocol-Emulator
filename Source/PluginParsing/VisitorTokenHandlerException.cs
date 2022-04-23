using System;

namespace PluginParsing
{
	public sealed class VisitorTokenHandlerException : Exception
	{
		public VisitorTokenHandlerException(string message) : base(message)
		{
		}
	}
}