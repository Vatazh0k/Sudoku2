using Sudoku2.Model;
using Sudoku2.Resource;
using Sudoku2.Resources;
using Sudoku2.View.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sudoku2.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Data

        private Page LoadingGamePage;

        private Page _CurrentPage;

        public MainWindow mainWindow { get; set; }
        public ICommand ExitCommand { get; set; }
        public Page CurrentPage
        {
            get { return _CurrentPage; }
            set => Set(ref _CurrentPage, value);
        }

        #endregion

        public MainWindowViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            LoadingGamePage = new LoadingGamePage(this);

            CurrentPage = LoadingGamePage;

            ExitCommand = new Command(ExitCommandAction, CanUseExitCommand);
        }

        private bool CanUseExitCommand(object p) => true;

        private void ExitCommandAction(object p)
        {
            mainWindow.Close();
        }
    }
}
  