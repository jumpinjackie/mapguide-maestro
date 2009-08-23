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
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Summary description for PreviewFeatureStyleColumn.
	/// </summary>
	public class PreviewFeatureStyleColumn
		: System.Windows.Forms.DataGridTextBoxColumn
	{
		public PreviewFeatureStyleColumn()
		{
			base.ReadOnly = true;
		}

		protected override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible)
		{

		}


		protected override void Paint(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, CurrencyManager source, int rowNum, System.Drawing.Brush backBrush, System.Drawing.Brush foreBrush, bool alignToRight)
		{
			g.FillRectangle(backBrush, bounds);
			
			DataRowView dr = source.List[rowNum] as DataRowView;
			if (dr == null)
				return;

			Rectangle r = new Rectangle(bounds.Left + 1, bounds.Top + 1, bounds.Width- 2, bounds.Height - 2);

			object item = dr[base.MappingName];
			if (item == null)
			{
				//TODO: Render the default style for point/area/line/font
				return;
			}

			if (item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.PointSymbolization2DType))
			{
				if (((OSGeo.MapGuide.MaestroAPI.PointSymbolization2DType)item).Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.MarkSymbolType))
					FeaturePreviewRender.RenderPreviewPoint(g, r, (OSGeo.MapGuide.MaestroAPI.MarkSymbolType)((OSGeo.MapGuide.MaestroAPI.PointSymbolization2DType)item).Item);
			}
			else if(item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection))
				FeaturePreviewRender.RenderPreviewLine(g, r, (OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection) item);
			else if(item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.AreaSymbolizationFillType))
				FeaturePreviewRender.RenderPreviewArea(g, r, (OSGeo.MapGuide.MaestroAPI.AreaSymbolizationFillType) item);
			else if (item as OSGeo.MapGuide.MaestroAPI.TextSymbolType != null)
				FeaturePreviewRender.RenderPreviewFont(g, r, (OSGeo.MapGuide.MaestroAPI.TextSymbolType) item);
			else
			{
				System.Collections.IList items = null;
				if (item as OSGeo.MapGuide.MaestroAPI.PointTypeStyleType != null)
					items = (item as OSGeo.MapGuide.MaestroAPI.PointTypeStyleType).PointRule;
				else if (item as OSGeo.MapGuide.MaestroAPI.LineTypeStyleType != null)
					items = (item as OSGeo.MapGuide.MaestroAPI.LineTypeStyleType).LineRule;
				else if (item as OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType != null)
					items = (item as OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType).AreaRule;

				if (items != null && items.Count > 0)
				{
					int effectiveItems = Math.Min(5, items.Count);
					int w = (r.Width / effectiveItems);
					Rectangle[] boxes = new Rectangle[effectiveItems];

					for(int i = 0; i < effectiveItems; i++)
						boxes[i] = new Rectangle(r.X + (w * i), r.Y, w - 2, r.Height);

					object[] previewItems = new object[effectiveItems];
					previewItems[0] = items[0];
					previewItems[effectiveItems - 1] = items[items.Count - 1];

					if (effectiveItems == 3)
						previewItems[1] = items[1];
					else if (effectiveItems == 4)
					{
						previewItems[1] = items[1];
						previewItems[2] = items[2];
					}
					else if (effectiveItems > 4)
					{
						previewItems[2] = null;
						previewItems[1] = items[items.Count / 3];
						previewItems[3] = items[(items.Count / 3) * 2];
					}

					for(int i = 0; i < boxes.Length; i++)
					{
						item = previewItems[i];
						if (item == null)
						{
							OSGeo.MapGuide.MaestroAPI.TextSymbolType text = new OSGeo.MapGuide.MaestroAPI.TextSymbolType();
							text.Text = "...";
							text.FontName = "Courier New";
							FeaturePreviewRender.RenderPreviewFont(g, boxes[i], text);
						}
						else if (item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.PointRuleType))
						{
							item = ((OSGeo.MapGuide.MaestroAPI.PointRuleType)item).Item.Item;
							if (item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.MarkSymbolType))
								FeaturePreviewRender.RenderPreviewPoint(g, boxes[i], (OSGeo.MapGuide.MaestroAPI.MarkSymbolType)item);
						}
						else if(item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.LineRuleType))
							FeaturePreviewRender.RenderPreviewLine(g, boxes[i], ((OSGeo.MapGuide.MaestroAPI.LineRuleType)item).Items);
						else if(item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.AreaRuleType))
							FeaturePreviewRender.RenderPreviewArea(g, boxes[i], ((OSGeo.MapGuide.MaestroAPI.AreaRuleType)item).Item);
						else if (item as OSGeo.MapGuide.MaestroAPI.TextSymbolType != null)
							FeaturePreviewRender.RenderPreviewFont(g, boxes[i], (OSGeo.MapGuide.MaestroAPI.TextSymbolType) item);

					}
				}

			}


		}

	}
}
