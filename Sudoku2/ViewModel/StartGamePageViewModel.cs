using Sudoku2.BuisnessLogic;
using Sudoku2.Model;
using Sudoku2.Resource;
using Sudoku2.Resources;
using Sudoku2.View.Pages;
using Sudoku2.View.Windows;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Sudoku2.ViewModel
{
    public class StartGamePageViewModel : ViewModelBase
    {
        #region Data
        #region PrivateData
        private StartGamePage startGamePage;
        private FieldModel model;
        private TimeSpan timeSpan;
        private string[,] TempField;
        private string _timer;

        private ObservableCollection<Brush> _color;
        private ObservableCollection<string> _CellsArray;

        #endregion
        #region PublicData
        public bool gameInProccess { get; set; }
        public bool isPause { get; set; }
        public string Timer
        {
            get { return _timer; }
            set => Set(ref _timer, value);
        }
        public DispatcherTimer timer { get; set; }
        public ObservableCollection<Brush> Color
        {
            get { return _color; }
            set => Set(ref _color, value);
        }
        public ObservableCollection<string> CellsArray
        {
            get => _CellsArray;
            set => Set(ref _CellsArray, value);
        }
        public NewNumbersWindow ElementChooseWindow { get; set; }
        #endregion
        #region Commands
        public ICommand ContinueCommand { get; set; }
        public ICommand PauseCommand { get; set; }
        public ICommand NewNumberAssigningCommand { get; set; }
        public ICommand CleanAllCommand { get; set; }
        public ICommand ShowDecision { get; set; }
        public ICommand NewGameCommand { get; set; }
        #endregion
        #endregion 

        public StartGamePageViewModel(StartGamePage startGamePage, FieldModel model)
        {
            #region Data
            this.startGamePage = startGamePage;
            this.model = model;
            #endregion

            #region Commands
            ShowDecision = new Command(ShowDecisionCommandAction, CanUseShowDecisionCommand);
            CleanAllCommand = new Command(CleanAllCommandAction, CanUseCleanCommand);
            ContinueCommand = new Command(ContinueCommandAction, CanUseContinueCommand);
            PauseCommand = new Command(PauseCommandAction, CanUsePauseCommand);
            NewNumberAssigningCommand = new Command(NewNumberAssigningCommandAction, CanUseNewNumberAssigningCommand);
            #endregion

            var _cells = Enumerable.Range(0, 81).Select(i => "");
            var _colors = Enumerable.Range(0, 81).Select(i => new SolidColorBrush(Colors.White));

            _color = new ObservableCollection<Brush>(_colors);
            _CellsArray = new ObservableCollection<string>(_cells);


            StartGame();
        }

        #region CanUseCommands
        private bool CanUseShowDecisionCommand(object p) => gameInProccess && !isPause;
        private bool CanUseCleanCommand(object p) => !isPause && gameInProccess;
        private bool CanUseAllCommand(object p) => true;
        private bool CanUseContinueCommand(object p) => isPause;
        private bool CanUsePauseCommand(object p) => !isPause;
        private bool CanUseNewNumberAssigningCommand(object p) => !isPause && gameInProccess;
        #endregion

        #region CommandsAction
        private void ShowDecisionCommandAction(object p)
        {
            string[,] tempArr = new string[9, 9];
            tempArr = CellsAsignment(tempArr);

            if (!IsAllCellsHasCorrectPositin())
            {
                MessageBox.Show("Somthing went wrong \nMaybe you has misstakes", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                tempArr = GameSolution.FieldSolution(tempArr);
                CellsArray = CellAssignment(tempArr);
            }
        } 
        private void CleanAllCommandAction(object p)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    CellsArray[i * 9 + j] = model.Field[i, j];
                    Color[i * 9 + j] = new SolidColorBrush(Colors.White);
                }
            }
        }
        private void ContinueCommandAction(object p)
        {
            isPause = false;
            timer.Start();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    CellsArray[i * 9 + j] = TempField[i, j];
                }
            }
        }
        private void PauseCommandAction(object p)
        {
            isPause = true;
            timer.Stop();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    TempField[i, j] = CellsArray[i * 9 + j];
                    CellsArray[i * 9 + j] = "";
                }
            }
        }
        private void NewNumberAssigningCommandAction(object p)
        {
            if (p.ToString() is null) return;

            var cellNumber = string.Empty;

            for (int i = 1; i < p.ToString().Length; i++)
            {
                cellNumber += p.ToString()[i];
            }

            var newElWindowViewModel = new NewNumberWindowViewModel(this, cellNumber);

            var newElWindow = new NewNumbersWindow
            {
                DataContext = newElWindowViewModel,
                Owner = Application.Current.MainWindow
            };

            ElementChooseWindow = newElWindow;

            newElWindow.ShowDialog();
        }
        #endregion

        #region PrivateMethods
        private string[,] CellsAsignment(string[,] tempArr)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    tempArr[i, j] = CellsArray[i * 9 + j];
                }
            }
            return tempArr;
        }
        private bool IsAllCellsHasCorrectPositin()
        {
            for (int i = 0; i < 81; i++)
            {
                if (Color[i].ToString() == Brushes.Red.ToString())
                    return false;
            }
            return true;
        }
        private void CollectionCreating()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Color[i * 9 + j] = new SolidColorBrush(Colors.Snow);
                    CellsArray[i * 9 + j] = model.Field[i, j];
                }
            }
        }
        private void Time()
        {
            timer = new DispatcherTimer();
            timeSpan = new TimeSpan();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;
            timer.Start();
        }
        private void TimerTick(object sender, EventArgs e)
        {

            timeSpan += TimeSpan.FromSeconds(1);
            Timer = timeSpan.ToString();
        }
        private void StartGame()
        {
            #region Data
            TempField = new string[9, 9];
            gameInProccess = true;
            isPause = false;
            #endregion
            CollectionCreating();

            if(startGamePage is null)
            startGamePage = new StartGamePage(model);

            Time();

        }
        private ObservableCollection<string> CellAssignment(string[,] tempArr)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if(tempArr[i,j] is null) return CellsArray;
                    CellsArray[i * 9 + j] = tempArr[i, j];
                    Color[i * 9 + j] = new SolidColorBrush(Colors.AntiqueWhite);
                }
            }

            return CellsArray;
        }
        #endregion
    } 
} 
            