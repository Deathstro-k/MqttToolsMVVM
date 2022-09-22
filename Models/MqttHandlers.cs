using MQTTnet.Server;
using System;
using System.Text;

namespace MqttToolsMVVM.Models
{
    internal class MqttHandlers
    {
        public static Action<MqttConnectionValidatorContext> onNewConnection;
        public static Action<MqttApplicationMessageInterceptorContext> onNewMessage;
        
        private string _infoConnection;
        private string _infoMessage;

       
        /// <summary>
        /// Вызов функции, когда произведено новое подключение
        /// </summary>
        /// <param name="context"></param>

        public void OnEnable()
        {
            onNewConnection += OnNewConnection;
            onNewMessage += OnNewMessage;

        }
        public void OnDisable()
        {
            onNewConnection -= OnNewConnection;
        }


        private string GetConnectionInformation(MqttConnectionValidatorContext context)
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

        private string GetMessageInformation(MqttApplicationMessageInterceptorContext context)
        {
            
            var payload = context.ApplicationMessage?.Payload == null ? null : Encoding.UTF8.GetString(context.ApplicationMessage?.Payload);
            _infoMessage = $"Отправлено сообщения пользователем (ID: {context.ClientId})\n" +
                $"На топик: {context.ApplicationMessage?.Topic}\n" +
                $"Сообщение: {payload}\n" +
                $"QoS: {context.ApplicationMessage?.QualityOfServiceLevel}\n" +
                $"Retain: {context.ApplicationMessage?.Retain}\n" +
                $"----------------------------------------------------------------------------";
            return _infoMessage;

        }


        public void OnNewConnection(MqttConnectionValidatorContext context)
        {
            GetConnectionInformation(context);
        }

        public void OnNewMessage(MqttApplicationMessageInterceptorContext context)
        {
            GetMessageInformation(context);
        }
    }
}
