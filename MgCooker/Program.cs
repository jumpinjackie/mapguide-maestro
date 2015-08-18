#region Disclaimer / License

// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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

using Duplicati.CommandLine;
using Maestro.Login;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Tile;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.Common;
using System;
using System.Collections.Generic;

namespace MgCooker
{
    public class Program
    {
        private static DateTime beginMap;
        private static DateTime beginGroup;
        private static DateTime beginScale;
        private static DateTime beginTile;
        private static DateTime lastUpdate;

        private static List<Exception> exceptionList = new List<Exception>();
        private static List<TimeSpan> tileRuns;
        private static long tileCount;
        private static long totalTiles;
        private static TimeSpan prevDuration;

        private static long mapCount;
        private static long groupCount;

        private static bool m_loggableProgress = false;

        private static bool hasConsole = true;

        [STAThread]
        public static void Main(string[] args)
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.DoEvents();
            PreferredSiteList.InitCulture();
            //Parameters:
            //mapagent=
            //username=
            //password=
            //mapdefinition=
            //scaleindex=0,1,2,3,4,5
            //basegroups="x","y"
            //extentoverride=minx,miny,maxx,maxy

            bool cmdLineMode = false;

            string mapdefinitions = string.Empty;
            string scaleindex = string.Empty;
            string basegroups = string.Empty;

            string limitRows = string.Empty;
            string limitCols = string.Empty;

            string tileWidth = string.Empty;
            string tileHeight = string.Empty;

            string DPI = string.Empty;
            string metersPerUnit = string.Empty;

            IEnvelope overrideExtents = null;

            List<string> largs = new List<string>(args);
            Dictionary<string, string> opts = CommandLineParser.ExtractOptions(largs);
            if (opts.ContainsKey(TileRunParameters.MAPDEFINITIONS))
                mapdefinitions = opts[TileRunParameters.MAPDEFINITIONS];
            if (opts.ContainsKey(TileRunParameters.SCALEINDEX))
                scaleindex = opts[TileRunParameters.SCALEINDEX];
            if (opts.ContainsKey(TileRunParameters.BASEGROUPS))
                basegroups = opts[TileRunParameters.BASEGROUPS];

            if (opts.ContainsKey(TileRunParameters.LIMITROWS))
                limitRows = opts[TileRunParameters.LIMITROWS];
            if (opts.ContainsKey(TileRunParameters.LIMITCOLS))
                limitCols = opts[TileRunParameters.LIMITCOLS];

            if (opts.ContainsKey(TileRunParameters.TILEWIDTH))
                tileWidth = opts[TileRunParameters.TILEWIDTH];
            if (opts.ContainsKey(TileRunParameters.TILEHEIGHT))
                tileHeight = opts[TileRunParameters.TILEHEIGHT];

