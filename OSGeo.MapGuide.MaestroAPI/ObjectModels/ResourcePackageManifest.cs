using System;
using System.Collections.Generic;
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
