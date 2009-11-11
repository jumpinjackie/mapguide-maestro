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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
    public partial class ThemeCreator : Form
    {
        private const int PREVIEW_ITEM_BOX_WIDTH = 20;
        private const int PREVIEW_ITEM_BOX_SPACING = 10;

        private static List<ColorBrewer> m_colorBrewer;

        private EditorInterface m_editor;
        private MaestroAPI.LayerDefinition m_layer;
        private MaestroAPI.FeatureSourceDescription.FeatureSourceSchema m_schema;
        private Dictionary<object, long> m_values;
        private Type m_dataType;
        
        private object m_ruleCollection;
        private object m_defaultItem;

        private static readonly Type[] NUMERIC_TYPES = null;

        private bool m_isUpdating = false;

        private class RuleItem
        {
            public string Label;
            public string Filter;
            public Color Color;

            public RuleItem(string Filter, string Label, Color Color)
            {
                this.Filter = Filter;
                this.Label = Label;
                this.Color = Color;
            }
        }

        static ThemeCreator ()
        {
            NUMERIC_TYPES = new Type[] { typeof(byte), typeof(int), typeof(float), typeof(double) };
        }

        public ThemeCreator(EditorInterface editor, MaestroAPI.LayerDefinition layer, MaestroAPI.FeatureSourceDescription.FeatureSourceSchema schema, object ruleCollection)
            : this()
        {
            m_editor = editor;
            m_layer = layer;
            m_schema = schema;
            m_ruleCollection = ruleCollection;

            //TODO: Would be nice if the user could specify the default styles
            PrepareDefaultItem();

            ColorBrewerColorSet.SetCustomRender(new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.CustomCombo.RenderCustomItem(DrawColorSetPreview));
        }

        private bool DrawColorSetPreview(DrawItemEventArgs args, object o)
        {
            if (o is ColorBrewer.ColorBrewerListItem)
            {
                ColorBrewer cb = (o as ColorBrewer.ColorBrewerListItem).Set;
                int maxItems = (args.Bounds.Width - 2) / 10;
                int items = Math.Min(maxItems, cb.Colors.Count);

                //Make odd number if there are too many
                if (maxItems < cb.Colors.Count && items % 2 == 0)
                    items--;

                int itemWidth = args.Bounds.Width / items;
                int leftOffset = (args.Bounds.Width - (itemWidth * items)) / 2;

                args.DrawBackground();

                for (int i = 0; i < items; i++)
                {
                    Rectangle area = new Rectangle(args.Bounds.Left + leftOffset + (i * itemWidth), args.Bounds.Top + 1, itemWidth, args.Bounds.Height - 3); ;

                    if (maxItems < cb.Colors.Count && i == items / 2)
                    {
                        RectangleF areaF = new RectangleF(area.X, area.Y, area.Width, area.Height);
                        System.Drawing.StringFormat sf = new StringFormat();
                        sf.Alignment = StringAlignment.Center;
                        using (Brush b = new SolidBrush(ColorBrewerColorSet.ForeColor))
                            args.Graphics.DrawString("...", ColorBrewerColorSet.Font, b, areaF, sf);
                    }
                    else
                    {
                        Color c = i < (items / 2) ? cb.Colors[i] : cb.Colors[cb.Colors.Count - (items - i)];
                        using (Brush b = new SolidBrush(c))
                            args.Graphics.FillRectangle(b, area);
                        args.Graphics.DrawRectangle(Pens.Black, area);
                    }

                }

                if (args.State == DrawItemState.Selected)
                    args.DrawFocusRectangle();

                return true;
            }
            else
                return false;
        }


        private ThemeCreator()
        {
            InitializeComponent();

            GradientFromColor.ResetColors();
            GradientToColor.ResetColors();

            if (m_colorBrewer == null)
                m_colorBrewer = ColorBrewer.ParseCSV(System.IO.Path.Combine(Application.StartupPath, "ColorBrewer.csv"));
        }

        private void ThemeCreator_Load(object sender, EventArgs e)
        {
            GradientFromColor.CurrentColor = Color.Red;
            GradientToColor.CurrentColor = Color.Green;

            ColumnCombo.Items.Clear();
            ColumnCombo.Items.Add("<Select column>");
            ColumnCombo.SelectedIndex = 0;

            foreach (MaestroAPI.FeatureSetColumn col in m_schema.Columns)
                if (col.Type == typeof(string) || Array.IndexOf<Type>(NUMERIC_TYPES, col.Type) >= 0) 
                    ColumnCombo.Items.Add(col.Name);
        }

        private void ColumnCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ColumnCombo.SelectedIndex == 0)
            {
                //Dummy item selected, just disable the form
                RuleCountPanel.Enabled =
                GroupPanel.Enabled =
                DisplayGroup.Enabled =
                PreviewGroup.Enabled =
                OKBtn.Enabled =
                    false;

            }
            else
            {
                DisplayGroup.Enabled =
                PreviewGroup.Enabled =
                OKBtn.Enabled =
                    true;

                m_values = new Dictionary<object, long>();

                MaestroAPI.FeatureSetColumn col = null;

                foreach (MaestroAPI.FeatureSetColumn c in m_schema.Columns)
                    if (c.Name == ColumnCombo.Text)
                    {
                        col = c;
                        break;
                    }

                //Not really possible
                if (col == null)
                    throw new Exception("Invalid column name");

                string filter = null; //Attempt raw reading initially
                Exception rawEx = null; //Original exception
                bool retry = true;

                while (retry)
                {
                    retry = false;
                    try
                    {
                        using (MaestroAPI.FeatureSetReader rd = m_editor.CurrentConnection.QueryFeatureSource(m_layer.Item.ResourceId, m_schema.Fullname, filter, new string[] { col.Name }))
                            while (rd.Read() && m_values.Count < 100000) //No more than 100.000 records in memory
                                if (!rd.Row.IsValueNull(col.Name))
                                {
                                    object value = rd.Row[col.Name];
                                    if (!m_values.ContainsKey(value))
                                        m_values.Add(value, 0);

                                    m_values[value]++;
                                }

                        rawEx = null; //Clear error

                    }
                    catch (Exception ex)
                    {
                        rawEx = ex;
                        if (filter == null && ex.Message.IndexOf("MgNullPropertyValueException") >= 0) //Known issue
                        {
                            retry = true;
                            filter = col.Name + " != NULL";
                        }
                    }
                }

                if (rawEx != null)
                {
                    MessageBox.Show(this, string.Format("Unable to read data from the selected column: {0}", rawEx.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ColumnCombo.SelectedIndex = 0;
                    return;
                }

                if (m_values.Count == 0)
                {
                    MessageBox.Show(this,"The selected column had no non-null values and cannot be used.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ColumnCombo.SelectedIndex = 0;
                    return;
                }

                m_dataType = col.Type;

                if (Array.IndexOf<Type>(NUMERIC_TYPES, col.Type) >= 0)
                {
                    if (m_values.Count >= 100000)
                        MessageBox.Show(this, "The selected column contains more than 100000 different values.\r\nThe calculated averages only accounts for the first 100000 distinct values.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    GroupPanel.Enabled = true;
                    RuleCountPanel.Enabled = true;
                    RuleCount.Minimum = 3;

                    if (m_values.Count <= 9)
                    {
                        AggregateCombo.SelectedIndex = AggregateCombo.Items.Count - 1;
                        AggregateCombo_SelectedIndexChanged(sender, e);

                        RefreshColorBrewerSet();
                    }
                    else
                    {
                        RuleCount.Value = 6;
                        if (!GradientColors.Checked)
                            ColorBrewerColors.Checked = true;

                        if (AggregateCombo.SelectedIndex < 0)
                            AggregateCombo.SelectedIndex = 0;
                        if (AggregateCombo.SelectedIndex == AggregateCombo.Items.Count - 1)
                            AggregateCombo.SelectedIndex = 0;

                        RefreshColorBrewerSet();
                    }

                }
                else //String type
                {
                    if (m_values.Count > 100)
                    {
                        MessageBox.Show(this, "The selected column contains more than 100 different values, and thus cannot be used for theming", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ColumnCombo.SelectedIndex = 0;
                        return;
                    }

                    RuleCountPanel.Enabled = 
                    GroupPanel.Enabled = 
                        false;

                    RuleCount.Minimum = 0;

                    //Select "Individual"
                    AggregateCombo.SelectedIndex = AggregateCombo.Items.Count - 1;
                    RuleCount.Value = m_values.Count;
                    GradientColors.Checked = true;

                    DisplayGroup.Enabled =
                    PreviewGroup.Enabled =
                    OKBtn.Enabled =
                        true;
                }

                RefreshPreview();
            }
        }

        private void RefreshColorBrewerSet()
        {
            try
            {
                m_isUpdating = true;
                string prevType = ColorBrewerDataType.Text;
                ColorBrewerDataType.Items.Clear();

                foreach (ColorBrewer cb in m_colorBrewer)
                    if (cb.Colors.Count == RuleCount.Value)
                        if (ColorBrewerDataType.FindString(cb.DisplayType) < 0)
                            ColorBrewerDataType.Items.Add(cb.DisplayType);

                if (ColorBrewerDataType.FindString(prevType) >= 0)
                    ColorBrewerDataType.SelectedIndex = ColorBrewerDataType.FindString(prevType);
                else if (ColorBrewerDataType.Items.Count > 0)
                    ColorBrewerDataType.SelectedIndex = 0;

                if (ColorBrewerDataType.Items.Count == 0)
                {
                    GradientColors.Checked = true;
                    ColorBrewerColors.Enabled = ColorBrewerLabel.Enabled = ColorBrewerPanel.Enabled = false;
                }
                else
                {
                    ColorBrewerColors.Enabled = ColorBrewerLabel.Enabled = ColorBrewerPanel.Enabled = true;
                    if (!GradientColors.Checked)
                        ColorBrewerColors.Checked = true;
                }
            }
            finally
            {
                m_isUpdating = false;
            }
        }

        private List<RuleItem> CalculateRuleSet()
        {
            Color[] colors = BuildColorSet(false);
            List<RuleItem> result = new List<RuleItem>();

            if (AggregateCombo.SelectedIndex == 0 || AggregateCombo.SelectedIndex == 1 || AggregateCombo.SelectedIndex == 2)
            {
                double min = double.MaxValue;
                double max = double.MinValue;
                double mean = 0;
                long count = 0;
                foreach (KeyValuePair<object, long> entry in m_values)
                {
                    double value = Convert.ToDouble(entry.Key);
                    min = Math.Min(value, min);
                    max = Math.Max(value, max);
                    mean += value * entry.Value;
                    count += entry.Value;
                }

                mean /= count;

                if (AggregateCombo.SelectedIndex == 0) //Equal
                {
                    double chunksize = (max - min) / colors.Length;
                    result.Add(new RuleItem(
                        string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" <= {1}", ColumnCombo.Text, FormatValue(chunksize + min)),
                        string.Format(System.Globalization.CultureInfo.InvariantCulture, "Less than {0}", FormatValue(chunksize + min)), 
                        colors[0]));

                    for (int i = 1; i < colors.Length - 1; i++)
                        result.Add(new RuleItem(
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" > {1} AND \"{0}\" <= {2}", ColumnCombo.Text, FormatValue(min + (i * chunksize)), FormatValue(min + ((i + 1) * chunksize))),
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, "Between {0} and {1}", FormatValue(min + (i * chunksize)), FormatValue(min + ((i + 1) * chunksize))),
                            colors[i]));

                    result.Add(new RuleItem(
                        string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" > {1}", ColumnCombo.Text, FormatValue(max - chunksize)),
                        string.Format(System.Globalization.CultureInfo.InvariantCulture, "More than {0}", FormatValue(max - chunksize)), 
                        colors[colors.Length - 1]));
                }
                else if (AggregateCombo.SelectedIndex == 1) //Standard Deviation
                {
                    double dev = 0;
                    foreach (KeyValuePair<object, long> entry in m_values)
                        dev += ((Convert.ToDouble(entry.Key) - mean) * (Convert.ToDouble(entry.Key) - mean)) * entry.Value;

                    dev /= count;
                    dev = Math.Sqrt(dev);

                    double span = (dev * (colors.Length / 2));
                    double lower = mean < span ? span - mean : mean - span;

                    if (colors.Length % 2 == 1)
                        lower += dev / 2; //The middle item goes half an alpha to each side

                    result.Add(new RuleItem(
                        string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" < {1}", ColumnCombo.Text, FormatValue(lower + dev)),
                        string.Format(System.Globalization.CultureInfo.InvariantCulture, "Less than {0}", FormatValue(lower + dev)),
                        colors[0]));

                    for (int i = 1; i < colors.Length - 1; i++)
                        result.Add(new RuleItem(
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" >= {1} AND \"{0}\" < {2}", ColumnCombo.Text, FormatValue(lower + (i * dev)), FormatValue(lower + ((i + 1) * dev))),
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, "Between {0} and {1}", FormatValue(lower + (i * dev)), FormatValue(lower + ((i + 1) * dev))),
                            colors[i]));

                    result.Add(new RuleItem(
                        string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" >= {1}", ColumnCombo.Text, FormatValue(lower + (dev * (colors.Length - 1)))),
                        string.Format(System.Globalization.CultureInfo.InvariantCulture, "More than {0}", FormatValue(lower + (dev * (colors.Length - 1)))),
                        colors[colors.Length - 1]));

                }
                else if (AggregateCombo.SelectedIndex == 2) //Quantile
                {
                    SortedDictionary<double, long> sort = new SortedDictionary<double, long>();
                    foreach (KeyValuePair<object, long> entry in m_values)
                        sort.Add(Convert.ToDouble(entry.Key), entry.Value);

                    double step = (1.0 / colors.Length) * count;
                    List<double> separators = new List<double>();
                    for(int i = 1; i < colors.Length; i++)
                    {
                        long limit = (long)Math.Round(step * i);
                        long cc = 0;
                        double item = double.NaN;

                        foreach(KeyValuePair<double, long> entry in sort)
                        {
                            item = entry.Key;
                            cc += entry.Value;
                            if (cc >= limit)
                                break;
                        }

                        separators.Add(item);
                    }

                    result.Add(new RuleItem(
                        string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" <= {1}", ColumnCombo.Text, FormatValue(separators[0])),
                        string.Format(System.Globalization.CultureInfo.InvariantCulture, "Less than {0}", FormatValue(separators[0])),
                        colors[0]));

                    for (int i = 1; i < colors.Length - 1; i++)
                        result.Add(new RuleItem(
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" > {1} AND \"{0}\" <= {2}", ColumnCombo.Text, FormatValue(separators[i - 1]), FormatValue(separators[i])),
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, "Between {0} and {1}", FormatValue(separators[i - 1]), FormatValue(separators[i])),
                            colors[i]));

                    result.Add(new RuleItem(
                        string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" > {1}", ColumnCombo.Text, FormatValue(separators[separators.Count - 1])),
                        string.Format(System.Globalization.CultureInfo.InvariantCulture, "More than {0}", FormatValue(separators[separators.Count - 1])),
                        colors[colors.Length - 1]));
                }
            }
            else if (AggregateCombo.SelectedIndex == 3) //Individual
            {
                List<object> items = new List<object>(m_values.Keys);
                items.Sort(); //Handles types correctly

                for (int i = 0; i < colors.Length; i++)
                    if (items[i] is string)
                        result.Add(new RuleItem(
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" = '{1}'", ColumnCombo.Text, items[i]),
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", items[i]), 
                            colors[i]));
                    else
                        result.Add(new RuleItem(
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" = {1}", ColumnCombo.Text, items[i]),
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", items[i]), 
                            colors[i])); 

            }


            return result;
        }

        private string FormatValue(double value)
        {
            if (m_dataType == typeof(double) || m_dataType == typeof(float))
                return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", value);
            else
                return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", (long)Math.Round(value));
        }

        /// <summary>
        /// Redraws the preview image
        /// </summary>
        private void RefreshPreview()
        {
            //TODO: More efficitent to use the "onpaint" event
            Bitmap bmp = new Bitmap(PreviewPicture.Width, PreviewPicture.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {

                List<Color?> colors = new List<Color?>();
                Color[] actualColors = BuildColorSet(true);
                if (actualColors.Length > 0)
                {

                    int num_boxes = (bmp.Width - PREVIEW_ITEM_BOX_SPACING) / (PREVIEW_ITEM_BOX_WIDTH + PREVIEW_ITEM_BOX_SPACING);
                    if (actualColors.Length > num_boxes)
                    {
                        if (num_boxes % 2 == 0)
                            num_boxes--;

                        int real = ((num_boxes - 1) / 2);
                        for (int i = 0; i < actualColors.Length; i++)
                            if (i < real || (actualColors.Length - i) <= real)
                                colors.Add(actualColors[i]);

                        colors.Insert(real, null);
                    }
                    else
                    {
                        for (int i = 0; i < actualColors.Length; i++)
                            colors.Add(actualColors[i]);
                        num_boxes = colors.Count;
                    }

                    int spacing = ((bmp.Width - PREVIEW_ITEM_BOX_SPACING) - (PREVIEW_ITEM_BOX_WIDTH * num_boxes)) / num_boxes;
                    int fullwidth = (spacing * (num_boxes - 1)) + (PREVIEW_ITEM_BOX_WIDTH * num_boxes);
                    int x = (bmp.Width - fullwidth) / 2;

                    int topy = bmp.Height / 4;
                    int height = topy * 2;


                    foreach (Color? c in colors)
                    {
                        if (!c.HasValue)
                            g.DrawString("...", new Font(FontFamily.GenericSansSerif, 12), Brushes.Black, x, topy);
                        else
                        {
                            g.DrawRectangle(Pens.Black, x, topy, PREVIEW_ITEM_BOX_WIDTH, height);
                            using (Brush b = new SolidBrush(c.Value))
                                g.FillRectangle(b, x, topy, PREVIEW_ITEM_BOX_WIDTH, height);
                        }
                        x += spacing + PREVIEW_ITEM_BOX_WIDTH;
                    }
                }
            }

            Image prevImage = PreviewPicture.Image;
            PreviewPicture.Image = bmp;
            if (prevImage != null)
                prevImage.Dispose();
        }

        /// <summary>
        /// Constructs the list of colors, based on the current selection
        /// </summary>
        /// <param name="forPreview">A value indicating if the color set is for preview use, which will put an upper limit on the number of generated colors</param>
        /// <returns>A list of colors matching the user selection</returns>
        private Color[] BuildColorSet(bool forPreview)
        {
            Color[] res = new Color[forPreview ? Math.Min((int)RuleCount.Value, 100) : (AggregateCombo.SelectedIndex == AggregateCombo.Items.Count - 1 ? m_values.Count : (int)RuleCount.Value)];

            if (GradientColors.Checked)
            {
                double stepR, stepG, stepB;
                stepR = (GradientToColor.CurrentColor.R - GradientFromColor.CurrentColor.R) / (double)res.Length;
                stepG = (GradientToColor.CurrentColor.G - GradientFromColor.CurrentColor.G) / (double)res.Length;
                stepB = (GradientToColor.CurrentColor.B - GradientFromColor.CurrentColor.B) / (double)res.Length;

                Color startColor = GradientFromColor.CurrentColor;

                for (int i = 0; i < res.Length; i++)
                    res[i] = Color.FromArgb(
                        Math.Min(255, Math.Max(0, (int)(startColor.R + Math.Round(i * stepR)))),
                        Math.Min(255, Math.Max(0, (int)(startColor.G + Math.Round(i * stepG)))),
                        Math.Min(255, Math.Max(0, (int)(startColor.B + Math.Round(i * stepB))))
                    );

            }
            else
            {
                ColorBrewer c = ((ColorBrewer.ColorBrewerListItem)ColorBrewerColorSet.SelectedItem).Set;
                if (c == null)
                    return new Color[0];

                for (int i = 0; i < res.Length; i++)
                    res[i] = c.Colors[i];
            }

            return res;
        }

        private void RuleCount_ValueChanged(object sender, EventArgs e)
        {
            RefreshColorBrewerSet();
            RefreshPreview();
        }

        private void AggregateCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AggregateCombo.SelectedIndex == AggregateCombo.Items.Count - 1)
            {
                if (m_values.Count > 100)
                {
                    MessageBox.Show(this, "The selected column contains more than 100 different values, and thus cannot be used for theming with individual values", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AggregateCombo.SelectedIndex = 0;
                    return;
                }

                RuleCount.Value = Math.Max(m_values.Count, RuleCount.Minimum);
                if (!GradientColors.Checked && ColorBrewerColors.Enabled)
                    ColorBrewerColors.Checked = true;
                else
                    GradientColors.Checked = true;

                RuleCountPanel.Enabled = false;
                RuleCount.Minimum = 0;
            }
            else if (AggregateCombo.Enabled)
            {
                RuleCountPanel.Enabled = true;
                RuleCount.Minimum = 3;
            }


            RefreshPreview();
        }

        private void GradientColors_CheckedChanged(object sender, EventArgs e)
        {
            RefreshPreview();
        }

        private void ColorBrewerColors_CheckedChanged(object sender, EventArgs e)
        {
            RefreshPreview();
        }

        private void ColorBrewerColorSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_isUpdating && ColorBrewerColorSet.SelectedIndex >= 0)
                ColorBrewerColors.Checked = true;

            RefreshPreview();
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            try
            {
                List<RuleItem> rules = CalculateRuleSet();

                if (rules.Count > 1000)
                {
                    if (MessageBox.Show(this, "You are creating a large number of rules, this will likely result in Maestro becoming unresponsive\r\nDo you want to continue?", Application.ProductName, MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                        return;
                }

                if (m_ruleCollection is MaestroAPI.PointTypeStyleType)
                {
                    MaestroAPI.PointTypeStyleType col = m_ruleCollection as MaestroAPI.PointTypeStyleType;

                    if (OverwriteRules.Checked)
                        col.PointRule.Clear();

                    foreach (RuleItem entry in rules)
                    {
                        MaestroAPI.PointRuleType r = new OSGeo.MapGuide.MaestroAPI.PointRuleType();
                        r.Item = MaestroAPI.Utility.DeepCopy(m_defaultItem) as MaestroAPI.PointSymbolization2DType;
                        r.Filter = entry.Filter;
                        r.LegendLabel = entry.Label;
                        if (r.Item.Item == null)
                            r.Item.Item = new OSGeo.MapGuide.MaestroAPI.MarkSymbolType();
                        if (r.Item.Item is MaestroAPI.MarkSymbolType)
                        {
                            if (((MaestroAPI.MarkSymbolType)r.Item.Item).Fill == null)
                                ((MaestroAPI.MarkSymbolType)r.Item.Item).Fill = new OSGeo.MapGuide.MaestroAPI.FillType();
                            ((MaestroAPI.MarkSymbolType)r.Item.Item).Fill.ForegroundColor = entry.Color;
                        }
                        else if (r.Item.Item is MaestroAPI.FontSymbolType)
                        {
                            ((MaestroAPI.FontSymbolType)r.Item.Item).ForegroundColor = entry.Color;
                        }

                        col.PointRule.Add(r);
                    }
                }
                else if (m_ruleCollection is MaestroAPI.LineTypeStyleType)
                {
                    MaestroAPI.LineTypeStyleType col = m_ruleCollection as MaestroAPI.LineTypeStyleType;

                    if (OverwriteRules.Checked)
                        col.LineRule.Clear();

                    foreach (RuleItem entry in rules)
                    {
                        MaestroAPI.LineRuleType l = new OSGeo.MapGuide.MaestroAPI.LineRuleType();
                        l.Items = MaestroAPI.Utility.XmlDeepCopy(m_defaultItem) as MaestroAPI.StrokeTypeCollection;
                        l.Filter = entry.Filter;
                        l.LegendLabel = entry.Label;
                        if (l.Items.Count == 0)
                            l.Items.Add(new OSGeo.MapGuide.MaestroAPI.StrokeType());
                        l.Items[0].Color = entry.Color;

                        col.LineRule.Add(l);
                    }
                }
                else if (m_ruleCollection is MaestroAPI.AreaTypeStyleType)
                {
                    MaestroAPI.AreaTypeStyleType col = m_ruleCollection as MaestroAPI.AreaTypeStyleType;

                    if (OverwriteRules.Checked)
                        col.AreaRule.Clear();

                    foreach (RuleItem entry in rules)
                    {
                        MaestroAPI.AreaRuleType r = new OSGeo.MapGuide.MaestroAPI.AreaRuleType();
                        r.Item = MaestroAPI.Utility.DeepCopy(m_defaultItem) as MaestroAPI.AreaSymbolizationFillType;
                        r.Filter = entry.Filter;
                        r.LegendLabel = entry.Label;
                        if (r.Item.Fill == null)
                            r.Item.Fill = new OSGeo.MapGuide.MaestroAPI.FillType();
                        r.Item.Fill.ForegroundColor = entry.Color;
                        col.AreaRule.Add(r);
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch(Exception ex)
            {
                m_editor.SetLastException(ex);
                MessageBox.Show(this, string.Format("An error occured: {0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void GradientFromColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_isUpdating)
                GradientColors.Checked = true;
            RefreshPreview();
        }

        private void GradientToColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_isUpdating)
                GradientColors.Checked = true;
            RefreshPreview();
        }

        private void PrepareDefaultItem()
        {
            if (m_ruleCollection is MaestroAPI.PointTypeStyleType)
            {
                MaestroAPI.PointSymbolization2DType i = new MaestroAPI.PointSymbolization2DType();
                MaestroAPI.MarkSymbolType m = new OSGeo.MapGuide.MaestroAPI.MarkSymbolType();

                i.Item = m;
                m.SizeContext = OSGeo.MapGuide.MaestroAPI.SizeContextType.DeviceUnits;
                m.SizeX = "10";
                m.SizeY = "10";
                m.Rotation = "0";
                m.Unit = OSGeo.MapGuide.MaestroAPI.LengthUnitType.Points;
                m.Fill = new OSGeo.MapGuide.MaestroAPI.FillType();
                m.Fill.FillPattern = "Solid";
                m.Fill.ForegroundColor = Color.Black;
                m.Fill.BackgroundColor = Color.Transparent;
                m.Edge = new OSGeo.MapGuide.MaestroAPI.StrokeType();
                m.Edge.Color = Color.Black;
                m.Edge.SizeContext = OSGeo.MapGuide.MaestroAPI.SizeContextType.DeviceUnits;
                m.Edge.Thickness = "1";
                m.Edge.Unit = OSGeo.MapGuide.MaestroAPI.LengthUnitType.Points;
                m.Edge.LineStyle = "Solid";

                m_defaultItem = i;
            }
            else if (m_ruleCollection is MaestroAPI.LineTypeStyleType)
            {
                MaestroAPI.StrokeTypeCollection i = new OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection();
                MaestroAPI.StrokeType s = new OSGeo.MapGuide.MaestroAPI.StrokeType();
                s.Color = Color.Black;
                s.LineStyle = "Solid";
                s.SizeContext = OSGeo.MapGuide.MaestroAPI.SizeContextType.DeviceUnits;
                s.Thickness = "1";
                s.Unit = OSGeo.MapGuide.MaestroAPI.LengthUnitType.Points;
                
                i.Add(s);
                
                m_defaultItem = i;
            }
            else if (m_ruleCollection is MaestroAPI.AreaTypeStyleType)
            {
                MaestroAPI.AreaSymbolizationFillType i = new MaestroAPI.AreaSymbolizationFillType();
                i.Fill = new OSGeo.MapGuide.MaestroAPI.FillType();
                i.Fill.BackgroundColor = Color.Transparent;
                i.Fill.ForegroundColor = Color.Black;
                i.Fill.FillPattern = "Solid";
                i.Stroke = new OSGeo.MapGuide.MaestroAPI.StrokeType();
                i.Stroke.Color = Color.Black;
                i.Stroke.LineStyle = "Solid";
                i.Stroke.SizeContext = OSGeo.MapGuide.MaestroAPI.SizeContextType.DeviceUnits;
                i.Stroke.Thickness = "1";
                i.Stroke.Unit = OSGeo.MapGuide.MaestroAPI.LengthUnitType.Points;
                
                m_defaultItem = i;
            }
        }

        private void ChangeBaseStyleBtn_Click(object sender, EventArgs e)
        {
            UserControl uc = null;
            if (m_ruleCollection is MaestroAPI.PointTypeStyleType)
            {
                uc = new GeometryStyleEditors.PointFeatureStyleEditor();
                ((GeometryStyleEditors.PointFeatureStyleEditor)uc).Item = (MaestroAPI.PointSymbolization2DType)MaestroAPI.Utility.XmlDeepCopy(m_defaultItem);
                ((GeometryStyleEditors.PointFeatureStyleEditor)uc).SetupForTheming();
            }
            else if (m_ruleCollection is MaestroAPI.LineTypeStyleType)
            {
                uc = new GeometryStyleEditors.LineFeatureStyleEditor();
                ((GeometryStyleEditors.LineFeatureStyleEditor)uc).Item = (MaestroAPI.StrokeTypeCollection)MaestroAPI.Utility.XmlDeepCopy(m_defaultItem);
                ((GeometryStyleEditors.LineFeatureStyleEditor)uc).SetupForTheming();
            }
            else if (m_ruleCollection is MaestroAPI.AreaTypeStyleType)
            {
                uc = new GeometryStyleEditors.AreaFeatureStyleEditor();
                ((GeometryStyleEditors.AreaFeatureStyleEditor)uc).Item = (MaestroAPI.AreaSymbolizationFillType)MaestroAPI.Utility.XmlDeepCopy(m_defaultItem);
                ((GeometryStyleEditors.AreaFeatureStyleEditor)uc).SetupForTheming();
            }

            if (uc != null)
            {
                LayerEditorControls.ScaleControls.EditorTemplateForm dlg = new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls.EditorTemplateForm();
                dlg.ItemPanel.Controls.Add(uc);
                uc.Dock = DockStyle.Fill;
                dlg.RefreshSize();

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    if (m_ruleCollection is MaestroAPI.PointTypeStyleType)
                        m_defaultItem = ((GeometryStyleEditors.PointFeatureStyleEditor)uc).Item;
                    else if (m_ruleCollection is MaestroAPI.LineTypeStyleType)
                        m_defaultItem = ((GeometryStyleEditors.LineFeatureStyleEditor)uc).Item;
                    else if (m_ruleCollection is MaestroAPI.AreaTypeStyleType)
                        m_defaultItem = ((GeometryStyleEditors.AreaFeatureStyleEditor)uc).Item;
                }
            }

        }

        private void ColorBrewerDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prevSet = ColorBrewerColorSet.Text;
            ColorBrewerColorSet.Items.Clear();

            foreach (ColorBrewer cb in m_colorBrewer)
                if (cb.Colors.Count == RuleCount.Value && cb.DisplayType == ColorBrewerDataType.Text)
                    ColorBrewerColorSet.Items.Add(new ColorBrewer.ColorBrewerListItem(cb, ColorBrewer.ColorBrewerListItem.DisplayMode.Set));

            if (ColorBrewerColorSet.FindString(prevSet) >= 0)
                ColorBrewerColorSet.SelectedIndex = ColorBrewerColorSet.FindString(prevSet);
            else if (ColorBrewerColorSet.Items.Count > 0)
                ColorBrewerColorSet.SelectedIndex = 0;

            if (!m_isUpdating)
                ColorBrewerColors.Checked = true;

            RefreshPreview();
        }

        private void ColorBrewerLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            m_editor.OpenUrl("http://colorbrewer.org/");
        }

    }
}
