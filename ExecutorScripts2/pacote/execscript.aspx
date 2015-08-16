<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="execscript.aspx.cs" Inherits="ExecutorScript.execscript" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
						<p>DESENVOLVIMENTO SSE - EXECUÇÃO DE SCRIPTS  - AMBIENTE DESENVOLVIMENTO</p>
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
            <!--
            <tr><td>Base Execução:</td><td><asp:TextBox ID="txtServidor" runat="server"></asp:TextBox></td>
                     </tr>
            -->
             <tr><td align="right">Owner de Execução:</td><td><asp:TextBox ID="txtUsuario" runat="server" MaxLength="15"></asp:TextBox></td>
                     </tr>
             <tr><td align="right">Senha do Owner:</td><td><asp:TextBox ID="txtsenha" runat="server" TextMode="Password" MaxLength="10"></asp:TextBox></td>
                 </tr>
             <tr><td align="right">Arquivo:</td><td><asp:FileUpload ID="fuparquivo" runat="server" AllowMultiple="false" style="margin-left: 13px" /></td>
                 </tr>
            <tr>
                 <td>&nbsp;</td><td>&nbsp;</td>
                 </tr>
                 <td align="center" colspan="2"><asp:Label ID="Label1" runat="server" Text="Para script com execuções que levam muito tempo na sua execução ou que demandam de muito processamento,<br> USAR as ferramentas diretamente (SQL PLUS ou PL/SQL Developer)" Font-Bold="True" ForeColor="#660033"></asp:Label></td>
                 </tr>
            <tr>
                 <td>&nbsp;</td><td>&nbsp;</td>
                 </tr>
             <tr>
             <tr><td align="center" colspan="2">
                     <asp:Button ID="btndeploy" runat="server" Text="Executar Procedimento" OnClick="btndeploy_Click" /> </td>
                 </tr>
             <tr>
                 <td>&nbsp;</td><td>&nbsp;</td>
                 </tr>
             <tr>
                 <td align="center" colspan="2">
                     <asp:TextBox ID="txt_response" runat="server" Height="436px" TextMode="MultiLine" Width="710px" ReadOnly="true"></asp:TextBox></td>
                 </td>
                 </tr>
         </table>
         
    </div>
    </form>
</body>
</html>
