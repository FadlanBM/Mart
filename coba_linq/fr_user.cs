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
    public partial class fr_user : Form
    {
        LKSMartDataContext db;
        Image cusImage = null;
        public fr_user()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            new fr_main().Show();
            this.Close();
        }

        private void fr_user_Load(object sender, EventArgs e)
        {
            db = new LKSMartDataContext();
            Customer customer = Helper.Helper.Customer;
            string workingDirec = Environment.CurrentDirectory;
            string path=Directory.GetParent(workingDirec).Parent.Parent.FullName + @"\coba_linq\assets\profile_img\";

            txt_name.Text = customer.name;
            txt_address.Text = customer.address;
            DateTime date = Convert.ToDateTime(customer.date_of_birth);
            txt_birth.Text = date.ToString();
            txt_gender.Text = customer.gender;
            txt_phone.Text = customer.phone_number;

            cusImage=Image.FromFile(path + "profile.jpg");
            string imagepath = path + customer.profile_image_name;
            if (customer.profile_image_name!=null)
            {
                cusImage = Image.FromFile(imagepath);
            }
            pb_profile.Invoke((MethodInvoker)delegate { pb_profile.Image = cusImage; });
            
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            new login().Show();
            this.Close (); 
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            new fr_edit_user().Show();
            cusImage.Dispose();
            this.Close ();
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
