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
    public partial class Products : Form
    {
        ProductCRUD productCRUD = new ProductCRUD();
        Product product;
        CategoryCRUD categoryCRUD = new CategoryCRUD();
        List<Category> categories;
        public Products()
        {
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            categories = categoryCRUD.GetAll();
            cmbCategory.DataSource = categories;
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "ID";
            cmbCategory2.DataSource = categories;
            cmbCategory2.DisplayMember = "Name";
            cmbCategory2.ValueMember = "ID";

            refreshList();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || numPrice.Value == 0 || numStock.Value == 0 || cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all fields!");
                return;
            }

            product = new Product
            {
                Name = txtName.Text,
                CategoryID = (int)cmbCategory.SelectedValue,
                Price = (double)numPrice.Value,
                Stock = (int)numStock.Value
            };

            productCRUD.Add(product);
            MessageBox.Show("Product added successfully!");
            clear();
            refreshList();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (listViewProducts.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a product to update!");
                return;
            }

            if (txtName2.Text == "" || numPrice2.Value == 0 || cmbCategory2.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all fields!");
                return;
            }

            ListViewItem selectedItem = listViewProducts.SelectedItems[0];
            Product selected = (Product)selectedItem.Tag;

            selected.Name = txtName2.Text;
            selected.CategoryID = (int)cmbCategory2.SelectedValue;
            selected.Price = (double)numPrice2.Value;
            selected.Stock = (int)numStock2.Value;

            productCRUD.Update(selected);
            MessageBox.Show("Product updated successfully!");
            clearUpdate();
            refreshList();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listViewProducts.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a product to delete!");
                return;
            }

            ListViewItem selectedItem = listViewProducts.SelectedItems[0];
            Product selected = (Product)selectedItem.Tag;
            int selectedID = selected.ID;

            productCRUD.Delete(selectedID);
            MessageBox.Show("Product deleted successfully!");
            clearUpdate();
            refreshList();
        }

        private void listViewProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewProducts.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listViewProducts.SelectedItems[0];
                Product selected = (Product)selectedItem.Tag;

                txtName2.Text = selected.Name;
                cmbCategory2.SelectedValue = selected.CategoryID;
                numPrice2.Value = (decimal)selected.Price;
                numStock2.Value = (int)selected.Stock;
            }
        }

        private void clear()
        {
            txtName.Clear();
            cmbCategory.SelectedIndex = -1;
            numPrice.Value = 1;
            numStock.Value = 1;
        }

        private void clearUpdate()
        {
            txtName2.Clear();
            cmbCategory2.SelectedIndex = -1;
            numPrice2.Value = 1;
            numStock2.Value = 1;
        }

        private void refreshList()
        {

            listViewProducts.Items.Clear();
            ListViewItem item;
            foreach (Product product in productCRUD.GetAll())
            {
                item = new ListViewItem(product.Name);
                item.SubItems.Add(new CategoryCRUD().GetCategory(product.CategoryID).Name);
                item.SubItems.Add(product.Price.ToString("C"));
                item.SubItems.Add(product.Stock.ToString());
                item.SubItems.Add(product.SalesCount.ToString());
                item.Tag = product;
                listViewProducts.Items.Add(item);
            }
        }
    }
}
