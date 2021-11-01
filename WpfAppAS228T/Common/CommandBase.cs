using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfAppAS228T.Common
{
    public class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;
        //public event EventHandler CanExecuteChanged
        //{
        //    add { CommandManager.RequerySuggested += value; }
        //    remove { CommandManager.RequerySuggested -= value; }
        //}

        public bool CanExecute(object parameter)
        {
            return DoCanExecute?.Invoke(parameter) == true;
            //throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            DoExecute?.Invoke(parameter);
            //throw new NotImplementedException();
        }

        public Action<object> DoExecute { get; set; }

        public Func<object, bool> DoCanExecute { get; set; }


        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
