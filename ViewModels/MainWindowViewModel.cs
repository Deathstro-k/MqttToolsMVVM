using MQTTnet;
using MQTTnet.Server;
using MqttToolsMVVM.Infrastructure.Commands;
using MqttToolsMVVM.Models;
using MqttToolsMVVM.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Net;
using System.Net.Sockets;
namespace MqttToolsMVVM.ViewModels
{
    internal class MainWindowViewModel: ViewModel
    {
        #region Поля и Свойства
        public static MqttServerModel mqttServerCreator = new MqttServerModel();
        private string _localip;
        private string _publicip;
        
        private string _port="1883";

        public string LocalIp
        {
            get 
            {
                return MqttServerModel.GetLocalIPAddressIPv4();
            }
            set => Set(ref _localip, value);
        } 
        public string PublicIp
        {
            get
            {
                return MqttServerModel.GetPublicIpAddressIpv4();
            }
            set => Set(ref _publicip, value);
        }

        public string Port
        {
            get
            {
                return _port;
            }
            set => Set(ref _port, value);
        }
        #endregion
        #region Команды
        #region Закрытие приложения
        public ICommand CloseApplicationCommand { get; }

        private bool CanCloseApplicationCommandExecuted(object p) => true;

        private void OnCloseApplicationCommandExecute(object p) 
        {
            Application.Current.Shutdown();
        }
        #endregion
        #region Запуск сервера
        public ICommand StartMqttServerCommand { get; }

        private bool CanStartMqttServetCommandExecuted(object p)
        {
            if (p != null) return true;
            else return false;  
        }
        

        private async void OnStartMqttServetCommandExecute(object p) 
        {   
            
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithDefaultEndpointBoundIPAddress(IPAddress.Parse(LocalIp))
                .WithDefaultEndpointPort(int.Parse(Port))
                .WithConnectionValidator(MqttHandlers.onNewConnection)
                .WithApplicationMessageInterceptor(MqttHandlers.onNewMessage);
            var mqttServer = new MqttFactory().CreateMqttServer();
            await mqttServer.StartAsync(optionsBuilder.Build());
        }
       
        #endregion
        #endregion
        public MainWindowViewModel()
        {
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecute, CanCloseApplicationCommandExecuted);
            StartMqttServerCommand = new LambdaCommand(OnStartMqttServetCommandExecute, CanStartMqttServetCommandExecuted);
        }
            
    }
}
