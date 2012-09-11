using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;

namespace OSGeo.MapGuide.MaestroAPI.Resource.Validation
{
    /// <summary>
    /// The base class of Symbol Definition validators
    /// </summary>
    public abstract class BaseSymbolDefinitionValidator : IResourceValidator
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
        /// Validats the specified resources for common issues associated with this
        /// resource type
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

            if (resource.ResourceType != ResourceTypes.SymbolDefinition)
                return null;

            var issues = new List<ValidationIssue>();
            var symDef = (ISymbolDefinitionBase)resource;
            if (symDef.Type == SymbolDefinitionType.Simple)
            {
                var ssym = (ISimpleSymbolDefinition)symDef;
                issues.AddRange(ValidateSimpleSymbolDefinition(ssym, context));
            }
            else if (symDef.Type == SymbolDefinitionType.Compound)
            {
                var csym = (ICompoundSymbolDefinition)symDef;
                foreach (var sym in csym.SimpleSymbol)
                {
                    if (sym.Type == SimpleSymbolReferenceType.Inline)
                    {
                        var inline = (ISimpleSymbolInlineReference)sym;
                        issues.AddRange(ValidateSimpleSymbolDefinition(inline.SimpleSymbolDefinition, context));
                    }
                    else if (sym.Type == SimpleSymbolReferenceType.Library)
                    {
                        var res = context.GetResource(((ISimpleSymbolLibraryReference)sym).ResourceId);
                        issues.AddRange(ValidateBase(context, res, false));
                    }
                }
            }

            context.MarkValidated(resource.ResourceID);
            return issues.ToArray();
        }

        /// <summary>
        /// Validates the simple symbol definition.
        /// </summary>
        /// <param name="ssym">The ssym.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected static IEnumerable<ValidationIssue> ValidateSimpleSymbolDefinition(ISimpleSymbolDefinition ssym, ResourceValidationContext context)
        {
            //Check that one geometry usage context has been specified
            if (ssym.AreaUsage == null &&
                ssym.LineUsage == null &&
                ssym.PointUsage == null)
            {
                yield return new ValidationIssue(ssym, ValidationStatus.Error, ValidationStatusCode.Error_SymbolDefinition_NoGeometryUsageContexts, Strings.SSDF_NoGeometryUsageContext);
            }

            //Validate image graphics
            foreach (var graphic in ssym.Graphics)
            {
                if (graphic.Type == GraphicElementType.Image)
                {
                    IImageGraphic image = (IImageGraphic)graphic;
                    if (image.Item != null)
                    {
                        if (image.Item.Type == ImageType.Reference)
                        {
                            IImageReference imgRef = (IImageReference)image.Item;
                            if (!context.ResourceExists(imgRef.ResourceId))
                            {
                                yield return new ValidationIssue(ssym, ValidationStatus.Error, ValidationStatusCode.Error_SymbolDefinition_ImageGraphicReferenceResourceIdNotFound, Strings.SSDF_ImageGraphicReferenceResourceIdNotFound);
                            }
                            else
                            {
                                var res = context.GetResource(imgRef.ResourceId);
                                var resData = res.EnumerateResourceData();
                                bool found = false;
                                foreach (var item in resData)
                                {
                                    if (item.Name == imgRef.LibraryItemName)
                                        found = true;
                                }

                                if (!found)
                                {
                                    yield return new ValidationIssue(ssym,
                                                                     ValidationStatus.Error,
                                                                     ValidationStatusCode.Error_SymbolDefinition_ImageGraphicReferenceResourceDataNotFound,
                                                                     string.Format(Strings.SSDF_ImageGraphicReferenceResourceDataNotFound,
                                                                        imgRef.ResourceId,
                                                                        imgRef.LibraryItemName));
                                }
                            }
                        }
                        else //inline
                        {
                            //TODO: Validate inline image content
                        }
                    }
                }
            }

            string xml = ResourceTypeRegistry.SerializeAsString(ssym);

            //Check non existent symbol parameters
            foreach (var paramDef in ssym.ParameterDefinition.Parameter)
            {
                string name = "%" + paramDef.Identifier + "%"; //NOXLATE
                if (!xml.Contains(name) && string.IsNullOrEmpty(paramDef.DefaultValue))
                {
                    yield return new ValidationIssue(ssym, ValidationStatus.Warning, ValidationStatusCode.Warning_SymbolDefinition_SymbolParameterNotUsed,
                        string.Format(Strings.SSDF_SymbolParameterNotUsed, paramDef.Identifier));
                }
            }

            //TODO: Do the reverse check. Placeholders not pointing to a symbol parameter
        }
    }
}
