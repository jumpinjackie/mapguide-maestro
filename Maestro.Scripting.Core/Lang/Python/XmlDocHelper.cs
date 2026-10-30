#region Disclaimer / License

// Copyright (C) 2025, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Maestro.Scripting.Core.Lang.Python
{
    /// <summary>
    /// Loads .NET XML documentation files and provides lookup of member summaries.
    /// Used as a fallback when the DLR's <c>GetDocumentation()</c> returns nothing,
    /// so C# doc comments appear in IronPython auto-complete tooltips.
    /// </summary>
    public static class XmlDocHelper
    {
        /// <summary>
        /// Holds the summary and parameter descriptions for a single XML doc member.
        /// </summary>
        private sealed class MemberDocInfo
        {
            public string? Summary;
            public Dictionary<string, string>? Parameters;
        }

        /// <summary>
        /// Cache of assembly name → (memberId → documentation info).
        /// </summary>
        private static readonly ConcurrentDictionary<string, Dictionary<string, MemberDocInfo>> _cache = new();

        /// <summary>
        /// Gets the documentation summary for a .NET member from its assembly's XML doc file.
        /// </summary>
        public static string? GetMemberDoc(object member)
        {
            var info = LookupMemberDoc(member);
            return info?.Summary;
        }

        /// <summary>
        /// Gets the full documentation for a .NET member including summary and parameter descriptions,
        /// formatted for display in a tooltip.
        /// </summary>
        public static string? GetFullMemberDoc(object member)
        {
            var info = LookupMemberDoc(member);
            if (info == null)
                return null;

            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(info.Summary))
                sb.Append(info.Summary);

            if (info.Parameters != null && info.Parameters.Count > 0)
            {
                if (sb.Length > 0)
                    sb.AppendLine();
                sb.AppendLine("Parameters:"); //NOXLATE
                foreach (var kvp in info.Parameters)
                {
                    sb.Append("  ");
                    sb.Append(kvp.Key);
                    sb.Append(": ");
                    sb.AppendLine(kvp.Value);
                }
            }

            return sb.Length > 0 ? sb.ToString().TrimEnd() : null;
        }

        /// <summary>
        /// Looks up the documentation info for a .NET member, handling IronPython wrappers
        /// and method overload matching.
        /// </summary>
        private static MemberDocInfo? LookupMemberDoc(object member)
        {
            if (member == null)
                return null;

            try
            {
                MemberInfo? mi = member as MemberInfo;
                if (mi == null)
                {
                    // IronPython wraps .NET members; try to unwrap
                    var type = member.GetType();

                    // 1. Try a direct "Member" property (some wrappers)
                    var miProp = type.GetProperty("Member", BindingFlags.Public | BindingFlags.Instance);
                    if (miProp != null)
                        mi = miProp.GetValue(member, null) as MemberInfo;

                    // 2. IronPython's BuiltinFunction wraps .NET methods with an Overloads list
                    if (mi == null)
                    {
                        var overloadsProp = type.GetProperty("Overloads", BindingFlags.Public | BindingFlags.Instance);
                        if (overloadsProp != null)
                        {
                            var overloads = overloadsProp.GetValue(member, null) as System.Collections.IList;
                            if (overloads != null && overloads.Count > 0)
                                mi = overloads[0] as MemberInfo;
                        }
                    }
                }

                if (mi == null)
                    return null;

                var asm = mi.DeclaringType?.Assembly;
                if (asm == null)
                    return null;

                var docs = GetAssemblyDocs(asm);
                if (docs == null)
                    return null;

                // Build the XML member ID prefix
                string prefix = BuildMemberIdPrefix(mi);
                if (prefix == null)
                    return null;

                // Try exact match first, then prefix match (for methods with overloads)
                if (docs.TryGetValue(prefix, out var info))
                    return info;

                // For methods, try prefix match (without parameter list)
                if (mi.MemberType == MemberTypes.Method)
                {
                    foreach (var kvp in docs)
                    {
                        if (kvp.Key.StartsWith(prefix, StringComparison.Ordinal))
                            return kvp.Value;
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Loads (or retrieves from cache) the XML documentation dictionary for an assembly.
        /// </summary>
        private static Dictionary<string, MemberDocInfo>? GetAssemblyDocs(Assembly asm)
        {
            string name = asm.GetName().Name!;
            if (_cache.TryGetValue(name, out var cached))
                return cached;

            string? xml = LoadXmlFile(asm);
            if (xml == null)
            {
                _cache[name] = new Dictionary<string, MemberDocInfo>(); // store empty to avoid re-try
                return null;
            }

            var docs = ParseXmlDoc(xml);
            _cache[name] = docs;
            return docs;
        }

        /// <summary>
        /// Finds the XML documentation file for an assembly.
        /// </summary>
        private static string? LoadXmlFile(Assembly asm)
        {
            string fileName = asm.GetName().Name + ".xml"; //NOXLATE

            // 1. Alongside the assembly
            try
            {
                if (!string.IsNullOrEmpty(asm.Location))
                {
                    string? dir = Path.GetDirectoryName(asm.Location);
                    if (dir != null)
                    {
                        string path = Path.Combine(dir, fileName);
                        if (File.Exists(path))
                            return File.ReadAllText(path);
                    }
                }
            }
            catch { }

            // 2. Application base directory
            string appBase = AppDomain.CurrentDomain.BaseDirectory;
            string appBaseXml = Path.Combine(appBase, fileName);
            if (File.Exists(appBaseXml))
                return File.ReadAllText(appBaseXml);

            // 3. Entry assembly directory
            var entryAsm = Assembly.GetEntryAssembly();
            if (entryAsm != null)
            {
                try
                {
                    string? entryDir = Path.GetDirectoryName(entryAsm.Location);
                    if (!string.IsNullOrEmpty(entryDir))
                    {
                        string entryXml = Path.Combine(entryDir, fileName);
                        if (File.Exists(entryXml))
                            return File.ReadAllText(entryXml);
                    }
                }
                catch { }
            }

            return null;
        }

        /// <summary>
        /// Parses an XML documentation file into a dictionary of member ID → MemberDocInfo.
        /// </summary>
        private static Dictionary<string, MemberDocInfo> ParseXmlDoc(string xml)
        {
            var result = new Dictionary<string, MemberDocInfo>();
            using (var reader = XmlReader.Create(new StringReader(xml)))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "member") //NOXLATE
                    {
                        string? memberName = reader.GetAttribute("name"); //NOXLATE
                        if (string.IsNullOrEmpty(memberName))
                            continue;

                        var info = ReadMemberElement(reader);
                        if (info != null)
                            result[memberName] = info;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Reads &lt;summary&gt; and &lt;param&gt; elements from inside a &lt;member&gt; element.
        /// </summary>
        private static MemberDocInfo? ReadMemberElement(XmlReader reader)
        {
            string? summary = null;
            var parameters = new Dictionary<string, string>();
            int depth = reader.Depth;

            while (reader.Read())
            {
                // Stop when we leave the <member> element
                if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "member" && reader.Depth == depth) //NOXLATE
                    break;

                if (reader.NodeType != XmlNodeType.Element || reader.Depth != depth + 1)
                    continue;

                if (reader.Name == "summary") //NOXLATE
                {
                    summary = NormalizeXml(reader.ReadInnerXml());
                }
                else if (reader.Name == "param") //NOXLATE
                {
                    string? paramName = reader.GetAttribute("name"); //NOXLATE
                    if (!string.IsNullOrEmpty(paramName))
                    {
                        string? paramDesc = NormalizeXml(reader.ReadInnerXml());
                        if (!string.IsNullOrEmpty(paramDesc))
                            parameters[paramName] = paramDesc;
                    }
                }
            }

            if (summary == null && parameters.Count == 0)
                return null;

            return new MemberDocInfo { Summary = summary, Parameters = parameters.Count > 0 ? parameters : null };
        }

        /// <summary>
        /// Strips XML tags and normalizes whitespace in documentation text.
        /// </summary>
        private static string NormalizeXml(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return string.Empty;

            // Strip inline XML tags like <see cref="..."/>, <paramref name="..."/>, <c>, etc.
            raw = Regex.Replace(raw, @"<[^>]+>", string.Empty);

            // Normalize whitespace: collapse multiple spaces/newlines into single space
            raw = Regex.Replace(raw, @"\s+", " ").Trim();

            return raw;
        }

        /// <summary>
        /// Builds the XML doc member ID prefix for a reflection MemberInfo.
        /// Returns null if the member type is unsupported.
        /// </summary>
        private static string? BuildMemberIdPrefix(MemberInfo mi)
        {
            if (mi.DeclaringType == null)
                return null;

            string typeName = GetXmlTypeName(mi.DeclaringType);
            char prefix;

            switch (mi.MemberType)
            {
                case MemberTypes.Method:
                case MemberTypes.Constructor:
                    prefix = 'M';
                    break;
                case MemberTypes.Property:
                    prefix = 'P';
                    break;
                case MemberTypes.Field:
                    prefix = 'F';
                    break;
                case MemberTypes.Event:
                    prefix = 'E';
                    break;
                case MemberTypes.TypeInfo:
                case MemberTypes.NestedType:
                    prefix = 'T';
                    return $"{prefix}:{GetXmlTypeName((Type)mi)}";
                default:
                    return null;
            }

            return $"{prefix}:{typeName}.{mi.Name}";
        }

        /// <summary>
        /// Gets the XML doc type name for a Type (e.g. "Namespace.OuterClass.InnerClass").
        /// </summary>
        private static string GetXmlTypeName(Type type)
        {
            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                // Use the generic type definition name without arity
                type = type.GetGenericTypeDefinition();
            }

            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(type.Namespace))
            {
                sb.Append(type.Namespace);
                sb.Append('.');
            }

            // Handle nested types
            var stack = new Stack<string>();
            Type? current = type;
            while (current != null)
            {
                string name = current.Name;
                // Strip generic arity suffix like `2
                int backtick = name.IndexOf('`');
                if (backtick >= 0)
                    name = name.Substring(0, backtick);
                stack.Push(name);
                current = current.DeclaringType;
            }

            while (stack.Count > 0)
            {
                if (stack.Count > 1 || type.Namespace != null) // not the first for top-level
                    sb.Append('.');
                sb.Append(stack.Pop());
            }

            return sb.ToString();
        }
    }
}
