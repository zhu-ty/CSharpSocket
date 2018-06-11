using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace CSharpSocket
{
    unsafe public class SKTcpSocket
    {
        private Socket socket = null;

        private static byte[] CopyPtrToArray(byte* src, int srclen)
        {
            byte[] dst;
            if (src == null || srclen < 0)
            {
                throw new ArgumentException();
            }
            dst = new byte[srclen];
            // 以下固定语句固定
            // dst 对象在内存中的位置，以使这两个对象
            // 不会被垃圾回收移动。
            fixed (byte* pDst = dst)
            {
                byte* ps = src;
                byte* pd = pDst;
                // 以 4 个字节的块为单位循环计数，一次复制
                // 一个整数（4 个字节）：
                for (int n = 0; n < srclen / 4; n++)
                {
                    *((int*)pd) = *((int*)ps);
                    pd += 4;
                    ps += 4;
                }
                // 移动未以 4 个字节的块移动的所有字节，
                // 从而完成复制：
                for (int n = 0; n < srclen % 4; n++)
                {
                    *pd = *ps;
                    pd++;
                    ps++;
                }
            }
            return dst;
        }

        private static void CopyArrayToPtr(byte[] src, byte* dst, int count)
        {
            if (src == null || dst == null || count <= 0 || count > src.Length)
            {
                throw new ArgumentException();
            }
            // 以下固定语句固定
            // dst 对象在内存中的位置，以使这两个对象
            // 不会被垃圾回收移动。
            fixed (byte* pSrc = src)
            {
                byte* ps = pSrc;
                byte* pd = dst;
                // 以 4 个字节的块为单位循环计数，一次复制
                // 一个整数（4 个字节）：
                for (int n = 0; n < count / 4; n++)
                {
                    *((int*)pd) = *((int*)ps);
                    pd += 4;
                    ps += 4;
                }
                // 移动未以 4 个字节的块移动的所有字节，
                // 从而完成复制：
                for (int n = 0; n < count % 4; n++)
                {
                    *pd = *ps;
                    pd++;
                    ps++;
                }
            }
            return;
        }

        private static byte[] byte_connect(List<byte[]> btlist)
        {
            int length = 0;
            int now = 0;
            for (int i = 0; i < btlist.Count; i++)
                length += btlist[i].Length;
            byte[] ret = new byte[length];
            for (int i = 0; i < btlist.Count; i++)
            {
                Array.Copy(btlist[i], 0, ret, now, btlist[i].Length);
                now += btlist[i].Length;
            }
            return ret;
        }

        ///// <summary>
        ///// 连接最长等待时间
        ///// </summary>
        //private const int max_connect_senconds = 10;
        ///// <summary>
        ///// 回包数据长度
        ///// </summary>
        //private const int data_len = 8;

        //private const int CONST_PORT = 1986;

        //private class ReceiveEventArgs
        //{
        //    public byte[] data;
        //    public List<byte> extra_data;//不定长数据
        //    public DateTime time;
        //}


        //private bool connect(IPAddress target_ip, int listen_port = CONST_PORT)
        //{
        //    //socket_lock.WaitOne();
        //    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //    IAsyncResult connect_result = socket.BeginConnect(target_ip, listen_port, null, null);
        //    connect_result.AsyncWaitHandle.WaitOne(max_connect_senconds * 1000);//10s
        //    if (!connect_result.IsCompleted)
        //    {
        //        socket.Close();
        //        return false;
        //    }
        //    //socket_lock.ReleaseMutex();
        //    return true;
        //}

        //private ReceiveEventArgs send_and_receive_sync(byte[] buffer)
        //{
        //    ReceiveEventArgs ret = new ReceiveEventArgs();
        //    try
        //    {
        //        socket.Send(buffer);
        //        byte[] rec_buf = new byte[data_len];
        //        socket.Receive(rec_buf);
        //        if (rec_buf[0] != 'R' || rec_buf[1] != 'E' || rec_buf[2] != 'T')
        //            throw new Exception("[Socket]收到了错误的数据，小概率事件，请重新连接服务器");
        //        ret.data = rec_buf;
        //        ret.time = DateTime.Now;
        //        //EXTRA
        //        if (ret.data[3] == 'L')
        //        {
        //            var x = BitConverter.ToUInt32(rec_buf, 4);
        //            if (x == 0)
        //                throw new Exception("[Socket]服务器端尚未准备好不定长数据，请等待服务端处理/采集数据");
        //            byte[] extra_buf = new byte[x];
        //            socket.Receive(extra_buf);
        //            ret.extra_data = new List<byte>(extra_buf);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //    return ret;
        //}

        //private bool send_receive(byte[] data)
        //{
        //    return false;
        //}

        //private bool test(byte* p, ref int len)
        //{

        //    Console.WriteLine(System.Text.Encoding.ASCII.GetString((CopyPtrToArray(p,len))));
        //    return true;
        //}

        //Replace Qt code start here

        public bool connected
        {
            get
            {
                if (socket == null)
                    return false;
                bool connectState = true;
                bool blockingState = socket.Blocking;
                try
                {
                    byte[] tmp = new byte[1];

                    socket.Blocking = false;
                    socket.Send(tmp, 0, 0);
                    connectState = true;
                }
                catch (SocketException e)
                {
                    // 10035 == WSAEWOULDBLOCK
                    if (e.NativeErrorCode.Equals(10035))
                    {
                        connectState = true;
                    }
                    else
                    {
                        connectState = false;
                    }
                }
                finally
                {
                    socket.Blocking = blockingState;
                }

                return connectState;
            }
        }

        /// <summary>
        /// don't use right now
        /// </summary>
        /// <returns></returns>
        public bool state()
        {
            return connected;
        }

        public void abort()
        {
            try
            {
                socket.Close();
            }
            catch(Exception e)
            {

            }
        }

        public bool connectToHost(byte *ip, int ip_len, int port, int exceed_time = 10000)
        {
            string target_ip_string = System.Text.Encoding.ASCII.GetString(
                CopyPtrToArray(ip, ip_len));
            IPAddress target_ip = IPAddress.Parse(target_ip_string);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.NoDelay = true;
            socket.ReceiveBufferSize = 8 * 4096 * 4096;
            IAsyncResult connect_result = socket.BeginConnect(target_ip, port, null, null);
            connect_result.AsyncWaitHandle.WaitOne(exceed_time);
            if (!connect_result.IsCompleted)
            {
                socket.Close();
                return false;
            }
            return true;
        }

        public bool write(byte *data, int data_len)
        {
            byte[] buffer = CopyPtrToArray(data, data_len);
            try
            {
                socket.Send(buffer);
            }
            catch(Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }

        public bool waitForReadyRead(int exceed_time = 10000)
        {
            for(int i = 0;i < exceed_time;i += 5)
            {
                if (socket.Available > 0)
                    return true;
                else
                    Thread.Sleep(5);
            }
            return false;
        }

        public int read(byte* data, int len)
        {
            byte[] data_receive = new byte[len];
            int len_rev = socket.Receive(data_receive, len, SocketFlags.None);
            CopyArrayToPtr(data_receive, data, len_rev);
            return len_rev;
        }

        

    }
}
