<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="listararquivos.aspx.cs" Inherits="WebApplication1.listararquivos" %>

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
						<p>DESENVOLVIMENTO SSE - LISTAR PRINICIPAIS ARQUIVOS DOS MÓDULOS (SERVIDOR DE DESENVOLVIMENTO)</p>
						</td>
						<td align="right" valign="bottom">
							<img src="img/login_logo.gif" width="136" height="31" alt="BMC logo" border="0"> 
						</td>
					</tr>
					</tbody></table>
				</td>
			</tr>
		</tbody></table>
        <p><a href="default.aspx">Home</a></p>
        <hr color="black" />
        <br />
            <br />
      <form id="form1" runat="server">
    <div>
         <table align="center" width="80%" >
             <tr><td>Ambiente:<asp:DropDownList ID="ddlambiente" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlambiente_SelectedIndexChanged"></asp:DropDownList></td>
                 </tr>
             <tr><td>Sistema:&nbsp;&nbsp; <asp:DropDownList ID="ddlsistema" runat="server"  ></asp:DropDownList></td>
                 </tr>
             <tr><td align="center">
                     <asp:Button ID="btndeploy" runat="server" Text="Listar Arquivos" OnClick="btndeploy_Click" /> </td>
                 </tr>
             <tr><td align="center">
                 <asp:Label id="lblNoRows" runat="server" visible="false" ForeColor="#CC0000" Font-Bold="True">Não há parametrizacao para esse sistema</asp:Label>
                <br />
        <asp:DataGrid runat="server" id="articleList" Font-Name="Verdana"
    AutoGenerateColumns="False" AlternatingItemStyle-BackColor="#eeeeee"
    HeaderStyle-BackColor="Navy" HeaderStyle-ForeColor="White"
    HeaderStyle-Font-Size="10pt" HeaderStyle-Font-Bold="True"
              OnSortCommand="articleList_SortCommand"
           AllowSorting="true"
            >
  <Columns>
    <asp:BoundColumn DataField="Name"  
           HeaderText="Nome" SortExpression="Name"  />
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
    </form>
</body>
</html>
