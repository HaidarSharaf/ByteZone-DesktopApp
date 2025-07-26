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
    public partial class Customers : Form
    {
        CustomerCRUD customerCRUD = new CustomerCRUD();
        Customer customer;

        public Customers()
        {
            InitializeComponent();
        }

        private void Customers_Load(object sender, EventArgs e)
        {
            refreshList();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtEmail.Text == "" || txtPhone.Text == "" || cmbCity.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all fields!");
                return;
            }

            customer = new Customer
            {
                Name = txtName.Text,
                Email = txtEmail.Text,
                PhoneNumber = txtPhone.Text,
                City = cmbCity.Text
            };

            customerCRUD.Add(customer);
            MessageBox.Show("Customer added successfully!");
            clear();
            refreshList();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (listViewCustomers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a customer to update!");
                return;
            }
            if (txtName2.Text == "" || txtEmail2.Text == "" || txtPhone2.Text == "" || cmbCity2.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all fields!");
                return;
            }

            ListViewItem selectedItem = listViewCustomers.SelectedItems[0];
            Customer selected = (Customer)selectedItem.Tag;

            selected.Name = txtName2.Text;
            selected.Email = txtEmail2.Text;
            selected.PhoneNumber = txtPhone2.Text;
            selected.City = cmbCity2.Text;

            customerCRUD.Update(selected);
            MessageBox.Show("Customer updated successfully!");
            clearUpdate();
            refreshList();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listViewCustomers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a customer to delete!");
                return;
            }

            ListViewItem selectedItem = listViewCustomers.SelectedItems[0];
            Customer selected = (Customer)selectedItem.Tag;
            int selectedID = selected.ID;

            customerCRUD.Delete(selectedID);
            MessageBox.Show("Customer deleted successfully!");
            clearUpdate();
            refreshList();
        }

        private void listViewCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewCustomers.SelectedItems.Count > 0) { 
                ListViewItem selectedItem = listViewCustomers.SelectedItems[0];
                Customer selected = (Customer)selectedItem.Tag;

                txtName2.Text = selected.Name;
                txtEmail2.Text = selected.Email;
                txtPhone2.Text = selected.PhoneNumber;
                cmbCity2.SelectedIndex = cmbCity2.Items.IndexOf(selected.City);
            }
        }

        private void clear()
        {
            txtName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            cmbCity.SelectedIndex = -1;
        }

        private void clearUpdate()
        {
            txtName2.Clear();
            txtEmail2.Clear();
            txtPhone2.Clear();
            cmbCity2.SelectedIndex = -1;
        }

        private void refreshList()
        {
            listViewCustomers.Items.Clear();
            ListViewItem item;
            foreach (Customer customer in customerCRUD.GetAll())
            {
                item = new ListViewItem(customer.Name);
                item.SubItems.Add(customer.Email);
                item.SubItems.Add(customer.PhoneNumber);
                item.SubItems.Add(customer.City);
                item.SubItems.Add(customer.TotalSpent.ToString("C2"));
                item.Tag = customer;
                listViewCustomers.Items.Add(item);
            }
        }
    }
}
