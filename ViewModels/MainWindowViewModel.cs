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

namespace MqttToolsMVVM.ViewModels
{
    internal class MainWindowViewModel: ViewModel
    {

        public static MqttServerModel mqttServerCreator = new MqttServerModel();
        private string _localip;
        private string _remoteip;   
        private string _port;
        public string LocalIp
        {
            get 
            {
                return MqttServerModel.GetLocalIPAddressIPv4();
            }
            set => Set(ref _localip, value);
        } 
        public string Port
        {
            get
            {
                return mqttServerCreator.Port.ToString();
            }
            set => Set(ref _port, value);
        }

        #region Команды
        #region Закрытие приложения
        public ICommand CloseApplicationCommand { get; }

        private bool CanCloseApplicationCommandExecuted(object p) => true;

        private void OnCloseApplicationCommandExecute(object p) 
        {
            Application.Current.Shutdown();
        }
        #endregion
        #region
        public ICommand StartMqttServerCommand { get; }

        private bool CanStartMqttServetCommandExecuted(object p)
        {
            if (p != null) return true;
            else return false;  
        }
        

        private async void OnStartMqttServetCommandExecute(object p) 
        {   
            
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithDefaultEndpointBoundIPAddress(mqttServerCreator.Ip)
                .WithDefaultEndpointPort(mqttServerCreator.Port)
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
