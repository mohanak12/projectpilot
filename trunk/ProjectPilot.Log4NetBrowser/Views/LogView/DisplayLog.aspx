<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisplayLog.aspx.cs" Inherits="ProjectPilot.Log4NetBrowser.Views.LogView.DisplayLog" %>
<%@ Import Namespace="ProjectPilot.Log4NetBrowser.Models"%>
<%@ Import Namespace="ProjectPilot.Extras.LogParser"%>
<%@ Import Namespace="System"%>
<%@ Import Namespace="System.Web"%>
<%@ Import Namespace="System.Web.Mvc"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">



<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <title><%= Html.Encode(ViewData["Title"]) %></title>
    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://jquery.com/src/jquery.js"></script>
	<script type="text/javascript">
	$(document).ready(function()
	{
			$("#menuDiv").click(function()
			{
			    $("#bottomFixedDiv").toggle("slow");
		    });
			
			$("#patternDiv").click(function()
			{
			    $("#bottomFixedDiv").toggle("slow");
		    });

		    $("#bottomFixedDiv").click(function()
		    {
			    $(this).toggle("slow");
			});

			/*$("tr").click(function() {
			    $("#bottomFixedDiv").toggle("slow");
			});*/
			$("tr").click(function() {

			    var id = $(this).attr("idShow");
			    id ="#" +id;
			    var html = $(id).html();
			    $("#bottomFixedDiv").html(html);

			    $("#bottomFixedDiv:reallyvisible").slideDown("slow");   //slideUp = hidden
			    if ($("#bottomFixedDiv").is(":hidden").bool) {
			        alert("Test!");
			        $("#bottomFixedDiv").is(":hidden").slideDown("slow");
			    }

			});
		  
	});
    </script>
</head>
<body>


    
    
<div id="containerDiv">
    <div id="headerDiv">
        <div id="menuDiv">Tole je meni. File select .... refresh...
            <%= Html.ActionLink("Select File","FileSelect","Home")%>
        </div>
        <div id="patternDiv">Time | ThreadID | Message</div>
    </div>

    <div id="contentDiv">
        <%
        ParserContent = ViewData["Content"] as LogDisplay;
        CalculateTableWidth(ParserContent.LineParse.ElementsPattern);
        %>   
        
        <table border="0" cellpadding="0" cellspacing="0">
        <tr valign="top">
            <%for (int i = 0; i < TableWidths.Count(); i++)
              {%>
                <td width="<%Response.Write(TableWidths[i].ToString());%>px">&nbsp</td>
            <%}%>
        </tr>
           <%
              int idx = 0;
               foreach (LogEntry logEntry in ParserContent.LineParse.ElementsLog) {
                   Response.Write(LogEntryToString(logEntry, idx));
                   idx++;
            }%>
        </table>
        
        
    </div>
    to je container
</div>


<div id="bottomFixedDiv">Pirakazi oz. skrije me!
	    <BR /><BR /><BR /><BR /> 
    
	    Test!!</div>


<%
    idx = 0;
    foreach (LogEntry logEntry in ParserContent.LineParse.ElementsLog) {%>
       <div class="invisible" id="<%Response.Write(idx);%>">
       <%Response.Write(logEntry.Elements[2].ToString());
       idx++;
        %>
       </div>
<%}%>

</body>
</hmtl>