<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisplayLogFiles.aspx.cs" Inherits="ProjectPilot.Log4NetBrowser.Views.LogView.DisplayLogFiles" %>
<%@ Import Namespace="System"%>
<%@ Import Namespace="System.Web"%>
<%@ Import Namespace="System.Web.Mvc"%>
<%@ Import Namespace="ProjectPilot.Extras.LogParser" %>
 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

 
 
<html xmlns="http://www.w3.org/1999/xhtml" />

<html xmlns="http://www.w3.org/1999/xhtml" />


<form method="post" action="/LogView/LoadFile">

    <head id="Head1">
        <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    </head>
        <title>	Log4NetBrowser - File select  </title> 
        <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
     
    <body>
	    <div id="Title"> Log4Net-Browser </div><br /><br />
	    <br /><br />
    	
	                <div id="filterDivHead" style="background-color: #2D659E;">
                            Filter:
                    </div>  
    	
	                <!-- FILTER -->
                    <div id="filterDiv" style="background-color: #c6c6c6;">
                        <table class="tableClass" style="height: 35px;" border="0" cellpadding="0" cellspacing="0">
                            <br />
                            <tr valign="middle">
                            <td width='225px' align='right'> 
                                    Start time:<input type="text" name ="startTime" size="19" />
                                    <br />
                                    End time:<input type="text" name ="endTime" size="19" />
                                </td>
                                <td width='150' align='center'>
                                    &nbsp;Level:
                                    <select id="levelSelect" name="level">
                                        <option value=""></option>
                                        <option value="TRACE">TRACE</option>
                                        <option value="DEBUG">DEBUG</option>
                                        <option value="INFO">INFO</option>
                                        <option value="WARN">WARN</option>
                                        <option value="ERROR">ERROR</option>
                                        <option value="FATAL">FATAL</option>
                                    </select>&nbsp;</td>
                                <td width='165' align='center'>
                                     ThreadId:<input type="text" name ="threadId" size="10"/> 
                                </td>
                                <td width='170' align='left'>
                                    &nbsp;&nbsp;Search:&nbsp;
                                    <input type="text" name ="searchContent" size="11"/>
                                    <br />
                                    &nbsp;&nbsp;SearchWholeWord:<input type="checkbox" name ="matchWholeWord" />
                                </td>
                                <td style="size: auto"></td>
                            </tr>
                        </table>
                    </div>
    	
                    <div id="advancedFilterDiv" style="background-color: #cFcFcF;">
                        <table class="tableClass" style="height: 35px;" border="0" cellpadding="0" cellspacing="0">
                           <br />
                           <tr valign = "middle">
                            <%
                               Response.Write("<td align=\"right\">Display items per page: <input id=\"NumOfItemsPerPage\" name=\"numberOfItemsPerPage\" type=\"text\" size=\"3\" value=\"" + (int)ViewData["numberOfItemsPerPage"] + "\" /> </td> ");
                               
                               LogParserFilter filter = (LogParserFilter) ViewData["filter"];
                               Response.Write("<td align=\"right\">Search items: <input id=\"Text1\" name=\"searchNumberOfItems\" type=\"text\" size=\"3\" value=\"" + filter.FilterNumberOfLogItems + "\" /> </td> ");
                            %>
                                <td align="right" width='220'>Start search index: <input id="Text2" name="startSearchIndex" type="text" size="4" /> <br />
                                End search index: <input id="Text3" type="text" name="endSearchIndex" size="4" /> </td>
                                <td align="right" width='220'>Start search:<input id="Text4" name="startSearchByte" type="text" size="6" />&nbsp;[Byte]<br />
                                End search:<input id="Text5" type="text" name="endSearchByte" size="6" />&nbsp;[Byte]</td>
                                <td style="size: auto"></td>
                           </tr>
                        </table>
                    </div>

                    <div id="filterDivSubmit" style="background-color: #2D659E;">
                        <input type="submit" name="submit" value="Display log file"/>
                    </div>  

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

                Response.Write("<input type=\"radio\" name=\"selectedFile\" value=\"" + logFiles.ElementAt(n).Key + "\"\">&nbsp;");
                Response.Write(Html.ActionLink(logFiles.ElementAt(n).Value, "Log/" + logFiles.ElementAt(n).Key, "LogView"));
                Response.Write("<BR />");
                Response.Write(@"</div>");
            }        
            %>
    	    
            </table>
	     </div>
	     <br /> <br />
	     &nbsp;<%Response.Write(Html.ActionLink("LoC Metrics", "SelectSolutionFile", "XML"));%>
	    </div>
    </body>
    </html>
</form>

