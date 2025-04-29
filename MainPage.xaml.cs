using Microsoft.Maui.ApplicationModel;
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
        private static int n = 100;
        public ICommand PickSort { get; set; }
        public delegate Task SortFunc(ObservableCollection<Item> data);
        public ICommand OnPickerSelectedIndexChanged { get; set; }

        public async Task BubleSortDown(ObservableCollection<Item> data)
        {
            for (int i=0;i<n;i++)
            {
                for (int j=0;j<n-1-i;j++)
                {
                    
                    if (data[j] < data[j+1])
                    {
                        data[j].Color = "Red";
                        data[j+1].Color = "Red";
                        Item.swap(data[j], data[j]);
                        Item.swap(data[j + 1], data[j + 1]);
                        await Task.Delay(10);
                        OnPropertyChanged(nameof(Items));
                        Item.swap(data[j], data[j + 1]);
                        await Task.Delay(10);
                        OnPropertyChanged(nameof(Items));
                    }
                    data[j].Color = "Green";
                    data[j+1].Color = "Green";
                    Item temp1 = new Item(data[j].Value);
                    data[j] = temp1.assign(data[j]);
                    Item temp2 = new Item(data[j+1].Value);
                    data[j + 1] = temp2.assign(data[j + 1]);

                }
                data[n - 1 - i].Color = "Blue";
                Item temp = new Item(data[n - 1 - i].Value);
                data[n - 1 - i] = temp.assign(data[n - 1 - i]);
                await Task.Delay(10);
                OnPropertyChanged(nameof(Items));
            }
        }

        public async Task BubleSortUp(ObservableCollection<Item> data)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
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

        public async Task heapifyDown(ObservableCollection<Item> data, int cur_n, int i)
        {
            int largest = i;
            data[i].Color = "Red";
            int l = 2 * i + 1;
            int r = 2 * i + 2;
            if (l < cur_n && data[i] < data[l])
            {
                largest = l;
                data[largest].Color = "Red";
            }
            if (r < cur_n && data[r] > data[largest])
            {
                data[largest].Color = "Green";
                largest = r;
                data[largest].Color = "Red";
            }

            if (largest != i)   
            {
                (data[largest], data[i]) = (data[i], data[largest]);
                OnPropertyChanged(nameof(Items));
                await Task.Delay(100);
                await heapifyDown(data, cur_n, largest);
                data[largest].Color = "Green";
                data[i].Color = "Green";
            }
        }

        public async Task HeapSortDown(ObservableCollection<Item> data)
        {
            int cur_n = n;
            while (cur_n > 0)
            {
                for (int i = cur_n / 2 - 1; i >= 0; i--)
                    await heapifyDown(data, cur_n, i);
                Item item = data[0];
                item.Color = "Blue";
                data.Remove(data[0]);
                OnPropertyChanged(nameof(Items));
                data.Add(item);
                OnPropertyChanged(nameof(Items));
                cur_n -= 1;
            }
        }


        public async Task heapifyUp(ObservableCollection<Item> data, int cur_n, int i)
        {
            int smallest = i;
            data[i].Color = "Red";
            int l = 2 * i + 1;
            int r = 2 * i + 2;
            if (l < cur_n && data[i] > data[l])
            {
                smallest = l;
                data[smallest].Color = "Red";
            }
            if (r < cur_n && data[r] < data[smallest])
            {
                data[smallest].Color = "Green";
                smallest = r;
                data[smallest].Color = "Red";
            }

            if (smallest != i)
            {
                (data[smallest], data[i]) = (data[i], data[smallest]);
                OnPropertyChanged(nameof(Items));
                await Task.Delay(100);
                await heapifyUp(data, cur_n, smallest);
                data[smallest].Color = "Green";
                data[i].Color = "Green";
            }
        }

        public async Task HeapSortUp(ObservableCollection<Item> data)
        {
            int cur_n = n;
            while (cur_n > 0)
            {
                for (int i = cur_n / 2 - 1; i >= 0; i--)
                    await heapifyUp(data, cur_n, i);
                Item item = data[0];
                item.Color = "Blue";
                data.Remove(data[0]);
                OnPropertyChanged(nameof(Items));
                data.Add(item);
                OnPropertyChanged(nameof(Items));
                cur_n -= 1;
            }
        }



        public async Task InsertUp( ObservableCollection<Item> data)
        {
            for (int i = 0; i < n; i++)
            {
                Item max = data[i];
                int k = i;
                for (int j = 0; j < n-i; j++)
                {
                    if (data[j] < max)
                    {
                        max = data[j];
                        k = j;
                    }
                }
                (data[n - i - 1], data[k]) = (max, data[n - i - 1]);
                OnPropertyChanged(nameof(Items));
                await Task.Delay(50);
            }
        }

        public async Task InsertDown(ObservableCollection<Item> data)
        {
            for (int i = 0; i < n; i++)
            {
                Item min = data[i];
                int k = i;
                for (int j = 0; j < n - i; j++)
                {
                    if (data[j] > min)
                    {
                        min = data[j];
                        k = j;
                    }
                }
                (data[n - i - 1], data[k]) = (min, data[n - i - 1]);
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
            for (int i = 1; i < n+1; i++)
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
                    case 5:
                        await Sort(items, HeapSortDown);
                        break;
                    case 6:
                        await Sort(items, HeapSortUp);
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
        private string _color = "";

        public Item(int i) { Value = i; Color = "Green"; }
        public static bool operator >(Item item1, Item item2)
        {
            return item1.Value > item2.Value;
        }

        public static bool operator <(Item item1, Item item2)
        {
            return item1.Value < item2.Value;
        }

       public Item assign(Item item)
        {
            Color = item.Color;
            Value = item.Value;
            return this;
        }

        public static void swap(Item item1, Item item2)
        {
            Item temp = new Item(item1.Value);
            temp.assign(item1);
            item1.assign(item2);
            item2.assign(temp);
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

        public string Color
        {
            get => _color;
            set
            {
                if (!_color.Equals(value))
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
