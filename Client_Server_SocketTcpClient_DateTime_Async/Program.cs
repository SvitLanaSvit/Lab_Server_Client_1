// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;
using System.Net;
using System.Text;

static async void Client_Async_2(string adress, int port)
{
    try
    {
        IPAddress address = IPAddress.Parse(adress);
        IPEndPoint endPoint = new IPEndPoint(address, port);
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        socket.Connect(endPoint);

        string message = null!;
        string str = "1. Show Date;\n" +
                     "2. Show Time.";
        Console.WriteLine(str);
        Console.Write("\nMake choose : ");
        int choose = int.Parse(Console.ReadLine()!);

        switch (choose)
        {
            case 1:
                {
                    message = "Date";
                    break;
                }
            case 2:
                {
                    message = "Time";
                    break;
                }
        }

        byte[] dataBuff = Encoding.Unicode.GetBytes(message);
        ArraySegment<byte> bufferSegment = new ArraySegment<byte>(dataBuff);
        await socket.SendAsync(bufferSegment, SocketFlags.None);

        //get answer
        dataBuff = new byte[256];
        StringBuilder stringBuilder = new StringBuilder();
        int bytes = 0;

        do
        {
            bytes = await socket.ReceiveAsync(bufferSegment, SocketFlags.None);
            stringBuilder.Append(Encoding.Unicode.GetString(bufferSegment.Array!, 0, bytes));

        } while (socket.Available > 0);

        Console.WriteLine($"Answer server: {stringBuilder}");

        //close socket
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();

    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
    Console.ReadLine();
}

Client_Async_2("127.0.0.1", 8005);
Console.ReadLine();