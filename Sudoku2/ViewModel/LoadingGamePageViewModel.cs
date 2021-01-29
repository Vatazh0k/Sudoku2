using Sudoku2.BuisnessLogic;
using Sudoku2.Model;
using Sudoku2.Resource;
using Sudoku2.Resources;
using Sudoku2.View.Pages;
using Sudoku2.View.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sudoku2.ViewModel
{
    public class LoadingGamePageViewModel : ViewModelBase
    {
        #region Data

        private FieldModel model;
        private Page StartGamePage;
        private MainWindowViewModel vm;
        private bool readyField = false;
   
        public ICommand LoadGameCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand RulesCommand { get; set; }
        #endregion

        public LoadingGamePageViewModel(MainWindowViewModel vm)
        {
            this.vm = vm;

            #region Commands
            LoadGameCommand = new Command(LoadGameCommandAction, CanUseLoadGameCommand);
            ExitCommand = new Command(ExitCommandAction, CanUseExitCommand);
            RulesCommand = new Command(RulesCommandAction, CanUseRulesCommand);
            #endregion
        }

        #region CanUseCommands
        private bool CanUseStartCommand(object p) => readyField;
        private bool CanUseLoadGameCommand(object p) => true; 
        private bool CanUseExitCommand(object p) => true;
        private bool CanUseRulesCommand(object p) => true;

        #endregion

        #region CommandsActions
        private void ExitCommandAction(object p)
        {
            vm.mainWindow.Close();
        }
        private void LoadGameCommandAction(object p)
        {
            model = FileReader.ReadFromFile();

            if (model is null)
            {
                readyField = false;
                MessageBox.Show("Incorrect field!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            readyField = true;
            StartGamePage = new StartGamePage(model);

            vm.CurrentPage = StartGamePage;

        }
        private void RulesCommandAction(object p)
        {
            RulesWindow rulesWindow = new RulesWindow();
            rulesWindow.ShowDialog();
        }

        #endregion
    }
}   
      