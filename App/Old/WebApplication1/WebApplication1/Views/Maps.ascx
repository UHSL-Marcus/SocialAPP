<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Maps.ascx.cs" Inherits="WebApplication1.Views.Maps" %>

<div class="flex-1 flex-container flex-column">
    <asp:UpdatePanel ID="StatsMainUpdatePanel" runat="server" UpdateMode="Conditional" class="flex-container">
        <ContentTemplate>
            <input type="hidden" runat="server" id="mapCoords" ClientIDMode="Static" /> 
            <input type="hidden" runat="server" id="mapZoom" ClientIDMode="Static" />
            <div runat="server" id="townInfo" ClientIDMode="Static"></div>
            <input type="hidden" id="placeData" runat="server" ClientIDMode="Static" />
            <!--<asp:TextBox runat="server" ID="mapsTownSelectionHidden" ClientIDMode="Static" AutoPostBack="true" OnTextChanged="mapsTownSelectionHidden_TextChanged" CssClass=""></asp:TextBox>-->
            
            <div id="control" style="margin-top:0.3%">
                <asp:DropDownList runat="server" ID="mapsTownList" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="mapsTownList_SelectedIndexChanged" />
                <select id="mapCatList"></select>
                <select id="mapSubCatList"></select>
                </br></br><label for ="mapFromInput">From</label><input type="text" id="mapFromInput" /> 
                <input type="button" id="mapsClearRoute" value="Clear Route" />
                </br><label for ="mapToInput">To</label><input type="text" id="mapToInput" />
                <input type="button" id="mapsFindRoute" value="Find Route" />
                </br></br><label for ="mapRadiusInput">Search Centre</label><input type="text" id="mapRadiusInput" /> <input type="button" id="mapsRadiusSearch" value="Toggle Radius/Full Town" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="flex-1" id="map-canvas">MAP</div>
</div>



