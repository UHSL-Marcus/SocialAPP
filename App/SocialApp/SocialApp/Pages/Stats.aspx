<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Stats.aspx.cs" Inherits="SocialApp.Pages.Stats" %>

<asp:Content ID="Content1" ContentPlaceHolderID="StyleContent" runat="server">
    <link rel="stylesheet" type="text/css" href="/Content/Pages/Stats.css">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript" src="/Scripts/Pages/Stats.js"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="flex-1 flex-container flex-center">
        <div id="statsTabNavDiv">
            <ul class="nav nav-pills nav-stacked" id="statsTabs">
                <li class="text-center active"><a class="statTab" href="#provsTown" data-toggle="pill">Profile<br/>vs.<br/>Town</a></li>
                <li class="text-center"><a class="statTab" href="#physServ" data-toggle="pill">Physical<br />Services</a></li>
                <li class="text-center"><a class="statTab" href="#virtuServ" data-toggle="pill">Virtual<br />Services</a></li>
            </ul>
        </div>
        <div class="flex-1 flex-container flex-column">
            <div class="flex-container">
                <asp:DropDownList runat="server" ID="statsTownList" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="statsTownList_SelectedIndexChanged" />
                <div class="flex-1"></div>
            </div>
            

            <div class="tab-content flex-container flex-1 content-padding">
                <div><span class="glyphicon glyphicon-chevron-left statGraphSwap" aria-hidden="true"></span></div>
                <div role="tabpanel" class="tab-pane active" id="provsTown" style="width:95%;">
                    <div class="proVsTownChartContainer statGraphContainer statViewHide"></div>          
                </div>
                <div role="tabpanel" class="tab-pane" id="physServ" style="width:95%;">
                    <div class="physServeChartContainer statGraphContainer statViewHide"></div>
                </div>
                <div role="tabpanel" class="tab-pane" id="virtuServ" style="width:95%;">
                    <div class="virtServeChartContainer statGraphContainer statViewHide"></div>
                </div>
                <div><span class="glyphicon glyphicon-chevron-right statGraphSwap"  aria-hidden="true"></span></div>   
            </div>  
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
    

</asp:Content>
