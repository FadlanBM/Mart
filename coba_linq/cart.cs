using System;
using System.Collections;
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
    public partial class cart : Form
    {
        LKSMartDataContext db;
        Dictionary<int, string> paymentList = new Dictionary<int, string>();
        public int newQty { get; set; }
        public cart()
        {
            InitializeComponent();
        }

        private void cart_Load(object sender, EventArgs e)
        {
            db=new LKSMartDataContext();
            loaddata();

           dg_cart.RowHeadersVisible = false;

            var payments = from p in db.PaymentTypes where p.deleted_at == null select p;

            foreach (var payment in payments) {
                paymentList.Add(payment.id, payment.name);
                cb_payment.Items.Add(payment.name);
            }
            cb_payment.SelectedIndex = 0;
            calculateTotal();
        }
        private void loaddata() { 
            string workingDirectory= Environment.CurrentDirectory;
            string path=Directory.GetParent(workingDirectory).Parent.Parent.FullName + @"\coba_linq\assets\product_img\";

            Dictionary<int, int> cart = Helper.Helper.Cart;

            dg_cart.Rows.Clear();
            foreach (var item in cart) { 
                int index=dg_cart.Rows.Add();
                var product = (from p in db.Products where p.id == item.Key select p).SingleOrDefault();

                Image productImage = Image.FromFile(path + "2.jpg");

                if (product.image_name !=null)
                {
                    if (File.Exists(path+product.image_name))
                    {
                        productImage = Image.FromFile(path + product.image_name);
                    }
                }
                dg_cart.Rows[index].Cells[0].Value =productImage;
                dg_cart.Rows[index].Cells[1].Value=product.id;
                dg_cart.Rows[index].Cells[2].Value =product.name;
                dg_cart.Rows[index].Cells[3].Value =product.price;
                dg_cart.Rows[index].Cells[4].Value =item.Value;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void calculateTotal() {
            Customer currentCustemer = Helper.Helper.Customer;
            decimal subTotal = 0;
            Dictionary<int,int>cart=Helper.Helper.Cart;

            foreach (var item in cart) {
                var product = (from p in db.Products where p.id == item.Key select p).SingleOrDefault();
                subTotal += product.price * item.Value;
            }

            lb_subtotal.Text =subTotal.ToString();
            lb_fee.Text=(subTotal*5/100).ToString();
            lb_total.Text = (Convert.ToDecimal(lb_subtotal.Text) + Convert.ToDecimal(lb_fee.Text)).ToString();
            lb_total2.Text = (Convert.ToDecimal(lb_subtotal.Text) + Convert.ToDecimal(lb_fee.Text)).ToString();
            if (lb_total2.Text=="0")
            {
                cek_point.Enabled = false;
            }
            if (currentCustemer.point.ToString() == "0")
            {
                cek_point.Enabled = false;
                lb_point.Text = "0";
            }
            if (cek_point.Checked)
            {
                lb_point.Text = currentCustemer.point.ToString();
            }
            else {
                lb_point.Text = "0";
            }
            lb_pay.Text = (Convert.ToDecimal(lb_total.Text) - Convert.ToDecimal(lb_point.Text)).ToString();
            decimal sta = Convert.ToDecimal(lb_pay.Text);
            if (sta<0)
            {
                lb_pay.Text = "0";
            }
        }
        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex==6)
            {
                if (MessageBox.Show("Apakah Anda Ingin Menghapus Data ini?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) 
                {
                    string id = dg_cart.Rows[e.RowIndex].Cells[1].Value.ToString();
                    Helper.Helper.removeCart(int.Parse(id));
                    loaddata();
                    calculateTotal();
                }
            }

            if (e.ColumnIndex==5)
            {
                string id = dg_cart.Rows[e.RowIndex].Cells[1].Value.ToString();
                string qty = dg_cart.Rows[e.RowIndex].Cells[4].Value.ToString();
                modal_cart f=new modal_cart();
                f.isBuy = true;
                f.productId=int.Parse(id);
                f.Count = int.Parse(qty);
                f.ShowDialog();
                loaddata();
                calculateTotal();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<int, int> cart = Helper.Helper.Cart;

            if (cart.Count==0)
            {
                if (MessageBox.Show("Card Stil Empty."+Environment.NewLine+"Are you Want to Add Product?","Confirmation",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    new mart().Show();
                    this.Hide();
                }
                return;
            }
            fr_payment f =new fr_payment();
            f.PaymentTypeName=cb_payment.Text;
            f.PaymentTypeId = paymentList.FirstOrDefault(x => x.Value == cb_payment.Text).Key;
            f.Subtotal = Convert.ToDecimal(lb_subtotal.Text);
            f.PointUsed = Convert.ToInt32(lb_point.Text);
            if (cek_point.Checked)
            {
                string point = "-" + lb_point.Text;
                f.PointGained = Convert.ToInt32(point);
            }
            else {
                decimal data = Convert.ToDecimal(lb_subtotal.Text);
                f.PointGained = Decimal.ToInt32(data) * 20 / 100;
            }
            f.Show();
            this.Hide();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            new fr_main().Show();
            this.Close();
        }

        private void cek_point_CheckedChanged(object sender, EventArgs e)
        {
            calculateTotal();
        }
    }
}