            if (opts.ContainsKey(TileRunParameters.DOTSPERINCH))
                DPI = opts[TileRunParameters.DOTSPERINCH];
            if (opts.ContainsKey(TileRunParameters.METERSPERUNIT))
                metersPerUnit = opts[TileRunParameters.METERSPERUNIT];
            if (opts.ContainsKey(TileRunParameters.EXTENTOVERRIDE))
            {
                string[] parts = opts[TileRunParameters.EXTENTOVERRIDE].Split(',');
                if (parts.Length == 4)
                {
                    double minx;
                    double miny;
                    double maxx;
                    double maxy;
                    if (
                        double.TryParse(parts[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out minx) &&
                        double.TryParse(parts[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out miny) &&
                        double.TryParse(parts[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out maxx) &&
                        double.TryParse(parts[3], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out maxy)
                        )
                    {
                        overrideExtents = ObjectFactory.CreateEnvelope(minx, miny, maxx, maxy);
                    }
                }
            }

            if (largs.IndexOf("batch") >= 0 || largs.IndexOf("/batch") >= 0 || largs.IndexOf("commandline") >= 0 || largs.IndexOf("/commandline") >= 0)
            {
                cmdLineMode = true;
            }

            try
            {
                Console.Clear();
                m_loggableProgress = true;
            }
            catch
            {
                hasConsole = false;
                m_loggableProgress = false;
            }

            IServerConnection connection = null;

            string[] maps = mapdefinitions.Split(',');

            string username = string.Empty;
            string password = string.Empty;
            if (opts.ContainsKey(TileRunParameters.PROVIDER) && opts.ContainsKey(TileRunParameters.CONNECTIONPARAMS))
            {
                var initP = ConnectionProviderRegistry.ParseConnectionString(opts[TileRunParameters.CONNECTIONPARAMS]);
                connection = ConnectionProviderRegistry.CreateConnection(opts[TileRunParameters.PROVIDER], initP);
            }
            else
            {
                if (cmdLineMode)
                {
                    throw new ArgumentException(string.Format(Strings.MissingRequiredConnectionParameters, TileRunParameters.PROVIDER, TileRunParameters.CONNECTIONPARAMS));
                }

                var frm = new LoginDialog();
                if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;

                username = frm.Username;
                password = frm.Password;

                connection = frm.Connection;
            }

            if (connection == null)
            {
                if (opts.ContainsKey(TileRunParameters.PROVIDER) && opts.ContainsKey(TileRunParameters.CONNECTIONPARAMS))
                {
                    var initP = ConnectionProviderRegistry.ParseConnectionString(opts[TileRunParameters.CONNECTIONPARAMS]);
                    connection = ConnectionProviderRegistry.CreateConnection(opts[TileRunParameters.PROVIDER], initP);
                }
                else
                {
                    throw new ArgumentException(string.Format(Strings.MissingRequiredConnectionParameters, TileRunParameters.PROVIDER, TileRunParameters.CONNECTIONPARAMS));
                }
            }

            if (!cmdLineMode)
            {
                SetupRun sr = null;
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                    sr = new SetupRun(username, password, connection, maps, opts);
                else
                    sr = new SetupRun(connection, maps, opts);

                using (sr)
                {
                    sr.ShowDialog();
                    return;
                }
            }

            TilingRunCollection bx = new TilingRunCollection(connection);

            int x;

            if (!string.IsNullOrEmpty(limitCols) && int.TryParse(limitCols, out x))
                bx.LimitCols(x);
            if (!string.IsNullOrEmpty(limitRows) && int.TryParse(limitRows, out x))
                bx.LimitRows(x);

            if (!string.IsNullOrEmpty(tileWidth) && int.TryParse(tileWidth, out x))
                bx.Config.TileWidth = x;
            if (!string.IsNullOrEmpty(tileHeight) && int.TryParse(tileHeight, out x))
                bx.Config.TileHeight = x;

            if (!string.IsNullOrEmpty(DPI) && int.TryParse(DPI, out x))
                bx.Config.DPI = x;

            double d;
            if (!string.IsNullOrEmpty(metersPerUnit) && double.TryParse(metersPerUnit, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentCulture, out d))
            {
                bx.Config.MetersPerUnit = d;
            }

            if (opts.ContainsKey(TileRunParameters.RANDOMTILEORDER))
                bx.Config.RandomizeTileSequence = true;

            if (opts.ContainsKey(TileRunParameters.THREADCOUNT) && int.TryParse(opts[TileRunParameters.THREADCOUNT], out x) && x > 0)
                bx.Config.ThreadCount = x;

            //Now that all global parameters are set, we can now add the map definitions
            bx.AddMapDefinitions(maps);

            //basegroups must be set in each mapdefinition
            if (!string.IsNullOrEmpty(basegroups))
            {
                List<string> groups = new List<string>();
                foreach (string s in basegroups.Split(','))
                {
                    string f = s;
                    if (f.StartsWith("\""))
                        f = f.Substring(1);
                    if (f.EndsWith("\""))
                        f = f.Substring(0, f.Length - 1);
                    groups.Add(f);
                }
                bx.SetGroups(groups.ToArray());
            }

            if (overrideExtents != null)
            {
                List<int> scales = new List<int>();
                int tmp;
                foreach (string s in scaleindex.Split(','))
                    if (int.TryParse(s, out tmp))
                        scales.Add(tmp);
                foreach (MapTilingConfiguration bm in bx.Maps)
                    bm.SetScalesAndExtend(scales.ToArray(), overrideExtents);
            }
            else if (!string.IsNullOrEmpty(scaleindex))
            {
                List<int> scales = new List<int>();
                int tmp;
                foreach (string s in scaleindex.Split(','))
                    if (int.TryParse(s, out tmp))
                        scales.Add(tmp);
                bx.SetScales(scales.ToArray());
            }

            if (!cmdLineMode)
            {
                Progress pg = new Progress(bx);
                pg.ShowDialog();
            }
            else
            {
                bx.BeginRenderingMap += OnBeginRenderingMap;
                bx.FinishRenderingMap += OnFinishRenderingMap;
                bx.BeginRenderingGroup += OnBeginRenderingGroup;
                bx.FinishRenderingGroup += OnFinishRenderingGroup;
                bx.BeginRenderingScale += OnBeginRenderingScale;
                bx.FinishRenderingScale += OnFinishRenderingScale;
                bx.BeginRenderingTile += OnBeginRenderingTile;
                bx.FinishRenderingTile += OnFinishRenderingTile;
                bx.FailedRenderingTile += OnFailedRenderingTile;

                mapCount = 0;
                lastUpdate = DateTime.Now;

                bx.RenderAll();
            }
        }

        private static void OnFailedRenderingTile(object sender, TileRenderingErrorEventArgs args)
        {
            if (args.Error != null)
            {
                exceptionList.Add(args.Error);
                args.Error = null;
            }
        }

        private static void DisplayProgress(object sender, TileProgressEventArgs args)
        {
            if (hasConsole)
                Console.Clear();

            var map = args.Map;
            var group = args.Group;
            var scaleindex = args.ScaleIndex;

            Console.WriteLine(string.Format(Strings.ConsoleUpdateTime.Replace("\\t", "\t"), DateTime.Now));
            Console.WriteLine(string.Format(Strings.ConsoleCurrentMap.Replace("\\t", "\t"), map.ResourceId, mapCount, map.Parent.Maps.Count));
            Console.WriteLine(string.Format(Strings.ConsoleCurrentGroup.Replace("\\t", "\t"), group, groupCount, map.TileSet.GroupCount));
            Console.WriteLine(string.Format(Strings.ConsoleCurrentScale.Replace("\\t", "\t"), map.TileSet.GetScaleAt(Array.IndexOf<int>(map.ScaleIndexMap, scaleindex)), Array.IndexOf<int>(map.ScaleIndexMap, scaleindex) + 1, map.TileSet.ScaleCount));
            Console.WriteLine(string.Format(Strings.ConsoleCurrentTile.Replace("\\t", "\t"), tileCount, totalTiles));
            Console.WriteLine();
            Console.WriteLine(string.Format(Strings.ConsoleGroupDuration.Replace("\\t", "\t"), DateTime.Now - beginGroup));
            Console.WriteLine(string.Format(Strings.ConsoleGroupEstimate.Replace("\\t", "\t"), new TimeSpan(prevDuration.Ticks * totalTiles)));

            if (exceptionList.Count != 0)
            {
                Console.WriteLine();
                Console.WriteLine(string.Format(Strings.ConsoleErrorSummary, exceptionList.Count, exceptionList[exceptionList.Count - 1].ToString()));
            }
        }

        private static void OnFinishRenderingGroup(object sender, TileProgressEventArgs args)
        {
            TimeSpan duration = DateTime.Now - beginGroup;
            if (m_loggableProgress)
                Console.WriteLine(string.Format(Strings.ConsoleOperationFinishGroup, DateTime.Now, args.Group, duration));
        }

        private static void OnBeginRenderingGroup(object sender, TileProgressEventArgs args)
        {
            groupCount++;
            beginGroup = DateTime.Now;

            if (m_loggableProgress)
                Console.WriteLine(string.Format(Strings.ConsoleOperationBeginGroup, beginGroup, args.Group, 1, 1));

            tileRuns = new List<TimeSpan>();
            tileCount = 0;
            totalTiles = args.Map.TotalTiles;
        }

        private static void OnFinishRenderingTile(object sender, TileProgressEventArgs args)
        {
            tileRuns.Add(DateTime.Now - beginTile);
            tileCount++;

            //Update display, after 1000 tiles
            if (tileRuns.Count > 500 || (DateTime.Now - lastUpdate).TotalSeconds > 5)
            {
                long d = 0;
                foreach (TimeSpan ts in tileRuns)
                    d += ts.Ticks;

                d /= tileRuns.Count;

                //For all other than the first calculation, we use the previous counts too
                if (tileCount != tileRuns.Count)
                    d = (d + prevDuration.Ticks) / 2;

                prevDuration = new TimeSpan(d);
                TimeSpan duration = new TimeSpan(d * totalTiles);

                tileRuns.Clear();
                lastUpdate = DateTime.Now;

                if (m_loggableProgress)
                    Console.WriteLine(string.Format(Strings.ConsoleOperationFinishTile, tileCount, totalTiles, args.Group, duration));
                else
                    DisplayProgress(sender, args);
            }
        }

        private static void OnBeginRenderingTile(object sender, TileProgressEventArgs args)
        {
            beginTile = DateTime.Now;
        }

        private static void OnFinishRenderingScale(object sender, TileProgressEventArgs args)
        {
            TimeSpan duration = DateTime.Now - beginScale;
            if (m_loggableProgress)
                Console.WriteLine(string.Format(Strings.ConsoleOperationFinishScale, DateTime.Now, args.Map.TileSet.GetScaleAt(args.ScaleIndex), duration));
        }

        private static void OnBeginRenderingScale(object sender, TileProgressEventArgs args)
        {
            beginScale = DateTime.Now;
            if (m_loggableProgress)
                Console.WriteLine(string.Format(Strings.ConsoleOperationBeginScale, beginMap, args.Map.TileSet.GetScaleAt(args.ScaleIndex), args.ScaleIndex, args.Map.Resolutions));
        }

        private static void OnFinishRenderingMap(object sender, TileProgressEventArgs args)
        {
            groupCount = 0;
            TimeSpan duration = DateTime.Now - beginMap;
            if (m_loggableProgress)
                Console.WriteLine(string.Format(Strings.ConsoleOperationFinishMap, DateTime.Now, args.Map.ResourceId, duration));
        }

        private static void OnBeginRenderingMap(object sender, TileProgressEventArgs args)
        {
            mapCount++;
            beginMap = DateTime.Now;
            if (m_loggableProgress)
                Console.WriteLine(string.Format(Strings.ConsoleOperationBeginMap, beginMap, args.Map.ResourceId));
        }
    }
}