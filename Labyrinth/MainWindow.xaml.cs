using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Labyrinth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private int _currentX, _currentY;
        private int _counter;
        private bool[,] _haveChecked;
        //public int y;
        public int numberX { get; set; } = 8;
        public int numberY { get; set; } = 8;

        private ObservableCollection<ObservableCollection<bool>> _map;
        public ObservableCollection<ObservableCollection<FillColor>> List {  get; set; }


        
        public MainWindow()
        {
            InitializeComponent();

            
            DataContext = this;
            while (!makeLab()) ;
            
            //MessageBox.Show(_counter.ToString());
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool makeLab()
        {
            _haveChecked = new bool[numberX, numberY];
            if (List != null || _map != null)
            {
                List.Clear();
                _map.Clear();
            }
            List = new ObservableCollection<ObservableCollection<FillColor>>();
            _map = new ObservableCollection<ObservableCollection<bool>>();
            _counter = 0;
            Random r = new Random();
            for (int i = 0; i < numberX; i++) 
            {
                List.Add(new ObservableCollection<FillColor>());
                _map.Add(new ObservableCollection<bool>());
                for (int j = 0; j < numberY; j++)
                {
                    if (r.Next(100) < 50)
                    {
                        _map[i].Add(true);
                        List[i].Add(new FillColor(Brushes.Black));
                    }
                    else
                    {
                        _map[i].Add(false);
                        List[i].Add(new FillColor(Brushes.Black));
                    }
                }
            }
            while (!StartingPoint()) ;
            if (Fibo(_currentX, _currentY) > 15)
            {
                return true;
            }

            else
            {
                return false;
            }
 
        }

        private bool StartingPoint()
        {
            Random r = new Random();
            _currentX = r.Next(_map.Count);
            _currentY = r.Next(_map.Count);
            if (_map[_currentX][_currentY] == true)
            {
                List[_currentX][_currentY].Fill = Brushes.LightGreen;
                return true;
            }
            else
            {
                 return false;
            }
        }


        public class FillColor : INotifyPropertyChanged
        {
            private SolidColorBrush _fill;
            public SolidColorBrush Fill
            {
                get { return _fill; }
                set
                {
                    if (value != _fill)
                    {
                        _fill = value;
                        OnPropertyChanged(nameof(Fill));
                    }
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public FillColor(SolidColorBrush fill)
            {
                Fill = fill;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (_currentY > 0)
                {
                    if (_map[_currentX][_currentY - 1] == true)
                    {
                        _currentY--;
                        List[_currentX][_currentY].Fill = Brushes.LightGreen;
                        List[_currentX][_currentY + 1].Fill = Brushes.LightSkyBlue;
                    }
                    else List[_currentX][_currentY - 1].Fill = Brushes.LightGray;
                }
            } 
            else if (e.Key == Key.Down)
            {
                if (_currentY < _map.Count - 1)
                {
                    if (_map[_currentX][_currentY + 1] == true)
                    {
                        _currentY++;
                        List[_currentX][_currentY].Fill = Brushes.LightGreen;
                        List[_currentX][_currentY - 1].Fill = Brushes.LightSkyBlue;
                    }
                    else List[_currentX][_currentY + 1].Fill = Brushes.LightGray;
                }
            } 
            else if ((e.Key == Key.Left)) 
            {
                if (_currentX > 0)
                {
                    if (_map[_currentX - 1][_currentY] == true)
                    {
                        _currentX--;
                        List[_currentX][_currentY].Fill = Brushes.LightGreen;
                        List[_currentX + 1][_currentY].Fill = Brushes.LightSkyBlue;
                    }
                    else List[_currentX - 1][_currentY].Fill = Brushes.LightGray;
                }
            } 
            else if ((e.Key == Key.Right)) 
            {
                if (_currentX < _map.Count - 1)
                {
                    if (_map[_currentX + 1][_currentY] == true)
                    {
                        _currentX++;
                        List[_currentX][_currentY].Fill = Brushes.LightGreen;
                        List[_currentX - 1][_currentY].Fill = Brushes.LightSkyBlue;
                    }
                    else List[_currentX + 1][_currentY].Fill = Brushes.LightGray;
                }
            }
        }

        private int Fibo(int x, int y)
        {
            int RecursiveX = x;
            int RecursiveY = y; 
            if (RecursiveX > 0 && _map[RecursiveX - 1][RecursiveY] == true && _haveChecked[RecursiveX - 1, RecursiveY] == false) 
            {
                _counter++;
                _haveChecked[RecursiveX - 1, RecursiveY] = true;
                _counter = Fibo(RecursiveX - 1, RecursiveY); 
            }
            if (RecursiveY > 0 && _map[RecursiveX][RecursiveY - 1] == true && _haveChecked[RecursiveX, RecursiveY - 1] == false) 
            {
                _counter++;
                _haveChecked[RecursiveX, RecursiveY - 1] = true;
                _counter = Fibo(RecursiveX, RecursiveY - 1); 
            }
            if (RecursiveX < _map[0].Count - 1 && _map[RecursiveX + 1][RecursiveY] == true && _haveChecked[RecursiveX + 1, RecursiveY] == false)  
            {
                _counter++;
                _haveChecked[RecursiveX + 1, RecursiveY] = true;
                _counter =Fibo(RecursiveX + 1, RecursiveY); 
            }
            if (RecursiveY < _map[0].Count - 1 && _map[RecursiveX][RecursiveY + 1] == true && _haveChecked[RecursiveX, RecursiveY + 1] == false) 
            {
                _counter++;
                _haveChecked[RecursiveX, RecursiveY + 1] = true;
                _counter = Fibo(RecursiveX, RecursiveY + 1); 
            }
            
            return _counter;
        }
    }
}