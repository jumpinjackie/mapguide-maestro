<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LayerInfo.aspx.cs" Inherits="SamplesWeb.Tasks.LayerInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Layer Information</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input id="MAPNAME" runat="server" type="hidden" />
        <input id="SESSION" runat="server" type="hidden" />
        <p>Select a map layer and click describe to get the properties of that layer and its associated class definition</p>
        <p>Layer:</p>
        <asp:DropDownList ID="ddlLayers" runat="server" />
        <asp:Button ID="btnDescribe" runat="server" Text="Describe" 
            onclick="btnDescribe_Click" />
        <hr />
        <div id="classDef" runat="server">
        </div>
    </div>
    </form>
</body>
</html>
