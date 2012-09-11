using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.WebLayout;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.ObjectModels;

namespace OSGeo.MapGuide.MaestroAPI.Resource.Validation
{
    /// <summary>
    /// Base class of all web layout validators. Provides common validation
    /// logic for web layouts
    /// </summary>
    public abstract class BaseWebLayoutValidator : IResourceValidator
    {
        /// <summary>
        /// Gets the resource type and version this validator supports
        /// </summary>
        /// <value></value>
        public abstract ResourceTypeDescriptor SupportedResourceAndVersion
        {
            get;
        }

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

            if (resource.ResourceType != OSGeo.MapGuide.MaestroAPI.ResourceTypes.WebLayout)
                return null;

            List<ValidationIssue> issues = new List<ValidationIssue>();

            IWebLayout layout = resource as IWebLayout;
            if (layout.Map == null || layout.Map.ResourceId == null)
            {
                issues.Add(new ValidationIssue(layout, ValidationStatus.Error, ValidationStatusCode.Error_WebLayout_MissingMap, string.Format(Strings.WL_MissingMapError)));
            }
            else
            {
                //Check for duplicate command names
                var cmdSet = layout.CommandSet;
                Dictionary<string, ICommand> cmds = new Dictionary<string, ICommand>();
                foreach (ICommand cmd in cmdSet.Commands)
                {
                    if (cmds.ContainsKey(cmd.Name))
                        issues.Add(new ValidationIssue(layout, ValidationStatus.Error, ValidationStatusCode.Error_WebLayout_DuplicateCommandName, string.Format(Strings.WL_DuplicateCommandName, cmd.Name)));
                    else
                        cmds[cmd.Name] = cmd;
                }

                //Check for duplicate property references in search commands
                foreach (ICommand cmd in cmdSet.Commands)
                {
                    if (cmd is ISearchCommand)
                    {
                        ISearchCommand search = (ISearchCommand)cmd;
                        Dictionary<string, string> resColProps = new Dictionary<string, string>();
                        foreach (IResultColumn resCol in search.ResultColumns.Column)
                        {
                            if (resColProps.ContainsKey(resCol.Property))
                                issues.Add(new ValidationIssue(layout, ValidationStatus.Error, ValidationStatusCode.Error_WebLayout_DuplicateSearchCommandResultColumn, string.Format(Strings.WL_DuplicateSearchResultColumn, search.Name, resCol.Property)));
                            else
                                resColProps.Add(resCol.Property, resCol.Property);
                        }
                    }
                }

                //Check for command references to non-existent commands
                foreach (IUIItem item in layout.ContextMenu.Items)
                {
                    if (item.Function == UIItemFunctionType.Command)
                    {
                        ICommandItem cmdRef = (ICommandItem)item;
                        if (!cmds.ContainsKey(cmdRef.Command))
                            issues.Add(new ValidationIssue(layout, ValidationStatus.Error, ValidationStatusCode.Error_WebLayout_NonExistentContextMenuCommandReference, string.Format(Strings.WL_NonExistentMenuCommandReference, cmdRef.Command)));
                    }
                }

                foreach (IUIItem item in layout.TaskPane.TaskBar.Items)
                {
                    if (item.Function == UIItemFunctionType.Command)
                    {
                        ICommandItem cmdRef = (ICommandItem)item;
                        if (!cmds.ContainsKey(cmdRef.Command))
                            issues.Add(new ValidationIssue(layout, ValidationStatus.Error, ValidationStatusCode.Error_WebLayout_NonExistentTaskPaneCommandReference, string.Format(Strings.WL_NonExistentTaskPaneCommandReference, cmdRef.Command)));
                    }
                }

                foreach (IUIItem item in layout.ToolBar.Items)
                {
                    if (item.Function == UIItemFunctionType.Command)
                    {
                        ICommandItem cmdRef = (ICommandItem)item;
                        if (!cmds.ContainsKey(cmdRef.Command))
                            issues.Add(new ValidationIssue(layout, ValidationStatus.Error, ValidationStatusCode.Error_WebLayout_NonExistentToolbarCommandReference, string.Format(Strings.WL_NonExistentToolbarCommandReference, cmdRef.Command)));
                    }
                }

                try
                {
                    IMapDefinition mdef = (IMapDefinition)context.GetResource(layout.Map.ResourceId);
                    if (layout.Map.InitialView != null)
                    {
                        var mapEnv = ObjectFactory.CreateEnvelope(mdef.Extents.MinX, mdef.Extents.MinY, mdef.Extents.MaxX, mdef.Extents.MaxY);
                        if (!mapEnv.Contains(layout.Map.InitialView.CenterX, layout.Map.InitialView.CenterY))
                            issues.Add(new ValidationIssue(mdef, ValidationStatus.Warning, ValidationStatusCode.Warning_WebLayout_InitialViewOutsideMapExtents, string.Format(Strings.WL_StartViewOutsideExtentsWarning)));
                    }

                    if (recurse)
                    {
                        issues.AddRange(ResourceValidatorSet.Validate(context, mdef, true));
                    }
                }
                catch (Exception ex)
                {
                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                    issues.Add(new ValidationIssue(layout, ValidationStatus.Error, ValidationStatusCode.Error_WebLayout_Generic, string.Format(Strings.WL_MapValidationError, layout.Map.ResourceId, msg)));
                }
            }

            context.MarkValidated(resource.ResourceID);

            return issues.ToArray();
        }
    }
}
