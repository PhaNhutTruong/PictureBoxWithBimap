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

namespace PictureBox
{
    public partial class Form1 : Form
    {
        RestoreEntities db = new RestoreEntities();
        public Form1()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            if (string.IsNullOrEmpty(Convert.ToString(pictureBox1.Image)) == false)
            {
                pictureBox1.Image.Dispose();
            }

            pictureBox1.Image = null;
            pictureBox1.Name = null;
            txtId.Text = null;
            txtName.Text = null;
        }

        public void FillData()
        {
            var list = db.PictureTests.ToList();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = list;
        }

        string UrlofImage;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files() | *.jpg; *.jpeg; *.bmp; ";
            if (open.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(Convert.ToString(pictureBox1.Image)) == false)
                {
                    pictureBox1.Image.Dispose();
                }
                pictureBox1.Image = null;
                pictureBox1.Name = null;
                pictureBox1.Image = new Bitmap(open.FileName);
                pictureBox1.Name = Path.GetFileName(open.FileName);
                UrlofImage = open.FileName;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PictureTest picture = new PictureTest();
            picture.name = txtName.Text;
            if (string.IsNullOrEmpty(Convert.ToString(pictureBox1.Name)) == false)
            {
                var list = db.PictureTests.Where(c => c.imagename == pictureBox1.Name).ToList();
                if (list.Count <= 0)
                {
                    File.Copy(UrlofImage, Path.Combine(Application.StartupPath + "\\picture\\", Convert.ToString(pictureBox1.Name)), true);
                    picture.imagename = pictureBox1.Name;
                }
            }
            db.PictureTests.Add(picture);
            db.SaveChanges();
            FillData();
            Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtId.Text);
            PictureTest picture = db.PictureTests.Where(c => c.id == id).First();
            string name = System.IO.Path.Combine(Application.StartupPath + "\\picture\\", picture.imagename);
            if (File.Exists(name))
            {
                if (string.IsNullOrEmpty(Convert.ToString(pictureBox1.Image)) == false)
                {
                    pictureBox1.Image.Dispose();
                }
                pictureBox1.Image = null;
                var list = db.PictureTests.Where(c => c.imagename == picture.imagename).ToList();
                if (list.Count >= 2)
                {

                }
                else
                {
                    File.Delete(name);
                }
            }
            db.PictureTests.Remove(picture);
            db.SaveChanges();
            FillData();
            Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtId.Text);
            PictureTest picture = db.PictureTests.Where(c => c.id == id).First();
            picture.name = txtName.Text;
            if (string.IsNullOrEmpty(Convert.ToString(pictureBox1.Name)) == false)
            {
                if (string.IsNullOrEmpty(Convert.ToString(picture.imagename)) == false)
                {
                    string name = System.IO.Path.Combine(Application.StartupPath + "\\picture\\", picture.imagename);
                    if (File.Exists(name))
                    {
                        var list = db.PictureTests.Where(c => c.imagename == picture.imagename).ToList();
                        if (list.Count >=2)
                        {
                            
                        }
                        else
                        {
                            File.Delete(name);
                        }
                    }
                }
                File.Copy(UrlofImage, Path.Combine(Application.StartupPath + "\\picture\\", Convert.ToString(pictureBox1.Name)), true);
                picture.imagename = pictureBox1.Name;
            }
            db.SaveChanges();
            FillData();
            Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FillData();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Clear();
            int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            PictureTest picture = db.PictureTests.Where(c => c.id == id).First();
            if (string.IsNullOrEmpty(Convert.ToString(picture.imagename)))
            {
                pictureBox1.Image = null;
            }
            else
            {
                if (File.Exists(Application.StartupPath + "\\picture\\" + picture.imagename))
                {
                    pictureBox1.Image = new Bitmap(Application.StartupPath + "\\picture\\" + picture.imagename);
                }
            }
            txtId.Text = picture.id.ToString();
            txtName.Text = picture.name;
        }
    }
}
