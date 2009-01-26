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
                    "<label>Start time:</label>&nbsp;" +
                    "<input type=\"text\" name=\"StartTime\" />&nbsp;");
                
                Response.Write(
                    "<label>End time:</label>&nbsp;" +
                    "<input type=\"text\" name=\"EndTime\" />&nbsp;");
            }
        
            if (ParserContent.LineParse.ElementsPattern.Contains("ThreadId"))
            {
                Response.Write(
                    "<label>ThreadId:</label>&nbsp;" +
                    "<input type=\"text\" name=\"ThreadId\" />&nbsp;");
            }
                        
            if (ParserContent.LineParse.ElementsPattern.Contains("Level"))
            {
                Response.Write(
                    "<label>Level:</label>&nbsp;" +
                    "<select id=\"levelSelect\" name=\"levelSelect\"\">" +
                    "<option value=\"\"></option>" + 
                    "<option value=\"TRACE\">TRACE</option>" + 
                    "<option value=\"DEBUG\">DEBUG</option>" +
                    "<option value=\"INFO\">INFO</option>" +
                    "<option value=\"WARN\">WARN</option>" + 
                    "<option value=\"ERROR\">ERROR</option>" +
                    "<option value=\"FATAL\">FATAL</option>" +
                    "</select>&nbsp;");
            }
            
        %>
  
        <label>Number of items:</label>&nbsp;
        <%
            if((int)ViewData["numberOfItemsPerPage"] != null)
                Response.Write("<input type=\"text\" name=\"numberOfItems\" size=\"3\" value=\"" + (int)ViewData["numberOfItemsPerPage"] + "\" />");
            else
                Response.Write("<input type=\"text\" name=\"numberOfItems\" size=\"3\" />");
        %> 
        <br />
        <br />
        
        <label>Search:</label>
        <input type="text" name="Search" />
        <input id="Checkbox1" type="checkbox" name="searchType" value="MatchWholeWord"/>
        <label>Match Whole Word</label>
        
    <br />
        <br />
        <input type="submit" value="Submit filter" />&nbsp;
        
        <%= Html.ActionLink("Select File","FileSelect","Home")%> 

        <br />
        <br /> 
        
        <%if ((bool)ViewData["showPreviousControl"])
            Response.Write(Html.ActionLink(" <- show previous", "ShowPreviousLogEntries", "Home"));%>

        <%Response.Write("  " + (int)ViewData["StartIndexOfLogItemsShow"] + " - " + (int)ViewData["EndIndexOfLogItemsShow"] + "  ");%>
        
        <%if ((bool)ViewData["showNextControl"])
              Response.Write(Html.ActionLink("show next -> ", "ShowNextLogEntries", "Home"));%>
        

           
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
                <br />
                <br /> 
                <% Response.Write("Number of log items in file: " + (int)ViewData["numberOfLogItemsInLogFile"]); %> 
    </div>
</asp:Content>
