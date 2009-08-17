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

        private static readonly Type[] NUMERIC_TYPES = null;

        private bool m_isUpdating = false;

        static ThemeCreator ()
        {
            NUMERIC_TYPES = new Type[] { typeof(byte), typeof(int), typeof(float), typeof(double) };
        }

        public ThemeCreator(EditorInterface editor, MaestroAPI.LayerDefinition layer, MaestroAPI.FeatureSourceDescription.FeatureSourceSchema schema)
            : this()
        {
            m_editor = editor;
            m_layer = layer;
            m_schema = schema;
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

                //TODO: Try/catch
                using (MaestroAPI.FeatureSetReader rd = m_editor.CurrentConnection.QueryFeatureSource(m_layer.Item.ResourceId, m_schema.Fullname, null, new string[] { col.Name }))
                    while (rd.Read() && m_values.Count < 100000) //No more than 100.000 records in memory
                        if (!rd.Row.IsValueNull(col.Name))
                        {
                            object value = rd.Row[col.Name];
                            if (!m_values.ContainsKey(value))
                                m_values.Add(value, 0);

                            m_values[value]++;
                        }

                if (Array.IndexOf<Type>(NUMERIC_TYPES, col.Type) >= 0)
                {
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

                        RefreshColorBrewerSet();
                    }

                }
                else //String type
                {
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
                string prev = ColorBrewerColorSet.Text;
                ColorBrewerColorSet.Items.Clear();

                foreach (ColorBrewer cb in m_colorBrewer)
                    if (cb.Colors.Count == RuleCount.Value)
                        ColorBrewerColorSet.Items.Add(cb);

                if (ColorBrewerColorSet.FindString(prev) >= 0)
                    ColorBrewerColorSet.SelectedIndex = ColorBrewerColorSet.FindString(prev);
                else if (ColorBrewerColorSet.Items.Count > 0)
                    ColorBrewerColorSet.SelectedIndex = 0;

                if (ColorBrewerColorSet.Items.Count == 0)
                {
                    GradientColors.Checked = true;
                    ColorBrewerColors.Enabled = ColorBrewerColorSet.Enabled = false;
                }
                else
                {
                    ColorBrewerColors.Enabled = ColorBrewerColorSet.Enabled = true;
                    if (!GradientColors.Checked)
                        ColorBrewerColors.Checked = true;
                }
            }
            finally
            {
                m_isUpdating = false;
            }
        }

        private List<KeyValuePair<string, Color>> CalculateRuleSet()
        {
            Color[] colors = BuildColorSet(false);
            List<KeyValuePair<string, Color>> result = new List<KeyValuePair<string,Color>>();

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
                    double chunksize = mean / colors.Length;
                    for (int i = 0; i < colors.Length; i++)
                    {
                        long start = (long)(min + ((i - 1) * chunksize));
                        long end = (long)(min + (i * chunksize));
                        result.Add(new KeyValuePair<string, Color>(string.Format("\"{0}\" < {1}", ColumnCombo.Text, end), colors[i]));
                    }
                }
                else if (AggregateCombo.SelectedIndex == 1) //Standard Deviation
                {
                    double dev = 0;
                    foreach (KeyValuePair<object, long> entry in m_values)
                        dev += Convert.ToDouble(entry.Key) - mean;

                    dev /= count;
                    dev = Math.Sqrt(dev);

                    double lower = mean - (dev * (colors.Length + 1) / 2);

                    if (colors.Length % 2 == 1)
                        lower += dev / 2; //The middle item goes half an alpha to each side

                    result.Add(new KeyValuePair<string,Color>(string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" < {1}", ColumnCombo.Text, lower), colors[0]));

                    for (int i = 1; i < colors.Length - 1; i++)
                        result.Add(new KeyValuePair<string, Color>(string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" >= {1} AND \"{0}\" < {2}", ColumnCombo.Text, lower + ((i - 1) * dev), lower + (i * dev)), colors[i]));

                    result.Add(new KeyValuePair<string, Color>(string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" >= {1}", ColumnCombo.Text, lower + (dev * colors.Length)), colors[colors.Length - 1]));

                }
                else if (AggregateCombo.SelectedIndex == 2) //Quantile
                {
                    SortedDictionary<double, long> sort = new SortedDictionary<double, long>();
                    foreach (KeyValuePair<object, long> entry in m_values)
                        sort.Add(Convert.ToDouble(entry.Key), entry.Value);

                    double step = (1 / colors.Length) * count;
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

                    result.Add(new KeyValuePair<string,Color>(string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" < {1}", ColumnCombo.Text, separators[0]), colors[0]));

                    for (int i = 1; i < colors.Length - 1; i++)
                        result.Add(new KeyValuePair<string, Color>(string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" >= {1} AND \"{0}\" < {2}", ColumnCombo.Text, separators[i - 1], separators[i]), colors[i]));

                    result.Add(new KeyValuePair<string, Color>(string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" >= {1}", ColumnCombo.Text, separators[separators.Count - 1]), colors[colors.Length - 1]));
                }
            }
            else if (AggregateCombo.SelectedIndex == 3) //Individual
            {
                List<object> items = new List<object>(m_values.Keys);
                items.Sort(); //Handles types correctly

                for (int i = 0; i < colors.Length; i++)
                    if (items[i] is string)
                        result.Add(new KeyValuePair<string, Color>(string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" < '{1}'", ColumnCombo.Text, items[i]), colors[i]));
                    else
                        result.Add(new KeyValuePair<string,Color>(string.Format(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\" < {1}", ColumnCombo.Text, items[i]), colors[i])); 

            }


            return result;
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
                    int x = PREVIEW_ITEM_BOX_SPACING / 2;

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
            Color[] res = new Color[forPreview ? Math.Min((int)RuleCount.Value, 100) : m_values.Count];

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
                ColorBrewer c = (ColorBrewer)ColorBrewerColorSet.SelectedItem;
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
                RuleCount.Value = Math.Max(m_values.Count, RuleCount.Minimum);
                if (!GradientColors.Checked && ColorBrewerColorSet.Enabled)
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
                List<KeyValuePair<string, Color>> rules = CalculateRuleSet();
                MaestroAPI.VectorLayerDefinitionType vldef = m_layer.Item as MaestroAPI.VectorLayerDefinitionType;
                MaestroAPI.AreaTypeStyleType col = vldef.VectorScaleRange[0].Items[0] as MaestroAPI.AreaTypeStyleType;

                if (OverwriteRules.Checked)
                    col.AreaRule.Clear();

                foreach (KeyValuePair<string, Color> entry in rules)
                {
                    MaestroAPI.AreaRuleType r = new OSGeo.MapGuide.MaestroAPI.AreaRuleType();
                    r.Filter = r.LegendLabel = entry.Key;
                    r.Item = new OSGeo.MapGuide.MaestroAPI.AreaSymbolizationFillType();
                    r.Item.Fill.BackgroundColor = entry.Value;

                    col.AreaRule.Add(r);
                }
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

    }
}
