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
         
            <!-- Profile Page -->
            <div id="ProfilePage" class="content-padding flex-container flex-column flex-1">
                <Label runat="server" ID="profileResult"/> 
                <div class="flex-1 flex-container">
                    <div role="tabpanel">
                        <!-- Nav tabs -->
                        <ul class="nav nav-pills nav-stacked" id="profileTabs">
                            <li class="active text-center"><a href="#profilePersonal" aria-controls="personal" role="tab" data-toggle="tab">Personal <span id="profilePersonalError" class="label label-danger"></span></a></li>
                            <li class="text-center"><a href="#profileLogin" aria-controls="profile" role="tab" data-toggle="tab">Login <span id="profileLoginError" class="label label-danger"></span></a></li>
                            <li class="text-center"><a href="#profileContact" aria-controls="contact" role="tab" data-toggle="tab">Contact <span id="profileContactError" class="label label-danger"></span></a></li>
                            <li class="text-center"><a href="#profileLifestyle" aria-controls="lifestyle" role="tab" data-toggle="tab">Lifestyle <span id="profileLifestyleError" class="label label-danger"></span></a></li>
                        </ul>
                    </div>
                    <!-- Tab panes -->
                    <div class=" flex-1 tab-content flex-container content-padding flex-center" id="profileTabsContent">
                        <!-- Personal -->
                        <div role="tabpanel" class="tab-pane active profile-box" id="profilePersonal">
                            <h2>Personal</h2>
                            <div class="form-horizontal">
                                <div class="form-group form has-feedback">
                                    <input type="text" runat="server" id="profilePersonalFName" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align form-label" for="profilePersonalFName">First Name</label>
                                </div>

                                <div class="form-group form has-feedback">
                                    <input type="text" runat="server" id="profilePersonalLName" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align form-label" for="profilePersonalLName" >Last Name</label>
                                </div>

                                <div class="form-group form">
                                    <asp:DropDownList runat="server" id="profileSelGender" CssClass="form-control profile-gender right-align" ClientIDMode ="Static">
                                        <asp:ListItem Value="Male" Text="Male" />
                                        <asp:ListItem Value="Female" Text="Female" />
                                        <asp:ListItem Value="Other" Text="Other" />
                                    </asp:DropDownList>
                                    <label class="control-label right-align form-label" for="selGender">Gender</label>
                                </div>
     
                                <div class="form-group form">
                                    <asp:DropDownList runat="server" id="profileSelYear" CssClass="form-control profile-date right-align" ClientIDMode="Static" />
                                    <asp:DropDownList runat="server" id="profileSelMonth" Cssclass="form-control profile-date right-align" ClientIDMode="Static" />
                                    <select id="profileSelDay" class="form-control create-date right-align"></select>
                                    <input runat="server" id="profileSelHiddenDay" class="" type="text" ClientIDMode="Static" />
                                    <label id="profileSelDOBLabel" class="control-label right-align form-label" for="selDay" >Date of Birth</label>
                                </div>

                            </div>
                        </div>
                        <!-- Contact -->
                        <div role="tabpanel" class="tab-pane profile-box" id="profileContact">
                            <h2>Contact</h2>
                            <div class="form-horizontal">
                                <div class="form-group form has-feedback">
                                    <input type="text" runat="server" id="profileHouseNumber" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align form-label" for="contactHouseNumber">House Name/Number</label>
                                </div>

                                <div class="form-group form has-feedback">
                                    <input type="text" runat="server" id="profileStreet" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align form-label" for="contactStreet">Street</label>
                                </div>

                                <div class="form-group form has-feedback">
                                    <input type="text" runat="server" id="profileTown" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align form-label" for="contactTown">Town</label>
                                </div>

                                <div class="form-group form has-feedback">
                                    <input type="text" runat="server" id="profilePostcode" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align form-label" for="contactPostcode">Postcode</label>
                                </div>

                                <div class="form-group form has-feedback">
                                    <input type="text" runat="server" id="profileTel" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align form-label" for="contactTel">Telephone Number</label>
                                </div>
                            </div>
                        </div>
                        <!-- Login -->
                        <div role="tabpanel" class="tab-pane profile-box" id="profileLogin">
                            <h2>Login</h2>
                            <div class="form-horizontal">
                                <div class="form-group form has-feedback">
                                    <input type="text" runat="server" id="profileEmail" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align form-label" for="loginEmail">Email</label>
                                </div>

                                <div class="form-group form has-feedback">
                                    <input type="text" runat="server" id="profileUsername" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align form-label" for="loginUsername">Username</label>
                                </div>

                                <div class="form-group form has-feedback">
                                    <input type="text" runat="server" id="profileChangePassword" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align form-label" for="loginPassword">Change Password</label>
                                </div>

                                <div class="form-group form has-feedback">
                                    <input type="text" runat="server" id="profileConfPassword" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align form-label" for="loginPassword">Confirm Password</label>
                                </div>
                            </div>
                        </div>
                        <!-- Lifestyle -->
                        <div role="tabpanel" class="tab-pane create-lifetyle-form content-padding" id="profileLifestyle">
                            <div class="flex-container flex-column fill">
                                <div>
                                    <h2>Lifestyle</h2>
                                </div>
                                <div class="flex-1">
                                    <div class="flex-container">
                                        <div class ="flex-1"></div>
                                        <div class="form-horizontal create-lifestyle content-padding">
                                            <div class="form-group form">
                                                <input runat="server" class="slider right-align" type="text" id="profileCat1" />
                                                <label id="profileCatLabel1" runat="server" class="control-label right-align slider-label" for="profileCat1">Cat1</label>
                                            </div>
                                            <div class="form-group form">
                                                <input runat="server" class="slider right-align" type="text" id="profileCat2" />
                                                <label id="profileCatLabel2" runat="server" class="control-label right-align slider-label" for="profileCat2">Cat1</label>
                                            </div>
                                            <div class="form-group form">
                                                <input runat="server" class="slider right-align" type="text" id="profileCat3" />
                                                <label id="profileCatLabel3" runat="server" class="control-label right-align slider-label" for="profileCat3">Cat1</label>
                                            </div>
                                            <div class="form-group form">
                                                <input runat="server" class="slider right-align" type="text" id="profileCat4" />
                                                <label id="profileCatLabel4" runat="server" class="control-label right-align slider-label" for="profileCat4">Cat1</label>
                                            </div>
                                            <div class="form-group form">
                                                <input runat="server" class="slider right-align" type="text" id="profileCat5" />
                                                <label id="profileCatLabel5" runat="server" class="control-label right-align slider-label" for="profileCat5">Cat1</label>
                                            </div>
                                            <div class="form-group form">
                                                <input runat="server" class="slider right-align" type="text" id="profileCat6" />
                                                <label id="profileCatLabel6" runat="server" class="control-label right-align slider-label" for="profileCat6">Cat1</label>
                                            </div>
                                            <div class="form-group form">
                                                <input runat="server" class="slider right-align" type="text" id="profileCat7" />
                                                <label id="profileCatLabel7" runat="server" class="control-label right-align slider-label" for="profileCat7">Cat1</label>
                                            </div>
                                            <div class="form-group form">
                                                <input runat="server" class="slider right-align" type="text" id="profileCat8" />
                                                <label id="profileCatLabel8" runat="server" class="control-label right-align slider-label" for="profileCat8">Cat1</label>
                                            </div>
                                            <div class="form-group form">
                                                <input runat="server" class="slider right-align" type="text" id="profileCat9" />
                                                <label id="profileCatLabel9" runat="server" class="control-label right-align slider-label" for="profileCat9">Cat1</label>
                                            </div>
                                        </div>
                                        <div class="form-horizontal create-lifestyle">
                                            <div class="form-group form">
                                                <input runat="server" class="slider right-align" type="text" id="profileCat10" />
                                                <label id="profileCatLabel10" runat="server" class="control-label right-align slider-label" for="profileCat10">Cat1</label>
                                            </div>
                                            <div class="form-group form">
                                                <input runat="server" class="slider right-align" type="text" id="profileCat11" />
                                                <label id="profileCatLabel11" runat="server" class="control-label right-align slider-label" for="profileCat11">Cat1</label>
                                            </div>
                                            <div class="form-group form">
                                                <input runat="server" class="slider right-align" type="text" id="profileCat12" />
                                                <label id="profileCatLabel12" runat="server" class="control-label right-align slider-label" for="profileCat12">Cat1</label>
                                            </div>
                                            <div class="form-group form">
                                                <input runat="server" class="slider right-align" type="text" id="profileCat13" />
                                                <label id="profileCatLabel13" runat="server" class="control-label right-align slider-label" for="profileCat13">Cat1</label>
                                            </div>
                                            <div class="form-group form">
                                                <input runat="server" class="slider right-align" type="text" id="profileCat14" />
                                                <label id="profileCatLabel14" runat="server" class="control-label right-align slider-label" for="profileCat14">Cat1</label>
                                            </div>
                                            <div class="form-group form">
                                                <input runat="server" class="slider right-align" type="text" id="profileCat15" />
                                                <label id="profileCatLabel15" runat="server" class="control-label right-align slider-label" for="profileCat15">Cat1</label>
                                            </div>
                                            <div class="form-group form">
                                                <input runat="server" class="slider right-align" type="text" id="profileCat16" />
                                                <label id="profileCatLabel16" runat="server" class="control-label right-align slider-label" for="profileCat16">Cat1</label>
                                            </div>
                                            <div class="form-group form">
                                                <input runat="server" class="slider right-align" type="text" id="profileCat17" />
                                                <label id="profileCatLabel17" runat="server" class="control-label right-align slider-label" for="profileCat17">Cat1</label>
                                            </div>
                                        </div>
                                        <div class="flex-1"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="flex-container">
                        <div class="flex-1"></div>
                        <div>
                            <asp:Button runat="server" ID="updateProfile" CssClass="btn btn-default" ClientIDMode="Static" Text="Update" OnClick="updateProfile_Click" />
                            <label runat="server" id="updateProfileMessage" ClientIDMode="Static" />
                        </div>
                    </div>
                </div>
            </div>  
        </ContentTemplate>
    </asp:UpdatePanel>
        
</asp:Content>
