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
using System.IO;
using System.Text;
using System.Windows.Forms;
using Maestro.Editors.Common;
using Maestro.Editors.LayerDefinition.Vector.Scales;
using Maestro.Editors.LayerDefinition.Vector.StyleEditors;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using Ldf = OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.MaestroAPI.Schema;
using System.Collections.Specialized;
using Maestro.Editors.Generic;
using Maestro.Shared.UI;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using OSGeo.MapGuide.MaestroAPI.Services;
using System.Diagnostics;

namespace Maestro.Editors.LayerDefinition.Vector.Thematics
{
    internal partial class ThemeCreator : Form
    {
        private const int PREVIEW_ITEM_BOX_WIDTH = 20;
        private const int PREVIEW_ITEM_BOX_SPACING = 10;
        const int MAX_NUMERIC_THEME_RULES = 100000;
        const int MAX_INDIVIDUAL_THEME_RULES = 100;
        const int THEME_RULE_WARNING_LIMIT = 1000;

        private static List<ColorBrewer> m_colorBrewer;

        private IEditorService m_editor;
        private ILayerDefinition m_layer;
        private ClassDefinition m_featureClass;
        private Dictionary<object, long> m_values;
        private DataPropertyType m_dataType;

        class LookupPair
        {
            public object Key;
            public object Value;
        }

        private List<LookupPair> m_lookupValues;
        
        private object m_ruleCollection;

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

        private ILayerElementFactory2 _factory;

        public ThemeCreator(IEditorService editor, ILayerDefinition layer, ClassDefinition schema, object ruleCollection)
            : this()
        {
            m_editor = editor;
            m_layer = layer;
            m_featureClass = schema;
            m_ruleCollection = ruleCollection;

            _factory = (ILayerElementFactory2)editor.GetEditedResource();

            ColorBrewerColorSet.SetCustomRender(new CustomCombo.RenderCustomItem(DrawColorSetPreview));
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
                m_colorBrewer = ColorBrewer.ParseCSV(GetCsvPath());

            UpdateThemeChoice();
        }

        private string GetCsvPath()
        {
            //First test root app path. If not there, try to write a copy from
            //our embedded resource copy. If that fails, write to a temp file
            //and return that path

            string path = System.IO.Path.Combine(Application.StartupPath, "ColorBrewer.csv");
            if (!File.Exists(path))
            {
                string content = Properties.Resources.ColorBrewer;
                try
                {
                    File.WriteAllText(path, content);
                }
                catch
                {
                    path = Path.GetTempFileName();
                    File.WriteAllText(path, content);
                }
            }
            return path;
        }

        private void ThemeCreator_Load(object sender, EventArgs e)
        {
            GradientFromColor.CurrentColor = Color.Red;
            GradientToColor.CurrentColor = Color.Green;

            ColumnCombo.Items.Clear();
            ColumnCombo.Items.Add(Strings.SelectColumnPlaceholder);
            ColumnCombo.SelectedIndex = 0;

            foreach (var col in m_featureClass.Properties)
            {
                if (col.Type == PropertyDefinitionType.Data)
                    ColumnCombo.Items.Add(col.Name);
            }
        }

