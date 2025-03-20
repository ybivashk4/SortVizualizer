using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SortVizualizer
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new Vizualizer_ViewModel();
        }       
    }
    public class Vizualizer_ViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<Item> Items { get; set; }

        public Vizualizer_ViewModel()
        {
            Items = new ObservableCollection<Item>();
            for (int i = 1; i < 101; i++)
            {
                Items.Add(new Item(i));
            }


        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class Item : BindableObject
    {
        public int Value { get; set; }
        public Item(int i) { Value = i; }
    }
}
