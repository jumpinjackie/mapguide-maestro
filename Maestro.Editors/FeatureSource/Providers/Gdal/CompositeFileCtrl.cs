#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.MaestroAPI.SchemaOverrides;
using System.Xml;
using OSGeo.MapGuide.MaestroAPI.Schema;
using System.IO;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI;
using System.Collections.Specialized;
using Maestro.Editors.Common;
using System.Globalization;

namespace Maestro.Editors.FeatureSource.Providers.Gdal
{
    [ToolboxItem(false)]
    internal partial class CompositeFileCtrl : EditorBase
    {
        internal CompositeFileCtrl()
        {
            InitializeComponent();
        }

        private IFeatureSource _fs;
        private GdalConfigurationDocument _conf;
        private IEditorService _service;

        public override void Bind(IEditorService service)
        {
            _service = service;
            _service.RegisterCustomNotifier(this);
            _fs = (IFeatureSource)_service.GetEditedResource();
            InitDefaults();
        }

        internal void InitDefaults()
        {
            string xml = _fs.GetConfigurationContent();
            if (!string.IsNullOrEmpty(xml))
            {
                try
                {
                    _conf = (GdalConfigurationDocument)ConfigurationDocument.LoadXml(xml);
                }
                catch
                {
                    BuildDefaultDocument();
                }

                lstView.Items.Clear();
                List<string> files = new List<string>();
                foreach (var loc in _conf.RasterLocations)
                {
                    AddRasterItems(loc.Location, loc.Items);
                }
            }
        }

        private void AddRasterItems(string dir, GdalRasterItem[] items)
        {
            foreach (var item in items)
            {
                AddRasterItem(dir, item);
            }
        }

        private void AddRasterItem(string dir, GdalRasterItem item)
        {
            foreach (ListViewItem li in lstView.Items)
            {
                //A list view item of the same file name exist. Abort.
                if (((GdalRasterItem)li.Tag).FileName == item.FileName)
                    return;
            }

            ListViewItem lvi = new ListViewItem();
            lvi.Name = Path.Combine(dir, item.FileName);
            lvi.Text = lvi.Name;
            lvi.Tag = item;
            lvi.ImageIndex = 0;

            lstView.Items.Add(lvi);
        }

