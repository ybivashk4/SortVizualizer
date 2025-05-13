using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
namespace SortVizualizer
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new VizualizerViewModel();
        }

        
    }
    public partial class VizualizerViewModel : INotifyPropertyChanged
    {
        private static readonly int min_speed = 2;
        private int _size = 100;
        private int _speed = 2;
        public int Size
        {
            get { return _size; }
            set
            {
                if (_size != value && value > 1 && value <= 100)
                {
                    _size = value;
                }
            }
        }

        public int Speed
        {
            get { return _speed; }
            set
            {
                if (value >= 1 || value <= 6 )
                {
                    switch (value)
                    {
                        case 1:
                            _speed = min_speed;
                            break;
                        case 2:
                            _speed = min_speed*5;
                            break;
                        case 3:
                            _speed = min_speed*5*5;
                            break;
                        case 4:
                            _speed = min_speed*5*5*5;
                            break;
                        case 5:
                            _speed = min_speed*5*5*5*5;
                            break;
                        case 6:
                            _speed = min_speed*5*5*5*5*5;
                            break;
                    }
                }
            }
        }

        public ICommand PickSort { get; set; }
        public ICommand ShuffleDataCommand { get; set; }
        public ICommand UpOrderCommand { get; set; }
        public ICommand DownOrderCommand {  get; set; }

        public delegate Task SortFunc(ObservableCollection<Item> data);

        public async Task BubleSortDown(ObservableCollection<Item> data)
        {
            for (int i=0;i< Size; i++)
            {
                for (int j=0;j< Size - 1-i;j++)
                {
                    
                    if (data[j] < data[j+1])
                    {
                        data[j].Color = "Red";
                        data[j+1].Color = "Red";
                        Item.Swap(data[j], data[j]);
                        Item.Swap(data[j + 1], data[j + 1]);
                        OnPropertyChanged(nameof(Items));
                        Item.Swap(data[j], data[j + 1]);
                        await Task.Delay(Speed);
                        OnPropertyChanged(nameof(Items));
                    }
                    data[j].Color = "Green";
                    data[j+1].Color = "Green";
                    Item temp1 = new (data[j].Value);
                    data[j] = temp1.Assign(data[j]);
                    Item temp2 = new (data[j+1].Value);
                    data[j + 1] = temp2.Assign(data[j + 1]);

                }
                data[Size - 1 - i].Color = "Blue";
                Item temp = new (data[Size - 1 - i].Value);
                data[Size - 1 - i] = temp.Assign(data[Size - 1 - i]);
                OnPropertyChanged(nameof(Items));
            }
        }   

        public async Task BubleSortUp(ObservableCollection<Item> data)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size - 1 - i; j++)
                {

                    if (data[j] > data[j + 1])
                    {
                        data[j].Color = "Red";
                        data[j + 1].Color = "Red";
                        Item.Swap(data[j], data[j]);
                        Item.Swap(data[j + 1], data[j + 1]);
                        OnPropertyChanged(nameof(Items));
                        Item.Swap(data[j], data[j + 1]);
                        await Task.Delay(Speed);
                        OnPropertyChanged(nameof(Items));
                    }
                    data[j].Color = "Green";
                    data[j + 1].Color = "Green";
                    Item temp1 = new (data[j].Value);
                    data[j] = temp1.Assign(data[j]);
                    Item temp2 = new (data[j + 1].Value);
                    data[j + 1] = temp2.Assign(data[j + 1]);

                }
                data[Size - 1 - i].Color = "Blue";
                Item temp = new (data[Size - 1 - i].Value);
                data[Size - 1 - i] = temp.Assign(data[Size - 1 - i]);
                OnPropertyChanged(nameof(Items));
            }
        }

        public async Task SelectionUp(ObservableCollection<Item> data)
        {
            for (int i = 0; i < Size; i++)
            {
                int min_index = i;
                for (int j = i; j < Size; j++)
                {
                    data[i].Color = "Red";
                    data[j].Color = "Red";
                    (data[i], data[j]) = (data[j], data[i]);
                    (data[i], data[j]) = (data[j], data[i]);
                    OnPropertyChanged(nameof(data));
                    await Task.Delay(Speed);
                    if (data[j] < data[i])
                    {
                        min_index = j;
                    }
                    data[j].Color = "Green";
                    data[j].Color = "Green";
                }
                (data[i], data[min_index]) = (data[min_index], data[i]);
                data[i].Color = "Blue";
                OnPropertyChanged(nameof(Items));
                await Task.Delay(Speed);
            }
        }

        public async Task SelectionDown(ObservableCollection<Item> data)
        {
            for (int i = 0; i < Size; i++)
            {
                int max_index = i;
                for (int j = i; j < Size; j++)
                {
                    data[i].Color = "Red";
                    data[j].Color = "Red";
                    (data[i], data[j]) = (data[j], data[i]);
                    (data[i], data[j]) = (data[j], data[i]);
                    OnPropertyChanged(nameof(data));
                    await Task.Delay(Speed);
                    if (data[j] > data[i])
                    {
                        max_index = j;
                    }
                    data[j].Color = "Green";
                    data[j].Color = "Green";
                }
                (data[i], data[max_index]) = (data[max_index], data[i]);
                data[i].Color = "Blue";
                OnPropertyChanged(nameof(Items));
                await Task.Delay(Speed);
            }
        }

        private async Task HeapifyDown(ObservableCollection<Item> data, int cur_n, int i)
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
                await Task.Delay(Speed);
                await HeapifyDown(data, cur_n, largest);
                data[largest].Color = "Green";
                data[i].Color = "Green";
            }
        }

        public async Task HeapSortDown(ObservableCollection<Item> data)
        {
            int cur_n = Size;
            while (cur_n > 0)
            {
                for (int i = cur_n / 2 - 1; i >= 0; i--)
                    await HeapifyDown(data, cur_n, i);
                Item item = data[0];
                item.Color = "Blue";
                data.Remove(data[0]);
                OnPropertyChanged(nameof(Items));
                data.Add(item);
                OnPropertyChanged(nameof(Items));
                cur_n -= 1;
            }
        }


        private async Task HeapifyUp(ObservableCollection<Item> data, int cur_n, int i)
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
                await Task.Delay(Speed);
                await HeapifyUp(data, cur_n, smallest);
                data[smallest].Color = "Green";
                data[i].Color = "Green";
            }
        }

        public async Task HeapSortUp(ObservableCollection<Item> data)
        {
            int cur_n = Size;
            while (cur_n > 0)
            {
                for (int i = cur_n / 2 - 1; i >= 0; i--)
                    await HeapifyUp(data, cur_n, i);
                Item item = data[0];
                item.Color = "Blue";
                data.Remove(data[0]);
                OnPropertyChanged(nameof(Items));
                data.Add(item);
                OnPropertyChanged(nameof(Items));
                cur_n -= 1;
            }
        }

        public async Task InsertSortUp(ObservableCollection<Item> data)
        {
            for (int i=1;i<Size;i++)
            {
                int j = i - 1;
                Item key = data[i];
                while (j >= 0 && data[j] > key)
                {
                    data[j + 1].Color = "Red";
                    data[j].Color = "Red";
                    data[j + 1] = data[j];
                    OnPropertyChanged(nameof(data));
                    await Task.Delay(Speed);
                    j -= 1;
                }
                data[j + 1] = key;
                OnPropertyChanged(nameof(data));
                await Task.Delay(Speed);
            }
        }

        public async Task InsertSortDown(ObservableCollection<Item> data)
        {
            for (int i = 1; i < Size; i++)
            {
                int j = i - 1;
                Item key = data[i];
                while (j >= 0 && data[j] < key)
                {
                    data[j + 1].Color = "Red";
                    data[j].Color = "Red";
                    data[j + 1] = data[j];
                    OnPropertyChanged(nameof(data));
                    await Task.Delay(Speed);
                    j -= 1;
                }
                data[j + 1] = key;
                OnPropertyChanged(nameof(data));
                await Task.Delay(Speed);
            }
        }

        public async Task MergeUp(ObservableCollection<Item> data, int l, int m, int r)
        {
            int n1 = m - l + 1;
            int n2 = r - m;
            int i, j;
            ObservableCollection<Item> L = [];
            ObservableCollection<Item> R = [];
            for (i = 0; i < n1; ++i)
                L.Add(data[l + i]);
            for (j = 0; j < n2; ++j)
                R.Add(data[m + 1 + j]);
            i = 0;
            j = 0;
            int k = l;
            while (i < n1 && j < n2)
            {
                if (L[i].Value <= R[j].Value)
                {
                    data[k] = L[i];
                    OnPropertyChanged(nameof (data));
                    await Task.Delay(Speed);
                    i++;
                }
                else
                {
                    OnPropertyChanged(nameof(data));
                    await Task.Delay(Speed);
                    data[k] = R[j];
                    j++;
                }
                k++;
            }
            while (i < n1)
            {
                data[k] = L[i];
                OnPropertyChanged(nameof(data));
                await Task.Delay(Speed);
                i++;
                k++;
            }
            while (j < n2)
            {
                data[k] = R[j];
                OnPropertyChanged(nameof(data));
                await Task.Delay(Speed);
                j++;
                k++;
            }
        }
        public async Task MergeSortApiUp(ObservableCollection<Item> data, int l, int r)
        {
            if (l < r)
            {
                int m = l + (r - l) / 2;
                await MergeSortApiUp(data, l, m);
                await MergeSortApiUp(data, m + 1, r);
                await MergeUp(data, l, m, r);
            }
        }

        public async Task MergeSortUp(ObservableCollection<Item> data)
        {
            await MergeSortApiUp(data, 0, Size-1);
        }


        public async Task MergeDown(ObservableCollection<Item> data, int l, int m, int r)
        {
            int n1 = m - l + 1;
            int n2 = r - m;
            int i, j;
            ObservableCollection<Item> L = [];
            ObservableCollection<Item> R = [];
            for (i = 0; i < n1; ++i)
                L.Add(data[l + i]);
            for (j = 0; j < n2; ++j)
                R.Add(data[m + 1 + j]);
            i = 0;
            j = 0;
            int k = l;
            while (i < n1 && j < n2)
            {
                if (L[i].Value >= R[j].Value)
                {
                    data[k] = L[i];
                    OnPropertyChanged(nameof(data));
                    await Task.Delay(Speed);
                    i++;
                }
                else
                {
                    OnPropertyChanged(nameof(data));
                    await Task.Delay(Speed);
                    data[k] = R[j];
                    j++;
                }
                k++;
            }
            while (i < n1)
            {
                data[k] = L[i];
                OnPropertyChanged(nameof(data));
                await Task.Delay(Speed);
                i++;
                k++;
            }
            while (j < n2)
            {
                data[k] = R[j];
                OnPropertyChanged(nameof(data));
                await Task.Delay(Speed);
                j++;
                k++;
            }
        }
        public async Task MergeSortApiDown(ObservableCollection<Item> data, int l, int r)
        {
            if (l < r)
            {
                int m = l + (r - l) / 2;
                await MergeSortApiDown(data, l, m);
                await MergeSortApiDown(data, m + 1, r);
                await MergeDown(data, l, m, r);
            }
        }

        public async Task MergeSortDown(ObservableCollection<Item> data)
        {
            await MergeSortApiDown(data, 0, Size - 1);
        }

        private int PartitionUp(ObservableCollection<Item> data, int low, int high)
        {

            Item pivot = data[high];
            int i = low - 1;
            for (int j = low; j <= high - 1; j++)
            {
                if (data[j] < pivot)
                {
                    i++;
                    (data[i], data[j]) = (data[j], data[i]);
                    data[i].Color = "Red";
                    data[j].Color = "Red";
                }
            }
            (data[i+1], data[high]) = (data[high], data[i+1]);
            return i + 1;
        }

        private async Task QuickSortApiUp(ObservableCollection<Item> data, int low, int high)
        {
            if (low < high)
            {
                int pi = PartitionUp(data, low, high);
                OnPropertyChanged(nameof(data));
                await Task.Delay(Speed);
                await QuickSortApiUp(data, low, pi - 1);
                await QuickSortApiUp(data, pi + 1, high);
            }
        }
        public async Task QuickSortUp (ObservableCollection<Item> data)
        {
            await QuickSortApiUp(data, 0, Size - 1);
        }

        private Task<int> PartitionDown(ObservableCollection<Item> data, int low, int high)
        {

            Item pivot = data[high];

            int i = low - 1;

            for (int j = low; j <= high - 1; j++)
            {
                if (data[j] > pivot)
                {
                    i++;
                    (data[i], data[j]) = (data[j], data[i]);
                    data[i].Color = "Red";
                    data[j].Color = "Red";
                }
            }

            (data[i + 1], data[high]) = (data[high], data[i + 1]);
            return Task<int>.Factory.StartNew(() => i+1);
        }

        private async Task QuickSortApiDown(ObservableCollection<Item> data, int low, int high)
        {
            if (low < high)
            {
                int pi = PartitionDown(data, low, high).Result;
                OnPropertyChanged(nameof(data));
                await Task.Delay(Speed);
                
                await QuickSortApiDown(data, low, pi - 1);
                await QuickSortApiDown(data, pi + 1, high);
            }
        }
        public async Task QuickSortDown(ObservableCollection<Item> data)
        {
            await QuickSortApiDown(data, 0, Size - 1);
        }

        public static async Task Sort(ObservableCollection<Item> data, SortFunc sortFunc)
        {
            await sortFunc(data);
        }
        public void ShuffleData(object obj)
        {
            ShuffleData(Items);
        }
        public void ShuffleData(ObservableCollection<Item> data)
        {

            List<Item> list = new List<Item>();
            Random random = new();
            foreach (Item item in data)
            {
                list.Add(item);
            }
            Span<Item> span = new(list.ToArray());
            random.Shuffle(span);
            data.Clear();
            foreach (var item in span)
            {
                data.Add(item);
            }
        }

        public void DownOrder(object obj)
        {
            DownOrder(Items);
        }
        public void DownOrder(ObservableCollection<Item> data)
        {
            data.Clear();
            for (int i=Size-1;i>=0;i--)
            {
                data.Add(new Item(i * 3));
            }
        }
        public void UpOrder(object obj) { UpOrder(Items); }
        public void UpOrder(ObservableCollection<Item> data)
        {
            data.Clear();
            for (int i = 0; i < Size; i++)
            {
                data.Add(new Item(i * 3));
            }
        }

        public ObservableCollection<Item> Items { get; set; }

        public VizualizerViewModel()
        {
            PickSort = new Command<object>(PickSortFunc);
            ShuffleDataCommand = new Command<object>(ShuffleData);
            UpOrderCommand = new Command<object>(UpOrder);
            DownOrderCommand = new Command<object>(DownOrder);

            Items = [];
            for (int i = 1; i < Size +1; i++)
            {
                Items.Add(new Item(i*3));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private async void PickSortFunc(object parametr)
        {
            await PickSortFunc(parametr, Items);
        }

        private async Task PickSortFunc(object parametr, ObservableCollection<Item> items)
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
                        await Sort(items, SelectionUp);
                        break;
                    case 4:
                        await Sort(items, SelectionDown);
                        break;
                    case 5:
                        await Sort(items, HeapSortDown);
                        break;
                    case 6:
                        await Sort(items, HeapSortUp);
                        break;
                    case 7:
                        await Sort(items, InsertSortDown);
                        break;
                    case 8:
                        await Sort(items, InsertSortUp);
                        break;
                    case 9:
                        await Sort(items, MergeSortUp);
                        break;
                    case 10:
                        await Sort(items, MergeSortDown);
                        break;
                    case 11:
                        await Sort(items, QuickSortUp);
                        break;
                    case 12:
                        await Sort(items, QuickSortDown);
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

        public override bool Equals(object? obj)
        {
            return obj is VizualizerViewModel model &&
                   _size == model._size &&
                   _speed == model._speed &&
                   Size == model.Size &&
                   Speed == model.Speed &&
                   EqualityComparer<ICommand>.Default.Equals(PickSort, model.PickSort) &&
                   EqualityComparer<ObservableCollection<Item>>.Default.Equals(Items, model.Items);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_size, _speed, Size, Speed, PickSort, Items);
        }
    }
    public partial class Item : BindableObject
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

       public Item Assign(Item item)
        {
            Color = item.Color;
            Value = item.Value;
            return this;
        }

        public static void Swap(Item item1, Item item2)
        {
            Item temp = new (item1.Value);
            temp.Assign(item1);
            item1.Assign(item2);
            item2.Assign(temp);
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
