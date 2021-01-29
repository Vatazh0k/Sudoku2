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
        public static bool NewNumberAssignment(int Cell, string[,]CellsArray, object p)
        {
            bool isSameElement = false;

            bool CicleEnd = false;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if ((i*9+j) == Cell)
                    {
                        CellsArray[i, j] = null;
                        CicleEnd = true;
                        isSameElement = GameSolution.SearchSameElemnt(CellsArray, i, j, Convert.ToInt32(p));
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
   