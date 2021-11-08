using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppAS228T.Common;
using WpfAppAS228T.DataAccess;
using WpfAppAS228T.ViewModel;

namespace WpfAppAS228T.Tool
{
    

    public class SerialPortHepler
    {
        public static SerialPort serialPort;

        private TestPageViewModel TestPageViewModel;

        private System.Timers.Timer _timerWaterTank = new System.Timers.Timer(500); //500ms的定时器任务
        public SerialPortHepler()
        {
            if (serialPort == null)
            {
                serialPort = new SerialPort("COM1", 19200, Parity.None, 8, StopBits.One);
            }

            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("串口开启",ex);
            }

            serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPortDataReceived); // 添加数据接收 

            string str = "%YY;22\r";
            byte[] decBytes = System.Text.Encoding.UTF8.GetBytes(str);
            serialPort.Write(decBytes, 0,decBytes.Length);
            
        }

        private Meter6000DataAccess meter6000DataAccess = new Meter6000DataAccess();
        private Meter2000DataAccess meter2000DataAccess = new Meter2000DataAccess();

        public SerialPortHepler(TestPageViewModel testPageViewModel)
        {
            TestPageViewModel = testPageViewModel;
            if (serialPort == null)
            {
                serialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
            }

            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("串口开启", ex);
            }

            serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPortDataReceived); // 添加数据接收 

            _timerWaterTank.Elapsed += new System.Timers.ElapsedEventHandler(_timerWaterTank_Tick);
            _timerWaterTank.AutoReset = true;
            _timerWaterTank.Start();

            //string str = "%00;22\r";
            //byte[] decBytes = System.Text.Encoding.ASCII.GetBytes(str);
            //serialPort.Write(decBytes, 0, decBytes.Length);
        }

        private void _timerWaterTank_Tick(object sender, EventArgs e)
        {
            System.Timers.Timer timer = sender as System.Timers.Timer;
            //要计时的时间秒数

            if (meter2000DataAccess.Rxpackage.State1.Count != 0)
            {
                if (((meter2000DataAccess.Rxpackage.State1[0] >> 4) & 0x01) == 0x01)
                {

                }
                else
                {
                    string str = "%1";
                    byte[] decBytes = System.Text.Encoding.ASCII.GetBytes(str);
                    serialPort.Write(decBytes, 0, decBytes.Length);
                    //System.Threading.Thread.Sleep(1000);
                }    
            }
            
        }

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort = sender as SerialPort;

            /*System.Threading.Thread.Sleep(100);*////延缓一会，用于防止硬件发送速率跟不上缓存数据导致的缓存数据杂乱
            int n = serialPort.BytesToRead;//先记录下来，避免某种原因，人为的原因，操作几次之间时间长，缓存不一致  
            byte[] buf = new byte[n];//声明一个临时数组存储当前来的串口数据  
            //received_count += n;//增加接收计数
            int hex = 0;
            //if (n != 0)
            //{
                //serialPort.Read(buf, 0, n);
                //hex = serialPort.ReadByte();
            //}

            while ((hex = serialPort.ReadByte()) != -1)
            {
                //this.meter6000DataAccess.GetRxpackage((byte)hex);
                //if (this.meter6000DataAccess.Running_sate == Meter6000DataAccess.PackageSate.END)
            //{
                //    string ing = Encoding.Default.GetString(this.meter6000DataAccess.RxPackage.Data_message.ToArray());
                //    this.TestPageViewModel.TestPageModel.TestValueCode = ing;
                //    this.meter6000DataAccess.Running_sate = Meter6000DataAccess.PackageSate.START;
            //}

                this.meter2000DataAccess.GetRxpackage((byte)hex);
                if (this.meter2000DataAccess.Running_sate == Meter2000DataAccess.PackageSate.END)
                        {
                    this.meter2000DataAccess.Rxpackage.Data_message.Reverse();

                    this.meter2000DataAccess.Rxpackage.Data_message.Insert(this.meter2000DataAccess.Rxpackage.Data_message.Count - (this.meter2000DataAccess.Rxpackage.State2[0] & 0x0F) + 1, (byte)'.');

                    if ((this.meter2000DataAccess.Rxpackage.State2[0] & 0x80) == 0x80)
                    {
                    this.meter2000DataAccess.Rxpackage.Data_message.Insert(0,(byte)'-');
                    }
                    else
                    {
                    this.meter2000DataAccess.Rxpackage.Data_message.Insert(0, (byte)' ');
                    }

                    string ing = Encoding.Default.GetString(this.meter2000DataAccess.Rxpackage.Data_message.ToArray());
                    this.TestPageViewModel.TestPageModel.TestValueCode = ing;
                    this.meter2000DataAccess.Running_sate = Meter2000DataAccess.PackageSate.START;


                }



                //        //System.Diagnostics.Debug.WriteLine(ing);
                //    System.Diagnostics.Debug.Write(item.ToString("X2") + " ");
                
            }

            //System.Diagnostics.Debug.Write(buf[0].ToString("X2") + " ");
        }
    }
}
