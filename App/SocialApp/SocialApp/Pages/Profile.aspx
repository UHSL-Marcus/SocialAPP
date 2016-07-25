<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="SocialApp.Pages.Profile" %>



<asp:Content ID="StyleContent" ContentPlaceHolderID="StyleContent" runat="server">
    <link rel="stylesheet" type="text/css" href="/Content/Pages/Profile.css">
    <link rel="stylesheet" type="text/css" href="/Content/Pages/Create_Profile_Common.css">
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">   
    <script type="text/javascript" src="/Scripts/Pages/Profile.js"></script> 
</asp:Content>




<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server" >
    <asp:UpdatePanel runat="server" ID="profileUpdatePanel" class="fill flex-container">
        <ContentTemplate>
            <input type="hidden" id="selected_heading" runat="server" value="personal" ClientIDMode="Static" />
            <!-- Profile Page -->
            <div id="ProfilePage" class="content-padding flex-container flex-column flex-1" style="background-color: forestgreen">
                <Label runat="server" ID="profileResult"/> 
                <div class="flex-1 flex-container flex-column flex-align-center" style="background-color: white">
                    <div class="flex-container">
                        <div id="personal-heading" class="section-heading">PERSONAL </div>
                        <div id="login-heading" class="section-heading">LOGIN </div>
                        <div id="contact-heading" class="section-heading">CONTACT </div>
                        <div id="lifestyle-heading" class="section-heading">LIFESTYLE</div>
                    </div>
                    <div="flex-container flex-column">
                        <div id="personal-content" class="secton-content hidden">
                            <div class="flex-container">
                                <input type="text" runat="server" id="profilePersonalFName" class="text-input" placeholder="First Name" ClientIDMode="Static" />
                                <span class="glyphicon glyphicon-ok hidden glyph-padding"></span>
                                <span class="glyphicon glyphicon-remove hidden glyph-padding"></span>
                            </div>
                            <div class="flex-container">
                                <input type="text" runat="server" id="profilePersonalLName" class="text-input" placeholder="Last Name" ClientIDMode="Static" />
                                <span class="glyphicon glyphicon-ok hidden glyph-padding"></span>
                                <span class="glyphicon glyphicon-remove hidden glyph-padding"></span>
                            </div>
                            <div class="flex-container gender-group">
                                <div class="gender-input flex-container flex-justify-center flex-align-center flex-1">
                                    <input type="radio" value="None" name="gender" id="gender-male" checked/>
                                    <label for="male" class="gender-label" >Male</label>
                                </div>
                                <div class="gender-input flex-container flex-justify-center flex-align-center flex-1">
                                    <input type="radio" value="None" name="gender" id="gender-female"/>
                                    <label for="female" class="gender-label">Female</label>
                                </div>
                            </div>
                            <div class="misc-text">Date of birth</div>
                            <div class="flex-1 flex-container">
                                <select id="profileSelDay" class="flex-1 dob-input"></select>
                                <asp:DropDownList runat="server" id="profileSelMonth" Cssclass="flex-1 dob-input" ClientIDMode="Static" />
                                <asp:DropDownList runat="server" id="profileSelYear" CssClass="flex-1 dob-input" ClientIDMode="Static" />
                                <input runat="server" id="profileSelHiddenDay" class="" type="hidden" ClientIDMode="Static" />
                            </div> 
                        </div>
                        <div id="login-content" class="secton-content hidden">LOGIN</div>
                        <div id="contact-content" class="secton-content hidden">CONTACT</div>
                        <div id="lifestyle-content" class="secton-content hidden">LIFESTYLE</div>
                    </div>               
                </div>
            </div>  
        </ContentTemplate>
    </asp:UpdatePanel>
        
</asp:Content>
