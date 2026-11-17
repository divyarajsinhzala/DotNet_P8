using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Inventory_Management
{
    public partial class Default : Page
    {
        string cs = ConfigurationManager
            .ConnectionStrings["InventoryConnection"]
            .ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        // READ
        private void LoadProducts()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Products ORDER BY ProductId";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvProducts.DataSource = dt;
                    gvProducts.DataBind();
                }
            }
        }

        // CREATE AND UPDATE
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtProductName.Text.Trim();
            int quantity;
            decimal price;

            if (name == "")
            {
                ShowMessage("Enter product name.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out quantity)
                || quantity < 0)
            {
                ShowMessage("Enter a valid quantity.");
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out price)
                || price < 0
                || price > 99999999.99m)
            {
                ShowMessage("Enter a valid price.");
                return;
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                string query;
                bool isInsert = (hfProductId.Value == "");

                if (isInsert)
                {
                    query = "INSERT INTO Products " +
                            "(ProductName, Quantity, Price) " +
                            "VALUES (@Name, @Quantity, @Price)";
                }
                else
                {
                    query = "UPDATE Products SET " +
                            "ProductName=@Name, Quantity=@Quantity, Price=@Price " +
                            "WHERE ProductId=@Id";
                }

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = name;
                    cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = quantity;

                    SqlParameter pPrice = cmd.Parameters.Add("@Price", SqlDbType.Decimal);
                    pPrice.Precision = 10;
                    pPrice.Scale = 2;
                    pPrice.Value = price;

                    if (!isInsert)
                    {
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value =
                            Convert.ToInt32(hfProductId.Value);
                    }

                    cmd.ExecuteNonQuery();
                }

                ShowMessage(isInsert
                    ? "Product added successfully."
                    : "Product updated successfully.");
            }

            ClearForm();
            LoadProducts();
        }

        // EDIT AND DELETE
        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id;

            if (!int.TryParse(Convert.ToString(e.CommandArgument), out id))
            {
                ShowMessage("Invalid product ID.");
                return;
            }

            if (e.CommandName == "EditProduct")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = "SELECT * FROM Products WHERE ProductId=@Id";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                hfProductId.Value = reader["ProductId"].ToString();
                                txtProductName.Text = reader["ProductName"].ToString();
                                txtQuantity.Text = reader["Quantity"].ToString();
                                txtPrice.Text = reader["Price"].ToString();

                                btnSave.Text = "Update Product";
                                ShowMessage("Edit the details and click Update Product.");
                            }
                            else
                            {
                                ShowMessage("Product not found.");
                            }
                        }
                    }
                }
            }
            else if (e.CommandName == "DeleteProduct")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = "DELETE FROM Products WHERE ProductId=@Id";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                        con.Open();
                        int rows = cmd.ExecuteNonQuery();

                        ShowMessage(rows > 0
                            ? "Product deleted successfully."
                            : "Product not found.");
                    }
                }

                ClearForm();
                LoadProducts();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            lblMessage.Text = "";
            LoadProducts();
        }

        private void ClearForm()
        {
            hfProductId.Value = "";
            txtProductName.Text = "";
            txtQuantity.Text = "";
            txtPrice.Text = "";
            btnSave.Text = "Add Product";
        }

        private void ShowMessage(string message)
        {
            lblMessage.Text = Server.HtmlEncode(message);
        }
    }
}
