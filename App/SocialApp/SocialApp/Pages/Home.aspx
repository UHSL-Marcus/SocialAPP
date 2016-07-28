<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SocialApp.Pages.Home" %>

<asp:Content ID="StyleContent" ContentPlaceHolderID="StyleContent" runat="server">
   <link rel="stylesheet" type="text/css" href="/Content/Pages/Home.css">
</asp:Content>


<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript" src="/Scripts/Pages/Home.js"></script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="homeUpdatePanel" class="fill flex-container">
        <ContentTemplate>
            <!-- **********************************Menu Buttons**************************************** -->
            <div class="flex-1 flex-container flex-center"> 
                <asp:ImageButton runat="server" ID="profileBtn" ImageUrl="/Resources/Icons/03-Profile.png" CssClass="defaultNavBtn"  OnClick="profileBtn_Click"  ClientIDMode="Static" /> 
                <asp:ImageButton runat="server" ID="statsBtn" ImageUrl="/Resources/Icons/02-Stats.png" CssClass="defaultNavBtn"  OnClick="statsBtn_Click"  ClientIDMode="Static" /> 
                <asp:ImageButton runat="server" ID="mapsBtn" ImageUrl="/Resources/Icons/04-Maps.png" CssClass="defaultNavBtn"  OnClick="mapsBtn_Click"  ClientIDMode="Static" />
                <input runat="server" type="hidden" id="followOnPath" />
            </div>

            <!-- ***********************************Login Modal*****************************************-->
    
            <div class="modal fade" id="loginModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header text-center">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h1 class="modal-title" id="loginModalLabel">Login</h1>
                        </div>
                        <div id="loginModalBody" class="modal-body">
                            <!-- Login -->
                            <div id="loginView" class="centre-form centering text-center">
                                <asp:Label ID="info" runat="server" Text="" />

                                <div class="form-group has-feedback">
                                    <label for="userIn" class="control-label">Username</label>
                                    <input type="text" runat="server" class="form-control" id="userIn" placeholder="Username" ClientIDMode="Static">
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                </div>
                                <div class="form-group has-feedback">
                                    <label for="passwordIn" class="control-label">Password</label>
                                    <input type="password" runat="server" class="form-control" id="passwordIn" placeholder="Password" ClientIDMode="Static"/>
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                </div>
                                <div class="flex-container">
                                    <asp:Button runat="server" id="loginBtn" CssClass="btn btn-default" onclick="loginBtn_Click" text="Login" ClientIDMode="Static" />
                                    <asp:Button runat="server" id="toggleCreate" CssClass="btn btn-default" OnClick="toggleCreate_Click" Text="Create New Account" ClientIDMode="Static"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

  
</asp:Content>
