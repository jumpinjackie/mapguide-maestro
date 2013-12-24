using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Schema;
using Maestro.Editors.WatermarkDefinition;

namespace Maestro.Editors.LayerDefinition
{
    /// <summary>
    /// Editor control for Vector Layer Definitions
    /// </summary>
    public partial class VectorLayerEditorCtrl : EditorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VectorLayerEditorCtrl"/> class.
        /// </summary>
        public VectorLayerEditorCtrl()
        {
            InitializeComponent();
            resSettings.FeatureClassChanged += new EventHandler(OnFeatureClassChanged);
            layerStyles.Owner = this;
        }

        void OnFeatureClassChanged(object sender, EventArgs e)
        {
            layerProperties.PopulatePropertyList();
        }

        private IEditorService _edsvc;
        private IVectorLayerDefinition _vl;

        internal IEditorService EditorService { get { return _edsvc; } }

        /// <summary>
        /// Sets the initial state of this editor and sets up any databinding
        /// within such that user interface changes will propagate back to the
        /// model.
        /// </summary>
        /// <param name="service"></param>
        public override void Bind(IEditorService service)
        {
            _edsvc = service;
            _vl = (IVectorLayerDefinition)((ILayerDefinition)service.GetEditedResource()).SubLayer;

            service.RegisterCustomNotifier(this);
            resSettings.Bind(service);
            layerProperties.Bind(service);
            layerStyles.Bind(service);

            //Add watermark component if supported
            var sl2 = _vl as ISubLayerDefinition2;
            if (sl2 != null)
            {
                this.Controls.Remove(resSettings);
                this.Controls.Remove(layerProperties);
                this.Controls.Remove(layerStyles);

                var wm = new WatermarkCollectionEditorCtrl(service, sl2);
                wm.Dock = DockStyle.Top;

                this.Controls.Add(wm);
                this.Controls.Add(layerStyles);
                this.Controls.Add(layerProperties);
                this.Controls.Add(resSettings);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.UserControl.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            //HACK: Maybe the controls were still in the process of databinding which
            //is why the feature class would still be empty. Nevertheless, it will be
            //set at this point so it's safe to call here.
            layerProperties.PopulatePropertyList();
        }

        internal IEditorService Editor
        {
            get { return _edsvc; }
        }

        internal string EditExpression(string expr, bool attachStylizationFunctions)
        {
            var fs = (IFeatureSource)_edsvc.ResourceService.GetResource(_vl.ResourceId);

            return _edsvc.EditExpression(expr, fs.GetClass(_vl.FeatureName), fs.Provider, _vl.ResourceId, attachStylizationFunctions);
        }

        internal void UpdateDisplay()
        {
            
        }

        internal void HasChanged()
        {
            _edsvc.HasChanged();
        }

        internal void SetLastException(Exception ex)
        {
            
        }

        internal void FlagDirty() { OnResourceChanged(); }

        internal string GetFdoProvider()
        {
            var fs = (IFeatureSource)_edsvc.ResourceService.GetResource(_vl.ResourceId);
            return fs.Provider;
        }

        internal ClassDefinition SelectedClass
        {
            get { return resSettings.GetSelectedClass(); }
        }

        internal string FeatureSourceId
        {
            get { return resSettings.FeatureSourceID; }
        }
    }
}
