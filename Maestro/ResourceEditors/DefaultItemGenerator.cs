#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.MaestroAPI;
using System.Drawing;

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
    /// <summary>
    /// This class creates new items, such as Point, Line and Area.
    /// By collecting all code releated to creation here, all functions behave 
    /// consistently when creating a new element.
    /// </summary>
    public static class DefaultItemGenerator
    {
        public static TextSymbolType CreateTextSymbolType()
        {
            TextSymbolType item = new TextSymbolType();
            item.AdvancedPlacement = null;
            item.BackgroundColor = Color.White;
            item.BackgroundStyle = BackgroundStyleType.Transparent;
            item.Bold = "false";
            item.FontName = "Arial";
            item.ForegroundColor = Color.Black;
            item.HorizontalAlignment = "'Center'";
            item.InsertionPointXSpecified = false;
            item.InsertionPointYSpecified = false;
            item.Italic = "false";
            item.MaintainAspect = true;
            item.MaintainAspectSpecified = true;
            item.Rotation = "0";
            item.SizeContext = SizeContextType.DeviceUnits;
            item.SizeX = "10";
            item.SizeY = "10";
            item.Text = "";
            item.Underlined = "false";
            item.Unit = LengthUnitType.Points;
            item.VerticalAlignment = "'Baseline'";

            return item;
        }

        public static PointRuleType CreatePointRuleType()
        {
            PointRuleType prt = new PointRuleType();
            prt.Item = new PointSymbolization2DType();
            MarkSymbolType mks = new MarkSymbolType();

            mks.Edge = CreateStrokeType();
            mks.Fill = CreateFillType();

            mks.SizeContext = SizeContextType.DeviceUnits;
            mks.InsertionPointXSpecified = false;
            mks.InsertionPointYSpecified = false;
            mks.MaintainAspect = true;
            mks.MaintainAspectSpecified = true;
            mks.Rotation = "0";
            mks.SizeX = "10";
            mks.SizeY = "10";
            mks.Unit = LengthUnitType.Points;

            prt.Item.Item = mks;

            return prt;
        }

        public static LineRuleType CreateLineRuleType()
        {
            LineRuleType lrt = new LineRuleType();
            lrt.Items = new StrokeTypeCollection();
            lrt.Items.Add(CreateStrokeType());

            return lrt;
        }

        public static AreaRuleType CreateAreaRuleType()
        {
            AreaRuleType art = new AreaRuleType();
            art.Item = new AreaSymbolizationFillType();
            art.Item.Fill = CreateFillType();
            art.Item.Stroke = CreateStrokeType();
            return art;
        }

        public static FillType CreateFillType()
        {
            FillType item = new FillType();
            item.BackgroundColor = Color.White;
            item.ForegroundColor = Color.Black;
            item.FillPattern = "Solid";
            return item;
        }

        public static StrokeType CreateStrokeType()
        {
            StrokeType item = new StrokeType();
            item.Color = Color.Black;
            item.LineStyle = "Solid";
            item.SizeContext = SizeContextType.DeviceUnits;
            item.Thickness = "1";
            item.Unit = LengthUnitType.Points;
            return item;
        }


        public static VectorScaleRangeType CreateVectorScaleRangeType()
        {
            VectorScaleRangeType item = new VectorScaleRangeType();
            item.Items = new System.Collections.ArrayList();
            PointTypeStyleType pst = new PointTypeStyleType();
            LineTypeStyleType lst = new LineTypeStyleType();
            AreaTypeStyleType ast = new AreaTypeStyleType();

            pst.PointRule = new PointRuleTypeCollection();
            lst.LineRule = new LineRuleTypeCollection();
            ast.AreaRule = new AreaRuleTypeCollection();

            pst.PointRule.Add(CreatePointRuleType());
            lst.LineRule.Add(CreateLineRuleType());
            ast.AreaRule.Add(CreateAreaRuleType());

            item.Items.AddRange(new object[] { pst, lst, ast });

            return item;
        }
    }
}
