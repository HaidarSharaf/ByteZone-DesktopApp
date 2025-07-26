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
    public partial class Categories : Form
    {
        CategoryCRUD categoryCRUD = new CategoryCRUD();
        Category category;

        public Categories()
        {
            InitializeComponent();
        }

        private void Categories_Load(object sender, EventArgs e)
        {
            refreshList();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (listViewCategories.SelectedItems.Count > 0)
            {
                MessageBox.Show("Cannot create. You already choosed a category from the list. You can only update or delete!");
                return;
            }


            if (txtName.Text == "")
            {
                MessageBox.Show("Please fill in all fields!");
                return;
            }

            category = new Category
            {
                Name = txtName.Text
            };

            categoryCRUD.Add(category);
            MessageBox.Show("Category added successfully!");
            clear();
            refreshList();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (listViewCategories.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a category to update!");
                return;
            }

            if (txtName.Text == "")
            {
                MessageBox.Show("Please fill in all fields!");
                return;
            }

            ListViewItem selectedItem = listViewCategories.SelectedItems[0];
            Category selected = (Category)selectedItem.Tag;

            selected.Name = txtName.Text;

            categoryCRUD.Update(selected);
            MessageBox.Show("Category updated successfully!");
            clear();
            refreshList();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listViewCategories.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a category to delete!");
                return;
            }

            ListViewItem selectedItem = listViewCategories.SelectedItems[0];
            Category selected = (Category)selectedItem.Tag;

            int selectedID = selected.ID;

            categoryCRUD.Delete(selectedID);
            MessageBox.Show("Category deleted successfully!");
            clear();
            refreshList();
        }

        private void clear()
        {
            txtName.Clear();
        }

        private void refreshList()
        {
            listViewCategories.Items.Clear();
            ListViewItem item;
            foreach (Category category in categoryCRUD.GetAll())
            {
                item = new ListViewItem(category.Name);
                item.Tag = category;
                listViewCategories.Items.Add(item);
            }
        }

        private void listViewCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewCategories.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listViewCategories.SelectedItems[0];
                Category selected = (Category)selectedItem.Tag;

                txtName.Text = selected.Name;
            }
        }
    }
}
