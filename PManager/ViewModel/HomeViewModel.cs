using Newtonsoft.Json;
using PManager.MyControls;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PManager.ViewModel
{
    /// <summary>
    /// View model utilizado para representar los datos y la lógica relacionados con la vista de inicio de la aplicación.
    /// Contiene propiedades y comandos utilizados en la vista de inicio para mostrar y gestionar los elementos principales de la aplicación.
    /// Este view model se encarga de coordinar las interacciones entre otros view models y la vista de inicio.
    /// </summary>
    public class HomeViewModel : BaseViewModel
    {

        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set
            {
                _currentDate = value;
                OnPropertyChanged(nameof(CurrentDate));
                UpdateCalendar();
            }
        }

        private DateTime _currentMonth;
        public DateTime CurrentMonthYear
        {
            get { return _currentMonth; }
            set
            {
                if (_currentMonth != value)
                {
                    _currentMonth = value;
                    OnPropertyChanged(nameof(CurrentMonthYear));
                    UpdateCalendar();
                }
            }
        }

        private bool _isCurrentDay;
        public bool IsCurrentDay
        {
            get { return _isCurrentDay; }
            set
            {
                _isCurrentDay = value;
                OnPropertyChanged(nameof(IsCurrentDay));
            }
        }

        public ObservableCollection<ButtonDay> DaysOfMonth { get; set; }
        public ICommand PreviousMonthCommand { get; set; }
        public ICommand NextMonthCommand { get; set; }
        public ICommand DaySelectedCommand { get; set; }


        public HomeViewModel()
        {
            DaysOfMonth = new ObservableCollection<ButtonDay>();

            IsCurrentDay = (CurrentDate.Date == DateTime.Today);

            CurrentMonthYear = DateTime.Now;
            CurrentDate = DateTime.Now;
            PreviousMonthCommand = new RelayCommand(PreviousMonth);
            NextMonthCommand = new RelayCommand(NextMonth);

            UpdateCalendar();
        }

        private void UpdateCalendar()
        {

            DaysOfMonth.Clear();

            int daysInMonth = DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month);

            for (int i = 1; i < (int)CurrentDate.DayOfWeek; i++)
            {
                var btn = new ButtonDay();
                btn.ContentValue = 0.ToString();

                DaysOfMonth.Add(btn);
            }

            for (int i = 1; i <= daysInMonth; i++)
            {
                var btn = new ButtonDay();
                btn.ContentValue = i.ToString();

                DateTime currentDate = DateTime.Today;

                if (i == currentDate.Day && CurrentDate.Month == currentDate.Month && CurrentDate.Year == currentDate.Year)
                {
                    btn.Background = new SolidColorBrush(Colors.Cyan);
                    btn.Foreground = new SolidColorBrush(Colors.Black);
                    btn.BorderBrush = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    btn.Background = new SolidColorBrush(Colors.Transparent);
                    btn.Foreground = new SolidColorBrush(Colors.White);
                    btn.BorderBrush = new SolidColorBrush(Colors.WhiteSmoke);
                }

                DaysOfMonth.Add(btn);
            }

        }

        private void PreviousMonth(object obj)
        {
            CurrentDate = CurrentDate.AddMonths(-1);
            CurrentMonthYear = CurrentDate;
            IsCurrentDay = (CurrentDate.Date == DateTime.Today);
        }

        private void NextMonth(object obj)
        {
            CurrentDate = CurrentDate.AddMonths(1);
            CurrentMonthYear = CurrentDate;
            IsCurrentDay = (CurrentDate.Date == DateTime.Today);
        }

    }
}
