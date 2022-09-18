using MQTTnet;
using MQTTnet.Server;
using MqttToolsMVVM.Infrastructure.Commands;
using MqttToolsMVVM.Models;
using MqttToolsMVVM.ViewModels.Base;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MqttToolsMVVM.Infrastructure.Commands.Base;
using System.Windows.Controls;

namespace MqttToolsMVVM.ViewModels
{
    internal sealed class MainWindowViewModel: ViewModel
    {
        #region Поля и Свойства
        public static MqttServerModel mqttServerCreator = new MqttServerModel();
        private string _localip;
        private string _publicip;
        private ListBoxItem _selectedip;
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
        public ListBoxItem SelectedIp
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

        private bool CanStartMqttServetCommandExecuted() => (SelectedIp != null && Port!=null) ? true : false;

        private async Task OnStartMqttServetCommandExecute()
        {

            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithDefaultEndpointBoundIPAddress(IPAddress.Parse(SelectedIp.Content.ToString()))
                .WithDefaultEndpointPort(int.Parse(Port));
                var mqttServer = new MqttFactory().CreateMqttServer();
                await mqttServer.StartAsync(optionsBuilder.Build());
            
           
        }
       
        #endregion
        #endregion
        public MainWindowViewModel()
        {
            CloseApplicationCommand = new Command(OnCloseApplicationCommandExecute, CanCloseApplicationCommandExecuted);
            StartMqttServerCommand = new AsyncCommand(OnStartMqttServetCommandExecute);
        }
            
    }
}
