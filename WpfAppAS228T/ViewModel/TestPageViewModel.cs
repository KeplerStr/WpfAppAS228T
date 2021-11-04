using NModbus;
using NModbus.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WpfAppAS228T.Common;
using WpfAppAS228T.Model;


namespace WpfAppAS228T.ViewModel
{

    
    class TestPageViewModel : NotifyBase
    {
        
        private ModbusFactory _factory;
        private IModbusMaster _master;

        private TcpClient _tcpClient;

        public TestPageModel TestPageModel { get; set; }

        public CommandBase OpenDeviceCommand { get; set; }
        public CommandBase CloseDeviceCommand { get; set; }

        public CommandBase SendMessageCommand { get; set; }

        private bool IsConnected { get; set; }

        public TestPageViewModel()
        {
            this.TestPageModel = new TestPageModel();

            this.TestPageModel.IP = "127.0.0.1";
            this.TestPageModel.Port = "502";
            this.TestPageModel.Slave_ID = "0x01";
            this.TestPageModel.Quantity = "0x01";
            this.TestPageModel.Value = "0x01";
            this.TestPageModel.Address = "0x64";
            this.TestPageModel.StateMessage = "错误提示";
            this.TestPageModel.FunComboBoxIndex = "0";

            this.IsConnected = false;

            

            this.OpenDeviceCommand = new CommandBase();
            this.OpenDeviceCommand.DoExecute = new Action<object>(DoOpenDevice);
            this.OpenDeviceCommand.DoCanExecute = new Func<object, bool>((o) => {
                return true;
            });

            this.CloseDeviceCommand = new CommandBase();
            this.CloseDeviceCommand.DoExecute = new Action<object>(DoCloseDevice);
            this.CloseDeviceCommand.DoCanExecute = new Func<object, bool>((o) => {
                return true;
            });

            this.SendMessageCommand = new CommandBase();
            this.SendMessageCommand.DoExecute = new Action<object>(DoSendMessage);
            this.SendMessageCommand.DoCanExecute = new Func<object, bool>((o) => {
                return true;
            });
        }

        ~TestPageViewModel()
        {
            if(this._tcpClient != null)
            {
                if (this._tcpClient.Connected)
                {
                    this._tcpClient.Close();
                    this.IsConnected = false;
                }
            }
        }

        

