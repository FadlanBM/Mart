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
    public partial class fr_main : Form
    {
        LKSMartDataContext db;
        Image cusImage = null;
        public fr_main()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            new fr_user().Show();
            this.Close();
        }

        private void fr_main_Load(object sender, EventArgs e)
        {
            db = new LKSMartDataContext();
            Customer customer = Helper.Helper.Customer;
            string workingDirec = Environment.CurrentDirectory;
            string path = Directory.GetParent(workingDirec).Parent.Parent.FullName + @"\coba_linq\assets\profile_img\";

            txt_profile.Text = customer.name;

            cusImage = Image.FromFile(path + "profile.jpg");
            string imagepath = path + customer.profile_image_name;
            if (customer.profile_image_name != null)
            {
                cusImage = Image.FromFile(imagepath);
            }
            img_prof.Invoke((MethodInvoker)delegate { img_prof.Image = cusImage; });
            time.Text = DateTime.Now.ToString("HH:mm:ss"); 
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(loadtime);
            timer.Start();
        }

        public void loadtime(object sender, EventArgs e) {
            time.Text = DateTime.Now.ToString("HH:mm:ss"); 

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            new mart().Show();
            this.Close();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            new history().Show();
            this.Close();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            new Pointhistory().Show();
            this.Close();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            new cart().Show();
            this.Close();
        }
    }
}
