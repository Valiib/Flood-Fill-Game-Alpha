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
        

        public MainViewModel ViewModel { get; set; }

        //CTOR
        public MainPage()
        {
            ViewModel = new MainViewModel();
            InitializeComponent();
            BindingContext = ViewModel;
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
            ViewModel.CheckAllNeighbours();
        }

        private void SelectAnotherColor(object sender, EventArgs e)
        {
            var color = ((BoxView)sender).BackgroundColor;
            if (((Color)color).ToArgb() != ViewModel.Squares[0][0].SqColor.ToArgb())
                ViewModel.Counter++;

            ViewModel.Squares[0][0].SqColor = color;


            ViewModel.CheckAllNeighbours();
            foreach (var row in ViewModel.Squares)
            foreach (var item in row)
                if (item.IsFocused)
                    item.SqColor = color;


            if (Math.Pow(ViewModel.Squares.Count, 2).Equals(ViewModel.ActiveSquares.Count))
                DisplayAlert("You have won!", "", "Ok");
        }


        private void RestartGame(object sender, EventArgs e)
        {
            ViewModel.ActiveSquares = new ObservableCollection<SquareModel>();


            for (var i = 0; i < ViewModel.BoardSize; i++)
            for (var j = 0; j < ViewModel.BoardSize; j++)
            {
                ViewModel.Squares[i][j].SqColor = Colors[RandomNumber.Next(Colors.Count)];
                ViewModel.Squares[i][j].IsFocused = false;
            }

            ViewModel.Squares[0][0].IsFocused = true;
            ViewModel.ActiveSquares.Add(ViewModel.Squares[0][0]);
            ViewModel.Counter = 0;
            ViewModel.CheckAllNeighbours();
        }
    }
}
