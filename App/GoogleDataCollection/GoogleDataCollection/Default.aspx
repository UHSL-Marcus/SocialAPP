<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GoogleDataCollection._Default" validateRequest="false" %>

<!DOCTYPE html>

<html lang="en">
    <head runat="server">
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no"/>
        <title>Title</title>

        <asp:PlaceHolder runat="server">
            <%: Scripts.Render("~/bundles/modernizr") %>
        </asp:PlaceHolder>

        <webopt:bundlereference runat="server" path="~/Content/css" />
        <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />

    </head>
    <body>
        
        <form runat="server">
            <asp:ScriptManager runat="server" EnablePartialRendering="true">
                <Scripts>
                    <%--To learn more about bundling scripts in ScriptManager see http://go.microsoft.com/fwlink/?LinkID=301884 --%>
                    <%--Framework Scripts--%>
                    <asp:ScriptReference Name="MsAjaxBundle" />
                    <asp:ScriptReference Name="jquery" />
                    <asp:ScriptReference Name="respond" />
                    <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                    <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                    <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                    <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                    <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                    <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                    <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                    <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                    <asp:ScriptReference Name="WebFormsBundle" />
                    <%--Site Scripts--%>
                    <asp:ScriptReference Path="~/bundles/Page" />
                    <asp:ScriptReference Path="~/bundles/Utils" />
                    <asp:ScriptReference Path="https://maps.googleapis.com/maps/api/js?key=AIzaSyDjcLgEOEkdQfwxEyGOu4hHlSY_s-LOGkQ&libraries=places" />
                </Scripts>
            </asp:ScriptManager>

            <div class="fill flex-container flex-column">
                <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="always">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="uploadResults" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="UpdateCountAndNormal" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                        <input type="hidden" runat="server" id="mapCoords" ClientIDMode="Static" /> 
                        <input type="hidden" runat="server" id="mapZoom" ClientIDMode="Static" />
                        <div runat="server" id="townInfo" ClientIDMode="Static"></div>
                        <input type="hidden" id="ServiceData" runat="server" ClientIDMode="Static" />
                        <input type="hidden" id="TownData" runat="server" ClientIDMode="Static" />
                        <input type="hidden" id="types" runat="server" ClientIDMode="Static" />
                        <div id="tempinfo" runat="server" ClientIDMode="Static" class="hidden"></div>

                        <div id="control" style="margin-top:0.3%">
                            <input type="hidden" id="reqcount" />
                            <input type="hidden" id="reqpend" />
                            <input type="hidden" id="reqaddedProc" />
                            <input type="text" id="towntoSearch" class="locateTown" value="Hatfield" />
                            <input type="button" id="locateTown" class="locateTown" value="Locate Town" />
                            <select id="selectedTown" class="scanTown" ></select> 
                            <input type="button" id="scanTown" class="scanTown" value="Scan Town" />
                            <input type="button" id="newSearch" class="scanTown" value="Search Again" />
                            <input type="text" readonly id="infoDisplay" class="info" />
                            <input type="button" id="cancelProcessing" class="info" value="Cancel" />
                            <asp:Button runat="server" ID="uploadResults" OnClick="uploadResults_Click" ClientIDMode="Static" Text="Upload" CssClass="hidden" />
                            <asp:Button runat="server" ID="UpdateCountAndNormal" OnClick="UpdateCountAndNormal_Click"  ClientIDMode="Static" Text="UploadCountNormal" CssClass="hidden" />
                        </div>
                        <div id="errorLog" style="margin-top:0.3%; margin-right:0.3%">
                            <textarea runat="server" cols="20" rows="25" id="errorText" ClientIDMode="Static"></textarea>
                            <input type="button" id="clearErrorText" value="Clear" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="flex-1" id="map-canvas">MAP</div>
    

    
            </div>



        </form>

    </body>
</html>
