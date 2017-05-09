//===========================================================
//    C# 实现端口扫描
//===========================================================
using System;
using System.Collections.Generic;
using System.Threading;

namespace ConApp
{
    class Program
    {
        //已扫描端口数目
        internal static int scannedCount = 0;
        //线程数
        internal static int runningThreadCount = 0;
        //已开放端口
        internal static List<int> openedPorts = new List<int>();
        //起始端口
        static int startPort = 1;
        //结束端口
        static int endPort = 500;
        //最大线程
        static int maxThread = 100;
        //起始地址
        static string startHost;
        //结束地址
        static string endHost;
        //地址池
        //static string[] ip_result = new string[255];
        static List<string> ip_result = new List<string>();
        static void Main(string[] args)
        {
            //简单提示
            Console.WriteLine("//************************************");
            Console.WriteLine("//  网络端口批量扫描");
            Console.WriteLine("//************************************");
            Console.WriteLine("请输入要扫描的主机 例如192.168.1.1-192.168.1.254");
            string hostRange = Console.ReadLine();
            Console.WriteLine("请输入扫描的端口 例如：1-800");
            string portRange = Console.ReadLine();
            Console.WriteLine("开始扫描请等待");

            //IP地址段转换为单独IP
            startHost = hostRange.Split('-')[0].Trim();
            endHost = hostRange.Split('-')[1].Trim();

            //IP地址转换为数字
            IPconversion ipc = new IPconversion();
            uint iStartip = ipc.ipTint(startHost);
            uint iEndIp = ipc.ipTint(endHost);

            if (iEndIp >= iStartip)
            {
                int k = 0;
                for (uint ip = iStartip; ip <= iEndIp; ip++)
                {
                    ip_result.Add(ipc.intTip(ip));
                    k++;
                    //ip_result = ip_result + intTip(ip)+"\r\n";  
                }
            }

            startPort = int.Parse(portRange.Split('-')[0].Trim());
            endPort = int.Parse(portRange.Split('-')[1].Trim());
            for (int i = 0; i < ip_result.Count; i++)
            {
                // Console.WriteLine("***开始扫描****" + ip_result[i]);
                scannedCount = 0;
                runningThreadCount = 0;
                openedPorts = new List<int>();

                for (int port = startPort; port <= endPort; port++)
                {
                    PortScan scanner = new PortScan(ip_result[i].ToString(), port);
                    Thread thread = new Thread(new ThreadStart(scanner.Scan));
                    thread.Name = port.ToString();
                    thread.IsBackground = true;
                    thread.Start();

                    runningThreadCount++;
                    Thread.Sleep(2);

                    //循环，直到某个线程工作完毕才启动另一新线程，也可以叫做推拉窗技术
                    while (runningThreadCount >= maxThread) ;
                }
                //输出结果
                Console.Write("IP：{0} 开放端口：{1}个 ", ip_result[i], openedPorts.Count);
                //空循环，直到所有端口扫描完毕
                foreach (int port in openedPorts)
                {
                    Console.Write("\t{0}", port.ToString().PadLeft(6));
                }
                //while (scannedCount + 1 < (endPort - startPort)) ;
                Console.WriteLine();
            }

            Console.ReadLine();

        }

    }
}