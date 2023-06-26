using PManager.Model;
using PManager.Repositorios;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PManager.ViewModel
{
    /// <summary>
    /// View model utilizado para representar los datos y la lógica relacionados con la lista de aplicaciones del usuario.
    /// Contiene propiedades y comandos utilizados para mostrar y gestionar la lista de aplicaciones en la vista correspondiente.
    /// Este view model se encarga de coordinar la interacción entre otros view models y la vista de la lista de aplicaciones.
    /// </summary>
    public class MyAppListViewModel : BaseViewModel
    {
        private ObservableCollection<ExpanderViewModel> _items;
        private UserContext user = UserContext.Instance;
        private AppRepositable appRepo;

        public ObservableCollection<ExpanderViewModel> Items
        {
            get { return _items; }
            set { _items = value; OnPropertyChanged("Items"); }
        }


        public MyAppListViewModel()
        {

            appRepo = new AppRepo();

            RefreshView();
        }

        private void RefreshView()
        {
            List<AppModel> appList = appRepo.UpdateData(user.CurrentUser.Id);
            List<string> appNames = new List<string>();

            Items = new ObservableCollection<ExpanderViewModel>();
            ObservableCollection<ContentViewModel> ContentList = new ObservableCollection<ContentViewModel>();

            appList = appList.OrderByDescending(app => app.PasswordsNumber)
                 .ThenBy(app => app.AppName)
                 .ToList();

            foreach (var appRegister in appList)
            {
                ContentList = new ObservableCollection<ContentViewModel>();

                foreach (var item in appRepo.GetContentList(appRegister.AppName, user.CurrentUser.Id))
                {
                    var contentViewModel = new ContentViewModel { UserAppName = item.UserAppName, UserAppPassword = item.UserAppPassword };
                    contentViewModel.ItemDeleted += OnItemDeleted;

                    ContentList.Add(contentViewModel);

                }

                if (!appNames.Contains(appRegister.AppName))
                {

                    Items.Add(new ExpanderViewModel { AppName = appRegister.AppName, UsersNumber = appRegister.UsersNumber, PasswordsNumber = appRegister.PasswordsNumber, ItemsContent = ContentList });
                    appNames.Add(appRegister.AppName);
                }

            }
        }

        private void OnItemDeleted(ContentViewModel contentViewModel)
        {
            // Eliminar el elemento de la lista
            foreach (var expanderViewModel in Items)
            {
                if (expanderViewModel.ItemsContent.Contains(contentViewModel))
                {
                    expanderViewModel.ItemsContent.Remove(contentViewModel);
                    break;
                }
            }

            RefreshView();
        }
    }
}
