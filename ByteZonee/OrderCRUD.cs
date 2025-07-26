using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByteZonee
{
    public class OrderCRUD
    {
        private static string connectionString = "Data Source=localhost;Initial Catalog=ByteZone;Integrated Security=True";
        public List<Order> GetAll()
        {
            List<Order> orders = new List<Order>();

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM Orders", con);

            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var order = new Order
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        TotalPrice = Convert.ToDouble(reader["TotalPrice"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"])
                    };
                    order.Details = new OrderDetailCRUD().GetOrder(order.ID);
                    orders.Add(order);
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
            return orders;
        }

        public void Add(Order order)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO Orders " +
                                            "(CustomerID, OrderDate, TotalPrice) " +
                                            "VALUES (@CustomerID, @OrderDate, @TotalPrice) " +
                                            "SELECT SCOPE_IDENTITY();",
                                            con);
            try
            {
                con.Open();
                cmd.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                cmd.Parameters.AddWithValue("@TotalPrice", order.TotalPrice);

                int orderID = Convert.ToInt32(cmd.ExecuteScalar());
                order.ID = orderID;

                foreach (var detail in order.Details)
                {
                    detail.OrderID = orderID;
                    new OrderDetailCRUD().Add(detail);
                }

                new CustomerCRUD().UpdateTotalSpent(order.CustomerID, order.TotalPrice);
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

        public int CountOrders()
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Orders", con);

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

        public double GetTotalSales()
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(TotalPrice), 0) FROM Orders", con);

            double totalSales = 0;
            try
            {
                con.Open();
                totalSales = Convert.ToDouble(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return totalSales;
        }
    }
}
