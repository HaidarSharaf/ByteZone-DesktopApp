using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByteZonee
{
    public class ProductCRUD
    {
        private static string connectionString = "Data Source=localhost;Initial Catalog=ByteZone;Integrated Security=True";
        public List<Product> GetAll()
        {
            List<Product> products = new List<Product>();

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM Products", con);

            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        Name = reader["Name"].ToString(),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        Price = Convert.ToDouble(reader["Price"]),
                        Stock = Convert.ToInt32(reader["Stock"]),
                        SalesCount = Convert.ToInt32(reader["SalesCount"])
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return products;
        }

        public void Add(Product product)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO Products " +
                                            "(Name, CategoryID, Stock, Price, SalesCount) " +
                                            "VALUES (@Name, @CategoryID, @Stock, @Price, @SalesCount)",
                                            con);
            try
            {
                con.Open();
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@CategoryID", product.CategoryID);
                cmd.Parameters.AddWithValue("@Stock", product.Stock);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@SalesCount", product.SalesCount);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public void Update(Product product)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("UPDATE Products " +
                                "SET Name = @Name, CategoryID = @CategoryID, Stock = @Stock, Price = @Price, SalesCount = @SalesCount " +
                                "WHERE ID = @ID",
                                con
                                );

            try
            {
                con.Open();
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@CategoryID", product.CategoryID);
                cmd.Parameters.AddWithValue("@Stock", product.Stock);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@SalesCount", product.SalesCount);
                cmd.Parameters.AddWithValue("@ID", product.ID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public void Delete(int id)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE ID = @ID", con);
            try
            {
                con.Open();
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public void UpdateStockAndSalesCount(int productId, int quantity)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("UPDATE Products " +
                                            "SET Stock = Stock - @Quantity, SalesCount = SalesCount + @Quantity " +
                                            "WHERE ID = @ProductID AND Stock >= @Quantity"
                                            , con);

            try
            {
                con.Open();
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new Exception("Insufficient stock!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public int CountProducts()
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Products", con);
            int count = 0;
            try
            {
                con.Open();
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return count;
        }
    }
}
