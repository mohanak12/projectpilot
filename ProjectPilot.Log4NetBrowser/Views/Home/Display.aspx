<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Display.aspx.cs" Inherits="ProjectPilot.Log4NetBrowser.Views.Home.Display" %>
<%@ Import Namespace="ProjectPilot.Extras.LogParser"%>

<asp:Content ID="displayContent" ContentPlaceHolderID="MainContent" runat="server">
    <%
        List<int> widths = CalculateTableWidth(ParserContent.LineParse.ElementsPattern);
    %>
    <table border="0" align="left" cellpadding="0" cellspacing="0">
    <tr>
        <%for (int i = 0; i < widths.Count(); i++)
          {%>
            <td width="<%Response.Write(widths[i].ToString());%>px">&nbsp</td>
        <%}%>
    </tr>
        <%foreach (LogEntry logEntry in this.ParserContent.LineParse.ElementsLog) {%>
            <tr>
             <%Response.Write(this.LogEntryToString(logEntry, (List<string>)this.ParserContent.LineParse.ElementsPattern));%>
            </tr>
        <%}%>
    </table>
</asp:Content>
