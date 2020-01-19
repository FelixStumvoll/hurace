using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Hurace.RaceControl.ViewModels.Util
{
    public static class MessageBoxUtil
    {
        public static void Error(string errorMessage) =>
            MessageBox.Show(errorMessage,
                            "Fehler",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
    }
}