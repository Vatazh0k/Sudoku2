using Sudoku2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sudoku2.View.Pages
{
    /// <summary>
    /// Interaction logic for LoadingGamePage.xaml
    /// </summary>
    public partial class LoadingGamePage : Page
    {
        public LoadingGamePage(MainWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = new LoadingGamePageViewModel(vm);
        }
    }
}
