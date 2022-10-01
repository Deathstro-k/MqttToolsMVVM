﻿using MQTTnet.Server;
using MQTTnet;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using MqttToolsMVVM.ViewModels;
using System.Collections.ObjectModel;
using System.Text;


namespace MqttToolsMVVM.Models
{
    internal class MqttServerModel
    {
       
        private string _infoConnection;
        private string _infoMessage;   
        public Action<MqttConnectionValidatorContext> onNewConnection;
        public Action<MqttApplicationMessageInterceptorContext> onNewMessage;
        public MqttServerOptionsBuilder optionsBuilder;
        public static IMqttServer mqttServer = new MqttFactory().CreateMqttServer();
        public MqttServerModel(string ip, string port, bool useConnectionHandler, bool useMessageHandler)
        {
            //mqttServer = new MqttFactory().CreateMqttServer();
            onNewConnection -= OnNewConnection;
            onNewMessage -= OnNewMessage;
            if (useConnectionHandler && useMessageHandler)
            {
                optionsBuilder = new MqttServerOptionsBuilder()
                                  .WithDefaultEndpointBoundIPAddress(IPAddress.Parse(ip))
                                  .WithDefaultEndpointPort(int.Parse(port))
                                  .WithConnectionValidator(onNewConnection += OnNewConnection)
                                  .WithApplicationMessageInterceptor(onNewMessage += OnNewMessage);
            }
            else if (useConnectionHandler && !useMessageHandler)
            {
                optionsBuilder = new MqttServerOptionsBuilder()
                                  .WithDefaultEndpointBoundIPAddress(IPAddress.Parse(ip))
                                  .WithDefaultEndpointPort(int.Parse(port))
                                  .WithConnectionValidator(onNewConnection += OnNewConnection);
            }
            else if (!useConnectionHandler && useMessageHandler)
            {
                optionsBuilder = new MqttServerOptionsBuilder()
                                  .WithDefaultEndpointBoundIPAddress(IPAddress.Parse(ip))
                                  .WithDefaultEndpointPort(int.Parse(port))
                                  .WithApplicationMessageInterceptor(onNewMessage += OnNewMessage);
            }
            else
            {
                optionsBuilder = new MqttServerOptionsBuilder()
                                 .WithDefaultEndpointBoundIPAddress(IPAddress.Parse(ip))
                                 .WithDefaultEndpointPort(int.Parse(port));
            }
        }

        public void GetConnectionInformation(MqttConnectionValidatorContext context)
        {
            _infoConnection = $"Подключился пользователь:\n " +
                    $"ID: {context.ClientId}\n" +
                    $"UserName: {context.Username}\n" +
                    $"Password: {context.Password} \n" +
                    $"Endpoint: {context.Endpoint} \n" +
                    $"IsSecureConnection: {context.IsSecureConnection}\n";
            Application.Current.Dispatcher.Invoke(() =>
            {
                MainWindowViewModel.LogMessages.Add(new LogMessage(_infoConnection));
            });          
        }
        private void GetMessageInformation(MqttApplicationMessageInterceptorContext context)
        {           
            var payload = context.ApplicationMessage?.Payload == null ? null : Encoding.UTF8.GetString(context.ApplicationMessage?.Payload);
            _infoMessage = $"Отправлено сообщения пользователем (ID: {context.ClientId})\n" +
                $"На топик: {context.ApplicationMessage?.Topic}\n" +
                $"Сообщение: {payload}\n" +
                $"QoS: {context.ApplicationMessage?.QualityOfServiceLevel}\n" +
                $"Retain: {context.ApplicationMessage?.Retain}\n";
            Application.Current.Dispatcher .Invoke(() =>
            {
                MainWindowViewModel.LogMessages.Add(new LogMessage(_infoMessage));
            });
        }
        public void OnNewConnection(MqttConnectionValidatorContext context)
        {
            GetConnectionInformation(context);            
        }
        public void OnNewMessage(MqttApplicationMessageInterceptorContext context)
        {
            GetMessageInformation(context);
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
    public class LogMessage
    {
        public LogMessage(string message)
        {
            
            Message = message;
        }
        public string Message { get; set; }
    }

    public class ItemHandler
    {
        public ObservableCollection<LogMessage> Items { get; private set; }
        public ItemHandler()
        {
            Items = new ObservableCollection<LogMessage>();
        }      

        public void Add(LogMessage item)
        {   
            
            Items.Add(item);          

        }


    }
    } 
