using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AiWin_API
{
    /// <summary>
    /// An IPv4 TCP connected socket.
    /// </summary>
    public  class ConnectedSocket : IDisposable
    {
        /// <summary>
        /// Encoding of the content sended and received
        /// </summary>
        private Encoding _encoding;

        /// <summary>
        /// The underlying socket.
        /// </summary>
        private Socket _socket;
        /// <summary>
        /// Constructs and connects the socket.
        /// </summary>
        /// <param name="endpoint">Endpoint to connect to</param>
        public ConnectedSocket(EndPoint endpoint) : this(endpoint, Encoding.UTF8) { }

        /// <summary>
        /// Constructs and connects the socket.
        /// </summary>
        /// <param name="endpoint">Endpoint to connect to</param>
        /// <param name="encoding">Encoding of the content sended and received by the socket</param>
        public ConnectedSocket(EndPoint endpoint, Encoding encoding)
        {
            _encoding = encoding;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(endpoint);
        }

        /// <summary>
        /// Constructs and connects the socket.
        /// </summary>
        /// <param name="host">Host to connect to</param>
        /// <param name="port">Port to connect to</param>
        public ConnectedSocket(string host, int port) : this(host, port, Encoding.UTF8) { }

        /// <summary>
        /// Constructs and connects the socket.
        /// </summary>
        /// <param name="host">Host to connect to</param>
        /// <param name="port">Port to connect to</param>
        /// <param name="encoding">Encoding of the content sended and received by the socket</param>
        public ConnectedSocket(string host, int port, Encoding encoding)
        {
            _encoding = encoding;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(host, port);
        }
     
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="socket">Socket to be wrapped</param>
        internal ConnectedSocket(Socket socket)
        {
            _encoding = Encoding.UTF8;
            _socket = socket;
        }

        /// <summary>
        /// True if there's any data to receive on the socket.
        /// </summary>
        public bool AnythingToReceive => _socket.Available > 0;

        /// <summary>
        /// Disposes the socket.
        /// </summary>
        public void Dispose()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Dispose();
           // _socket.Close();
            _socket = null;
        }

        /// <summary>
        /// Receives any pending data.
        /// This blocks execution until there's data available.
        /// </summary>
        /// <param name="bufferSize">Amount of data to read</param>
        /// <returns>Received data</returns>
        public string Receive(int bufferSize = 1024)
        {
            //if (AnythingToReceive)
            //{
            var buffer = new byte[bufferSize];
            _socket.Receive(buffer);
            var result = BitConverter.ToString(buffer, 0);
            //}

            return result;
            // return Encoding.ASCII.GetString(result, 0, receiveLength);

        }
     
        // <summary>
        /// Sends the given data.

        /// <param name="data">Data to send</param>
        public void Send(byte[] data)
        {
            // var bytes = _encoding.GetBytes(data);
            _socket.Send(data);
        }
    }
}
