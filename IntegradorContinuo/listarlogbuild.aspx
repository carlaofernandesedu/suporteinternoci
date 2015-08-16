<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="listarlogbuild.aspx.cs" Inherits="WebApplication1.listarlogbuild" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <table border="0" cellpadding="0" cellspacing="0" height="100%" width="100%">
	<!-- main table (wraps around sub-tables) -->
	<tbody><tr>
		<td valign="top">
		<table border="0" cellpadding="20" cellspacing="0" width="100%">
			<!-- header table -->
			<tbody><tr>
				<td bgcolor="#ffffff" width="100%">
					<table border="0" cellpadding="20" cellspacing="0" width="100%">
					<tbody><tr>
						<td valign="bottom">
						<p>DESENVOLVIMENTO SSE - LISTAR LOG DE ARQUIVOS ALTERADOS (SERVIDOR DE DESENVOLVIMENTO) BUILD PORTALNET</p>
						</td>
						<td align="right" valign="bottom">
							<img src="img/login_logo.gif" width="136" height="31" alt="BMC logo" border="0"> 
						</td>
					</tr>
					</tbody></table>
				</td>
			</tr>
		</tbody></table>
        <p><a href="default.aspx">Home</a>&nbsp;&nbsp;<a href="listarlogbuild.aspx">Voltar</a></p>
        <hr color="black" />
        <br />
        
            <br />
      <form id="form1" runat="server">
    <div id="dvresult1" runat="server">
         <table align="center" width="80%" >
             <tr><td align="center">
        <asp:DataGrid runat="server" id="articleList" Font-Name="Verdana"
    AutoGenerateColumns="False" AlternatingItemStyle-BackColor="#eeeeee"
    HeaderStyle-BackColor="Navy" HeaderStyle-ForeColor="White"
    HeaderStyle-Font-Size="10pt" HeaderStyle-Font-Bold="True"
              OnSortCommand="articleList_SortCommand"
           AllowSorting="true"
            >
  <Columns>
     <asp:TemplateColumn HeaderText="Nome" SortExpression="Name">
          <ItemTemplate>
             <asp:HyperLink runat="server" ID="lnknome" NavigateUrl='<%# String.Concat("listarlogbuild.aspx?p_arquivo=",Eval("Name")) %>'  ><%# DataBinder.Eval(Container, "DataItem.Name")%></asp:HyperLink>
          </ItemTemplate>
    </asp:TemplateColumn>
    <asp:BoundColumn DataField="LastWriteTime" HeaderText="Data Ultima Modificação"
        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:g}" SortExpression="LastWriteTime" />
    <asp:BoundColumn DataField="Length" HeaderText="Tamanho"
		ItemStyle-HorizontalAlign="Right" 
		DataFormatString="{0:#,### bytes}" />
  </Columns>
</asp:DataGrid>
                     </td>
             </tr>
             </table>  
    </div>
    <div id="dvresult2" runat="server">
             <table align="center" width="80%" >
             <tr>
                 <td align="center">
                 <asp:DataGrid runat="server" id="dgarquivo" Font-Name="Verdana"
                                AutoGenerateColumns="False" AlternatingItemStyle-BackColor="#eeeeee"
                                HeaderStyle-BackColor="Navy" HeaderStyle-ForeColor="White"
                                HeaderStyle-Font-Size="10pt" HeaderStyle-Font-Bold="True"
                                          OnSortCommand="dgarquivo_SortCommand"
                                       AllowSorting="true"
                                        >
                              <Columns>
                                <asp:BoundColumn DataField="Name" HeaderText="Name" SortExpression="Name"
		                            ItemStyle-HorizontalAlign="Left" 
		                             />
                                   <asp:BoundColumn DataField="LastWriteTime" HeaderText="Data Ultima Modificação" SortExpression="LastWriteTime"
                                    ItemStyle-HorizontalAlign="Center" DataFormatString="{0:g}"  />
                                <asp:BoundColumn DataField="Length" HeaderText="Tamanho"
		                            ItemStyle-HorizontalAlign="Right" 
		                            DataFormatString="{0:#,### bytes}" />
                              </Columns>
                            </asp:DataGrid>
                     </td>
             </tr>
             </table>  
    </div>
    <input type="hidden" id="hid_arquivo" runat="server" />
    </form>
</body>
</html>
