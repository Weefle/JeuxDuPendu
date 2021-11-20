using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace JeuxDuPendu
{
    public class AsyncServer
    {

        public int Port { get; set; }
        [Key] public string Name { get; set; }
        public TcpListener? tcpListener;

 

       

        public AsyncServer(string Name)
        {
            this.Name = Name;
            this.Port = FindNextPort();
            tcpListener = new TcpListener(IPAddress.Loopback, Port);
        }
        public async Task StartServer()
        {
            await Task.Run(Run);
        }

        public int FindNextPort()
        {

            int serverport = new Random().Next(3000) + (6000 - 3000);
            while (IsUsedPort(serverport))
            {
                serverport = new Random().Next(3000) + (6000 - 3000);
            }

            return serverport;
        }

        public static bool IsUsedPort(int port)
        {
            using (var db = new BloggingContext())
            {

                return db.servers.Any(x => x.Port == port);
         

            }
       
        }

        private void Run()
        {
            Debug.WriteLine("Running");
            tcpListener?.Start();
            while (true)
            {
                Debug.WriteLine("Before Accept");
                var state = new ServerState { WorkSocket = tcpListener?.AcceptSocket() };
                Debug.WriteLine("Before Receive");
                Receive(state);
            }
        }

        public void Stop()
        {

            tcpListener?.Server?.Close();

        }


        private void Receive(ServerState state)
        {
            state.WorkSocket?.BeginReceive(state.Buffer, 0, ServerState.BufferSize, 0, ReceiveCallBack, state);
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            Debug.WriteLine("ReceiveCallBack");
            var state = (ServerState)ar.AsyncState;
            try
            {
                int byteReceived = state.WorkSocket.EndReceive(ar);
                if (byteReceived > 0)
                {
                    var receivedString = Encoding.UTF8.GetString(state.Buffer, 0, byteReceived);
                    Debug.WriteLine("Received: " + receivedString);
                    var bytesToSend = Encoding.UTF8.GetBytes("Server Got --> " + receivedString);
                    Array.Copy(bytesToSend, state.Buffer, bytesToSend.Length);
                    state.WorkSocket.Send(state.Buffer, 0, bytesToSend.Length, SocketFlags.None);
                    Array.Clear(state.Buffer, 0, state.Buffer.Length);
                    Receive(state);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }


        public class ServerState
        {
            public const int BufferSize = 1024;
            public readonly byte[] Buffer = new byte[1024];
            public Socket? WorkSocket;
        }
    }
    
    }