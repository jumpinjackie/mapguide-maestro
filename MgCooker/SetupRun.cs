#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OSGeo.MapGuide.MgCooker
{
    public partial class SetupRun : Form
    {
        private MaestroAPI.ServerConnectionI m_connection;
        private Dictionary<string, string> m_commandlineargs;

        private SetupRun()
        {
            InitializeComponent();
        }


        public SetupRun(MaestroAPI.ServerConnectionI connection, string[] maps, Dictionary<string, string> args)
            : this()
        {
            m_connection = connection;
            m_commandlineargs = args;

            if (m_commandlineargs.ContainsKey("mapdefinitions"))
                m_commandlineargs.Remove("mapdefinitions");
            if (m_commandlineargs.ContainsKey("scaleindex"))
                m_commandlineargs.Remove("scaleindex");
            if (m_commandlineargs.ContainsKey("basegroups"))
                m_commandlineargs.Remove("basegroups");

            if (maps == null || maps.Length == 0 || (maps.Length == 1 && maps[0].Trim().Length == 0))
            {
                List<string> tmp = new List<string>();
                foreach (MaestroAPI.ResourceListResourceDocument doc in m_connection.GetRepositoryResources("Library://", "MapDefinition").Items)
                    tmp.Add(doc.ResourceId);
                maps = tmp.ToArray();
            }

            treeView1.Nodes.Clear();
            foreach (string m in maps)
            {
                MaestroAPI.MapDefinition mdef = m_connection.GetMapDefinition(m);
                if (mdef.BaseMapDefinition != null && mdef.BaseMapDefinition.FiniteDisplayScale != null && mdef.BaseMapDefinition.BaseMapLayerGroup != null)
                {
                    TreeNode mn = treeView1.Nodes.Add(m);
                    mn.Checked = true;
                    mn.ImageIndex  = mn.SelectedImageIndex = 0;
                    foreach (MaestroAPI.BaseMapLayerGroupCommonType g in mdef.BaseMapDefinition.BaseMapLayerGroup)
                    {
                        TreeNode gn = mn.Nodes.Add(g.Name);
                        gn.Tag = g;
                        gn.Checked = true;
                        gn.ImageIndex = gn.SelectedImageIndex = 1;

                        foreach (double d in mdef.BaseMapDefinition.FiniteDisplayScale)
                        {
                            TreeNode sn = gn.Nodes.Add(d.ToString(System.Globalization.CultureInfo.CurrentUICulture));
                            sn.Checked = true;
                            sn.ImageIndex = sn.SelectedImageIndex = 3;
                        }
                    }

                    mn.Expand();
                }
                
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BatchSettings bx = new BatchSettings(m_connection);
            foreach (Config c in ReadTree())
            {
                BatchMap bm = new BatchMap(bx, c.MapDefinition);
                bm.SetGroups(new string[] { c.Group });
                bm.SetScales(c.ScaleIndexes);
                bx.Maps.Add(bm);
            }

            Progress p = new Progress(bx);
            if (p.ShowDialog(this) != DialogResult.Cancel)
                this.Close();
        }

        private List<Config> ReadTree()
        {
            List<Config> lst = new List<Config>();
            foreach(TreeNode mn in treeView1.Nodes)
                if (mn.Checked)
                {
                    foreach(TreeNode gn in mn.Nodes)
                        if (gn.Checked)
                        {
                            List<int> ix = new List<int>();
                            foreach (TreeNode sn in gn.Nodes)
                                if (sn.Checked)
                                    ix.Add(sn.Index);

                            if (ix.Count > 0)
                                lst.Add(new Config(mn.Text, gn.Text, ix.ToArray()));
                        }
                }

            return lst;
        }

        private class Config
        {
            public string MapDefinition;
            public string Group;
            public int[] ScaleIndexes;

            public Config(string MapDefinition, string Group, int[] ScaleIndexes)
            {
                this.MapDefinition = MapDefinition;
                this.Group = Group;
                this.ScaleIndexes = ScaleIndexes;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (System.Environment.OSVersion.Platform == PlatformID.Unix)
                saveFileDialog1.Filter = "Shell Script (*.sh)|*.sh|All files (*.*)|*.*";

            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                string executable = System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                executable = System.IO.Path.Combine(Application.StartupPath, executable);

                executable = "\"" + executable + "\"";

                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.FileName))
                {
                    if (System.Environment.OSVersion.Platform == PlatformID.Unix)
                    {
                        sw.WriteLine("#!/bin/sh");
                        executable = "mono " + executable;
                    }
                    else
                    {
                        sw.WriteLine("@echo off");
                    }


                    foreach (Config c in ReadTree())
                    {
                        sw.Write(executable);
                        sw.Write(" batch");
                        sw.Write(" --mapdefinitions=\"");
                        sw.Write(c.MapDefinition);
                        sw.Write("\" --basegroups=\"");
                        sw.Write(c.Group);
                        sw.Write("\" --scaleindex=");
                        for (int i = 0; i < c.ScaleIndexes.Length; i++)
                        {
                            if (i != 0)
                                sw.Write(",");
                            sw.Write(i.ToString());
                        }

                        foreach (string s in m_commandlineargs.Keys)
                        {
                            sw.Write(" --");
                            sw.Write(s);
                            sw.Write("=\"");
                            sw.Write(m_commandlineargs[s]);
                            sw.Write("\"");
                        }

                        sw.WriteLine();
                    }
                }
            }
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            bool byuser = e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse;

            if (e.Node == null)
                return;

            if (byuser)
            {
                foreach (TreeNode n in e.Node.Nodes)
                    n.Checked = e.Node.Checked;

                if (e.Node.Parent != null)
                {
                    int uc = 0;
                    int c = 0;

                    foreach (TreeNode n in e.Node.Parent.Nodes)
                        if (n.Checked)
                            c++;
                        else
                            uc++;

                    e.Node.Parent.Checked = c > 0;
                }
            }
        }
    }
}