<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs"
    Inherits="Inventory_Management.Default" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Inventory Management</title>

    <style>
        body {
            font-family: Arial;
            margin: 30px;
            background: #f2f2f2;
        }

        .container {
            max-width: 900px;
            margin: auto;
            padding: 20px;
            background: white;
        }

        .input {
            width: 100%;
            padding: 8px;
            margin: 8px 0 15px;
            box-sizing: border-box;
        }

        .button {
            padding: 9px 15px;
            margin-right: 8px;
            cursor: pointer;
        }

        .grid {
            width: 100%;
            margin-top: 20px;
        }

        .grid th, .grid td {
            padding: 10px;
            border: 1px solid #ccc;
        }

        .message {
            display: block;
            margin-top: 12px;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Inventory Management System</h1>

            <asp:HiddenField ID="hfProductId" runat="server" />

            <label>Product Name</label>
            <asp:TextBox ID="txtProductName" runat="server"
                CssClass="input" MaxLength="100" />

            <label>Quantity</label>
            <asp:TextBox ID="txtQuantity" runat="server"
                CssClass="input" TextMode="Number" />

            <label>Price</label>
            <asp:TextBox ID="txtPrice" runat="server"
                CssClass="input" />

            <asp:Button ID="btnSave" runat="server"
                Text="Add Product" CssClass="button"
                OnClick="btnSave_Click" />

            <asp:Button ID="btnClear" runat="server"
                Text="Clear" CssClass="button"
                CausesValidation="false"
                OnClick="btnClear_Click" />

            <asp:Label ID="lblMessage" runat="server"
                CssClass="message" />

            <h2>Product List</h2>

            <asp:GridView ID="gvProducts" runat="server"
                AutoGenerateColumns="False"
                CssClass="grid"
                OnRowCommand="gvProducts_RowCommand"
                EmptyDataText="No products available.">
                <Columns>
                    <asp:BoundField DataField="ProductId"
                        HeaderText="Product ID" />
                    <asp:BoundField DataField="ProductName"
                        HeaderText="Product Name" />
                    <asp:BoundField DataField="Quantity"
                        HeaderText="Quantity" />
                    <asp:BoundField DataField="Price"
                        HeaderText="Price"
                        DataFormatString="{0:N2}" />

                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server"
                                Text="Edit"
                                CommandName="EditProduct"
                                CommandArgument='<%# Eval("ProductId") %>'
                                CausesValidation="false" />
                            &nbsp;
                            <asp:LinkButton ID="btnDelete" runat="server"
                                Text="Delete"
                                CommandName="DeleteProduct"
                                CommandArgument='<%# Eval("ProductId") %>'
                                CausesValidation="false"
                                OnClientClick="return confirm('Delete this product?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
