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
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            labelNumProducts.Text = new ProductCRUD().CountProducts().ToString();
            labelNumCustomers.Text = new CustomerCRUD().CountCustomers().ToString();
            labelNumCategories.Text = new CategoryCRUD().CountCategories().ToString();
            labelNumOrders.Text = new OrderCRUD().CountOrders().ToString();
            labelNumSales.Text = new OrderCRUD().GetTotalSales().ToString("C");
        }

        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Customers frm = new Customers();
            frm.Show();
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Products frm = new Products();
            frm.Show();
        }

        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Categories frm = new Categories();
            frm.Show();
        }

        private void ordersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Orders frm = new Orders();
            frm.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
