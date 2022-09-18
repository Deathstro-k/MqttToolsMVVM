using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MqttToolsMVVM.Models
{
    internal class MqttServerModel
    {     

        public static string GetLocalIPAddressIPv4()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                return endPoint.Address.ToString();
            }
        }

        public static string GetPublicIpAddressIpv4()
        {
            string publicIp = new WebClient().DownloadString("https://api.ipify.org");
            return publicIp;
        }
    }
} 
