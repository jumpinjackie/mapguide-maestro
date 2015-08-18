#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
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

#endregion Disclaimer / License
using OSGeo.FDO.Expressions;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Maestro.Editors.Common.Expression
{
    internal interface IExpressionErrorSource
    {
        void SetCursor(int line, int col);

        void HighlightToken(string token);
    }

    internal partial class MalformedExpressionDialog : Form
    {
        private MalformedExpressionDialog()
        {
            InitializeComponent();
        }

        private readonly FdoMalformedExpressionException _ex;
        private readonly IExpressionErrorSource _source;

        public MalformedExpressionDialog(FdoMalformedExpressionException ex, IExpressionErrorSource source)
            : this()
        {
            _ex = ex;
            _source = source;
            txtErrorDetails.Text = _ex.Messages[0].Message;
        }

        private void btnClose_Click(object sender, EventArgs e) => this.DialogResult = System.Windows.Forms.DialogResult.OK;

        private void btnGotoError_Click(object sender, EventArgs e)
        {
            var msg = _ex.Messages.First();
            _source.SetCursor(msg.LineNumber, msg.Column - 1);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
