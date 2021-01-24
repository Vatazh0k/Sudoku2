using Microsoft.Win32;
using Sudoku2.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sudoku2.BuisnessLogic
{
    public static class FileReader
    {
        public static FieldModel ReadFromFile()
        {
            FieldModel model = new FieldModel();
            string numbersFromFile = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "txt Files|*.txt|All files|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                var FilePath = openFileDialog.FileName;
                using (StreamReader sr = new StreamReader(FilePath))
                {
                    try
                    {
                        numbersFromFile = sr.ReadToEnd();
                    }
                    catch (Exception)
                    { 
                        return null;
                    }
                }
            }

            if (numbersFromFile is null) return null;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    try
                    {
                        if (numbersFromFile[(i * 9 + j)] == '*')
                            model.Field[i, j] = null;
                        else
                            model.Field[i, j] = numbersFromFile[(i * 9 + j)].ToString();
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }

            return model;
        }
    }
}
 