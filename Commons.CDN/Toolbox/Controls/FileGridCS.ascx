<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileGridCS.ascx.cs" Inherits="bOS.Commons.UI.Controls.FileGridCS" %>

<div id="page-wrapper">
            
    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">Files Explorer</h1>
        </div>
        <!-- /.col-lg-12 -->
    </div>
    <!-- /.row -->
    <div class="row">

        <div class="col-lg-12">
            <div class="panel panel-default">
                
                <div class="panel-heading">
                    <i class="fa fa-files-o fa-fw"></i> <asp:Label ID="lblCurrentPath" runat="server"></asp:Label>
                    <div class="pull-right">
                        <div class="btn-group">
                            <button type="button" class="btn btn-default btn-xs dropdown-toggle" data-toggle="dropdown">
                                Actions
                                <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu pull-right" role="menu">
                                <li><a href="#">Action</a>
                                </li>
                                <li><a href="#">Another action</a>
                                </li>
                                <li><a href="#">Something else here</a>
                                </li>
                                <li class="divider"></li>
                                <li><a href="#">Separated link</a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>

                <div class="panel-body">
                    <div class="table-responsive">
                        <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-striped"
                            OnPageIndexChanging="gvFiles_PageIndexChanging"
                            OnRowCommand="gvFiles_RowCommand" OnRowDataBound="gvFiles_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lbFolderItem" CommandName="OpenFolder" CommandArgument='<%# Eval("Name") %>'></asp:LinkButton>
                                        <asp:Literal runat="server" ID="ltlFileItem"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FileSystemType" HeaderText="Type"
                                    SortExpression="FileSystemType" />
                                <asp:BoundField DataField="LastWriteTime" HeaderText="Date Modified"
                                    SortExpression="LastWriteTime" />
                                <asp:TemplateField HeaderText="Size" SortExpression="Size" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%# DisplaySize((long?) Eval("Size")) %>
                                    </ItemTemplate>

                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle CssClass="cursor-pointer" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>