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
        public static bool NewNumberAssignment(int Cell, List<string>CellsArray, object p)
        {
            #region Data
            bool isSameElement = false;
            string[,] tempField = new string[9, 9];
            #endregion  
              
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    tempField[i, j] = CellsArray[i * 9 + j];
                }
            }

            CellsArray[Cell] = p.ToString();

            bool CicleEnd = false;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if ((i*9+j) == Cell)
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
                return false;

            return true;
             
        }

    }
}
  