using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SortVizualizer
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
            
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new Vizualizer_ViewModel();
        }

        public class Vizualizer_ViewModel : INotifyPropertyChanged
        {

            public ObservableCollection<int> Items { get; set; }

            public Vizualizer_ViewModel()
            {
                Items = new ObservableCollection<int>();
                for (int i=1;i<101;i++)
                {
                    Items.Add(i);
                }


            }

            public event PropertyChangedEventHandler? PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
