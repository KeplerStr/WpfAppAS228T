using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppAS228T.DataAccess
{


    public class Meter6000DataAccess
    {
        public class Rxpackage
        {
            List<byte> m_data_output_attribute = new List<byte>(2);
            List<byte> m_data_message = new List<byte>(11);
            List<byte> m_data_type = new List<byte>(2);
            List<byte> m_meter_state = new List<byte>(5);
            List<byte> m_meter_communicate_state = new List<byte>(5);
            List<byte> m_range_index = new List<byte>(2);
            List<byte> m_channel_index = new List<byte>(2);

            public Rxpackage()
            {
                this.ClearFields();
            }

            public List<byte> Data_output_attribute { get => m_data_output_attribute; set => m_data_output_attribute = value; }
            public List<byte> Data_message { get => m_data_message; set => m_data_message = value; }
            public List<byte> Data_type { get => m_data_type; set => m_data_type = value; }
            public List<byte> Meter_state { get => m_meter_state; set => m_meter_state = value; }
            public List<byte> Meter_communicate_state { get => m_meter_communicate_state; set => m_meter_communicate_state = value; }
            public List<byte> Range_index { get => m_range_index; set => m_range_index = value; }
            public List<byte> Channel_index { get => m_channel_index; set => m_channel_index = value; }

            public void ClearFields()
            {
                this.Data_output_attribute.Clear();
                this.Data_message.Clear();
                this.Data_type.Clear();
                this.Channel_index.Clear();
                this.Meter_communicate_state.Clear();
                this.Meter_state.Clear();
                this.Range_index.Clear();
            }
        }

        public enum PackageSate
        { 
            START = 0,
            DATA_OUT_ATTRIBUTE,
            DATA_MESSAGE,
            DATA_TYPE,
            METER_STATE,
            METER_COMMUNICATE_STATE,
            RANGE_INDEX,
            CHANNEL_INDEX,
            END
        }

        private PackageSate m_running_sate;

        public Rxpackage m_rxpackage;
        public Rxpackage RxPackage { get => m_rxpackage; set => m_rxpackage = value; }
        public PackageSate Running_sate { get => m_running_sate; set => m_running_sate = value; }

        public Meter6000DataAccess()
        {
            this.Running_sate = PackageSate.START;
            this.RxPackage = new Rxpackage();
        }

        public PackageSate GetRxpackage(byte mes)
        {
            switch (this.Running_sate)
            {
                case PackageSate.START:
                    RxPackage.ClearFields();
                    Running_sate = PackageSate.START;
                    if (mes == 0xFF)
                    {
                        this.Running_sate = PackageSate.DATA_OUT_ATTRIBUTE;
                    }
                    break;
                case PackageSate.DATA_OUT_ATTRIBUTE:
                    if (mes == 0x3B)
                    {
                        this.Running_sate = PackageSate.DATA_MESSAGE;
                    }
                    else
                    {
                        if (RxPackage.Data_output_attribute.Count < 2)
                        {
                            this.RxPackage.Data_output_attribute.Add(mes);
                        }
                        else
                        {
                            this.Running_sate = PackageSate.START;
                        }
                    }
                    break;
                case PackageSate.DATA_MESSAGE:
                    if (mes == 0x3B)
                    {
                        this.Running_sate = PackageSate.DATA_TYPE;
                    }
                    else
                    {
                        if (RxPackage.Data_message.Count < 12)
                        {
                            this.RxPackage.Data_message.Add(mes);
                        }
                        else
                        {
                            this.Running_sate = PackageSate.START;
                        }
                    }
                    break;
                case PackageSate.DATA_TYPE:
                    if (mes == 0x3B)
                    {
                        this.Running_sate = PackageSate.METER_STATE;
                    }
                    else
                    {
                        if (RxPackage.Data_type.Count < 2)
                        {
                            this.RxPackage.Data_type.Add(mes);
                        }
                        else
                        {
                            this.Running_sate = PackageSate.START;
                        }
                    }
                    break;
                case PackageSate.METER_STATE:
                    if (mes == 0x3B)
                    {
                        this.Running_sate = PackageSate.METER_COMMUNICATE_STATE;
                    }
                    else
                    {
                        if (RxPackage.Meter_state.Count < 6)
                        {
                            this.RxPackage.Meter_state.Add(mes);
                        }
                        else
                        {
                            this.Running_sate = PackageSate.START;
                        }
                    }
                    break;
                case PackageSate.METER_COMMUNICATE_STATE:
                    if (mes == 0x3B)
                    {
                        this.Running_sate = PackageSate.RANGE_INDEX;
                    }
                    else
                    {
                        if (RxPackage.Meter_communicate_state.Count < 2)
                        {
                            this.RxPackage.Meter_communicate_state.Add(mes);
                        }
                        else
                        {
                            this.Running_sate = PackageSate.START;
                        }
                    }
                    break;
                case PackageSate.RANGE_INDEX:
                    if (mes == 0x3B)
                    {
                        this.Running_sate = PackageSate.CHANNEL_INDEX;
                    }
                    else
                    {
                        if (RxPackage.Range_index.Count < 2)
                        {
                            this.RxPackage.Range_index.Add(mes);
                        }
                        else
                        {
                            this.Running_sate = PackageSate.START;
                        }
                    }
                    break;
                case PackageSate.CHANNEL_INDEX:
                    if (mes == 0x0D)
                    {
                        this.Running_sate = PackageSate.END;
                    }
                    else
                    {
                        if (RxPackage.Channel_index.Count < 2)
                        {
                            this.RxPackage.Channel_index.Add(mes);
                        }
                        else
                        {
                            this.Running_sate = PackageSate.START;
                        }
                    }
                    break;
                case PackageSate.END:
                    break;
            
            }
            return this.Running_sate;
        }
    }
}
