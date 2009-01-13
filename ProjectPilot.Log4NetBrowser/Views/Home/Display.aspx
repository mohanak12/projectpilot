﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Display.aspx.cs" Inherits="ProjectPilot.Log4NetBrowser.Views.Home.Display" %>
<%@ Import Namespace="ProjectPilot.Extras.LogParser"%>

<asp:Content ID="displayContent" ContentPlaceHolderID="MainContent" runat="server">
    <%
        CalculateTableWidth(ParserContent.LineParse.ElementsPattern);
    %>
    <div>
        <style type="text/css">
            a:link {text-decoration: none}
            a:visited {text-decoration: none}
            a:active {text-decoration: none}
            a:hover {text-decoration: underline; color: black;}
        </style>
        <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <%for (int i = 0; i < TableWidths.Count(); i++)
              {%>
                <td width="<%Response.Write(TableWidths[i].ToString());%>px">&nbsp</td>
            <%}%>
        </tr>
           <%
              int idx = 0;
               foreach (LogEntry logEntry in ParserContent.LineParse.ElementsLog) {%>
                <tr>
                 <%Response.Write(LogEntryToString(logEntry, idx, (int)ViewData["Index"]));
                   idx++;%>
                </tr>
            <%}%>
        </table>
    </div>
</asp:Content>