        private void UpdateUIForClassSelection()
        {
            if (ColumnCombo.SelectedIndex == 0)
            {
                DisableThemeOptions();
            }
            else
            {
                DisplayGroup.Enabled =
                PreviewGroup.Enabled =
                OKBtn.Enabled =
                    true;

                m_values = new Dictionary<object, long>();

                PropertyDefinition col = m_featureClass.FindProperty(ColumnCombo.Text);

                //Not really possible
                if (col == null)
                    throw new Exception(Strings.InvalidColumnNameError);

                string filter = null; //Attempt raw reading initially
                Exception rawEx = null; //Original exception
                bool retry = true;

                while (retry)
                {
                    retry = false;
                    try
                    {
                        IVectorLayerDefinition vl = (IVectorLayerDefinition)m_layer.SubLayer;
                        if (!string.IsNullOrEmpty(vl.Filter))
                            filter = vl.Filter;
                        try
                        {
                            //Either UNIQUE() is an undocumented FDO expression function (!!!)
                            //Or it is FDO expression sugar to work around the fact there is no distinct
                            //flag in the SELECTAGGREGATES operation that's exposed over HTTP. Either
                            //case, try this method first.
                            using (var rd = m_editor.FeatureService.AggregateQueryFeatureSource(
                                                    vl.ResourceId,
                                                    m_featureClass.QualifiedName,
                                                    filter,
                                                    new NameValueCollection() {
                                                        { "value", "UNIQUE(\"" + col.Name + "\")" } 
                                                    }))
                            {
                                while (rd.ReadNext() && m_values.Count < MAX_NUMERIC_THEME_RULES) //No more than 100.000 records in memory
                                {
                                    if (!rd.IsNull("value"))
                                    {
                                        object value = rd["value"];
                                        if (!m_values.ContainsKey(value))
                                            m_values.Add(value, 0);

                                        m_values[value]++;
                                    }
                                }
                                rd.Close();
                            }
                        }
                        catch
                        {

                            using (var rd = m_editor.FeatureService.QueryFeatureSource(vl.ResourceId, m_featureClass.QualifiedName, filter, new string[] { col.Name }))
                            {
                                while (rd.ReadNext() && m_values.Count < MAX_NUMERIC_THEME_RULES) //No more than 100.000 records in memory
                                {
                                    if (!rd.IsNull(col.Name))
                                    {
                                        object value = rd[col.Name];
                                        if (!m_values.ContainsKey(value))
                                            m_values.Add(value, 0);

                                        m_values[value]++;
                                    }
                                }
                                rd.Close();
                            }
                        }
                        rawEx = null; //Clear error

                    }
                    catch (Exception ex)
                    {
                        rawEx = ex;
                        if (filter == null && ex.Message.IndexOf("MgNullPropertyValueException") >= 0) //Known issue
                        {
                            retry = true;
                            filter = "NOT " + col.Name + " NULL";
                        }
                    }
                }

                if (rawEx != null)
                {
                    MessageBox.Show(this, string.Format(Strings.DataReadError, rawEx.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ColumnCombo.SelectedIndex = 0;
                    return;
                }

                if (m_values.Count == 0)
                {
                    MessageBox.Show(this, Strings.ColumnHasNoValidDataError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ColumnCombo.SelectedIndex = 0;
                    return;
                }

                if (col.Type == PropertyDefinitionType.Data)
                {
                    DataPropertyDefinition dp = ((DataPropertyDefinition)col);
                    m_dataType = dp.DataType;

                    if (dp.IsNumericType())
                    {
                        if (m_values.Count >= MAX_NUMERIC_THEME_RULES)
                            MessageBox.Show(this, string.Format(Strings.TooMuchDataWarning, MAX_NUMERIC_THEME_RULES), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        GroupPanel.Enabled = true;
                        RuleCountPanel.Enabled = true;
                        RuleCount.Minimum = 3;

                        if (m_values.Count <= 9)
                        {
                            AggregateCombo.SelectedIndex = AggregateCombo.Items.Count - 1;
                            AggregateCombo_SelectedIndexChanged(this, EventArgs.Empty);

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
                        }

                    }
                    else //String type
                    {
                        if (m_values.Count > MAX_INDIVIDUAL_THEME_RULES)
                        {
                            MessageBox.Show(this, string.Format(Strings.TooManyValuesError, MAX_INDIVIDUAL_THEME_RULES), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                }

                RefreshColorBrewerSet();
                RefreshPreview();
            }
        }

        private void DisableThemeOptions()
        {
            //Dummy item selected, just disable the form
            RuleCountPanel.Enabled =
            GroupPanel.Enabled =
            DisplayGroup.Enabled =
            PreviewGroup.Enabled =
            OKBtn.Enabled =
                false;
        }

        private void ColumnCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ColumnCombo.SelectedIndex == 0)
            {
                DisableThemeOptions();
            }
            else
            {
                if (rdValuesFromClass.Checked)
                {
                    UpdateUIForClassSelection();
                }
                else if (rdValuesFromLookup.Checked)
                {
                    UpdateUIForExternalLookup();
                }
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

            if (rdValuesFromClass.Checked)
            {
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
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, Strings.LessThanLabel, FormatValue(chunksize + min)),
                            colors[0]));

                        for (int i = 1; i < colors.Length - 1; i++)
                            result.Add(new RuleItem(
                                string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" > {1} AND \"{0}\" <= {2}", ColumnCombo.Text, FormatValue(min + (i * chunksize)), FormatValue(min + ((i + 1) * chunksize))),
                                string.Format(System.Globalization.CultureInfo.InvariantCulture, Strings.BetweenLabel, FormatValue(min + (i * chunksize)), FormatValue(min + ((i + 1) * chunksize))),
                                colors[i]));

                        result.Add(new RuleItem(
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" > {1}", ColumnCombo.Text, FormatValue(max - chunksize)),
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, Strings.MoreThanLabel, FormatValue(max - chunksize)),
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
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, Strings.LessThanLabel, FormatValue(lower + dev)),
                            colors[0]));

                        for (int i = 1; i < colors.Length - 1; i++)
                            result.Add(new RuleItem(
                                string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" >= {1} AND \"{0}\" < {2}", ColumnCombo.Text, FormatValue(lower + (i * dev)), FormatValue(lower + ((i + 1) * dev))),
                                string.Format(System.Globalization.CultureInfo.InvariantCulture, Strings.BetweenLabel, FormatValue(lower + (i * dev)), FormatValue(lower + ((i + 1) * dev))),
                                colors[i]));

