using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace JeuxDuPendu
{
    public class AsyncClient
    {
        private const int Port = 9999;
        [Key] public string Name { get; set; }

        public AsyncClient(string Name)
        {
            this.Name = Name;
        }

        public void StartClient()
        {
            try
            {
                var workSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var state = new ClientState { WorkSocket = workSocket };
                workSocket.BeginConnect(new IPEndPoint(IPAddress.Loopback, Port), ConnectCallBack, state);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void ConnectCallBack(IAsyncResult ar)
        {
            var state = (ClientState)ar.AsyncState;
            state.WorkSocket.EndConnect(ar);
            Send(state);
        }

        private void Receive(ClientState clientState)
        {
            clientState.WorkSocket.BeginReceive(clientState.Buffer, 0, ClientState.BufferSize, 0, ReceiveCallBack, clientState);
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            var state = (ClientState)ar.AsyncState;
            Socket client = state.WorkSocket;
            int byteReceived = client.EndReceive(ar);
            if (byteReceived > 0)
            {
                var receivedString = Encoding.UTF8.GetString(state.Buffer, 0, byteReceived);
                Debug.WriteLine("From Server: " + receivedString);
                Array.Clear(state.Buffer, 0, state.Buffer.Length);
                state.Count++;
                Thread.Sleep(1000);
                Send(state);
            }
        }

        private void Send(ClientState clientState)
        {
            Debug.WriteLine("Sending " + Name);
            byte[] buffer = Encoding.UTF8.GetBytes(string.Format("Send from Thread {0} Client id {1} Count {2}", Thread.CurrentThread.ManagedThreadId, Name, clientState.Count));
            clientState.WorkSocket.BeginSend(buffer, 0, buffer.Length, 0, BeginSendCallBack, clientState);
        }

        private void BeginSendCallBack(IAsyncResult ar)
        {
            var state = (ClientState)ar.AsyncState;
            int byteSent = state.WorkSocket.EndSend(ar);
            Debug.WriteLine("Sent {0} bytes to server.", byteSent);
            Receive(state);
        }


        public class ClientState
        {
            // Client socket.
            public Socket WorkSocket = null;
            // Size of receive buffer.
            public const int BufferSize = 1024;
            // Receive buffer.
            public byte[] Buffer = new byte[BufferSize];

            public int Count = 0;
        }
    }
}