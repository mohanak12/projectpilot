<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true"
    CodeBehind="Home.aspx.cs" Inherits="ProjectPilot.Portal.Views.Home.Home" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <center>
            <br />
            <img alt="ProjectPilot" src="../../Content/Images/ProjectPilotLogo.png" />
            <br />
            <input type="text" tabindex="10" size="80" value="" class="input" id="SearchQuery"
                name="SearchQuery" />
            <br />
            <input type="submit" value="Search" name="Search" />
            <br />
            <div style="margin: 2em;">
                <table align="center">
                    <tbody>
                        <tr>
                            <td>
                                <div>
                                    Projects:</div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <div id="projectlist">
                                    <table>
                                        <tbody>
                                            <asp:ListView ID="ProjectList" class="ProjectList" runat="server">
                                                <LayoutTemplate>
                                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <%# 
                        Html.ActionLink((string)Eval("ProjectName"), String.Format("Overview/{0}/", Eval("ProjectId")), "ProjectView")%>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    No projects are currently configured.
                                                </EditItemTemplate>
                                            </asp:ListView>
                                        </tbody>
                </table>
            </div>
        </td> </tr> </tbody> </table>
    </div>
    </center> </div>
</asp:Content>
