using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ConvertResX
{
    class Program
    {
        const string NAMESPACE = "OSGeo.MapGuide";

        static void Main(string[] args)
        {
            if (args == null || args.Length != 1)
            {
                Console.WriteLine("Input folder not specified, please specify input folder as the only single argument");
                return;
            }

            if (!System.IO.Directory.Exists(args[0]))
            {
                Console.WriteLine(string.Format("The folder {0} does not exist", args[0]));
                return;
            }

            string languageFilename =
                (from f in System.IO.Directory.GetFiles(args[0], "*.resx")
                 select f).FirstOrDefault();

            if (languageFilename == null)
            {
                Console.WriteLine(string.Format("The folder {0} is empty", args[0]));
                return;
            }

            string language = System.IO.Path.GetFileNameWithoutExtension(languageFilename);
            language = System.IO.Path.GetExtension(language);
            if (string.IsNullOrEmpty(language))
            {
                Console.WriteLine(string.Format("The filename {0} does not have a language extension", languageFilename));
                return;
            }

            if (language.StartsWith("."))
                language = language.Substring(1);

            string sourceFolder = args[0];

            Console.WriteLine("Patching language " + language); 

            string targetFolder = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), language);
            string locTool = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "LocalizationTool.exe");
            if (System.IO.Directory.Exists(targetFolder))
                System.Diagnostics.Process.Start(locTool, "update " + language).WaitForExit();
            else
                System.Diagnostics.Process.Start(locTool, "create " + language).WaitForExit();


            var sourcedocs = (from filename in System.IO.Directory.GetFiles(sourceFolder, "*." + language + ".resx")
                              let document = XDocument.Load(filename)
                              select new { filename, document }).ToList();

            var dupefinder = (from x in sourcedocs
                                       from node in x.document.Element("root").Elements("data")
                                       let name = node.Attribute("name").Value
                                       orderby name
                                       select new { name, node }).ToList();

            var dupes = (from x in dupefinder
                        where (
                            from y in dupefinder
                            where y.name == x.name
                            select y)
                        .Count() > 1
                        orderby x.name
                        select x).ToList();

                foreach (var a in dupes)
                    if ((from x in dupes
                        where x.name == a.name
                        select x).Count() > 1)
                    {
                        Console.WriteLine("Duplicate removed: " + a.name);
                        a.node.Remove();
                    }


            Dictionary<string, XElement> sourcestrings = (from x in sourcedocs
                                                       from y in x.document.Element("root").Elements("data")
                                                       select y).ToDictionary(c => c.Attribute("name").Value);

            var translates = (from x in XDocument.Load("translate.xml").Element("root").Elements("translate")
                              let @from = x.Attribute("from").Value
                              let to = x.Attribute("to").Value
                              select new { @from, to });

            foreach (var x in translates)
            {
                foreach(var y in (from c in sourcestrings.Keys
                                 where c.StartsWith(x.@from)
                                 select c).ToList())
                {
                    var tmp = sourcestrings[y];
                    sourcestrings.Remove(y);
                    string newname = x.to + y.Substring(x.@from.Length);
                    if (sourcestrings.ContainsKey(newname))
                        tmp.Remove();
                    else
                        sourcestrings[x.to + y.Substring(x.@from.Length)] = tmp;
                }
            }

            string targetfolder = System.IO.Path.Combine(@"C:\Users\Kenneth\Documents\Maestro\Localization\", language);

            foreach (string s in GetFormFiles(targetfolder, language))
                MoveStrings(s, sourcestrings, language, targetFolder);

            Dictionary<string, XElement> toremove = new Dictionary<string, XElement>();
            foreach (string s in GetStringFiles(targetfolder, language))
                TranslateStrings(s, sourcestrings, toremove);

            foreach (string key in toremove.Keys)
            {
                sourcestrings[key].Remove();
                sourcestrings.Remove(key);
            }

            foreach (var doc in sourcedocs)
                doc.document.Save(doc.filename);

            var docRemap = (from node in sourcestrings.Values
                            let doc = (from y in sourcedocs
                                       where y.document == node.Document
                                       select y).First()
                            select new { node, doc }).ToDictionary(c => c.node);

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("missing." + language + ".txt"))
            {
                foreach (string s in sourcestrings.Keys)
                    sw.WriteLine(
                        string.Format(
                        "Document: {0}\r\nOriginal key: {1}\r\nTranslated key: {2}\r\nValue: {3}\r\n",
                        docRemap[sourcestrings[s]].doc.filename,
                        sourcestrings[s].Attribute("name").Value,
                        s,
                        sourcestrings[s].Element("value").Value));


                var doclist = (from s in sourcestrings.Values
                                     select s.Document).ToList();

                var unused = from f in sourcedocs
                             where !doclist.Contains(f.document)
                             select f.filename;

                var used = from f in sourcedocs
                             where !doclist.Contains(f.document)
                             select f.filename;

                /*sw.WriteLine();

                foreach (var f in unused)
                    sw.WriteLine("File {0} may be erased", f);

                sw.WriteLine();

                foreach (var f in used)
                    sw.WriteLine("File {0} still contains strings", f); */
            }
        }   

        static void TranslateStrings(string s, Dictionary<string, XElement> sources, Dictionary<string, XElement> toremove)
        {
            XDocument outdoc = XDocument.Load(s);

            var targetNames = (from node in outdoc.Element("root").Elements("data")
                               let name = node.Attribute("name").Value
                               let value = node.Element("value").Value.Replace("\r\n", "\\n")
                               where !string.IsNullOrEmpty(name)
                               select new { name, value, node });

            foreach (var item in targetNames)
            {
                string key = item.value.Trim();

                if (!sources.ContainsKey(key))
                    foreach (string x in sources.Keys)
                        if (string.Equals(x, key, StringComparison.InvariantCultureIgnoreCase))
                        {
                            key = x;
                            break;
                        }

                if (sources.ContainsKey(key))
                {
                    Console.WriteLine(string.Format("Match on {0}, old string: {1}, new string: {2}", key, item.value, sources[key].Element("value").Value));

                    item.node.Element("value").Value = sources[key].Element("value").Value.Replace("\\n", "\r\n");
                    toremove[key] = sources[key];
                }
            }

            outdoc.Save(s);
        }

        static void MoveStrings(string s, Dictionary<string, XElement> sources, string language, string targetfolder)
        {
            string classname = s.Substring(targetfolder.Length + 1);
            classname = classname.Substring(0, classname.Length - ".resx".Length);
            classname = classname.Replace(System.IO.Path.DirectorySeparatorChar.ToString(), ".");

            string otherfile = System.IO.Path.ChangeExtension(s, "." + language + ".resx");
            var targetNames = (from node in XDocument.Load(s).Element("root").Elements("data")
                               let name = node.Attribute("name").Value
                               let value = node.Element("value").Value
                               where !name.StartsWith(">>")
                               select new { name, value, node });
            

            var dupes = (from x in targetNames
                        where (from y in targetNames
                               where x.name == y.name
                               select x.name).Count() > 1
                        select x.name).ToList();

            foreach (string d in dupes)
                Console.WriteLine("Dupe: " + d);

            XDocument outdoc = XDocument.Load(otherfile);

            var currentNames = (from node in outdoc.Element("root").Elements("data")
                               let name = node.Attribute("name").Value
                               let value = node.Element("value").Value
                               where !name.StartsWith(">>")
                               select new { name, value, node });

            var currentLookup = currentNames.ToDictionary(c => c.name);

            var targetDict  = targetNames.ToDictionary(c => c.name);
            foreach (string k in targetDict.Keys)
            {
                string oldKey;
                if (k.StartsWith("$this."))
                    oldKey = NAMESPACE + "." + classname + "." + k.Substring("$this.".Length);
                else
                    oldKey = NAMESPACE + "." + classname + "." + k;

                /*if (!sources.ContainsKey(oldKey))
                {
                    var minlen  =(NAMESPACE + "." + classname).Length + k.Length;
                    var possible = (from f in sources.Keys
                                   where 
                                        f.StartsWith(NAMESPACE + "." + classname) 
                                        && 
                                        f.EndsWith(k)
                                        &&
                                        f.Length > minlen
                                   select f).ToList();
                    if (possible.Count() == 1)
                        oldKey = possible[0];
                }*/

                if (sources.ContainsKey(oldKey))
                {
                    Console.WriteLine(string.Format("Match on {0}, old string: {1}, new string: {2}", oldKey, targetDict[k].value, sources[oldKey].Element("value").Value));
                    if (currentLookup.ContainsKey(k))
                        Console.WriteLine("Already updated");
                    else
                    {
                        XElement ne = new XElement(sources[oldKey]);
                        ne.Attribute("name").Value = k;
                        outdoc.Element("root").Add(ne);
                        currentLookup.Add(k, null);
                    }

                    sources[oldKey].Remove();
                    sources.Remove(oldKey);
                }
                else if (oldKey.EndsWith(".Location"))
                {
                    oldKey = oldKey.Substring(0, oldKey.Length - ".Location".Length);
                    if (sources.ContainsKey(oldKey + ".Left") || sources.ContainsKey(oldKey + ".Top") || sources.ContainsKey(oldKey + ".X") || sources.ContainsKey(oldKey + ".Y"))
                    {
                        string[] loc = targetDict[k].value.Split(',');
                        if (loc.Length == 2)
                        {
                            if (sources.ContainsKey(oldKey + ".Left"))
                            {
                                loc[0] = sources[oldKey + ".Left"].Element("value").Value.Trim();
                                sources.Remove(oldKey + ".Left");
                            }
                            else if (sources.ContainsKey(oldKey + ".X"))
                            {
                                loc[0] = sources[oldKey + ".X"].Element("value").Value.Trim();
                                sources.Remove(oldKey + ".X");
                            }
                            if (sources.ContainsKey(oldKey + ".Top"))
                            {
                                loc[1] = sources[oldKey + ".Top"].Element("value").Value.Trim();
                                sources.Remove(oldKey + ".Top");
                            }
                            else if (sources.ContainsKey(oldKey + ".Y"))
                            {
                                loc[0] = sources[oldKey + ".Y"].Element("value").Value.Trim();
                                sources.Remove(oldKey + ".Y");
                            }

                            if (!currentLookup.ContainsKey(k))
                            {
                                XElement e = new XElement(targetDict[k].node);
                                e.Element("value").Value = string.Join(", ", loc);
                                outdoc.Element("root").Add(e);
                                currentLookup.Add(k, null);
                            }
                        }
                    }
                }
                else if (oldKey.EndsWith(".Size"))
                {
                    oldKey = oldKey.Substring(0, oldKey.Length - ".Size".Length);
                    if (sources.ContainsKey(oldKey + ".Width") || sources.ContainsKey(oldKey + ".Height"))
                    {
                        string[] loc = targetDict[k].value.Split(',');
                        if (loc.Length == 2)
                        {
                            if (sources.ContainsKey(oldKey + ".Width"))
                            {
                                loc[0] = sources[oldKey + ".Width"].Element("value").Value.Trim();
                                sources.Remove(oldKey + ".Width");
                            }
                            if (sources.ContainsKey(oldKey + ".Height"))
                            {
                                loc[1] = sources[oldKey + ".Height"].Element("value").Value.Trim();
                                sources.Remove(oldKey + ".Height");
                            }

                            if (!currentLookup.ContainsKey(k))
                            {
                                XElement e = new XElement(targetDict[k].node);
                                e.Element("value").Value = string.Join(", ", loc);
                                outdoc.Element("root").Add(e);
                                currentLookup.Add(k, null);
                            }
                        }
                    }
                }
                
            }

            var hasDrawingAlias =
                (from x in outdoc.Element("root").Elements("assembly")
                where x.Attribute("alias").Value == "System.Drawing"
                select x).FirstOrDefault();

            if (hasDrawingAlias == null)
            {
                outdoc.Element("root").Elements("resheader").Last().AddAfterSelf(
                    new XElement("assembly", 
                        new XAttribute("alias", "System.Drawing"),
                        new XAttribute("name", "System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
                ));
            }

            outdoc.Save(otherfile);
        }

        static List<string> GetFormFiles(string basefolder, string language)
        {
            Queue<string> folders = new Queue<string>();
            folders.Enqueue(basefolder);

            List<string> files = new List<string>();

            while (folders.Count > 0)
            {
                string folder = folders.Dequeue();

                foreach (string s in System.IO.Directory.GetDirectories(folder))
                    folders.Enqueue(s);

                files.AddRange(
                    from f in System.IO.Directory.GetFiles(folder, "*.resx")
                    where !f.ToLower().EndsWith(("." + language + ".resx").ToLower())
                    select f);
            }

            return files;
        }

        static List<string> GetStringFiles(string basefolder, string language)
        {
            Queue<string> folders = new Queue<string>();
            folders.Enqueue(basefolder);

            List<string> files = new List<string>();

            while (folders.Count > 0)
            {
                string folder = folders.Dequeue();

                foreach (string s in System.IO.Directory.GetDirectories(folder))
                    folders.Enqueue(s);

                files.AddRange(
                    from f in System.IO.Directory.GetFiles(folder, "*." + language + ".resx")
                    where !System.IO.File.Exists(f.Substring(0, f.Length - ("." + language + ".resx").Length) + ".resx")
                    select f);
            }

            return files;
        }
    }
}
