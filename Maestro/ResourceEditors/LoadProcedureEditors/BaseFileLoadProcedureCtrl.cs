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

namespace OSGeo.MapGuide.Maestro.ResourceEditors.LoadProcedureEditors
{
    public partial class BaseFileLoadProcedureCtrl : UserControl, ILoadProcedureEditor
    {
        protected EditorInterface _ed;
        private string _resourceID;

        public event EventHandler ResourceModified;

        public BaseFileLoadProcedureCtrl()
        {
            InitializeComponent();
        }

        public BaseFileLoadProcedureCtrl(EditorInterface ed)
            : this()
        {
            InitializeComponent();
            _ed = ed;
        }

        public BaseFileLoadProcedureCtrl(EditorInterface ed, string resourceID)
            : base()
        {
            InitializeComponent();
            _ed = ed;
            _resourceID = resourceID;
        }

        public virtual bool CanExecute
        {
            get { return true; }
        }

        public object Resource
        {
            get;
            set;
        }

        public virtual void UpdateDisplay()
        {
            
        }

        public string ResourceId
        {
            get;
            set;
        }

        public virtual bool Preview()
        {
            return false;
        }

        public virtual bool ValidateResource(bool recursive)
        {
            return false;
        }

        public virtual bool Profile()
        {
            return false;
        }

        public virtual bool SupportsPreview
        {
            get { return false; }
        }

        public virtual bool SupportsValidate
        {
            get { return false; }
        }

        public virtual bool SupportsProfiling
        {
            get { return false; }
        }

        public virtual bool Save(string savename)
        {
            return false;
        }

        protected virtual void RaiseModified()
        {
            var handler = this.ResourceModified;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public virtual string[] GetAffectedResourceIds()
        {
            return new string[0];
        }
    }
}
