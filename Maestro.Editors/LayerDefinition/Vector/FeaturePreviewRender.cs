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
using Maestro.Editors.LayerDefinition.Vector.StyleEditors;

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
                Brush b = null;
            
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
                        Color? bg = null;
                        Color? fg = null;
                        try
                        {
                            bg = Utility.ParseHTMLColor(item.Fill.BackgroundColor);
                            fg = Utility.ParseHTMLColor(item.Fill.ForegroundColor);
                        }
                        catch { }

                        Bitmap bmp = new Bitmap(img.Image);
                        for (int y = 0; y < bmp.Height; y++)
                            for (int x = 0; x < bmp.Width; x++)
                            {
                                Color c = bmp.GetPixel(x, y);

                                if (c.A > 0x7F && fg.HasValue /*&& c.R == Color.Black.R && c.B == Color.Black.B && c.G == Color.Black.G*/)
                                    bmp.SetPixel(x, y, fg.Value);
                                else if (bg.HasValue)//if (c.R == Color.White.R && c.B == Color.White.B && c.G == Color.White.G)
                                    bmp.SetPixel(x, y, bg.Value);
                            }


                        texture = bmp;
                        break;
                    }
                }

                Color? bgColor = null;
                try
                {
                    bgColor = Utility.ParseHTMLColor(item.Fill.BackgroundColor);
                }
                catch { }
                if (texture == null && bgColor.HasValue)
                    b = new SolidBrush(bgColor.Value);
                else if (texture != null)
                    b = new TextureBrush(texture);

                if (b != null)
                {
                    g.FillPolygon(b, points);
                    b.Dispose();
                }
            }

            if (item.Stroke != null && !string.IsNullOrEmpty(item.Stroke.Color))
            {
                Color? c = null;
                try
                {
                    c = Utility.ParseHTMLColor(item.Stroke.Color);
                }
                catch { }
                if (c.HasValue)
                {
                    using (Pen p = new Pen(c.Value, /*float.Parse(item.Stroke.Thickness)*/ 1)) //TODO: Calculate appropriate thickness
                        g.DrawPolygon(p, points); //TODO: Implement line dash
                }
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
                    Color? c = null;
                    try
                    {
                        c = string.IsNullOrEmpty(st.Color) ? Color.White : Utility.ParseHTMLColor(st.Color);
                    }
                    catch { }

                    if (c.HasValue)
                    {
                        using (Pen p = new Pen(c.Value, /*float.Parse(st.Thickness)*/ 1)) //TODO: Calculate appropriate thickness
                        {
                            g.DrawLine(p, new Point(size.Left, size.Top + (size.Height / 2)), new Point(size.Right, size.Top + (size.Height / 2)));
                        }
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
            Color? foreground = null;
            Color? background = null;
            string text = string.Empty;
            BackgroundStyleType bgStyle;
            

            if (item == null || item.FontName == null)
            {
                font = new Font("Arial", 12); //NOXLATE
                foreground = Color.Black;
                background = Color.White;
                text = Strings.EmptyText;
                bgStyle = BackgroundStyleType.Transparent;
            }
            else
            {
                try { font = new Font(item.FontName, 12); }
                catch { font = new Font("Arial", 12); } //NOXLATE
                try
                {
                    foreground = Utility.ParseHTMLColor(item.ForegroundColor);
                    background = Utility.ParseHTMLColor(item.BackgroundColor);
                }
                catch { }
                bgStyle = item.BackgroundStyle;

                FontStyle fs = FontStyle.Regular;
                if (item.Bold == "true") //NOXLATE
                    fs |= FontStyle.Bold;
                if (item.Italic == "true") //NOXLATE
                    fs |= FontStyle.Italic;
                if (item.Underlined == "true") //NOXLATE
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

                    if (background.HasValue)
                    {
                        using (Pen p = new Pen(background.Value, 1.5f))
                            g.DrawPath(p, pth);
                    }

                    if (foreground.HasValue)
                    {
                        using (Brush b = new SolidBrush(foreground.Value))
                            g.FillPath(b, pth);
                    }
                }
            }
            else
            {
                if (bgStyle == BackgroundStyleType.Opaque)
                {
                    SizeF bgSize = g.MeasureString(text, font, new SizeF(size.Width, size.Height));
                    if (background.HasValue)
                    {
                        using (Brush b = new SolidBrush(background.Value))
                            g.FillRectangle(b, new Rectangle(size.Top, size.Left, (int)bgSize.Width, (int)bgSize.Height));
                    }
                }

                if (foreground.HasValue)
                {
                    using (Brush b = new SolidBrush(foreground.Value))
                        g.DrawString(text, font, b, size);
                }
            }
        }

        public static void RenderPreviewFontSymbol(Graphics g, Rectangle size, IFontSymbol item)
        {
            Font font;
            Color? foreground = null;
            string text = string.Empty;

            if (item == null || item.FontName == null)
            {
                RenderPreviewFont(g, size, null);
                return;
            }
            else
            {
                try { font = new Font(item.FontName, 12); }
                catch { font = new Font("Arial", 12); } //NOXLATE

                try
                {
                    if (string.IsNullOrEmpty(item.ForegroundColor))
                        foreground = Color.Black;
                    else
                        foreground = Utility.ParseHTMLColor(item.ForegroundColor);
                }
                catch { }

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

            if (foreground.HasValue)
            {
                using (Brush b = new SolidBrush(foreground.Value))
                    g.DrawString(text, font, b, center);
            }
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

                Color? bgColor = null;
                Color? fgColor = null;

                try
                {
                    bgColor = Utility.ParseHTMLColor(item.Fill.BackgroundColor);
                    fgColor = Utility.ParseHTMLColor(item.Fill.ForegroundColor);
                }
                catch { }

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

                                if (c.R == Color.Black.R && c.B == Color.Black.B && c.G == Color.Black.G && fgColor.HasValue)
                                    bmp.SetPixel(x, y, fgColor.Value);
                                else if (c.R == Color.White.R && c.B == Color.White.B && c.G == Color.White.G && bgColor.HasValue)
                                    bmp.SetPixel(x, y, bgColor.Value);
                            }


                        texture = bmp;
                        break;
                    }
                }

                if (texture == null && bgColor.HasValue)
                    b = new SolidBrush(bgColor.Value);
                else
                    b = new TextureBrush(texture);

                if (b != null)
                {
                    g.FillPolygon(b, points);
                    b.Dispose();
                }
            }

            if (item.Edge != null)
            {
                Color? c = null;
                try
                {
                    c = string.IsNullOrEmpty(item.Edge.Color) ? Color.White : Utility.ParseHTMLColor(item.Edge.Color);
                }
                catch { }

                if (c.HasValue)
                {
                    using (Pen p = new Pen(c.Value, /* float.Parse(item.Edge.Thickness) */ 1)) //TODO: Calculate appropriate thickness
                        g.DrawPolygon(p, points); //TODO: Implement line dash
                }
            }
        }

        public static void RenderW2DImage(Graphics graphics, Rectangle rectangle, IW2DSymbol symb, Image image)
        {
            //TODO: This will currently just draw the default W2D image, it will not consider color overrides nor size parameters
            //But something is better than nothing at the moment.
            
            if (image != null)
            {
                //Start from center
                var location = new Point(rectangle.Width / 2, rectangle.Height / 2);

                //Displace according to image size
                location.X -= image.Width / 2;
                location.Y -= image.Height / 2;

                //Draw the result. 
                graphics.DrawImage(image, location);
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
                    m_fillImages = StyleImageCache.FillImages;
                return m_fillImages;
            }
        }

        public static ImageStylePicker.NamedImage[] LineStyles
        {
            get
            {
                if (m_lineStyles == null)
                    m_lineStyles = StyleImageCache.LineStyles;
                return m_lineStyles;
            }
        }

        internal static void RenderNoPreview(Graphics graphics, Rectangle rect)
        {
            
        }
    }
}
