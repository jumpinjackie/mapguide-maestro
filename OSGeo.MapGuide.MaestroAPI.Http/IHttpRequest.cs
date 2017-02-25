using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSGeo.MapGuide.MaestroAPI.Http
{
    internal interface IHttpRequest
    {
        byte[] DownloadDataSync(string req);

        Stream OpenReadSync(string req);

        void SendRequestSync(string req);
    }
}
