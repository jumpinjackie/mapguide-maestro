Known Development Issues
========================

If you are working on the MapGuide Maestro source (and not consuming its API), you should be aware of the following issues (and their documented workarounds):

## 1. Assembly Loading Errors (OSGeo.MapGuide.ObjectModels) when opening any Form, Control or UserControl component of Maestro.Editors in the WinForms designer.

### Cause: 

For reasons beyond our control, the WinForms designer wants the signed (`net461`) version of `OSGeo.MapGuide.ObjectModels` but the project reference to `OSGeo.MapGuide.ObjectModels` wants the `netstandard2.0` version, which is not signed causing an assembly load error when opening any Form, Control or UserControl

### Workaround: 

Exit Visual Studio and momentarily edit the following projects and remove the `netstandard2.0` target under the `<TargetFrameworks>` property:

 * `OSGeo.FDO.Expressions`
 * `OSGeo.MapGuide.ObjectModels`
 * `OSGeo.MapGuide.MaestroAPI`

Rebuild and your Form, Control and UserControl items should now be able to open in the VS WinForms designer.

Once you've made whatever changes in your Form/Control/UserControl, make sure to restore the above projects to their original settings.