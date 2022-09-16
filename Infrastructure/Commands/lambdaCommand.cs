using MqttToolsMVVM.Infrastructure.Commands.Base;
using System;

namespace MqttToolsMVVM.Infrastructure.Commands
{
    internal class lambdaCommand : Command
    {

        private Action<object> _Execute;

        private Func<object, bool> _CanExecute;
        public void  LambdaCommand(Action<object> Execute, Func<object, bool> CanExecute = null) 
        {
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _CanExecute = CanExecute;
        }
        public override bool CanExecute(object parameter) => _CanExecute?.Invoke(parameter) ?? true;
       

        public override void Execute(object parameter) => _Execute(parameter); 
       
    }
}
