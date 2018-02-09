using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AiWin_API
{
    public class TcpConn
    {
        public static void ConnectWithTimeout(Socket socket, EndPoint endpoint, int timeout)
        {
            var completed = new AutoResetEvent(false);
            var args = new SocketAsyncEventArgs {RemoteEndPoint = endpoint};
            args.Completed += OnConnectCompleted;
            args.UserToken = completed;
            socket.ConnectAsync(args);
            if (!completed.WaitOne(timeout) || !socket.Connected)
            {
                using (socket)
                {
                    throw new TimeoutException("Could not connect to " + endpoint);
                }
            }
        }

        private static void OnConnectCompleted(object sender, SocketAsyncEventArgs args)
        {
            EventWaitHandle handle = (EventWaitHandle)args.UserToken;
            handle.Set();
        }
    }
}
