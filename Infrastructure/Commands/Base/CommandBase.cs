using System;
using System.Windows.Input;

namespace MqttToolsMVVM.Infrastructure.Commands.Base
{
    internal abstract class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested+=value;
            remove => CommandManager.RequerySuggested -= value;                    
        }

        /// <summary>
        /// Проверяет, возмодно ли сделать эту команду
        /// </summary>
      
        public abstract bool CanExecute(object parameter);


        /// <summary>
        /// Основная логика команды
        /// </summary>        

        public abstract void Execute(object parameter);
      
    }
}
