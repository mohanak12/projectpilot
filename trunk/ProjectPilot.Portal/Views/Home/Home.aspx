<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="ProjectPilot.Portal.Views.Home.Home" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ProjectPilot Home</title>
</head>
<body>
    <div>
        <center>
            <br />
            <img alt="ProjectPilot" src="/ProjectPilot/Content/Images/ProjectPilotLogo.png" />
            <br /> 
            <input type="text" tabindex="10" size="80" value="" class="input" id="SearchQuery" name="SearchQuery"/> 
            <br />
            <input type="submit" value="Search" name="Search"/>
            <br />
            
            <asp:ListView ID="ProjectList" runat="server">
                <LayoutTemplate>
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                </LayoutTemplate>
                <ItemTemplate>
                    <br />
                    <%# 
                        Html.ActionLink((string)Eval("Name"), String.Format("Details/{0}", Eval("Name")), "Project")%>
                </ItemTemplate>
                <EditItemTemplate>
                    No projects are currently configured.
                </EditItemTemplate>
            </asp:ListView>
            
        </center>
    </div>
</body>
</html>
