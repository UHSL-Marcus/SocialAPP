<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SocialApp._Default" %>

<asp:Content ID="StyleContent" ContentPlaceHolderID="StyleContent" runat="server">
   <link rel="stylesheet" type="text/css" href="/Content/Pages/Default.css">
</asp:Content>


<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
    <!--<script type="text/javascript" src="/Scripts/Pages/Default.js"></script> -->
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="flex-1 flex-container flex-center"> 
        <asp:ImageButton runat="server" ID="profileBtn" ImageUrl="/Resources/Icons/03-Profile.png" CssClass="defaultNavBtn"  OnClick="profileBtn_Click"  ClientIDMode="Static" /> 
        <asp:ImageButton runat="server" ID="statsBtn" ImageUrl="/Resources/Icons/02-Stats.png" CssClass="defaultNavBtn"  OnClick="statsBtn_Click"  ClientIDMode="Static" /> 
        <asp:ImageButton runat="server" ID="mapsBtn" ImageUrl="/Resources/Icons/04-Maps.png" CssClass="defaultNavBtn"  OnClick="mapsBtn_Click"  ClientIDMode="Static" /> 
    </div>

  
</asp:Content>
