// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;

static async void Client_Async_1(string adress, int port)
{
    IPAddress iPAddress = IPAddress.Parse(adress);
    IPEndPoint ipEP = new IPEndPoint(iPAddress, port);
    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    try
    {
        //connect to the remote host
        await socket.ConnectAsync(ipEP);
        //Console.Write("Enter message: ");
        //string message = Console.ReadLine()!;
        string message = "Hello server!";
        byte[] dataBuff = Encoding.Unicode.GetBytes(message);

        ArraySegment<byte> bufferSegment = new ArraySegment<byte>(dataBuff);

        await socket.SendAsync(new ArraySegment<byte>(dataBuff), SocketFlags.None);

        //get answer
        dataBuff = new byte[256];
        StringBuilder stringBuilder = new StringBuilder();
        int bytes = 0;

        do
        {
            bytes = await socket.ReceiveAsync(bufferSegment, SocketFlags.None);
            stringBuilder.Append(Encoding.Unicode.GetString(bufferSegment.Array!, 0, bytes));

        } while (bytes > 0);

        Console.WriteLine("Answer server:\n" + stringBuilder.ToString());

        //close socket
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }
    catch (SocketException ex)
    {
        Console.WriteLine(ex.Message);
    }
    Console.ReadLine();
}

Client_Async_1("127.0.0.1", 8005);
Console.ReadLine();