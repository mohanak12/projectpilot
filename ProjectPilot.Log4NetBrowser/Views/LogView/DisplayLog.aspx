<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisplayLog.aspx.cs" Inherits="ProjectPilot.Log4NetBrowser.Views.LogView.DisplayLog" %>

<%@ Import Namespace="ProjectPilot.Log4NetBrowser.Models" %>
<%@ Import Namespace="ProjectPilot.Extras.LogParser" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Web.Mvc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<form method="post" action="/LogView/Reload">
    <head id="Head1">
        <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
        <title>Log4NetBrowser </title>
        <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />

        <script type="text/javascript" src="http://jquery.com/src/jquery.js"></script>

        <script type="text/javascript">
	        var prev = 0;
	        var id = 0;
	        $(document).ready(function() {
	            $("#bottomFixedDiv").addClass('invisible');
	            $("#filterDiv").addClass('invisible')
	            $("#advancedFilterDiv").addClass('invisible')
	            $("#filterDivSubmit").addClass('invisible')
    	    
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
    	        
	            var advancedFilterVisible = 1;
	             $("#advancedFilter").click(function() {	
                    if (advancedFilterVisible == 0)
				    {
                    $("#advancedFilterDiv").slideUp("slow");
					    advancedFilterVisible = 1;		
				    }
				    else
				    {
                    $("#advancedFilterDiv").slideDown("slow");
					    advancedFilterVisible = 0;
				    }
	            });
    	        
    	        
	            var filterVisible = 0;
	            $("#filter").click(function() {	
				    if (filterVisible == 0)
				    {
					    $("#filterDiv").slideDown("normal");
					    $("#filterDivSubmit").slideDown("slow");
					    filterVisible = 1;
    					
					    if (advancedFilterVisible == 0)
				            {
                              $("#advancedFilterDiv").slideDown("slow");		
				            }				
				    }
				    else
				    {

					    $("#filterDiv").slideUp("normal");
					    $("#filterDivSubmit").slideUp("normal");
					    $("#advancedFilterDiv").slideUp("slow");
					    filterVisible = 0;
    					
					    if (advancedFilterVisible == 0)
				            {
                              $("#advancedFilterDiv").slideUp("slow");	
				            }
				    }
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
                                <%Response.Write(Html.ActionLink("File select", "DisplayLogFiles", "LogView"));%>
                            </li>
                            <li>
                                <%Response.Write(Html.ActionLink("Refresh", "Log/" + (string)ViewData["Id"], "LogView"));%>
                            </li>
                            <li id="filter"><a href="#">Filter</a> </li>
                        </ul>
                    </div>
                    <div id="Title">
                        Log4Net-Browser</div>
                </div>
                <%
                    ParserContent = ViewData["Content"] as LogDisplay;
                    FindLevelIndex(ParserContent.LineParse.ElementsPattern);
                %>
                <div id="patternDiv">
                    <table class="tableClass" style="height: 25px;" border="0" cellpadding="0" cellspacing="0">
                        <tr valign="middle">
                            <% //Time pattern in head of table
                                foreach (string pattern in ParserContent.LineParse.ElementsPattern)
                                {
                                    Response.Write("<td align=\"center\" class=\"" + pattern.ToLower() + "\">" + pattern + "</td>");
                                }
                            %>
                        </tr>
                    </table>
                </div>
                <!-- FILTER -->
                <div id="filterDiv" style="background-color: #c6c6c6;">
                    <table class="tableClass" style="height: 35px;" border="0" cellpadding="0" cellspacing="0">
                        <br />
                        <tr valign="middle">
                            <%
                                bool search = false;

                                foreach (string pattern in ParserContent.LineParse.ElementsPattern)
                                {
                                    switch (pattern.ToLower())
                                    {
                                        case "time":
                                            {
                                                Response.Write(
                                                    "<td class='time' align='right'> Start:" +
                                                    "<input type=\"text\" name =\"startTime\" size=\"19\" />" +
                                                    "<br />" +
                                                    "End:" +
                                                    "<input type=\"text\" name =\"endTime\" size=\"19\" /></td>");
                                                break;
                                            }

                                        case "threadid":
                                            {
                                                Response.Write(
                                                   "<td class='threadid' align='center'> <input type=\"text\" name =\"threadId\" size=\"10\"/> </td>");
                                                break;
                                            }

                                        case "level":
                                            {
                                                Response.Write(
                                                    "<td class='level' align='center'>&nbsp;" +
                                                    "<select id=\"levelSelect\" name=\"level\"\">" +
                                                    "<option value=\"\"></option>" +
                                                    "<option value=\"TRACE\">TRACE</option>" +
                                                    "<option value=\"DEBUG\">DEBUG</option>" +
                                                    "<option value=\"INFO\">INFO</option>" +
                                                    "<option value=\"WARN\">WARN</option>" +
                                                    "<option value=\"ERROR\">ERROR</option>" +
                                                    "<option value=\"FATAL\">FATAL</option>" +
                                                    "</select>&nbsp;" +
                                                    "</td>");
                                                break;
                                            }
                                        case "message":
                                            {
                                                Response.Write("<td class='message' align='left'>");

                                                if (search == false)
                                                {
                                                    Response.Write(
                                                        "&nbsp;&nbsp;Search:&nbsp;" +
                                                        "<input type=\"text\" name =\"searchContent\" size=\"11\"/>" +
                                                        "<br />" +
                                                        "&nbsp;&nbsp;SearchWholeWord:" +
                                                        "<input type=\"checkbox\" name =\"matchWholeWord\"/>");

                                                    search = true;
                                                }

                                                Response.Write("</td>");

                                                break;
                                            }

                                        case "ndc":
                                            {
                                                Response.Write("<td class='ndc' align='left'>");

                                                if (search == false)
                                                {
                                                    Response.Write(
                                                        "&nbsp;&nbsp;Search:&nbsp;" +
                                                        "<input type=\"text\" name =\"searchContent\" size=\"11\"/>" +
                                                        "<br />" +
                                                        "&nbsp;&nbsp;SearchWholeWord:" +
                                                        "<input type=\"checkbox\" name =\"matchWholeWord\"/>");

                                                    search = true;
                                                }

                                                Response.Write("</td>");

                                                break;
                                            }
                                        case "namespace":
                                            {
                                                Response.Write("<td class='namespace' align='left'>");

                                                if (search == false)
                                                {
                                                    Response.Write(
                                                        "&nbsp;&nbsp;Search:&nbsp;" +
                                                        "<input type=\"text\" name =\"searchContent\" size=\"11\"/>" +
                                                        "<br />" +
                                                        "&nbsp;&nbsp;SearchWholeWord:" +
                                                        "<input type=\"checkbox\" name =\"matchWholeWord\"/>");

                                                    search = true;
                                                }

                                                Response.Write("</td>");

                                                break;
                                            }

                                    }
                                } 
                            %>
                        </tr>
                    </table>
                </div>
                
                 <!-- ADVANCED FILTER -->
                <div id="advancedFilterDiv" style="background-color: #cFcFcF;">
                    <table class="tableClass" style="height: 35px;" border="0" cellpadding="0" cellspacing="0">
                       <br />
                       <tr valign = "middle">
                            <td align="right">Display items per page: <input id="NumOfItemsPerPage" name="numberOfItemsPerPage" type="text" size="3" /> </td> 
                            <td align="right">Search items: <input id="Text1" name="searchNumberOfItems" type="text" size="3" /></td>
                            <td align="right">Start search index: <input id="Text2" name="startSearchIndex" type="text" size="4" /> <br />
                            End search index: <input id="Text3" type="text" name="endSearchIndex" size="4" /> </td>
                            <td align="right">Start search:<input id="Text4" name="startSearchByte" type="text" size="6" />&nbsp;[Byte]<br />
                            End search:<input id="Text5" type="text" name="endSearchByte" size="6" />&nbsp;[Byte]</td>
                            <td style="size: auto"></td>
                       </tr>
                    </table>
                </div>
                
                <!-- SUBMIT - FILTER -->
                <div id="filterDivSubmit" style="background-color: #2D659E;">
                    <table  class="tableClass" border="0" cellpadding="0" cellspacing="0">
                        <tr valign="middle">
                            <td align="left"><input type="submit" name="Submit" value="Submit filter"/></td>
                            <td align="right"><a id="advancedFilter">Advanced</a></td>
                        </tr>
                    </table>
                </div>    
                        
            </div>
            <div id="contentDiv">
                <table class="tableClass" border="0" cellpadding="0" cellspacing="0">
                    <%
                        int idx = 0;
                        foreach (LogEntry logEntry in ParserContent.LineParse.ElementsLog)
                        {
                            Response.Write(LogEntryToString(logEntry, idx));
                            idx++;
                        }
                    %>
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
        <p>&nbsp;</p>
    </body>
 </form>
</hmtl>