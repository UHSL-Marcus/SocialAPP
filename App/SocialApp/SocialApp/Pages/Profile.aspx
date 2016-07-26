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
                        <div id="personal_heading" class="section-heading">PERSONAL </div>
                        <div id="login_heading" class="section-heading">LOGIN </div>
                        <div id="contact_heading" class="section-heading">CONTACT </div>
                        <div id="lifestyle_heading" class="section-heading section-hide-next">LIFESTYLE</div>
                    </div>
                    <div="flex-container flex-column">
                        <div id="personal_content" class="secton-content hidden">
                            <div class="flex-container">
                                <input type="text" runat="server" id="profilePersonalFName" class="text-input" placeholder="First Name" ClientIDMode="Static" />
                                <span class="glyphicon glyphicon-ok hidden glyph-padding-textbox"></span>
                                <span class="glyphicon glyphicon-remove hidden glyph-padding-textbox"></span>
                            </div>
                            <div class="flex-container">
                                <input type="text" runat="server" id="profilePersonalLName" class="text-input" placeholder="Last Name" ClientIDMode="Static" />
                                <span class="glyphicon glyphicon-ok hidden glyph-padding-textbox"></span>
                                <span class="glyphicon glyphicon-remove hidden glyph-padding-textbox"></span>
                            </div>
                            <div class="flex-container gender-group">
                                <div class="gender-input flex-container flex-justify-center flex-align-center flex-1">
                                    <input type="radio" value="None" name="gender" id="gender_male" runat="server" checked/>
                                    <label for="male" class="gender-label" >Male</label>
                                </div>
                                <div class="gender-input flex-container flex-justify-center flex-align-center flex-1">
                                    <input type="radio" value="None" name="gender" id="gender_female" runat="server"/>
                                    <label for="female" class="gender-label">Female</label>
                                </div>
                            </div>
                            <div class="misc-text">Date of birth</div>
                            <div class="flex-1 flex-container">
                                <select id="profileSelDay" class="flex-1 dob-input"></select>
                                <asp:DropDownList runat="server" id="profileSelMonth" Cssclass="flex-1 dob-input" ClientIDMode="Static" />
                                <asp:DropDownList runat="server" id="profileSelYear" CssClass="flex-1 dob-input" ClientIDMode="Static" />
                                <input runat="server" id="profileSelHiddenDay" class="" type="hidden" ClientIDMode="Static" />
                                <span class="glyphicon glyphicon-ok hidden glyph-padding-dropdown"></span>
                                <span class="glyphicon glyphicon-remove hidden glyph-padding-dropdown"></span>
                            </div> 
                        </div>
                        <div id="login_content" class="secton-content hidden">
                           <div class="flex-container">
                                <input type="text" runat="server" id="profileEmail" class="text-input" placeholder="Email" ClientIDMode ="Static" />
                                <span class="glyphicon glyphicon-ok hidden glyph-padding-textbox"></span>
                                <span class="glyphicon glyphicon-remove hidden glyph-padding-textbox"></span>
                            </div>
                            <div class="flex-container">
                                <input type="text" runat="server" id="profileUsername" class="text-input" placeholder="Username" ClientIDMode ="Static" />
                                <span class="glyphicon glyphicon-ok hidden glyph-padding-textbox"></span>
                                <span class="glyphicon glyphicon-remove hidden glyph-padding-textbox"></span>
                            </div>
                            <div class="flex-container">
                                <input type="text" runat="server" id="profileChangePassword" class="text-input" placeholder="New Password" ClientIDMode ="Static" />
                                <span class="glyphicon glyphicon-ok hidden glyph-padding-textbox"></span>
                                <span class="glyphicon glyphicon-remove hidden glyph-padding-textbox"></span>
                            </div>
                            <div class="flex-container">
                                <input type="text" runat="server" id="profileConfPassword" class="text-input" placeholder="Confirm Password" ClientIDMode ="Static" />
                                <span class="glyphicon glyphicon-ok hidden glyph-padding-textbox"></span>
                                <span class="glyphicon glyphicon-remove hidden glyph-padding-textbox"></span>
                            </div>
                        </div>
                        <div id="contact_content" class="secton-content hidden">
                            <div class="flex-container">
                                <input type="text" runat="server" id="profileHouseNumber" class="text-input" placeholder="House Number" ClientIDMode ="Static" />
                                <span class="glyphicon glyphicon-ok hidden glyph-padding-textbox"></span>
                                <span class="glyphicon glyphicon-remove hidden glyph-padding-textbox"></span>
                            </div>
                            <div class="flex-container">
                                <input type="text" runat="server" id="profileStreet" class="text-input" placeholder="Street" ClientIDMode ="Static" />
                                <span class="glyphicon glyphicon-ok hidden glyph-padding-textbox"></span>
                                <span class="glyphicon glyphicon-remove hidden glyph-padding-textbox"></span>
                            </div>
                            <div class="flex-container">
                                <input type="text" runat="server" id="profileTown" class="text-input" placeholder="Town" ClientIDMode ="Static" />
                                <span class="glyphicon glyphicon-ok hidden glyph-padding-textbox"></span>
                                <span class="glyphicon glyphicon-remove hidden glyph-padding-textbox"></span>
                            </div>
                            <div class="flex-container">
                                <input type="text" runat="server" id="profilePostcode" class="text-input" placeholder="Post Code" ClientIDMode ="Static" />
                                <span class="glyphicon glyphicon-ok hidden glyph-padding-textbox"></span>
                                <span class="glyphicon glyphicon-remove hidden glyph-padding-textbox"></span>
                            </div>
                            <div class="flex-container">
                                <input type="text" runat="server" id="profileTel" class="text-input" placeholder="Telephone Number" ClientIDMode ="Static" />
                                <span class="glyphicon glyphicon-ok hidden glyph-padding-textbox"></span>
                                <span class="glyphicon glyphicon-remove hidden glyph-padding-textbox"></span>
                            </div>
                        </div>
                        <div id="lifestyle_content" class="secton-content hidden" runat="server" ClientIDMode="Static">
                            
                            
                        </div>
                        <input type="text" runat="server" ClientIDMode="Static" id="lifestyle_info"/>
                        <div class="flex-container">
                            <input type="button" id="section_next_btn" value="Next" />
                            <asp:Button runat="server" ID="profileUpdateBtn" ClientIDMode="Static" Text="Update" OnClick="updateProfile_Click"/>
                            <asp:Label runat="server" ID="updateProfileMessage"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>  
        </ContentTemplate>
    </asp:UpdatePanel>
        
</asp:Content>
