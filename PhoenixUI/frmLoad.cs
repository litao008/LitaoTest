using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BinHong.PhoenixCore.LogIn;
using BinHong.PhoenixVM;
using C1.Win.C1FlexGrid;

namespace BinHong.PhoenixUI
{
    public partial class FrmLoad : Form
    {
        public FrmLoad()
        {
            InitializeComponent();

            BindingSource source = new BindingSource();
            source.DataSource = VmManager.LogInView.Devices;
            this.flgView.DataSource = source;

            flgView.Cols["IP"].Caption = "登录IP";
            flgView.Cols["Type"].Caption = "类型";
            flgView.Cols["LocalIp"].Caption = "本机IP";
            flgView.Cols["Port"].Caption = "端口";
            flgView.Cols["Status"].Caption = "状态";
            flgView.Cols["BelongTo"].Caption = "所属板卡";
            flgView.Cols["LoginReq"].Caption = "是否登录";

            //事件绑定
            this.Load += OnLoaded; //主界面加载
            this.btnLogin.Click += VmManager.LogInView.LogIn;//登陆
            this.btnDel.Click += OnDel;//删除配置
            this.btnLoad.Click += OnLoadDeviceConfig;//装载设备配置
            this.btnSave.Click += OnSaveDeviceConfig;//保存设备配置
        }

        /// <summary>
        /// 界面加载事件
        /// </summary>
        private void OnLoaded(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 删除选中的设备
        /// </summary>
        private void OnDel(object sender, EventArgs e)
        {
            RowCollection delDevices = this.flgView.Rows.Selected;
            foreach (Row delDevice in delDevices)
            {
                DeviceInfo info = delDevice.DataSource as DeviceInfo;
                VmManager.LogInView.Del(info);
            }
        }

        /// <summary>
        /// 装载设备配置
        /// </summary>
        private void OnLoadDeviceConfig(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog=new OpenFileDialog();
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = @"txt files(*.txt)|*.txt";
            DialogResult result=openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                VmManager.LogInView.LoadConfig(openFileDialog.FileName);
            }
        }

        /// <summary>
        /// 保存设备配置
        /// </summary>
        private void OnSaveDeviceConfig(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog=new SaveFileDialog();
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Filter = @"txt files(*.txt)|*.txt";
            DialogResult result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<DeviceInfo> devices=new List<DeviceInfo>();
                foreach (var row in VmManager.LogInView.Devices)
                {
                    devices.Add(row );
                }
                VmManager.LogInView.SaveConfig(saveFileDialog.FileName, devices);
            }
        }
    }
}