        // This should really come from GetSchemaMapping, but it's broken:  minX, minY, maxX, maxY
        private const string TEMPLATE_CFG = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><fdo:DataStore xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xmlns:gml=\"http://www.opengis.net/gml\" xmlns:fdo=\"http://fdo.osgeo.org/schemas\" xmlns:fds=\"http://fdo.osgeo.org/schemas/fds\"><gml:DerivedCRS gml:id=\"Default\"><gml:metaDataProperty><gml:GenericMetaData><fdo:SCExtentType>dynamic</fdo:SCExtentType><fdo:XYTolerance>0.001000</fdo:XYTolerance><fdo:ZTolerance>0.001000</fdo:ZTolerance></gml:GenericMetaData></gml:metaDataProperty><gml:remarks>System generated default FDO Spatial Context</gml:remarks><gml:srsName>Default</gml:srsName><gml:validArea><gml:boundingBox><gml:pos>{0} {1}</gml:pos><gml:pos>{2} {3}</gml:pos></gml:boundingBox></gml:validArea><gml:baseCRS>" +  //NOXLATE
            "<fdo:WKTCRS gml:id=\"Default\"><gml:srsName>Default</gml:srsName><fdo:WKT>LOCAL_CS[\"*XY-MT*\",LOCAL_DATUM[\"*X-Y*\",10000],UNIT[\"Meter\", 1],AXIS[\"X\",EAST],AXIS[\"Y\",NORTH]]</fdo:WKT></fdo:WKTCRS></gml:baseCRS><gml:definedByConversion xlink:href=\"http://fdo.osgeo.org/coord_conversions#identity\"/><gml:derivedCRSType codeSpace=\"http://fdo.osgeo.org/crs_types\">geographic</gml:derivedCRSType><gml:usesCS xlink:href=\"http://fdo.osgeo.org/cs#default_cartesian\"/></gml:DerivedCRS><xs:schema xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:fdo=\"http://fdo.osgeo.org/schemas\" xmlns:gml=\"http://www.opengis.net/gml\" xmlns:default=\"http://fdo.osgeo.org/schemas/feature/default\" targetNamespace=\"http://fdo.osgeo.org/schemas/feature/default\" elementFormDefault=\"qualified\" attributeFormDefault=\"unqualified\"><xs:annotation><xs:appinfo source=\"http://fdo.osgeo.org/schemas\"/></xs:annotation><xs:element name=\"default\" type=\"default:defaultType\" abstract=\"false\" substitutionGroup=\"gml:_Feature\"><xs:key name=\"defaultKey\"><xs:selector xpath=\".//default\"/>" +  //NOXLATE
            "<xs:field xpath=\"FeatId\"/></xs:key></xs:element><xs:complexType name=\"defaultType\" abstract=\"false\" fdo:hasGeometry=\"false\"><xs:annotation><xs:appinfo source=\"http://fdo.osgeo.org/schemas\"/></xs:annotation><xs:complexContent><xs:extension base=\"gml:AbstractFeatureType\"><xs:sequence><xs:element name=\"FeatId\"><xs:annotation><xs:appinfo source=\"http://fdo.osgeo.org/schemas\"/></xs:annotation><xs:simpleType><xs:restriction base=\"xs:string\"><xs:maxLength value=\"256\"/></xs:restriction></xs:simpleType></xs:element><xs:element name=\"Raster\" type=\"fdo:RasterPropertyType\" fdo:defaultImageXSize=\"1024\" fdo:defaultImageYSize=\"1024\" fdo:srsName=\"Default\"><xs:annotation>" +  //NOXLATE
            "<xs:appinfo source=\"http://fdo.osgeo.org/schemas\"><fdo:DefaultDataModel dataModelType=\"Bitonal\" dataType=\"Unknown\" organization=\"Pixel\" bitsPerPixel=\"1\" tileSizeX=\"256\" tileSizeY=\"256\"/></xs:appinfo></xs:annotation></xs:element></xs:sequence></xs:extension></xs:complexContent></xs:complexType></xs:schema><SchemaMapping xmlns=\"http://fdogrfp.osgeo.org/schemas\" provider=\"OSGeo.Gdal.3.2\" name=\"default\"></SchemaMapping></fdo:DataStore>";  //NOXLATE


        private void BuildDefaultDocument()
        {
            _conf = (GdalConfigurationDocument)ConfigurationDocument.LoadXml(string.Format(TEMPLATE_CFG, -10000000, -10000000, 10000000, 10000000));
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<string> files = new List<string>();
            List<ListViewItem> items = new List<ListViewItem>();
            foreach (ListViewItem item in lstView.SelectedItems)
            {
                items.Add(item);
                files.Add(item.Text);
            }
            DoUpdateConfiguration(new string[0], files.ToArray());
            foreach (var it in items)
            {
                lstView.Items.Remove(it);
            }
        }

        private void lstView_SelectedIndexChanged(object sender, EventArgs e)
        {
            //btnRefresh.Enabled = 
            btnDelete.Enabled = (lstView.SelectedItems.Count > 0);
        }

        private void btnRebuild_Click(object sender, EventArgs e)
        {
            BuildDefaultDocument();
            List<string> files = new List<string>();
            foreach (ListViewItem item in lstView.Items)
            {
                files.Add(item.Text);
            }
            lstView.Clear(); //Clear now. It will be repopulated after rebuild
            DoUpdateConfiguration(files.ToArray(), new string[0]);
        }

