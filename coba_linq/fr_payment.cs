using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace coba_linq
{
    public partial class fr_payment : Form
    {
        string PaymentCode;
        public string PaymentTypeName { get; set; }
        public int PaymentTypeId { get; set; }
        public decimal Subtotal { get; set; }
        public int PointUsed { get; set; }
        public int PointGained { get; set; }
        public fr_payment()
        {
            InitializeComponent();
        }

        private void tb_payment_TextChanged(object sender, EventArgs e)
        {

        }

        private void fr_payment_Load(object sender, EventArgs e)
        {
            string[] key ={
                "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
                "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
                "1","2","3","4","5","6","7","8","9","0"
            };

            PaymentCode = "";
            Random r = new Random();
            int index;
            for (int i=0;i<12;i++) { 
                index = r.Next(0,key.Length);
                PaymentCode = PaymentCode+ key[index];
            }
            tb_payment.Text = "Pesanan Kamu berhasil di di Submit" + Environment.NewLine +
                             "Tekan Continue untuk menyelsaikan pemesanan" + Environment.NewLine +
                             PaymentTypeName + "Application." + Environment.NewLine + Environment.NewLine +
                             "Silahkan masukkan " + Environment.NewLine + PaymentCode;
            tb_payment.TextAlign=HorizontalAlignment.Center;
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LKSMartDataContext db= new LKSMartDataContext();
            Customer customer = Helper.Helper.Customer;

            HeaderTransaction header =new HeaderTransaction();
            header.customer_id = customer.id;
            header.payment_type_id = PaymentTypeId; 
            header.datetime=DateTime.Now;
            header.sub_total = Subtotal;
            header.point_used=PointUsed;
            header.payment_code= PaymentCode;
            header.created_at=DateTime.Now;

            db.HeaderTransactions.InsertOnSubmit(header);
            db.SubmitChanges();

            Dictionary<int, int> cart = Helper.Helper.Cart;
            foreach (var item in cart) {
                var product = (from p in db.Products
                               where p.id==item.Key
                               select p).SingleOrDefault();
                DetailTransaction detail = new DetailTransaction();
                detail.header_transaction_id = header.id;
                detail.product_id = item.Key;
                detail.price = product.price;
                detail.quantity=item.Value;
                detail.created_at= DateTime.Now;

                product.stock -= item.Value;
                 db.DetailTransactions.InsertOnSubmit(detail); 
                db.SubmitChanges();
            }

            int pointAfter = 0;
            if (PointGained < 0)
            {
                pointAfter = customer.point - PointUsed;
            }
            else { 
                pointAfter = customer.point-PointUsed+PointGained;
            }
            PointHistory point= new PointHistory();
            point.customer_id = customer.id;
            point.header_transaction_id= header.id;
            point.point_deducted= PointUsed;
            point.point_gained=PointGained;
            point.point_before = customer.point;
            point.point_after = pointAfter;
            point.created_at=DateTime.Now;

            db.PointHistories.InsertOnSubmit(point);
            db.SubmitChanges();

            var currentCustumer = (from c in db.Customers
                                   where c.id == customer.id
                                   select c).SingleOrDefault();
            currentCustumer.point = pointAfter;
            Helper.Helper.Customer = currentCustumer;
            db.SubmitChanges();
            Helper.Helper.Cart.Clear();
            MessageBox.Show("Success Checkout,", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            new fr_main().Show();
            this.Hide();
        }
    }
}
