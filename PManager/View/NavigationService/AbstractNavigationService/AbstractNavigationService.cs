using Microsoft.Win32;
using PManager.View.NavigationService.InterfaceNavigationService;

namespace PManager.View.NavigationService.AbstractNavigationService
{
    public class AbstractNavigationService : INavigationService
    {

        public void ShowRegisterView()
        {
            new RegisterView().ShowDialog();
        }
    }
}
