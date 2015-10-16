using System;
using System.Windows.Forms;
using BinHong.PhoenixVM;

namespace BinHong.PhoenixUI
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();

            this.cmdLogin.Click += ShowLogInUi;
            this.menuCoreLogin.Click += ShowLogInUi; //todo 可以把menuCoreLogin的类型修改成C1的类型，就不用这一行了。
        }

        /// <summary>
        /// 显示登陆界面
        /// </summary>
        public void ShowLogInUi(object o, EventArgs e)
        {
            var frmLoad=new FrmLoad();
            frmLoad.ShowDialog();
        }

        #region Log窗口控制
        private void cmdStartRecLogs_Click(object sender, C1.Win.C1Command.ClickEventArgs e)
        {
            cmdStartRecLogs.Enabled = false;
            cmdStopRecLog.Enabled = true;
        }

        private void cmdStopRecLog_Click(object sender, C1.Win.C1Command.ClickEventArgs e)
        {
            cmdStartRecLogs.Enabled = true;
            cmdStopRecLog.Enabled = false;
        }
        #endregion

    }
}
