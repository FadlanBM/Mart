using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace coba_linq
{
    public partial class modal_cart : Form
    {
        public int productId { get; set; }

        public bool isBuy { get; set; }=false;
        public int Count { get; set; } = 1;
        Product product;
        public modal_cart()
        {
            InitializeComponent();
        }

        private void modal_cart_Load(object sender, EventArgs e)
        {
            LKSMartDataContext data = new LKSMartDataContext(); 
            string workingDirectory=Environment.CurrentDirectory;
            string path=Directory.GetParent(workingDirectory).Parent.FullName+@"\assets\product_img\";

            product = (from p in data.Products
                       where p.id == productId
                       select p).SingleOrDefault();
            if (product.image_name !=null)
            {
                string imagePath = path + product.image_name;
                if (File.Exists(imagePath))
                {
                    pb_product.Image = Image.FromFile(imagePath);
                }
                else {
                    pb_product.Image = Image.FromFile(path + "2.jpg");
                }

            }
            else
            {
                pb_product.Image=Image.FromFile(path+"2.jpg");
            }

            string desc = "Name :" + product.name + Environment.NewLine +
                "Price :" + product.price + Environment.NewLine +
                "Stock :" + product.stock;
            txt_desc.Text = desc;
            txt_count.Text=Count.ToString();
            txt_name.Text=product.name;
            txt_price.Text=product.price.ToString();
        }

        private void pb_product_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (Count >1)
            {
                Count--;
                txt_count.Text=Count.ToString();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (Count < product.stock)
            {
                Count++;
                txt_count.Text = Count.ToString();
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (isBuy)
            {
                Helper.Helper.editQtyCart(productId, Count);
            }
            else { 
                Helper.Helper.addCart(product.id, Count);
                MessageBox.Show("Product Telah Berhasil di tambahkan di Cart", "Tambah Ke keranjang", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            this.Close();
        }

        private void txt_desc_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