                        result.Add(new RuleItem(
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" >= {1}", ColumnCombo.Text, FormatValue(lower + (dev * (colors.Length - 1)))),
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, Strings.MoreThanLabel, FormatValue(lower + (dev * (colors.Length - 1)))),
                            colors[colors.Length - 1]));

                    }
                    else if (AggregateCombo.SelectedIndex == 2) //Quantile
                    {
                        SortedDictionary<double, long> sort = new SortedDictionary<double, long>();
                        foreach (KeyValuePair<object, long> entry in m_values)
                            sort.Add(Convert.ToDouble(entry.Key), entry.Value);

                        double step = (1.0 / colors.Length) * count;
                        List<double> separators = new List<double>();
                        for (int i = 1; i < colors.Length; i++)
                        {
                            long limit = (long)Math.Round(step * i);
                            long cc = 0;
                            double item = double.NaN;

                            foreach (KeyValuePair<double, long> entry in sort)
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
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, Strings.LessThanLabel, FormatValue(separators[0])),
                            colors[0]));

                        for (int i = 1; i < colors.Length - 1; i++)
                            result.Add(new RuleItem(
                                string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" > {1} AND \"{0}\" <= {2}", ColumnCombo.Text, FormatValue(separators[i - 1]), FormatValue(separators[i])),
                                string.Format(System.Globalization.CultureInfo.InvariantCulture, Strings.BetweenLabel, FormatValue(separators[i - 1]), FormatValue(separators[i])),
                                colors[i]));

                        result.Add(new RuleItem(
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" > {1}", ColumnCombo.Text, FormatValue(separators[separators.Count - 1])),
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, Strings.MoreThanLabel, FormatValue(separators[separators.Count - 1])),
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
            }
            else if (rdValuesFromLookup.Checked)
            {
                //We use the type of the primary property and not the secondary key property
                //as the filter will be generated against the primary property
                PropertyDefinition keyProp = m_featureClass.FindProperty(ColumnCombo.Text);
                bool isStringKeyProp = false;
                if (keyProp.Type == PropertyDefinitionType.Data)
                {
                    DataPropertyDefinition dp = (DataPropertyDefinition)keyProp;
                    isStringKeyProp = (dp.DataType == DataPropertyType.String);
                }

                for (int i = 0; i < colors.Length; i++)
                {
                    var pair = m_lookupValues[i];
                    if (isStringKeyProp)
                    {
                        result.Add(
                            new RuleItem(
                                string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" = '{1}'", ColumnCombo.Text, pair.Key),
                                pair.Value.ToString(),
                                colors[i]
                            ));
                    }
                    else
                    {
                        result.Add(
                            new RuleItem(
                                string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" = {1}", ColumnCombo.Text, pair.Key),
                                pair.Value.ToString(),
                                colors[i]
                            ));
                    }
                }
            }

            return result;
        }

        private string FormatValue(double value)
        {
            if (m_dataType == DataPropertyType.Double || m_dataType == DataPropertyType.Single)
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

            System.Drawing.Image prevImage = PreviewPicture.Image;
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
            Color[] res = null;
            if (rdValuesFromClass.Checked)
                res = new Color[forPreview ? Math.Min((int)RuleCount.Value, MAX_INDIVIDUAL_THEME_RULES) : (AggregateCombo.SelectedIndex == AggregateCombo.Items.Count - 1 ? m_values.Count : (int)RuleCount.Value)];
            else if (rdValuesFromLookup.Checked)
                res = new Color[forPreview ? Math.Min((int)RuleCount.Value, MAX_INDIVIDUAL_THEME_RULES) : (AggregateCombo.SelectedIndex == AggregateCombo.Items.Count - 1 ? m_lookupValues.Count : (int)RuleCount.Value)];

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
                if (ColorBrewerColorSet.SelectedItem == null)
                    return new Color[0];

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
                if (m_values.Count > MAX_INDIVIDUAL_THEME_RULES)
                {
                    MessageBox.Show(this, string.Format(Strings.TooManyIndiviualValuesError, MAX_INDIVIDUAL_THEME_RULES), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            btnFlipColorBrewer.Enabled = false;
            RefreshPreview();
        }

        private void ColorBrewerColors_CheckedChanged(object sender, EventArgs e)
        {
            btnFlipColorBrewer.Enabled = true;
            RefreshPreview();
        }

        private void ColorBrewerColorSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_isUpdating && ColorBrewerColorSet.SelectedIndex >= 0)
                ColorBrewerColors.Checked = true;

            RefreshPreview();
        }

        private IPointRule CreatePointRule(IPointRule template, ILayerElementFactory factory)
        {
            var ptRule = factory.CreateDefaultPointRule();

            var srcSym = template.PointSymbolization2D;
            if (srcSym != null)
            {
                ptRule.PointSymbolization2D = srcSym.Clone();
            }
            var srcLabel = template.Label;
            if (srcLabel != null)
            {
                ptRule.Label = srcLabel.Clone();
            }
            return ptRule;
        }

        private ILineRule CreateLineRule(ILineRule template, ILayerElementFactory factory)
        {
            var lrule = factory.CreateDefaultLineRule();
            foreach (var st in template.Strokes)
            {
                lrule.AddStroke(st.Clone());
            }
            if (template.Label != null)
                lrule.Label = template.Label.Clone();
            return lrule;
        }

        private IAreaRule CreateAreaRule(IAreaRule template, ILayerElementFactory factory)
        {
            var arule = factory.CreateDefaultAreaRule();
            if (template.AreaSymbolization2D != null)
                arule.AreaSymbolization2D = template.AreaSymbolization2D.Clone();
            if (template.Label != null)
                arule.Label = template.Label.Clone();
            return arule;
        }

        private ICompositeRule CreateCompositeRule(ICompositeRule template, ILayerElementFactory2 factory)
        {
            var crule = factory.CreateDefaultCompositeRule();
            if (template.CompositeSymbolization != null)
                crule.CompositeSymbolization = factory.CloneCompositeSymbolization(template.CompositeSymbolization);
            return crule;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            try
            {
                List<RuleItem> rules = CalculateRuleSet();

                if (rules.Count > THEME_RULE_WARNING_LIMIT)
                {
                    if (MessageBox.Show(this, Strings.TooManyRulesWarning, Application.ProductName, MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                        return;
                }

                IPointVectorStyle pts = m_ruleCollection as IPointVectorStyle;
                ILineVectorStyle lts = m_ruleCollection as ILineVectorStyle;
                IAreaVectorStyle ats = m_ruleCollection as IAreaVectorStyle;
                ICompositeTypeStyle cts = m_ruleCollection as ICompositeTypeStyle;

                if (pts != null)
                {
                    GeneratePointThemeRules(rules, pts);
                }
                else if (lts != null)
                {
                    GenerateLineThemeRules(rules, lts);
                }
                else if (ats != null)
                {
                    GenerateAreaThemeRules(rules, ats);
                }
                else if (cts != null)
                {
                    GenerateCompositeThemeRules(rules, cts);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch(Exception ex)
            {
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                //m_editor.SetLastException(ex);
                MessageBox.Show(this, string.Format(Strings.GenericError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        private static ISymbolDefinitionBase GetSymbolFromReference(IResourceService resSvc, ISymbolInstanceReference symRef)
        {
            switch(symRef.Type)
            {
                case SymbolInstanceType.Inline:
                    return ((ISymbolInstanceReferenceInline)symRef).SymbolDefinition;
                case SymbolInstanceType.Reference:
                    return (ISymbolDefinitionBase)resSvc.GetResource(((ISymbolInstanceReferenceLibrary)symRef).ResourceId);
            }
            return null;
        }

        enum FillColorSource
        {
            PathFillColor,
            SymbolParameterFillColorDefaultValue,
            SymbolParameterFillColorOverride,
            PathLineColor,
            SymbolParameterLineColorDefaultValue,
            SymbolParameterLineColorOverride
        }

        private void GenerateCompositeThemeRules(List<RuleItem> rules, ICompositeTypeStyle col)
        {
            if (!chkUseFirstRuleAsTemplate.Checked || col.RuleCount == 0)
            {
                MessageBox.Show(Strings.CompositeThemeRequiresFirstRuleAsTemplate);
                return;
            }

            //TODO: Composite styles for lines probably don't have fill colors, as such theme rule generation
            //probably won't work for line styles atm

            FillColorSource? source = null;
            string fillAlpha = "";
            ICompositeRule template = null;
            if (chkUseFirstRuleAsTemplate.Checked && col.RuleCount > 0)
            {
                template = col.GetRuleAt(0);
                if (template.CompositeSymbolization != null)
                {
                    IdentifyColorSource(template, ref source, ref fillAlpha);
                }
            }

            if (OverwriteRules.Checked)
            {
                col.RemoveAllRules();
            }

            foreach (RuleItem entry in rules)
            {
                var r = (template != null) ? CreateCompositeRule(template, _factory) : _factory.CreateDefaultCompositeRule();
                Debug.WriteLine("Made rule {0}", r.GetHashCode());
                r.Filter = entry.Filter;
                r.LegendLabel = entry.Label;
                if (r.CompositeSymbolization != null)
                {
                    SetColorForCompositeRule(source, fillAlpha, entry, r);
                }

                col.AddCompositeRule(r);
            }
        }

        private void SetColorForCompositeRule(FillColorSource? source, string fillAlpha, RuleItem entry, ICompositeRule r)
        {
            // NOTE: Same naivete as IdentifyColorSource(). Refer to that method for all the gory details

            bool bSetFill = false;
            foreach (ISymbolInstance symInst in r.CompositeSymbolization.SymbolInstance)
            {
                if (bSetFill)
                    break;

                var symRef = GetSymbolFromReference(m_editor.ResourceService, symInst.Reference);
                var simpleSym = symRef as ISimpleSymbolDefinition;
                if (simpleSym == null)
                    throw new NotSupportedException(Strings.CannotCreateThemeFromCompoundSymbolInstance);

                var symName = simpleSym.Name;
                //Find the first path graphic with a fill color
                foreach (var graphic in simpleSym.Graphics)
                {
                    if (bSetFill)
                        break;

                    if (graphic.Type == GraphicElementType.Path)
                    {
                        IPathGraphic path = (IPathGraphic)graphic;
                        if (path.FillColor != null)
                        {
                            string color = path.FillColor;
                            if (source.Value == FillColorSource.PathFillColor)
                            {
                                path.FillColor = "0x" + fillAlpha + Utility.SerializeHTMLColor(entry.Color, string.IsNullOrEmpty(fillAlpha));
                                Debug.WriteLine(string.Format("Set fill color to {0} for symbol instance {1} of symbolization {2} in rule {3}", path.FillColor, symInst.GetHashCode(), r.CompositeSymbolization.GetHashCode(), r.GetHashCode()));
                                bSetFill = true;
                                break;
                            }
                            //Is this a parameter?
                            if (color.StartsWith("%") && color.EndsWith("%"))
                            {
                                string paramName = color.Substring(1, color.Length - 2);
                                if (simpleSym.ParameterDefinition != null)
                                {
                                    foreach (var paramDef in simpleSym.ParameterDefinition.Parameter)
                                    {
                                        if (bSetFill)
                                            break;

                                        if (paramDef.Name == paramName)
                                        {
                                            if (source.Value == FillColorSource.SymbolParameterFillColorDefaultValue)
                                            {
                                                paramDef.DefaultValue = "0x" + fillAlpha + Utility.SerializeHTMLColor(entry.Color, string.IsNullOrEmpty(fillAlpha));
                                                Debug.WriteLine(string.Format("Set fill color default parameter value to {0} for symbol instance {1} of symbolization {2} in rule {3}", paramDef.DefaultValue, symInst.GetHashCode(), r.CompositeSymbolization.GetHashCode(), r.GetHashCode()));
                                                bSetFill = true;
                                                break;
                                            }

                                            //But wait ... Is there an override for this too?
                                            var ov = symInst.ParameterOverrides;
                                            if (ov != null)
                                            {
                                                foreach (var pov in ov.Override)
                                                {
                                                    if (bSetFill)
                                                        break;

                                                    if (pov.SymbolName == symName && pov.ParameterIdentifier == paramName)
                                                    {
                                                        if (source.Value == FillColorSource.SymbolParameterFillColorOverride)
                                                        {
                                                            pov.ParameterValue = "0x" + fillAlpha + Utility.SerializeHTMLColor(entry.Color, string.IsNullOrEmpty(fillAlpha));
                                                            Debug.WriteLine(string.Format("Set fill color parameter override value to {0} for symbol instance {1} of symbolization {2} in rule {3}", pov.ParameterValue, symInst.GetHashCode(), r.CompositeSymbolization.GetHashCode(), r.GetHashCode()));
                                                            bSetFill = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (path.LineColor != null && simpleSym.LineUsage != null)
                        {
                            string color = path.LineColor;
                            if (source.Value == FillColorSource.PathLineColor)
                            {
                                path.LineColor = "0x" + fillAlpha + Utility.SerializeHTMLColor(entry.Color, string.IsNullOrEmpty(fillAlpha));
                                Debug.WriteLine(string.Format("Set line color to {0} for symbol instance {1} of symbolization {2} in rule {3}", path.FillColor, symInst.GetHashCode(), r.CompositeSymbolization.GetHashCode(), r.GetHashCode()));
                                bSetFill = true;
                                break;
                            }
                            //Is this a parameter?
                            if (color.StartsWith("%") && color.EndsWith("%"))
                            {
                                string paramName = color.Substring(1, color.Length - 2);
                                if (simpleSym.ParameterDefinition != null)
                                {
                                    foreach (var paramDef in simpleSym.ParameterDefinition.Parameter)
                                    {
                                        if (bSetFill)
                                            break;

                                        if (paramDef.Name == paramName)
                                        {
                                            if (source.Value == FillColorSource.SymbolParameterLineColorDefaultValue)
                                            {
                                                paramDef.DefaultValue = "0x" + fillAlpha + Utility.SerializeHTMLColor(entry.Color, string.IsNullOrEmpty(fillAlpha));
                                                Debug.WriteLine(string.Format("Set line color default parameter value to {0} for symbol instance {1} of symbolization {2} in rule {3}", paramDef.DefaultValue, symInst.GetHashCode(), r.CompositeSymbolization.GetHashCode(), r.GetHashCode()));
                                                bSetFill = true;
                                                break;
                                            }

                                            //But wait ... Is there an override for this too?
                                            var ov = symInst.ParameterOverrides;
                                            if (ov != null)
                                            {
                                                foreach (var pov in ov.Override)
                                                {
                                                    if (bSetFill)
                                                        break;

                                                    if (pov.SymbolName == symName && pov.ParameterIdentifier == paramName)
                                                    {
                                                        if (source.Value == FillColorSource.SymbolParameterLineColorOverride)
                                                        {
                                                            pov.ParameterValue = "0x" + fillAlpha + Utility.SerializeHTMLColor(entry.Color, string.IsNullOrEmpty(fillAlpha));
                                                            Debug.WriteLine(string.Format("Set line color parameter override value to {0} for symbol instance {1} of symbolization {2} in rule {3}", pov.ParameterValue, symInst.GetHashCode(), r.CompositeSymbolization.GetHashCode(), r.GetHashCode()));
                                                            bSetFill = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void IdentifyColorSource(ICompositeRule template, ref FillColorSource? source, ref string fillAlpha)
        {
            // FIXME: This is very naive. It will identify the first color it finds and runs with it.
            // It doesn't take into consideration things such as usage contexts, which we currently only care about on 
            // the 2nd pass when we still can't identify a color and presumably we're dealing with a composite symbolization 
            // with line usage (where fill colors won't exist most of the time). Thematics should be fine on the most basic
            // of composite symbolization cases, but I expect this process to break down hardcore on the most elaborate of 
            // composite symbolization cases. Still, some thematic support is better than none.

            // So anyways, 1st pass: Assume point/area and identify the first fill color we find
            foreach (ISymbolInstance symInst in template.CompositeSymbolization.SymbolInstance)
            {
                if (source.HasValue)
                    break;

                var symRef = GetSymbolFromReference(m_editor.ResourceService, symInst.Reference);
                var simpleSym = symRef as ISimpleSymbolDefinition;
                if (simpleSym == null)
                    throw new NotSupportedException(Strings.CannotCreateThemeFromCompoundSymbolInstance);

                var symName = simpleSym.Name;
                //Find the first path graphic with a fill color
                foreach (var graphic in simpleSym.Graphics)
                {
                    if (source.HasValue)
                        break;

                    if (graphic.Type == GraphicElementType.Path)
                    {
                        IPathGraphic path = (IPathGraphic)graphic;
                        if (path.FillColor != null)
                        {
                            var hexIdx = path.FillColor.IndexOf("0x");
                            if (hexIdx >= 0 && hexIdx + 4 < path.FillColor.Length)
                            {
                                fillAlpha = path.FillColor.Substring(hexIdx + 2, 2);
                                source = FillColorSource.PathFillColor;
                            }
                            else
                            {
                                string color = path.FillColor;
                                //Is this a parameter?
                                if (color.StartsWith("%") && color.EndsWith("%"))
                                {
                                    string paramName = color.Substring(1, color.Length - 2);
                                    if (simpleSym.ParameterDefinition != null)
                                    {
                                        foreach (var paramDef in simpleSym.ParameterDefinition.Parameter)
                                        {
                                            if (source.HasValue)
                                                break;

                                            if (paramDef.Name == paramName)
                                            {
                                                hexIdx = paramDef.DefaultValue.IndexOf("0x");
                                                if (hexIdx >= 0 && hexIdx + 4 < paramDef.DefaultValue.Length)
                                                {
                                                    fillAlpha = paramDef.DefaultValue.Substring(hexIdx + 2, 2);
                                                    source = FillColorSource.SymbolParameterFillColorDefaultValue;
                                                }
                                                //But wait ... Is there an override for this too?

                                                var ov = symInst.ParameterOverrides;
                                                if (ov != null)
                                                {
                                                    foreach (var pov in ov.Override)
                                                    {
                                                        if (pov.SymbolName == symName && pov.ParameterIdentifier == paramName)
                                                        {
                                                            hexIdx = pov.ParameterValue.IndexOf("0x");
                                                            if (hexIdx >= 0 && hexIdx + 4 < pov.ParameterValue.Length)
                                                            {
                                                                fillAlpha = pov.ParameterValue.Substring(hexIdx + 2, 2);
                                                                source = FillColorSource.SymbolParameterFillColorOverride;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (!source.HasValue)
            {
                //Still no source? Time for a 2nd pass, this time find the first line color where the symbol instance has a line usage
                //context
                foreach (ISymbolInstance symInst in template.CompositeSymbolization.SymbolInstance)
                {
                    if (source.HasValue)
                        break;

                    var symRef = GetSymbolFromReference(m_editor.ResourceService, symInst.Reference);
                    var simpleSym = symRef as ISimpleSymbolDefinition;
                    if (simpleSym == null)
                        throw new NotSupportedException(Strings.CannotCreateThemeFromCompoundSymbolInstance);

                    var symName = simpleSym.Name;
                    //Find the first path graphic with a fill color
                    foreach (var graphic in simpleSym.Graphics)
                    {
                        if (source.HasValue)
                            break;

                        if (graphic.Type == GraphicElementType.Path)
                        {
                            IPathGraphic path = (IPathGraphic)graphic;
                            if (path.LineColor != null)
                            {
                                //Bail on this symbol if it has no line usage context
                                if (simpleSym.LineUsage == null)
                                    continue;

                                var hexIdx = path.LineColor.IndexOf("0x");
                                if (hexIdx >= 0 && hexIdx + 4 < path.LineColor.Length)
                                {
                                    fillAlpha = path.LineColor.Substring(hexIdx + 2, 2);
                                    source = FillColorSource.PathLineColor;
                                }
                                else
                                {
                                    string color = path.LineColor;
                                    //Is this a parameter?
                                    if (color.StartsWith("%") && color.EndsWith("%"))
                                    {
                                        string paramName = color.Substring(1, color.Length - 2);
                                        if (simpleSym.ParameterDefinition != null)
                                        {
                                            foreach (var paramDef in simpleSym.ParameterDefinition.Parameter)
                                            {
                                                if (source.HasValue)
                                                    break;

                                                if (paramDef.Name == paramName)
                                                {
                                                    hexIdx = paramDef.DefaultValue.IndexOf("0x");
                                                    if (hexIdx >= 0 && hexIdx + 4 < paramDef.DefaultValue.Length)
                                                    {
                                                        fillAlpha = paramDef.DefaultValue.Substring(hexIdx + 2, 2);
                                                        source = FillColorSource.SymbolParameterLineColorDefaultValue;
                                                    }
                                                    //But wait ... Is there an override for this too?

                                                    var ov = symInst.ParameterOverrides;
                                                    if (ov != null)
                                                    {
                                                        foreach (var pov in ov.Override)
                                                        {
                                                            if (pov.SymbolName == symName && pov.ParameterIdentifier == paramName)
                                                            {
                                                                hexIdx = pov.ParameterValue.IndexOf("0x");
                                                                if (hexIdx >= 0 && hexIdx + 4 < pov.ParameterValue.Length)
                                                                {
                                                                    fillAlpha = pov.ParameterValue.Substring(hexIdx + 2, 2);
                                                                    source = FillColorSource.SymbolParameterLineColorOverride;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void GeneratePointThemeRules(List<RuleItem> rules, IPointVectorStyle col)
        {
            string fillAlpha = "";
            IPointRule template = null;
            if (chkUseFirstRuleAsTemplate.Checked && col.RuleCount > 0)
            {
                template = col.GetRuleAt(0);
                var sym = template.PointSymbolization2D.Symbol;
                if (sym.Type == PointSymbolType.Mark)
                {
                    string htmlColor = ((IMarkSymbol)sym).Fill.ForegroundColor;
                    if (htmlColor.Length == 8)
                        fillAlpha = htmlColor.Substring(0, 2);
                }
                else if (sym.Type == PointSymbolType.Font)
                {
                    string htmlColor = ((IFontSymbol)sym).ForegroundColor;
                    if (htmlColor.Length == 8)
                        fillAlpha = htmlColor.Substring(0, 2);
                }
            }

            if (OverwriteRules.Checked)
                col.RemoveAllRules();

            foreach (RuleItem entry in rules)
            {
                IPointRule r = (template != null) ? CreatePointRule(template, _factory) : _factory.CreateDefaultPointRule();
                r.Filter = entry.Filter;
                r.LegendLabel = entry.Label;
                var sym = r.PointSymbolization2D.Symbol;
                if (sym.Type == PointSymbolType.Mark)
                {
                    ((IMarkSymbol)sym).Fill.ForegroundColor = fillAlpha + Utility.SerializeHTMLColor(entry.Color, string.IsNullOrEmpty(fillAlpha));
                }
                else if (sym.Type == PointSymbolType.Font)
                {
                    ((IFontSymbol)sym).ForegroundColor = fillAlpha + Utility.SerializeHTMLColor(entry.Color, string.IsNullOrEmpty(fillAlpha));
                }
                col.AddRule(r);
            }
        }

        private void GenerateLineThemeRules(List<RuleItem> rules, ILineVectorStyle col)
        {
            string bordAlpha = "";
            ILineRule template = null;
            if (chkUseFirstRuleAsTemplate.Checked && col.RuleCount > 0)
            {
                template = col.GetRuleAt(0);

                //TODO: Composite lines? Which "alpha" value wins there?
                foreach (var st in template.Strokes)
                {
                    if (st.Color.Length == 8)
                    {
                        bordAlpha = st.Color.Substring(0, 2);
                        break;
                    }
                }
            }

            if (OverwriteRules.Checked)
                col.RemoveAllRules();

            foreach (RuleItem entry in rules)
            {
                var l = (template != null) ? CreateLineRule(template, _factory) : _factory.CreateDefaultLineRule();
                l.Filter = entry.Filter;
                l.LegendLabel = entry.Label;
                foreach (var st in l.Strokes)
                {
                    st.Color = bordAlpha + Utility.SerializeHTMLColor(entry.Color, string.IsNullOrEmpty(bordAlpha));
                }
                col.AddRule(l);
            }
        }

        private void GenerateAreaThemeRules(List<RuleItem> rules, IAreaVectorStyle col)
        {
            string fillAlpha = "";
            IAreaRule template = null;
            if (chkUseFirstRuleAsTemplate.Checked && col.RuleCount > 0)
            {
                template = col.GetRuleAt(0);

                if (template.AreaSymbolization2D != null)
                {
                    if (template.AreaSymbolization2D.Fill != null)
                    {
                        if (template.AreaSymbolization2D.Fill.ForegroundColor.Length == 8)
                            fillAlpha = template.AreaSymbolization2D.Fill.ForegroundColor.Substring(0, 2);
                    }
                }
            }

            if (OverwriteRules.Checked)
                col.RemoveAllRules();

            foreach (RuleItem entry in rules)
            {
                var r = (template != null) ? CreateAreaRule(template, _factory) : _factory.CreateDefaultAreaRule();
                r.Filter = entry.Filter;
                r.LegendLabel = entry.Label;
                r.AreaSymbolization2D.Fill.ForegroundColor = fillAlpha + Utility.SerializeHTMLColor(entry.Color, string.IsNullOrEmpty(fillAlpha));

                col.AddRule(r);
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

        private void ColorBrewerDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadColorBrewerOptions();
        }

        private void LoadColorBrewerOptions()
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

        private void btnFlipColorBrewer_Click(object sender, EventArgs e)
        {
            foreach (var cb in m_colorBrewer)
            {
                cb.Flip();
            }
            LoadColorBrewerOptions();
        }

        private void UpdateThemeChoice()
        {
            GroupPanel.Enabled = rdValuesFromClass.Checked;
            grpValuesFromLookup.Enabled = rdValuesFromLookup.Checked;
        }

        private void rdValuesFromClass_CheckedChanged(object sender, EventArgs e)
        {
            UpdateThemeChoice();
            if (rdValuesFromClass.Checked)
                UpdateUIForClassSelection();
        }

        private void rdValuesFromLookup_CheckedChanged(object sender, EventArgs e)
        {
            UpdateThemeChoice();
            if (rdValuesFromLookup.Checked)
                UpdateUIForExternalLookup();
        }

        private void UpdateUIForExternalLookup()
        {
            DisableThemeOptions();
        }

        private void btnBrowseFeatureSource_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(m_editor.ResourceService, ResourceTypes.FeatureSource, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtFeatureSource.Text = picker.ResourceID;
                }
            }
        }

        private void ClearExternalClassDropdowns()
        {
            cmbFeatureClass.DataSource = null;
            cmbKeyProperty.DataSource = null;
            cmbValueProperty.DataSource = null;
            btnUpdateThemeParameters.Enabled = false;
        }

        private void txtFeatureSource_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFeatureSource.Text))
            {
                var names = m_editor.FeatureService.GetClassNames(txtFeatureSource.Text, null);
                if (names.Length > 0)
                {
                    cmbFeatureClass.DataSource = names;
                    cmbFeatureClass.SelectedIndex = 0;
                }
                else
                {
                    ClearExternalClassDropdowns();
                }
            }
            else
            {
                ClearExternalClassDropdowns();
            }
        }

        private void cmbFeatureClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFeatureClass.SelectedItem != null)
            {
                var className = cmbFeatureClass.SelectedItem.ToString();
                var clsDef = m_editor.FeatureService.GetClassDefinition(txtFeatureSource.Text, className);

                var keyPropNames = new List<string>();
                var valuePropNames = new List<string>();

                foreach (var prop in clsDef.Properties)
                {
                    keyPropNames.Add(prop.Name);
                    valuePropNames.Add(prop.Name);
                }

                cmbKeyProperty.DataSource = keyPropNames;
                cmbValueProperty.DataSource = valuePropNames;

                cmbKeyProperty.SelectedIndex = 0;
                cmbValueProperty.SelectedIndex = 0;
                btnUpdateThemeParameters.Enabled = true;
            }
            else
            {
                btnUpdateThemeParameters.Enabled = false;
            }
        }

        private void btnUpdateThemeParameters_Click(object sender, EventArgs e)
        {
            string fsId = txtFeatureSource.Text;
            string className = cmbFeatureClass.SelectedItem.ToString();
            string key = cmbKeyProperty.SelectedItem.ToString();
            string value = cmbValueProperty.SelectedItem.ToString();
            string filter = null;
            if (!string.IsNullOrEmpty(txtFilter.Text))
                filter = txtFilter.Text;

            BusyWaitDialog.Run(Strings.ComputingThemeParameters, 
            () => { //Worker method
                List<LookupPair> res = new List<LookupPair>();
                using (var reader = m_editor.FeatureService.QueryFeatureSource(fsId, className, filter, new string[] { key, value }))
                {
                    while(reader.ReadNext())
                    {
                        if (!reader.IsNull(key) && !reader.IsNull(value))
                        {
                            res.Add(new LookupPair()
                            {
                                Key = reader[key].ToString(),
                                Value = reader[value].ToString()
                            });
                        }
                    }
                    reader.Close();
                }
                return res;
            },
            (res, ex) => { //Worker completion
                if (ex != null)
                {
                    ErrorDialog.Show(ex);
                }
                else
                {
                    m_lookupValues = (List<LookupPair>)res;

                    if (m_lookupValues.Count > MAX_INDIVIDUAL_THEME_RULES)
                    {
                        MessageBox.Show(this, string.Format(Strings.TooManyValuesError, MAX_INDIVIDUAL_THEME_RULES), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ColumnCombo.SelectedIndex = 0;
                        return;
                    }

                    RuleCountPanel.Enabled =
                    GroupPanel.Enabled =
                        false;

                    RuleCount.Minimum = 0;

                    //Select "Individual"
                    RuleCount.Value = m_lookupValues.Count;
                    GradientColors.Checked = true;

                    DisplayGroup.Enabled =
                    PreviewGroup.Enabled =
                    OKBtn.Enabled =
                        true;

                    RefreshColorBrewerSet();
                    RefreshPreview();

                    if (ColumnCombo.SelectedIndex == 0)
                    {
                        MessageBox.Show(Strings.ThemePrimaryKeyPropertyNotSelected);
                        OKBtn.Enabled = false;
                    }
                    else
                    {
                        OKBtn.Enabled = true;
                    }
                }
            });
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            string fsId = txtFeatureSource.Text;
            string className = cmbFeatureClass.SelectedItem.ToString();
            IFeatureSource fs = (IFeatureSource)m_editor.ResourceService.GetResource(fsId);
            ClassDefinition clsDef = m_editor.FeatureService.GetClassDefinition(fsId, className);
            string expr = m_editor.EditExpression(txtFilter.Text, clsDef, fs.Provider, fsId, false);
            if (expr != null)
                txtFilter.Text = expr;
        }
    }
}
