using Microsoft.VisualBasic.ApplicationServices;
using PManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PManager.Model
{
    public class UserAccountModel
    {

        public String UserName { get; set; }
        public String DisplayName { get; set; }
        public String Email { get; set; }
        public BitmapImage ProfilePicture { get; set; }

    }
}
