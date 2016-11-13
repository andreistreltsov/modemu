using SerialPort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkSerialPort
{
    public class FrameworkSerialPortAdapter : ISerialPort
    {
        private readonly System.IO.Ports.SerialPort sp = new System.IO.Ports.SerialPort();

        public int BaudRate
        {
            get
            {
                return sp.BaudRate;
            }
        }

        public void Close()
        {
            sp.Close();
        }

        public void Dispose()
        {
            sp.Dispose();
        }

        public bool IsOpen()
        {
            return sp.IsOpen;
        }

        public ISerialPort Open(string port, int baudRate)
        {
            return Open(port, baudRate, -1);
        }

        public ISerialPort Open(string port, int baudRate, int timeoutMs)
        {
            if (sp.IsOpen)
            {
                throw new InvalidOperationException("Port is already open.");
            }

            sp.PortName = port;
            sp.BaudRate = baudRate;
            sp.ReadTimeout = timeoutMs;
            sp.WriteTimeout = timeoutMs;
            sp.Open();
            return this;
        }

        public byte Read()
        {
            return (byte)sp.ReadByte();
        }

        public void Read(byte[] buffer, int offset, int count)
        {
            sp.Read(buffer, offset, count);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            sp.Write(buffer, offset, count);
        }
    }

}
