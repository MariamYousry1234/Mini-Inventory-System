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
    public partial class UpdateProduct : Form
    {
        public UpdateProduct(ListViewItem ItemToUpdate)
        {
            InitializeComponent();

            txtProductName.Text = ItemToUpdate.Text;
            nudNewQuantity.Value = Convert.ToDecimal(ItemToUpdate.SubItems[1].Text);
        }

        public int NewQuantiy = 0;
        
       

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            NewQuantiy =Convert.ToInt32(nudNewQuantity.Value);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void nudNewQuantity_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

       
    }
}
