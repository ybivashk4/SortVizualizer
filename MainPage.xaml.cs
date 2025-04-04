using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
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
        public ICommand PickSort { get; set; }
        public delegate Task SortFunc(ObservableCollection<Item> data);
        public ICommand OnPickerSelectedIndexChanged { get; set; }

        public async Task BubleSortDown(ObservableCollection<Item> data)
        {
            for (int i=0;i<100;i++)
            {
                for (int j=0;j<100-1;j++)
                {
                    if (data[j] < data[j+1])
                    {
                        (data[j], data[j+1]) = (data[j+1], data[j]);
                        OnPropertyChanged(nameof(Items));
                        await Task.Delay(10);
                    }
                }
            }
        }

        public async Task BubleSortUp(ObservableCollection<Item> data)
        {
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    if (data[j] > data[j + 1])
                    {
                        (data[j], data[j + 1]) = (data[j + 1], data[j]);
                        OnPropertyChanged(nameof(Items));
                        await Task.Delay(10);
                    }
                }
            }
        }

        public async Task InsertUp( ObservableCollection<Item> data)
        {
            for (int i = 0; i < 100; i++)
            {
                Item max = data[i];
                int k = i;
                for (int j = 0; j < 100-i; j++)
                {
                    if (data[j] < max)
                    {
                        max = data[j];
                        k = j;
                    }
                }
                (data[100 - i - 1], data[k]) = (max, data[100 - i - 1]);
                OnPropertyChanged(nameof(Items));
                await Task.Delay(50);
            }
        }

        public async Task InsertDown(ObservableCollection<Item> data)
        {
            for (int i = 0; i < 100; i++)
            {
                Item min = data[i];
                int k = i;
                for (int j = 0; j < 100 - i; j++)
                {
                    if (data[j] > min)
                    {
                        min = data[j];
                        k = j;
                    }
                }
                (data[100 - i - 1], data[k]) = (min, data[100 - i - 1]);
                OnPropertyChanged(nameof(Items));
                await Task.Delay(50);
            }
        }

        public async Task Sort(ObservableCollection<Item> data, SortFunc sortFunc)
        {
            sortFunc(data);
        }

        public ObservableCollection<Item> Items { get; set; }

        public Vizualizer_ViewModel()
        {
            PickSort = new Command<object>(pickSort);
            Items = new ObservableCollection<Item>();
            for (int i = 1; i < 101; i++)
            {
                Items.Add(new Item(i*3));
            }


        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private async void pickSort(object parametr)
        {
            await pickSort(parametr, Items);
        }

        private async Task pickSort(object parametr, ObservableCollection<Item> items)
        {
            if (parametr is string strParam)
            {
                var SortId = int.Parse(strParam);
                switch (SortId)
                {
                    case 1:
                        await Sort(items, BubleSortUp);
                        break;
                    case 2:
                        await Sort(items, BubleSortDown);
                        break;
                    case 3:
                        await Sort(items, InsertUp);
                        break;
                    case 4:
                        await Sort(items, InsertDown);
                        break;
                    default:
                        break;
                }
            }
        }



        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class Item : BindableObject
    {
        private int _value;
        private String _color;
        public Item(int i) { Value = i; }
        public static bool operator >(Item item1, Item item2)
        {
            return item1.Value > item2.Value;
        }

        public static bool operator <(Item item1, Item item2)
        {
            return item1.Value < item2.Value;
        }

        public int Value 
        { 
            get => _value; 
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged(); // Уведомляем об изменении
                }
            }
        }
        public String Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged(); // Уведомляем об изменении
                }
            }
        }

    }
}
