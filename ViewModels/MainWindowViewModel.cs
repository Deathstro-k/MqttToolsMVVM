using MqttToolsMVVM.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttToolsMVVM.ViewModels
{
    internal class MainWindowViewModel: ViewModel
    {
        private string _title;
        private string Title
        {
            get { return _title; }
            set => Set(ref _title, value); 
        }
    }
}
