<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisplayLog.aspx.cs" Inherits="ProjectPilot.Log4NetBrowser.Views.LogView.DisplayLog" %>
<%@ Import Namespace="ProjectPilot.Log4NetBrowser.Models"%>
<%@ Import Namespace="ProjectPilot.Extras.LogParser"%>
<%@ Import Namespace="System"%>
<%@ Import Namespace="System.Web"%>
<%@ Import Namespace="System.Web.Mvc"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">



<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1"><meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" /><title>
	Log4NetBrowser
</title><link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://jquery.com/src/jquery.js"></script>
	<script type="text/javascript">
	    var prev = 0;
	    var id = 0;
	    $(document).ready(function() {
	        $("#bottomFixedDiv").click(function() {
	            $(this).slideUp("slow");
	        });

	        $("tr").click(function() {

	            id = $(this).attr("idShow");
	            id = "#" + id;
	            if (id != prev) {
	                $(id).parent().addClass('selectedTD');
	                $(prev).parent().removeClass('selectedTD');
	            }
	            prev = id;

	            var html = $(id).html();
	            $("#bottomFixedContent").html(html);
	            $("#bottomFixedDiv").slideDown("slow");   //slideUp = hidden
	        });
	    });
    </script>
</head>
<body>





<div id="containerDiv">
    <div id="headerDiv">
        <div id="menuDiv">
           <div id="menuList">
            <ul id="menu">
                <li>
                <a href="#"><%Response.Write(Html.ActionLink("File select", "DisplayLogFiles", "LogView"));%></a>
                </li>
                <li>
                <a href="#">Refresh</a>
                </li>
            </ul>
            </div>
            <div id="Title">Log4Net-Browser</div>
        </div>
        <div id="patternDiv">
        <table class="tableClass" style="height:25px;" border="0" cellpadding="0" cellspacing="0">
        <tr valign="middle">
            
                <td width="200px" align="center">Time</td>
            
                <td width="75px">ThreadID</td>
            
                <td width="780px" align="center">Message</td>
            
        </tr>
        </table>
        </div>
    </div>

    <div id="contentDiv">
        <%
        ParserContent = ViewData["Content"] as LogDisplay;
        FindLevelIndex(ParserContent.LineParse.ElementsPattern);
        %>  
        
        <table class="tableClass" border="0" cellpadding="0" cellspacing="0">
            
                   <%
          int idx = 0;
           foreach (LogEntry logEntry in ParserContent.LineParse.ElementsLog) {
               Response.Write(LogEntryToString(logEntry, idx));
               idx++;
        }%>
        </table>
        
        
    </div>
</div>

<div id="bottomFixedDiv">
    <div id="bottomFixedHeader">
    Message content:
    </div>
    <div id="bottomFixedContent">

    </div>
</div>

</body>
</hmtl>