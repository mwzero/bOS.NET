<%@ Page Title="" Language="C#" MasterPageFile="~/Design/Admin.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="bOS.Services.CDN.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CssContentPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SidebarContentPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="JsContentPlaceHolder" runat="server">
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="SheetContentPlaceHolder" runat="server">

<div id="page-wrapper">
            
    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">Dashboard</h1>
        </div>
        <!-- /.col-lg-12 -->
    </div>
    
    <div class="row">
        <div class="col-lg-12">
            <div class="table-responsive">
                <asp:GridView ID="gridAuditLogs" runat="server" CssClass="table table-bordered table-hover table-striped" GridLines="None" AutoGenerateColumns="False"
                    ShowHeaderWhenEmpty="True" ShowHeader="true" >
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <label>Data</label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "DateTimeAudit", "{0:dd/MM/yyyy HH:mm:ss}" )%> 
                            </ItemTemplate>
                            <HeaderStyle Wrap="False" VerticalAlign="Top" />
                        </asp:TemplateField>

                        <asp:BoundField DataField="Description" HeaderText="Description" />
                        
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <label>Link</label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "Link", "{0}" )%>
                            </ItemTemplate>
                            <HeaderStyle Wrap="False" VerticalAlign="Top" />
                        </asp:TemplateField>

                    </Columns>
                    <RowStyle CssClass="cursor-pointer" />
                </asp:GridView>
                    
            </div>
                           
        </div>
                        
    </div>

</div>

</asp:Content>
