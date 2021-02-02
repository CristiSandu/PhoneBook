using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace AgendaTelefonica.Models
{
    class DialModeliNotifay : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _number;
        public string Number
        {
            get => _number;
            set
            {
                if (_number != value)
                {
                    _number = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Number)));
                }
            }
        }
    }
}
