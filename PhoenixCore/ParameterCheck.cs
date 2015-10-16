using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BinHong.PhoenixCore.LogIn;

namespace BinHong.PhoenixCore
{
    public class ParameterCheck
    {
        public static bool IsIpAddress(string ip)
        {
            IPAddress ipAddress;
            if (IPAddress.TryParse(ip, out ipAddress))
            {
                return true;
            }
            return false;
        }
    }
}
