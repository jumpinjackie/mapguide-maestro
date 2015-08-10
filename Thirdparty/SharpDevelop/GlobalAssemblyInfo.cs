// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Daniel Grunwald" email="daniel@danielgrunwald.de"/>
//     <version>$Revision: 1040 $</version>
// </file>

/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                         //
// DO NOT EDIT GlobalAssemblyInfo.cs, it is recreated using AssemblyInfo.template whenever //
// StartUp is compiled.                                                                    //
//                                                                                         //
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////

using System.Resources;
using System.Reflection;

[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: AssemblyCompany("ic#code")]
[assembly: AssemblyProduct("SharpDevelop")]
[assembly: AssemblyCopyright("2000-2012 AlphaSierraPapa")]
[assembly: AssemblyVersion(RevisionClass.FullVersion)]
[assembly: NeutralResourcesLanguage("en-US")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2243:AttributeStringLiteralsShouldParseCorrectly",
	Justification = "AssemblyInformationalVersion does not need to be a parsable version")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancies in Code", "RECS0145:Removes 'private' modifiers that are not required", Justification = "The author likes to be explicit with accessibility modifiers")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancies in Code", "RECS0129:Removes 'internal' modifiers that are not required", Justification = "The author likes to be explicit with accessibility modifiers")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Language", "CSE0003:Use expression-bodied members", Justification = "The author prefers debuggability over succintness")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancies in Symbol Declarations", "RECS0007:The default underlying type of enums is int, so defining it explicitly is redundant.", Justification = "The author prefers to be explicit about underlying enum types")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancies in Symbol Declarations", "RECS0072", Justification = "The author prefers explicit context for non-field member access")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancies in Symbol Declarations", "IDE0003", Justification = "The author prefers explicit context for non-field member access")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Virtual member call in constructor", "RECS0021")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Use 'var' keyword", "RECS0091")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Nullable type can be simplified", "RECS0013")]

internal static class RevisionClass
{
	public const string Major = "4";
	public const string Minor = "2";
	public const string Build = "0";
	public const string Revision = "8783";
	
	public const string MainVersion = Major + "." + Minor;
	public const string FullVersion = Major + "." + Minor + "." + Build + "." + Revision;
}
