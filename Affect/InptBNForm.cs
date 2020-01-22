using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Affect
{
    public partial class InptBNForm : Form
    {
        public string resultTxt { get; set; }
        public InptBNForm()
        {
            InitializeComponent();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                if (string.IsNullOrEmpty(this.textBox1.Text))
                {
                    MessageBox.Show("新不允许为空");
                    return;
                }
                else if (Main.lstBillNo.Contains(this.textBox1.Text))
                {
                    MessageBox.Show("新单号已存在");
                    return;
                }
                else if (this.textBox1.Text.IndexOfAny(new char[] { '@', '.', ',', '!' }) > -1)
                {
                    MessageBox.Show("包含特殊字符");
                    return;                  
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.resultTxt = this.textBox1.Text;
                    return;
                }
            }
        }

        private void InptBNForm_Load(object sender, EventArgs e)
        {

        }
    }
}
