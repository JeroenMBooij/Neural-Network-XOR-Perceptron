using NNOne.Logic;
using System.Reflection;
using System.Windows;

namespace NNOne.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public NetworkVM NetworkVM { get; set; }
        private Network _network;

        public MainWindow()
        {
            InitializeComponent();

            _network = new Network();
            NetworkVM = new NetworkVM(_network);

            DataContext = NetworkVM;

        }

        private void RunNetwork_Click(object sender, RoutedEventArgs e)
        {
            _network.UpdateNetwork(NetworkVM.SelectedIndex);

            foreach (PropertyInfo prop in NetworkVM.GetType().GetProperties())
            {
                NetworkVM.OnPropertyChanged(prop.Name);
            }
        }

        private void TrainNetwork_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < NetworkVM.TrainingAmount; i++)
            {
                _network.TrainNetworkBatchWise();
            }

            _network.UpdateNetwork(NetworkVM.SelectedIndex);

            foreach (PropertyInfo prop in NetworkVM.GetType().GetProperties())
            {
                NetworkVM.OnPropertyChanged(prop.Name);
            }
        }

    }
}
