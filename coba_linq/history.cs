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
    public partial class history : Form
    {
        LKSMartDataContext db;
        public history()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new fr_main().Show();
            this.Close();
        }

        private void history_Load(object sender, EventArgs e)
        {
            db = new LKSMartDataContext();
            Customer customer = Helper.Helper.Customer;

            var transsaction = from h in db.HeaderTransactions
                               join d in db.DetailTransactions
                               on h.id equals d.header_transaction_id
                               join p in db.PointHistories
                               on h.id equals p.header_transaction_id
                               where h.deleted_at == null && h.customer_id == customer.id
                               select new
                               {
                                   Id = h.id,
                                   Date = h.datetime,
                                   TotalPayment = h.sub_total,
                                   PointGained = p.point_gained,
                                   PointDeducated = p.point_deducted,
                                   PaymentCode = h.payment_code
                               };
            transsaction = transsaction.Distinct();
            dg_detail.RowHeadersVisible = false;
            dg_transaksi.RowHeadersVisible = false;

            foreach (var transactions in transsaction) {
                int index = dg_transaksi.Rows.Add();

                dg_transaksi.Rows[index].Cells[0].Value= transactions.Id;
                dg_transaksi.Rows[index].Cells[1].Value= transactions.Date;
                dg_transaksi.Rows[index].Cells[2].Value= transactions.TotalPayment;

                if (transactions.PointGained != 0)
                {
                    dg_transaksi.Rows[index].Cells[3].Value= transactions.PointDeducated;
                }
                else
                {
                    dg_transaksi.Rows[index].Cells[4].Value= transactions.PointGained;
                }
                dg_transaksi.Rows[index].Cells[4].Value=transactions.PaymentCode;
            }
        }

        private void dg_transaksi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string workingDirectory = Environment.CurrentDirectory;
            string path=Directory.GetParent(workingDirectory).Parent.Parent.FullName + @"\coba_linq\assets\product_img\";

            string id = dg_transaksi.Rows[e.RowIndex].Cells[0].Value.ToString();
            var transaction = from p in db.Products
                              join d in db.DetailTransactions
                              on p.id equals d.product_id
                              where d.header_transaction_id == Convert.ToInt32(id)
                              select new
                              {
                                  ProductImg=p.image_name,
                                  Name=p.name,
                                  Price=p.price,
                                  Qty=d.quantity
                              };
           dg_detail.Rows.Clear();
            foreach (var item in transaction)
            {
                int index = dg_detail.Rows.Add();

                Image ProduImage = Image.FromFile(path+"2.jpg");
                if (item.ProductImg!=null)
                {
                    if (File.Exists(path+item.ProductImg))
                    {
                        ProduImage = Image.FromFile(path + item.ProductImg);
                    }
                }
                dg_detail.Rows[index].Cells[0].Value= ProduImage;
                dg_detail.Rows[index].Cells[1].Value = item.Name;
                dg_detail.Rows[index].Cells[2].Value = item.Price;
                dg_detail.Rows[index].Cells[3].Value = item.Qty;
                 
            }
        }

        private void dg_detail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
