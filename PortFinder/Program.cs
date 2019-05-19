using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace PortFinding
{
    class Program
    {
        static void Main(string[] args)
        {



            string host;
            int startPort = 1, stopPort = 65535, cntrThread = 200;
            try
            {
                Console.WriteLine("Please enter the ip address to be scanned.");
                host = Console.ReadLine();

            }
            catch
            {
                printUsage();
                return;
            }

            PortFinding scn = new PortFinding(host, startPort, stopPort);
            scn.start(cntrThread);


        }
        static void printUsage()
        {
            Console.WriteLine("Scans all ports from 0 - 65535.\n");
        }

    }
    public class PortFinding
    {
        private string host;
        private PortList Ports;

        public PortFinding(string host, int startPort, int stopPort)
        {
            this.host = host;
            this.Ports = new PortList(startPort, stopPort);
        }
        public PortFinding(string host) : this(host, 1, 65535) { }
        public PortFinding() : this("127.0.0.1") { }
        public void start(int threadCntr)
        {
            for (int i = 0; i < threadCntr; i++)
            {
                Thread th = new Thread(new ThreadStart(run));
                th.Start();
            }
        }
        public void run()
        {
            int port;
            TcpClient tcp = new TcpClient();
            while ((port = Ports.getNext()) != -1)
            {
                try
                {
                    tcp = new TcpClient(host, port);
                }
                catch
                {

                    continue;
                }
                finally
                {
                    try
                    {
                        tcp.Close();
                    }
                    catch { }
                }
                Console.WriteLine("TCP Port" +" "+ port +" "+ "opened");
            }
        }

    }

    public class PortList
    {
        private int start;
        private int stop;
        private int prt;
        public PortList(int start, int stop)
        {
            this.start = start;
            this.stop = stop;
            this.prt = start;

        }
        public PortList() : this(1, 65535) { }
        public bool hasMore()
        {
            return (stop - prt) >= 0;
        }
        public int getNext()
        {
            if (hasMore()) return prt++; return -1;

        }
        
    }
    
}
