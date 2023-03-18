using PManager.Model;
using PManager.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PManager.ViewModel
{
    public class AddAppViewModel : BaseViewModel
    {
        //
        private String appName = "";
        private String userAppName = "";
        private String userAppPwd = "";
        private String userAppPwdConfirm = "";
        private AppRepositable appRepo;

        //Properties
        public string AppName { get => appName; set { appName = value; OnPropertyChanged(nameof(appName)); } }
        public string UserAppName { get => userAppName; set { userAppName = value; OnPropertyChanged(nameof(appName)); } }
        public string UserAppPwd { get => userAppPwd; set { userAppPwd = value; OnPropertyChanged(nameof(userAppPwd)); } }
        public string UserAppPwdConfirm { get => userAppPwdConfirm; set { userAppPwdConfirm = value; OnPropertyChanged(nameof(userAppPwdConfirm)); } }

        //
        private string passwordLength = "";
        private string quantityOfNumbers = "";
        private string capitalsNumber = "";
        private string lowerCaseNumber = "";
        private string specialChar = "";
        private PasswordGenerator pGenerator;

        //Properties
        public string PasswordLength { get => passwordLength; set { passwordLength = value; OnPropertyChanged(nameof(passwordLength)); } }
        public string QuantityOfNumbers { get => quantityOfNumbers; set { quantityOfNumbers = value; OnPropertyChanged(nameof(quantityOfNumbers)); } }
        public string CapitalsNumber { get => capitalsNumber; set { capitalsNumber = value; OnPropertyChanged(nameof(capitalsNumber)); } }
        public string LowerCaseNumber { get => lowerCaseNumber; set { lowerCaseNumber = value; OnPropertyChanged(nameof(lowerCaseNumber)); } }
        public string SpecialChar { get => specialChar; set { specialChar = value; OnPropertyChanged(nameof(specialChar)); } }

        //Commands
        public ICommand SaveNewAppCmd { get; }
        public ICommand GeneratePasswordCmd { get; }

        //Constructor
        public AddAppViewModel()
        {
            appRepo = new AppRepo();
            SaveNewAppCmd = new RelayCommand(ExecSaveNewAppCmd);

            pGenerator = new PasswordGenerator();
            GeneratePasswordCmd = new RelayCommand(ExecGenPasswordCmd);
        }

        private void ExecGenPasswordCmd(object obj)
        {
            if (CheckPwdData())
            {
                pGenerator = new PasswordGenerator(new List<string>() { PasswordLength, QuantityOfNumbers, CapitalsNumber, LowerCaseNumber, SpecialChar.ToString() });
                UserAppPwd = pGenerator.Password;
                UserAppPwdConfirm = pGenerator.Password;
                PasswordLength = "";
                QuantityOfNumbers = "";
                CapitalsNumber = "";
                LowerCaseNumber = "";
                SpecialChar = "";
            }
        }

        private void ExecSaveNewAppCmd(object obj)
        {
            if (CheckData())
            {
                appRepo.AddApp(new AppModel(AppName, UserAppName, UserAppPwd));
                AppName = "";
                UserAppName = "";
                UserAppPwd = "";
                UserAppPwdConfirm = "";
            }
        }

        private bool CheckData()
        {

            if(string.IsNullOrWhiteSpace(AppName) || string.IsNullOrWhiteSpace(UserAppName) || string.IsNullOrWhiteSpace(UserAppPwd)||string.IsNullOrEmpty(UserAppPwdConfirm))
            {

                return false;
            }else if (!UserAppPwd.Equals(UserAppPwdConfirm)){

                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckPwdData()
        {

            try
            {

                Int32.Parse(PasswordLength);
                Int32.Parse(QuantityOfNumbers);
                Int32.Parse(CapitalsNumber);
                Int32.Parse(LowerCaseNumber);

            }
            catch (ArgumentNullException)
            {

                return false;
            }
            catch (FormatException)
            {

                return false;
            }
            catch (OverflowException)
            {

                return false;
            }

            return true;
        }
    }

}
