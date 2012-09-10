#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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
using System.Windows.Forms;
using System.Drawing;

namespace Maestro.Editors.Common
{
    /// <summary>
    /// A custom combobox to choose image styles
    /// </summary>
	public class ImageStylePicker
		: CustomCombo
	{
		private int m_itemHeight = 20;
		private int m_textWidth = 40;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageStylePicker"/> class.
        /// </summary>
		public ImageStylePicker()
			: base()
		{
			if (!this.DesignMode)
			{
				base.SetCustomRender(new RenderCustomItem(ImageComboRender));
                base.ValueMember = "Name"; //NOXLATE
                base.DisplayMember = "Name"; //NOXLATE
			}
		}

        /// <summary>
        /// Gets or sets the width of the text.
        /// </summary>
        /// <value>The width of the text.</value>
		public int TextWidth
		{
			get { return m_textWidth; }
			set { m_textWidth = value; }
		}

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ComboBox.MeasureItem"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Forms.MeasureItemEventArgs"/> that was raised.</param>
		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			if (e.Index < 0)
				return;

			Image bmp = this.Items[e.Index] as Image;

			if (bmp == null && this.Items[e.Index] as NamedImage != null)
				bmp = (this.Items[e.Index] as NamedImage).Image;

			if (bmp != null)
				e.ItemHeight = m_itemHeight + MARGIN * 3;
			else
				base.OnMeasureItem(e);
		}

		private const int MARGIN = 2;

		private bool ImageComboRender(DrawItemEventArgs e, object value)
		{
			Image bmp;
			string text;

			if (value as NamedImage != null)
			{
				bmp = (value as NamedImage).Image;
				text = (value as NamedImage).Name;
			}
			else if (value as Image != null)
			{
				bmp = value as Image;
				text = null;
			}
			else 
				return false;

			e.DrawBackground();

			Color brushcolor = this.Enabled ? this.ForeColor : System.Drawing.SystemColors.GrayText; 
			int imageWidth = e.Bounds.Width - m_textWidth - MARGIN * 2;

            int startX = e.Bounds.X + MARGIN;
            int startY = e.Bounds.Y + MARGIN;

			//TODO: Apply B/W + dim filter here
            Rectangle r = new Rectangle(startX + 1, startY + 1, imageWidth - 1, m_itemHeight - 1);
            e.Graphics.DrawImageUnscaledAndClipped(bmp, r);

            r = new Rectangle(startX, startY, imageWidth, m_itemHeight);
			using(Pen p = new Pen(brushcolor))
				e.Graphics.DrawRectangle(p, r);

			if (text != null && text.Length > 0)
			{
				StringFormat format = new StringFormat();
				format.Trimming = StringTrimming.EllipsisCharacter;
				format.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.FitBlackBox;

				int textHeight = (int)e.Graphics.MeasureString(text, this.Font, m_textWidth, format).Height;
				int centerY = ((e.Bounds.Height - textHeight) / 2) + e.Bounds.Y;

				r = new Rectangle(e.Bounds.X + MARGIN * 2 + imageWidth, centerY, m_textWidth, m_itemHeight);
				using(Brush b = new SolidBrush(brushcolor))
					e.Graphics.DrawString(text, this.Font, b, r, format);

				if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
					e.DrawFocusRectangle();
			}
			return true;
		}

        /// <summary>
        /// A named image instance
        /// </summary>
		public class NamedImage
		{
            /// <summary>
            /// Initializes a new instance of the <see cref="NamedImage"/> class.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="image">The image.</param>
			public NamedImage(string name, Image image)
			{
				m_name = name;
				m_image = image;
			}

			private Image m_image;
			private string m_name;

            /// <summary>
            /// Gets or sets the image.
            /// </summary>
            /// <value>The image.</value>
			public Image Image
			{
				get { return m_image; }
				set { m_image = value; }
			}

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>The name.</value>
			public string Name 
			{
				get { return m_name; }
				set { m_name = value; }
			}
        }
    }
}
