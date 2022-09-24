using MqttToolsMVVM.Infrastructure.Commands;
using MqttToolsMVVM.Models;
using MqttToolsMVVM.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MqttToolsMVVM.Infrastructure.Commands.Base;
using System.Collections.Generic;

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
        private string _logMessages;
        private static ItemHandler itemHandler = new ItemHandler();
        public static List<Item> Items
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
        public string LogMessages 
        { 
            get
            {
                return _logMessages;
            }
            set => Set(ref _logMessages, value);
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

            await serverModel.StartMqttServer(SelectedIp.ToString(),Port);
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
