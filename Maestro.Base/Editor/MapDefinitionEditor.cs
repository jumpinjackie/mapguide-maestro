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
using Maestro.Editors.MapDefinition;

#pragma warning disable 1591

namespace Maestro.Base.Editor
{
    /// <summary>
    /// A specialized editor for Map Definition resources.
    /// </summary>
    /// <remarks>
    /// Although public, this class is undocumented and reserved for internal use by built-in Maestro AddIns
    /// </remarks>
    public partial class MapDefinitionEditor : EditorContentBase
    {
        public MapDefinitionEditor()
        {
            InitializeComponent();
        }

        private IResource _res;
        private IEditorService _edsvc;
        private bool _init = false;

        protected override void Bind(IEditorService service)
        {
            if (!_init)
            {
                _edsvc = service;
                _res = _edsvc.GetEditedResource();
                _init = true;
            }

            panelBody.Controls.Clear();
            var mapEditorCtrl = new MapDefinitionEditorCtrl();
            mapEditorCtrl.Dock = DockStyle.Fill;
            panelBody.Controls.Add(mapEditorCtrl);

            mapEditorCtrl.Bind(service);
        }

        protected override void OnBeforeSave(object sender, CancelEventArgs e)
        {
            var mdf = (IMapDefinition)_edsvc.GetEditedResource();
            //Unfortunately we can't hook in the standard validation as the act of
            //committing the xml content back, will trigger the invalid XML content error
            //before the standard validation is performed. So check this one particular
            //validation rule here before we go ahead
            if (mdf.BaseMap != null && mdf.BaseMap.HasLayers())
            {
                if (mdf.BaseMap.ScaleCount == 0)
                {
                    MessageService.ShowMessage(Strings.NoFiniteDisplayScalesSpecified);
                    e.Cancel = true;
                    return;
                }
            }

            if (_edsvc.IsNew)
            {
                base.OnBeforeSave(sender, e);
                return;
            }

            if (mdf.BaseMap != null)
            {
                if (mdf.BaseMap.HasLayers())
                {
                    if (!MessageService.AskQuestion(Strings.ConfirmBaseMapInvalidation))
                    {
                        e.Cancel = true;
                        return;
                    }
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

        public override Icon ViewIcon
        {
            get
            {
                return Properties.Resources.icon_mapdefinition;
            }
        }
    }
}
