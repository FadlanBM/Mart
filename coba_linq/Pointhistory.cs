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
    public partial class Pointhistory : Form
    {
      
        public Pointhistory()
        {
            InitializeComponent();
        }

        private void history_Load(object sender, EventArgs e)
        {
            LKSMartDataContext db = new LKSMartDataContext();
            Customer customer =Helper.Helper.Customer;

            cr_point.Text=customer.point.ToString();

            var points = from p in db.PointHistories
                        join h in db.HeaderTransactions
                        on p.header_transaction_id equals h.id
                        where p.deleted_at == null
                        select new
                        {
                            Date = h.datetime,
                            PaymentCode = h.payment_code,
                            PointGained = p.point_gained,
                            PointBefore = p.point_before,
                            PointAfter = p.point_after,
                        };


            dg_history.RowHeadersVisible = false;
            foreach(var point in points){
                int index = dg_history.Rows.Add();

                dg_history.Rows[index].Cells[0].Value=point.Date;
                dg_history.Rows[index].Cells[1].Value = point.PaymentCode;
                dg_history.Rows[index].Cells[2].Value = point.PointGained;
                dg_history.Rows[index].Cells[3].Value = point.PointBefore;
                dg_history.Rows[index].Cells[4].Value = point.PointAfter;

                if (point.PointGained < 0)
                {
                    dg_history.Rows[index].Cells[2].Style.ForeColor = Color.Red;
                }
                else {
                    dg_history.Rows[index].Cells[2].Style.ForeColor= Color.Green;
                }

            }
        }

        private void dg_history_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            new fr_main().Show();
            this.Close();
        }
    }
}
