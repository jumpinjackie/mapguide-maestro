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
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Maestro.Editors.Common;
using Maestro.Editors.FeatureSource.Extensions;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace Maestro.Editors.FeatureSource
{
    [ToolboxItem(false)]
    internal partial class ExtensionsCtrl : EditorBindableCollapsiblePanel
    {
        public ExtensionsCtrl()
        {
            InitializeComponent();
        }

        const int IDX_EXTENSION = 0;
        const int IDX_CALC = 1;
        const int IDX_JOIN = 2;

        private IFeatureSource _fs;
        private IEditorService _edSvc;

        public override void Bind(IEditorService service)
        {
            service.RegisterCustomNotifier(this);
            _edSvc = service;
            _edSvc.Saved += OnResourceSaved;
            _fs = (IFeatureSource)_edSvc.GetEditedResource();

            //Build tree
            if (_fs.Extension != null)
            {
                foreach (var ext in _fs.Extension)
                {
                    TreeNode node = new TreeNode();
                    node.Tag = ext;
                    node.ImageIndex = node.SelectedImageIndex = IDX_EXTENSION;

                    node.Text = ext.Name;
                    node.ToolTipText = string.Format(Strings.ExtendedClassTooltip, ext.FeatureClass);

                    ext.PropertyChanged += (s, evt) =>
                    {
                        if (evt.PropertyName == "Name") //NOXLATE
                        {
                            node.Text = ext.Name;
                        }
                        else if (evt.PropertyName == "FeatureClass") //NOXLATE
                        {
                            node.ToolTipText = string.Format(Strings.ExtendedClassTooltip, ext.FeatureClass);
                        }
                    };

                    trvExtensions.Nodes.Add(node);

                    if (ext.CalculatedProperty != null)
                    {
                        foreach (var calc in ext.CalculatedProperty)
                        {
                            var cNode = new TreeNode();
                            cNode.ImageIndex = cNode.SelectedImageIndex = IDX_CALC;
                            cNode.Tag = calc;

                            cNode.Text = calc.Name;
                            cNode.ToolTipText = calc.Expression;

                            calc.PropertyChanged += (s, evt) =>
                            {
                                if (evt.PropertyName == "Name") //NOXLATE
                                {
                                    cNode.Text = calc.Name;
                                }
                                else if (evt.PropertyName == "Expression") //NOXLATE
                                {
                                    cNode.ToolTipText = calc.Expression;
                                }
                            };

                            node.Nodes.Add(cNode);
                        }
                    }
                    if (ext.AttributeRelate != null)
                    {
                        foreach (var join in ext.AttributeRelate)
                        {
                            var jNode = new TreeNode();
                            jNode.Tag = join;
                            jNode.ImageIndex = jNode.SelectedImageIndex = IDX_JOIN;

                            jNode.Text = join.Name;

                            join.PropertyChanged += (s, evt) =>
                            {
                                if (evt.PropertyName == "Name") //NOXLATE
                                {
                                    jNode.Text = join.Name;
                                }
                            };

                            node.Nodes.Add(jNode);
                        }
                    }
                    node.ExpandAll();
                }
            }
        }

        protected override void UnsubscribeEventHandlers()
        {
            base.UnsubscribeEventHandlers();
            _edSvc.Saved -= OnResourceSaved;
        }

        void OnResourceSaved(object sender, EventArgs e)
        {
            Debug.Assert(!_edSvc.IsNew);
        }

        private void btnNewExtension_Click(object sender, EventArgs e)
        {
            if (_edSvc.IsNew)
            {
                MessageBox.Show(Strings.SaveResourceFirst);
                return;
            }

            var ext = ObjectFactory.CreateFeatureSourceExtension();
            TreeNode node = new TreeNode();
            node.Tag = ext;
            node.ImageIndex = node.SelectedImageIndex = IDX_EXTENSION;
            ext.PropertyChanged += (s, evt) =>
            {
                if (evt.PropertyName == "Name") //NOXLATE
                {
                    node.Text = ext.Name;
                }
                else if (evt.PropertyName == "FeatureClass") //NOXLATE
                {
                    node.ToolTipText = string.Format(Strings.ExtendedClassTooltip, ext.FeatureClass);
                }
            };

            _fs.AddExtension(ext);
            OnResourceChanged();

            trvExtensions.Nodes.Add(node);
            trvExtensions.SelectedNode = node;
        }

        private void btnNewCalculation_Click(object sender, EventArgs e)
        {
            if (_edSvc.IsNew)
            {
                MessageBox.Show(Strings.SaveResourceFirst);
                return;
            }

            var node = trvExtensions.SelectedNode;
            if (node != null)
            {
                var ext = node.Tag as IFeatureSourceExtension;
                if (ext != null)
                {
                    var calc = ObjectFactory.CreateCalculatedProperty();
                    var cNode = new TreeNode();
                    cNode.ImageIndex = cNode.SelectedImageIndex = IDX_CALC;
                    cNode.Tag = calc;
                    calc.PropertyChanged += (s, evt) =>
                    {
                        if (evt.PropertyName == "Name") //NOXLATE
                        {
                            cNode.Text = calc.Name;
                        }
                        else if (evt.PropertyName == "Expression") //NOXLATE
                        {
                            cNode.ToolTipText = calc.Expression;
                        }
                    };

                    node.Nodes.Add(cNode);
                    node.Expand();

                    ext.AddCalculatedProperty(calc);
                    OnResourceChanged();

                    trvExtensions.SelectedNode = cNode;
                }
            }
        }

        private void btnNewJoin_Click(object sender, EventArgs e)
        {
            if (_edSvc.IsNew)
            {
                MessageBox.Show(Strings.SaveResourceFirst);
                return;
            }

            var node = trvExtensions.SelectedNode;
            if (node != null)
            {
                var ext = node.Tag as IFeatureSourceExtension;
                if (ext != null)
                {
                    var join = ObjectFactory.CreateAttributeRelation();
                    var jNode = new TreeNode();
                    jNode.Tag = join;
                    jNode.ImageIndex = jNode.SelectedImageIndex = IDX_JOIN;
                    join.PropertyChanged += (s, evt) =>
                    {
                        if (evt.PropertyName == "Name") //NOXLATE
                        {
                            jNode.Text = join.Name;
                        }
                    };

                    node.Nodes.Add(jNode);
                    node.Expand();

                    ext.AddRelation(join);
                    OnResourceChanged();

                    trvExtensions.SelectedNode = jNode;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var node = trvExtensions.SelectedNode;
            if (node != null)
            {
                var ext = node.Tag as IFeatureSourceExtension;
                var join = node.Tag as IAttributeRelation;
                var calc = node.Tag as ICalculatedProperty;
                if (ext != null)
                {
                    _fs.RemoveExtension(ext);
                    OnResourceChanged();
                    trvExtensions.Nodes.Remove(node);
                }
                else if (join != null)
                {
                    //Disassociate from parent
                    ext = node.Parent.Tag as IFeatureSourceExtension;
                    if (ext != null)
                    {
                        ext.RemoveRelation(join);
                        OnResourceChanged();
                        node.Parent.Nodes.Remove(node);
                    }
                }
                else if (calc != null)
                {
                    //Disassociate from parent
                    ext = node.Parent.Tag as IFeatureSourceExtension;
                    if (ext != null)
                    {
                        ext.RemoveCalculatedProperty(calc);
                        OnResourceChanged();
                        node.Parent.Nodes.Remove(node);
                    }
                }
            }
        }

        private bool IsValidExtension(IFeatureSourceExtension ext)
        {
            //TODO: Check class name and extended class name for validity
            return !string.IsNullOrEmpty(ext.FeatureClass) && !string.IsNullOrEmpty(ext.Name);
        }

        private void trvExtensions_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_edSvc.IsNew)
            {
                return;
            }

            var ext = e.Node.Tag as IFeatureSourceExtension;
            var join = e.Node.Tag as IAttributeRelation;
            var calc = e.Node.Tag as ICalculatedProperty;
            if (ext != null)
            {
                var ctl = new ExtendedClassSettings(_fs, GetAllClassNames(), ext);
                ctl.Dock = DockStyle.Fill;
                //If editing to something valid, update the toolbar
                ctl.ResourceChanged += (s, evt) =>
                {
                    btnNewJoin.Enabled = btnNewCalculation.Enabled = IsValidExtension(ext);
                };

                splitContainer1.Panel2.Controls.Clear();
                splitContainer1.Panel2.Controls.Add(ctl);
                btnDelete.Enabled = true;

                btnNewCalculation.Enabled = btnNewJoin.Enabled = IsValidExtension(ext);
            }
            else if (join != null)
            {
                ext = e.Node.Parent.Tag as IFeatureSourceExtension;
                if (ext != null)
                {
                    if (ext.FeatureClass != null)
                    {
                        //NOTE: The feature source id here may be session based, but this is still okay
                        //as we're only giving context (the primary class to join on) for the secondary join UI. 
                        //This feature source id is never written into the actual document
                        var ctl = new JoinSettings(_fs.ResourceID, ext.FeatureClass, join);
                        ctl.Bind(_edSvc);
                        ctl.Dock = DockStyle.Fill;
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(ctl);
                        btnDelete.Enabled = true;
                    }
                }
            }
            else if (calc != null)
            {
                ext = e.Node.Parent.Tag as IFeatureSourceExtension;
                if (ext != null)
                {
                    ClassDefinition cls = _fs.GetClass(ext.FeatureClass); //TODO: Cache?
                    if (cls != null)
                    {
                        var ctl = new CalculationSettings(_edSvc, cls, _fs, calc);
                        ctl.Dock = DockStyle.Fill;
                        splitContainer1.Panel2.Controls.Clear();
                        splitContainer1.Panel2.Controls.Add(ctl);
                        btnDelete.Enabled = true;
                    }
                }
            }
            else 
            {
                splitContainer1.Panel2.Controls.Clear();
            }
        }

        string[] GetAllClassNames()
        {
            var names = new List<string>();
            foreach (var sn in _fs.GetSchemaNames())
            {
                names.AddRange(_fs.GetClassNames(sn));
            }
            return names.ToArray();
        }
    }
}
