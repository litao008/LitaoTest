using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using BinHong.PhoenixCore.Connect;

namespace BinHong.PhoenixCore.LogIn
{
    /// <summary>
    /// 设备登陆
    /// </summary>
    /// <remarks>
    /// 实现添加删除设备以及登陆这些设备的功能
    /// </remarks>
    public class DeviceLogIn
    {
        private readonly DeviceCollection _deviceCollection;

        /// <summary>
        /// 初始化设备集合
        /// </summary>
        public DeviceLogIn(DeviceCollection deviceCollection)
        {
            _deviceCollection = deviceCollection;
        }

        /// <summary>
        /// 初始化UDP连接
        /// </summary>
        private void InitializeUdp()
        {
            GlobalObjects.UdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            GlobalObjects.UdpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, GlobalObjects.UdpRecvBufferSize);
            IPEndPoint ipepSend = new IPEndPoint(IPAddress.Any, 200);//Server Udp Port
            GlobalObjects.UdpSocket.Bind(ipepSend);
            GlobalObjects.UdpSocket.ReceiveBufferSize = 1024 * 1024 * 1000;
            GlobalObjects.UdpSocket.ReceiveTimeout = 10000;
            GlobalObjects.UdpSocket.SendTimeout = 10000;
        }

        /// <summary>
        /// 尝试登陆某一个设备
        /// </summary>
        /// <returns>是否登陆成功</returns>
        private bool LoginDevice(DeviceInfo loginInfo)
        {
            //某个设备可能回登陆失败所以要try。
            //todo
            try
            {
            }
            catch (Exception)
            {
                
                throw;
            }
            return true;
        }

        /// <summary>
        /// 登陆
        /// </summary>
        public void LogIn()
        {
            foreach (var device in _deviceCollection)
            {
                LoginDevice(device);
            }
        }

        /// <summary>
        /// 测试某一个设备是否可以登陆
        /// </summary>
        /// <param name="loginInfo">要测试的设备</param>
        public void TestLogIn(DeviceInfo loginInfo)
        {
            //todo
        }
    }

    
}
