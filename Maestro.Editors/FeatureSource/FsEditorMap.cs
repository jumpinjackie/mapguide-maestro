using System;
using System.Collections.Generic;
using System.Text;
using Maestro.Shared.UI;
using Maestro.Editors.FeatureSource.Providers;
using System.Xml;

namespace Maestro.Editors.FeatureSource
{
    internal static class FsEditorMap
    {
        static Dictionary<string, Type> _editors;

        static FsEditorMap()
        {
            _editors = new Dictionary<string, Type>();
            if (System.IO.File.Exists("FsEditorMap.xml")) //NOXLATE
            {
                var doc = new XmlDocument();
                doc.Load("FsEditorMap.xml"); //NOXLATE
                var list = doc.SelectNodes("//FeatureSourceEditorMap/Editor"); //NOXLATE
                foreach (XmlNode node in list)
                {
                    try
                    {
                        string provider = node.Attributes["provider"].Value.ToUpper(); //NOXLATE
                        string typeName = node.Attributes["type"].Value; //NOXLATE

                        _editors[provider] = Type.GetType(typeName);
                    }
                    catch { }
                }
            }
        }

        static string Normalize(string provider)
        {
            string[] tokens = provider.Split('.'); //NOXLATE
            if (tokens.Length == 2)
                return provider;
            else
                return tokens[0] + "." + tokens[1]; //NOXLATE
        }

        public static CollapsiblePanel GetPanel(string provider)
        {
            CollapsiblePanel panel = null;
            provider = Normalize(provider);

            //Try to obtain a specialized editor if possible, otherwise
            //fall back to the generic one
            string name = provider.ToUpper();
            try
            {
                if (_editors.ContainsKey(name))
                {
                    panel = (CollapsiblePanel)Activator.CreateInstance(_editors[name]);
                }
                else
                {
                    panel = new GenericCtrl();
                }
            }
            finally
            {
                if (panel == null)
                    panel = new GenericCtrl();
            }

            return panel;
        }
    }
}
