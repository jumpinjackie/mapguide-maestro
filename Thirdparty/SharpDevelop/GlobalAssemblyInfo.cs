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
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Replace with '|='", "RECS0033", Justification = "The author prefers clarity of code")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Replace with '&='", "RECS0093", Justification = "The author prefers clarity of code")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Not implemented", "RECS0083", Justification = "A NotImplementedException is thrown for a reason")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0029", Justification = "Needed to satisfy an XML serialization contract")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancies in Symbol Declarations", "RECS0001", Justification = "Other parts may exist in the future")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancies in Code", "RECS0071", Justification = "This is just plain pedantry")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancies in Code", "RECS0138", Justification = "The author prefers to be explicit")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancies in Symbol Declarations", "RECS0004", Justification = "If one is specified, it's to satisfy an XML serialization contract")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Common Practices and Code Improvements", "RECS0060", Justification = "To be reviewed at a later time")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Common Practices and Code Improvements", "RECS0061", Justification = "To be reviewed at a later time")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Common Practices and Code Improvements", "RECS0062", Justification = "To be reviewed at a later time")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Common Practices and Code Improvements", "RECS0063", Justification = "To be reviewed at a later time")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Language", "CSE0001", Justification = "<Pending>")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0163", Justification = "<Pending>")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Common Practices and Code Improvements", "RECS0092", Justification = "<Pending>")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Language Usage Opportunities", "RECS0011", Justification = "<Pending>")]

internal static class RevisionClass
{
	public const string Major = "4";
	public const string Minor = "2";
	public const string Build = "0";
	public const string Revision = "8783";
	
	public const string MainVersion = Major + "." + Minor;
	public const string FullVersion = Major + "." + Minor + "." + Build + "." + Revision;
}
