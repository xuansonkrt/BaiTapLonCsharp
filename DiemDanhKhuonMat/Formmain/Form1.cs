using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Formmain
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void thêmLớpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmThemlop frm = new frmThemlop();
            frm.ShowDialog();
        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripContainer1_BottomToolStripPanel_Click(object sender, EventArgs e)
        {

        }
   /*     SqlConnection connect = null;
        string strconnect = " Server= HUUMANH; Database=QuanlyNV; User Id=SCDLdiemdanh; pwd=@123";
        private void OpenCSDL()
        {
            try
            {
                connect = new SqlConnection(strconnect);
                connect.Open();
                MessageBox.Show(" Ke noi thanh cong");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private void CloseCSDL()
        {
            if( connect!=null && connect.State == ConnectionState.Open)
            {
                connect.Close();
                MessageBox.Show(" Da dong CSDl thanh cong");
            }
        }
        private void HienThiToanBoSV()
        {

            SqlDataAdapter adapter = null;
            adapter = new SqlDataAdapter("Select * from SinhVien", connect);
            DataSet ds = new DataSet();
            SqlCommandBuilder builer = new SqlCommandBuilder(adapter);
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

        }*/
        private void Form1_Load(object sender, EventArgs e)
        {
          // OpenCSDL();
           // HienThiToanBoSV();
        }

        private void thêmSinhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmThemSinhVien frm = new frmThemSinhVien();
            frm.ShowDialog();
        }

        private void thoatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void điểmDanhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDiemDanh frm = new frmDiemDanh();
            frm.ShowDialog();
        }
    }
}
