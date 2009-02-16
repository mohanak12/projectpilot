<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="XML.aspx.cs" Inherits="ProjectPilot.Log4NetBrowser.Views.XML.XMLView" %>
<%@ Import Namespace="System.Xml"%>
<%@ Import Namespace="System"%>
<%@ Import Namespace="System.Web"%>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.IO"%>

     <%
         using (StreamReader reader = new StreamReader(@"XML_report.xml"))
         {
             Response.ContentType = "text/xml";
           
             while (!reader.EndOfStream)
             {
                Response.Write(reader.ReadLine());
             }
         }
     %>



