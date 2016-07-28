<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Stats.aspx.cs" Inherits="SocialApp.Pages.Stats" %>

<asp:Content ID="Content1" ContentPlaceHolderID="StyleContent" runat="server">
    <link rel="stylesheet" type="text/css" href="/Content/Pages/Stats.css">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript" src="/Scripts/Pages/Stats.js"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="flex-container flex-column flex-1 content-padding">
        <div class="flex-1 flex-container flex-column flex-align-center">
            <div class="flex-container">
                <div class="page-title">Stats</div>
            </div>
            <div class="flex-container">
                <input type="hidden" id="selected_heading" runat="server" value="suitability" ClientIDMode="Static" />
                <div id="suitability_heading" class="section-heading">SUITABILITY </div>
                <div id="physical_heading" class="section-heading">PHYSICAL </div>
                <div id="virtual_heading" class="section-heading">VIRTUAL </div>
            </div>
            <div="flex-container">
                <div><span class="glyphicon glyphicon-chevron-left statGraphSwap"></span></div>
                <div id="suitability_content" class="section-content hidden">
                    <div class="proVsTownChartContainer statGraphContainer statViewHide"></div>          
                </div>
                <div id="physical_content" class="section-content hidden">
                    <div class="physServeChartContainer statGraphContainer statViewHide"></div>
                </div>
                <div id="virtual_content" class="section-content hidden">
                    <div class="virtServeChartContainer statGraphContainer statViewHide"></div>
                </div>
                <div><span class="glyphicon glyphicon-chevron-right statGraphSwap" ></span></div>   
            </div>
            <div class="flex-container">
                <asp:DropDownList runat="server" ID="statsTownList" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="statsTownList_SelectedIndexChanged" />
                <div class="flex-1"></div>
            </div>
        </div>

        <div class="modal fade" id="statExpandModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="exampleModalLabel">New message</h4>
                    </div>
                    <div runat="server" id="statExpandModalBody" class="modal-body" ClientIDMode ="Static">
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

        

</asp:Content>
