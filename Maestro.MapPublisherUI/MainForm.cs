using Maestro.MapPublisher.Common;
using Maestro.MapPublisherUI.Controls;
using OSGeo.MapGuide.MaestroAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Maestro.MapPublisherUI
{
    public partial class MainForm : Form
    {
        private PublishProfile _profile;
        private OpenLayersViewerOptions _olViewerOptions;
        private LeafletViewerOptions _leafletViewerOptions;
        private MapGuideReactLayoutViewerOptions _mrlViewerOptions;

        private List<ExternalBaseLayer> _externalBaseLayers;
        private IServerConnection _conn;
        private int _baseLayerCounter = 0;

        public MainForm()
        {
            InitializeComponent();
            _externalBaseLayers = new List<ExternalBaseLayer>();
            _profile = new PublishProfile();
            _profile.ExternalBaseLayers = _externalBaseLayers;
            _olViewerOptions = new OpenLayersViewerOptions();
            _leafletViewerOptions = new LeafletViewerOptions();
            _mrlViewerOptions = new MapGuideReactLayoutViewerOptions();
        }

        private void wizardPage1_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            var siteList = Login.PreferredSiteList.Load();
            httpLoginCtrl.AddSites(siteList.Sites);
            //In case the site was removed...
            try { httpLoginCtrl.SetPreferredSite(siteList.PreferedSite); }
            catch { }
        }

        private void CheckWizard2NextStatus()
        {
            wizardPage2.AllowNext = rdLeaflet.Checked || rdOpenLayers.Checked || rdMrl.Checked;
            if (rdLeaflet.Checked)
                _profile.ViewerOptions = _leafletViewerOptions;
            else if (rdOpenLayers.Checked)
                _profile.ViewerOptions = _olViewerOptions;
            else if (rdMrl.Checked)
                _profile.ViewerOptions = _mrlViewerOptions;
        }

        private void wizardPage2_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            CheckWizard2NextStatus();
        }

        private void HACKUpdateExternalLayersList()
        {
            // HACK: Why are we not using a data-bound BindingList<T>? Because of this:
            // https://stackoverflow.com/questions/41450370/why-does-the-selectedindexchanged-event-fire-in-a-listbox-when-the-selected-item
            // Completely wrecks our contextual UI that depends on BindingList<T> item changes *not* triggering
            // a SelectedIndexChanged event on the ListBox!!!
            lstExternalLayers.Items.Clear();
            foreach (var item in _externalBaseLayers)
            {
                lstExternalLayers.Items.Add(item);
            }
        }

        private void wizardPage3_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            HACKUpdateExternalLayersList();
            btnDeleteExternalBaseLayer.Enabled = false;
            externalBaseLayerSplitContainer.Panel2.Controls.Clear();
        }

        private void wizardPage4_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {

        }

        private void wizardPage5_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {

        }

        private void rdOpenLayers_CheckedChanged(object sender, EventArgs e) => CheckWizard2NextStatus();

        private void rdLeaflet_CheckedChanged(object sender, EventArgs e) => CheckWizard2NextStatus();

        private void rdMrl_CheckedChanged(object sender, EventArgs e) => CheckWizard2NextStatus();

        private void httpLoginCtrl_EnableOk(object sender, EventArgs e)
        {
            wizardPage1.AllowNext = true;
        }

        private void httpLoginCtrl_DisabledOk(object sender, EventArgs e)
        {
            wizardPage1.AllowNext = false;
        }

        private void wizardPage1_Commit(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            var builder = new System.Data.Common.DbConnectionStringBuilder();
            builder["Url"] = httpLoginCtrl.Server; //NOXLATE
            builder["Username"] = httpLoginCtrl.Username; //NOXLATE
            builder["Password"] = httpLoginCtrl.Password; //NOXLATE
            builder["Locale"] = httpLoginCtrl.Language; //NOXLATE
            builder["AllowUntestedVersion"] = true; //NOXLATE

            string agent = "MapGuide Maestro v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); //NOXLATE

            try
            {
                _conn = ConnectionProviderRegistry.CreateConnection("Maestro.Http", builder.ToString()); //NOXLATE
                _conn.SetCustomProperty("UserAgent", agent); //NOXLATE

                // Do a version check to see if this connection checks out
                var ver = _conn.SiteVersion;
            }
            catch (Exception ex)
            {
                _conn = null;
                MessageBox.Show($"Failed to connect: {ex.Message}");
                e.Cancel = true;
            }
        }

        private object _lastSelectedExternalLayer;

        private bool _updatingLstExternalLayers = false;

        private void lstExternalLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updatingLstExternalLayers)
                return;

            btnDeleteExternalBaseLayer.Enabled = lstExternalLayers.SelectedItem != null;
            if (_lastSelectedExternalLayer != lstExternalLayers.SelectedItem)
            {
                externalBaseLayerSplitContainer.Panel2.Controls.Clear();
                if (lstExternalLayers.SelectedItem != null)
                {
                    var ctrl = new ExternalBaseLayerEditorCtrl();
                    ctrl.Layer = (ExternalBaseLayer)lstExternalLayers.SelectedItem;
                    ctrl.Dock = DockStyle.Fill;
                    externalBaseLayerSplitContainer.Panel2.Controls.Add(ctrl);
                }
            }
            _lastSelectedExternalLayer = lstExternalLayers.SelectedItem;

            HACKUpdateExternalLayersList();
            try
            {
                _updatingLstExternalLayers = true;
                lstExternalLayers.SelectedItem = _lastSelectedExternalLayer;
            }
            finally
            {
                _updatingLstExternalLayers = false;
            }
        }

        private void AddItem(ExternalBaseLayer item)
        {
            _externalBaseLayers.Add(item);

            HACKUpdateExternalLayersList();

            lstExternalLayers.SelectedItem = item;
        }

        private void openStreetMapToolStripMenuItem_Click(object sender, EventArgs e) => AddItem(new OSMBaseLayer { Name = $"ExternalBaseLayer{_baseLayerCounter++}" });

        private void stamenToolStripMenuItem_Click(object sender, EventArgs e) => AddItem(new StamenBaseLayer { Name = $"ExternalBaseLayer{_baseLayerCounter++}" });

        private void bingMapsToolStripMenuItem_Click(object sender, EventArgs e) => AddItem(new BingMapsBaseLayer { Name = $"ExternalBaseLayer{_baseLayerCounter++}" });

        private void customXYZTileSetToolStripMenuItem_Click(object sender, EventArgs e) => AddItem(new XYZBaseLayer { Name = $"ExternalBaseLayer{_baseLayerCounter++}" });

        private void btnDeleteExternalBaseLayer_Click(object sender, EventArgs e)
        {
            if (lstExternalLayers.SelectedItem != null)
            {
                _externalBaseLayers.Remove((ExternalBaseLayer)lstExternalLayers.SelectedItem);
                lstExternalLayers.Items.Remove(lstExternalLayers.SelectedItem);
                if (_externalBaseLayers.Count == 0)
                    externalBaseLayerSplitContainer.Panel2.Controls.Clear();
            }
        }
    }
}
