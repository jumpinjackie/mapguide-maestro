#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

#endregion Disclaimer / License

using ICSharpCode.Core;
using Maestro.Editors;
using Maestro.Editors.LayerDefinition;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#pragma warning disable 1591

namespace Maestro.Base.Editor
{
    /// <summary>
    /// A specialized editor for Layer Definition resources.
    /// </summary>
    /// <remarks>
    /// Although public, this class is undocumented and reserved for internal use by built-in Maestro AddIns
    /// </remarks>
    public partial class LayerDefinitionEditor : EditorContentBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LayerDefinitionEditor()
        {
            InitializeComponent();
        }

        private ILayerDefinition _res;
        private IEditorService _edsvc;
        private bool _init = false;

        /// <summary>
        /// Binds the specified resource to this control. This effectively initializes
        /// all the fields in this control and sets up databinding on all fields. All
        /// subclasses *must* override this method.
        ///
        /// Also note that this method may be called more than once (e.g. Returning from
        /// and XML edit of this resource). Thus subclasses must take this scenario into
        /// account when implementing
        /// </summary>
        /// <param name="service">The editor service</param>
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
                var ed = new VectorLayerEditorCtrl();
                ed.Bind(service);
                ed.Dock = DockStyle.Fill;
                panelBody.Controls.Add(ed);
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
                throw new NotSupportedException(Strings.LayerSubTypeNotSupported);
            }
        }

        protected override void OnBeforeSave(object sender, CancelEventArgs e)
        {
            if (_edsvc.IsNew)
            {
                base.OnBeforeSave(sender, e);
                return;
            }

            List<string> affectedMapDefinitions = new List<string>();
            var resSvc = _edsvc.CurrentConnection.ResourceService;
            var refs = resSvc.EnumerateResourceReferences(_edsvc.ResourceID);
            foreach (var r in refs.ResourceId)
            {
                var resId = new ResourceIdentifier(r);
                if (resId.ResourceType == ResourceTypes.MapDefinition.ToString())
                {
                    var mdf = (IMapDefinition)resSvc.GetResource(r);
                    if (mdf.BaseMap != null)
                    {
                        foreach (var blg in mdf.BaseMap.BaseMapLayerGroups)
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
                if (!MessageService.AskQuestionFormatted(Strings.Confirm, Strings.ConfirmBaseMapInvalidationLayerSave, string.Join(Environment.NewLine, affectedMapDefinitions.ToArray()) + Environment.NewLine))
                {
                    e.Cancel = true;
                    return;
                }
            }

            base.OnBeforeSave(sender, e);
        }

        /// <summary>
        /// Gets whether this resource can be profiled
        /// </summary>
        public override bool CanProfile
        {
            get
            {
                return true;
            }
        }

        private void panelBody_Resize(object sender, EventArgs e)
        {
        }

        public override Icon ViewIcon
        {
            get
            {
                return Properties.Resources.icon_layerdefinition;
            }
        }
    }
}