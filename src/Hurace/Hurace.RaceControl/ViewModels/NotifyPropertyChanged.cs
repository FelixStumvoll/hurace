using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hurace.RaceControl.ViewModels
{
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        protected void Set<T>(ref T field, T value, bool ignoreEqual = false,
            [CallerMemberName] string propertyName = "")
        {
            if ((ignoreEqual == false && EqualityComparer<T>.Default.Equals(value, field))) return;
            field = value;
            InvokePropertyChanged(propertyName);
        }

        protected void InvokePropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}