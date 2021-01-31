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
        public static string[,] FieldSolution(string[,] TempField)
        {
            int nullElemntsCount = 0;
            bool readyField = false;
            string[,] field = new string[9, 9];

            field = CellsAssign(field, TempField);

            nullElemntsCount = 0;

            while(nullElemntsCount != SearchNullElements(nullElemntsCount, TempField))
            {
                nullElemntsCount = SearchNullElements(nullElemntsCount, TempField);

                SolutionAlgorithm(TempField);
    
                if (nullElemntsCount == 0) 
                    readyField = true;
            }

            if (!readyField)
                field = DecisionInEmptyField(field, TempField);


            if (readyField)
                field = CellsAssign(field, TempField);
            return field;
        }

        public static bool SearchSameElemnt(string[,] Arr, int CurrentRow, int CurrentColumn, int CurrentElement)
        {
            bool SameElemntInRow = false;
            bool SameElemntInColumn = false;
            bool SameElemntInSquar = false;


            for (int i = 0; i < Arr.GetLength(0); i++)//row
            {
                if (CurrentElement.ToString() == Arr[i, CurrentColumn] && Arr[i, CurrentColumn] != null)
                {
                    SameElemntInRow = true;
                    break;
                }
            }

            for (int i = 0; i < Arr.GetLength(0); i++)//Column
            {
                if (CurrentElement.ToString() == Arr[CurrentRow, i] && Arr[CurrentRow, i] != null)
                {
                    SameElemntInColumn = true;
                    break;
                }
            }

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
            }//Squar

            if (!SameElemntInRow && !SameElemntInColumn && !SameElemntInSquar)
                return false;
            return true;
        }

        #region PrivateMethods
        private static string[,] DecisionInEmptyField(string[,] Field, string[,] InitialField)
        {
            int FirstNumberToAssigning = 0;
            int NullElementsCount = 0;
            int IterartionCount = 0;
            NullElementsCount = SearchNullElements(NullElementsCount, Field);

            while (IterartionCount != 9)
            {
                Field = CellsAssign(Field, InitialField);

                FirstNumberToAssigning++;

                _ = FirstNumberToAssigning == 10 ?
                    FirstNumberToAssigning = 1 : FirstNumberToAssigning;

                DeepSolutionAlgorithm(FirstNumberToAssigning, Field);

                NullElementsCount = SearchNullElements(NullElementsCount, Field);
                if (NullElementsCount == 0)
                    return Field;

                IterartionCount++;
            }

            MessageBox.Show(" Somthing went wrong. .  .\n Maybe you have a mistake", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return InitialField;
        }
        private static void DeepSolutionAlgorithm(int FirstElementToAssign, string[,] Field)
        {
            bool CanBreakCicle = false;
            bool isSameElementFounded;
            int CurrentElement;
            int CurrentElementItterationCount = 0;

            for (int i = 0; i < 9; i++)
            {
                CurrentElement = FirstElementToAssign;

                for (int j = 0; j < 9; j++)
                {
                    if (CurrentElement == 10)
                        CurrentElement = 1;

                    if (Field[i, j] != null) continue;

                    isSameElementFounded = GameSolution.SearchSameElemnt(Field, i, j, CurrentElement);

                    CurrentElementItterationCount++;
                    if (isSameElementFounded is true)
                    {
                        CurrentElement++;
                        if (CurrentElement == 10)
                            CurrentElement = 1;
                        j--;
                        if (CurrentElementItterationCount == 10)
                        {
                            CanBreakCicle = true;
                            break;
                        }
                        continue;
                    }

                    CurrentElementItterationCount = 0;

                    if (isSameElementFounded is false)
                        Field[i, j] = CurrentElement.ToString();


                    CurrentElement++;
                }
                if (CanBreakCicle) break;
            }
        }
        private static void SolutionAlgorithm(string[,] TempField)
        {
            string FixedCurrentElement = null;
            bool CanAssignMoreThanOneElement;
            bool sameElement = false;

            for (int i = 0; i < TempField.GetLength(0); i++)
            {
                for (int j = 0; j < TempField.GetLength(1); j++)
                {
                    if (TempField[i, j] != null)
                        continue;

                    CanAssignMoreThanOneElement = false;

                    for (int k = 1; k <= 9; k++)
                    {
                        sameElement = SearchSameElemnt(TempField, i, j, k);

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
                                FixedCurrentElement = k.ToString();
                                CanAssignMoreThanOneElement = true;
                            }
                        }
                    }

                    TempField[i, j] = FixedCurrentElement;
                    FixedCurrentElement = null;
                }
            }
        }  
        private static int SearchNullElements(int nullElementsCount, string[,] TempField)
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
        private static string[,] CellsAssign(string[,] assignableArray, string[,] assignArray)
        {
            for (int i = 0; i < assignableArray.GetLength(0); i++)
            {
                for (int j = 0; j < assignableArray.GetLength(1); j++)
                {
                    assignableArray[i, j] = assignArray[i, j];
                }
            }
            return assignableArray;
        }
        #endregion

    } 
}
              