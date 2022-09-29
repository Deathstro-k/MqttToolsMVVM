using MqttToolsMVVM.Infrastructure.Commands;
using MqttToolsMVVM.Models;
using MqttToolsMVVM.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MqttToolsMVVM.Infrastructure.Commands.Base;
using System.Collections.Generic;
using MQTTnet.Server;
using System.Net;
using System;
using MQTTnet;
using System.Collections.ObjectModel;

namespace MqttToolsMVVM.ViewModels
{
    internal sealed class MainWindowViewModel: ViewModel
    {
        #region Поля и Свойства
        
        private string _localip;
        private string _publicip;
        private string _selectedip;
        private string _port="1883";
        private string _status = "/Resourses/Images/ServerOffline.png";
        private string _statusTooltip = "Сервер Offline";
       
        private static readonly ItemHandler itemHandler = new ItemHandler();
        private readonly static IMqttServer mqttServer = new MqttFactory().CreateMqttServer();
        public static ObservableCollection<LogMessage> LogMessages
        {
            get { return itemHandler.Items; }
        }

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
        public string SelectedIp
        {
            get
            {
                return _selectedip;
            }
            set => Set(ref _selectedip, value);
        }
        public string Port
        {
            get
            {
                return _port;
            }
            set => Set(ref _port, value);
        }
        public string Status
        {
            get
            {
                return _status;
            }
            set => Set(ref _status, value);
        }
        public string StatusTooltip
        {
            get
            {
                return _statusTooltip;
            }
            set => Set(ref _statusTooltip, value);
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
        public IAsyncCommand StartMqttServerCommand { get; private set; }
       
        private async Task OnStartMqttServerCommandExecute()
        {
            MqttServerModel serverModel = new MqttServerModel();
            Status = "/Resourses/Images/ServerOnline.png";
            StatusTooltip = "Сервер Online";
            var optionsBuilder = new MqttServerOptionsBuilder()
                       .WithDefaultEndpointBoundIPAddress(IPAddress.Parse(SelectedIp))
                       .WithDefaultEndpointPort(int.Parse(Port))
                       .WithConnectionValidator(serverModel.onNewConnection += serverModel.OnNewConnection)
                       .WithApplicationMessageInterceptor(serverModel.onNewMessage += serverModel.OnNewMessage);
            try
            {
                await mqttServer.StartAsync(optionsBuilder.Build());
            }
            catch (InvalidOperationException) { }

        }

        #endregion
        #region Остановка сервера
        public IAsyncCommand StopMqttServerCommand { get; private set; }
        private async Task OnStopMqttServerCommandExecute()
        {
            Status = "/Resourses/Images/ServerOffline.png";
            StatusTooltip = "Сервер Offline";
            await MqttServerModel.StopMqttServer();
        }
       
        #endregion
        #endregion
        public MainWindowViewModel()
        {
            
            CloseApplicationCommand = new Command(OnCloseApplicationCommandExecute, CanCloseApplicationCommandExecuted);
            StartMqttServerCommand = new AsyncCommand(OnStartMqttServerCommandExecute);
            StopMqttServerCommand = new AsyncCommand(OnStopMqttServerCommandExecute);
        }
        

    }
}
