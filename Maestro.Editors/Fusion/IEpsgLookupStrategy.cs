using OSGeo.MapGuide.MaestroAPI.Http;

namespace Maestro.Editors.Fusion
{
    public interface IEpsgLookupStrategy
    {
        CustomProjectionEntry TryGet(IHttpRequestor http, string epsg);
    }

    public class SpatialReferenceOrgLookupStrategy : IEpsgLookupStrategy
    {
        public CustomProjectionEntry TryGet(IHttpRequestor http, string epsg)
        {
            throw new System.NotImplementedException();
        }
    }
}
