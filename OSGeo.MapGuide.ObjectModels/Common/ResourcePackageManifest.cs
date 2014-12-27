#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

#endregion Disclaimer / License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.ObjectModels.Common
{
    partial class ResourcePackageManifestOperationsOperationParameters
    {
        /// <summary>
        /// Gets the value of the specified parameter
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetParameterValue(string name)
        {
            if (this.parameterField != null)
            {
                foreach (var p in this.parameterField)
                {
                    if (p.Name.Equals(name))
                    {
                        return p.Value;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Sets the value of the specified parameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetParameterValue(string name, string value)
        {
            if (this.parameterField == null)
                this.parameterField = new System.ComponentModel.BindingList<ResourcePackageManifestOperationsOperationParametersParameter>();

            foreach (var p in this.parameterField)
            {
                if (p.Name.Equals(name))
                {
                    p.Value = value;
                    return;
                }
            }

            this.parameterField.Add(new ResourcePackageManifestOperationsOperationParametersParameter()
            {
                Name = name,
                Value = value
            });
        }
    }
}