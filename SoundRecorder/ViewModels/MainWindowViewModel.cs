using Microsoft.VisualBasic;
using ReactiveUI;
using System.Windows.Input;

namespace SoundRecorder.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel() 
        {

        
        }
        public string Greeting => "Welcome to Avalonia!";
        //public ICommand RecordCommand { get; set; }

        //public ICommand StopRecCommand { get; set; }
    }
}