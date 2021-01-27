using Sudoku2.Model;
using Sudoku2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sudoku2.View.Pages
{
    public partial class StartGamePage : Page
    {
        private Button[,] button = new Button[9, 9];

        public StartGamePage(FieldModel model)
        {
            var vm = new StartGamePageViewModel(this, model);
            DataContext = vm;

            InitializeComponent();

            #region Date
            double MarginBottom = 0;
            double MarginRight = 0;
            #endregion

            CreatingButtons(MarginRight, MarginBottom, vm);
        }

        #region PrivateMethods
        private void GridCreating()
        {
            ColumnDefinition column = new ColumnDefinition();
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(40);
            column.Width = new GridLength(40);
            Field.ColumnDefinitions.Add(column);
            Field.RowDefinitions.Add(row);
        }

        private void CreatingButtons(double MarginRight, double MarginBottom, StartGamePageViewModel vm)
        {
            for (int i = 1; i <= 9; i++)
            {
                for (int j = 1; j <= 9; j++)
                {
                    GridCreating();


                    if (i % 3 == 0 && j % 3 != 0)
                        ButtonParamets(i, j, MarginRight = 1.5, MarginBottom = 4, vm);


                    else if (i % 3 == 0 && j % 3 == 0)
                        ButtonParamets(i, j, MarginRight = 4, MarginBottom = 4, vm);


                    else if (j % 3 == 0)
                        ButtonParamets(i, j, MarginRight = 4, MarginBottom = 1.5, vm);


                    else
                        ButtonParamets(i, j, MarginRight = 1.5, MarginBottom = 1.5, vm);

                }
            }

        }

        private void ButtonParamets(int i, int j, double a, double b, StartGamePageViewModel vm)
        {
            button[i - 1, j - 1] = new Button();

            button[i - 1, j - 1].Name = $"C{(i-1) * 9 + (j-1)}";
            button[i - 1, j - 1].Width = 40;
            button[i - 1, j - 1].Height = 40;
            button[i - 1, j - 1].BorderBrush = Brushes.DarkBlue;
            button[i - 1, j - 1].BorderThickness = new Thickness(1);
            button[i - 1, j - 1].Margin = new Thickness(1, 1, a, b);

            Binding colorBinding = new Binding();
            colorBinding.Source = vm;
            colorBinding.Path = new PropertyPath($"Color[{(i-1) * 9 + (j-1)}]");
            colorBinding.Mode = BindingMode.OneWay;
            button[i - 1, j - 1].SetBinding(Button.ForegroundProperty, colorBinding);

            Binding binding = new Binding();
            binding.Source = vm;
            binding.Path = new PropertyPath($"CellsArray[{(i-1) * 9 + (j-1)}]");
            binding.Mode = BindingMode.OneWay;

            button[i - 1, j - 1].SetBinding(Button.ContentProperty, binding);

            if (button[i - 1, j - 1].Content is null)
            {
                button[i - 1, j - 1].Command = vm.NewNumberAssigningCommand;
                button[i - 1, j - 1].CommandParameter = button[i - 1, j - 1].Name;
            }
            else
            {
                button[i - 1, j - 1].FontWeight = FontWeights.Bold;
            }


            Grid.SetRow(button[i - 1, j - 1], i - 1);
            Grid.SetColumn(button[i - 1, j - 1], j - 1);
            Field.Children.Add(button[i - 1, j - 1]);
        }
        #endregion
    }
}
 