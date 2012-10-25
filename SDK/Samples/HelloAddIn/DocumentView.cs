using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Maestro.Shared.UI;

namespace HelloAddIn
{
    public partial class DocumentView : ViewContentBase
    {
        static int counter = 0;

        public DocumentView()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Title = "Document " + (++counter);
        }

        public override ViewRegion DefaultRegion
        {
            get
            {
                return ViewRegion.Document;
            }
        }
    }
}
