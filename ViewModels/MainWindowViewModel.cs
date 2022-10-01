using MqttToolsMVVM.Infrastructure.Commands;
using MqttToolsMVVM.Models;
using MqttToolsMVVM.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MqttToolsMVVM.Infrastructure.Commands.Base;
using MQTTnet.Server;
using System;
using MQTTnet;
using System.Collections.ObjectModel;
using System.Net;

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
        private bool _useConnectionHandler;
        private bool _useMessageHandler;
        private bool _autoScroll;
        private bool _permissionToEdit = true;
        private bool _permissionToStart = true;
        private bool _permissionToStop = false;


        private static readonly ItemHandler itemHandler = new ItemHandler();        
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
        public bool UseConnectionHandler
        {
            get
            {
                return _useConnectionHandler;
            }
            set
            {
                Set(ref _useConnectionHandler, value);
            }
        }
        public bool UseMessageHandler
        {
            get
            {
                return _useMessageHandler;
            }
            set
            {
                Set(ref _useMessageHandler, value);
            }
        }              
        public bool AutoScroll
        {
            get
            {
                return _autoScroll;
            }
            set
            {
                _autoScroll = Set(ref _autoScroll, value);
            }
        } 
        public bool PermissionToEdit
        {
            get
            {
                return _permissionToEdit;
            }
            set
            {
                Set(ref _permissionToEdit, value);
            }
        }
        public bool PermissionToStart
        {
            get
            {
                return _permissionToStart;
            }
            set
            {
                Set(ref _permissionToStart, value);
            }
        }
        public bool PermissionToStop
        {
            get
            {
                return _permissionToStop;
            }
            set
            {
                Set(ref _permissionToStop, value);
            }
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
        public IAsyncCommand StartMqttServerCommand { get; set; }
       
        private async Task OnStartMqttServerCommandExecute()
        {
            
            MqttServerModel serverModel = new MqttServerModel(SelectedIp,Port,UseConnectionHandler,UseMessageHandler);
            
            Status = "/Resourses/Images/ServerOnline.png";
            StatusTooltip = "Сервер Online";
            PermissionToEdit = false;
            PermissionToStart = false;
            PermissionToStop = true;
            try
            {
                await MqttServerModel.mqttServer.StartAsync(serverModel.optionsBuilder.Build());
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
            PermissionToEdit = true;
            PermissionToStart = true;
            PermissionToStop = false;
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
