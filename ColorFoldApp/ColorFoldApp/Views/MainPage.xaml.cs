using System;
using System.Collections.ObjectModel;
using System.Linq;
using ColorFoldApp.Models;
using ColorFoldApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static ColorFoldApp.App;
using static ColorFoldApp.ViewModels.MainViewModel;
using Color = System.Drawing.Color;

namespace ColorFoldApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        //Variables that are linked with view
        private int _counter;

        public int Counter
        {
            get => _counter;
            set
            {
                _counter = value;
                CounterScreen.Text = _counter.ToString();
            }
        }

        private ObservableCollection<SquareModel> ActiveSquares { get; set; }

        public MainViewModel ViewModel { get; set; }

        //CTOR
        public MainPage()
        {
            ViewModel = new MainViewModel();
            ActiveSquares = new ObservableCollection<SquareModel>();
            InitializeComponent();
            BindingContext = ViewModel;
            ActiveSquares.Add(ViewModel.Squares[0][0]);
            DrawMainBoard();
        }

        private void DrawMainBoard()
        {
            var gameGrid = new Grid();
            gameGrid.WidthRequest = ViewModel.Squares.Count * (((ScreenWidth * 90) / 100) / ViewModel.BoardSize); ;
            gameGrid.HorizontalOptions = LayoutOptions.StartAndExpand;
            gameGrid.VerticalOptions = LayoutOptions.Center;

            for (var i = 0; i < ViewModel.Squares.Count; i++)
            {
                gameGrid.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
                gameGrid.RowSpacing = 0;
                var gameRow = new Grid();

                for (var j = 0; j < ViewModel.Squares[i].Count; j++)
                {
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    gameRow.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Auto});
                    gameRow.ColumnSpacing = 0;
                    var BoxSize = ((ScreenWidth *90)/100)/ ViewModel.BoardSize;
                    var boxView = new BoxView {HeightRequest = BoxSize , WidthRequest = BoxSize };
                    tapGestureRecognizer.Tapped += SelectAnotherColor;
                    boxView.GestureRecognizers.Add(tapGestureRecognizer);
                    boxView.BindingContext = ViewModel.Squares[i][j];
                    boxView.SetBinding(BackgroundColorProperty, "SqColor");
                    gameRow.Children.AddHorizontal(boxView);
                }

                gameGrid.Children.AddVertical(gameRow);
            }

            GameLayout.Children.Add(gameGrid);
            CheckAllNeighbours();
        }

        private void SelectAnotherColor(object sender, EventArgs e)
        {
            var color = ((BoxView) sender).BackgroundColor;
            if (((Color) color).ToArgb() != ViewModel.Squares[0][0].SqColor.ToArgb())
                Counter++;

            ViewModel.Squares[0][0].SqColor = color;


            CheckAllNeighbours();
            foreach (var row in ViewModel.Squares)
            foreach (var item in row)
                if (item.IsFocused)
                    item.SqColor = color;


            if (Math.Pow(ViewModel.Squares.Count,2).Equals(ActiveSquares.Count) )
                DisplayAlert("You have won!", "", "Ok");
        }

        private void CheckAllNeighbours()
        {
            var index = 0;
            var length = ActiveSquares.Count;
            while (index != length)
            {
                CheckNeighbors(ActiveSquares[index].Position.x, ActiveSquares[index].Position.y);
                length = ActiveSquares.Count;
                index++;
            }
        }


        protected bool CheckNeighbors(int x, int y)
        {
            foreach (var coord in Cords)
                if (ViewModel != null && coord.x + x >= 0 && coord.y + y >= 0 &&
                    ViewModel.Squares.Count - 1 >= coord.y + y && ViewModel.Squares.Count - 1 >= coord.x + x)
                {
                    var checkingSquare = ViewModel.Squares[coord.x + x][coord.y + y];
                    if (!checkingSquare.IsFocused && checkingSquare != null && checkingSquare.SqColor.ToArgb() ==
                        ViewModel.Squares[0][0].SqColor.ToArgb())
                    {
                        ViewModel.Squares[coord.x + x][coord.y + y].IsFocused = true;
                        ActiveSquares.Add(checkingSquare);
                    }
                }

            return true;
        }


        private void RestartGame(object sender, EventArgs e)
        {
            ActiveSquares = new ObservableCollection<SquareModel>();


            for (var i = 0; i < ViewModel.BoardSize; i++)
            for (var j = 0; j < ViewModel.BoardSize; j++)
            {
                ViewModel.Squares[i][j].SqColor = Colors[RandomNumber.Next(Colors.Count)];
                ViewModel.Squares[i][j].IsFocused = false;
            }

            ViewModel.Squares[0][0].IsFocused = true;
            ActiveSquares.Add(ViewModel.Squares[0][0]);
            Counter = 0;
            CheckAllNeighbours();
        }
    }
}
