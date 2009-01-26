<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ProjectPilot.Log4NetBrowser.Views.Home.Index" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.Encode(ViewData["Message"]) %></h2>
    <p>
    <%= Html.ActionLink("Select File","FileSelect","Home")%>
    </p>
</asp:Content>
