<%@ Page Language="C#" MasterPageFile="~/Design/Admin.master" AutoEventWireup="true" Inherits="Commons.CDN.Toolbox.InternalStoreFolderList" %>

<%@ Register TagName="FileGridCS" TagPrefix="uc1" Src="~/Toolbox/Controls/FileGridCS.ascx"  %>
 


<asp:Content ID="Content2" ContentPlaceHolderID="SheetContentPlaceHolder" runat="server">
 
        <uc1:FileGridCS ID="FileGridCS1" HomeFolder="~/UploadFilesFolder" runat="server" PageSize="10" />

</asp:Content>
