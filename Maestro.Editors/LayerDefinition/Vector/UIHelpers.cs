#region Disclaimer / License

// Copyright (C) 2023, Jackie Ng
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

using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.Services;
using System;

namespace Maestro.Editors.LayerDefinition.Vector
{
    internal static class UIHelpers
    {
        public static void LoadClassDefinition(IFeatureService featSvc,
                                               string fsId,
                                               string className,
                                               Action<ClassDefinition> onSuccess,
                                               Action onFailure = null)
        {
            BusyWaitDialog.Run(Strings.LoadingClassDefinition, () =>
            {
                ClassDefinition cls = null;
                try
                {
                    cls = featSvc.GetClassDefinition(fsId, className);
                }
                catch
                {
                    //Do nothing, layer settings control does this check and should flag the feature class field as something requiring attention
                }
                return cls;
            }, (res, ex) =>
            {
                if (res is ClassDefinition cls)
                    onSuccess(cls);
                else
                    onFailure?.Invoke();
            });
        }
    }
}
