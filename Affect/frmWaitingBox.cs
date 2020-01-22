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
    public partial class frmWaitingBox : Form
    {
        private frmWaitingBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体正在执行的异步动作
        /// </summary>
        private readonly Task _task;

        /// <summary>
        /// 保存窗体的构造函数
        /// </summary>
        /// <param name="task">执行的动作</param>
        public frmWaitingBox(Task task)
            : this()
        {
            _task = task;
        }

        /// <summary>
        /// 加载窗体触发
        /// </summary>
        private void frmWaitingBox_Load(object sender, EventArgs e)
        {
            _task.ContinueWith(task =>
            {
                
                if (_task.Exception != null)
                {
                    MessageBox.Show(@"程序执行异常！具体原因是：{_task.Exception.Message}" + _task.Exception);
                }
                BeginInvoke((Action)(Close));
                this.DialogResult = DialogResult.OK;
                return ;
            });
            if (_task.IsCompleted || _task.IsFaulted || _task.IsCanceled)
            {
                if (_task.Exception != null)
                {
                    MessageBox.Show(@"程序执行异常！具体原因是：{_task.Exception.Message}");
                }
                BeginInvoke((Action)(Close));
                this.DialogResult = DialogResult.OK;
                return;
            }
        }

        //解决界面闪烁
        protected override CreateParams CreateParams
        {

            get
            {

                CreateParams cp = base.CreateParams;

                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED

                return cp;

            }

        }

    }
}
