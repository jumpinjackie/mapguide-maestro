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
using System.Drawing.Design;

namespace Maestro.Shared.UI
{
    /// <summary>
    /// A simple collapsible panel with basic properties for configuring header color/font
    /// and content color. 
    /// 
    /// This control works best when Dock = Top and any content below is also Dock = Top and this
    /// control was built with these assumptions in place.
    /// 
    /// Note that there is no designer support for this control (ie. Drag and drop does not do what
    /// you would hope it would do). The way to use this control is to derive from this class and add your custom 
    /// content there. Also this class does not appear in the VS Toolbox, thus you must apply ToolboxItemAttribute(true)
    /// on your derived classes if you want to make the control available for design.
    /// </summary>
    [ToolboxItem(false)]
    public partial class CollapsiblePanel : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollapsiblePanel"/> class.
        /// </summary>
        protected CollapsiblePanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.UserControl.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            btnCollapse.Enabled = !this.Collapsed;
            btnExpand.Enabled = this.Collapsed;
        }

        /// <summary>
        /// Gets or sets the color of the header background.
        /// </summary>
        /// <value>The color of the header background.</value>
        [Category("Collapsible Panel Header")] //NOXLATE
        public Color HeaderBackgroundColor
        {
            get { return headerPanel.BackColor; }
            set { headerPanel.BackColor = value; }
        }

        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        /// <value>The header text.</value>
        [Category("Collapsible Panel Header")] //NOXLATE
        [Localizable(true)]
        public string HeaderText
        {
            get { return lblHeaderText.Text; }
            set { lblHeaderText.Text = value; }
        }

        /// <summary>
        /// Gets or sets the header font.
        /// </summary>
        /// <value>The header font.</value>
        [Category("Collapsible Panel Header")] //NOXLATE
        public Font HeaderFont
        {
            get { return lblHeaderText.Font; }
            set { lblHeaderText.Font = value; }
        }

        /// <summary>
        /// Gets or sets the color of the content background.
        /// </summary>
        /// <value>The color of the content background.</value>
        [Category("Collapsible Panel Content")] //NOXLATE
        public Color ContentBackgroundColor
        {
            get { return contentPanel.BackColor; }
            set { contentPanel.BackColor = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can collapse.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can collapse; otherwise, <c>false</c>.
        /// </value>
        [DefaultValue(true)]
        [Category("Collapsible Panel")] //NOXLATE
        public bool CanCollapse
        {
            get { return btnCollapse.Visible && btnExpand.Visible; }
            set { btnCollapse.Visible = btnExpand.Visible = value; }
        }

        private bool _collapsed;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CollapsiblePanel"/> is collapsed.
        /// </summary>
        /// <value><c>true</c> if collapsed; otherwise, <c>false</c>.</value>
        [DefaultValue(false)]
        [Category("Collapsible Panel")] //NOXLATE
        public bool Collapsed
        {
            get
            {
                return _collapsed;
            }
            set
            {
                _collapsed = value;
                if (value)
                {
                    if (contentPanel.Height > 0)
                    {
                        restoreHeight = contentPanel.Height;
                        this.Height -= restoreHeight;
                    }
                }
                else
                {
                    if (contentPanel.Height < restoreHeight)
                    {
                        this.Height += restoreHeight;
                    }
                }
                btnCollapse.Enabled = !_collapsed;
                btnExpand.Enabled = _collapsed;
            }
        }

        private int restoreHeight;

        private void btnCollapse_Click(object sender, EventArgs e)
        {
            this.Collapsed = true;
        }

        private void btnExpand_Click(object sender, EventArgs e)
        {
            this.Collapsed = false;
        }
    }
}
