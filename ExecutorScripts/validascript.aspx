<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="validascript.aspx.cs" Inherits="WebApplication1.validascript" %>

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
						<p>DESENVOLVIMENTO SSE - VALIDAÇÃO DE SCRIPTS  - AMBIENTE DESENVOLVIMENTO</p>
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
             <tr><td>Owner:&nbsp;&nbsp; <asp:DropDownList ID="ddlsistema" runat="server"></asp:DropDownList></td>
                 </tr>
             <tr><td>Arquivo:<asp:FileUpload ID="fuparquivo" runat="server" AllowMultiple="true" style="margin-left: 13px" /></td>
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
                     <asp:TextBox ID="txt_response" runat="server" Height="298px" TextMode="MultiLine" Width="710px" ReadOnly="true"></asp:TextBox></td>
                 </td>
                 </tr>
         </table>
         
    </div>
    </form>
</body>
</html>
