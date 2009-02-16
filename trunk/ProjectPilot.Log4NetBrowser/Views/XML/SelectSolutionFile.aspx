<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectSolutionFile.aspx.cs" Inherits="ProjectPilot.Log4NetBrowser.Views.XML.SelectSolutionFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<form method="post" action="/XML/GenerateXML">
    <html xmlns="http://www.w3.org/1999/xhtml" >
        <head id="Head1">
            <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
            <title>	LoC Metrics </title> 
            <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
        </head>
    
    
    <body>
        <div id="Title"> LoC Metrics </div>
        <div id="headerDiv"><br /> <br /> <br />
        <font color="white">&nbsp;  Solution file path: </font> &nbsp; <input id="input" type="text" name="solutionFilePath" size="30" /> &nbsp; <input type="submit" name="Submit" value="Generate XML"/> 
        
        <br /> <br />
        &nbsp;<%Response.Write(Html.ActionLink("Log4NetBrowser", "DisplayLogFiles", "LogView"));%>
        </div>
    </body>
    </html>
</form>
