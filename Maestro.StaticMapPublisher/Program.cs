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

using CommandLine;
using Maestro.StaticMapPublisher.Common;
using Newtonsoft.Json;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Maestro.StaticMapPublisher
{
    [Verb("publish")]
    public class PublishOptions
    {
        [Option("wait", Default = false)]
        public bool Wait { get; set; }

        [Option("publish-profile-path", Required = true, HelpText = "The path of the publish profile")]
        public string PublishProfilePath { get; set; }

        public IStaticMapPublishingOptions PublishingOptions { get; set; }

        public void Validate(TextWriter stdout)
        {
            if (!File.Exists(this.PublishProfilePath))
                throw new Exception("Specified publish profile not found");

            stdout.WriteLine($"Loading publishing profile from: {this.PublishProfilePath}");

            var content = File.ReadAllText(this.PublishProfilePath);
            var pp = JsonConvert.DeserializeObject<PublishProfile>(content);

            pp.Validate(stdout);
            this.PublishingOptions = pp;
        }
    }


    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var result = Parser
                .Default
                .ParseArguments<PublishOptions>(args)
                .MapResult(opts => Run(opts), _ => Task.FromResult(1));
            var retCode = await result;
            return retCode;
        }

        static async Task<int> Run(object arg)
        {
            var stdout = Console.Out;
            try
            {
                switch (arg)
                {
                    case PublishOptions po:
                        {
                            po.Validate(stdout);

                            var pubOpts = po.PublishingOptions;
                            var pub = new Maestro.StaticMapPublisher.Common.StaticMapPublisher(stdout);
                            //var ret = await pub.PublishAsync(pubOpts);
                            var ret = 0;
                            var bounds = pubOpts.Bounds;

                            // Generate index.html
                            var vm = new MapViewerModel
                            {
                                Title = pubOpts.Title,
                                UTFGridRelPath = Common.StaticMapPublisher.GetResourceRelPath(pubOpts, o => o.UTFGridTileSetDefinition),
                                XYZImageRelPath = Common.StaticMapPublisher.GetResourceRelPath(pubOpts, o => o.ImageTileSetDefinition),
                                LatLngBounds = new [] { bounds.MinX, bounds.MinY, bounds.MaxX, bounds.MaxY },
                                ExternalBaseLayers = pubOpts.ExternalBaseLayers
                            };

                            string result;
                            switch (pubOpts.Viewer)
                            {
                                case ViewerType.OpenLayers:
                                    {
                                        string template = File.ReadAllText("viewer_content/viewer_ol.cshtml");
                                        result = Engine.Razor.RunCompile(template, "templateKey", null, vm);
                                    }
                                    break;
                                case ViewerType.Leaflet:
                                    {
                                        string template = File.ReadAllText("viewer_content/viewer_leaflet.cshtml");
                                        result = Engine.Razor.RunCompile(template, "templateKey", null, vm);
                                    }
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException("Unknown or unsupported viewer type");
                            }

                            var outputHtmlPath = Path.Combine(pubOpts.OutputDirectory, "index.html");
                            File.WriteAllText(outputHtmlPath, result);
                            stdout.WriteLine($"Written: {outputHtmlPath}");

                            // Copy assets
                            var assetsDir = Path.Combine(pubOpts.OutputDirectory, "assets");
                            if (!Directory.Exists(assetsDir))
                            {
                                Directory.CreateDirectory(assetsDir);
                            }
                            var files = Directory.GetFiles("viewer_content/assets", "*", SearchOption.AllDirectories);
                            foreach (var f in files)
                            {
                                var fileName = f.Substring("viewer_content/assets".Length).Trim('\\', '/'); //Path.GetFileName(f);
                                var targetFileName = Path.GetFullPath(Path.Combine(assetsDir, fileName));
                                var targetParentDir = Path.GetDirectoryName(targetFileName);
                                if (!Directory.Exists(targetParentDir))
                                    Directory.CreateDirectory(targetParentDir);
                                File.Copy(f, targetFileName, true);
                                stdout.WriteLine($"Copied to assets: {targetFileName}");
                            }

                            if (po.Wait)
                            {
                                stdout.WriteLine("Press any key to continue");
                                Console.Read();
                            }

                            return ret;
                        }
                    default:
                        throw new ArgumentException();
                }
            }
            catch (Exception ex)
            {
                stdout.WriteLine($"ERROR: {ex}");
                return 1;
            }
        }
    }
}
