<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SamplesWeb.Tasks.Home" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Home</title>
    <script type="text/javascript">
    
        function Go(url)
        {
            var map = parent.parent.GetMapFrame();
            
            url += "?SESSION=" + map.GetSessionId();
            url += "&MAPNAME=" + map.GetMapName();
            
            window.location = url;
        }
    
    </script>
</head>
<body>
    <p>At any time, click the <strong>home button</strong> in the task bar to return to this list of samples.</p>
    
    <p>Samples</p>
    <ul>
        <li><a href="#" onclick="Go('ToggleParcelsLayer.aspx')">Toggle Parcels Layer</a></li>
        <li><a href="#" onclick="Go('AddTracksLayer.aspx')">Add Tracks Layer</a></li>
    </ul>
</body>
</html>
