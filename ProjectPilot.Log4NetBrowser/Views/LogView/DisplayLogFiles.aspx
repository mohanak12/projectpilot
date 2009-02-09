<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisplayLogFiles.aspx.cs" Inherits="ProjectPilot.Log4NetBrowser.Views.LogView.DisplayLogFiles" %>
<%@ Import Namespace="System"%>
<%@ Import Namespace="System.Web"%>
<%@ Import Namespace="System.Web.Mvc"%>

 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

 
 
<html xmlns="http://www.w3.org/1999/xhtml" >

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1">
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <title>	Log4NetBrowser - File select  </title> 
    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
 
<body>
	<div id="Title"> Log4Net-Browser </div><br /><br />
	<br />
	<div id="headerDiv" ><font color="white"> &nbsp Select log file: </font></div>
	
    <div>
	<br />
	
	<br />
	  <div id="fileListDiv">		 
	    <table class="tableClass" border="0" cellpadding="0" cellspacing="0">
	    
	    <%
        GetLogFiles();        
    
        Dictionary<string, string> logFiles = (Dictionary<string, string>)ViewData["LogFiles"];

        bool change = true;   
	        
        for (int n = 0; n < logFiles.Count; n++)
        {
            if (change == true)
            {
                Response.Write("<div class=\"even\">");
                change = false;
            }
            else
            {
                Response.Write("<div class=\"odd\">");
                change = true;  
            }
            
            Response.Write(Html.ActionLink(logFiles.ElementAt(n).Value, "Log/" + logFiles.ElementAt(n).Key, "LogView"));
            Response.Write("<BR />");
            Response.Write(@"</div>");
        }        
        %>
	    
        </table>
	 </div>
	</div>
</body>
</html>

