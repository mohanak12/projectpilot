<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Display.aspx.cs" Inherits="ProjectPilot.Log4NetBrowser.Views.Home.Display" %>
<%@ Import Namespace="ProjectPilot.Extras.LogParser"%>

<asp:Content ID="displayContent" ContentPlaceHolderID="MainContent" runat="server">
    <%
        foreach (LogEntry logEntry in this.ParserContent.LineParse.ElementsLog)
        {
            //Response.Write(((ParsedElementBase)logEntry.Elements[0]).Element.ToString());
            Response.Write(this.LogEntryToString(logEntry,(List<string>) this.ParserContent.LineParse.ElementsPattern));
            %><br /> <%
        }
     %>
</asp:Content>
