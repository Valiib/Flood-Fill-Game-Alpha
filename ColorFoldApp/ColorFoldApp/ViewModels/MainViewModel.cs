using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using ColorFoldApp.Annotations;
using ColorFoldApp.Models;

namespace ColorFoldApp.ViewModels
{
    public class MainViewModel :INotifyPropertyChanged
    {
        private int _counter;

        public int Counter
        {
            get => _counter;
            set
            {
                _counter = value;
                OnPropertyChanged("Counter");
            }
        }

      
        public ObservableCollection<SquareModel> ActiveSquares { get; set; }

        public int ViewSize { get; set; } = 0;

        public static Random RandomNumber = new Random();
        public int BoardSize { get; set; } = 10;

        public static List<Color> Colors = new List<Color>()
        {Color.FromArgb(255, 255, 228, 46), Color.FromArgb(29, 239, 29), Color.FromArgb(234, 50, 50), Color.FromArgb(51, 51, 229), Color.FromArgb(223, 72, 255)};


        public static List<Coordinate> Cords = new List<Coordinate>()
        {
            new Coordinate(){x=1,y=0},
            new Coordinate(){x=0,y=1},
            new Coordinate(){x =-1,y=0},
            new Coordinate(){x=0,y=-1}
        };

        private ObservableCollection<ObservableCollection<SquareModel>> _squares;

        public ObservableCollection<ObservableCollection<SquareModel>> Squares
        {
            get => _squares;
            set {
                if (_squares == value)
                {
                    return;
                }
                _squares = value;
                
                OnPropertyChanged("Squares");
            }
        }

        public MainViewModel()
        {
            Counter = 0;
            Squares = new ObservableCollection<ObservableCollection<SquareModel>>();
            ActiveSquares = new ObservableCollection<SquareModel>();
            for (int i = 0; i < BoardSize; i++)
            {
                var currentSquare = new ObservableCollection<SquareModel>();
                for (int j = 0; j < BoardSize; j++)
                {
                    int randomIndex = RandomNumber.Next(0, Colors.Count);
                    currentSquare.Add(new SquareModel() { SqColor = Colors[randomIndex], Position = new Coordinate(){x = i,y=j}});
                }
                Squares.Add(currentSquare);
            }

            Squares[0][0].IsFocused = true;
            ActiveSquares.Add(Squares[0][0]);
        }


     
        public void CheckAllNeighbours()
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
                if (this != null && coord.x + x >= 0 && coord.y + y >= 0 &&
                    this.Squares.Count - 1 >= coord.y + y && this.Squares.Count - 1 >= coord.x + x)
                {
                    var checkingSquare = this.Squares[coord.x + x][coord.y + y];
                    if (!checkingSquare.IsFocused && checkingSquare != null && checkingSquare.SqColor.ToArgb() ==
                        this.Squares[0][0].SqColor.ToArgb())
                    {
                        this.Squares[coord.x + x][coord.y + y].IsFocused = true;
                        ActiveSquares.Add(checkingSquare);
                    }
                }

            return true;
        }


       


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}