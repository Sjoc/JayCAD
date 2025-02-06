using System.Globalization;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace PyComm
{

    public class PythonComm
    {
        private NamedPipeServerStream m_Pipe;
        private BinaryReader m_Reader;
        private BinaryWriter m_Writer;
        private bool thread_run = false;

        private string pipeName = "ImagServ";
        public UInt16 Command_out = 0;
        public UInt16 Command_in = 0;

        public PythonComm()
        {

        }
        public void Connect()
        {
            thread_run = true;
            m_Pipe = new NamedPipeServerStream(pipeName);
            m_Pipe.WaitForConnectionAsync();

        }
        public void Disconnect()
        {
            thread_run = false;
            if (m_Pipe != null)
            {
                m_Pipe.Close();
            }
        }
        private void WriteCommand(string command)
        {

            var buf = Encoding.ASCII.GetBytes(command);
            m_Writer.Write((uint)buf.Length);
            m_Writer.Write(buf);
        }
        private object ReadCommand()
        {
            var len = m_Reader.ReadUInt32();
            var temp = new string(m_Reader.ReadChars((int)len));
            return temp;
        }
        private void CommThread()
        {
            while (thread_run)
            {
                if(m_Pipe != null)
                {
                    WriteCommand(Command_out.ToString());
                }
                Thread.Sleep(100);
            }
        }
    }
}