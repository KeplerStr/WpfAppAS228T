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
    public class LoginViewModel : NotifyBase
    {
        public CommandBase CloseWindowCommand { get; set; }
    
        public CommandBase LoginCommand { get; set; }

        public LoginModel LoginModel { get; set; }

        private Visibility _showProgress = Visibility.Collapsed;
        public Visibility ShowProgress 
        {   
            get { return _showProgress; } 
            set 
            {
                _showProgress = value;
                this.DoNotify();
            } 
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; this.DoNotify(); }
        }

        public LoginViewModel()
        {
            


            this.LoginModel = new LoginModel();

            this.LoginModel.UserName = "kepler";
            this.LoginModel.Password = "123456";
            this.LoginModel.ValidationCode = "AT45";

            this.CloseWindowCommand = new CommandBase();
            this.CloseWindowCommand.DoExecute = new Action<object>((o) => 
            {
                (o as Window).Close();
            });
            this.CloseWindowCommand.DoCanExecute = new Func<object, bool>((o) => 
            {
                return true;
            });

            this.LoginCommand = new CommandBase();
            this.LoginCommand.DoExecute = new Action<object>(DoLogin);
            this.LoginCommand.DoCanExecute = new Func<object, bool>((o)=> { 
                return ShowProgress == Visibility.Collapsed; });
        }

        private void DoLogin(object obj)
        {
            this.ShowProgress = Visibility.Visible;
            this.ErrorMessage = "";

            if (string.IsNullOrEmpty(LoginModel.UserName))
            {
                this.ErrorMessage = "请输入用户名！";
                this.ShowProgress = Visibility.Collapsed;
                return;
            }

            if (string.IsNullOrEmpty(LoginModel.Password))
            {
                this.ErrorMessage = "请输入密码！";
                this.ShowProgress = Visibility.Collapsed;
                return;
            }
            if (string.IsNullOrEmpty(LoginModel.ValidationCode))
            {
                this.ErrorMessage = "请输入验证码！";
                this.ShowProgress = Visibility.Collapsed;
                return;
            }
            if (LoginModel.ValidationCode.ToLower() != "at45")
            {
                this.ErrorMessage = "验证码输入不正确！";
                this.ShowProgress = Visibility.Collapsed;
                return;
            }


            Task.Run(new Action(() =>
            {
                try
                {

                    if (LoginModel.UserName == "kepler" && LoginModel.Password == "123456")
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            (obj as Window).DialogResult = true;
                        }));
                    }
                    else
                    {
                        throw new Exception("登录失败！用户名或密码错误！");
                    }

                    //var user = LocalDataAccess.GetInstance().CheckUserInfo(LoginModel.UserName, LoginModel.Password);
                    //if (user == null)
                    //{
                    //    throw new Exception("登录失败！用户名或密码错误！");
                    //}

                    //GlobalValues.UserInfo = user;


                }
                catch (Exception ex)
                {
                    this.ErrorMessage = ex.Message;
                }
                finally
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        this.ShowProgress = Visibility.Collapsed;
                    }));
                }
            }));


            //throw new NotImplementedException();
        }
    }
}