        private void DoSendMessage(object obj)
        {
            int index = int.Parse(this.TestPageModel.FunComboBoxIndex);

            byte slaveID;
            ushort address;
            ushort pointNum;
            ushort[] data_ToWrite_R;
            bool[] data__ToWrite_C;

            ushort[] data_ToRead_HI_R;
            bool[] data_ToRead_C_I;

            slaveID = TestPageModel.Slave_ID.IndexOf("0x") != -1
                ? (byte)int.Parse(TestPageModel.Slave_ID.Substring(2), System.Globalization.NumberStyles.HexNumber)
                : (byte)int.Parse(TestPageModel.Slave_ID);

            address = TestPageModel.Address.IndexOf("0x") != -1
                ? (ushort)int.Parse(TestPageModel.Address.Substring(2), System.Globalization.NumberStyles.HexNumber)
                : (ushort)int.Parse(TestPageModel.Address);

            pointNum = TestPageModel.Quantity.IndexOf("0x") != -1
                ? (ushort)int.Parse(TestPageModel.Quantity.Substring(2), System.Globalization.NumberStyles.HexNumber)
                : (ushort)int.Parse(TestPageModel.Quantity);


            string[] hexValuesSplit = TestPageModel.Value.Substring(2).Split(' ');

            List<bool> temp_list_C = new List<bool>();         //写线圈的数据
            foreach (string hex in hexValuesSplit)
            {
                bool temp;
                temp = hex == "00" ? false : true;
                temp_list_C.Add(temp);
            }
            data__ToWrite_C = temp_list_C.ToArray();

            List<ushort> temp_list_R = new List<ushort>();     //写寄存器的数据
            foreach (string hex in hexValuesSplit)
            {
                ushort temp = (ushort)int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                temp_list_R.Add(temp);
            }
            data_ToWrite_R = temp_list_R.ToArray();

            string result;


            if (IsConnected)
            {
                try
                {
                    switch (index)
                    {
                        case 0:             // 读线圈
                            data_ToRead_C_I = _master.ReadCoils(slaveID,address,pointNum);
                            result = string.Join(",", data_ToRead_C_I);
                            TestPageModel.StateMessage = "读地址" + address + " " + pointNum + "个线圈状态为" + "{" + result + "}";
                            break;
                        case 1:             // 读离散量输入
                            data_ToRead_C_I = _master.ReadInputs(slaveID, address, pointNum);
                            result = string.Join(",", data_ToRead_C_I);
                            TestPageModel.StateMessage = "读地址" + address + " " + pointNum + "个离散量状态为" + "{" + result + "}";
                            break;
                        case 2:             // 读输入寄存器
                            data_ToRead_HI_R = _master.ReadInputRegisters(slaveID, address, pointNum);
                            result = string.Join(",", data_ToRead_HI_R);
                            TestPageModel.StateMessage = "读地址" + address + " " + pointNum + "个输入寄存器为" + "{" + result + "}";
                            break;
                        case 3:             // 读保持寄存器
                            data_ToRead_HI_R = _master.ReadHoldingRegisters(slaveID, address, pointNum);
                            result = string.Join(",", data_ToRead_HI_R);
                            TestPageModel.StateMessage = "读地址" + address + " " + pointNum + "个保持寄存器为" + "{" + result + "}";
                            break;
                        case 4:             // 写单个线圈
                            _master.WriteSingleCoil(slaveID, address, data__ToWrite_C[0]);
                            this.TestPageModel.StateMessage = "写地址"+ address + "单个线圈" + data__ToWrite_C[0].ToString();
                            break;
                        case 5:             // 写多个线圈
                            _master.WriteMultipleCoils(slaveID,address,data__ToWrite_C);
                            result = string.Join(",", data__ToWrite_C);
                            TestPageModel.StateMessage = "写地址" + address + "线圈数据为" + "{" + result + "}";
                            break;
                        case 6:             // 写单个保持寄存器
                            _master.WriteSingleRegister(slaveID, address, data_ToWrite_R[0]);
                            this.TestPageModel.StateMessage = "写地址" + address + "单个保持寄存器" + data_ToWrite_R[0].ToString();
                            break;
                        case 7:             // 写多个保持寄存器
                            _master.WriteMultipleRegisters(slaveID, address, data_ToWrite_R);
                            result = string.Join(",", data_ToWrite_R);
                            this.TestPageModel.StateMessage = "写地址" + address + "多个保持寄存器数据为" + "{" + result + "}";
                            break;
                        default:
                            this.TestPageModel.StateMessage = "default";
                            break;
                    }
                }
                catch (Exception ex)
                {
                    this.TestPageModel.StateMessage = $"{ex}";
                }
            }
            else
            {
                this.TestPageModel.StateMessage = "未建立连接";
            }
        }

        private void DoOpenDevice(object obj)
        {
            int tcpPort = int.Parse(this.TestPageModel.Port);
            string ipAddress = this.TestPageModel.IP;

            if (!IsConnected)
            {
                try
                {
                    var ping = new Ping();
                    var reply = ping.Send(ipAddress, 1000);
                    if (reply.Status == IPStatus.Success)
                    {
                        try
                        {
                            _tcpClient = new TcpClient(ipAddress, tcpPort);
                            _factory = new ModbusFactory();
                            _master = _factory.CreateMaster(_tcpClient);
                            TestPageModel.StateMessage = "设备打开";
                            IsConnected = true;
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteLog("TCP连接", ex);
                            TestPageModel.StateMessage = $"{ex}";
                        }
                    }
                    else
                    {
                        throw new Exception("IP地址错误");
                    }
                }
                catch (Exception ex)
                {
                    TestPageModel.StateMessage = ex.Message;
                    LogHelper.WriteLog("IP地址", ex);
                }
            }
        }

        private void DoCloseDevice(object obj)
        {
            if (this.IsConnected)
            {
                _tcpClient.Close();
                this.TestPageModel.StateMessage = "设备关闭";
                this.IsConnected = false;
            }
        }
    }
}
