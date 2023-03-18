using PManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PManager.Model
{
    public interface AppRepositable
    {

        public void AddApp(AppModel appModel);
        public void EditApp(AppModel appModel);
        public void DeleteApp(AppModel appModel);

        AppModel GetAppById(int id);
        AppModel GetAppByName(string name);
        List<AppModel> UpdateData();
        List<ContentViewModel> GetContentList(string _appName);
    }
}
