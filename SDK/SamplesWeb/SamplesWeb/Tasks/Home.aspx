<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SamplesWeb.Tasks.Home" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Home</title>
    <script type="text/javascript">
    
        function Go()
        {
            var map = parent.parent.GetMapFrame();
            var form = parent.parent.GetFormFrame();
            
            var url = arguments[0];
            var params = new Array();
            params.push("SESSION");
            params.push(map.GetSessionId());
            params.push("MAPNAME");
            params.push(map.GetMapName());
            params.push("SELECTION");
            params.push(encodeURIComponent(map.GetSelectionXML()));
            
            if (arguments.length > 1)
            {
                for (var i = 1; i < arguments.length; i++)
                {
                    params.push(arguments[i]);
                }
            }
            
            form.Submit(url, params, "taskPaneFrame"); //The name of the task pane frame
        }
    
    </script>
</head>
<body>
    <h3>Samples</h3>
    <hr />
    <p>At any time, click the <strong>home button</strong> in the task bar or the <strong>Go back</strong> link to return to this list of samples.</p>
    <p>Map/Layer Manipulation:</p>
    <ul>
        <li><a href="#" onclick="Go('../SamplesWeb/Tasks/ToggleParcelsLayer.aspx')">Add/Remove Parcels Layer</a></li>
        <li><a href="#" onclick="Go('../SamplesWeb/Tasks/AddTracksLayer.aspx')">Add Tracks Layer</a></li>
        <li><a href="#" onclick="Go('../SamplesWeb/Tasks/AddThemedDistrictsLayer.aspx')">Add Themed Districts Layer</a></li>
        <li><a href="#" onclick="Go('../SamplesWeb/Tasks/ToggleGroupVisibility.aspx','GROUPNAME','Base Map')">Toggle "Base Map" Group</a></li>
        <li><a href="#" onclick="Go('../SamplesWeb/Tasks/ToggleLayerVisibility.aspx','LAYERNAME','Parcels')">Toggle "Parcels" Layer</a></li>
        <li><a href="#" onclick="Go('../SamplesWeb/Tasks/ModifyParcelsFilter.aspx')">Change "Parcels" layer filter</a></li>
    </ul>
    <p>Feature Selection:</p>
    <ul>
        <li><a href="#" onclick="Go('../SamplesWeb/Tasks/ListSelection.aspx')">List Selected Features</a></li>
        <li><a href="#" onclick="Go('../SamplesWeb/Tasks/SetSelectedFeatures.aspx')">Set Selected Features</a></li>
    </ul>
    <p>Map/Layer Information:</p>
    <ul>
        <li><a href="#" onclick="Go('../SamplesWeb/Tasks/LayerInfo.aspx')">Layer Information</a></li>
    </ul>
</body>
</html>
