using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mini_Inventory_System
{
    public partial class MiniInventorySystem : Form
    {
        public MiniInventorySystem()
        {
            InitializeComponent();
        }

        public ListViewItem ItemToUpdate;
        ListViewItem[] arrSearchItems = new ListViewItem[200];
        int NumberOfSearchedItems = 0;
        int TotalInventoryValue = 0;

        void AddSearchedItemToArray(ListViewItem Item)
        {
            arrSearchItems[NumberOfSearchedItems] = Item;
            NumberOfSearchedItems++;
        }
        void SetSearchedItems(string ProductName)
        {
            for(int i=0;i<listView1.Items.Count;i++)
            {
                if(ProductName.Trim().ToLower()==listView1.Items[i].Text.ToLower())
                {
                    AddSearchedItemToArray(listView1.Items[i]);
                }
            }
        }

        void ShowItems(string Items)
        {
            MessageBox.Show(Items,"Search");
        }
        void ShowSearchedItems(string ProductName)
        {
            SetSearchedItems(ProductName);

            if(NumberOfSearchedItems==0)
            {
                MessageBox.Show("There is no items found match this item");
                return;
            }

            string sItems = "";

            sItems += "Item:\n\n\n";
            sItems += "Product | Quantity | Total Price\n";
            for(int i=0;i<NumberOfSearchedItems;i++)
            {
                sItems += arrSearchItems[i].Text +"\t"+arrSearchItems[i].SubItems[1].Text+
                   "\t" + arrSearchItems[i].SubItems[2].Text+ "\n";
            }

            ShowItems(sItems);
            NumberOfSearchedItems = 0;
        }

        void Clear()
        {
            listView1.Items.Clear();
            cbProducts.SelectedIndex = 0;
            nudQuantity.Value = 1;
            TotalInventoryValue = 0;
            UpdateTotal();
            NumberOfSearchedItems = 0;
            txtSearch.Text = "🔍 Search here...";
        }
        void UpdateTotal()
        {
            lblTotal.Text = TotalInventoryValue.ToString();
        }
        bool IsProductExist(string ProductName)
        {
            for(int i=0;i<listView1.Items.Count;i++)
            {
                if (ProductName == listView1.Items[i].Text)
                    return true;
            }

            return false;
        }

        int GetItemPrice(string ProductName)
        {
            switch(ProductName.ToLower())
            {
                case "laptop":
                    return 15000;

                case "mouse":
                    return 200;

                case "keyboard":
                    return 350;

                case "headphones":
                    return 600;

                default:
                    return 0;
            }
        }

        int GetTotalPrice(string ProductName,int Quantity)
        {
            return (int) GetItemPrice(ProductName) * Quantity;
        }

        void IncreaseProductQuantity(string ProductName, int Quantity)
        {

            for(int i=0;i<listView1.Items.Count;i++)
            {
                if(ProductName==listView1.Items[i].Text)
                {
                    int NewQantity = (Convert.ToInt32(listView1.Items[i].SubItems[1].Text) + Quantity);
                    UpdateQuantity(listView1.Items[i],NewQantity);
                    return;
                }

             
            }
        }
        void AddItemToListView(string ProductName,int Quantity)
        {
            if(IsProductExist(ProductName))
            {
                IncreaseProductQuantity(ProductName, Quantity);
                return;
            }

            ListViewItem item = new ListViewItem(ProductName);
            item.SubItems.Add(Quantity.ToString());
            int ProductTotal = GetTotalPrice(ProductName, Quantity);
            item.SubItems.Add(ProductTotal.ToString());

            listView1.Items.Add(item);

            TotalInventoryValue += ProductTotal;
            UpdateTotal();
        }

        void RemoveItem()
        {
            if (listView1.SelectedItems.Count > 0)
            {
                if(MessageBox.Show("Are you sure you want to remove this item?", "Confirm"
                    ,MessageBoxButtons.OKCancel,MessageBoxIcon.Information)==DialogResult.OK)
                {
                    TotalInventoryValue -= Convert.ToInt32(listView1.SelectedItems[0].SubItems[2].Text);
                    listView1.SelectedItems[0].Remove();
                    UpdateTotal();
                    MessageBox.Show("Removed successfully");
            
                }
            }
 
        }

        
        void UpdateQuantity(ListViewItem ItemUpdate,int NewQuantiy)
        {
            TotalInventoryValue -= Convert.ToInt32(ItemUpdate.SubItems[2].Text);

            ItemUpdate.SubItems[1].Text = NewQuantiy.ToString();
            ItemUpdate.SubItems[2].Text = GetTotalPrice(ItemUpdate.Text,NewQuantiy).ToString();

            TotalInventoryValue += Convert.ToInt32(ItemUpdate.SubItems[2].Text);
            UpdateTotal();
        }
        void UpdateItem()
        {
           UpdateProduct frmUpdateProduct = new UpdateProduct(ItemToUpdate);
           if( frmUpdateProduct.ShowDialog()== DialogResult.OK)
            {
                UpdateQuantity(ItemToUpdate,frmUpdateProduct.NewQuantiy);
            }   
        }

        void Update()
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ItemToUpdate = listView1.SelectedItems[0];
                UpdateItem();
            }
        }
        void EnabledContextMenu()
        {
            if (listView1.SelectedItems.Count > 0)
            {
                updateToolStripMenuItem.Enabled = true;
                removeToolStripMenuItem.Enabled = true;
                clearToolStripMenuItem.Enabled = false;
            }
            else
            {
                updateToolStripMenuItem.Enabled = false;
                removeToolStripMenuItem.Enabled = false;
                clearToolStripMenuItem.Enabled = true;
            }
        }
        private void MiniInventorySystem_Load(object sender, EventArgs e)
        {
            cbProducts.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddItemToListView(cbProducts.SelectedItem.ToString(),
                Convert.ToInt32(nudQuantity.Value));
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select item to remove");
                return;
            }
            RemoveItem();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count==0)
            {
                MessageBox.Show("Please select item to update");
                return;
            }
            Update();
        }

        private void nudQuantity_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if(txtSearch.Text == "🔍 Search here...")        
                  txtSearch.Clear();
       
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtSearch.Text.Trim()))
                txtSearch.Text = "🔍 Search here...";
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ShowSearchedItems(txtSearch.Text);

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text.Trim()) &&
                txtSearch.Text != "🔍 Search here...")
                btnSearch.Enabled = true;
            else
                btnSearch.Enabled = false;
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Update();
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button==MouseButtons.Right)
            {
                ListViewItem item = listView1.GetItemAt(e.X,e.Y);

                if(item !=null)
                {
                    item.Selected = true;
                }
                else
                {
                    listView1.SelectedItems.Clear();
                }

            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            EnabledContextMenu();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveItem();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clear();
        }

      
    }
}
