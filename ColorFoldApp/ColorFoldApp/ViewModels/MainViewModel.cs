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
            Random random = new Random();
            Squares = new ObservableCollection<ObservableCollection<SquareModel>>();
            Color[] listColors = { Color.Yellow, Color.Green, Color.Red, Color.Blue };
            var customSize = 10;
            for (int i = 0; i < customSize; i++)
            {
                var currentSquare = new ObservableCollection<SquareModel>();
                for (int j = 0; j < customSize; j++)
                {
                    int randomIndex = random.Next(0, listColors.Length);
                    currentSquare.Add(new SquareModel() { SqColor = listColors[randomIndex], Position = new Coordinate(){x = i,y=j}});
                }
                Squares.Add(currentSquare);
            }

            Squares[0][0].IsFocused = true;
          
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}