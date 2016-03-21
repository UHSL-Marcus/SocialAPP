﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>

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
                    <asp:ScriptReference Name="bootstrap" />
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
                    <asp:ScriptReference Path="~/bundles/Pages" />
                    <asp:ScriptReference Path="~/bundles/Utils" />
                    <asp:ScriptReference Path="https://maps.googleapis.com/maps/api/js?key=AIzaSyDjcLgEOEkdQfwxEyGOu4hHlSY_s-LOGkQ&libraries=places" />
                </Scripts>
            </asp:ScriptManager>
            <div class="fill flex-container flex-column">
                
                <asp:updatepanel runat="server" id="MainContentUP" UpdateMode="Conditional" class="flex-1 flex-container flex-column">
                    <Triggers>
                    </Triggers>
                    <ContentTemplate>
                        <div id="mouseoverHeaderDiv"></div>
                        <div id="header" class="flex-container headerOverlay">
                            <div class="flex-container">
                                <asp:ImageButton runat="server" ID="homeBtn" ImageUrl="/Resources/Icons/backArrow.png" CssClass="allowPointer headerBackWidth" OnClick="homeBtn_Click" ClientIDMode="Static" />
                            </div>
                            
                            <div class="flex-1"></div>

                            <div class="flex-container flex-column">
                                <div class="flex-container flex-centre">
                                    <asp:LinkButton runat="server" ID="userNameHeaderLbl" CssClass="nonAnimatedLink allowPointer" Text="Not Signed In" OnClick="profileBtn_Click" ClientIDMode="Static" ></asp:LinkButton>
                                    <asp:ImageButton runat="server" ID="headerProfileBtn" ImageUrl="/Resources/Icons/03-Profile.png" CssClass="allowPointer headerProfileWidth" OnClick="profileBtn_Click" ClientIDMode="Static" />
                                </div>
                                <div class="flex-container">
                                    <div class="flex-1"></div>
                                    <div id="signoutBtnContainer" class="flex-container flex-centre headerProfileWidth">
                                        <asp:LinkButton runat="server" ID="signOutBtn" Text="Sign Out" CssClass="nonAnimatedLink allowPointer" OnClick="signOutBtn_Click" ClientIDMode="Static" ></asp:LinkButton>
                                    </div>
                            
                                </div>
                            </div>
                        </div>  
                        <div id="mouseoverHeaderHideDiv"></div>

                        

                        <div runat="server" id="MenuButtons" class="flex-1 flex-container flex-centre">
                            <div class="flex-container">
                                <div class="flex-1"></div>
                                <asp:ImageButton runat="server" ID="profileBtn" ImageUrl="/Resources/Icons/03-Profile.png" CssClass="homePgBtn"  OnClick="profileBtn_Click"  ClientIDMode="Static" /> 
                                <asp:ImageButton runat="server" ID="statsBtn" ImageUrl="/Resources/Icons/02-Stats.png" CssClass="homePgBtn"  OnClick="statsBtn_Click"  ClientIDMode="Static" /> 
                                <asp:ImageButton runat="server" ID="mapsBtn" ImageUrl="/Resources/Icons/04-Maps.png" CssClass="homePgBtn"  OnClick="mapsBtn_Click"  ClientIDMode="Static" /> 
                                <div class="flex-1"></div>
                            </div>
                        </div>
                        <asp:PlaceHolder ID="ContentPlaceholder" runat="server">
                        </asp:PlaceHolder>

                        

                        <div class="modal fade" id="loginModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
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
                </asp:updatepanel>
                <div id="footer" class="footerOverlay">
                    Words <br/> more word
                </div>
                
            </div>
           
            
            

        
         
        </form>
        <div class="modal fade" id="loadingModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div id="loadingModalBody" class="modal-body">
                        Loading...
                    </div>
                </div>
            </div>
        </div>
        

    </body>
</html>
