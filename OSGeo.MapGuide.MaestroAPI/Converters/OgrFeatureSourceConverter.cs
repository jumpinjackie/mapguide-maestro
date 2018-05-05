using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using System;
using System.Linq;

namespace OSGeo.MapGuide.MaestroAPI.Converters
{
    /// <summary>
    /// Converts a given feature source to an equivalent feature source using the OGR FDO provider
    /// </summary>
    public class OgrFeatureSourceConverter
    {
        readonly IFeatureSource _fs;
        readonly IResourceService _resSvc;
        readonly IFeatureService _featSvc;

        public OgrFeatureSourceConverter(IFeatureSource fs, IResourceService resSvc, IFeatureService featSvc)
        {
            _fs = fs;
            _resSvc = resSvc;
            _featSvc = featSvc;
        }

        public void Convert(string newFeatureSourceId)
        {
            //Source cannot be OGR
            var provider = _fs.Provider.ToUpper();
            switch (provider)
            {
                case "OSGEO.OGR":
                    throw new Exception(Strings.CannotConvertOgrProviderToItself);
                case "OSGEO.SHP":
                    ConvertSHP(newFeatureSourceId);
                    break;
                default:
                    throw new Exception(Strings.UnsupportedOgrProviderSourceProvider);
            }
        }

        private void ConvertSHP(string newFeatureSourceId)
        {
            _resSvc.CopyResource(_fs.ResourceID, newFeatureSourceId, true);
            var newFs = (IFeatureSource)_resSvc.GetResource(newFeatureSourceId);
            var path = newFs.GetConnectionProperty("DefaultFileLocation");
            newFs.Provider = "OSGeo.OGR";
            newFs.ClearConnectionProperties();
            newFs.SetConnectionProperty("DataSource", path);

            var provider = _featSvc.GetFeatureProvider("OSGeo.OGR");
            //Does it have the new default schema property? Use it for a more seamless transition
            //Otherwise it will default to "OGRSchema"
            if (provider.ConnectionProperties.Any(p => p.Name == "DefaultSchemaName"))
            {
                newFs.SetConnectionProperty("DefaultSchemaName", "Default");
            }

            _resSvc.SaveResource(newFs);
        }
    }
}
