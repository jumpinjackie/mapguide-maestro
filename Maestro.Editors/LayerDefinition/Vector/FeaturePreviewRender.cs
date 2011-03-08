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
using System.Drawing;
using System.Collections;
using System.Drawing.Imaging;
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using System.Collections.Generic;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Editors.LayerDefinition.Vector
{

	internal class FeaturePreviewRender
	{
		private static ImageStylePicker.NamedImage[] m_fillImages = null;
		private static ImageStylePicker.NamedImage[] m_lineStyles = null;


		public static void RenderPreviewArea(Graphics g, Rectangle size, IAreaSymbolizationFill item)
		{
			if (item == null)
			{
				RenderPreviewFont(g, size, null);
				return;
			}

			//Adjust, since painting always excludes top/right and includes left/bottom
			Rectangle size_adj = new Rectangle(size.X, size.Y, size.Width - 1, size.Height - 1);

			Point[] points = new Point[] 
			{
				new Point(size_adj.Left, size_adj.Top),
				new Point(size_adj.Right, size_adj.Top),
				new Point(size_adj.Right, size_adj.Bottom),
				new Point(size_adj.Left, size_adj.Bottom),
				new Point(size_adj.Left, size_adj.Top)
			};

			if (item.Fill != null)
			{
				Brush b;
			
				Image texture = null;
                foreach (ImageStylePicker.NamedImage img in FillImages)
                {
                    if (img.Name == item.Fill.FillPattern)
                    {
                        //TODO: Figure out why we can't modify the palette...
                        //TODO: When using the transparent png's, it might be possible to paint it usign only the alpha channel mask
                        /*Image bmp = (Image)img.Image.Clone();
                        for(int i = 0; i < bmp.Palette.Entries.Length; i++)
                            if (bmp.Palette.Entries[i].R == Color.Black.R && bmp.Palette.Entries[i].B == Color.Black.B && bmp.Palette.Entries[i].G == Color.Black.G)
                                bmp.Palette.Entries[i] = item.Fill.ForegroundColor;
                            else if (bmp.Palette.Entries[i].R == Color.White.R && bmp.Palette.Entries[i].B == Color.White.B && bmp.Palette.Entries[i].G == Color.White.G)
                                bmp.Palette.Entries[i] = item.Fill.BackgroundColor;
                        */

                        //Until the above is resolved, this is a VERY slow way to do it, even with app. 200 pixels
                        //Unfortunately I don't want "unsafe" code here...
                        Bitmap bmp = new Bitmap(img.Image);
                        for (int y = 0; y < bmp.Height; y++)
                            for (int x = 0; x < bmp.Width; x++)
                            {
                                Color c = bmp.GetPixel(x, y);

                                if (c.A > 0x7F /*&& c.R == Color.Black.R && c.B == Color.Black.B && c.G == Color.Black.G*/)
                                    bmp.SetPixel(x, y, Utility.ParseHTMLColor(item.Fill.ForegroundColor));
                                else //if (c.R == Color.White.R && c.B == Color.White.B && c.G == Color.White.G)
                                    bmp.SetPixel(x, y, Utility.ParseHTMLColor(item.Fill.BackgroundColor));
                            }


                        texture = bmp;
                        break;
                    }
                }
			
				if (texture == null)
					b = new SolidBrush(Utility.ParseHTMLColor(item.Fill.BackgroundColor));
				else
					b = new TextureBrush(texture);
			
				g.FillPolygon(b, points);
				b.Dispose();
			}

			if (item.Stroke != null && !string.IsNullOrEmpty(item.Stroke.Color))
			{
                var c = Utility.ParseHTMLColor(item.Stroke.Color);
                using (Pen p = new Pen(c, /*float.Parse(item.Stroke.Thickness)*/ 1)) //TODO: Calculate appropriate thickness
					g.DrawPolygon(p, points); //TODO: Implement line dash
			}

		}

		public static void RenderPreviewLine(Graphics g, Rectangle size, IEnumerable<IStroke> item)
		{
			if (item == null)
			{
				RenderPreviewFont(g, size, null);
				return;
			}

            try
            {
                //TODO: Implement line dash
                foreach (IStroke st in item)
                {
                    using (Pen p = new Pen(string.IsNullOrEmpty(st.Color) ? Color.White : Utility.ParseHTMLColor(st.Color), /*float.Parse(st.Thickness)*/ 1)) //TODO: Calculate appropriate thickness
                    {
                        g.DrawLine(p, new Point(size.Left, size.Top + (size.Height / 2)), new Point(size.Right, size.Top + (size.Height / 2)));
                    }
                }
            }
            catch
            {
            }
		}

		public static void RenderPreviewFont(Graphics g, Rectangle size, ITextSymbol item)
		{
			Font font;
			Color foreground;
			Color background;
			string text = "";
            BackgroundStyleType bgStyle;
            

			if (item == null || item.FontName == null)
			{
				font = new Font("Arial", 12);
				foreground = Color.Black;
				background = Color.White;
				text = Properties.Resources.EmptyText;
                bgStyle = BackgroundStyleType.Transparent;
			}
			else
			{
				try { font = new Font(item.FontName, 12); }
				catch { font = new Font("Arial", 12); }
				foreground = Utility.ParseHTMLColor(item.ForegroundColor);
				background = Utility.ParseHTMLColor(item.BackgroundColor);
                bgStyle = item.BackgroundStyle;

				FontStyle fs = FontStyle.Regular;
				if (item.Bold == "true")
					fs |= FontStyle.Bold;
				if (item.Italic == "true")
					fs |= FontStyle.Italic;
				if (item.Underlined == "true")
					fs |= FontStyle.Underline;
				font = new Font(font, fs);

				text = item.Text;
			}

            if (bgStyle == BackgroundStyleType.Ghosted)
            {
                using (System.Drawing.Drawing2D.GraphicsPath pth = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    pth.AddString(text, font.FontFamily, (int)font.Style, font.Size * 1.25f, size.Location, StringFormat.GenericDefault);

                    using (Pen p = new Pen(background, 1.5f))
                        g.DrawPath(p, pth);

                    using (Brush b = new SolidBrush(foreground))
                        g.FillPath(b, pth);
                }
            }
            else
            {
                if (bgStyle == BackgroundStyleType.Opaque)
                {
                    SizeF bgSize = g.MeasureString(text, font, new SizeF(size.Width, size.Height));
                    using (Brush b = new SolidBrush(background))
                        g.FillRectangle(b, new Rectangle(size.Top, size.Left, (int)bgSize.Width, (int)bgSize.Height));
                }

                using (Brush b = new SolidBrush(foreground))
                    g.DrawString(text, font, b, size);
            }
		}

		public static void RenderPreviewFontSymbol(Graphics g, Rectangle size, IFontSymbol item)
		{
			Font font;
			Color foreground;
			string text = "";

            if (item == null || item.FontName == null)
            {
                RenderPreviewFont(g, size, null);
                return;
            }
            else
            {
                try { font = new Font(item.FontName, 12); }
                catch { font = new Font("Arial", 12); }

                if (string.IsNullOrEmpty(item.ForegroundColor))
                    foreground = Color.Black;
                else
                    foreground = Utility.ParseHTMLColor(item.ForegroundColor);

                FontStyle fs = FontStyle.Regular;
                if (item.Bold.HasValue && item.Bold.Value)
                    fs |= FontStyle.Bold;
                if (item.Italic.HasValue && item.Italic.Value)
                    fs |= FontStyle.Italic;
                if (item.Underlined.HasValue && item.Underlined.Value)
                    fs |= FontStyle.Underline;
                font = new Font(font, fs);

                text = item.Character;
            }

            SizeF textSize = g.MeasureString(text, font);

            PointF center = new PointF((size.Width - textSize.Width) / 2, (size.Height - textSize.Height) / 2);

			using (Brush b = new SolidBrush(foreground))
				g.DrawString(text, font, b, center);
		}

		public static void RenderPreviewPoint(Graphics g, Rectangle size, IMarkSymbol item)
		{
			if (item == null)
			{
				RenderPreviewFont(g, size, null);
				return;
			}

			//Adjust, since painting always excludes top/right and includes left/bottom
			Rectangle size_adj = new Rectangle(size.X, size.Y, size.Width - 1, size.Height - 1);

			Point[] points = null;

            int radius = Math.Min(size_adj.Width / 2, size_adj.Height / 2);
            int npoints = Math.Min(Math.Max(15, radius / 5), 100);
            Point center = new Point(size_adj.X + size_adj.Width / 2, size_adj.Y + size_adj.Height / 2);

            switch(item.Shape)
			{
                case ShapeType.Square:
                    points = Rotate(CreateNGon(center, radius, 4), center, Math.PI / 4);
                    break;
                case ShapeType.Star:
                    Point[] outerStar = Rotate(CreateNGon(center, radius, 5), center, -Math.PI / 2);
                    Point[] innerStar = Rotate(Rotate(CreateNGon(center, radius / 2, 5), center, -Math.PI / 2), center, Math.PI / 5);
                    points = new Point[outerStar.Length + innerStar.Length];
                    for (int i = 0; i < points.Length; i++)
                        points[i] = i % 2 == 0 ? outerStar[i >> 1] : innerStar[i >> 1];
                    //points = innerStar;
                    break;
                case ShapeType.Triangle:
                    points = Rotate(CreateNGon(center, radius, 3), center, Math.PI / 6);
                    break;
                case ShapeType.Cross:
                case ShapeType.X:
                    Point[] outerCross = Rotate(CreateNGon(center, radius, 4), center, -Math.PI / 2);
                    Point[] innerCross = Rotate(Rotate(CreateNGon(center, radius / 2, 4), center, -Math.PI / 2), center, Math.PI / 4);
                    points = new Point[13];
                    points[0] = new Point(innerCross[0].X, outerCross[0].Y);
                    points[1] = innerCross[0];
                    points[2] = new Point(outerCross[1].X, innerCross[0].Y);
                    points[3] = new Point(outerCross[1].X, innerCross[1].Y);
                    points[4] = innerCross[1];
                    points[5] = new Point(innerCross[1].X, outerCross[2].Y);
                    points[6] = new Point(innerCross[2].X, outerCross[2].Y);
                    points[7] = innerCross[2];
                    points[8] = new Point(outerCross[3].X, innerCross[2].Y);
                    points[9] = new Point(outerCross[3].X, innerCross[3].Y);
                    points[10] = innerCross[3];
                    points[11] = new Point(innerCross[3].X, outerCross[0].Y);
                    points[12] = new Point(innerCross[0].X, outerCross[0].Y);

                    if (item.Shape == ShapeType.X)
                        points = Rotate(points, center, Math.PI / 4);
                    break;
				default: //Circle
				{
                    points = CreateNGon(center, radius, Math.Min(Math.Max(15, radius / 5), 100));
					break;
				}				
			}

			if (item.Fill != null)
			{
				Brush b;
			
				Image texture = null;
                foreach (ImageStylePicker.NamedImage img in FillImages)
                {
                    if (img.Name == item.Fill.FillPattern)
                    {
                        //TODO: Figure out why we can't modify the palette...
                        /*Image bmp = (Image)img.Image.Clone();
                        for(int i = 0; i < bmp.Palette.Entries.Length; i++)
                            if (bmp.Palette.Entries[i].R == Color.Black.R && bmp.Palette.Entries[i].B == Color.Black.B && bmp.Palette.Entries[i].G == Color.Black.G)
                                bmp.Palette.Entries[i] = item.Fill.ForegroundColor;
                            else if (bmp.Palette.Entries[i].R == Color.White.R && bmp.Palette.Entries[i].B == Color.White.B && bmp.Palette.Entries[i].G == Color.White.G)
                                bmp.Palette.Entries[i] = item.Fill.BackgroundColor;
                        */

                        //Until the above is resolved, this is a VERY slow way to do it, even with app. 200 pixels
                        //Unfortunately I don't want "unsafe" code here...
                        Bitmap bmp = new Bitmap(img.Image);
                        for (int y = 0; y < bmp.Height; y++)
                            for (int x = 0; x < bmp.Width; x++)
                            {
                                Color c = bmp.GetPixel(x, y);

                                if (c.R == Color.Black.R && c.B == Color.Black.B && c.G == Color.Black.G)
                                    bmp.SetPixel(x, y, Utility.ParseHTMLColor(item.Fill.ForegroundColor));
                                else if (c.R == Color.White.R && c.B == Color.White.B && c.G == Color.White.G)
                                    bmp.SetPixel(x, y, Utility.ParseHTMLColor(item.Fill.BackgroundColor));
                            }


                        texture = bmp;
                        break;
                    }
                }

				if (texture == null)
					b = new SolidBrush(Utility.ParseHTMLColor(item.Fill.BackgroundColor));
				else
					b = new TextureBrush(texture);
			
				g.FillPolygon(b, points);
				b.Dispose();
			}

			if (item.Edge != null)
			{
				using(Pen p = new Pen(string.IsNullOrEmpty(item.Edge.Color) ? Color.White : Utility.ParseHTMLColor(item.Edge.Color), /* float.Parse(item.Edge.Thickness) */ 1)) //TODO: Calculate appropriate thickness
					g.DrawPolygon(p, points); //TODO: Implement line dash
			}
		
		}

        public static Point[] Rotate(Point[] points, Point center, double radians)
        {
            double sin = Math.Sin(radians);
            double cos = Math.Cos(radians);

            Point[] res = new Point[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                int x = points[i].X - center.X;
                int y = points[i].Y - center.Y;
                res[i] = new Point(
                    center.X + (int)((x * cos) - (y * sin)),
                    center.Y + (int)((y * cos) + (x * sin))
                );
            }
            return res;
        }

        public static Point[] CreateNGon(Point center, int radius, int npoints)
        {
            double interval = (2 * Math.PI) / (double)npoints;

            Point[] points = new Point[npoints + 1];
            for (int i = 0; i < npoints; i++)
            {
                double angle = interval * i;
                points[i] = new Point((int)Math.Round(center.X + Math.Cos(angle) * radius), (int)Math.Round(center.Y + Math.Sin(angle) * radius));
            }

            //Close the polygon
            points[points.Length - 1] = points[0];

            return points;
        }

		public static ImageStylePicker.NamedImage[] FillImages
		{
			get
			{
				if (m_fillImages == null)
					m_fillImages = ReadImagesFromDisk("WebStudio" + System.IO.Path.DirectorySeparatorChar.ToString() + "areas"); //typeof(GeometryStyleEditors.ImageStylePicker).Namespace + ".Fillstyles");
				return m_fillImages;
			}
		}

		public static ImageStylePicker.NamedImage[] LineStyles
		{
			get
			{
				if (m_lineStyles  == null)
					m_lineStyles = ReadImagesFromDisk("WebStudio" + System.IO.Path.DirectorySeparatorChar.ToString() + "lines");  //typeof(GeometryStyleEditors.ImageStylePicker).Namespace + ".Linestyles");
				return m_lineStyles;
			}
		}

		private static ImageStylePicker.NamedImage[] ReadImagesFromDisk(string folderpath)
		{
			string path = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, folderpath);
			if (!System.IO.Directory.Exists(path))
				return new ImageStylePicker.NamedImage[0];

			string[] filenames = System.IO.Directory.GetFiles(path, "*.png");
			ArrayList lst = new ArrayList();
			foreach(string s in filenames)
			{
				Image img = Image.FromFile(s);
				//For some odd reason these have borders around
				if (img.Width == 120 && img.Height == 20)
				{
					Bitmap bmp = new Bitmap(115, 18);
					using (Graphics g = Graphics.FromImage(bmp))
					{
						g.Clear(Color.Transparent);
						g.DrawImage(img, new Rectangle(new Point(0,0), bmp.Size), new Rectangle(2, 1, 115, 18), GraphicsUnit.Pixel);
					}
					img = bmp;
				}

				lst.Add(
					new ImageStylePicker.NamedImage
					(System.IO.Path.GetFileNameWithoutExtension(s), img)
					);
			}
			return (ImageStylePicker.NamedImage[])lst.ToArray(typeof(ImageStylePicker.NamedImage));
		}

		/*private static GeometryStyleEditors.ImageStylePicker.NamedImage[] ReadImages(string assemblypath)
		{
			SortedList lst = new SortedList();
			System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
			string itemnamespace = assemblypath;
			foreach(string s in asm.GetManifestResourceNames())
				if (s.StartsWith(itemnamespace) && s.ToLower().EndsWith(".gif"))
				{
					Image img = Bitmap.FromStream(asm.GetManifestResourceStream(s));
					string imagename = s.Substring(itemnamespace.Length + 1, s.LastIndexOf(".") - itemnamespace.Length - 1); 
					string index = imagename.Substring(0, imagename.IndexOf("."));
					imagename = imagename.Substring(index.Length + 1);

					lst.Add(index, new GeometryStyleEditors.ImageStylePicker.NamedImage(imagename, img));
				}

			GeometryStyleEditors.ImageStylePicker.NamedImage[] retval = new GeometryStyleEditors.ImageStylePicker.NamedImage[lst.Count];
			int i = 0;
			foreach(GeometryStyleEditors.ImageStylePicker.NamedImage nm in lst.Values)
				retval[i++] = nm;

			return retval;
		}*/
	}
}
