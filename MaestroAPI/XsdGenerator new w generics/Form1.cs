#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace XsdGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "XSD Files (*.xsd)|*.xsd|All files (*.*)|*.*";
            dlg.Title = "Select XSD files";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                foreach (string s in dlg.FileNames)
                    checkedListBox1.SetItemChecked( checkedListBox1.Items.Add(s), true);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string outdir = textBox2.Text;

            if (outdir.Length <= 0)
                outdir = Application.StartupPath;

            foreach (string s in checkedListBox1.CheckedItems)
            {
                string filename = System.IO.Path.Combine(outdir, System.IO.Path.ChangeExtension(System.IO.Path.GetFileNameWithoutExtension(s).Replace(".", "_"), ".cs"));

                if (System.IO.File.Exists(filename))
                    System.IO.File.Delete(filename);

                string path = System.Environment.ExpandEnvironmentVariables("%programfiles%\\Microsoft SDKs\\Windows\\v6.0A\\bin\\xsd.exe");
                if (!System.IO.File.Exists(path))
                    path = System.Environment.ExpandEnvironmentVariables("%programfiles%\\Microsoft Visual Studio 8\\SDK\\v2.0\\Bin\\xsd.exe");

                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.Arguments = "\"" + s + "\" /c /o:\"" + outdir + "\" /l:CS /n:" + textBox1.Text + "";
                p.StartInfo.FileName = path;
                p.StartInfo.WorkingDirectory = outdir;
                p.Start();
                p.WaitForExit();

                if (System.IO.File.Exists(filename))
                {
                    string cont = "";

                    using (System.IO.StreamReader sr = new System.IO.StreamReader(filename))
                        cont = sr.ReadToEnd();

                    System.IO.File.Delete(filename);

                    System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("(private|public) (.+\\[\\])");

                    System.Text.RegularExpressions.Match m = r.Match(cont);
                    List<string> arrayFields = new List<string>();
                    while (m.Success)
                    {                        
                        arrayFields.Add(m.Groups[2].Value);
                        m = m.NextMatch();
                    }

                    foreach (string v in arrayFields)
                        if (v != null && v.Length > 2)
                            cont = cont.Replace(v, "System.Collections.Generic.List<" + v.Substring(0, v.Length - 2) + ">");

                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filename))
                        sw.Write(cont);

                }
            }
        }
    }
}