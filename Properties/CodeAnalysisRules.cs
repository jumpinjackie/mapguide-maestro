#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
//

#endregion Disclaimer / License

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

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
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0022", Justification = "If I'm swallowing up an exception, it is intentional")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Language Usage Opportunities", "RECS0011", Justification = "It's if/else because it's more readable that a single long ternary expression")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Common Practices and Code Improvements", "RECS0030", Justification = "<Pending>")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0025", Justification = "<Pending>")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "CS0108", Justification = "Shadowing is the result of constraints imposed by interfaces we're implementing")]

//TODO: Review these ones as we get further clarity about what string comparisons should be culture-sensitive and which ones shouldn't be
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Common Practices and Code Improvements", "RECS0060", Justification = "To be reviewed at a later time")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Common Practices and Code Improvements", "RECS0061", Justification = "To be reviewed at a later time")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Common Practices and Code Improvements", "RECS0062", Justification = "To be reviewed at a later time")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Common Practices and Code Improvements", "RECS0063", Justification = "To be reviewed at a later time")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Common Practices and Code Improvements", "RECS0064", Justification = "To be reviewed at a later time")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Common Practices and Code Improvements", "RECS0119", Justification = "To be reviewed at a later time")]