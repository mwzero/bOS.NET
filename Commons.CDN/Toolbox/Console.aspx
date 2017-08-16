
<%@ Page Title="" Language="C#" MasterPageFile="~/Design/Admin.master" AutoEventWireup="true" Inherits="Commons.CDN.Toolbox.Console" %>

<asp:Content ID="Content2" ContentPlaceHolderID="SheetContentPlaceHolder" runat="server">

<div id="page-wrapper">
            
    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">Console</h1>
        </div>
        <!-- /.col-lg-12 -->
    </div>
    <!-- /.row -->
    <div class="row">
        <div class="col-lg-12">
            <div class="table-responsive">
                <table class="table table-hover table-striped">
                    <tr>
                        <td>Schedulatore</td>
                        <td>
                            <asp:Button ID="btnScheduler" runat="server" Text="" CssClass="btn btn-primary" 
                                onclick="btnScheduler_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>Svuota Cache</td>
                        <td>
                            <asp:Button ID="btnClearCache" runat="server" Text="Svuota" CssClass="btn btn-primary" 
                                onclick="btnClearCache_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</div>

</asp:Content>

