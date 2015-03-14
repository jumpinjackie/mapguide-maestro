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

        [STAThread()]
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

            Boolean cmdLineMode = false;

            string mapdefinitions = "";
            string scaleindex = "";
            string basegroups = "";

            string limitRows = "";
            string limitCols = "";

            string tileWidth = "";
            string tileHeight = "";

            string DPI = "";
            string metersPerUnit = "";

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
                bx.BeginRenderingMap += new ProgressCallback(bx_BeginRenderingMap);
                bx.FinishRenderingMap += new ProgressCallback(bx_FinishRenderingMap);
                bx.BeginRenderingGroup += new ProgressCallback(bx_BeginRenderingGroup);
                bx.FinishRenderingGroup += new ProgressCallback(bx_FinishRenderingGroup);
                bx.BeginRenderingScale += new ProgressCallback(bx_BeginRenderingScale);
                bx.FinishRenderingScale += new ProgressCallback(bx_FinishRenderingScale);
                bx.BeginRenderingTile += new ProgressCallback(bx_BeginRenderingTile);
                bx.FinishRenderingTile += new ProgressCallback(bx_FinishRenderingTile);

                bx.FailedRenderingTile += new ErrorCallback(bx_FailedRenderingTile);

                mapCount = 0;
                lastUpdate = DateTime.Now;

                bx.RenderAll();
            }
        }

        private static void bx_FailedRenderingTile(CallbackStates state, MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref Exception exception)
        {
            exceptionList.Add(exception);
            exception = null;
        }

        private static void DisplayProgress(MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            if (hasConsole)
                Console.Clear();
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

        private static void bx_FinishRenderingGroup(CallbackStates state, MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            TimeSpan duration = DateTime.Now - beginGroup;
            if (m_loggableProgress)
                Console.WriteLine(string.Format(Strings.ConsoleOperationFinishGroup, DateTime.Now, group, duration));
        }

        private static void bx_BeginRenderingGroup(CallbackStates state, MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            groupCount++;
            beginGroup = DateTime.Now;

            if (m_loggableProgress)
                Console.WriteLine(string.Format(Strings.ConsoleOperationBeginGroup, beginGroup, group, 1, 1));

            tileRuns = new List<TimeSpan>();
            tileCount = 0;
            totalTiles = map.TotalTiles;
        }

        private static void bx_FinishRenderingTile(CallbackStates state, MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref bool cancel)
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
                    Console.WriteLine(string.Format(Strings.ConsoleOperationFinishTile, tileCount, totalTiles, group, duration));
                else
                    DisplayProgress(map, group, scaleindex, row, column, ref cancel);
            }
        }

        private static void bx_BeginRenderingTile(CallbackStates state, MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            beginTile = DateTime.Now;
        }

        private static void bx_FinishRenderingScale(CallbackStates state, MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            TimeSpan duration = DateTime.Now - beginScale;
            if (m_loggableProgress)
                Console.WriteLine(string.Format(Strings.ConsoleOperationFinishScale, DateTime.Now, map.TileSet.GetScaleAt(scaleindex), duration));
        }

        private static void bx_BeginRenderingScale(CallbackStates state, MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            beginScale = DateTime.Now;
            if (m_loggableProgress)
                Console.WriteLine(string.Format(Strings.ConsoleOperationBeginScale, beginMap, map.TileSet.GetScaleAt(scaleindex), scaleindex, map.Resolutions));
        }

        private static void bx_FinishRenderingMap(CallbackStates state, MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            groupCount = 0;
            TimeSpan duration = DateTime.Now - beginMap;
            if (m_loggableProgress)
                Console.WriteLine(string.Format(Strings.ConsoleOperationFinishMap, DateTime.Now, map.ResourceId, duration));
        }

        private static void bx_BeginRenderingMap(CallbackStates state, MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            mapCount++;
            beginMap = DateTime.Now;
            if (m_loggableProgress)
                Console.WriteLine(string.Format(Strings.ConsoleOperationBeginMap, beginMap, map.ResourceId));
        }
    }
}