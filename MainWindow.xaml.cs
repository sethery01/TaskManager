using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window // Controller
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnAddTaskClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Add Task Clicked!");
        }

        private void OnEditTaskClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Edit Task Clicked!");
        }

        private void OnCompleteTaskClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Complete Task Clicked!");
        }
    }
}