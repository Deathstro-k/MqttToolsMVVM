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
using System.Linq;
using System.Net.NetworkInformation;

namespace MqttToolsMVVM.ViewModels
{
    internal sealed class MainWindowViewModel: ViewModel
    {
        #region Поля и Свойства
        
        private string _localip;
        private string _publicip;
        private string _selectedip;
        private string _port ="1883";
        private string _status = "/Resourses/Images/ServerOffline.png";
        private string _statusTooltip = "Сервер Offline";
        private bool _useConnectionHandler;
        private bool _useMessageHandler;
        private bool _autoScroll;
        private bool _permissionToManipulation = true;
       


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
        public bool PermissionToManipulation
        {
            get
            {
                return _permissionToManipulation;
            }
            set
            {
                Set(ref _permissionToManipulation, value);
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
        #region Случайный порт
        public ICommand RandomFreePortCommand { get; }  

        private void OnRandomFreePortCommandExecute(object p) 
        {
            int port=0;
            port = (port > 0) ? port : new Random().Next(1, 65535);
            while (!IsFree(port))
            {
                port += 1;
            }
            bool IsFree(int prt)
            {
                IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
                IPEndPoint[] listeners = properties.GetActiveTcpListeners();
                int[] openPorts = listeners.Select(item => item.Port).ToArray<int>();
                return openPorts.All(openPort => openPort != prt);
            }
            Port = port.ToString();
        }
        private bool CanRandomFreePortCommandExecuted(object p) => true;
        #endregion
       
        #endregion
        #region Запуск сервера
        public IAsyncCommand StartMqttServerCommand { get; set; }
       
        private async Task OnStartMqttServerCommandExecute()
        {
            
            MqttServerModel serverModel = new MqttServerModel(SelectedIp,Port,UseConnectionHandler,UseMessageHandler);
            
            Status = "/Resourses/Images/ServerOnline.png";
            StatusTooltip = "Сервер Online";
            PermissionToManipulation = false;
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
            PermissionToManipulation = true;
            await MqttServerModel.StopMqttServer();
        }

       
        #endregion
        #endregion
        public MainWindowViewModel()
        {            
            CloseApplicationCommand = new Command(OnCloseApplicationCommandExecute, CanCloseApplicationCommandExecuted);
            RandomFreePortCommand = new Command(OnRandomFreePortCommandExecute, CanRandomFreePortCommandExecuted);
            StartMqttServerCommand = new AsyncCommand(OnStartMqttServerCommandExecute);
            StopMqttServerCommand = new AsyncCommand(OnStopMqttServerCommandExecute);
           

        }

        


    }
}
