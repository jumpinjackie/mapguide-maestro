using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;

namespace OSGeo.MapGuide.MaestroAPI.Resource.Validation
{
    /// <summary>
    /// Base class of all load procedure validations. Provides common load procedure validation
    /// logic.
    /// </summary>
    public abstract class BaseLoadProcedureValidator : IResourceValidator
    {
        /// <summary>
        /// Gets the resource type and version this validator supports
        /// </summary>
        /// <value></value>
        public abstract ResourceTypeDescriptor SupportedResourceAndVersion { get; }

        /// <summary>
        /// Validats the specified resources for common issues associated with this
        /// resource type
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resource"></param>
        /// <param name="recurse"></param>
        /// <returns></returns>
        public virtual ValidationIssue[] Validate(ResourceValidationContext context, IResource resource, bool recurse)
        {
            if (!resource.GetResourceTypeDescriptor().Equals(this.SupportedResourceAndVersion))
                return null;

            return ValidateBase(context, resource, recurse);
        }

        /// <summary>
        /// Performs base validation logic
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resource"></param>
        /// <param name="recurse"></param>
        /// <returns></returns>
        protected static ValidationIssue[] ValidateBase(ResourceValidationContext context, IResource resource, bool recurse)
        {
            Check.NotNull(context, "context"); //NOXLATE

            if (context.IsAlreadyValidated(resource.ResourceID))
                return null;

            if (resource.ResourceType != OSGeo.MapGuide.MaestroAPI.ResourceTypes.LoadProcedure)
                return null;

            if (resource.ResourceVersion != new Version(1, 0, 0))
                return null;

            var set = new ValidationResultSet();

            var loadProc = (resource as ILoadProcedure).SubType;

            if (loadProc.Type == LoadType.Dwg)
            {
                set.AddIssue(new ValidationIssue(resource, ValidationStatus.Warning, ValidationStatusCode.Warning_LoadProcedure_DwgNotSupported, Strings.LPROC_DWGNotSupported));
                return set.GetAllIssues(); //all she wrote
            }

            if (loadProc.Type == LoadType.Raster)
            {
                set.AddIssue(new ValidationIssue(resource, ValidationStatus.Warning, ValidationStatusCode.Warning_LoadProcedure_RasterNotSupported, Strings.LPROC_RasterNotSupported));
                return set.GetAllIssues(); //all she wrote
            }

            if (loadProc.Type == LoadType.Sdf)
            {
                set.AddIssue(new ValidationIssue(resource, ValidationStatus.Warning, ValidationStatusCode.Warning_LoadProcedure_Sdf2OptionsNotSupported, Strings.LPROC_Sdf2OptionsNotSupported));
                set.AddIssue(new ValidationIssue(resource, ValidationStatus.Warning, ValidationStatusCode.Warning_LoadProcedure_GeneralizationNotSupported, Strings.LPROC_GeneralizationNotSupported));
            }

            if (loadProc.Type == LoadType.Shp)
            {
                set.AddIssue(new ValidationIssue(resource, ValidationStatus.Warning, ValidationStatusCode.Warning_LoadProcedure_ConvertToSdf3NotSupported, Strings.LPROC_ConvertToSdf3NotSupported));
                set.AddIssue(new ValidationIssue(resource, ValidationStatus.Warning, ValidationStatusCode.Warning_LoadProcedure_GeneralizationNotSupported, Strings.LPROC_GeneralizationNotSupported));
            }

            if (loadProc.Type == LoadType.Sqlite)
            {
                set.AddIssue(new ValidationIssue(resource, ValidationStatus.Warning, ValidationStatusCode.Warning_LoadProcedure_GeneralizationNotSupported, Strings.LPROC_GeneralizationNotSupported));
            }

            var fproc = loadProc as IBaseLoadProcedure;
            if (fproc != null)
            {
                foreach (var fn in fproc.SourceFile)
                {
                    if (!System.IO.File.Exists(fn))
                    {
                        set.AddIssue(new ValidationIssue(resource, ValidationStatus.Warning, ValidationStatusCode.Warning_LoadProcedure_SourceFileNotFound, string.Format(Strings.LPROC_SourceFileNotFound, fn)));
                    }
                }
            }

            context.MarkValidated(resource.ResourceID);

            return set.GetAllIssues();
        }
    }
}
