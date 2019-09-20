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

using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Tile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Maestro.MapPublisher.Common
{
    public class StaticMapPublisher : IProgress<TileProgress>
    {
        readonly TextWriter _stdout;

        public StaticMapPublisher(TextWriter stdout)
        {
            _stdout = stdout;
        }

        static string BuildUrlTemplate(IStaticMapPublishingOptions options, Func<IStaticMapPublishingOptions, string> getResource, Func<IStaticMapPublishingOptions, string> getGroupName)
        {
            return $"{options.MapAgent}?USERNAME={options.Username}&PASSWORD={options.Password}&LOCALE=en&CLIENTAGENT=Maestro.MapPublisher&OPERATION=GETTILEIMAGE&VERSION=1.2.0&MAPDEFINITION={getResource(options)}&BASEMAPLAYERGROUPNAME={getGroupName(options)}&SCALEINDEX={{z}}&TILEROW={{x}}&TILECOL={{y}}";
        }

        public static string GetResourceRelPath(IStaticMapPublishingOptions options, Func<IStaticMapPublishingOptions, string> getResource)
        {
            return getResource(options)
                ?.Replace(".TileSetDefinition", string.Empty)
                ?.Replace("Library://", string.Empty)
                ?.Replace(".", string.Empty)
                ?.Replace("/", "_");
        }

        static string GetImageTileSaveDirectory(IStaticMapPublishingOptions options, Func<IStaticMapPublishingOptions, string> getResource)
        {
            return Path.Combine(options.OutputDirectory, GetResourceRelPath(options, getResource));
        }

        private DateTime _tileStart;

        public async Task<int> PublishAsync(IStaticMapPublishingOptions options)
        {
            _tileStart = DateTime.UtcNow;
            string imgDir = null;
            string utfDir = null;

            if (!string.IsNullOrEmpty(options.ImageTileSet?.ResourceID))
            {
                imgDir = GetImageTileSaveDirectory(options, o => o.ImageTileSet.ResourceID);
                Directory.CreateDirectory(imgDir);
            }
            if (!string.IsNullOrEmpty(options.UTFGridTileSet?.ResourceID))
            {
                utfDir = GetImageTileSaveDirectory(options, o => o.UTFGridTileSet.ResourceID);
                Directory.CreateDirectory(utfDir);
            }

            if (!string.IsNullOrEmpty(options.ImageTileSet?.ResourceID))
            {
                var tileSvc = new XYZTileService(BuildUrlTemplate(options, o => o.ImageTileSet.ResourceID, o => o.ImageTileSet.GroupName));
                var walker = new XYZTileWalker(options.Bounds.MinX, options.Bounds.MinY, options.Bounds.MaxX, options.Bounds.MaxY);
                var seedOpts = new TileSeederOptions
                {
                    MaxDegreeOfParallelism = options.MaxDegreeOfParallelism
                };
                if (Directory.Exists(imgDir))
                {
                    seedOpts.SaveTile = (tr, stream) =>
                    {
                        var tilePath = Path.Combine(imgDir, tr.GroupName, $"{tr.Scale}" /* z */, $"{tr.Row}"  /* x */, $"{tr.Col}.png" /* y */);
                        var parentDir = Path.GetDirectoryName(tilePath);
                        if (!Directory.Exists(parentDir))
                            Directory.CreateDirectory(parentDir);
                        using (var fw = File.OpenWrite(tilePath))
                        {
                            Utility.CopyStream(stream, fw);
                        }
                    };
                }
                else
                {
                    seedOpts.SaveTile = null;
                }
                seedOpts.ErrorLogger = (tr, ex) =>
                {

                };

                var seeder = new TileSeeder(tileSvc, walker, seedOpts);
                seeder.RandomizeRequests = options.RandomizeRequests;
                await seeder.RunAsync(this);
            }

            var imageElapsed = DateTime.UtcNow - _tileStart;
            _tileStart = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(options.UTFGridTileSet?.ResourceID))
            {
                var tileSvc = new XYZTileService(BuildUrlTemplate(options, o => o.UTFGridTileSet.ResourceID, o => o.UTFGridTileSet.GroupName));
                var walker = new XYZTileWalker(options.Bounds.MinX, options.Bounds.MinY, options.Bounds.MaxX, options.Bounds.MaxY);
                var seedOpts = new TileSeederOptions
                {
                    MaxDegreeOfParallelism = options.MaxDegreeOfParallelism
                };
                if (Directory.Exists(utfDir))
                {
                    seedOpts.SaveTile = (tr, stream) =>
                    {
                        var tilePath = Path.Combine(utfDir, tr.GroupName, $"{tr.Scale}" /* z */, $"{tr.Row}"  /* x */, $"{tr.Col}.json" /* y */);
                        var parentDir = Path.GetDirectoryName(tilePath);
                        if (!Directory.Exists(parentDir))
                            Directory.CreateDirectory(parentDir);
                        using (var fw = File.OpenWrite(tilePath))
                        {
                            Utility.CopyStream(stream, fw);
                        }
                    };
                }
                else
                {
                    seedOpts.SaveTile = null;
                }
                seedOpts.ErrorLogger = (tr, ex) =>
                {

                };
                var seeder = new TileSeeder(tileSvc, walker, seedOpts);
                seeder.RandomizeRequests = options.RandomizeRequests;
                await seeder.RunAsync(this);
            }

            var utfElapsed = DateTime.UtcNow - _tileStart;
            _stdout.WriteLine($"Image tiles downloaded in {(int)imageElapsed.TotalSeconds}s");
            _stdout.WriteLine($"UTFGrid tiles downloaded in {(int)utfElapsed.TotalSeconds}s");
            return 0;
        }

        public void Report(TileProgress value)
        {
            //NOTE: This method is being called at a resolution such that using DateTime 
            //subtraction is an acceptable means of measuring elapsed duration
            var elapsed = DateTime.UtcNow - _tileStart;
            var processed = value.Rendered + value.Failed;
            _stdout.WriteLine($"Processed ({processed}/{value.Total}) tiles [{((double)processed / (double)value.Total):P}] - {value.Rendered} rendered, {value.Failed} failed ({(int)elapsed.TotalSeconds}s elapsed)");
        }
    }
}
