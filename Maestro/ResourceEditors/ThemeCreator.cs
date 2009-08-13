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

                    if (m_values.Count <= 9)
                    {
                        AggregateCombo.SelectedIndex = AggregateCombo.Items.Count - 1;
                        RuleCount.Value = m_values.Count;
                        if (!GradientColors.Checked)
                            ColorBrewerColors.Checked = true;

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
            string prev = ColorBrewerColorSet.Text;
            ColorBrewerColorSet.Items.Clear();

            string cbtype = null;
            switch (AggregateCombo.SelectedIndex)
            {
                case 0:
                    cbtype = "qual";
                    break;
                case 1:
                    cbtype = "seq";
                    break;
                case 2:
                    cbtype = "div";
                    break;
            }

            if (cbtype == null)
                return;

            foreach(ColorBrewer cb in m_colorBrewer)
                if (cb.Colors.Count == RuleCount.Value && cb.Type.ToLower().Trim() == cbtype)
                    ColorBrewerColorSet.Items.Add(cb);

            if (ColorBrewerColorSet.FindString(prev) >= 0)
                ColorBrewerColorSet.SelectedIndex = ColorBrewerColorSet.FindString(prev);
            else
                ColorBrewerColorSet.SelectedIndex = 0;
        }


        private void RefreshPreview()
        {
            Bitmap bmp = new Bitmap(PreviewPicture.Width, PreviewPicture.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {

                //TODO: Build list of colors
                List<Color?> colors = new List<Color?>();
                Color[] actualColors = BuildColorSet();
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

        private Color[] BuildColorSet()
        {
            Color[] res = new Color[(int)RuleCount.Value];

            if (GradientColors.Checked)
            {
                double stepR, stepG, stepB;
                stepR = (GradientToColor.CurrentColor.R - GradientFromColor.CurrentColor.R) / (double)RuleCount.Value;
                stepG = (GradientToColor.CurrentColor.G - GradientFromColor.CurrentColor.G) / (double)RuleCount.Value;
                stepB = (GradientToColor.CurrentColor.B - GradientFromColor.CurrentColor.B) / (double)RuleCount.Value;

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
            ColorBrewerColors.Enabled = ColorBrewerColorSet.Enabled = 
                AggregateCombo.SelectedIndex != AggregateCombo.Items.Count - 1;

            if (!ColorBrewerColors.Enabled)
                GradientColors.Checked = true;
            else
                RefreshColorBrewerSet();

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
            RefreshPreview();
        }

    }
}
