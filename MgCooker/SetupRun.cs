#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
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
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using System.Collections.Specialized;

namespace MgCooker
{
    public partial class SetupRun : Form
    {
        private IServerConnection m_connection;
        private Dictionary<string, string> m_commandlineargs;
        private Dictionary<string, IEnvelope> m_coordinateOverrides;
        private bool m_isUpdating = false;

        private SetupRun()
        {
            InitializeComponent();
        }

        internal SetupRun(string userName, string password, IServerConnection connection, string[] maps, Dictionary<string, string> args)
            : this(connection, maps, args)
        {
            Username.Text = userName;
            Password.Text = password;
        }

        public SetupRun(IServerConnection connection, string[] maps, Dictionary<string, string> args)
            : this()
        {
            m_connection = connection;
            m_commandlineargs = args;
            m_coordinateOverrides = new Dictionary<string, IEnvelope>();

            //HttpServerConnection hc = connection as HttpServerConnection;
            var url = connection.GetCustomProperty("BaseUrl");
            if (url != null)
                MapAgent.Text = url.ToString();

            if (m_commandlineargs.ContainsKey("mapdefinitions"))
                m_commandlineargs.Remove("mapdefinitions");
            if (m_commandlineargs.ContainsKey("scaleindex"))
                m_commandlineargs.Remove("scaleindex");
            if (m_commandlineargs.ContainsKey("basegroups"))
                m_commandlineargs.Remove("basegroups");

            if (m_commandlineargs.ContainsKey("mapagent"))
                MapAgent.Text = m_commandlineargs["mapagent"];
            if (m_commandlineargs.ContainsKey("username"))
                Username.Text = m_commandlineargs["username"];
            if (m_commandlineargs.ContainsKey("password"))
                Password.Text = m_commandlineargs["password"];

            if (m_commandlineargs.ContainsKey("native-connection"))
                UseNativeAPI.Checked = true;

            if (m_commandlineargs.ContainsKey("limitrows"))
            {
                int i;
                if (int.TryParse(m_commandlineargs["limitrows"], out i) && i > 0)
                {
                    MaxRowLimit.Value = i;
                    TilesetLimitPanel.Enabled = true;
                }
            }

            if (m_commandlineargs.ContainsKey("limitcols"))
            {
                int i;
                if (int.TryParse(m_commandlineargs["limitcols"], out i) && i > 0)
                {
                    MaxColLimit.Value = i;
                    TilesetLimitPanel.Enabled = true;
                }
            }

            if (m_commandlineargs.ContainsKey("metersperunit"))
            {
                double d;
                if (
                    double.TryParse(m_commandlineargs["metersperunit"], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentUICulture, out d)
                    || double.TryParse(m_commandlineargs["metersperunit"], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out d)
                    )
                    if (d >= (double)MetersPerUnit.Minimum && d <= (double)MetersPerUnit.Maximum)
                    {
                        UseOfficialMethod.Checked = true;
                        MetersPerUnit.Value = (decimal)d;
                    }
            }

            if (maps == null || maps.Length == 0 || (maps.Length == 1 && maps[0].Trim().Length == 0))
            {
                List<string> tmp = new List<string>();
                foreach (ResourceListResourceDocument doc in m_connection.ResourceService.GetRepositoryResources("Library://", "MapDefinition").Items)
                    tmp.Add(doc.ResourceId);
                maps = tmp.ToArray();
            }

            MapTree.Nodes.Clear();
            foreach (string m in maps)
            {
                IMapDefinition mdef = (IMapDefinition)m_connection.ResourceService.GetResource(m);
                IBaseMapDefinition baseMap = mdef.BaseMap;
                if (baseMap != null &&
                    baseMap.ScaleCount > 0 && 
                    baseMap.HasGroups())
                {
                    TreeNode mn = MapTree.Nodes.Add(m);
                    //mn.Checked = true;
                    mn.ImageIndex = mn.SelectedImageIndex = 0;
                    mn.Tag = mdef;
                    foreach (var g in baseMap.BaseMapLayerGroup)
                    {
                        TreeNode gn = mn.Nodes.Add(g.Name);
                        gn.Tag = g;
                        //gn.Checked = true;
                        gn.ImageIndex = gn.SelectedImageIndex = 1;

                        foreach (double d in baseMap.FiniteDisplayScale)
                        {
                            TreeNode sn = gn.Nodes.Add(d.ToString(System.Globalization.CultureInfo.CurrentUICulture));
                            //sn.Checked = true;
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
            IServerConnection con = null;

            if (UseNativeAPI.Checked)
            {
                string webconfig = System.IO.Path.Combine(Application.StartupPath, "webconfig.ini");
                if (!System.IO.File.Exists(webconfig))
                {
                    MessageBox.Show(this, string.Format(Properties.Resources.MissingWebConfigFile, webconfig), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    var initP = new NameValueCollection();

                    initP["ConfigFile"] = webconfig;
                    initP["Username"] = Username.Text;
                    initP["Password"] = Password.Text;

                    con = ConnectionProviderRegistry.CreateConnection("Maestro.LocalNative", initP);
                }
                catch (Exception ex)
                {
                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                    MessageBox.Show(this, string.Format(Properties.Resources.ConnectionError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                try
                {
                    var initP = new NameValueCollection();

                    initP["Url"] = MapAgent.Text;
                    initP["Username"] = Username.Text;
                    initP["Password"] = Password.Text;
                    initP["AllowUntestedVersion"] = "true";

                    con = ConnectionProviderRegistry.CreateConnection("Maestro.Http", initP);
                }
                catch (Exception ex)
                {
                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                    MessageBox.Show(this, string.Format(Properties.Resources.ConnectionError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            try
            {
                BatchSettings bx = new BatchSettings(con);

                if (LimitTileset.Checked)
                {
                    if (MaxRowLimit.Value > 0)
                        bx.LimitRows((int)MaxRowLimit.Value);
                    if (MaxColLimit.Value > 0)
                        bx.LimitCols((int)MaxColLimit.Value);
                }

                if (UseOfficialMethod.Checked)
                {
                    bx.Config.MetersPerUnit = (double)MetersPerUnit.Value;
                    bx.Config.UseOfficialMethod = true;
                }

                bx.Config.ThreadCount = (int)ThreadCount.Value;
                bx.Config.RandomizeTileSequence = RandomTileOrder.Checked;

                foreach (Config c in ReadTree())
                {
                    BatchMap bm = new BatchMap(bx, c.MapDefinition);
                    bm.SetGroups(new string[] { c.Group });
                    bm.SetScales(c.ScaleIndexes);
                    if (c.ExtentOverride != null)
                        bm.MaxExtent = c.ExtentOverride;
                    bx.Maps.Add(bm);
                }

                Progress p = new Progress(bx);
                if (p.ShowDialog(this) != DialogResult.Cancel)
                    this.Close();
            }
            catch (Exception ex)
            {
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                MessageBox.Show(this, string.Format(Properties.Resources.InternalError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<Config> ReadTree()
        {
            List<Config> lst = new List<Config>();
            foreach(TreeNode mn in MapTree.Nodes)
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
                                lst.Add(new Config(mn.Text, gn.Text, ix.ToArray(), (m_coordinateOverrides.ContainsKey(mn.Text) ? m_coordinateOverrides[mn.Text] : null)));
                        }
                }

            return lst;
        }

        private class Config
        {
            public string MapDefinition;
            public string Group;
            public int[] ScaleIndexes;
            public IEnvelope ExtentOverride = null;

            public Config(string MapDefinition, string Group, int[] ScaleIndexes, IEnvelope ExtentOverride)
            {
                this.MapDefinition = MapDefinition;
                this.Group = Group;
                this.ScaleIndexes = ScaleIndexes;
                this.ExtentOverride = ExtentOverride;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (System.Environment.OSVersion.Platform == PlatformID.Unix)
                saveFileDialog1.Filter = 
                    string.Format(Properties.Resources.FileTypeShellScript + "|{0}", "*.sh") +
                    string.Format(Properties.Resources.FileTypeAllFiles + "|{0}", "*.*");

            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                StringBuilder args = new StringBuilder();
                args.Append("--mapagent=\"" + MapAgent.Text + "\" ");
                args.Append("--username=\"" + Username.Text + "\" ");
                args.Append("--password=\"" + Password.Text + "\" ");

                if (LimitTileset.Checked)
                {
                    if (MaxRowLimit.Value > 0)
                        args.Append("--limitrows=\"" + ((int)MaxRowLimit.Value).ToString() + "\" ");
                    if (MaxColLimit.Value > 0)
                        args.Append("--limitcols=\"" + ((int)MaxColLimit.Value).ToString() + "\" ");
                }

                if (UseNativeAPI.Checked)
                    args.Append("--native-connection ");
                if (UseOfficialMethod.Checked)
                    args.Append("--metersperunit=" + ((double)MetersPerUnit.Value).ToString(System.Globalization.CultureInfo.InvariantCulture) + " ");

                args.Append("--threadcount=" + ((int)ThreadCount.Value).ToString() + " ");
                if (RandomTileOrder.Checked)
                    args.Append("--random-tile-order ");


                string executable = System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                //Windows has problems with console output from GUI applications...
                if (System.Environment.OSVersion.Platform != PlatformID.Unix && executable == "MgCooker.exe" && System.IO.File.Exists(System.IO.Path.Combine(Application.StartupPath, "MgCookerCommandline.exe")))
                    executable = System.IO.Path.Combine(Application.StartupPath, "MgCookerCommandline.exe");
                else
                    executable = System.IO.Path.Combine(Application.StartupPath, executable);

                string exeName = System.IO.Path.GetFileName(executable);
                string exePath = System.IO.Path.GetDirectoryName(executable);

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

                    //If on windows, wrap the exe call in a pushd/popd so that the executable is 
                    //executed from its own directory

                    if (System.Environment.OSVersion.Platform != PlatformID.MacOSX ||
                        System.Environment.OSVersion.Platform != PlatformID.Unix)
                    {
                        sw.WriteLine("pushd \"" + exePath + "\"");
                    }

                    foreach (Config c in ReadTree())
                    {
                        if (System.Environment.OSVersion.Platform != PlatformID.MacOSX ||
                            System.Environment.OSVersion.Platform != PlatformID.Unix)
                        {
                            sw.Write(exeName);
                        }
                        else
                        {
                            sw.Write(executable);
                        }
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
                            sw.Write(c.ScaleIndexes[i].ToString());
                        }
                        sw.Write(" "); // dont forget the space after the list of scaleindexes ticket #1316
                        if (c.ExtentOverride != null)
                        {
                            sw.Write(" --extentoverride=");
                            sw.Write(c.ExtentOverride.MinX.ToString(System.Globalization.CultureInfo.InvariantCulture));
                            sw.Write(",");
                            sw.Write(c.ExtentOverride.MinY.ToString(System.Globalization.CultureInfo.InvariantCulture));
                            sw.Write(",");
                            sw.Write(c.ExtentOverride.MaxX.ToString(System.Globalization.CultureInfo.InvariantCulture));
                            sw.Write(",");
                            sw.Write(c.ExtentOverride.MaxY.ToString(System.Globalization.CultureInfo.InvariantCulture));
                        }

                        sw.Write(args.ToString());
                        sw.WriteLine();
                    }

                    if (System.Environment.OSVersion.Platform != PlatformID.MacOSX ||
                        System.Environment.OSVersion.Platform != PlatformID.Unix)
                    {
                        sw.WriteLine("popd");
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
                {
                    foreach (TreeNode tn in n.Nodes)
                        tn.Checked = e.Node.Checked;
                    
                    n.Checked = e.Node.Checked;
                }

                if (e.Node.Parent != null)
                {
                    int c = 0;

                    foreach (TreeNode n in e.Node.Parent.Nodes)
                        if (n.Checked)
                            c++;

                    if (c > 0)
                    {
                        e.Node.Parent.Checked = true;
                        if (e.Node.Parent.Parent != null)
                            e.Node.Parent.Parent.Checked = true;
                    }
                }
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {

        }

        private void LimitTileset_CheckedChanged(object sender, EventArgs e)
        {
            TilesetLimitPanel.Enabled = LimitTileset.Checked;
        }

        private void UseOfficialMethod_CheckedChanged(object sender, EventArgs e)
        {
            OfficialMethodPanel.Enabled = UseOfficialMethod.Checked;
            MapTree_AfterSelect(null, null);
        }

        private void MapTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (m_isUpdating)
                return;

            if (MapTree.SelectedNode == null || !UseOfficialMethod.Checked)
            {
                BoundsOverride.Enabled = false;
                BoundsOverride.Tag = null;
            }
            else
            {
                BoundsOverride.Enabled = true;
                TreeNode root = MapTree.SelectedNode;
                while (root.Parent != null)
                    root = root.Parent;

                IEnvelope box;
                if (m_coordinateOverrides.ContainsKey(root.Text))
                    box = m_coordinateOverrides[root.Text];
                else
                    box = ((IMapDefinition)root.Tag).Extents;

                BoundsOverride.Tag = root;

                try
                {
                    m_isUpdating = true;
                    txtLowerX.Text = box.MinX.ToString(System.Globalization.CultureInfo.CurrentUICulture);
                    txtLowerY.Text = box.MinY.ToString(System.Globalization.CultureInfo.CurrentUICulture);
                    txtUpperX.Text = box.MaxX.ToString(System.Globalization.CultureInfo.CurrentUICulture);
                    txtUpperY.Text = box.MaxY.ToString(System.Globalization.CultureInfo.CurrentUICulture);
                }
                finally
                {
                    m_isUpdating = false;
                }

                ModfiedOverrideWarning.Visible = m_coordinateOverrides.ContainsKey(root.Text);
            }
        }

        private void ResetBounds_Click(object sender, EventArgs e)
        {
            if (BoundsOverride.Tag as TreeNode == null)
                return;

            TreeNode root = BoundsOverride.Tag as TreeNode;

            if (m_coordinateOverrides.ContainsKey(root.Text))
                m_coordinateOverrides.Remove(root.Text);

            MapTree_AfterSelect(null, null);
        }

        private void CoordinateItem_TextChanged(object sender, EventArgs e)
        {
            if (BoundsOverride.Tag as TreeNode == null || m_isUpdating)
                return;

            TreeNode root = BoundsOverride.Tag as TreeNode;

            if (!m_coordinateOverrides.ContainsKey(root.Text))
            {
                //IEnvelope newbox = new OSGeo.MapGuide.IEnvelope();
                IEnvelope origbox = ((IMapDefinition)root.Tag).Extents;
                IEnvelope newbox = origbox.Clone();

                m_coordinateOverrides.Add(root.Text, newbox);
            }

            double d;
            if (double.TryParse(txtLowerX.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentUICulture, out d))
                m_coordinateOverrides[root.Text].MinX = d;
            if (double.TryParse(txtLowerY.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentUICulture, out d))
                m_coordinateOverrides[root.Text].MinY = d;
            if (double.TryParse(txtUpperX.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentUICulture, out d))
                m_coordinateOverrides[root.Text].MaxX = d;
            if (double.TryParse(txtUpperY.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentUICulture, out d))
                m_coordinateOverrides[root.Text].MaxY = d;
        }
    }
}
