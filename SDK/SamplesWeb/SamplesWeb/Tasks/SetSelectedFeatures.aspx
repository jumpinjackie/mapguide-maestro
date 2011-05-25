<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetSelectedFeatures.aspx.cs" Inherits="SamplesWeb.Tasks.SetSelectedFeatures" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <h3>Set Selected Features</h3>
    <hr />
    <form id="form1" runat="server">
    <div>
        <input id="MAPNAME" runat="server" type="hidden" />
        <input id="SESSION" runat="server" type="hidden" />
        <p>Select a map layer and specify a filter (eg. Layer: Parcels, Filter: RNAME LIKE 'SCHMITT%')</p>
        <p>Features in this layer that match the filter will be selected</p>
        <p>Layer:</p>
        <asp:DropDownList ID="ddlLayers" runat="server" />
        <p>Filter:</p>
        <asp:TextBox ID="txtFilter" runat="server" Rows="4" TextMode="MultiLine" />
        <div></div>
        <asp:Button ID="btnSelect" runat="server" Text="Select" 
            onclick="btnSelect_Click" />
        <hr />
        <asp:Label ID="lblMessage" runat="server" />
    </div>
    </form>
</body>
</html>
