using OSGeo.MapGuide.ObjectModels.SelectionModel;
using System.Windows.Forms;

namespace Maestro.MapViewer
{
    public partial class SelectionPanel : UserControl
    {
        public SelectionPanel()
        {
            InitializeComponent();
        }

        private FeatureInformation _fi;

        public FeatureInformation SelectedFeatureAttributes
        {
            get => _fi;
            set 
            { 
                _fi = value;
                propertyGrid1.SelectedObject = _fi.SelectedFeatures;
            }
        }
    }
}
