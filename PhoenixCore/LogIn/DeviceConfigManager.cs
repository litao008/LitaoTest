using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BinHong.PhoenixCore.LogIn
{
    public class DeviceConfigManager
    {
        public DeviceConfigManager()
        {
            
        }

        /// <summary>
        /// 把对象转换成String
        /// </summary>
        private string ObjectToString(DeviceInfo deviceLoginInfo)
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6}",
                deviceLoginInfo.Ip, deviceLoginInfo.Type, deviceLoginInfo.Port, deviceLoginInfo.LoginReq,
                deviceLoginInfo.Status, deviceLoginInfo.BelongTo, deviceLoginInfo.LocalIp);
        }

        /// <summary>
        /// 把String转换成Object
        /// </summary>
        /// <param name="deviceConfig">按一定格式排列的设备信息字符串</param>
        private DeviceInfo StringToObject(string deviceConfig)
        {
            string[] configs = deviceConfig.Split(',');
            if (configs.Length == 7)
            {
                try
                {
                    DeviceInfo deviceLoginInfo = new DeviceInfo();

                    if (ParameterCheck.IsIpAddress(configs[0]))
                    {
                        deviceLoginInfo.Ip = configs[0];
                    }

                    LogInType type;
                    if (Enum.TryParse(configs[1],true,out type))
                    {
                        deviceLoginInfo.Type = type;
                    }

                    int port;
                    if (int.TryParse(configs[2], out port))
                    {
                        deviceLoginInfo.Port = port;
                    }

                    deviceLoginInfo.LoginReq = configs[3].Equals("true",StringComparison.CurrentCultureIgnoreCase);
                    LogInStatus status;
                    if (Enum.TryParse(configs[4], true, out status))
                    {
                        deviceLoginInfo.Status = status;
                    }

                    deviceLoginInfo.BelongTo = configs[5];

                    if (ParameterCheck.IsIpAddress(configs[6]))
                    {
                        deviceLoginInfo.LocalIp = configs[6];
                    } 
                    return deviceLoginInfo;
                }
                catch (Exception)
                {
                    //todo log
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// 装载配置
        /// </summary>
        public List<DeviceInfo> LoadConfig(string configPath)
        {
            if (!File.Exists(configPath))
            {
                return null;
            }

            string[] lines = File.ReadAllLines(configPath);
            List<DeviceInfo> devices = lines.Select(line => StringToObject(line)).ToList();
            return devices;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void SaveConfig(List<DeviceInfo> devices,string configPath)
        {
            List<string> configs= devices.Select(device => ObjectToString(device)).ToList();
            File.WriteAllLines(configPath,configs);
        }
    }
}
