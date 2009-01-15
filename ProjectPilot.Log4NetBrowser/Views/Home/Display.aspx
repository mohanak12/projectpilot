<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Display.aspx.cs" Inherits="ProjectPilot.Log4NetBrowser.Views.Home.Display" %>
<%@ Import Namespace="ProjectPilot.Log4NetBrowser.Models"%>
<%@ Import Namespace="ProjectPilot.Extras.LogParser"%>

<asp:Content ID="displayContent" ContentPlaceHolderID="MainContent" runat="server">
    <%
        ParserContent = ViewData["Content"] as LogDisplay;
        CalculateTableWidth(ParserContent.LineParse.ElementsPattern);
    %>
    <div id="logEntries">
        <table border="0" cellpadding="0" cellspacing="0">
        <tr valign="top">
            <%for (int i = 0; i < TableWidths.Count(); i++)
              {%>
                <td width="<%Response.Write(TableWidths[i].ToString());%>px">&nbsp</td>
            <%}%>
        </tr>
           <%
              int idx = 0;
               foreach (LogEntry logEntry in ParserContent.LineParse.ElementsLog) {%>
                <tr valign="top">
                 <%Response.Write(LogEntryToString(logEntry, idx, ParserContent.IndexList,(int)ViewData["Id"]));
                   idx++;%>
                </tr>
            <%}%>
        </table>
    </div>
</asp:Content>
