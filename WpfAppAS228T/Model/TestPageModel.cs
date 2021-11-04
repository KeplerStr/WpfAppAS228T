using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppAS228T.Common;

namespace WpfAppAS228T.Model
{
    class TestPageModel : NotifyBase
    {
        private string _IP;
        public string IP
        {
            get { return _IP; }
            set
            {
                _IP = value;
                this.DoNotify();
            }
        }

        private string _Port;
        public string Port
        {
            get { return _Port; }
            set
            {
                _Port = value;
                this.DoNotify();
            }
        }

        private string _Address;
        public string Address
        {
            get { return _Address; }
            set
            {
                _Address = value;
                this.DoNotify();
            }
        }

        private string _slave_ID;
        public string Slave_ID
        {
            get { return _slave_ID; }
            set
            {
                _slave_ID = value;
                this.DoNotify();
            }
        }

        private string _quantity;
        public string Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                this.DoNotify();
            }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                this.DoNotify();
            }
        }

        private string _time;
        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                this.DoNotify();
            }
        }

        private string _data;
        public string Data
        {
            get { return _data; }
            set
            {
                _data = value;
                this.DoNotify();
            }
        }

        private string _stateMessage;
        public string StateMessage
        {
            get { return _stateMessage; }
            set { _stateMessage = value; this.DoNotify(); }
        }

        private string _funcomboboxIndex;
        public string FunComboBoxIndex
        {
            get { return _funcomboboxIndex; }
            set { _funcomboboxIndex = value; this.DoNotify(); }
        }
    }
}
