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
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using System.Diagnostics;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI.Commands;

namespace Maestro.Editors.LoadProcedure
{
    /// <summary>
    /// Editor control for Load Procedures
    /// </summary>
    public partial class LoadProcedureEditorCtrl : EditorBase 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadProcedureEditorCtrl"/> class.
        /// </summary>
        public LoadProcedureEditorCtrl()
        {
            InitializeComponent();
        }

        private OSGeo.MapGuide.ObjectModels.LoadProcedure.ILoadProcedure _lp;

        private IEditorService _ed;

        /// <summary>
        /// Binds the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        public override void Bind(IEditorService service)
        {
            _ed = service;
            _lp = _ed.GetEditedResource() as OSGeo.MapGuide.ObjectModels.LoadProcedure.ILoadProcedure;
            Debug.Assert(_lp != null);
            
            service.RegisterCustomNotifier(this);

            CollapsiblePanel tp = null;

            if (_lp.SubType.Type == LoadType.Sdf)
            {
                var trans = new SdfTransformationCtrl();
                trans.Bind(service);
                tp = trans;
            }
            else if (_lp.SubType.Type == LoadType.Shp)
            {
                var trans = new ShpTransformationCtrl();
                trans.Bind(service);
                tp = trans;
            }
            else if (_lp.SubType.Type == LoadType.Dwf)
            {
                var trans = new DwfTransformationCtrl();
                trans.Bind(service);
                tp = trans;
            }
            else if (_lp.SubType.Type == LoadType.Sqlite)
            {
                var trans = new SqliteTransformationCtrl();
                trans.Bind(service);
                tp = trans;
            }
            else
            {
                throw new NotSupportedException();
            }

            var input = new InputFilesCtrl();
            input.Bind(service);

            var target = new LoadTargetCtrl();
            target.Bind(service);

            tp.Dock = DockStyle.Top;
            input.Dock = DockStyle.Top;
            target.Dock = DockStyle.Top;

            var exec = new ExecuteCtrl();
            exec.Dock = DockStyle.Bottom;
            exec.Execute += new EventHandler(OnExecute);

            this.Controls.Add(exec);
            this.Controls.Add(target);
            this.Controls.Add(tp);
            this.Controls.Add(input);
        }

        void OnExecute(object sender, EventArgs e)
        {
            var pdlg = new ProgressDialog();
            pdlg.CancelAbortsThread = true;

            var worker = new ProgressDialog.DoBackgroundWork(ExecuteLoadProcedure);
            try
            {
                _ed.SyncSessionCopy();
                var result = pdlg.RunOperationAsync(this.ParentForm, worker, _ed, _lp);
                MessageBox.Show(Strings.OperationCompleted);
                _ed.RequestRefresh(_lp.SubType.RootPath);

                //Load procedure may have modified this resource as part of executioin
                _ed.SyncSessionCopy();
                //HACK: Force dirty state as successful execution writes some extra XML content to the resource
                _ed.MarkDirty();
            }
            catch (CancelException)
            {
                MessageBox.Show(Strings.OperationCancelled);
            }
        }

        object ExecuteLoadProcedure(BackgroundWorker worker, DoWorkEventArgs e, params object[] args)
        {
            OSGeo.MapGuide.MaestroAPI.LengthyOperationProgressCallBack cb = (s, cbArgs) =>
            {
                worker.ReportProgress(cbArgs.Progress, cbArgs.StatusMessage);
            };

            IEditorService ed = (IEditorService)args[0];
            var proc = (ILoadProcedure)args[1];

            return proc.CurrentConnection.ExecuteLoadProcedure(proc, cb, true);
        }
    }
}
