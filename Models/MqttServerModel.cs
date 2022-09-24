using MQTTnet.Server;
using MQTTnet;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using MqttToolsMVVM.ViewModels;
using System.Collections.Generic;
using System.Windows.Threading;
using System.ServiceModel.Channels;

namespace MqttToolsMVVM.Models
{
    internal class MqttServerModel
    {
        private readonly static IMqttServer mqttServer = new MqttFactory().CreateMqttServer();
        private string _infoConnection;
        public MainWindowViewModel mainWindowView = new MainWindowViewModel();
        public static Action<MqttConnectionValidatorContext> onNewConnection;

        
        public async Task StartMqttServer(string ipString, string portString)
        {
           var optionsBuilder = new MqttServerOptionsBuilder()
                       .WithDefaultEndpointBoundIPAddress(IPAddress.Parse(ipString))
                       .WithDefaultEndpointPort(int.Parse(portString))
                       .WithConnectionValidator(onNewConnection += OnNewConnection);

            try
            {
               await mqttServer.StartAsync(optionsBuilder.Build());
            }
            catch (InvalidOperationException) { }

        }
      
        
        public string GetConnectionInformation(MqttConnectionValidatorContext context)
        {
            _infoConnection = $"Подключючился пользователь:\n " +
                    $"ID: {context.ClientId}\n" +
                    $"UserName: {context.Username}\n" +
                    $"Password: {context.Password} \n" +
                    $"Endpoint: {context.Endpoint} \n" +
                    $"IsSecureConnection: {context.IsSecureConnection}\n" +
                    $"----------------------------------------------------------------------------";
            return _infoConnection;
        }
        private void OnNewConnection(MqttConnectionValidatorContext context)
        {

           // MainWindowViewModel.Items.Add(new Item(GetConnectionInformation(context)));
       
            
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
    public class Item
    {
        public Item(string message)
        {
            
            Message = message;
        }
        public string Message { get; set; }
    }

    public class ItemHandler
    {
        public List<Item> Items { get; private set; }
        public ItemHandler()
        {
            Items = new List<Item>();
        }

        

        public void Add(Item item)
        {   
            
            Items.Add(item);          

        }


    }
    } 
