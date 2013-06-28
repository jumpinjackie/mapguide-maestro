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
using OSGeo.MapGuide.MaestroAPI.Tile;
using OSGeo.MapGuide.ObjectModels;

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
            saveFileDialog1.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickBat, "bat") + "|" + //NOXLATE
                                     OSGeo.MapGuide.MaestroAPI.StringConstants.AllFilesFilter; //NOXLATE
            MapAgent.Text = "http://localhost/mapguide/mapagent/mapagent.fcgi"; //NOXLATE
            Username.Text = "Anonymous"; //NOXLATE
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

            grpDifferentConnection.Enabled = chkUseDifferentConnection.Enabled = !m_connection.ProviderName.ToUpper().Equals("MAESTRO.LOCAL"); //NOXLATE
            m_commandlineargs = args;
            m_coordinateOverrides = new Dictionary<string, IEnvelope>();
            IEnvelope overrideExtents = null;

            //HttpServerConnection hc = connection as HttpServerConnection;
            try
            {
                var url = connection.GetCustomProperty("BaseUrl"); //NOXLATE
                if (url != null)
                    MapAgent.Text = url.ToString();
            }
            catch { }

            if (m_commandlineargs.ContainsKey("mapdefinitions")) //NOXLATE
                m_commandlineargs.Remove("mapdefinitions"); //NOXLATE
            if (m_commandlineargs.ContainsKey("mapagent")) //NOXLATE
                MapAgent.Text = m_commandlineargs["mapagent"]; //NOXLATE
            if (m_commandlineargs.ContainsKey("username")) //NOXLATE
                Username.Text = m_commandlineargs["username"]; //NOXLATE
            if (m_commandlineargs.ContainsKey("password")) //NOXLATE
                Password.Text = m_commandlineargs["password"]; //NOXLATE

            if (m_commandlineargs.ContainsKey("native-connection")) //NOXLATE
                UseNativeAPI.Checked = true;

            if (m_commandlineargs.ContainsKey("limitrows")) //NOXLATE
            {
                int i;
                if (int.TryParse(m_commandlineargs["limitrows"], out i) && i > 0) //NOXLATE
                {
                    MaxRowLimit.Value = i;
                    TilesetLimitPanel.Enabled = true;
                }
            }

            if (m_commandlineargs.ContainsKey("limitcols")) //NOXLATE
            {
                int i;
                if (int.TryParse(m_commandlineargs["limitcols"], out i) && i > 0) //NOXLATE
                {
                    MaxColLimit.Value = i;
                    TilesetLimitPanel.Enabled = true;
                }
            }

            if (m_commandlineargs.ContainsKey("extentoverride")) //NOXLATE
            {
                 string[] parts = m_commandlineargs["extentoverride"].Split(',');
                if (parts.Length == 4)
                {
                    double minx;
                    double miny;
                    double maxx;
                    double maxy;
                    if (
                        double.TryParse(parts[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out minx) &&
                        double.TryParse(parts[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out miny) &&
                        double.TryParse(parts[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out maxx) &&
                        double.TryParse(parts[3], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out maxy)
                        )
                    {
                        overrideExtents = ObjectFactory.CreateEnvelope(minx, miny, maxx, maxy);
                    }
                }

            }

            if (m_commandlineargs.ContainsKey("metersperunit")) //NOXLATE
            {
                double d;
                if (
                    double.TryParse(m_commandlineargs["metersperunit"], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentUICulture, out d) //NOXLATE
                    || double.TryParse(m_commandlineargs["metersperunit"], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out d) //NOXLATE
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
                foreach (ResourceListResourceDocument doc in m_connection.ResourceService.GetRepositoryResources(StringConstants.RootIdentifier, ResourceTypes.MapDefinition.ToString()).Items)
                    tmp.Add(doc.ResourceId);
                maps = tmp.ToArray();
            }

            
            var basegroupsSelected = new List<string>();
            if (m_commandlineargs.ContainsKey("basegroups"))//NOXLATE
            {
                basegroupsSelected = new List<string>(m_commandlineargs["basegroups"].Split(','));//NOXLATE
                m_commandlineargs.Remove("basegroups"); //NOXLATE
            }

            var scalesSelected = new List<int>();
            if (m_commandlineargs.ContainsKey("scaleindex")) //NOXLATE
            {
                foreach (string scaleIndex in m_commandlineargs["scaleindex"].Split(','))//NOXLATE
                {
                    scalesSelected.Add(int.Parse(scaleIndex));
                }
                m_commandlineargs.Remove("scaleindex"); //NOXLATE
            }

            MapTree.Nodes.Clear();
            foreach (string m in maps)
            {
                IMapDefinition mdef = m_connection.ResourceService.GetResource(m) as IMapDefinition;
                if (mdef == null) //Skip unknown Map Definition version (which would be returned as UntypedResource objects)
                    continue;

                IBaseMapDefinition baseMap = mdef.BaseMap;
                if (baseMap != null &&
                    baseMap.ScaleCount > 0 && 
                    baseMap.HasGroups())
                {
                    TreeNode mn = MapTree.Nodes.Add(m);
                    
                    mn.ImageIndex = mn.SelectedImageIndex = 0;
                    mn.Tag = mdef;
                    foreach (var g in baseMap.BaseMapLayerGroup)
                    {
                        TreeNode gn = mn.Nodes.Add(g.Name);
                        gn.Tag = g;
                        if (basegroupsSelected.Contains(g.Name))
                        {
                            mn.Checked = true;
                            gn.Checked = true;
                            if (overrideExtents != null && !m_coordinateOverrides.ContainsKey(m))
                            {
                                m_coordinateOverrides.Add(m, overrideExtents);
                            }
                        }
                        
                        gn.ImageIndex = gn.SelectedImageIndex = 1;

                        int counter = 0;
                        foreach (double d in baseMap.FiniteDisplayScale)
                        {
                            TreeNode sn = gn.Nodes.Add(d.ToString(System.Globalization.CultureInfo.CurrentUICulture));
                            if (gn.Checked && scalesSelected.Contains(counter))
                            {
                                sn.Checked = true;
                                
                            }
                            sn.ImageIndex = sn.SelectedImageIndex = 3;
                            counter++;
                        }
                    }

                    mn.Expand();
                }
            }
            MapTree_AfterSelect(null, null);
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
            IServerConnection con = m_connection;
            if (chkUseDifferentConnection.Checked)
            {
                if (UseNativeAPI.Checked)
                {
                    string webconfig = System.IO.Path.Combine(Application.StartupPath, "webconfig.ini"); //NOXLATE
                    if (!System.IO.File.Exists(webconfig))
                    {
                        MessageBox.Show(this, string.Format(Strings.MissingWebConfigFile, webconfig), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    try
                    {
                        var initP = new NameValueCollection();

                        initP["ConfigFile"] = webconfig; //NOXLATE
                        initP["Username"] = Username.Text; //NOXLATE
                        initP["Password"] = Password.Text; //NOXLATE

                        con = ConnectionProviderRegistry.CreateConnection("Maestro.LocalNative", initP); //NOXLATE
                    }
                    catch (Exception ex)
                    {
                        string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                        MessageBox.Show(this, string.Format(Strings.ConnectionError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    try
                    {
                        var initP = new NameValueCollection();

                        initP["Url"] = MapAgent.Text; //NOXLATE
                        initP["Username"] = Username.Text; //NOXLATE
                        initP["Password"] = Password.Text; //NOXLATE
                        initP["AllowUntestedVersion"] = "true"; //NOXLATE

                        con = ConnectionProviderRegistry.CreateConnection("Maestro.Http", initP); //NOXLATE
                    }
                    catch (Exception ex)
                    {
                        string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                        MessageBox.Show(this, string.Format(Strings.ConnectionError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            try
            {
                TilingRunCollection bx = new TilingRunCollection(con);

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
                    MapTilingConfiguration bm = new MapTilingConfiguration(bx, c.MapDefinition);
                    bm.SetGroups(new string[] { c.Group });
                    bm.SetScalesAndExtend(c.ScaleIndexes,c.ExtentOverride);
                   
                    bx.Maps.Add(bm);
                }

                Progress p = new Progress(bx);
                if (p.ShowDialog(this) != DialogResult.Cancel)
                {
                    var ts = p.TotalTime;
                    MessageBox.Show(string.Format(Strings.TileGenerationCompleted, ((ts.Days * 24) + ts.Hours), ts.Minutes, ts.Seconds));
                }
            }
            catch (Exception ex)
            {
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                MessageBox.Show(this, string.Format(Strings.InternalError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    string.Format(Strings.FileTypeShellScript + "|{0}", "*.sh") + //NOXLATE
                    string.Format(Strings.FileTypeAllFiles + "|{0}", "*.*"); //NOXLATE

            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                //Common args for all map defintions to be tiled
                List<string> args = new List<string>();
                args.Add("--mapagent=\"" + MapAgent.Text + "\""); //NOXLATE
                args.Add("--username=\"" + Username.Text + "\""); //NOXLATE
                args.Add("--password=\"" + Password.Text + "\""); //NOXLATE

                if (LimitTileset.Checked)
                {
                    if (MaxRowLimit.Value > 0)
                        args.Add("--limitrows=\"" + ((int)MaxRowLimit.Value).ToString() + "\""); //NOXLATE
                    if (MaxColLimit.Value > 0)
                        args.Add("--limitcols=\"" + ((int)MaxColLimit.Value).ToString() + "\""); //NOXLATE
                }

                if (UseNativeAPI.Checked)
                    args.Add("--native-connection"); //NOXLATE
                if (UseOfficialMethod.Checked)
                    args.Add("--metersperunit=" + ((double)MetersPerUnit.Value).ToString(System.Globalization.CultureInfo.InvariantCulture)); //NOXLATE

                args.Add("--threadcount=" + ((int)ThreadCount.Value).ToString()); //NOXLATE
                if (RandomTileOrder.Checked)
                    args.Add("--random-tile-order"); //NOXLATE


                string executable = System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string cmdExecutable = "MgCookerCmd.exe"; //NOXLATE

                //Windows has problems with console output from GUI applications...
                if (System.Environment.OSVersion.Platform != PlatformID.Unix && executable == "MgCooker.exe" && System.IO.File.Exists(System.IO.Path.Combine(Application.StartupPath, cmdExecutable))) //NOXLATE
                    executable = System.IO.Path.Combine(Application.StartupPath, cmdExecutable); //NOXLATE
                else
                    executable = System.IO.Path.Combine(Application.StartupPath, executable);

                string exeName = System.IO.Path.GetFileName(executable);
                string exePath = System.IO.Path.GetDirectoryName(executable);

                executable = "\"" + executable + "\""; //NOXLATE

                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.FileName))
                {
                    if (System.Environment.OSVersion.Platform == PlatformID.Unix)
                    {
                        sw.WriteLine("#!/bin/sh"); //NOXLATE
                        executable = "mono " + executable; //NOXLATE
                    }
                    else
                    {
                        sw.WriteLine("@echo off"); //NOXLATE
                    }

                    //If on windows, wrap the exe call in a pushd/popd so that the executable is 
                    //executed from its own directory

                    if (System.Environment.OSVersion.Platform != PlatformID.MacOSX ||
                        System.Environment.OSVersion.Platform != PlatformID.Unix)
                    {
                        sw.WriteLine("pushd \"" + exePath + "\""); //NOXLATE
                    }

                    foreach (Config c in ReadTree())
                    {
                        //Map-specific args
                        List<string> argsMap = new List<string>();
                        if (System.Environment.OSVersion.Platform != PlatformID.MacOSX ||
                            System.Environment.OSVersion.Platform != PlatformID.Unix)
                        {
                            argsMap.Add(exeName);
                        }
                        else
                        {
                            argsMap.Add(executable);
                        }

                        argsMap.Add("batch"); //NOXLATE
                        argsMap.Add("--mapdefinitions=\"" + c.MapDefinition + "\"");
                        argsMap.Add("--basegroups=\"" + c.Group + "\"");
                        StringBuilder si = new StringBuilder("--scaleindex="); //NOXLATE
                        for (int i = 0; i < c.ScaleIndexes.Length; i++)
                        {
                            if (i != 0)
                                si.Append(","); //NOXLATE
                            si.Append(c.ScaleIndexes[i].ToString());
                        }
                        argsMap.Add(si.ToString());

                        if (c.ExtentOverride != null)
                        {
                            StringBuilder ov = new StringBuilder("--extentoverride="); //NOXLATE
                            ov.Append(c.ExtentOverride.MinX.ToString(System.Globalization.CultureInfo.InvariantCulture));
                            ov.Append(","); //NOXLATE
                            ov.Append(c.ExtentOverride.MinY.ToString(System.Globalization.CultureInfo.InvariantCulture));
                            ov.Append(","); //NOXLATE
                            ov.Append(c.ExtentOverride.MaxX.ToString(System.Globalization.CultureInfo.InvariantCulture));
                            ov.Append(","); //NOXLATE
                            ov.Append(c.ExtentOverride.MaxY.ToString(System.Globalization.CultureInfo.InvariantCulture));
                            argsMap.Add(ov.ToString());
                        }

                        string[] argsFinal = new string[args.Count + argsMap.Count];
                        int a = 0;
                        //Map-specific args first (as this contains the executable name)
                        foreach (string arg in argsMap)
                        {
                            argsFinal[a] = arg;
                            a++;
                        }
                        //Then the common args
                        foreach (string arg in args)
                        {
                            argsFinal[a] = arg;
                            a++;
                        }

                        sw.Write(string.Join(" ", argsFinal));
                        sw.WriteLine();
                    }

                    if (System.Environment.OSVersion.Platform != PlatformID.MacOSX ||
                        System.Environment.OSVersion.Platform != PlatformID.Unix)
                    {
                        sw.WriteLine("popd"); //NOXLATE
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

            if (MapTree.SelectedNode == null)
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
