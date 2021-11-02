using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppAS228T.Common;
using WpfAppAS228T.Model;

namespace WpfAppAS228T.ViewModel
{
    class TestPageViewModel : NotifyBase
    {
        public TestPageMode TestPageMode { get; set; }

        public TestPageViewModel()
        {
            this.TestPageMode = new TestPageMode();

            this.TestPageMode.IP = "192.168.0.111";
            this.TestPageMode.Port = "502";
            this.TestPageMode.Slave_ID = "0x01";
            this.TestPageMode.Quantity = "0x01";
            this.TestPageMode.Value = "0x01";
            this.TestPageMode.Address = "0x0001";


        }

    }
}
