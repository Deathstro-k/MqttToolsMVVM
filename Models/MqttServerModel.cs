using MQTTnet.Server;
using MQTTnet;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace MqttToolsMVVM.Models
{
    internal class MqttServerModel
    {
        private readonly static IMqttServer mqttServer = new MqttFactory().CreateMqttServer();
        private string _infoConnection;

        public static Action<MqttConnectionValidatorContext> onNewConnection;

        public static async Task StartMqttServer(string ipString, string portString)
        {
            var optionsBuilder = new MqttServerOptionsBuilder()
                   .WithDefaultEndpointBoundIPAddress(IPAddress.Parse(ipString))
                   .WithDefaultEndpointPort(int.Parse(portString));
                   
            try
            {
               await mqttServer.StartAsync(optionsBuilder.Build());
            }
            catch (InvalidOperationException) { }

        }
       
        private void GetConnectionInformation(MqttConnectionValidatorContext context)
        {
            _infoConnection = $"Подключючился пользователь:\n " +
                    $"ID: {context.ClientId}\n" +
                    $"UserName: {context.Username}\n" +
                    $"Password: {context.Password} \n" +
                    $"Endpoint: {context.Endpoint} \n" +
                    $"IsSecureConnection: {context.IsSecureConnection}\n" +
                    $"----------------------------------------------------------------------------";          

        }
        private void OnNewConnection(MqttConnectionValidatorContext context)
        {
            GetConnectionInformation(context);
            MessageBox.Show("Хуй");
        }

        public void OnEnable()
        {
            onNewConnection += OnNewConnection;            
        }
       
    

    public static async Task StopMqttServer()
        {
            await mqttServer.StopAsync();           

        }
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
