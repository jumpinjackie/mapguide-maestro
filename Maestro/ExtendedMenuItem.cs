#region Disclaimer / License
// Copyright (C) 2007, Kenneth Skovhede
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

namespace WallpaperChanger
{
	/// <summary>
	/// A menuitem with extra variables for images and fonts
	/// </summary>
	public class ExtendedMenuItem
		: MenuItem
	{
		private Font m_font;
		private Image m_image;
		private Icon m_icon;

		/// <summary>
		/// Spacing constant for space between text and icon
		/// </summary>
		private const int ICON_SPACING = 0;
		/// <summary>
		/// Constant for spacing at the left and right edge of the menu item
		/// </summary>
		private const int MENU_SPACING_WIDTH = 2;
		/// <summary>
		/// Constant for spacing at the top and bottom of the menu item
		/// </summary>
		private const int MENU_SPACING_HEIGHT = 2;

		/// <summary>
		/// Basic default constructor
		/// </summary>
		public ExtendedMenuItem()
			: base()
		{
			m_font = SystemInformation.MenuFont;
			base.OwnerDraw = true;
		}

		/// <summary>
		/// Specialized constructor
		/// </summary>
		/// <param name="text">The text of the item</param>
		/// <param name="clickhandler">The delegate that is called when the menu is clicked</param>
		public ExtendedMenuItem(string text, System.EventHandler clickhandler)
			: base (text, clickhandler)
		{
			m_font = SystemInformation.MenuFont;
			base.OwnerDraw = true;
		}

		/// <summary>
		/// Gets or sets the font of the menu
		/// </summary>
		public Font Font
		{
			get { return m_font; }
			set { m_font = value; }
		}

		/// <summary>
		/// Gets or sets the image for the item. The image overrides any icon
		/// </summary>
		public Image Image
		{
			get { return m_image; }
			set { m_image = value; }
		}

		/// <summary>
		/// Gets or sets the icon for the item. The image overrides any icon
		/// </summary>
		public Icon Icon
		{
			get { return m_icon; }
			set { m_icon = value; }
		}

		/// <summary>
		/// Internal handler that ensures the menuitem is big enough to hold text and image/icon
		/// </summary>
		/// <param name="e">An object with measuring information</param>
		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			
			//Get the size of the string, when using the current font
			System.Drawing.StringFormat sf = new System.Drawing.StringFormat();
			sf.Alignment = System.Drawing.StringAlignment.Near;
			sf.FormatFlags = System.Drawing.StringFormatFlags.NoWrap;
			sf.Trimming = System.Drawing.StringTrimming.Character;
			System.Drawing.SizeF size = e.Graphics.MeasureString(base.Text, this.Font, new System.Drawing.Point(0,0), sf);

			//TODO: Calculate the max width of all the images in this menu, and adjust the text according
			//If we have an image or an icon, allocate space for that as well
			if (m_image != null)
			{
				size.Width += m_image.Width + ICON_SPACING;
				size.Height = Math.Max(size.Height, m_image.Height);
			}
			else if (m_icon != null)
			{
				size.Width += m_icon.Width + ICON_SPACING;
				size.Height = Math.Max(size.Height, m_icon.Height);
			}
			//Add spacing to the return values
			e.ItemWidth = (int)size.Width + (MENU_SPACING_WIDTH * 2);
			e.ItemHeight = (int)size.Height + (MENU_SPACING_HEIGHT * 2);
		}

		/// <summary>
		/// Internal handler that draws the menuitem
		/// </summary>
		/// <param name="e">An object with various drawing helper methods and objects</param>
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			//Select the current background color and foreground color, based on the system colors
			System.Drawing.Color textColor = (e.State & DrawItemState.Selected) == 0 ? System.Drawing.SystemColors.MenuText : System.Drawing.SystemColors.HighlightText;
			System.Drawing.Color backColor = (e.State & DrawItemState.Selected) == 0 ? System.Drawing.SystemColors.Menu : System.Drawing.SystemColors.Highlight;

			//This call is broken for some reason...
			//e.DrawBackground();
			//So we emulate it the right way
            using(System.Drawing.Pen p = new System.Drawing.Pen(backColor))
				e.Graphics.DrawRectangle(p, e.Bounds);
			using(System.Drawing.SolidBrush b = new System.Drawing.SolidBrush(backColor))
				e.Graphics.FillRectangle(b, e.Bounds);

			//Have not seen this effect being active...
			if ((e.State & DrawItemState.Focus) != 0)
				e.DrawFocusRectangle();

			//Setup the string format and drawing area
			System.Drawing.StringFormat sf = new System.Drawing.StringFormat();
			sf.Alignment = System.Drawing.StringAlignment.Near;
			sf.FormatFlags = System.Drawing.StringFormatFlags.NoWrap;
			sf.Trimming = System.Drawing.StringTrimming.Character;
			System.Drawing.Rectangle r = new System.Drawing.Rectangle(e.Bounds.Location, e.Bounds.Size);
			
			//Adjust with some spacing top and left
			r.X += MENU_SPACING_WIDTH;
			r.Y += MENU_SPACING_HEIGHT;

			//TODO: Calculate the max width of all the images in this menu, and adjust the text according
			//If we have an image or an icon, draw it, and ajust the string rectangle
			if (m_image != null)
			{
				e.Graphics.DrawImage(m_image, r.X, r.Y);
				r.X += m_image.Width + ICON_SPACING;
				r.Width -= m_image.Width + ICON_SPACING;
			}
			else if (m_icon != null)
			{
				e.Graphics.DrawIcon(m_icon, r.X, r.Y);
				r.X += m_icon.Width + ICON_SPACING;
				r.Width -= m_icon.Width + ICON_SPACING;
			}

			//Place the string in the middle of the box, as opposed to the top of the box
			System.Drawing.SizeF sh = e.Graphics.MeasureString(base.Text, this.Font, new System.Drawing.Point(0,0), sf);
			r.Y += ((int)((e.Bounds.Height - sh.Height) / 2)) - 1;

			//Draw the current text
			using(System.Drawing.SolidBrush b = new System.Drawing.SolidBrush(textColor))
				e.Graphics.DrawString(base.Text, e.Font, b, r, sf);
		}


	}
}
