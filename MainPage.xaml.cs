using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public delegate void SortFunc(ref ObservableCollection<Item> data);
        public ICommand OnPickerSelectedIndexChanged { get; set; }

        public static void BubleSortDown(ref ObservableCollection<Item> data)
        {
            for (int i=0;i<100;i++)
            {
                for (int j=0;j<100;j++)
                {
                    if (data[i] > data[j])
                    {
                        (data[i], data[j]) = (data[j], data[i]);
                    }
                }
            }
        }

        public static void BubleSortUp(ref ObservableCollection<Item> data)
        {
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    if (data[i] < data[j])
                    {
                        (data[i], data[j]) = (data[j], data[i]);
                    }
                }
            }
        }

        public static void InsertUp(ref ObservableCollection<Item> data)
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
            }
        }

        public static void InsertDown(ref ObservableCollection<Item> data)
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
            }
        }

        public static void Sort(ref ObservableCollection<Item> data, SortFunc sortFunc)
        {
            sortFunc(ref data);
        }

        public ObservableCollection<Item> Items { get; set; }

        public Vizualizer_ViewModel()
        {
            PickSort = new Command<object>(pickSort);
            Items = new ObservableCollection<Item>();
            OnPickerSelectedIndexChanged = new Command<int>(onPickerSelectedIndexChanged);
            for (int i = 1; i < 101; i++)
            {
                Items.Add(new Item(i));
            }


        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void pickSort(object parametr)
        {
            pickSort(parametr, Items);
        }

        private void pickSort(object parametr, ObservableCollection<Item> items)
        {
            if (parametr is string strParam)
            {
                var SortId = int.Parse(strParam);
                switch (SortId)
                {
                    case 1:
                        Sort(ref items, BubleSortUp);
                        break;
                    case 2:
                        Sort(ref items, BubleSortDown);
                        break;
                    case 3:
                        Sort(ref items, InsertUp);
                        break;
                    case 4:
                        Sort(ref items, InsertDown);
                        break;
                    default:
                        break;
                }
            }
        }

        // Нейро-код
        private void onPickerSelectedIndexChanged(int selectedIndex)
        {
            
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class Item : BindableObject
    {
        private int _value;
        
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

    }
}
