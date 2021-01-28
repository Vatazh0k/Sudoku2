using Sudoku2.Model;
using Sudoku2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Sudoku2.BuisnessLogic
{
    public static class GameSolution 
    {
        public static List<string> FieldSolution(FieldModel model, List<string> field)
        {
            #region private Data
            int nullElemntsCount = 0;
            int nullElementCountAfterFieldFilling = 0;
            bool readyField = false;
            bool sameElement = false;
            int iterationCount = 0;
            #endregion

            TempField = CellsAssign(TempField, field);
            nullElemntsCount = NullElemntsCount(nullElemntsCount);

            while (true)
            {
                bool notCorrectField = SolutionAlgorithm(sameElement);
                nullElementCountAfterFieldFilling = CheckTheCorectFieldCells(ref readyField, nullElementCountAfterFieldFilling);
                if (readyField) break;

                iterationCount++;
                if (iterationCount > 9 || notCorrectField)
                {
                    MessageBox.Show(" Somthing went wrong. .  .\n Maybe you have a mistake", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
            }

            if(readyField)
            field = CellsAssign(field, TempField);

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
                    if (CurrentElement.ToString() == Arr[((CurrentRow / 3) * 3) + k, ((CurrentColumn / 3) * 3) + n] &&
                        Arr[((CurrentRow / 3) * 3) + k, ((CurrentColumn / 3) * 3) + n] != null)
                    {
                        SameElemntInSquar = true;
                        break;
                    }
                 
                }

            }
            #endregion

            if (!SameElemntInRow && !SameElemntInColumn && !SameElemntInSquar)
                return false;
            return true;
        }

        private static string[,] CellsAssign(string[,] assignableArray, List<string> assignArray)
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
        private static List<string> CellsAssign(List<string> assignArray, string[,] assignableArray)
        {
            for (int i = 0; i < assignableArray.GetLength(0); i++)
            {
                for (int j = 0; j < assignableArray.GetLength(1); j++)
                {
                    assignArray[i * 9 + j] = assignableArray[i, j];
                }
            }
            return assignArray;
        }
        private static bool SolutionAlgorithm(bool sameElement)
        {
            int CurrentNewElemnt;
            string FixedCurrentElement = null;
            bool CanAssignMoreThanOneElement;


            for (int i = 0; i < TempField.GetLength(0); i++)
            {
                for (int j = 0; j < TempField.GetLength(1); j++)
                {
                    if (TempField[i, j] != null)
                    {
                        CurrentNewElemnt = Convert.ToInt32(TempField[i, j]);
                        TempField[i, j] = null;
                        sameElement = SearchSameElemnt(TempField, i, j, CurrentNewElemnt);
                        if (sameElement is true)
                        {
                            return true;
                        }
                        TempField[i, j] = CurrentNewElemnt.ToString();
                        continue;
                    }

                    CurrentNewElemnt = 0;
                    CanAssignMoreThanOneElement = false;

                    while (true)
                    {

                        CurrentNewElemnt++;
                        if (CurrentNewElemnt == 10)
                            break;


                        sameElement = SearchSameElemnt(TempField, i, j, CurrentNewElemnt);

                        if (sameElement is true)
                        {
                            continue;
                        }
                        else
                        {
                            if (CanAssignMoreThanOneElement is true)
                            {
                                FixedCurrentElement = null;
                                break;
                            }
                            else if (CanAssignMoreThanOneElement is false)
                            {
                                FixedCurrentElement = CurrentNewElemnt.ToString();
                                CanAssignMoreThanOneElement = true;
                            }
                        }





/*
                        if (TempField[i, j] is null)
                        {
                            TempField[i, j] = CurrentNewElemnt.ToString();
                        }
                        else
                        {
                            TempField[i, j] = null;
                            break;
                        }
*/
                    }

                    TempField[i, j] = FixedCurrentElement;
                    FixedCurrentElement = null;
                }
            }
            return false;
        }
       
        private static int NullElemntsCount(int nullElementsCount)
        {
            nullElementsCount = 0;
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
        private static int CheckTheCorectFieldCells(ref bool readyField, int nullElementCountAfterFieldFilling)
        {
            readyField = true;
            nullElementCountAfterFieldFilling = 0;
            for (int i = 0; i < TempField.GetLength(0); i++)
            {
                for (int j = 0; j < TempField.GetLength(1); j++)
                {
                    if (TempField[i, j] == null)
                    {
                        readyField = false;
                        nullElementCountAfterFieldFilling++;
                    }
                }
            }
            return nullElementCountAfterFieldFilling;
        }
        #endregion

    } 
}
          