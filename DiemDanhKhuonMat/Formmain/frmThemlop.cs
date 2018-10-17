using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Formmain
{
    public partial class frmThemlop : Form
    {
        public frmThemlop()
        {
            InitializeComponent();
            cbKieuLop.Items.Add("Lớp chuyên ngành");
            cbKieuLop.Items.Add("Lớp tín chỉ");
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            //them lop vao treeview
            this.Dispose(); 
            frmThemSinhVien frm = new frmThemSinhVien();
            frm.ShowDialog();

            
        }
    }
}
