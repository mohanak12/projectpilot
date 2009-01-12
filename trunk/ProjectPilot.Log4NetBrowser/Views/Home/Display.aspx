<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Display.aspx.cs" Inherits="ProjectPilot.Log4NetBrowser.Views.Home.Display" %>
<%@ Import Namespace="ProjectPilot.Extras.LogParser"%>

<asp:Content ID="displayContent" ContentPlaceHolderID="MainContent" runat="server">
    
    
    <%
        {
            List<int> widths;
        }
%>
    <table border="0" align="left" cellpadding="0" cellspacing="0">
    <tr>
        <td width="130px">row 1, cell 1</td>
        <td width="50px">row 1, cell 2</td>
    </tr>
    <tr>
        <td>row 2, cell 1</td>
        <td>row 2, cell 2</td>
    </tr>
    </table>
    <br /><br />
    <%
    foreach (LogEntry logEntry in this.ParserContent.LineParse.ElementsLog)
    {
        //Response.Write(((ParsedElementBase)logEntry.Elements[0]).Element.ToString());
        Response.Write(this.LogEntryToString(logEntry, (List<string>)this.ParserContent.LineParse.ElementsPattern));
            %><br /> <%
    }
     %>
</asp:Content>
