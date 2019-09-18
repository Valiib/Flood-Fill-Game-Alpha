using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

namespace ColorFoldApp.Models
{
    public class SquareModel : INotifyPropertyChanged
    {
        public Coordinate Position { get; set; }

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked == value)
                {
                    return;
                }
                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                
                if (_isFocused == value)
                {
                    return;
                }
                _isFocused = value;
                OnPropertyChanged("IsFocused");
            }
        }

        private Color _sqColor;
        private bool _isFocused;
        private bool _isChecked;

        public Color SqColor
        {
            get => _sqColor;
            set
            {
                if (_sqColor == value)
                {
                    return;
                }
                _sqColor = value;
                OnPropertyChanged("SqColor");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

   
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
