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
            bool correctAssignment = false;
            #endregion

            var listElementsCount = Enumerable.Range(0, 81).Select(i => vm.CellsArray[i]);
            List<string> TempList = new List<string>(listElementsCount);
            
            correctAssignment = NumberAssignment.NewNumberAssignment(Cell, TempList, p);
            if (correctAssignment is false)
            {
                vm.CellsArray[Cell] = p.ToString();
                vm.Color[Cell] = new SolidColorBrush(Colors.Red);
            }
            else if (correctAssignment is true)
            {
                vm.CellsArray[Cell] = p.ToString();
                vm.Color[Cell] = new SolidColorBrush(Colors.AntiqueWhite);
            }

            isFullField = isFieldHasNullElements();
              
            if (isFullField)
                isGameWinning = CheckTheCorrectCells(vm.Color);

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
        private bool isFieldHasNullElements()
        {
            for (int i = 0; i < 81; i++)
                if (vm.CellsArray[i] is null)
                    return false;
            return true;

        }
        private bool CheckTheCorrectCells(ObservableCollection<Brush> color)
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