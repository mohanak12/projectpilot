<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Display.aspx.cs" Inherits="ProjectPilot.Log4NetBrowser.Views.Home.Display" %>
<%@ Import Namespace="ProjectPilot.Log4NetBrowser.Models"%>
<%@ Import Namespace="ProjectPilot.Extras.LogParser"%>
<%@ Import Namespace="System"%>
<%@ Import Namespace="System.Web"%>
<%@ Import Namespace="System.Web.Mvc"%>

<asp:Content ID="displayContent" ContentPlaceHolderID="MainContent" runat="server">
    <%
        ParserContent = ViewData["Content"] as LogDisplay;
        CalculateTableWidth(ParserContent.LineParse.ElementsPattern);
    %>
    <div>
    <form method="post" action="/Home/Reload">
        <%
            if (ParserContent.LineParse.ElementsPattern.Contains("Time"))
            {
                Response.Write(
                    "<label>Start time:</label>" + 
                    "<input type=\"text\" name=\"StartTime\" />");
                
                Response.Write(
                    "<label>End time:</label>" +
                    "<input type=\"text\" name=\"EndTime\" />");
            }
        
            if (ParserContent.LineParse.ElementsPattern.Contains("ThreadId"))
            {
                Response.Write(
                    "<label>ThreadId:</label>" +
                    "<input type=\"text\" name=\"ThreadId\" />");
            }
                        
            if (ParserContent.LineParse.ElementsPattern.Contains("Level"))
            {
                Response.Write(
                    "<label>Level:</label>" +
                    "<select id=\"levelSelect\" name=\"levelSelect\"\">" +
                    "<option value=\"\"></option>" + 
                    "<option value=\"TRACE\">TRACE</option>" + 
                    "<option value=\"DEBUG\">DEBUG</option>" +
                    "<option value=\"INFO\">INFO</option>" +
                    "<option value=\"WARN\">WARN</option>" + 
                    "<option value=\"ERROR\">ERROR</option>" +
                    "<option value=\"FATAL\">FATAL</option>" +
                    "</select>");
            }
            
        %>
        <label>
        <br />
        Number of items:</label>
        <input type="text"name="numberOfItems" />&nbsp;
        <label>Log pattern:</label>
        <input type="text" name="logPattern" />&nbsp;
        <label>Separator:</label>
        <input type="text" name="separator" />&nbsp;
        <br />
        <input name="fileSelect" type="file" />
        <input type="submit" value="Submit filter" />&nbsp;
   
       <%
            
    %>
   
    </form>
    </div>
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
