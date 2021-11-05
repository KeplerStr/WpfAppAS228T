using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppAS228T.Common;

namespace WpfAppAS228T.Tool
{
    

    public class SerialPortHepler
    {
        public static SerialPort serialPort;

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
        }

        private System.Collections.Queue queue = new System.Collections.Queue();
        private List<byte> vsdata = new List<byte>();
        private int state = 0;
        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort = (SerialPort)(sender);

            System.Threading.Thread.Sleep(100);///延缓一会，用于防止硬件发送速率跟不上缓存数据导致的缓存数据杂乱
            int n = serialPort.BytesToRead;//先记录下来，避免某种原因，人为的原因，操作几次之间时间长，缓存不一致  
            byte[] buf = new byte[n];//声明一个临时数组存储当前来的串口数据  
            //received_count += n;//增加接收计数
            int hex = 0;
            //if (n != 0)
            //{
                //serialPort.Read(buf, 0, n);
                //hex = serialPort.ReadByte();
            //}

            //if (hex != -1)
            //{
            //    System.Diagnostics.Debug.Write(hex.ToString("X2") + " ");
            //}

            while ((hex = serialPort.ReadByte()) != -1)
            {
                switch (state)
                {
                    case 0:
                        this.state = 0;
                        this.vsdata.Clear();
                        if (hex == 0xFF)
                        {
                            this.state = 1;
                            this.vsdata.Add((byte)hex);
                        }
                        break;
                    case 1:
                        if (hex == 0x0D)
                        {
                            this.state = 2;
                            this.vsdata.Add((byte)hex);
                        }
                        else
                        {
                            if (this.vsdata.Count() != 35)
                            {
                                this.vsdata.Add((byte)hex);
                            }
                        }
                        break;
                    case 2:
                        if (hex == 0x0A)
                        {
                            this.vsdata.Add((byte)hex);
                            this.state = 3;
                        }
                        else
                            this.state = 0;
                        break;
                    case 3:
                        this.state = 0;
                        //queue.Enqueue(this.vsdata);
                        //foreach (var item in this.vsdata)
                        //{
                        //    System.Diagnostics.Debug.Write(item.ToString("X2") + " ");
                        //}

                        string ing = Encoding.Default.GetString(this.vsdata.Skip(5).Take(11).ToArray());

                        System.Diagnostics.Debug.WriteLine(ing);

                        break;
                    default:
                        break;
                }
            }

            //System.Diagnostics.Debug.Write(buf[0].ToString("X2") + " ");
        }
    }
}
