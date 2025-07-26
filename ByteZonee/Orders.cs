using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByteZonee
{
    public partial class Orders : Form
    {
        OrderCRUD orderCRUD = new OrderCRUD();
        ProductCRUD productCRUD = new ProductCRUD();
        Order order;

        List<OrderDetail> cartItems = new List<OrderDetail>();

        public Orders()
        {
            InitializeComponent();
        }

        private void Orders_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'byteZoneDataSet.Customers' table. You can move, or remove it, as needed.
            this.customersTableAdapter.Fill(this.byteZoneDataSet.Customers);
            refreshListOrders();
            refreshListProducts();
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (listViewProducts.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a product to add to the cart!");
                return;
            }

            ListViewItem selectedItem = listViewProducts.SelectedItems[0];
            Product selectedProduct = (Product)selectedItem.Tag;

            if (selectedProduct.Stock == 0)
            {
                MessageBox.Show("This product is out of stock!");
                return;
            }


            int quantity = (int)numQuantity.Value;

            if (quantity > selectedProduct.Stock)
            {
                MessageBox.Show("Not enough stock!");
                numQuantity.Value = 1;
                return;
            }

            if (cartItems.FirstOrDefault(product => product.ProductID == selectedProduct.ID) != null)
            {
                MessageBox.Show("Product already in cart!");
                return;
            }

            cartItems.Add(new OrderDetail
            {
                ProductID = selectedProduct.ID,
                Quantity = quantity,
                ProductName = selectedProduct.Name,
                UnitPrice = selectedProduct.Price * quantity
            });

            refreshListCart();
            numQuantity.Value = 1;
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a customer!");
                return;
            }

            if (cartItems.Count == 0)
            {
                MessageBox.Show("Your cart is empty!");
                return;
            }

            order = new Order
            {
                CustomerID = Convert.ToInt32(cmbCustomer.SelectedValue),
                OrderDate = DateTime.Now,
                TotalPrice = double.Parse(txtSubtotal.Text.Replace("$", "").Replace(",", "")),
                Details = cartItems
            };

            orderCRUD.Add(order);
            MessageBox.Show("Order placed successfully!");
            clearCart();
            refreshListProducts();
            refreshListOrders();
            cmbCustomer.SelectedIndex = -1;
            numQuantity.Value = 1;
        }

        private void clearCart()
        {
            listViewCart.Items.Clear();
            cartItems.Clear();
            txtSubtotal.Text = "$0.00";
        }

        private void refreshListOrders()
        {
            listViewAllOrders.Items.Clear();
            ListViewItem item;
            foreach (Order order in orderCRUD.GetAll())
            {
                item = new ListViewItem(new CustomerCRUD().GetCustomerById(order.CustomerID).Name);
                item.SubItems.Add(order.OrderDate.ToString("dd/MM/yyyy"));
                string str = "";
                foreach(OrderDetail detail in order.Details)
                {
                    str += detail.ProductName + " (" + detail.Quantity + "), ";
                }
                item.SubItems.Add(str.TrimEnd(',', ' '));
                item.SubItems.Add(order.TotalPrice.ToString("C2"));
                listViewAllOrders.Items.Add(item);
            }
        }

        private void refreshListProducts()
        {
            listViewProducts.Items.Clear();
            foreach (Product product in productCRUD.GetAll())
            {
                ListViewItem item = new ListViewItem(product.Name);
                item.SubItems.Add(product.Price.ToString("C2"));
                item.SubItems.Add(product.Stock.ToString());
                item.Tag = product;
                listViewProducts.Items.Add(item);
            }
        }

        private void refreshListCart()
        {
            listViewCart.Items.Clear();
            double subtotal = 0;
            ListViewItem item;

            foreach (OrderDetail detail in cartItems)
            {
                subtotal += detail.UnitPrice * detail.Quantity;
                item = new ListViewItem(detail.ProductName);
                item.SubItems.Add(detail.UnitPrice.ToString("C2"));
                item.SubItems.Add(detail.Quantity.ToString());
                item.SubItems.Add((detail.UnitPrice * detail.Quantity).ToString("C2"));

                item.Tag = detail;

                listViewCart.Items.Add(item);
            }

            txtSubtotal.Text = subtotal.ToString("C2");
        }
    }
}
