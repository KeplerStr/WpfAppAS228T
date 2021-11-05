using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppAS228T.DataAccess
{
    public class Meter2000DataAccess
    {
        public class RxPackage
        {

            private List<byte> m_data_message = new List<byte>(6);
            private List<byte> m_state1 = new List<byte>(1);
            private List<byte> m_state2 = new List<byte>(1);

            public List<byte> Data_message { get => m_data_message; set => m_data_message = value; }
            public List<byte> State1 { get => m_state1; set => m_state1 = value; }
            public List<byte> State2 { get => m_state2; set => m_state2 = value; }

            public RxPackage()
            {
                this.ClearFields();
            }

            public void ClearFields()
            {
                this.Data_message.Clear();
                this.State1.Clear();
                this.State2.Clear();
            }

        }



        private RxPackage rxPackage;
        public enum PackageSate
        {
            START = 0,
            DATA_MESSAGE,
            END
        }

        private PackageSate m_running_sate;
        public PackageSate Running_sate { get => m_running_sate; set => m_running_sate = value; }
        public RxPackage Rxpackage { get => rxPackage; set => rxPackage = value; }

        public Meter2000DataAccess()
        {
            this.Rxpackage = new RxPackage();
        
        }

        public void GetRxpackage(byte mes)
        {
            switch (this.Running_sate)
            {
                case PackageSate.START:
                    Rxpackage.ClearFields();
                    Running_sate = PackageSate.START;
                    if (mes == 0xFF)
                    {
                        this.Running_sate = PackageSate.DATA_MESSAGE;
                    }
                    break;
                case PackageSate.DATA_MESSAGE:
                    if (mes == 0x0D)
                    {
                        this.Running_sate = PackageSate.END;
                    }
                    else
                    {
                        if (Rxpackage.Data_message.Count < 6)
                        {
                            this.Rxpackage.Data_message.Add(mes);
                        }
                        else
                        {
                            if (Rxpackage.State1.Count < 1)
                            {
                                this.Rxpackage.State1.Add(mes);
                            }
                            else
                            {
                                if (Rxpackage.State2.Count < 1)
                                {
                                    this.Rxpackage.State2.Add(mes);
                                }
                                else
                                {
                                    this.Running_sate = PackageSate.START;
                                }
                            }
                            
                        }
                    }
                    break;
                case PackageSate.END:
                    break;
            }
        }


    }
}
