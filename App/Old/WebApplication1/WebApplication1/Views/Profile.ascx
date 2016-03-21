<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Profile.ascx.cs" Inherits="WebApplication1.Views.Profile" %>

<asp:UpdatePanel ID="ProfileMainUpdatePanel" runat="server" UpdateMode="Conditional" OnUnload="UpdatePanel_Unload" class="flex-1 flex-container">
    <ContentTemplate>
        <!-- Create Account Page -->
        <div runat="server" id="CreatePage" class="flex-1 content-padding flex-container flex-column" ClientIDMode="Static">
            <div class="text-center">
                <h1>Create</h1>
            </div>
            <div class="flex-1 flex-container">
                <div role="tabpanel">
                    <!-- Nav tabs -->
                    <ul class="nav nav-pills nav-stacked" id="createTabs">
                        <li class="active text-center"><a href="#createPersonal" aria-controls="personal" role="tab" data-toggle="tab">Personal Details <span id="createPersonalError" class="label label-danger"></span></a></li>
                        <li class="text-center"><a href="#createAccount" aria-controls="profile" role="tab" data-toggle="tab">Account Details <span id="createAccountError" class="label label-danger"></span></a></li>
                        <li class="text-center"><a href="#createContact" aria-controls="contact" role="tab" data-toggle="tab">Contact Details <span id="createContactError" class="label label-danger"></span></a></li>
                        <li class="text-center"><a href="#createLifestyle" aria-controls="lifestyle" role="tab" data-toggle="tab">Lifestyle <span id="createLifestyleError" class="label label-danger"></span></a></li>
                    </ul>
                </div>
            
                <!-- Tab panes -->
                <div class="tab-content flex-container flex-center-container content-padding flex-1">
                    <!-- Personal -->
                    <div role="tabpanel" class="tab-pane active create-form" id="createPersonal">
                        <div class="create-form-inner">
                            <div class="form-horizontal">
                                <div class="form-group profile-form has-feedback">
                                    <input type="text" runat="server" id="createPersonalFName" class="form-control create-text-box right-align" ClientIDMode="Static" placeholder="First Name" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align profile-form-label" for="createPersonalFName">First Name</label>
                                </div>

                                <div class="form-group profile-form has-feedback">
                                    <input type="text" runat="server" id="createPersonalLName" class="form-control create-text-box right-align" ClientIDMode="Static" placeholder="Last Name" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align profile-form-label" for="createPersonalLName" >Last Name</label>
                                </div>

                                <div class="form-group profile-form">
                                    <asp:DropDownList runat="server" id="createSelGender" CssClass="form-control create-gender right-align" ClientIDMode ="Static">
                                        <asp:ListItem Value="" Text="" />
                                        <asp:ListItem Value="Male" Text="Male" />
                                        <asp:ListItem Value="Female" Text="Female" />
                                        <asp:ListItem Value="Other" Text="Other" />
                                    </asp:DropDownList>
                                    <label class="control-label right-align profile-form-label" for="createSelGender">Gender</label>
                                </div>

                                <div class="form-group profile-form">
                                    <asp:DropDownList runat="server" id="createSelYear" CssClass="form-control create-date right-align" ClientIDMode="Static" />
                                    <asp:DropDownList runat="server" id="createSelMonth" CssClass="form-control create-date right-align" ClientIDMode="Static" />
                                    <select id="createSelDay" class="form-control create-date right-align"></select>
                                    <input runat="server" id="createSelHiddenDay" class="hidden" type="text" ClientIDMode="Static" />
                                    <label id="createDOBlabel" class="control-label right-align profile-form-label" for="createSelDay">Date of Birth</label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Profile -->
                    <div role="tabpanel" class="tab-pane create-form" id="createAccount">
                        <div class="create-form-inner">
                            <div class="form-horizontal">
                                <div class="form-group profile-form has-feedback">
                                    <input runat="server" type="text" id="createEmail" class="form-control create-text-box right-align" ClientIDMode ="Static" placeholder="Email" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align profile-form-label" for="createEmail">Email</label>
                                </div>
                                <div class="form-group profile-form has-feedback">
                                    <input runat="server" type="text" id="createUsername" class="form-control create-text-box right-align" ClientIDMode ="Static" placeholder="Username" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align profile-form-label" for="createUsername">Username</label>
                                </div>
                                <div class="form-group profile-form has-feedback">
                                    <input runat="server" type="password" id="createPassword" class="form-control create-text-box right-align" ClientIDMode ="Static" placeholder="Password" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align profile-form-label" for="createPassword">Password</label>
                                </div>
                                <div class="form-group profile-form has-feedback">
                                    <input runat="server" type="password" id="createConfPassword" class="form-control create-text-box right-align" ClientIDMode ="Static" placeholder="Confirm Password" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align profile-form-label" for="createConfPassword">Confirm Password</label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!--Lifestyle-->
                    <div role="tabpanel" class="tab-pane create-lifetyle-form" id="createLifestyle">
                        <div class="flex-container">
                            <div class="flex-1"></div>
                            <div class="form-horizontal create-lifestyle content-padding">
                                <div class="form-group profile-form">
                                    <input runat="server" class="profile-slider right-align" type="text" id="createCat1" ClientIDMode ="Static" value="5" />
                                    <label id="createCatL1" runat="server" class="control-label right-align profile-slider-label" for="createCat1">Enviroment</label>
                                </div>
                                <div class="form-group profile-form">
                                    <input runat="server" class="profile-slider right-align" type="text" id="createCat2" ClientIDMode ="Static" value="5" />
                                    <label id="createCatL2" runat="server" class="control-label right-align profile-slider-label" for="createCat2">verylongwordverylong</label>
                                </div>
                                <div class="form-group profile-form">
                                    <input runat="server" class="profile-slider right-align" type="text" id="createCat3" ClientIDMode ="Static" value="5" />
                                    <label id="createCatL3" runat="server" class="control-label right-align profile-slider-label" for="createCat3">Cat1</label>
                                </div>
                                <div class="form-group profile-form">
                                    <input runat="server" class="profile-slider right-align" type="text" id="createCat4" ClientIDMode ="Static" value="5" />
                                    <label id="createCatL4" runat="server" class="control-label right-align profile-slider-label" for="createCat4">Cat1</label>
                                </div>
                                <div class="form-group profile-form">
                                    <input runat="server" class="profile-slider right-align" type="text" id="createCat5" ClientIDMode ="Static" value="5" />
                                    <label id="createCatL5" runat="server" class="control-label right-align profile-slider-label" for="createCat5">Cat1</label>
                                </div>
                                <div class="form-group profile-form">
                                    <input runat="server" class="profile-slider right-align" type="text" id="createCat6" ClientIDMode ="Static" value="5" />
                                    <label id="createCatL6" runat="server" class="control-label right-align profile-slider-label" for="createCat6">Cat1</label>
                                </div>
                                <div class="form-group profile-form">
                                    <input runat="server" class="profile-slider right-align" type="text" id="createCat7" ClientIDMode ="Static" value="5" />
                                    <label id="createCatL7" runat="server" class="control-label right-align profile-slider-label" for="createCat7">Cat1</label>
                                </div>
                                <div class="form-group profile-form">
                                    <input runat="server" class="profile-slider right-align" type="text" id="createCat8" ClientIDMode ="Static" value="5" />
                                    <label id="createCatL8" runat="server" class="control-label right-align profile-slider-label" for="createCat8">Cat1</label>
                                </div>
                                <div class="form-group profile-form">
                                    <input runat="server" class="profile-slider right-align" type="text" id="createCat9" ClientIDMode ="Static" value="5" />
                                    <label id="createCatL9" runat="server" class="control-label right-align profile-slider-label" for="createCat9">Cat1</label>
                                </div>
                            </div>
                            <div class="form-horizontal create-lifestyle content-padding">
                                <div class="form-group profile-form">
                                    <input runat="server" class="profile-slider right-align" type="text" id="createCat10" ClientIDMode ="Static" value="5" />
                                    <label id="createCatL10" runat="server" class="control-label right-align profile-slider-label" for="createCat10">Cat1</label>
                                </div>
                                <div class="form-group profile-form">
                                    <input runat="server" class="profile-slider right-align" type="text" id="createCat11" ClientIDMode ="Static" value="5" />
                                    <label id="createCatL11" runat="server" class="control-label right-align profile-slider-label" for="createCat11">Cat1</label>
                                </div>
                                <div class="form-group profile-form">
                                    <input runat="server" class="profile-slider right-align" type="text" id="createCat12" ClientIDMode ="Static" value="5" />
                                    <label id="createCatL12" runat="server" class="control-label right-align profile-slider-label" for="createCat12">Cat1</label>
                                </div>
                                <div class="form-group profile-form">
                                    <input runat="server" class="profile-slider right-align" type="text" id="createCat13" ClientIDMode ="Static" value="5" />
                                    <label id="createCatL13" runat="server" class="control-label right-align profile-slider-label" for="createCat13">Cat1</label>
                                </div>
                                <div class="form-group profile-form">
                                    <input runat="server" class="profile-slider right-align" type="text" id="createCat14" ClientIDMode ="Static" value="5" />
                                    <label id="createCatL14" runat="server" class="control-label right-align profile-slider-label" for="createCat14">Cat1</label>
                                </div>
                                <div class="form-group profile-form">
                                    <input runat="server" class="profile-slider right-align" type="text" id="createCat15" ClientIDMode ="Static" value="5" />
                                    <label id="createCatL15" runat="server" class="control-label right-align profile-slider-label" for="createCat15">Cat1</label>
                                </div>
                                <div class="form-group profile-form">
                                    <input runat="server" class="profile-slider right-align" type="text" id="createCat16" ClientIDMode ="Static" value="5" />
                                    <label id="createCatL16" runat="server" class="control-label right-align profile-slider-label" for="createCat16">Cat1</label>
                                </div>
                                <div class="form-group profile-form">
                                    <input runat="server" class="profile-slider right-align" type="text" id="createCat17" ClientIDMode ="Static" value="5" />
                                    <label id="createCatL17" runat="server" class="control-label right-align profile-slider-label" for="createCat17">Cat1</label>
                                </div>
                            </div>
                            <div class="flex-1"></div>
                        </div>
                    </div>

                    <!--Contact-->
                    <div role="tabpanel" class="tab-pane create-form" id="createContact">
                        <div class="create-form-inner">
                            <div class="form-horizontal">
                                <div class="form-group profile-form has-feedback">
                                    <input runat="server" type="text" id="createHouseNumber" class="form-control create-text-box right-align" ClientIDMode ="Static" placeholder="House Name/Number" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align profile-form-label" for="createHouseNumber">House Name/Number</label>
                                </div>
                                <div class="form-group profile-form has-feedback">
                                    <input runat="server" type="text" id="createStreet" class="form-control create-text-box right-align" ClientIDMode ="Static" placeholder="Street" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align profile-form-label" for="createStreet">Street</label>
                                </div>
                                <div class="form-group profile-form has-feedback">
                                    <input runat="server" type="text" id="createTown" class="form-control create-text-box right-align" ClientIDMode ="Static" placeholder="Town" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align profile-form-label" for="createTown">Town</label>
                                </div>
                                <div class="form-group profile-form has-feedback">
                                    <input runat="server" type="text" id="createPostcode" class="form-control create-text-box right-align" ClientIDMode ="Static" placeholder="Postcode" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align profile-form-label" for="createPostcode">Postcode</label>
                                </div>
                                <div class="form-group profile-form has-feedback">
                                    <input runat="server" type="tel" id="createTel" class="form-control create-text-box right-align" ClientIDMode ="Static" placeholder="Telephone" />
                                    <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                    <label class="control-label right-align profile-form-label" for="createTel">Telephone Number</label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div>
                    <button type="button" class="btn btn-default" data-toggle="modal" data-target="#loginModal">Login</button>
                   </br> <asp:Button runat="server" ID="submitCreate" CssClass="btn btn-default" ClientIDMode="Static" Text="Submit" OnClick="submitCreate_Click" />
                   </br> <label runat="server" id="errorMessage" ClientIDMode="Static" />
                </div>
            </div>
        </div>






        <!-- Profile Page -->
        <div runat="server" id="ProfilePage" class="content-padding flex-container flex-column flex-1" ClientIDMode="Static">
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
                <div class=" flex-1 tab-content flex-container content-padding flex-justify-center" id="profileTabsContent">
                    <!-- Personal -->
                    <div role="tabpanel" class="tab-pane active profile-box" id="profilePersonal">
                        <h2>Personal</h2>
                        <div class="form-horizontal">
                            <div class="form-group profile-form has-feedback">
                                <input type="text" runat="server" id="profilePersonalFName" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                <label class="control-label right-align profile-form-label" for="profilePersonalFName">First Name</label>
                            </div>

                            <div class="form-group profile-form has-feedback">
                                <input type="text" runat="server" id="profilePersonalLName" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                <label class="control-label right-align profile-form-label" for="profilePersonalLName" >Last Name</label>
                            </div>

                            <div class="form-group profile-form">
                                <asp:DropDownList runat="server" id="profileSelGender" CssClass="form-control profile-gender right-align" ClientIDMode ="Static">
                                    <asp:ListItem Value="Male" Text="Male" />
                                    <asp:ListItem Value="Female" Text="Female" />
                                    <asp:ListItem Value="Other" Text="Other" />
                                </asp:DropDownList>
                                <label class="control-label right-align profile-form-label" for="selGender">Gender</label>
                            </div>
     
                            <div class="form-group profile-form">
                                <asp:DropDownList runat="server" id="profileSelYear" CssClass="form-control profile-date right-align" />
                                <asp:DropDownList runat="server" id="profileSelMonth" Cssclass="form-control profile-date right-align" />
                                <select id="profileSelDay" class="form-control create-date right-align"></select>
                                <input runat="server" id="profileSelHiddenDay" class="hidden" type="text" ClientIDMode="Static" />
                                <label id="profileSelDOBLabel" class="control-label right-align profile-form-label" for="selDay" >Date of Birth</label>
                            </div>

                        </div>
                    </div>
                    <!-- Contact -->
                    <div role="tabpanel" class="tab-pane profile-box" id="profileContact">
                        <h2>Contact</h2>
                        <div class="form-horizontal">
                            <div class="form-group profile-form has-feedback">
                                <input type="text" runat="server" id="profileHouseNumber" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                <label class="control-label right-align profile-form-label" for="contactHouseNumber">House Name/Number</label>
                            </div>

                            <div class="form-group profile-form has-feedback">
                                <input type="text" runat="server" id="profileStreet" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                <label class="control-label right-align profile-form-label" for="contactStreet">Street</label>
                            </div>

                            <div class="form-group profile-form has-feedback">
                                <input type="text" runat="server" id="profileTown" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                <label class="control-label right-align profile-form-label" for="contactTown">Town</label>
                            </div>

                            <div class="form-group profile-form has-feedback">
                                <input type="text" runat="server" id="profilePostcode" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                <label class="control-label right-align profile-form-label" for="contactPostcode">Postcode</label>
                            </div>

                            <div class="form-group profile-form has-feedback">
                                <input type="text" runat="server" id="profileTel" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                <label class="control-label right-align profile-form-label" for="contactTel">Telephone Number</label>
                            </div>
                        </div>
                    </div>
                    <!-- Login -->
                    <div role="tabpanel" class="tab-pane profile-box" id="profileLogin">
                        <h2>Login</h2>
                        <div class="form-horizontal">
                            <div class="form-group profile-form has-feedback">
                                <input type="text" runat="server" id="profileEmail" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                <label class="control-label right-align profile-form-label" for="loginEmail">Email</label>
                            </div>

                            <div class="form-group profile-form has-feedback">
                                <input type="text" runat="server" id="profileUsername" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                <label class="control-label right-align profile-form-label" for="loginUsername">Username</label>
                            </div>

                            <div class="form-group profile-form has-feedback">
                                <input type="text" runat="server" id="profileChangePassword" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                <label class="control-label right-align profile-form-label" for="loginPassword">Change Password</label>
                            </div>

                            <div class="form-group profile-form has-feedback">
                                <input type="text" runat="server" id="profileConfPassword" class="form-control profile-text-box right-align" ClientIDMode ="Static" />
                                <span class="glyphicon glyphicon-ok form-control-feedback hidden"></span>
                                <span class="glyphicon glyphicon-remove form-control-feedback hidden"></span>
                                <label class="control-label right-align profile-form-label" for="loginPassword">Confirm Password</label>
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
                                        <div class="form-group profile-form">
                                            <input runat="server" class="profile-slider right-align" type="text" id="profileCat1" />
                                            <label id="profileCatLabel1" runat="server" class="control-label right-align profile-slider-label" for="profileCat1">Cat1</label>
                                        </div>
                                        <div class="form-group profile-form">
                                            <input runat="server" class="profile-slider right-align" type="text" id="profileCat2" />
                                            <label id="profileCatLabel2" runat="server" class="control-label right-align profile-slider-label" for="profileCat2">Cat1</label>
                                        </div>
                                        <div class="form-group profile-form">
                                            <input runat="server" class="profile-slider right-align" type="text" id="profileCat3" />
                                            <label id="profileCatLabel3" runat="server" class="control-label right-align profile-slider-label" for="profileCat3">Cat1</label>
                                        </div>
                                        <div class="form-group profile-form">
                                            <input runat="server" class="profile-slider right-align" type="text" id="profileCat4" />
                                            <label id="profileCatLabel4" runat="server" class="control-label right-align profile-slider-label" for="profileCat4">Cat1</label>
                                        </div>
                                        <div class="form-group profile-form">
                                            <input runat="server" class="profile-slider right-align" type="text" id="profileCat5" />
                                            <label id="profileCatLabel5" runat="server" class="control-label right-align profile-slider-label" for="profileCat5">Cat1</label>
                                        </div>
                                        <div class="form-group profile-form">
                                            <input runat="server" class="profile-slider right-align" type="text" id="profileCat6" />
                                            <label id="profileCatLabel6" runat="server" class="control-label right-align profile-slider-label" for="profileCat6">Cat1</label>
                                        </div>
                                        <div class="form-group profile-form">
                                            <input runat="server" class="profile-slider right-align" type="text" id="profileCat7" />
                                            <label id="profileCatLabel7" runat="server" class="control-label right-align profile-slider-label" for="profileCat7">Cat1</label>
                                        </div>
                                        <div class="form-group profile-form">
                                            <input runat="server" class="profile-slider right-align" type="text" id="profileCat8" />
                                            <label id="profileCatLabel8" runat="server" class="control-label right-align profile-slider-label" for="profileCat8">Cat1</label>
                                        </div>
                                        <div class="form-group profile-form">
                                            <input runat="server" class="profile-slider right-align" type="text" id="profileCat9" />
                                            <label id="profileCatLabel9" runat="server" class="control-label right-align profile-slider-label" for="profileCat9">Cat1</label>
                                        </div>
                                    </div>
                                    <div class="form-horizontal create-lifestyle">
                                        <div class="form-group profile-form">
                                            <input runat="server" class="profile-slider right-align" type="text" id="profileCat10" />
                                            <label id="profileCatLabel10" runat="server" class="control-label right-align profile-slider-label" for="profileCat10">Cat1</label>
                                        </div>
                                        <div class="form-group profile-form">
                                            <input runat="server" class="profile-slider right-align" type="text" id="profileCat11" />
                                            <label id="profileCatLabel11" runat="server" class="control-label right-align profile-slider-label" for="profileCat11">Cat1</label>
                                        </div>
                                        <div class="form-group profile-form">
                                            <input runat="server" class="profile-slider right-align" type="text" id="profileCat12" />
                                            <label id="profileCatLabel12" runat="server" class="control-label right-align profile-slider-label" for="profileCat12">Cat1</label>
                                        </div>
                                        <div class="form-group profile-form">
                                            <input runat="server" class="profile-slider right-align" type="text" id="profileCat13" />
                                            <label id="profileCatLabel13" runat="server" class="control-label right-align profile-slider-label" for="profileCat13">Cat1</label>
                                        </div>
                                        <div class="form-group profile-form">
                                            <input runat="server" class="profile-slider right-align" type="text" id="profileCat14" />
                                            <label id="profileCatLabel14" runat="server" class="control-label right-align profile-slider-label" for="profileCat14">Cat1</label>
                                        </div>
                                        <div class="form-group profile-form">
                                            <input runat="server" class="profile-slider right-align" type="text" id="profileCat15" />
                                            <label id="profileCatLabel15" runat="server" class="control-label right-align profile-slider-label" for="profileCat15">Cat1</label>
                                        </div>
                                        <div class="form-group profile-form">
                                            <input runat="server" class="profile-slider right-align" type="text" id="profileCat16" />
                                            <label id="profileCatLabel16" runat="server" class="control-label right-align profile-slider-label" for="profileCat16">Cat1</label>
                                        </div>
                                        <div class="form-group profile-form">
                                            <input runat="server" class="profile-slider right-align" type="text" id="profileCat17" />
                                            <label id="profileCatLabel17" runat="server" class="control-label right-align profile-slider-label" for="profileCat17">Cat1</label>
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
                




