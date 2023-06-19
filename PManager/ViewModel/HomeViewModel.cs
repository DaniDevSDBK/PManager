using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PManager.ViewModel
{
    //https://newsapi.org/v2/everything?q=cybersecurity&apiKey=61b9f93e91d84003837ff300f31a6d09

    /// <summary>
    /// View model utilizado para representar los datos y la lógica relacionados con la vista de inicio de la aplicación.
    /// Contiene propiedades y comandos utilizados en la vista de inicio para mostrar y gestionar los elementos principales de la aplicación.
    /// Este view model se encarga de coordinar las interacciones entre otros view models y la vista de inicio.
    /// </summary>
    public class HomeViewModel : BaseViewModel
    {
        private const string ApiKey = "61b9f93e91d84003837ff300f31a6d09";
        private const string ApiUrl = "https://newsapi.org/v2/everything";

        private ObservableCollection<NewsItem> _newsItems;
        public ObservableCollection<NewsItem> NewsItems
        {
            get { return _newsItems; }
            set
            {
                _newsItems = value;
                OnPropertyChanged(nameof(NewsItems));
            }
        }

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

        public ObservableCollection<int> DaysOfMonth { get; set; }
        public ICommand PreviousMonthCommand { get; set; }
        public ICommand NextMonthCommand { get; set; }
        public ICommand DaySelectedCommand { get; set; }


        public HomeViewModel()
        {
            DaysOfMonth = new ObservableCollection<int>();
            NewsItems = new ObservableCollection<NewsItem>();

            IsCurrentDay = (CurrentDate.Date == DateTime.Today);

            CurrentMonthYear = DateTime.Now;
            CurrentDate = DateTime.Now;
            PreviousMonthCommand = new RelayCommand(PreviousMonth);
            NextMonthCommand = new RelayCommand(NextMonth);
            DaySelectedCommand = new RelayCommand(DaySelected);

            UpdateCalendar();
            LoadNewsAsync();
        }

        private void UpdateCalendar()
        {
            DaysOfMonth.Clear();

            int daysInMonth = DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month);

            // Fill days before the first day of the month
            for (int i = 1; i < (int)CurrentDate.DayOfWeek; i++)
            {
                DaysOfMonth.Add(0);
            }

            // Fill days of the month
            for (int i = 1; i <= daysInMonth; i++)
            {
                DaysOfMonth.Add(i);
            }
        }

        private void PreviousMonth(object obj)
        {
            CurrentDate = CurrentDate.AddMonths(-1);
            CurrentMonthYear = CurrentDate;
        }

        private void NextMonth(object obj)
        {
            CurrentDate = CurrentDate.AddMonths(1);
            CurrentMonthYear = CurrentDate;
        }

        public void DaySelected(object obj)
        {
            if (obj is int day)
            {
                IsCurrentDay = day == DateTime.Today.Day;
            }
        }

        public async Task LoadNewsAsync()
        {
            using (var client = new HttpClient())
            {
                string url = $"{ApiUrl}?q=cybersecurity&apiKey={ApiKey}&pageSize=15";

                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    NewsApiResponse apiResponse = JsonConvert.DeserializeObject<NewsApiResponse>(responseBody);

                    if (apiResponse != null && apiResponse.Articles != null)
                    {
                        NewsItems = new ObservableCollection<NewsItem>(apiResponse.Articles);
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw;
                }
            }
        }

        public class NewsItem
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Date { get; set; }
            public string ImageUrl { get; set; }
        }

        public class NewsApiResponse
        {
            public NewsItem[] Articles { get; set; }
        }

    }
}
