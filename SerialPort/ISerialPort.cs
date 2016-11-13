using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPort
{

    public interface ISerialPort: IDisposable
    {
        bool IsOpen();
        ISerialPort Open(string port, int baudRate, int timeoutMs);
        void Close();
        void Read(byte[] buffer, int offset, int count);
        void Write(byte[] buffer, int offset, int count);
        byte Read();
        int BaudRate { get; }
    }
}
