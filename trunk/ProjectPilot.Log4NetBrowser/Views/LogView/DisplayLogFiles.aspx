<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisplayLogFiles.aspx.cs" Inherits="ProjectPilot.Log4NetBrowser.Views.LogView.DisplayLogFiles" %>
<%@ Import Namespace="System"%>
<%@ Import Namespace="System.Web"%>
<%@ Import Namespace="System.Web.Mvc"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <div>
    <br />
    <%
        GetLogFiles();        
    
        Dictionary<string, string> logFiles = (Dictionary<string, string>)ViewData["LogFiles"];
    
        for (int n = 0; n < logFiles.Count; n++)
        {
            Response.Write(Html.ActionLink(logFiles.ElementAt(n).Value, "Log/" + logFiles.ElementAt(n).Key, "LogView"));
            
            Response.Write("<BR />");
        }        
    %>
    </div>
</body>
</html>
