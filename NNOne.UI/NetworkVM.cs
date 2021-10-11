using NNOne.Logic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NNOne.UI
{
    public class NetworkVM : INotifyPropertyChanged
    {
        public NetworkVM(Network network)
        {
            _network = network;

            SelectedIndex = 0;
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _selectedIndex;

        public int TrainingAmount
        {
            get => _trainingAmount;
            set
            {
                if (_trainingAmount != value)
                {
                    _trainingAmount = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _trainingAmount = 1200;

        public string a00
        {
            get => _network.a0[0].ToString();
        }
        public string a01
        {
            get => _network.a0[1].ToString();
        }

        public string a10
        {
            get => _network.a1[0].ToString();
        }
        public string a11
        {
            get => _network.a1[1].ToString();
        }
        public string a12
        {
            get => _network.a1[2].ToString();
        }
        public string a13
        {
            get => _network.a1[3].ToString();
        }

        public string a20
        {
            get => _network.a2[0].ToString();
        }

        public string w100
        {
            get => _network.w1[0, 0].ToString();
        }
        public string w101
        {
            get => _network.w1[0, 1].ToString();
        }
        public string w110
        {
            get => _network.w1[1, 0].ToString();
        }
        public string w111
        {
            get => _network.w1[1, 1].ToString();
        }
        public string w120
        {
            get => _network.w1[2, 0].ToString();
        }
        public string w121
        {
            get => _network.w1[2, 1].ToString();
        }
        public string w130
        {
            get => _network.w1[3, 0].ToString();
        }
        public string w131
        {
            get => _network.w1[3, 1].ToString();
        }

        public string w200
        {
            get => _network.w2[0, 0].ToString();
        }
        public string w201
        {
            get => _network.w2[0, 1].ToString();
        }
        public string w202
        {
            get => _network.w2[0, 2].ToString();
        }
        public string w203
        {
            get => _network.w2[0, 3].ToString();
        }


        public string b10
        {
            get => _network.b1[0].ToString();
        }
        public string b11
        {
            get => _network.b1[1].ToString();
        }
        public string b12
        {
            get => _network.b1[2].ToString();
        }
        public string b13
        {
            get => _network.b1[3].ToString();
        }


        public string b20
        {
            get => _network.b2[0].ToString();
        }

        public string Cost
        {
            get => _network.Cost.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly Network _network;
    }
}
