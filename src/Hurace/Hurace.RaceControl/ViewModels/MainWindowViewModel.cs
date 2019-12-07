using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Hurace.Core.Api;
using Hurace.Core.Api.RaceControl;
using Hurace.Core.Api.RaceCrud;
using Hurace.Dal.Domain;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.Pages;
using Hurace.RaceControl.ViewModels.Commands;
using Hurace.RaceControl.ViewModels.Util;

namespace Hurace.RaceControl.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        public Page CurrentPage { get; set; }
        private Page _racePage;
        private Page _mainPage;

        public MainViewModel()
        {
            _racePage = new RacePage();
            _mainPage = new MainPage();
            CurrentPage = _mainPage;
        }
    }
}