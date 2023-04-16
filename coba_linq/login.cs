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
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (lb_username.Text.Trim() == "" || lb_password.Text.Trim() == "")
            {
                MessageBox.Show("form Harus di Isi Semua", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            int pin;
            string input = lb_password.Text.Trim();
            double valida = double.Parse(input);
            if (valida<100000000)
            {
                if (int.TryParse(input, out pin))
                {

                    LKSMartDataContext db = new LKSMartDataContext();
                    var customer = (from c in db.Customers
                                    where c.email == lb_username.Text ||
                                    c.phone_number == lb_username.Text
                                    select c).SingleOrDefault();
                    if (customer != null)
                    {
                        if (customer.pin_number == lb_password.Text)
                        {
                            Helper.Helper.Customer = customer;
                            new fr_main().Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Password Yang anda masukan salah", "Waring", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Username yang anda masukkan salah", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
                else
                {
                    MessageBox.Show("Pin harus Number", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            else {
                MessageBox.Show("Pin Yang Anda masukkan melebihi batas yang di perbolehkan", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        

        private void login_Load(object sender, EventArgs e)
        {

        }
    }
}
