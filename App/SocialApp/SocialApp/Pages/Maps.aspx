<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Maps.aspx.cs" Inherits="SocialApp.Pages.Maps" %>
<asp:Content ID="Content1" ContentPlaceHolderID="StyleContent" runat="server">
    <link rel="stylesheet" type="text/css" href="/Content/Pages/Maps.css">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript" src="/Scripts/Pages/Maps.js"></script> 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="fill flex-container flex-column">

        <asp:UpdatePanel ID="mapInputUpdatePanel" runat="server" UpdateMode="Conditional" class="flex-container">
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
                    <input type="button" id="mapsRouteReport" value="Show Report" disabled="disabled" />
                    </br></br><label for ="mapRadiusInput">Search Centre</label><input type="text" id="mapRadiusInput" /> <input type="button" id="mapsRadiusSearch" value="Toggle Radius/Full Town" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="flex-1" id="map_canvas">MAP</div>

    </div>

    <!-- ***********************************Route Report Modal*****************************************-->
    
    <div class="modal fade" id="routeReportModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header text-center">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h1 class="modal-title" id="loginModalLabel">Route Report</h1>
                    <h2><span id="modalUsefulnessInfo"/></h2>
                </div>
          
                <div id="routeReportModalBody" class="modal-body">
                    <div>
                        <button class="btn btn-default" type="button" data-toggle="collapse" data-target="#temptargetCollapseInfo">temp button</button>
                        <div class="collapse" id="temptargetCollapseInfo">
                            here is some stuffff!!
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
