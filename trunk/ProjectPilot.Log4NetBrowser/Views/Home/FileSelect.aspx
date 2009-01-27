<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="FileSelect.aspx.cs" Inherits="ProjectPilot.Log4NetBrowser.Views.Home.FileSelect" %>

<asp:Content ID="FileSelectContent" ContentPlaceHolderID="MainContent" runat="server">
    <form method="post" action="/Home/Load"> 
        <br />
        <label>Number of items:</label>
        <input type="text"name="numberOfItems" />&nbsp;
        <label>(Default = 255)</label>&nbsp;&nbsp;&nbsp;
         <label>Number of items per page:</label>
        <input type="text"name="numberOfItemsPerPage" />&nbsp;
         <label>(Default = 50)</label>
        <br />
        <br />
        <label>Start time:</label>
        <input type="text" name="StartTime" />
                

        <label>End time:</label>
        <input type="text" name="EndTime" />

        <label>ThreadId:</label>
        <input type="text" name="ThreadId" />


        <label>Level:</label>
        <select id="levelSelect" name="levelSelect"">
            <option value=""></option>
            <option value="TRACE">TRACE</option>
            <option value="DEBUG">DEBUG</option>
            <option value="INFO">INFO</option>
            <option value="WARN">WARN</option>
            <option value="ERROR">ERROR</option>
            <option value="FATAL">FATAL</option>
        </select>
        <br />
        <br />
            <label>Search:</label>
            <input type="text" name="Search" />
            <input id="Checkbox1" type="checkbox" name="searchType" value="MatchWholeWord"/>
            <label>Match Whole Word</label>      
        <br />
        <br />
        <input type="submit" value="OK" />&nbsp;
        <br />
        <br />
        
            
    <%
        Dictionary<string, string> logFilesList = ViewData["logFilesList"] as Dictionary<string, string>;

        foreach (string logFile in logFilesList.Keys)
        {
            Response.Write(Html.ActionLink(logFile, "Ndc/" + logFile, "Home"));
            Response.Write("<BR />");
        }

    %>
    </form>
</asp:Content>