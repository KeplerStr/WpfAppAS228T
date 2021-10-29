using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfAppAS228T.Common;
using WpfAppAS228T.Model;

namespace WpfAppAS228T.ViewModel
{
    public class LoginViewModel
    {
        public CommandBase CloseWindowCommand { get; set; }

        public LoginModel LoginModel { get; set; }

        public LoginViewModel()
        {
            this.LoginModel = new LoginModel();

            this.LoginModel.UserName = "kepler";
            this.LoginModel.Password = "123456";

            this.CloseWindowCommand = new CommandBase();
            this.CloseWindowCommand.DoExecute = new Action<object>((o) => 
            {
                (o as Window).Close();
            });
            this.CloseWindowCommand.DoCanExecute = new Func<object, bool>((o) => 
            {
                return true;
            });
        }
        
    }
}
