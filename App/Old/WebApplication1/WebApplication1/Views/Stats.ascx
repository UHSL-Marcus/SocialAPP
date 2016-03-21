<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Stats.ascx.cs" Inherits="WebApplication1.Views.Stats" %>
<asp:UpdatePanel ID="StatsMainUpdatePanel" runat="server" UpdateMode="Conditional" OnUnload="UpdatePanel_Unload" class="flex-container flex-1">
    <ContentTemplate>
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
            

            <div class="tab-content flex-container flex-1 content-padding flex-centre">
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
        <!-- Data Inputs -->
        <!-- Profile vs Town -->      
        <div id="profileVsTownData" class="hidden">      
            <input runat="server" class="statInput right-align" type="text" id="PvSCat1" ClientIDMode ="Static" />
            <input runat="server" class="statInput right-align" type="text" id="PvSCat1T" ClientIDMode ="Static" />
            <label id="PvSCatL1" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PvSCat1T"></label>

            <input runat="server" class="statInput right-align" type="text" id="PvSCat2" ClientIDMode ="Static" />
            <input runat="server" class="statInput right-align" type="text" id="PvSCat2T" ClientIDMode ="Static" />
            <label id="PvSCatL2" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PvSCat2T"></label>

            <input runat="server" class="statInput right-align" type="text" id="PvSCat3" ClientIDMode ="Static" />
            <input runat="server" class="statInput right-align" type="text" id="PvSCat3T" ClientIDMode ="Static" />
            <label id="PvSCatL3" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PvSCat3T"></label>

            <input runat="server" class="statInput right-align" type="text" id="PvSCat4" ClientIDMode ="Static" />
            <input runat="server" class="statInput right-align" type="text" id="PvSCat4T" ClientIDMode ="Static" />
            <label id="PvSCatL4" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PvSCat4T"></label>

            <input runat="server" class="statInput right-align" type="text" id="PvSCat5" ClientIDMode ="Static" />
            <input runat="server" class="statInput right-align" type="text" id="PvSCat5T" ClientIDMode ="Static" />
            <label id="PvSCatL5" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PvSCat5T"></label>

            <input runat="server" class="statInput right-align" type="text" id="PvSCat6" ClientIDMode ="Static" />
            <input runat="server" class="statInput right-align" type="text" id="PvSCat6T" ClientIDMode ="Static" />
            <label id="PvSCatL6" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PvSCat6T"></label>

            <input runat="server" class="statInput right-align" type="text" id="PvSCat7" ClientIDMode ="Static" />
            <input runat="server" class="statInput right-align" type="text" id="PvSCat7T" ClientIDMode ="Static" />
            <label id="PvSCatL7" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PvSCat7T"></label>

            <input runat="server" class="statInput right-align" type="text" id="PvSCat8" ClientIDMode ="Static" />
            <input runat="server" class="statInput right-align" type="text" id="PvSCat8T" ClientIDMode ="Static" />
            <label id="PvSCatL8" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PvSCat8T"></label>

            <input runat="server" class="statInput right-align" type="text" id="PvSCat9" ClientIDMode ="Static" />
            <input runat="server" class="statInput right-align" type="text" id="PvSCat9T" ClientIDMode ="Static" />
            <label id="PvSCatL9" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PvSCat9T"></label>

            <input runat="server" class="statInput right-align" type="text" id="PvSCat10" ClientIDMode ="Static" />
            <input runat="server" class="statInput right-align" type="text" id="PvSCat10T" ClientIDMode ="Static" />
            <label id="PvSCatL10" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PvSCat10T"></label>

            <input runat="server" class="statInput right-align" type="text" id="PvSCat11" ClientIDMode ="Static" />
            <input runat="server" class="statInput right-align" type="text" id="PvSCat11T" ClientIDMode ="Static" />
            <label id="PvSCatL11" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PvSCat11T"></label>

            <input runat="server" class="statInput right-align" type="text" id="PvSCat12" ClientIDMode ="Static" />
            <input runat="server" class="statInput right-align" type="text" id="PvSCat12T" ClientIDMode ="Static" />
            <label id="PvSCatL12" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PvSCat12T"></label>

            <input runat="server" class="statInput right-align" type="text" id="PvSCat13" ClientIDMode ="Static" />
            <input runat="server" class="statInput right-align" type="text" id="PvSCat13T" ClientIDMode ="Static" />
            <label id="PvSCatL13" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PvSCat13T"></label>

            <input runat="server" class="statInput right-align" type="text" id="PvSCat14" ClientIDMode ="Static" />
            <input runat="server" class="statInput right-align" type="text" id="PvSCat14T" ClientIDMode ="Static" />
            <label id="PvSCatL14" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PvSCat14T"></label>

            <input runat="server" class="statInput right-align" type="text" id="PvSCat15" ClientIDMode ="Static" />
            <input runat="server" class="statInput right-align" type="text" id="PvSCat15T" ClientIDMode ="Static" />
            <label id="PvSCatL15" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PvSCat15T"></label>

            <input runat="server" class="statInput right-align" type="text" id="PvSCat16" ClientIDMode ="Static" />
            <input runat="server" class="statInput right-align" type="text" id="PvSCat16T" ClientIDMode ="Static" />
            <label id="PvSCatL16" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PvSCat16T"></label>

            <input runat="server" class="statInput right-align" type="text" id="PvSCat17" ClientIDMode ="Static" />
            <input runat="server" class="statInput right-align" type="text" id="PvSCat17T" ClientIDMode ="Static" />
            <label id="PvSCatL17" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PvSCat17T"></label>
        </div>
        <!-- Physcial Services-->
        <div id="physServicesData" class="hidden">
            <input runat="server" class="statInput right-align" type="text" id="PCat1" ClientIDMode ="Static" />
            <label id="PCatL1" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PCat1T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="PCat2" ClientIDMode ="Static" />
            <label id="PCatL2" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PCat2T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="PCat3" ClientIDMode ="Static" />
            <label id="PCatL3" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PCat3T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="PCat4" ClientIDMode ="Static" />
            <label id="PCatL4" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PCat4T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="PCat5" ClientIDMode ="Static" />
            <label id="PCatL5" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PCat5T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="PCat6" ClientIDMode ="Static" />
            <label id="PCatL6" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PCat6T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="PCat7" ClientIDMode ="Static" />
            <label id="PCatL7" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PCat7T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="PCat8" ClientIDMode ="Static" />
            <label id="PCatL8" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PCat8T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="PCat9" ClientIDMode ="Static" />
            <label id="PCatL9" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PCat9T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="PCat10" ClientIDMode ="Static" />
            <label id="PCatL10" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PCat10T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="PCat11" ClientIDMode ="Static" />
            <label id="PCatL11" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PCat11T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="PCat12" ClientIDMode ="Static" />
            <label id="PCatL12" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PCat12T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="PCat13" ClientIDMode ="Static" />
            <label id="PCatL13" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PCat13T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="PCat14" ClientIDMode ="Static" />
            <label id="PCatL14" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PCat14T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="PCat15" ClientIDMode ="Static" />
            <label id="PCatL15" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PCat15T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="PCat16" ClientIDMode ="Static" />
            <label id="PCatL16" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PCat16T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="PCat17" ClientIDMode ="Static" />
            <label id="PCatL17" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="PCat17T"></label>                     
        </div>
        <!-- Virtual Services -->
        <div id="virtServicesData" class="hidden">
            <input runat="server" class="statInput right-align" type="text" id="VCat1" ClientIDMode ="Static" />
            <label id="VCatL1" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="VCat1T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="VCat2" ClientIDMode ="Static" />
            <label id="VCatL2" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="VCat2T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="VCat3" ClientIDMode ="Static" />
            <label id="VCatL3" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="VCat3T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="VCat4" ClientIDMode ="Static" />
            <label id="VCatL4" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="VCat4T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="VCat5" ClientIDMode ="Static" />
            <label id="VCatL5" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="VCat5T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="VCat6" ClientIDMode ="Static" />
            <label id="VCatL6" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="VCat6T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="VCat7" ClientIDMode ="Static" />
            <label id="VCatL7" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="VCat7T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="VCat8" ClientIDMode ="Static" />
            <label id="VCatL8" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="VCat8T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="VCat9" ClientIDMode ="Static" />
            <label id="VCatL9" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="VCat9T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="VCat10" ClientIDMode ="Static" />
            <label id="VCatL10" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="VCat10T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="VCat11" ClientIDMode ="Static" />
            <label id="VCatL11" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="VCat11T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="VCat12" ClientIDMode ="Static" />
            <label id="VCatL12" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="VCat12T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="VCat13" ClientIDMode ="Static" />
            <label id="VCatL13" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="VCat13T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="VCat14" ClientIDMode ="Static" />
            <label id="VCatL14" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="VCat14T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="VCat15" ClientIDMode ="Static" />
            <label id="VCatL15" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="VCat15T"></label>
                               
            <input runat="server" class="statInput right-align" type="text" id="VCat16" ClientIDMode ="Static" />
            <label id="VCatL16" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="VCat16T"></label>
                                
            <input runat="server" class="statInput right-align" type="text" id="VCat17" ClientIDMode ="Static" />
            <label id="VCatL17" runat="server" class="control-label right-align profile-slider-label" ClientIDMode ="Static" for="VCat17T"></label>
        </div>



    </ContentTemplate>
</asp:UpdatePanel>