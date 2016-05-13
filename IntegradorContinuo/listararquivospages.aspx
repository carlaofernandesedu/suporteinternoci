<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="listararquivospages.aspx.cs" Inherits="WebApplication1.listararquivospages" %>

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
						<p>DESENVOLVIMENTO SSE - LISTAR ARQUIVOS DOS MÓDULOS PARA O PORTALNET A PARTIR DA SUBPASTA PAGINAS (SERVIDOR DE DESENVOLVIMENTO)</p>
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
             <tr><td>Informe o nome do Modulo:&nbsp;&nbsp; <asp:Textbox ID="txtcaminhosubpasta" runat="server" Width="701px"  >.Pages</asp:Textbox></td>
                 </tr>
             <tr><td align="center">
                     <asp:Button ID="btndeploy" runat="server" Text="Listar Arquivos" OnClick="btndeploy_Click" /> </td>
                 </tr>
             <tr><td align="center">
                 <asp:Label id="lblNoRows" runat="server" visible="false" ForeColor="#CC0000" Font-Bold="True">Nao foi possivel encontrar o caminho solicitado</asp:Label>
                <br />
                 <div id="dvtree" runat="server">
                 <asp:TreeView ID="TreeView1" runat="server" ImageSet="XPFileExplorer" NodeIndent="15">
    <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
    <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" HorizontalPadding="2px"
        NodeSpacing="0px" VerticalPadding="2px"></NodeStyle>
    <ParentNodeStyle Font-Bold="False" />
    <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" HorizontalPadding="0px"
        VerticalPadding="0px" />
</asp:TreeView>
                     </div>
                 </td>
             </tr>
             </table>  
    </div>
    </form>
</body>
</html>
