using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteZonee
{
    public class OrderDetailCRUD
    {
        private static string connectionString = "Data Source=localhost;Initial Catalog=ByteZone;Integrated Security=True";

        public List<OrderDetail> GetOrder(int orderId)
        {
            List<OrderDetail> details = new List<OrderDetail>();

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand("SELECT ord.ID, ord.OrderID, ord.ProductID, ord.Quantity, p.Name AS ProductName, p.Price " +
                                                "FROM OrderDetails ord " +
                                                "JOIN Products p ON ord.ProductID = p.ID " +
                                                "WHERE ord.OrderID = @OrderID",
                                            con);

            cmd.Parameters.AddWithValue("@OrderID", orderId);

            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    details.Add(new OrderDetail
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ProductName = reader["ProductName"].ToString(),
                        UnitPrice = Convert.ToDouble(reader["Price"])
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return details;
        }

        public void Add(OrderDetail detail)
        {
            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand("INSERT INTO OrderDetails (OrderID, ProductID, Quantity) " +
                                            "VALUES (@OrderID, @ProductID, @Quantity)",
                                            con);
            try
            {
                con.Open();
                cmd.Parameters.AddWithValue("@OrderID", detail.OrderID);
                cmd.Parameters.AddWithValue("@ProductID", detail.ProductID);
                cmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
                cmd.ExecuteNonQuery();
                new ProductCRUD().UpdateStockAndSalesCount(detail.ProductID, detail.Quantity);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
