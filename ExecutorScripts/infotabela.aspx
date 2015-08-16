<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="infotabela.aspx.cs" Inherits="WebApplication1.infotabela" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
    <title></title>
        <style type="text/css">
            #txtsearch {
                height: 22px;
                width: 651px;
                margin-left: 0px;
                margin-top: 4px;
            }
            #dvcorpo {
                    font-family:Calibri,Verdana;  
                 }
            .title{font-weight:bold;text-transform:capitalize;}
        </style>
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
						<p>DESENVOLVIMENTO SSE - CONSULTA DADOS TABELA  - AMBIENTE REFERÊNCIA</p>
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
    <form id="form1" runat="server" onsubmit="return validate();"  >
    <div id="dvcorpo">
      
      <p align="center">Owner:<asp:DropDownList ID="dropowner" runat="server"></asp:DropDownList>&nbsp;&nbsp;<input  id="txtsearch" runat="server" type="search" min="4"  maxlength="25" placeholder="Informe a tabela" /><asp:ImageButton  ID="btnsearch" runat="server" ImageUrl="~/img/search.jpg" Height="31px" style="margin-right: 0px" Width="50px" ImageAlign="Middle" OnClick="btnsearch_Click"  />
          </p>
        <p align="center"><asp:Label runat="server" ID="lblvalidacao" Visible="False" ></asp:Label></p>
        

        <div id="dvtabelas" runat="server" style="left" >
        <p class="title">Tabelas</p>
        <hr style="color: #C0C0C0" />
        
            <!--column_name,data_type ,nullable,data_length-->
            <table width="80%" >
             <tr><td >
                 <asp:Label runat="server" ID="lbltabelas" Visible="false" ></asp:Label>
        <asp:DataGrid runat="server" id="dgtabelas" Font-Name="Verdana" AutoGenerateColumns="False" AlternatingItemStyle-BackColor="#eeeeee" HeaderStyle-BackColor="#87481" HeaderStyle-ForeColor="White"
                    HeaderStyle-Font-Size="10pt" HeaderStyle-Font-Bold="True" >
            <Columns>
                    <asp:TemplateColumn HeaderText="Nome">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="lnktabela" NavigateUrl='<%# String.Concat("infotabela.aspx?p_tabela=",Eval("table_name"),"&p_owner=",Eval("owner")) %>'  ><%# DataBinder.Eval(Container, "DataItem.table_name")%></asp:HyperLink>
                        </ItemTemplate>
                   </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
                 </td>
             </tr>
             </table>  
        </div>

        
        <div id="dvcolunas" runat="server" style="left" >
        <p class="title">Colunas</p>
        <hr style="color: #C0C0C0" />
        
            <!--column_name,data_type ,nullable,data_length-->
            <table width="80%" >
             <tr><td >
                 <asp:Label runat="server" ID="lblcoluna" Visible="false" ></asp:Label>
        <asp:DataGrid runat="server" id="dgcolunas" Font-Name="Verdana" AutoGenerateColumns="False" AlternatingItemStyle-BackColor="#eeeeee" HeaderStyle-BackColor="#87481" HeaderStyle-ForeColor="White"
                    HeaderStyle-Font-Size="10pt" HeaderStyle-Font-Bold="True" >
            <Columns>
                    <asp:BoundColumn DataField="table_name" HeaderText="Tabela"  />    
                    <asp:BoundColumn DataField="column_name" HeaderText="Coluna"   />
                    <asp:BoundColumn DataField="data_type" HeaderText="Tipo"   />
                    <asp:BoundColumn DataField="data_length" HeaderText="Tamanho"  ItemStyle-HorizontalAlign="Right"  />
                    <asp:BoundColumn DataField="nullable" HeaderText="Permite Nulo"  />
            </Columns>
        </asp:DataGrid>
                 </td>
             </tr>
             </table>  
        </div>
        <div id="dvindices" runat="server" style="left" >
        <p class="title">Índices</p>
        <hr style="color: #C0C0C0" />
            <table width="80%" >
             <tr><td>
                 <asp:Label runat="server" ID="lblindice" Visible="false" ></asp:Label>
        <asp:DataGrid runat="server" id="dgindices" Font-Name="Verdana" AutoGenerateColumns="False" AlternatingItemStyle-BackColor="#eeeeee" HeaderStyle-BackColor="#87481" HeaderStyle-ForeColor="White"
                    HeaderStyle-Font-Size="10pt" HeaderStyle-Font-Bold="True" >
            <Columns>
                    <asp:BoundColumn DataField="table_name" HeaderText="Tabela"  />        
                    <asp:BoundColumn DataField="index_name" HeaderText="Indice"  />
                    <asp:BoundColumn DataField="column_name" HeaderText="Coluna"   />
                    <asp:BoundColumn DataField="column_position" HeaderText="Posicao"   />
            </Columns>
        </asp:DataGrid>
                 </td>
             </tr>
             </table>  
        </div>
        <div id="dvconstraint" runat="server" style="left" >
        <p class="title">Constraints</p>
        <hr style="color: #C0C0C0" />
            <table width="80%" >
             <tr><td >
         <asp:Label runat="server" ID="lblconstraint" Visible="false" ></asp:Label>
        <asp:DataGrid runat="server" id="dgconstraint" Font-Name="Verdana" AutoGenerateColumns="False" AlternatingItemStyle-BackColor="#eeeeee" HeaderStyle-BackColor="#87481" HeaderStyle-ForeColor="White"
                    HeaderStyle-Font-Size="10pt" HeaderStyle-Font-Bold="True" >
            <Columns>
                <asp:BoundColumn DataField="constraint_name" HeaderText="Constraint"  />    
                <asp:TemplateColumn HeaderText="Tabela_Ref">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="lnkconstraint" NavigateUrl='<%# String.Concat("infotabela.aspx?p_tabela=",Eval("ref_table"),"&p_owner=",Eval("ref_owner")) %>'  ><%# DataBinder.Eval(Container, "DataItem.ref_table")%></asp:HyperLink>
                        </ItemTemplate>
                   </asp:TemplateColumn>
                <asp:BoundColumn DataField="ref_owner" HeaderText="Owner_Ref"  />
                <asp:BoundColumn DataField="table_name" HeaderText="Tabela"  />    
                <asp:BoundColumn DataField="owner" HeaderText="Owner"  />
                <asp:BoundColumn DataField="column_name" HeaderText="Coluna"  />
                <asp:BoundColumn DataField="position" HeaderText="Posicao"  />
            </Columns>
        </asp:DataGrid>
                 </td>
             </tr>
             </table>  
        </div>
        <div id="dvgrant" runat="server" style="left" >
        <p class="title">Grants</p>
        <hr style="color: #C0C0C0" />
        
            <!--column_name,data_type ,nullable,data_length-->
            <table width="80%" >
             <tr><td >
                 <asp:Label runat="server" ID="lblgrant" Visible="false" ></asp:Label>
        <asp:DataGrid runat="server" id="dggrant" Font-Name="Verdana" AutoGenerateColumns="False" AlternatingItemStyle-BackColor="#eeeeee" HeaderStyle-BackColor="#87481" HeaderStyle-ForeColor="White"
                    HeaderStyle-Font-Size="10pt" HeaderStyle-Font-Bold="True" >
            <Columns>
                    <asp:BoundColumn DataField="table_name" HeaderText="Tabela"  />    
                    <asp:BoundColumn DataField="owner" HeaderText="Coluna"   />
                    <asp:BoundColumn DataField="grantee" HeaderText="grantee"   />
                    <asp:BoundColumn DataField="grantor" HeaderText="grantor"  />
                    <asp:BoundColumn DataField="privilege" HeaderText="privilege"  />
            </Columns>
        </asp:DataGrid>
                 </td>
             </tr>
             </table>  
        </div>
    </div>
    </form>
</body>
<script type="text/javascript" >
    function validate() {
        submitFlag = true;
        search = document.getElementById("txtsearch");
        if (search.value.length < 4) {
            submitFlag = false;
            alert("Informe pelo menos 4 caracteres");
        }
        return submitFlag;
    }
</script>
</html>
