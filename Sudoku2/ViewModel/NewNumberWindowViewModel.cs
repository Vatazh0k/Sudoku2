using Sudoku2.BuisnessLogic;
using Sudoku2.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Sudoku2.ViewModel
{
    public class NewNumberWindowViewModel
    {
        #region Data

        private StartGamePageViewModel vm;
        private string cellNumber;

        public ICommand Numeral { get; set; }
        public ICommand CleanCommand { get; set; }

        #endregion
        public NewNumberWindowViewModel(StartGamePageViewModel StartGamePageViewModel, string cellNumber)
        {
            #region Data
            this.vm = StartGamePageViewModel;
            this.cellNumber = cellNumber;
            #endregion

            #region Commands
            CleanCommand = new Command(CleanCommandAction, CanUseCleanCommand);
            Numeral = new Command(ChangeNumberCommandExecuted, CanUseCommandExecute);
            #endregion
        }


        #region CanUseCommand
        private bool CanUseCommandExecute(object p) => true;                              
        private bool CanUseCleanCommand(object p) => true;

        #endregion

        #region CommandsActions
        private void CleanCommandAction(object p)
        {
            int Cell = Convert.ToInt32(cellNumber);
            vm.Color[Cell] = new SolidColorBrush(Colors.White);
            vm.CellsArray[Cell] = null;
            vm.ElementChooseWindow.Close();
        }
        private void ChangeNumberCommandExecuted(object p)
        {
            int Cell = Convert.ToInt32(cellNumber);

            #region Data
            bool isFullField = false;
            bool isGameWinning = false;
            #endregion

            isFullField = NumberAssignment.NewNumberAssignment(Cell, vm, p);

            if (isFullField)
                isGameWinning = IsFieldReady(vm.Color);

            if(!isGameWinning && isFullField)
                MessageBox.Show("SomtingWrong, try to change...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            if (isGameWinning)
            {
                vm.ElementChooseWindow.Close();
                vm.timer.Stop();
                MessageBox.Show("You win!", "Congratulations", MessageBoxButton.OK);
                return;
            }

            vm.ElementChooseWindow.Close();

        }

        #endregion

        #region PrivateMethods
        private bool IsFieldReady(ObservableCollection<Brush> color)
        {
            for (int i = 0; i < 81; i++)
            {
                if (color[i].ToString() == Brushes.Red.ToString())
                    return false;
            }

            return true;
        }

        #endregion
    }
}   