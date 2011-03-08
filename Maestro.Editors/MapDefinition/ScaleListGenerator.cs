#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
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
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Editors.MapDefinition
{
    internal enum ScaleGenerationMethod
    {
        Linear,
        Exponential
    }

    internal enum ScaleRoundingMethod
    {
        None,
        Regular,
        Pretty
    }

    internal static class ScaleListGenerator
    {
        public static double[] GenerateScales(double minScale, double maxScale, ScaleGenerationMethod method, ScaleRoundingMethod rounding, int scaleCount)
        {
            Check.Precondition(minScale < maxScale, "minScale < maxScale");
            Check.Precondition(scaleCount > 0, "scaleCount > 0");

            List<double> vals = new List<double>();

            if (method == ScaleGenerationMethod.Linear)
            {
                double inc = (double)(maxScale - minScale) / (double)scaleCount;
                double cur = (double)minScale;
                for (int i = 0; i < scaleCount; i++)
                {
                    vals.Add(cur);
                    cur += inc;
                }

                //In case the rounding sucks
                vals[(int)(scaleCount - 1)] = (double)maxScale;
            }
            else if (method == ScaleGenerationMethod.Exponential)
            {
                double b = Math.Pow((double)(maxScale - minScale), 1 / (double)(scaleCount));
                double cur = (double)minScale;
                for (int i = 0; i < scaleCount; i++)
                {
                    vals.Add(cur);
                    cur = ((double)maxScale) / Math.Pow(b, (int)scaleCount - i - 1) + (double)minScale;
                }
            }
            else
            {
                vals.Clear();
                double span = (double)maxScale - (double)minScale;
                double b = Math.Log10((double)maxScale / (double)(scaleCount - 1));

                for (int i = 0; i < scaleCount; i++)
                {
                    vals.Add(Math.Pow(i, b) + (double)minScale);
                }
            }

            if (rounding == ScaleRoundingMethod.Regular || rounding == ScaleRoundingMethod.Pretty)
            {
                for (int i = 0; i < vals.Count; i++)
                {
                    vals[i] = Math.Round(vals[i]);
                }
            }

            if (rounding == ScaleRoundingMethod.Pretty)
            {
                for (int i = 0; i < vals.Count; i++)
                {
                    int group = (int)Math.Floor(Math.Log10(vals[i]));
                    group--;

                    vals[i] = Math.Round(Math.Round((vals[i] / Math.Pow(10, group))) * Math.Pow(10, group));
                }
            }

            //Sort and weed out dupes
            var sorted = new SortedList<double, double>();
            foreach (var d in vals)
            {
                if (!sorted.ContainsKey(d))
                    sorted.Add(d, d);
            }

            return new List<double>(sorted.Keys).ToArray();
        }
    }
}
