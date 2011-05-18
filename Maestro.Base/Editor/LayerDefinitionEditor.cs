#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
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
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using System.Diagnostics;
using Maestro.Editors.LayerDefinition;
using OSGeo.MapGuide.MaestroAPI.Resource;
using Maestro.Editors;
using ICSharpCode.Core;
using OSGeo.MapGuide.ObjectModels;
using Maestro.Base.UI.Preferences;
using OSGeo.MapGuide.ObjectModels.MapDefinition;

namespace Maestro.Base.Editor
{
    public partial class LayerDefinitionEditor : EditorContentBase
    {
        public LayerDefinitionEditor()
        {
            InitializeComponent();
        }

        private ILayerDefinition _res;
        private IEditorService _edsvc;
        private bool _init = false;

        protected override void Bind(Maestro.Editors.IEditorService service)
        {
            if (!_init)
            {
                _edsvc = service;
                _res = service.GetEditedResource() as ILayerDefinition;
                Debug.Assert(_res != null);
                _init = true;
            }

            panelBody.Controls.Clear();

            var vl = _res.SubLayer as IVectorLayerDefinition;
            var gl = _res.SubLayer as IRasterLayerDefinition;
            var dl = _res.SubLayer as IDrawingLayerDefinition;
            if (vl != null)
            {
                //TODO: This is a stopgap measure until we have proper editor support for Composite Type styles.
                //Until then, we check if it has composite style rules. Show a placeholder if we find any because this
                //editor cannot edit them
                if (HasCompositeRules(vl))
                {
                    var ed = new UnsupportedEditorControl();
                    ed.Dock = DockStyle.Fill;
                    panelBody.Controls.Add(ed);
                }
                else
                {
                    var ed = new VectorLayerEditorCtrl();
                    ed.Bind(service);
                    ed.Dock = DockStyle.Fill;
                    panelBody.Controls.Add(ed);
                }
            }
            else if (gl != null)
            {
                var ed = new RasterLayerEditorCtrl();
                ed.Bind(service);
                ed.Dock = DockStyle.Fill;
                panelBody.Controls.Add(ed);
            }
            else if (dl != null)
            {
                var ed = new DrawingLayerEditorCtrl();
                ed.Bind(service);
                ed.Dock = DockStyle.Fill;
                panelBody.Controls.Add(ed);
            }
            else
            {
                throw new NotSupportedException(Properties.Resources.LayerSubTypeNotSupported);
            }
        }

        private static bool HasCompositeRules(IVectorLayerDefinition vl)
        {
            foreach (var vsr in vl.VectorScaleRange)
            {
                var vsr3 = vsr as IVectorScaleRange3;
                if (vsr3 != null && vsr3.CompositeStyle != null)
                    return true;
            }
            return false;
        }

        protected override void OnBeforeSave(object sender, CancelEventArgs e)
        {
            if (_edsvc.IsNew)
            {
                base.OnBeforeSave(sender, e);
                return;
            }

            List<string> affectedMapDefinitions = new List<string>();
            var refs = _edsvc.ResourceService.EnumerateResourceReferences(_edsvc.ResourceID);
            foreach (var r in refs.ResourceId)
            {
                var resId = new ResourceIdentifier(r);
                if (resId.ResourceType == OSGeo.MapGuide.MaestroAPI.ResourceTypes.MapDefinition)
                {
                    var mdf = (IMapDefinition)_edsvc.ResourceService.GetResource(r);
                    if (mdf.BaseMap != null)
                    {
                        foreach (var blg in mdf.BaseMap.BaseMapLayerGroup)
                        {
                            foreach (var bl in blg.BaseMapLayer)
                            {
                                if (bl.ResourceId.Equals(_edsvc.ResourceID))
                                {
                                    affectedMapDefinitions.Add(r);
                                }
                            }
                        }
                    }
                }
            }

            if (affectedMapDefinitions.Count > 0)
            {
                if (!MessageService.AskQuestionFormatted(Properties.Resources.Confirm, Properties.Resources.ConfirmBaseMapInvalidationLayerSave, string.Join(Environment.NewLine, affectedMapDefinitions.ToArray()) + Environment.NewLine))
                {
                    e.Cancel = true;
                    return;
                }
            }

            base.OnBeforeSave(sender, e);
        }

        public override bool CanProfile
        {
            get
            {
                return true;
            }
        }
    }
}
