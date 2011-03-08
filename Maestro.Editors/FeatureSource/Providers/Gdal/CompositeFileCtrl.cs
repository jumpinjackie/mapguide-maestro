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

namespace Maestro.Editors.FeatureSource.Providers.Gdal
{
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
            ListViewItem lvi = new ListViewItem();
            lvi.Name = Path.Combine(dir, item.FileName);
            lvi.Text = lvi.Name;
            lvi.Tag = item;
            lvi.ImageIndex = 0;

            lstView.Items.Add(lvi);
        }

        // This should really come from GetSchemaMapping, but it's broken:  minX, minY, maxX, maxY
        private const string TEMPLATE_CFG = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><fdo:DataStore xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xmlns:gml=\"http://www.opengis.net/gml\" xmlns:fdo=\"http://fdo.osgeo.org/schemas\" xmlns:fds=\"http://fdo.osgeo.org/schemas/fds\"><gml:DerivedCRS gml:id=\"Default\"><gml:metaDataProperty><gml:GenericMetaData><fdo:SCExtentType>dynamic</fdo:SCExtentType><fdo:XYTolerance>0.001000</fdo:XYTolerance><fdo:ZTolerance>0.001000</fdo:ZTolerance></gml:GenericMetaData></gml:metaDataProperty><gml:remarks>System generated default FDO Spatial Context</gml:remarks><gml:srsName>Default</gml:srsName><gml:validArea><gml:boundingBox><gml:pos>{0} {1}</gml:pos><gml:pos>{2} {3}</gml:pos></gml:boundingBox></gml:validArea><gml:baseCRS>" +
            "<fdo:WKTCRS gml:id=\"Default\"><gml:srsName>Default</gml:srsName><fdo:WKT>LOCAL_CS[\"*XY-MT*\",LOCAL_DATUM[\"*X-Y*\",10000],UNIT[\"Meter\", 1],AXIS[\"X\",EAST],AXIS[\"Y\",NORTH]]</fdo:WKT></fdo:WKTCRS></gml:baseCRS><gml:definedByConversion xlink:href=\"http://fdo.osgeo.org/coord_conversions#identity\"/><gml:derivedCRSType codeSpace=\"http://fdo.osgeo.org/crs_types\">geographic</gml:derivedCRSType><gml:usesCS xlink:href=\"http://fdo.osgeo.org/cs#default_cartesian\"/></gml:DerivedCRS><xs:schema xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:fdo=\"http://fdo.osgeo.org/schemas\" xmlns:gml=\"http://www.opengis.net/gml\" xmlns:default=\"http://fdo.osgeo.org/schemas/feature/default\" targetNamespace=\"http://fdo.osgeo.org/schemas/feature/default\" elementFormDefault=\"qualified\" attributeFormDefault=\"unqualified\"><xs:annotation><xs:appinfo source=\"http://fdo.osgeo.org/schemas\"/></xs:annotation><xs:element name=\"default\" type=\"default:defaultType\" abstract=\"false\" substitutionGroup=\"gml:_Feature\"><xs:key name=\"defaultKey\"><xs:selector xpath=\".//default\"/>" +
            "<xs:field xpath=\"FeatId\"/></xs:key></xs:element><xs:complexType name=\"defaultType\" abstract=\"false\" fdo:hasGeometry=\"false\"><xs:annotation><xs:appinfo source=\"http://fdo.osgeo.org/schemas\"/></xs:annotation><xs:complexContent><xs:extension base=\"gml:AbstractFeatureType\"><xs:sequence><xs:element name=\"FeatId\"><xs:annotation><xs:appinfo source=\"http://fdo.osgeo.org/schemas\"/></xs:annotation><xs:simpleType><xs:restriction base=\"xs:string\"><xs:maxLength value=\"256\"/></xs:restriction></xs:simpleType></xs:element><xs:element name=\"Image\" type=\"fdo:RasterPropertyType\" fdo:defaultImageXSize=\"1024\" fdo:defaultImageYSize=\"1024\" fdo:srsName=\"Default\"><xs:annotation>" +
            "<xs:appinfo source=\"http://fdo.osgeo.org/schemas\"><fdo:DefaultDataModel dataModelType=\"Bitonal\" dataType=\"Unknown\" organization=\"Pixel\" bitsPerPixel=\"1\" tileSizeX=\"256\" tileSizeY=\"256\"/></xs:appinfo></xs:annotation></xs:element></xs:sequence></xs:extension></xs:complexContent></xs:complexType></xs:schema><SchemaMapping xmlns=\"http://fdogrfp.osgeo.org/schemas\" provider=\"OSGeo.Gdal.3.2\" name=\"default\"></SchemaMapping></fdo:DataStore>";


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
            if (_conf == null)
                BuildDefaultDocument();

            var pdlg = new ProgressDialog();
            pdlg.CancelAbortsThread = true;
            var worker = new ProgressDialog.DoBackgroundWork(UpdateConfigurationDocument);
            var result = (UpdateConfigResult)pdlg.RunOperationAsync(null, worker, _conf, _fs.CurrentConnection, toAdd, toRemove);
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
                    var dir = Path.GetDirectoryName(added);
                    var fileName = Path.GetFileName(added);

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

            worker.ReportProgress(0, Properties.Resources.UpdatingConfiguration);

            int total = toAdd.Length + toRemove.Length;
            int unit = (total / 100);
            int progress = 0;

            var result = new UpdateConfigResult() { Added = new List<string>(), Removed = new List<string>() };

            foreach (var add in toAdd)
            {
                var dir = Path.GetDirectoryName(add);
                var loc = conf.AddLocation(dir);

                //Create a temp feature source to attempt interrogation of extents
                var values = new NameValueCollection();
                values["DefaultRasterFileLocation"] = add;
                var fs = ObjectFactory.CreateFeatureSource(conn, "OSGeo.Gdal", values);

                var resId = new ResourceIdentifier("Session:" + conn.SessionID + "//" + Guid.NewGuid() + ".FeatureSource");
                fs.ResourceID = resId.ToString();
                conn.ResourceService.SaveResource(fs);

                var scList = fs.GetSpatialInfo(false);
                
                var raster = new GdalRasterItem()
                {
                    FileName = Path.GetFileName(add)
                };

                if (scList.SpatialContext.Count > 0)
                {
                    raster.MinX = Convert.ToDouble(scList.SpatialContext[0].Extent.LowerLeftCoordinate.X);
                    raster.MinY = Convert.ToDouble(scList.SpatialContext[0].Extent.LowerLeftCoordinate.Y);
                    raster.MaxX = Convert.ToDouble(scList.SpatialContext[0].Extent.UpperRightCoordinate.X);
                    raster.MaxY = Convert.ToDouble(scList.SpatialContext[0].Extent.UpperRightCoordinate.Y);
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
                worker.ReportProgress(progress, string.Format(Properties.Resources.ProcessedItem, add));
            }

            foreach (var remove in toRemove)
            {
                var dir = Path.GetDirectoryName(remove);
                var loc = FindLocation(conf, dir);
                if (null != loc)
                {
                    loc.RemoveItem(Path.GetFileName(remove));
                    result.Removed.Add(remove);
                    if (loc.Items.Length == 0)
                        conf.RemoveLocation(loc);
                }
                progress += unit;
                worker.ReportProgress(progress, string.Format(Properties.Resources.ProcessedItem, remove));
            }

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
                files.AddRange(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.png"));
                files.AddRange(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.jpg"));
                files.AddRange(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.jpeg"));
                files.AddRange(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.tif"));
                files.AddRange(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.tiff"));
                files.AddRange(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.ecw"));
                files.AddRange(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.sid"));
                files.AddRange(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.dem"));
                files.AddRange(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.gif"));
                files.AddRange(Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.bmp"));

                DoUpdateConfiguration(files.ToArray(), new string[0]);
            }
        }
    }
}
