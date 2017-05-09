//********************************
//端口扫描
//********************************
using ConApp;
using System.Net.Sockets;

class PortScan
{
    string m_host;
    int m_port;

    public PortScan(string host, int port)
    {
        m_host = host;
        m_port = port;
    }
    public void Scan()
    {
        TcpClient tc = new TcpClient();
        tc.SendTimeout = tc.ReceiveTimeout = 2000;

        try
        {
            tc.Connect(m_host, m_port);
            if (tc.Connected)
            {
                //Console.WriteLine("Port {0} is Open", m_port.ToString().PadRight(6));
                Program.openedPorts.Add(m_port);
            }
        }
        catch
        {
            //Console.WriteLine("Port {0} is Closed", m_port.ToString().PadRight(6));
        }
        finally
        {
            tc.Close();
            tc = null;
            Program.scannedCount++;
            Program.runningThreadCount--;
        }
    }
}

