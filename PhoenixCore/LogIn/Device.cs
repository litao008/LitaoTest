using System.ComponentModel;
using System.Net;

namespace BinHong.PhoenixCore.LogIn
{
    /// <summary>
    /// 设备集合
    /// </summary>
    public class DeviceCollection : BindingList<DeviceInfo>
    {
    }

    /// <summary>
    /// 设备的登陆信息
    /// </summary>
    public class DeviceInfo
    {
        private string _ip;
        private string _localIp;

        /// <summary>
        /// 要登录IP
        /// </summary>
        public string Ip
        {
            get { return _ip; }
            set
            {
                if (ParameterCheck.IsIpAddress(value))
                {
                    _ip = value;
                }
            }
        }

        /// <summary>
        /// 登录类型
        /// </summary>
        public LogInType Type { get; set; }

        /// <summary>
        /// 本机IP
        /// </summary>
        public string LocalIp
        {
            get { return _localIp; }
            set
            {
                if (ParameterCheck.IsIpAddress(value))
                {
                    _localIp = value;
                }
            }
        }

        /// <summary>
        /// 登录端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public LogInStatus Status { get; set; }

        /// <summary>
        /// 所属板卡
        /// </summary>
        public string BelongTo { get; set; }

        /// <summary>
        /// 登录确认
        /// </summary>
        public bool LoginReq { get; set; } 
    }

    /// <summary>
    /// 登录类型
    /// </summary>
    public enum LogInType
    {
        DSP,
        PPC
    }

    /// <summary>
    /// 登录状态
    /// </summary>
    public enum LogInStatus
    {
        Status1,
        Status2
    }
}
