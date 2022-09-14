// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;

static void Client_1(string adress, int port)
{
	try
	{
		IPAddress iPAddress = IPAddress.Parse(adress);
		IPEndPoint ipEP = new IPEndPoint(iPAddress, port);
		Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //connect to the remote host
		socket.Connect(ipEP);
		//Console.Write("Enter message: ");
		//string message = Console.ReadLine()!;
		string message = "Hello server!";
		byte[] dataBuff = Encoding.Unicode.GetBytes(message);
		socket.Send(dataBuff);

		//get answer
		dataBuff = new byte[256];
		StringBuilder stringBuilder = new StringBuilder();
		int bytes = 0;

		do
		{
			bytes = socket.Receive(dataBuff, dataBuff.Length, 0);
			stringBuilder.Append(Encoding.Unicode.GetString(dataBuff, 0, bytes));

		} while (socket.Available > 0);

		Console.WriteLine("Answer server:\n" + stringBuilder.ToString());

		//close socket
		socket.Shutdown(SocketShutdown.Both);
		socket.Close();
    }
    catch (Exception ex)
	{
		Console.WriteLine(ex.Message);
	}
	Console.ReadLine();
}

Client_1("127.0.0.1", 8005);