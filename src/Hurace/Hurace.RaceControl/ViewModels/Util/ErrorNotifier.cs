using System.Windows;

namespace Hurace.RaceControl.ViewModels.Util
{
    public static class ErrorNotifier
    {
        public static void OnLoadError() => 
            MessageBox.Show("Fehler beim Laden der Daten", 
                            "Fehler", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Error);
        
        public static void OnSaveError() => 
            MessageBox.Show("Fehler beim Speichern der Daten", 
                            "Fehler", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Error);
    }
}