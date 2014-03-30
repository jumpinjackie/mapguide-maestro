#region Disclaimer / License
// Copyright (C) 2014, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
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
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Maestro.Editors.LayerDefinition.Vector.GridEditor
{
    internal interface IRuleModel : INotifyPropertyChanged
    {
        [Browsable(false)]
        int Index { get; }
        string Filter { get; }
        string LegendLabel { get; set; }
        Image Style { get; }

        void SetRuleStylePreview(Image image);
        void SwapIndicesWith(IRuleModel model);
        void SetIndex(int index);
        object UnwrapRule();

        IRuleModel CloneRuleModel(ILayerElementFactory2 factory);
    }

    internal interface ILabeledRuleModel : INotifyPropertyChanged
    {
        [Browsable(false)]
        int Index { get; }
        string Filter { get; }
        string LegendLabel { get; set; }
        Image Style { get; }
        Image Label { get; }
        void SwapIndicesWith(ILabeledRuleModel model);
        void SetIndex(int index);
        object UnwrapRule();

        ILabeledRuleModel CloneLabeledRuleModel(ILayerElementFactory2 factory);
    }

    internal abstract class RuleModel : IRuleModel
    {
        [Browsable(false)]
        public int Index { get; protected set; }

        public abstract string Filter { get; set; }

        public abstract string LegendLabel { get; set; }

        public abstract Image Style { get; protected set; }

        public void SetIndex(int index) { this.Index = index; }

        [Browsable(false)]
        public bool HasStyle { get; protected set; }

        public void SetRuleStylePreview(Image preview)
        {
            if (this.Style != null)
                this.Style.Dispose();
            this.Style = preview;
        }

        protected void OnPropertyChanged(string name)
        {
            var h = this.PropertyChanged;
            if (h != null)
                h(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SwapIndicesWith(IRuleModel model)
        {
            var m = (RuleModel)model;
            var temp = m.Index;
            this.Index = m.Index;
            m.Index = temp;
        }

        public abstract object UnwrapRule();

        public abstract IRuleModel CloneRuleModel(ILayerElementFactory2 factory);
    }

    internal abstract class BasicVectorRuleModel<TRuleType, TSymbolizationStyleType> : RuleModel, ILabeledRuleModel where TRuleType : IBasicVectorRule
    {
        protected TRuleType _rule;

        protected BasicVectorRuleModel(TRuleType rule, int index)
        {
            _rule = rule;
            this.Index = index;
            UpdateLabelPreview(_rule.Label);
        }

        public override object UnwrapRule()
        {
            return _rule;
        }

        public override string Filter
        {
            get
            {
                return _rule.Filter;
            }
            set
            {
                if (value != _rule.Filter)
                {
                    _rule.Filter = value;
                    OnPropertyChanged("Filter");
                }
            }
        }

        public override string LegendLabel
        {
            get
            {
                return _rule.LegendLabel;
            }
            set
            {
                if (value != _rule.LegendLabel)
                {
                    _rule.LegendLabel = value;
                    OnPropertyChanged("LegendLabel");
                }
            }
        }

        private Image _style;

        public override Image Style
        {
            get
            {
                return _style;
            }
            protected set
            {
                if (value != _style)
                {
                    _style = value;
                    OnPropertyChanged("Style");
                }
            }
        }

        protected void UpdateLabelPreview(ITextSymbol style)
        {
            if (style != null)
            {
                Image img = new Bitmap(80, 20);
                using (Graphics g = Graphics.FromImage(img))
                {
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    FeaturePreviewRender.RenderPreviewFont(g, new Rectangle(new Point(0, 0), img.Size), style);
                }
                this.SetLabelStylePreview(img);
                this.HasLabel = true;
            }
            else
            {
                this.SetLabelStylePreview(FeaturePreviewRender.RenderNullLabel(80, 20));
                this.HasLabel = false;
            }
        }

        private Image _label;

        [Browsable(true)]
        public Image Label
        {
            get
            {
                return _label;
            }
            protected set
            {
                if (value != _label)
                {
                    _label = value;
                    OnPropertyChanged("Label");
                }
            }
        }

        [Browsable(false)]
        public bool HasLabel { get; protected set; }

        public void SetLabelStylePreview(Image preview)
        {
            if (this.Label != null)
                this.Label.Dispose();
            this.Label = preview;
        }

        public abstract TSymbolizationStyleType GetSymbolizationStyle();

        public abstract void SetSymbolizationStyle(TSymbolizationStyleType style);

        public ITextSymbol GetLabelStyle()
        {
            return _rule.Label;
        }

        public void SetLabelStyle(ITextSymbol style)
        {
            _rule.Label = style;
            UpdateLabelPreview(style);
        }

        public void SwapIndicesWith(ILabeledRuleModel model)
        {
            var m = (BasicVectorRuleModel<TRuleType, TSymbolizationStyleType>)model;
            var temp = m.Index;
            this.Index = m.Index;
            m.Index = temp;
        }

        public override IRuleModel CloneRuleModel(ILayerElementFactory2 factory)
        {
            throw new NotImplementedException();
        }

        public abstract ILabeledRuleModel CloneLabeledRuleModel(ILayerElementFactory2 factory);
    }

    internal class PointRuleModel : BasicVectorRuleModel<IPointRule, IPointSymbolization2D>
    {
        public PointRuleModel(IPointRule rule, int index)
            : base(rule, index)
        {
            this.HasStyle = (_rule.PointSymbolization2D != null);
        }

        public override IPointSymbolization2D GetSymbolizationStyle()
        {
            return _rule.PointSymbolization2D;
        }

        public override void SetSymbolizationStyle(IPointSymbolization2D style)
        {
            _rule.PointSymbolization2D = style;
            this.HasStyle = (style != null);
        }

        public override ILabeledRuleModel CloneLabeledRuleModel(ILayerElementFactory2 factory)
        {
            var clone = factory.CreateDefaultPointRule();
            clone.Filter = _rule.Filter;
            clone.LegendLabel = _rule.LegendLabel;
            if (_rule.Label != null)
                clone.Label = _rule.Label.Clone();
            if (_rule.PointSymbolization2D != null)
                clone.PointSymbolization2D = _rule.PointSymbolization2D.Clone();
            return new PointRuleModel(clone, -1);
        }
    }

    internal class BasicLineSymbolizationAdapter : ILineRule
    {
        private ILineRule _wrappee;

        public BasicLineSymbolizationAdapter(ILineRule wrappee)
        {
            _wrappee = wrappee;
        }

        public int StrokeCount
        {
            get { return _wrappee.StrokeCount; }
        }

        public IEnumerable<IStroke> Strokes
        {
            get { return _wrappee.Strokes; }
        }

        public void SetStrokes(IEnumerable<IStroke> strokes)
        {
            _wrappee.SetStrokes(strokes);
        }

        public void AddStroke(IStroke stroke)
        {
            _wrappee.AddStroke(stroke);
        }

        public void RemoveStroke(IStroke stroke)
        {
            _wrappee.RemoveStroke(stroke);
        }

        public ITextSymbol Label
        {
            get
            {
                return _wrappee.Label;
            }
            set
            {
                _wrappee.Label = value;
            }
        }

        public string LegendLabel
        {
            get
            {
                return _wrappee.LegendLabel;
            }
            set
            {
                _wrappee.LegendLabel = value;
            }
        }

        public string Filter
        {
            get
            {
                return _wrappee.Filter;
            }
            set
            {
                _wrappee.Filter = value;
            }
        }
    }

    internal class LineRuleModel : BasicVectorRuleModel<ILineRule, BasicLineSymbolizationAdapter>
    {
        public LineRuleModel(ILineRule rule, int index)
            : base(rule, index)
        {
            this.HasStyle = (_rule.Strokes != null && _rule.StrokeCount > 0);
        }

        public override void SetSymbolizationStyle(BasicLineSymbolizationAdapter style)
        {
            //Do nothing, whatever changes have already been applied to the wrapped instance
            //by the adapter that's returned
            this.HasStyle = (_rule.Strokes != null && _rule.StrokeCount > 0);
        }

        public override BasicLineSymbolizationAdapter GetSymbolizationStyle()
        {
            return new BasicLineSymbolizationAdapter(_rule);
        }

        public override ILabeledRuleModel CloneLabeledRuleModel(ILayerElementFactory2 factory)
        {
            var clone = factory.CreateDefaultLineRule();
            clone.Filter = _rule.Filter;
            clone.LegendLabel = _rule.LegendLabel;
            if (_rule.Label != null)
                clone.Label = _rule.Label.Clone();
            if (_rule.Strokes != null && _rule.StrokeCount > 0)
            {
                var strokes = new List<IStroke>();
                foreach (var st in _rule.Strokes)
                {
                    strokes.Add(st.Clone());
                }
                clone.SetStrokes(strokes);
            }
            return new LineRuleModel(clone, -1);
        }
    }

    internal class AreaRuleModel : BasicVectorRuleModel<IAreaRule, IAreaSymbolizationFill>
    {
        public AreaRuleModel(IAreaRule rule, int index)
            : base(rule, index)
        {
            this.HasStyle = (_rule.AreaSymbolization2D != null);
        }

        public override IAreaSymbolizationFill GetSymbolizationStyle()
        {
            return _rule.AreaSymbolization2D;
        }

        public override void SetSymbolizationStyle(IAreaSymbolizationFill style)
        {
            _rule.AreaSymbolization2D = style;
            this.HasStyle = (style != null);
        }

        public override ILabeledRuleModel CloneLabeledRuleModel(ILayerElementFactory2 factory)
        {
            var clone = factory.CreateDefaultAreaRule();
            clone.Filter = _rule.Filter;
            clone.LegendLabel = _rule.LegendLabel;
            if (_rule.Label != null)
                clone.Label = _rule.Label.Clone();
            if (_rule.AreaSymbolization2D != null)
                clone.AreaSymbolization2D = _rule.AreaSymbolization2D.Clone();
            return new AreaRuleModel(clone, -1);
        }
    }
    
    internal class CompositeRuleModel : RuleModel
    {
        private ICompositeRule _rule;

        public CompositeRuleModel(ICompositeRule rule, int index)
        {
            _rule = rule;
            this.Index = index;
            this.HasStyle = (_rule.CompositeSymbolization != null);
        }

        public override object UnwrapRule()
        {
            return _rule;
        }

        public ICompositeSymbolization GetSymbolizationStyle()
        {
            return _rule.CompositeSymbolization;
        }

        public void SetSymbolizationStyle(ICompositeSymbolization style)
        {
            _rule.CompositeSymbolization = style;
            this.HasStyle = (style != null);
        }

        public override string Filter
        {
            get
            {
                return _rule.Filter;
            }
            set
            {
                if (value != _rule.Filter)
                {
                    _rule.Filter = value;
                    OnPropertyChanged("Filter");
                }
            }
        }

        public override string LegendLabel
        {
            get
            {
                return _rule.LegendLabel;
            }
            set
            {
                if (value != _rule.LegendLabel)
                {
                    _rule.LegendLabel = value;
                    OnPropertyChanged("LegendLabel");
                }
            }
        }

        private Image _style;

        public override Image Style
        {
            get
            {
                return _style;
            }
            protected set
            {
                if (value != _style)
                {
                    _style = value;
                    OnPropertyChanged("Style");
                }
            }
        }

        public override IRuleModel CloneRuleModel(ILayerElementFactory2 factory)
        {
            var clone = factory.CreateDefaultCompositeRule();
            clone.Filter = _rule.Filter;
            clone.LegendLabel = _rule.LegendLabel;
            if (_rule.CompositeSymbolization != null)
                clone.CompositeSymbolization = factory.CloneCompositeSymbolization(_rule.CompositeSymbolization);
            return new CompositeRuleModel(clone, -1);
        }
    }
}
