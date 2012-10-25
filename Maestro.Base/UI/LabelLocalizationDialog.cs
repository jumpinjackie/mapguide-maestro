#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ICSharpCode.Core;

namespace Maestro.Base.UI
{
    internal partial class LabelLocalizationDialog : Form
    {
        private LabelLocalizationDialog()
        {
            InitializeComponent();
        }

        private BindingList<LocalizableElement> _elements;

        public LabelLocalizationDialog(XmlDocument doc, string[] localizableTags)
            : this()
        {
            _elements = new BindingList<LocalizableElement>();
            var items = new Dictionary<string, LocalizableElement>();
            foreach (var tag in localizableTags)
            {
                XmlNodeList list = doc.GetElementsByTagName(tag);
                foreach (XmlNode node in list)
                {
                    if (items.ContainsKey(node.InnerText))
                    {
                        items[node.InnerText].AddNode(node);
                    }
                    else
                    {
                        var lel = new LocalizableElement(node);
                        items[node.InnerText] = lel;
                        _elements.Add(lel);
                    }
                }
            }
            grdStrings.DataSource = _elements;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            int translated = 0;
            foreach (var el in _elements)
            {
                translated += el.Apply();
            }
            MessageBox.Show(string.Format(Strings.ItemsTranslated, translated.ToString()));
            this.DialogResult = DialogResult.OK;
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _elements.Count; i++)
            {
                _elements[i].Translate = true;
            }
        }
    }

    internal class LocalizableElement : INotifyPropertyChanged
    {
        private List<XmlNode> _element;

        public LocalizableElement(XmlNode el)
        {
            _element = new List<XmlNode>();
            _element.Add(el);
            this.Label = el.InnerText;
            this.Translate = false;
        }

        public void AddNode(XmlNode node) { _element.Add(node); }

        private bool _translate;

        public bool Translate
        {
            get { return _translate; }
            set
            {
                if (value.Equals(_translate))
                    return;

                _translate = value;
                OnPropertyChanged("Translate"); //NOXLATE
            }
        }

        public string Label { get; private set; }

        private string _translation;

        public string Translation
        {
            get { return _translation; }
            set
            {
                if (value == _translation)
                    return;

                _translation = value;
                OnPropertyChanged("Translation"); //NOXLATE
            }
        }

        public int Apply()
        {
            int translated = 0;
            if (Translate && !string.IsNullOrEmpty(Translation))
            {
                foreach (var el in _element)
                {
                    el.InnerText = Translation;
                    translated++;
                }
            }
            return translated;
        }

        private void OnPropertyChanged(string name)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(name));
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
