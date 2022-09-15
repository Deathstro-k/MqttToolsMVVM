using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MqttToolsMVVM.ViewModels.Base
{
    internal abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Генератор события "Изменение свойства" для классов наследников
        /// </summary>
        /// <param name="PropertyName">Имя свойства</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }


        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        { 
            if(Equals(field,value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }    
    }
}
