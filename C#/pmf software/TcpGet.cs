using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PMF
{
    class TcpGet
    {
        private IPEndPoint ipep;
        private Socket newsock;

        public TcpGet(int port)
        {
            ipep = new IPEndPoint(IPAddress.Any, port);
            newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            newsock.Bind(ipep);     //绑定
            newsock.Listen(10);     //监听
        }
        public void getData(int bufSize,int channels,double[]timelen,double[,]res,double[,] errorMap, double[]X,double[]Y,
                            double []e ,double[]current,double[] position,ref bool value_get,ref bool get_cable_pos)
        {
            int recv = -1;     //客户端发送信息长度
            byte[] data;  //缓存客户端发送的信息，sockets传递的信息必须为字节数组

            int rnd = 0;

            /**每次数据的传输分4轮 rnd记录其值
             * 第一轮传输时间轴数据、第二次传输8个线圈波形图，第三次传输计算的errormap数据，第四次传输线圈位置、电动势、电流等数据
             * 传输端每次发完数据都会重新断开其连接，主要保证同步**/
         
                Socket client = newsock.Accept();
                client.ReceiveBufferSize = 80005;
                Thread.Sleep(500);
                IPEndPoint clientip = (IPEndPoint)client.RemoteEndPoint;
                data = new byte[bufSize * 8 * channels];
                recv = client.Receive(data);

           
                client.Send(data, recv, SocketFlags.None);
                int datalen = recv / 8;

                //  convert the bytes stream to double
                double[] ans = new double[datalen];
                for (int i = 0; i < datalen; ++i)
                {
                    byte[] temp = new byte[8];
                    for (int j = 0; j < 8; ++j)
                        temp[j] = data[i * 8 + j];
                    ans[i] = BitConverter.ToDouble(temp, 0);
                }

                // get the time axis value
                if (rnd == 0)
                {
                    for (int i = 0; i < datalen; ++i)
                    {

                        timelen[i] = ans[i];
                    }
                    rnd++;

                }
                // get the volts value of coil
                else if (rnd == 1)
                {
                    for (int i = 0; i < datalen; ++i)
                    {
                        res[i / bufSize, i % bufSize] = ans[i];
                    }
                    value_get = true;
                    rnd++;
                }
                // get the value of errormap
                else if (rnd == 2)
                {
                    for (int i = 0; i < datalen; ++i)
                    {
                        if (i < 35) X[i] = ans[i];
                        else if (i >= 35 && i < 70) Y[i - 35] = ans[i];
                        else
                        {
                            errorMap[(i - 70) / 35, (i - 70) % 35] = ans[i];
                        }
                    }

                    rnd++;
                }
                // get the value of current , voltage , position
                else
                {
                    for (int i = 0; i < datalen; ++i)
                    {
                        if (i < 8) e[i] = ans[i];
                        else if (i >= 8 && i < 11) position[i - 8] = ans[i];
                        else current[i - 11] = ans[i];
                    }
                    get_cable_pos = true;
                    rnd++;
                }
                if (rnd == 4) { rnd = 0; }
                client.Close();

            

            newsock.Close();

        }
        
    }
}
