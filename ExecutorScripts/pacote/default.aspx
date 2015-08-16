<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="WebApplication1.Default2" %>

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
						<p>EQUIPE DESENVOLVIMENTO SSE</p>
						</td>
						<td align="right" valign="bottom">
							<img src="img/login_logo.gif" width="136" height="31" alt="BMC logo" border="0"> 
						</td>
					</tr>
					</tbody></table>
				</td>
			</tr>
		</tbody></table>
		<hr color="black">
    <ul>
        <li><a href="validascript.aspx">Validador de Script</a></li>
        <li><a href="http:\\10.200.240.19\dbscripts2\execscript.aspx">Executor de Script</a></li>
		<li><a href="infotabela.aspx">Consulta Tabela (Ambiente Referência)</a></li>
        <li><a href="comparatabelas.aspx">Comparação Tabela (Ambiente Referência x Ambiente GMUD)</a></li>
    </ul>
</body>
</html>
