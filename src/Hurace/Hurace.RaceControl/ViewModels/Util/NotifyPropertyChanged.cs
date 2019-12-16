using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FluentValidation;

namespace Hurace.RaceControl.ViewModels.Util
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
        
        protected void InvokePropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}