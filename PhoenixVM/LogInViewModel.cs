using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using BinHong.PhoenixCore;
using BinHong.PhoenixCore.LogIn;

namespace BinHong.PhoenixVM
{
    /// <summary>
    /// 设备登陆
    /// </summary>
    /// <remarks>
    /// 实现添加删除设备以及登陆这些设备的功能
    /// </remarks>
    public class LogInViewModel
    {
        private readonly DeviceCollection _deviceCollection;
        private readonly DeviceLogIn _deviceLogIn;
        private readonly DeviceConfigManager _deviceConfigManager;

        /// <summary>
        /// 设备列表
        /// </summary>
        public BindingList<DeviceInfo> Devices
        {
            get { return _deviceCollection; }
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        public LogInViewModel()
        {
            //在这里控制Core中类的构造顺序
            _deviceCollection=new DeviceCollection();
            _deviceLogIn = new DeviceLogIn(_deviceCollection);
            _deviceConfigManager=new DeviceConfigManager();


            //todo 用于测试，要删除。
            Devices.Add(new DeviceInfo { Type =  LogInType.DSP});
            Devices.Add(new DeviceInfo { Type = LogInType.DSP, Status = LogInStatus.Status1 });
            Devices.Add(new DeviceInfo { Type = LogInType.DSP,Status = LogInStatus.Status2});
        }


        /// <summary>
        /// 登陆
        /// </summary>
        public void LogIn(object o,EventArgs e)
        {
             _deviceLogIn.LogIn();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void Del(DeviceInfo info)
        {
            Devices.Remove(info);
        }

        /// <summary>
        /// 装载配置
        /// </summary>
        public void LoadConfig(string path)
        {
            List<DeviceInfo> devices=_deviceConfigManager.LoadConfig(path);
            _deviceCollection.Clear();
            foreach (var device in devices)
            {
                _deviceCollection.Add(device);
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void SaveConfig(string path,List<DeviceInfo> devices)
        {
            _deviceConfigManager.SaveConfig(devices, path);
        }
    }
}
