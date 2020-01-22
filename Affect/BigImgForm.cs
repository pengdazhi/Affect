using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Affect
{
    public partial class BigImgForm : Form
    {

        MouseEventArgs p0;
        Main main;
        private int Index;
        private string GroupName;
        public BigImgForm(Main mai)
        {
            main = mai;
            InitializeComponent();
            this.MouseWheel += new MouseEventHandler(CandidateForm_MouseWheel);
        }

        private void BigImgForm_Load(object sender, EventArgs e)
        {

        }

        public void ShowImg(int idx, string name, string text, string url)
        {
            Index = idx;
            GroupName = name;
            this.Text = GroupName + text;
            FileStream fileStream = new FileStream(url, FileMode.Open, FileAccess.Read);
            int byteLength = (int)fileStream.Length;
            byte[] fileBytes = new byte[byteLength];
            fileStream.Read(fileBytes, 0, byteLength);
            //文件流关闭,文件解除锁定
            fileStream.Close();
            fileStream.Dispose();
            this.pictureBox1.Image = Image.FromStream(new MemoryStream(fileBytes));
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ++Index;
            string text = main.GetText(GroupName, Index);
            string url = main.GetUrl(GroupName, Index);
            if (text != "error" && url != "error")
            {
                this.Text = GroupName + text;
                FileStream fileStream = new FileStream(url, FileMode.Open, FileAccess.Read);
                int byteLength = (int)fileStream.Length;
                byte[] fileBytes = new byte[byteLength];
                fileStream.Read(fileBytes, 0, byteLength);
                //文件流关闭,文件解除锁定
                fileStream.Close();
                fileStream.Dispose();
                this.pictureBox1.Image = Image.FromStream(new MemoryStream(fileBytes));
            }
            else
            {
                --Index;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            --Index;
            string text = main.GetText(GroupName, Index);
            string url = main.GetUrl(GroupName, Index);
            if (text != "error" && url != "error")
            {
                this.Text = GroupName + text;
                FileStream fileStream = new FileStream(url, FileMode.Open, FileAccess.Read);
                int byteLength = (int)fileStream.Length;
                byte[] fileBytes = new byte[byteLength];
                fileStream.Read(fileBytes, 0, byteLength);
                //文件流关闭,文件解除锁定
                fileStream.Close();
                fileStream.Dispose();
                this.pictureBox1.Image = Image.FromStream(new MemoryStream(fileBytes));
            }
            else
            {
                ++Index;
            }
        }

        /// <summary>
        /// 鼠标滚动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CandidateForm_MouseWheel(object sender, MouseEventArgs e)
        {
            this.pictureBox1.Dock = DockStyle.None;
            this.pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            Size size = this.pictureBox1.Size;
            size.Width += e.Delta;
            if (size.Width > pictureBox1.Image.Width)
            {
                pictureBox1.Width = pictureBox1.Image.Width;
                pictureBox1.Height = pictureBox1.Image.Height;
            }
            else if (size.Width * pictureBox1.Image.Height / pictureBox1.Image.Width < pictureBox1.Parent.Height - 200)
            {
                return;
            }
            else
            {
                pictureBox1.Width = size.Width;
                pictureBox1.Height = size.Width * pictureBox1.Image.Height / pictureBox1.Image.Width;
            }
            pictureBox1.Left = (pictureBox1.Parent.Width - pictureBox1.Width) / 2;
            pictureBox1.Top = (pictureBox1.Parent.Height - pictureBox1.Height) / 2;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {


        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            p0 = e;


        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox1.Location =
                              new Point(pictureBox1.Left + e.X - p0.X, pictureBox1.Top + e.Y - p0.Y);
        }

    }
}
