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
        private string _number_Printer;

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

        public string Number_Printer
        {
            get => _number_Printer;
            set
            {
                if (_number_Printer != value)
                {
                    _number_Printer = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Number_Printer)));
                }
            }
        }
    }
}
