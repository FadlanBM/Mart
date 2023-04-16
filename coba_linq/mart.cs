using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace coba_linq
{
    public partial class mart : Form
    {
        public mart()
        {
            InitializeComponent();
        }

        public void tampil() {
            string workingDirectory = Environment.CurrentDirectory;
            string path=Directory.GetParent(workingDirectory).Parent.FullName + @"\assets\product_img\";

            LKSMartDataContext data = new LKSMartDataContext();

            var products = from p in data.Products
                          where p.deleted_at == null && p.stock != 0
                          select new
                          {
                              Id = p.id,
                              ImageName = p.image_name,
                              Name = p.name,
                              Price = p.price,
                              Stock = p.stock,
                          };
            if (txt_search.Text!=null)
            {
                products = products.Where(e => e.Name.Contains(txt_search.Text));
            }
            if (txt_min.Text!="")
            {
                int min;
            if (int.TryParse(txt_min.Text, out min))
                {
                    products = products.Where(e => e.Price >= min);
                }
                else
                {
                    MessageBox.Show("Harga harus Angka", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_min.Text = "";
                }
            }
            if (txt_max.Text!="")
            {            
                int max;
            if (int.TryParse(txt_max.Text, out max))
                {
                    products = products.Where(e => e.Price <= max);
                }
                else
                {
                    MessageBox.Show("Harga Harus Angka", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_max.Text = "";
                }
            }
                dataGridView1.Rows.Clear();
                foreach (var product in products) { 
                    int index= dataGridView1.Rows.Add();
                    Image productImg = Image.FromFile(path + "2.jpg");
                    if (product.ImageName!=null)
                    {
                        if (File.Exists(path + product.ImageName))
                        {
                            productImg = Image.FromFile(path + product.ImageName);
                        }
                    }
                    dataGridView1.Rows[index].Cells[0].Value = product.Id;
                    dataGridView1.Rows[index].Cells[1].Value =productImg;
                    dataGridView1.Rows[index].Cells[2].Value =product.Name;
                    dataGridView1.Rows[index].Cells[3].Value =product.Price;
                    dataGridView1.Rows[index].Cells[4].Value =product.Stock;
                }
        }

        private void mart_Load(object sender, EventArgs e)
        {
            tampil();


        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
            new fr_main().Show();
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            tampil();
        }

        private void txt_min_TextChanged(object sender, EventArgs e)
        {
            tampil();
        }

        private void txt_max_TextChanged(object sender, EventArgs e)
        {
            tampil();
        }

        private void dg_shop_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex==5)
            {
                modal_cart f= new modal_cart();
                int id=(int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                f.productId=id;
                f.ShowDialog();
            }
        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
