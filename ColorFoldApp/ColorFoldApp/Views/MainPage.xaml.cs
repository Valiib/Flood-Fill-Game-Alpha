using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Schema;
using ColorFoldApp.Models;
using ColorFoldApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Color = System.Drawing.Color;

namespace ColorFoldApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {

        public ObservableCollection<SquareModel> ActiveSquares { get; set; }
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

            for (int i = 0; i < ViewModel.Squares.Count; i++)
            {
                gameGrid.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
                gameGrid.RowSpacing = 0;
                var gameRow = new Grid() { };
                for (int j = 0; j < ViewModel.Squares[i].Count; j++)
                {
                    gameRow.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Auto});
                    gameRow.ColumnSpacing = 0;
                    var boxView = new BoxView() {HeightRequest = 30, Margin = 1, WidthRequest = 30};

                    boxView.BindingContext = ViewModel.Squares[i][j];
                    boxView.SetBinding(BackgroundColorProperty, "SqColor");
                    gameRow.Children.AddHorizontal(boxView);
                }

                gameGrid.Children.AddVertical(gameRow);
            }
            this.GameLayout.Children.Add(gameGrid);
            FindMyNeighboursByColor();
        }

        private void FindMyNeighboursByColor()
        {
            foreach (var item in ActiveSquares.ToList())
            {
                CheckNeighbors(item.Position.x, item.Position.y);
            }
        }

   
        protected bool CheckNeighbors(int x, int y)
        {
            foreach (var coord in MainViewModel.Cords)
            {
                if (ViewModel != null && coord.x + x >= 0 && coord.y + y >= 0 &&
                    ViewModel.Squares.Count - 1 >= coord.y + y && ViewModel.Squares.Count - 1 >= coord.x + x)
                {
                    var currentSquare = ViewModel.Squares[x][y];
                    var checkingSquare = ViewModel.Squares[coord.x + x][coord.y + y];
                    if (!checkingSquare.IsFocused  && checkingSquare != null && checkingSquare.SqColor.Name == currentSquare.SqColor.Name)
                    {
                        ViewModel.Squares[coord.x + x][coord.y + y].IsFocused = true;
                        ActiveSquares.Add(checkingSquare);
                        CheckNeighbors(coord.x + x, coord.y + y);
                    }
                }
            }
            return true;
        }

        private void ChangeAllSelectedColors(object sender, EventArgs e)
        {
            var rand = new Random();
            Color[] listColors = { Color.Yellow, Color.Green, Color.Red, Color.Blue };
            var constIndex = rand.Next(listColors.Length);
            FindMyNeighboursByColor();
            foreach (var row in ViewModel.Squares)
            {
                foreach (var item in row)
                    if (item.IsFocused)
                        item.SqColor = listColors[constIndex];
            }
            
        }
    }

}
