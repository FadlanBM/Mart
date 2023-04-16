using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace coba_linq
{
    public partial class fr_edit_user : Form
    {
        LKSMartDataContext db;
        string imagePath;
        Image cusImage = null;
        public fr_edit_user()
        {
            InitializeComponent();
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            new login().Show();
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (lb_Name.Text.Trim()=="")
            {
                warning_name.Visible = true;
                warning_name.Text = "Field Waajib Di isi";
                return;
            }

            Customer CurrentCustomer = Helper.Helper.Customer;
           DialogResult result = MessageBox.Show("Apakah anda yakin akan Melakukan perubahan?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result==DialogResult.OK)
            {
                var customer = (from c in db.Customers where c.id == CurrentCustomer.id select c).SingleOrDefault();

                customer.name = lb_Name.Text;

                db.SubmitChanges();
                Helper.Helper.Customer = customer;
                MessageBox.Show("Berhasil Merubah Data", "Update Profile", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void fr_edit_user_Load(object sender, EventArgs e)
        {
            db = new LKSMartDataContext();
            Customer customer = Helper.Helper.Customer;
            string workingDirectory = Environment.CurrentDirectory;
            string path = Directory.GetParent(workingDirectory).Parent.Parent.FullName + @"\coba_linq\assets\profile_img\";

            lb_Name.Text = customer.name;
            lb_email.Text= customer.email;
            lb_pin.Text = customer.pin_number;
            lb_phone.Text = customer.phone_number;
            DateTime dateTime = Convert.ToDateTime(customer.date_of_birth);
            lb_birth.Value=dateTime;
            lb_address.Text= customer.address;
            if (customer.gender=="Laki Laki")
            {
                rd_laki.Checked = true;
            }
            if (customer.gender=="Perempuan")
            {
                rd_perempuan.Checked = true;
            }

            cusImage = Image.FromFile(path + "profile.jpg");
            if (customer.profile_image_name !=null)
            {
                string imagePath = path + customer.profile_image_name;
                if (File.Exists(imagePath))
                {
                    cusImage = Image.FromFile(imagePath);
                }
            }
            px_img.Invoke((MethodInvoker)delegate { px_img.Image = cusImage; });
        }

        private void btn_edit_pin_Click(object sender, EventArgs e)
        {
            db = new LKSMartDataContext();
            Customer customer1 = Helper.Helper.Customer;
            string workingCustomer = Environment.CurrentDirectory;
            if (lb_pin.Text==customer1.pin_number)
            {
                MessageBox.Show("Pin Tidak Boleh sama", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (lb_pin.Text.Trim()=="") { 
                lb_pin.Visible = true;
                lb_pin.Text = "Fieled Wajib Di Isi";
                return;
            }
            DialogResult result =MessageBox.Show("Apakah Anda yakin Mau Merubah pin", "Informasion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result==DialogResult.Yes)
            {
                var customerid = Helper.Helper.Customer;

                var customer = (from c in db.Customers where c.id == customerid.id select c).SingleOrDefault();

                customer.pin_number = lb_pin.Text;

                db.SubmitChanges();
                Helper.Helper.Customer = customer;

                MessageBox.Show("Berhasil Merubah Data", "Merubah Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("Anda Harus Login Ulang", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
               new login().Show();
                this.Close();
            }
            }
        }

        private void btn_edit_birth_Click(object sender, EventArgs e)
        {

            db =new LKSMartDataContext();
            Customer customer=Helper.Helper.Customer;
            string workingDirectory = Environment.CurrentDirectory;

            DateTime dateTime = Convert.ToDateTime(customer.date_of_birth);
            string mydate = dateTime.ToString();
            if (mydate=="")
            {
                warning_birth.Visible = true;
                warning_birth.Text = "Field Wajib Di Isi";
                return;
            }
            DialogResult result = MessageBox.Show("Apakah Anda yakin Mau mengubah Taggal Lahir", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result==DialogResult.OK)
            {
                var customerid1 = Helper.Helper.Customer;

                var customer1 = (from c in db.Customers where c.id == customerid1.id select c).SingleOrDefault();

                customer1.date_of_birth = lb_birth.Value;
                db.SubmitChanges();
                Helper.Helper.Customer = customer;
                MessageBox.Show("Berhasil merubah data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void btn_edit_address_Click(object sender, EventArgs e)
        {
            var customerid2= Helper.Helper.Customer;
            var customer2 = (from c in db.Customers where c.id == customerid2.id select c).SingleOrDefault();
            DialogResult result= MessageBox.Show("Apakah Anda yakin Akan merubah Alamat?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result==DialogResult.OK)
            {
                customer2.address = lb_address.Text;
                db.SubmitChanges();
                MessageBox.Show("Berhasil merubah data", "information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void btn_edit_gender_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yakin akan merubah data Gender ?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result==DialogResult.OK)
            {
                var customerid3 = Helper.Helper.Customer;

            var customer3 = (from c in db.Customers where c.id == customerid3.id select c).SingleOrDefault();
            if (rd_laki.Checked)
            {
                string rblaki = rd_laki.Text;
                customer3.gender = rblaki;
            }
            if (rd_perempuan.Checked)
            {
                string rbperempuan = rd_perempuan.Text;
                customer3.gender = rbperempuan;
            }
            db.SubmitChanges();
            MessageBox.Show("Berhasil Merubah Data", "Ïnformation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void px_img_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Files |*.BMP;*.JPG;*.JPEG;*.PNG";

            if (openFileDialog.ShowDialog()==DialogResult.OK)
            {
                imagePath= openFileDialog.FileName; 
                px_img.Image=Image.FromFile(openFileDialog.FileName); 
            }
        }

        private void btn_upload_img_Click(object sender, EventArgs e)
        {
            Customer customerid = Helper.Helper.Customer;
            string workingDirectory = Environment.CurrentDirectory;
            string path=Directory.GetParent(workingDirectory).Parent.Parent.FullName+ @"\coba_linq\assets\profile_img\";

            var custumer = (from c in db.Customers where c.id == customerid.id select c).SingleOrDefault();

            cusImage.Dispose();
            try
            {
                if (File.Exists(path + customerid.profile_image_name)||customerid.profile_image_name==null)
                {
                    if (File.Exists(imagePath))
                    {
                        if (customerid.profile_image_name != null)
                        {  
                            File.Delete(path + customerid.profile_image_name);
                        }
                        File.Copy(imagePath, path + customerid.id + " .bmp;.jpg;.jpeg;.png");

                        custumer.profile_image_name=customerid.id+ " .bmp;.jpg;.jpeg;.png";
                        db.SubmitChanges();
                        Helper.Helper.Customer = custumer;
                        MessageBox.Show("Berhasil Mengupdate Image", "Image Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
            catch(Exception ex) {
                MessageBox.Show(ex.ToString());
                MessageBox.Show("Image Failed To Update", "Update Profile", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
      }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            cusImage.Dispose();
            this.Close();
            new fr_user().Show();
        }
    }
}
