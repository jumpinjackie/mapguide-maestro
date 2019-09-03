#region Disclaimer / License

// Copyright (C) 2019, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

#endregion Disclaimer / License

using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI.Tile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Maestro.StaticMapPublisher.Common
{
    public class StaticMapPublisher : IProgress<TileProgress>
    {
        readonly TextWriter _stdout;

        public StaticMapPublisher(TextWriter stdout)
        {
            _stdout = stdout;
        }

        static string BuildUrlTemplate(IStaticMapPublishingOptions options, Func<IStaticMapPublishingOptions, string> getResource)
        {
            return $"{options.MapAgent}?USERNAME={options.Username}&PASSWORD={options.Password}&LOCALE=en&CLIENTAGENT=Maestro.StaticMapPublisher&OPERATION=GETTILEIMAGE&VERSION=1.2.0&MAPDEFINITION={getResource(options)}&SCALE={{z}}&ROW={{x}}&COL={{y}}";
        }

        static string GetImageTileSaveDirectory(IStaticMapPublishingOptions options, Func<IStaticMapPublishingOptions, string> getResource)
        {
            return Path.Combine(options.OutputDirectory,
                                getResource(options)
                                    .Replace(":", string.Empty)
                                    .Replace(".", string.Empty)
                                    .Replace("/", "_"));
        }

        public async Task<int> PublishAsync(IStaticMapPublishingOptions options)
        {
            string imgDir = null;
            string utfDir = null;

            if (!string.IsNullOrEmpty(options.ImageTileSetDefinition))
            {
                imgDir = GetImageTileSaveDirectory(options, o => o.ImageTileSetDefinition);
                Directory.CreateDirectory(imgDir);
            }
            if (!string.IsNullOrEmpty(options.UTFGridTileSetDefinition))
            {
                utfDir = GetImageTileSaveDirectory(options, o => o.UTFGridTileSetDefinition);
                Directory.CreateDirectory(utfDir);
            }
            var tileSvc = new XYZTileService(BuildUrlTemplate(options, o => o.ImageTileSetDefinition));
            var walker = new XYZTileWalker(options.Bounds.MinX, options.Bounds.MinY, options.Bounds.MaxX, options.Bounds.MaxY);
            var seedOpts = new TileSeederOptions();
            if (Directory.Exists(imgDir))
            {
                seedOpts.SaveTile = (tr, stream) =>
                {
                    var tilePath = Path.Combine(imgDir, tr.GroupName, $"{tr.Scale}" /* z */, $"{tr.Row}"  /* x */, $"{tr.Col}" /* y */);
                    using (var fw = File.OpenWrite(tilePath))
                    {
                        stream.CopyTo(fw);
                    }
                };
            }
            var seeder = new TileSeeder(tileSvc, walker, seedOpts);
            seeder.Run(this);

            tileSvc.SetUrlTemplate(BuildUrlTemplate(options, o => o.UTFGridTileSetDefinition));
            if (Directory.Exists(utfDir))
            {
                seedOpts.SaveTile = (tr, stream) =>
                {
                    var tilePath = Path.Combine(utfDir, tr.GroupName, $"{tr.Scale}" /* z */, $"{tr.Row}"  /* x */, $"{tr.Col}" /* y */);
                    using (var fw = File.OpenWrite(tilePath))
                    {
                        stream.CopyTo(fw);
                    }
                };
            }
            else
            {
                seedOpts.SaveTile = null;
            }
            seeder = new TileSeeder(tileSvc, walker, seedOpts);
            seeder.Run(this);
            return 0;
        }

        public void Report(TileProgress value)
        {
            var processed = value.Rendered + value.Failed;
            _stdout.WriteLine($"Processed ({processed}/{value.Total}) tiles [{((double)processed / (double)value.Total):P}] - {value.Rendered} rendered, {value.Failed} failed");
        }
    }
}
