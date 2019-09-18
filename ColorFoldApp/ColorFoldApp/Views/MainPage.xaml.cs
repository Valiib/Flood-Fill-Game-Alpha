using System;
using System.Collections.ObjectModel;
using System.Linq;
using ColorFoldApp.Models;
using ColorFoldApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Color = System.Drawing.Color;

namespace ColorFoldApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
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

        public MainPage()
        {
            ViewModel = new MainViewModel();
            ActiveSquares = new ObservableCollection<SquareModel>();
            InitializeComponent();
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ActiveSquares.Add(ViewModel.Squares[0][0]);

            DrawMainBoard();
        }

        private void DrawMainBoard()
        {
            var gameGrid = new Grid();
            gameGrid.WidthRequest = ViewModel.Squares.Count * 32;
            gameGrid.HorizontalOptions = LayoutOptions.Center;
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
                    var boxView = new BoxView {HeightRequest = 30, Margin = 1, WidthRequest = 30};

                    tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;

                    boxView.GestureRecognizers.Add(tapGestureRecognizer);

                    boxView.BindingContext = ViewModel.Squares[i][j];
                    boxView.SetBinding(BackgroundColorProperty, "SqColor");

                    gameRow.Children.AddHorizontal(boxView);
                }

                gameGrid.Children.AddVertical(gameRow);
            }

            GameLayout.Children.Add(gameGrid);
            FindMyNeighboursByColor();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var color = ((BoxView) sender).BackgroundColor;
            if (((Color)color).ToArgb() != ViewModel.Squares[0][0].SqColor.ToArgb())
                Counter++;

            ViewModel.Squares[0][0].SqColor = color;
            
           
            FindMyNeighboursByColor();
            foreach (var row in ViewModel.Squares)
            foreach (var item in row)
                if (item.IsFocused)
                    item.SqColor = color;

           
            if (ActiveSquares.Count == (int) Math.Pow(ViewModel.Squares.Count, 2) + 1)
                DisplayAlert("You have won!", "", "Ok");
        }

        private void FindMyNeighboursByColor()
        {
            foreach (var item in ActiveSquares.ToList())
                CheckNeighbors(item.Position.x, item.Position.y);
        }


        protected bool CheckNeighbors(int x, int y)
        {
            foreach (var coord in MainViewModel.Cords)
                if (ViewModel != null && coord.x + x >= 0 && coord.y + y >= 0 &&
                    ViewModel.Squares.Count - 1 >= coord.y + y && ViewModel.Squares.Count - 1 >= coord.x + x)
                {
                    var currentSquare = ViewModel.Squares[x][y];
                    var checkingSquare = ViewModel.Squares[coord.x + x][coord.y + y];
                    if (!checkingSquare.IsFocused && checkingSquare != null && checkingSquare.SqColor.ToArgb() ==
                        ViewModel.Squares[0][0].SqColor.ToArgb())
                    {
                        ViewModel.Squares[coord.x + x][coord.y + y].IsFocused = true;
                        ActiveSquares.Add(checkingSquare);
                        
                        CheckNeighbors(coord.x + x, coord.y + y);
                    }
                }
            return true;
            
        }

     

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            ActiveSquares = new ObservableCollection<SquareModel>();
            var random = new Random();
            Color[] listColors = {Color.Yellow, Color.Green, Color.Red, Color.Blue};
            var customSize = 10;
            for (var i = 0; i < customSize; i++)
            for (var j = 0; j < customSize; j++)
            {
                ViewModel.Squares[i][j].SqColor = listColors[random.Next(listColors.Length)];
                ViewModel.Squares[i][j].IsFocused = false;
            }

            ActiveSquares.Add(ViewModel.Squares[0][0]);
            Counter = 0;
            CheckNeighbors(0, 0);
        }
    }
}