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
using OSGeo.MapGuide.MaestroAPI.Resource;
using Maestro.Editors;
using ICSharpCode.Core;
using OSGeo.MapGuide.ObjectModels.MapDefinition;

namespace Maestro.Base.Editor
{
    public partial class FeatureSourceEditor : EditorContentBase
    {
        public FeatureSourceEditor()
        {
            InitializeComponent();
        }

        private IResource _res;
        private IEditorService _edsvc;

        protected override void Bind(IEditorService service)
        {
            _edsvc = service;
            _res = _edsvc.GetEditedResource();
            var fsOpts = new FsEditorOptionPanel();
            fsOpts.Dock = DockStyle.Top;
            fsOpts.Bind(service);
            fsEditorCtrl.Controls.Add(fsOpts);
            fsEditorCtrl.Bind(service);
        }

        protected override void OnBeforeSave(object sender, CancelEventArgs e)
        {
            List<string> affectedMapDefinitions = new List<string>();
            var refs = _edsvc.ResourceService.EnumerateResourceReferences(_edsvc.ResourceID);
            foreach (var r in refs.ResourceId)
            {
                ResourceIdentifier rid = new ResourceIdentifier(r);
                if (rid.ResourceType == OSGeo.MapGuide.MaestroAPI.ResourceTypes.LayerDefinition)
                {
                    var lrefs = _edsvc.ResourceService.EnumerateResourceReferences(r);
                    foreach (var lr in lrefs.ResourceId)
                    {
                        ResourceIdentifier rid2 = new ResourceIdentifier(lr);
                        if (rid2.ResourceType == OSGeo.MapGuide.MaestroAPI.ResourceTypes.MapDefinition)
                        {
                            var mdf = (IMapDefinition)_edsvc.ResourceService.GetResource(lr);
                            if (mdf.BaseMap != null)
                            {
                                foreach (var blg in mdf.BaseMap.BaseMapLayerGroup)
                                {
                                    foreach (var bl in blg.BaseMapLayer)
                                    {
                                        if (bl.ResourceId.Equals(r))
                                        {
                                            affectedMapDefinitions.Add(r);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (affectedMapDefinitions.Count > 0)
            {
                if (!MessageService.AskQuestionFormatted(Properties.Resources.Confirm, Properties.Resources.ConfirmBaseMapInvalidationFeatureSourceSave, string.Join(Environment.NewLine, affectedMapDefinitions.ToArray()) + Environment.NewLine))
                {
                    e.Cancel = true;
                    return;
                }
            }
            base.OnBeforeSave(sender, e);
        }
    }
}
