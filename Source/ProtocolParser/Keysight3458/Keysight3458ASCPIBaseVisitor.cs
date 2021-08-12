//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.9.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:\Develop\Open-SCPI-Protocol-Emulator\ProtocolParser\Keysight3458\Keysight3458ASCPI.g4 by ANTLR 4.9.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace ProtocolParser.Keysight3458A {
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IKeysight3458ASCPIVisitor{Result}"/>,
/// which can be extended to create a visitor which only needs to handle a subset
/// of the available methods.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.9.1")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CLSCompliant(false)]
public partial class Keysight3458ASCPIBaseVisitor<Result> : AbstractParseTreeVisitor<Result>, IKeysight3458ASCPIVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="Keysight3458ASCPIParser.command"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitCommand([NotNull] Keysight3458ASCPIParser.CommandContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="Keysight3458ASCPIParser.identificationQuery"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitIdentificationQuery([NotNull] Keysight3458ASCPIParser.IdentificationQueryContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="Keysight3458ASCPIParser.readQuery"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitReadQuery([NotNull] Keysight3458ASCPIParser.ReadQueryContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="Keysight3458ASCPIParser.abortCommand"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitAbortCommand([NotNull] Keysight3458ASCPIParser.AbortCommandContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="Keysight3458ASCPIParser.configureCurrentCommand"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitConfigureCurrentCommand([NotNull] Keysight3458ASCPIParser.ConfigureCurrentCommandContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="Keysight3458ASCPIParser.configureVoltageCommand"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitConfigureVoltageCommand([NotNull] Keysight3458ASCPIParser.ConfigureVoltageCommandContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="Keysight3458ASCPIParser.measureCurrentQuery"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitMeasureCurrentQuery([NotNull] Keysight3458ASCPIParser.MeasureCurrentQueryContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="Keysight3458ASCPIParser.measureVoltageQuery"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitMeasureVoltageQuery([NotNull] Keysight3458ASCPIParser.MeasureVoltageQueryContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="Keysight3458ASCPIParser.currentParameters"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitCurrentParameters([NotNull] Keysight3458ASCPIParser.CurrentParametersContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="Keysight3458ASCPIParser.voltageParameters"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitVoltageParameters([NotNull] Keysight3458ASCPIParser.VoltageParametersContext context) { return VisitChildren(context); }
}
} // namespace ProtocolParser.Keysight3458A
