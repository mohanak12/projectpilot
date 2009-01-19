<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="FileSelect.aspx.cs" Inherits="ProjectPilot.Log4NetBrowser.Views.Home.FileSelect" %>

<asp:Content ID="FileSelectContent" ContentPlaceHolderID="MainContent" runat="server">
    <form method="post" action="/Home/Load">
    
        <label>File selected:</label>
        <input name="fileSelect" type="file" />
        <br />
        <br />
        <label>Log pattern:</label>
        <input type="text" name="logPattern" />&nbsp;
        <label>Separator:</label>
        <input type="text" name="separator" />&nbsp;
        <label>Number of items:</label>
        <input type="text"name="numberOfItems" />&nbsp;
        <label>(Default = 255)</label>
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
        <input type="submit" value="OK" />&nbsp;
    </form>
</asp:Content>