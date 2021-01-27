using Sudoku2.Model;
using Sudoku2.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Sudoku2.BuisnessLogic
{
    public static class GameSolution 
    {
        public static ObservableCollection<string> FieldSolution(FieldModel model, ObservableCollection<string> field, ObservableCollection<Brush> color)
        {
            #region private Data
            int nullElemntsCount = 0;
            int nullElementCountAfterFieldFilling = 0;
            bool readyField = false;
            bool sameElement = false;

            #endregion

            TempField = CellsAssign(TempField, field);
            nullElemntsCount = NullElemntsCount(nullElemntsCount);

            while (true)
            {
                SolutionAlgorithm(sameElement);
                CheckTheCorectFieldCells(ref readyField, ref nullElementCountAfterFieldFilling);
                if (readyField) break;

                //TODO: if (nullElemntsCount == nullElementCountAfterFieldFilling) {new algo}
            }

            field = CellsAssign(field, TempField, color);

            return field;
        }
     

        #region PrivateMethods
        private static string[,] TempField = new string[9, 9];


        public static bool SearchSameElemnt(string[,] Arr, int CurrentRow, int CurrentColumn, int CurrentElement)
        {
            bool SameElemntInRow = false;
            bool SameElemntInColumn = false;
            bool SameElemntInSquar = false;

            #region Row
            for (int i = 0; i < Arr.GetLength(0); i++)
            {
                if (CurrentElement.ToString() == Arr[i, CurrentColumn] && Arr[i, CurrentColumn] != null)
                {
                    SameElemntInRow = true;
                    break;
                }
            }
            #endregion
            #region Column
            for (int i = 0; i < Arr.GetLength(0); i++)
            {
                if (CurrentElement.ToString() == Arr[CurrentRow, i] && Arr[CurrentRow, i] != null)
                {
                    SameElemntInColumn = true;
                    break;
                }
            }
            #endregion
            #region Squar
            for (int k = 0; k < 3; k++)
            {
                for (int n = 0; n < 3; n++)
                {

                    if (SearchSameElemntInSquar(CurrentElement, CurrentRow, ref CurrentColumn, ref Arr, k, n, 1.4, 1.3, 1.4, 1.3, ref SameElemntInSquar)) break;

                    if (SearchSameElemntInSquar(CurrentElement, CurrentRow, ref CurrentColumn, ref Arr, k - 1, n - 1, 1.7, 1.6, 1.7, 1.6, ref SameElemntInSquar)) break;

                    if (SearchSameElemntInSquar(CurrentElement, CurrentRow, ref CurrentColumn, ref Arr, k - 2, n - 2, 1.1, 0.9, 1.1, 0.9, ref SameElemntInSquar)) break;


                    if (SearchSameElemntInSquar(CurrentElement, CurrentRow, ref CurrentColumn, ref Arr, k, n - 1, 1.4, 1.2, 1.7, 1.6, ref SameElemntInSquar)) break;

                    if (SearchSameElemntInSquar(CurrentElement, CurrentRow, ref CurrentColumn, ref Arr, k, n - 2, 1.4, 1.2, 1.1, 0.9, ref SameElemntInSquar)) break;


                    if (SearchSameElemntInSquar(CurrentElement, CurrentRow, ref CurrentColumn, ref Arr, k - 1, n, 1.7, 1.6, 1.4, 1.3, ref SameElemntInSquar)) break;

                    if (SearchSameElemntInSquar(CurrentElement, CurrentRow, ref CurrentColumn, ref Arr, k - 1, n - 2, 1.7, 1.6, 1.1, 0.9, ref SameElemntInSquar)) break;


                    if (SearchSameElemntInSquar(CurrentElement, CurrentRow, ref CurrentColumn, ref Arr, k - 2, n, 1.1, 0.9, 1.4, 1.3, ref SameElemntInSquar)) break;

                    if (SearchSameElemntInSquar(CurrentElement, CurrentRow, ref CurrentColumn, ref Arr, k - 2, n - 1, 1.1, 0.9, 1.7, 1.6, ref SameElemntInSquar)) break;

                }

            }
            #endregion

            if (!SameElemntInRow && !SameElemntInColumn && !SameElemntInSquar)
                return false;
            return true;
        }

        private static string[,] CellsAssign(string[,] assignableArray, ObservableCollection<string> assignArray)
        {
            for (int i = 0; i < assignableArray.GetLength(0); i++)
            {
                for (int j = 0; j < assignableArray.GetLength(1); j++)
                {
                    assignableArray[i, j] = assignArray[i * 9 + j];
                }
            }
            return assignableArray;
        }
        private static ObservableCollection<string> CellsAssign(ObservableCollection<string> assignArray, string[,] assignableArray, ObservableCollection<Brush> color)
        {
            for (int i = 0; i < assignableArray.GetLength(0); i++)
            {
                for (int j = 0; j < assignableArray.GetLength(1); j++)
                {
                    color[i * 9 + j] = new SolidColorBrush(Colors.AntiqueWhite);
                    assignArray[i * 9 + j] = assignableArray[i, j];
                }
            }
            return assignArray;
        }
        private static void SolutionAlgorithm(bool sameElement)
        {

            for (int i = 0; i < TempField.GetLength(0); i++)
            {
                for (int j = 0; j < TempField.GetLength(1); j++)
                {
                    if (TempField[i, j] != null) continue;


                    int CurrentNewElemnt = 0;

                    while (true)
                    {

                        CurrentNewElemnt++;
                        if (CurrentNewElemnt == 10)
                            break;


                        sameElement = SearchSameElemnt(TempField, i, j, CurrentNewElemnt);

                        if (sameElement) continue;


                        if (!sameElement && TempField[i, j] is null)
                            TempField[i, j] = CurrentNewElemnt.ToString();

                        else
                        {
                            //TODO: Написать второй(запасной) алгоритм по решению row+1 column+1
                            TempField[i, j] = null;
                            break;
                        }

                    }
                }
            }
        }
        private static bool SearchSameElemntInSquar(int CurrentElemnt, int i, ref int j, ref string[,] Field, int k, int n, double x1, double y1, double x2, double y2, ref bool SameElemntInSquar)
        {


            double newI = (Convert.ToDouble(i) + 1) / 3;
            double newJ = (Convert.ToDouble(j) + 1) / 3;

            if ((newI - (int)newI) + 1 < x1 && (newI - (int)newI) + 1 > y1)
            {
                if ((newJ - (int)newJ) + 1 < x2 && (newJ - (int)newJ) + 1 > y2)
                {

                    if (CurrentElemnt.ToString() == Field[i + k, j + n] && Field[i + k, j + n] != null)
                    {
                        SameElemntInSquar = true;
                        return true;
                    }

                }
            }
            return false;
        }
        private static int NullElemntsCount(int nullElementsCount)
        {
            for (int i = 0; i < TempField.GetLength(0); i++)
            {
                for (int j = 0; j < TempField.GetLength(1); j++)
                {
                    if (TempField[i, j] is null)
                        nullElementsCount++;
                }
            }
            return nullElementsCount;
        }
        private static void CheckTheCorectFieldCells(ref bool readyField, ref int nullElementCountAfterFieldFilling)
        {
            readyField = true;
            for (int i = 0; i < TempField.GetLength(0); i++)
            {
                for (int j = 0; j < TempField.GetLength(1); j++)
                {
                    if (TempField[i, j] == null)
                    {
                        readyField = false;
                        nullElementCountAfterFieldFilling = i * 9 + j;
                    }
                }
            }
        }
        #endregion

    } 
}
         