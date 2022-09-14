// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;

static async void Server_Async_2(string adress, int port)
{
    IPAddress iPAddress = IPAddress.Parse(adress);
    IPEndPoint ipEP = new IPEndPoint(iPAddress, port);
    Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    try
    {
        listenSocket.Bind(ipEP);
        listenSocket.Listen(10);
        Console.WriteLine("Server was started. Waiting for connection...");

        while (true)
        {
            Socket handler = await listenSocket.AcceptAsync();
            StringBuilder stringBuilder = new StringBuilder();
            int bytes = 0;
            byte[] dataBuffer = new byte[256];
            ArraySegment<byte> bufferSegment = new ArraySegment<byte>(dataBuffer);

            do
            {
                bytes = await handler.ReceiveAsync(bufferSegment, SocketFlags.None);
                stringBuilder.Append(Encoding.Unicode.GetString(bufferSegment.Array!, 0, bytes));

            } while (handler.Available > 0);

            Console.WriteLine($"In {DateTime.Now.ToShortTimeString()} from {adress} has been got string: {stringBuilder}");


            //answer to client
            string message = null!;
            if (stringBuilder.ToString() == "Date")
            {
                message = $"{DateTime.Now.ToShortDateString()}";
            }

            if (stringBuilder.ToString() == "Time")
            {
                message = $"{DateTime.Now.ToShortTimeString()}";
            }

            bufferSegment = Encoding.Unicode.GetBytes(message);
            await handler.SendAsync(bufferSegment, SocketFlags.None);
            //close socket
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

Server_Async_2("127.0.0.1", 8005);
Console.ReadLine();