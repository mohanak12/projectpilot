<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Ndc.aspx.cs" Inherits="ProjectPilot.Log4NetBrowser.Views.Home.Ndc" %>

<asp:Content ID="displayContent" ContentPlaceHolderID="MainContent" runat="server">
<%= Html.Encode(ViewData["Data"]) %>
</asp:Content>
