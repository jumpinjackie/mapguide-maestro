﻿#region Disclaimer / License

// Copyright (C) 2011, Jackie Ng
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

using Maestro.Editors.Common;
using Maestro.Shared.UI;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using System;
using System.Windows.Forms;

namespace Maestro.Editors.SymbolDefinition.GraphicsEditors
{
    internal partial class TextDialog : Form
    {
        private ISimpleSymbolDefinition _sym;
        private EditorBindableCollapsiblePanel _ed;
        private readonly ITextGraphic _text;
        private ITextFrame _frame;

        public TextDialog(EditorBindableCollapsiblePanel parent, ISimpleSymbolDefinition sym, ITextGraphic text)
        {
            InitializeComponent();
            _ed = parent;
            _sym = sym;
            _text = text;
        }

        private bool _init = false;

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                _init = true;

                _frame = _text.Frame;

                chkTextFrame.Checked = (_frame != null);
                grpTextFrame.Enabled = chkTextFrame.Checked;

                if (_frame == null)
                    _frame = _sym.CreateFrame();

                TextBoxBinder.BindText(txtContent, _text, nameof(_text.Content));

                symAlignmentHorizontal.Bind(_text, nameof(_text.HorizontalAlignment));
                symAlignmentJustification.Bind(_text, nameof(_text.Justification));
                symAlignmentVertical.Bind(_text, nameof(_text.VerticalAlignment));
                symAngle.Bind(_text, nameof(_text.Angle));
                symFontBold.Bind(_text, nameof(_text.Bold));
                symFontFamily.Bind(_text, nameof(_text.FontName));
                symFontItalic.Bind(_text, nameof(_text.Italic));
                symFontUnderlined.Bind(_text, nameof(_text.Underlined));
                symGhostColor.Bind(_text, nameof(_text.GhostColor));
                symHeight.Bind(_text, nameof(_text.Height));
                symHeightScalable.Bind(_text, nameof(_text.HeightScalable));
                symLineSpacing.Bind(_text, nameof(_text.LineSpacing));
                symPositionX.Bind(_text, nameof(_text.PositionX));
                symPositionY.Bind(_text, nameof(_text.PositionY));
                symTextColor.Bind(_text, nameof(_text.TextColor));

                symFillColor.Bind(_frame, nameof(_frame.FillColor));
                symLineColor.Bind(_frame, nameof(_frame.LineColor));
                symOffsetX.Bind(_frame, nameof(_frame.OffsetX));
                symOffsetY.Bind(_frame, nameof(_frame.OffsetY));

                var text2 = _text as ITextGraphic2;
                if (text2 != null)
                {
                    symOverlined.Bind(text2, nameof(text2.Overlined));
                    symObliqueAngle.Bind(text2, nameof(text2.ObliqueAngle));
                    symTrackSpacing.Bind(text2, nameof(text2.TrackSpacing));
                    symMarkup.Bind(text2, nameof(text2.Markup));
                }
                else
                {
                    tabControl1.TabPages.Remove(TAB_ADVANCED);
                }
            }
            finally
            {
                _init = false;
            }
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            if (_init)
                return;
            _ed.RaiseResourceChanged();
        }

        private void OnRequestBrowse(SymbolField sender) => ParameterSelector.ShowParameterSelector(_sym.ParameterDefinition.Parameter, sender);

        private void chkTextFrame_CheckedChanged(object sender, EventArgs e)
        {
            grpTextFrame.Enabled = chkTextFrame.Checked;
            if (_init)
                return;

            if (chkTextFrame.Checked)
                _text.Frame = _frame;
            else
                _text.Frame = null;
        }

        private void btnContent_Click(object sender, EventArgs e)
        {
            string content = null;
            ParameterSelector.ShowParameterSelector(_sym.ParameterDefinition.Parameter, ref content);
            if (content != null)
                txtContent.Text = content;
        }

        private void btnClose_Click(object sender, EventArgs e) => this.Close();

        private void lnkSelectFont_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                var font = fd.Font;
                _text.FontName = $"'{font.Name}'"; //NOXLATE
                if (font.Bold)
                    _text.Bold = font.Bold.ToString();
                if (font.Italic)
                    _text.Italic = font.Italic.ToString();
                if (font.Underline)
                    _text.Underlined = font.Underline.ToString();
                _text.Height = font.Size.ToString();
            }
        }
    }
}