<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="uploadmodulo.aspx.cs" Inherits="WebApplication1.Default" %>

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
						<p>DESENVOLVIMENTO SSE - UPLOAD DOS MÓDULOS PARA O SERVIDOR DE DESENVOLVIMENTO</p>
						</td>
						<td align="right" valign="bottom">
							<img src="img/login_logo.gif" width="136" height="31" alt="BMC logo" border="0"> 
						</td>
					</tr>
					</tbody></table>
				</td>
			</tr>
		</tbody></table>
            <p><a href="default.aspx">Home</a>&nbsp;&nbsp;&nbsp;<a target="_blank" href="uploadmodulo.pdf"><b><font color="#000099" >Instruções de Uso</font></b></a></p>
        <hr color="black" />
        <br />
            <br />
    <form id="form1" runat="server">
         <table align="center" width="80%" >
             <tr><td>Ambiente:<asp:DropDownList ID="ddlambiente" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlambiente_SelectedIndexChanged"></asp:DropDownList>&nbsp;&nbsp;<i>(Escolha o ambiente "simulado" no servidor de desenvolvimento)</i></td>
                 </tr>
             <tr><td>Sistema:&nbsp;&nbsp; <asp:DropDownList ID="ddlsistema" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlsistema_SelectedIndexChanged" ></asp:DropDownList></td>
                 </tr>
             <tr><td>Arquivo:<asp:FileUpload ID="fuparquivo" runat="server" style="margin-left: 13px" />&nbsp;&nbsp;<i>(Informe o nome do arquivo (.zip) com o mesmo nome indicado no campo Sistema)</i></td>
                 </tr>
             <tr>
                 <td><asp:CheckBox ID="chkenvio" runat="server" Text="Disponibilizar o arquivo(.zip) para uso de GMUD.&nbsp;&nbsp;Para alguns sistemas essa opção não está disponível. <BR>
                     <b>&nbsp;&nbsp;&nbsp;&nbsp;Ao checar essa opção ATENTAR-SE para o envio formal  de solicitação de GMUD.</b>" /></td>
             </tr>
             <tr>
                 <td>&nbsp;</td>
            </tr>
              <tr>
                 <td>&nbsp;</td>
            </tr>
             <tr><td align="center">
                     <asp:Button ID="btndeploy" runat="server" Text="Executar Procedimento" OnClick="btndeploy_Click" /> </td>
                 </tr>
             <tr>
                 <td>&nbsp;</td>
            </tr>
             <tr>
                 <td>&nbsp;</td>
            </tr>
             <tr>
                 <td align="center">
                     <asp:Label ID="lblresultado" runat="server" Font-Bold="True" ForeColor="#000099"></asp:Label>
                 </td>
             </tr>
             <tr>
                 <td align="center">
                 <asp:TextBox ID="txt_response" runat="server" Height="298px" TextMode="MultiLine" Width="710px" ReadOnly="true"></asp:TextBox></td>
             </tr>
             <tr>
                 <td>&nbsp;</td>
            </tr>
         </table>
    </form>
</body>
</html>