        private void browseFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DoUpdateConfiguration(openFileDialog.FileNames, new string[0]);
            }
        }

        private void DoUpdateConfiguration(string[] toAdd, string[] toRemove)
        {
            DoUpdateConfiguration(toAdd, toRemove, false);
        }

        private void DoUpdateConfiguration(string[] toAdd, string[] toRemove, bool isAlias)
        {
            if (_conf == null)
                BuildDefaultDocument();

            var pdlg = new ProgressDialog();
            pdlg.CancelAbortsThread = true;
            var worker = new ProgressDialog.DoBackgroundWork(UpdateConfigurationDocument);
            var result = (UpdateConfigResult)pdlg.RunOperationAsync(null, worker, _conf, _fs.CurrentConnection, toAdd, toRemove, isAlias);
            if (result.Added.Count > 0 || result.Removed.Count > 0)
            {
                _fs.SetConfigurationContent(_conf.ToXml());
                List<ListViewItem> remove = new List<ListViewItem>();
                foreach (ListViewItem lvi in lstView.Items)
                {
                    if (result.Removed.Contains(lvi.Text))
                        remove.Add(lvi);
                }
                foreach (var added in result.Added)
                {
                    string dir = null;
                    string fileName = null;
                    if (isAlias)
                    {
                        dir = added.Substring(0, added.LastIndexOf("\\")); //NOXLATE
                        fileName = added.Substring(added.LastIndexOf("\\") + 1); //NOXLATE
                    }
                    else
                    {
                        dir = Path.GetDirectoryName(added);
                        fileName = Path.GetFileName(added);
                    }

                    foreach (var loc in _conf.RasterLocations)
                    {
                        if (loc.Location == dir)
                        {
                            foreach (var item in loc.Items)
                            {
                                if (item.FileName == fileName)
                                {
                                    AddRasterItem(dir, item);
                                }
                            }
                        }
                    }
                }
                OnResourceChanged();
            }
        }

        class UpdateConfigResult
        {
            public List<string> Added { get; set; }

            public List<string> Removed { get; set; }
        }

        object UpdateConfigurationDocument(BackgroundWorker worker, DoWorkEventArgs e, params object[] args)
        {
            GdalConfigurationDocument conf = (GdalConfigurationDocument)args[0];
         
            IServerConnection conn = (IServerConnection)args[1];
            string [] toAdd = args[2] as string[];
            string [] toRemove = args[3] as string[];
            bool isAlias = (bool)args[4];

            worker.ReportProgress(0, Strings.UpdatingConfiguration);

            int total = toAdd.Length + toRemove.Length;
            int unit = (total / 100);
            int progress = 0;

            var result = new UpdateConfigResult() { Added = new List<string>(), Removed = new List<string>() };

            //Remove first
            foreach (var remove in toRemove)
            {
                string dir = null;
                if (isAlias)
                {
                    dir = remove.Substring(0, remove.LastIndexOf("\\")); //NOXLATE
                }
                else
                {
                    dir = Path.GetDirectoryName(remove);
                }

                var loc = FindLocation(conf, dir);
                if (null != loc)
                {
                    string f = isAlias ? remove.Substring(remove.LastIndexOf("\\") + 1) : Path.GetFileName(remove); //NOXLATE
                    loc.RemoveItem(f);
                    result.Removed.Add(remove);
                    if (loc.Items.Length == 0)
                        conf.RemoveLocation(loc);
                }
                progress += unit;
                worker.ReportProgress(progress, string.Format(Strings.ProcessedItem, remove));
            }

            //Then add
            foreach (var add in toAdd)
            {
                string dir = null;
                if (isAlias)
                {
                    int idx = add.LastIndexOf("/"); //NOXLATE
                    if (idx >= 0)
                        dir = add.Substring(0, idx);
                    else
                        dir = add.Substring(0, add.LastIndexOf("%") + 1); //NOXLATE
                }
                else
                {
                    dir = Path.GetDirectoryName(add);
                }
                var loc = conf.AddLocation(dir);

                //Create a temp feature source to attempt interrogation of extents
                var values = new NameValueCollection();
                values["DefaultRasterFileLocation"] = add; //NOXLATE
                var fs = ObjectFactory.CreateFeatureSource(conn, "OSGeo.Gdal", values); //NOXLATE

                var resId = new ResourceIdentifier("Session:" + conn.SessionID + "//" + Guid.NewGuid() + ".FeatureSource"); //NOXLATE
                fs.ResourceID = resId.ToString();
                conn.ResourceService.SaveResource(fs);

                var scList = fs.GetSpatialInfo(false);
                
                var raster = new GdalRasterItem();

                if (isAlias)
                {
                    int idx = add.LastIndexOf("/"); //NOXLATE
                    if (idx >= 0)
                        raster.FileName = add.Substring(add.LastIndexOf("/") + 1); //NOXLATE
                    else
                        raster.FileName = add.Substring(add.LastIndexOf("%") + 1); //NOXLATE
                }
                else
                {
                    raster.FileName = Path.GetFileName(add);
                }

                if (scList.SpatialContext.Count > 0)
                {
                    raster.MinX = Convert.ToDouble(scList.SpatialContext[0].Extent.LowerLeftCoordinate.X, CultureInfo.InvariantCulture);
                    raster.MinY = Convert.ToDouble(scList.SpatialContext[0].Extent.LowerLeftCoordinate.Y, CultureInfo.InvariantCulture);
                    raster.MaxX = Convert.ToDouble(scList.SpatialContext[0].Extent.UpperRightCoordinate.X, CultureInfo.InvariantCulture);
                    raster.MaxY = Convert.ToDouble(scList.SpatialContext[0].Extent.UpperRightCoordinate.Y, CultureInfo.InvariantCulture);
                }
                else
                {
                    raster.MinX = -10000000;
                    raster.MinY = -10000000;
                    raster.MaxX = 10000000;
                    raster.MaxY = 10000000;
                }

                loc.AddItem(raster);

                result.Added.Add(Path.Combine(dir, raster.FileName));

                progress += unit;
                worker.ReportProgress(progress, string.Format(Strings.ProcessedItem, add));
            }

            //Re-calculate combined extent for spatial context
            var env = conf.CalculateExtent();
            if (env != null)
                conf.SpatialContexts[0].Extent = env;

            return result;
        }

        private static GdalRasterLocationItem FindLocation(GdalConfigurationDocument conf, string directory)
        {
            foreach (var loc in conf.RasterLocations)
            {
                if (loc.Location == directory)
                    return loc;
            }
            return null;
        }

        private void browseFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                List<string> files = new List<string>();
                files.AddRange(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.png")); //NOXLATE
                files.AddRange(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.jpg")); //NOXLATE
                files.AddRange(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.jpeg")); //NOXLATE
                files.AddRange(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.tif")); //NOXLATE
                files.AddRange(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.tiff")); //NOXLATE
                files.AddRange(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.ecw")); //NOXLATE
                files.AddRange(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.sid")); //NOXLATE
                files.AddRange(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.dem")); //NOXLATE
                files.AddRange(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.gif")); //NOXLATE
                files.AddRange(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.bmp")); //NOXLATE

                DoUpdateConfiguration(files.ToArray(), new string[0]);
            }
        }

        private void browseAliasedFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var picker = new UnmanagedFileBrowser(_service.ResourceService))
            {
                picker.AllowMultipleSelection = true;
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    DoUpdateConfiguration(picker.SelectedItems, new string[0], true);
                }
            }
        }

        private void browseAliasedFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var picker = new UnmanagedFileBrowser(_service.ResourceService))
            {
                picker.AllowMultipleSelection = false;
                picker.SelectFoldersOnly = true;
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    List<string> files = new List<string>();
                    var folder = picker.SelectedItem;
                    if (!string.IsNullOrEmpty(folder))
                    {
                        folder = folder.Replace("%MG_DATA_PATH_ALIAS[", "[") //NOXLATE
                                       .Replace("]%", "]"); //NOXLATE
                    }
                    var list = _service.ResourceService.EnumerateUnmanagedData(folder, string.Empty, false, UnmanagedDataTypes.Files);
                    var extensions = new List<string>(new string[] { 
                        ".png", //NOXLATE
                        ".jpg", //NOXLATE
                        ".jpeg", //NOXLATE
                        ".tif", //NOXLATE
                        ".tiff", //NOXLATE
                        ".ecw", //NOXLATE
                        ".sid", //NOXLATE
                        ".dem", //NOXLATE
                        ".gif", //NOXLATE
                        ".bmp" //NOXLATE
                    });
                    foreach (var f in list.Items)
                    {
                        var file = f as OSGeo.MapGuide.ObjectModels.Common.UnmanagedDataListUnmanagedDataFile;
                        if (file != null)
                        {
                            foreach (var ext in extensions)
                            {
                                if (file.FileName.ToLower().EndsWith(ext))
                                {
                                    var leftpart = file.UnmanagedDataId.Substring(0, file.UnmanagedDataId.IndexOf("]")); //NOXLATE
                                    var rightpart = file.UnmanagedDataId.Substring(file.UnmanagedDataId.IndexOf("]") + 1); //NOXLATE
                                    var item = "%MG_DATA_PATH_ALIAS" + leftpart + "]%" + rightpart; //NOXLATE
                                    files.Add(item);
                                    break;
                                }
                            }
                        }
                    }
                    DoUpdateConfiguration(files.ToArray(), new string[0], true);
                }
            }
        }
    }
}
