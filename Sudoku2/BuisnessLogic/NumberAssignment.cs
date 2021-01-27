using Sudoku2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Sudoku2.BuisnessLogic
{
    public static class NumberAssignment
    {
        public static bool NewNumberAssignment(int Cell, StartGamePageViewModel vm, object p)
        {
            #region Data
            bool isSameElement = false;
            string[,] tempField = new string[9, 9];
            #endregion  
              
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    tempField[i, j] = vm.CellsArray[i * 9 + j];
                }
            }

            vm.Color[Cell] = new SolidColorBrush(Colors.AntiqueWhite);
            vm.CellsArray[Cell] = p.ToString();

            bool CicleEnd = false;
            for (int i = 0, CellSearch = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++, CellSearch++)
                {
                    if (CellSearch == Cell)
                    {
                        tempField[i, j] = null;
                        CicleEnd = true;
                        isSameElement = GameSolution.SearchSameElemnt(tempField, i, j, Convert.ToInt32(p));
                        break;
                    }
                }
                if (CicleEnd) break;
            }

            if (isSameElement is true)
                vm.Color[Cell] = new SolidColorBrush(Colors.Red);

            for (int i = 0; i < 81; i++)
                if (vm.CellsArray[i] is null)
                    return false;
            return true;


        }

    }
}
 