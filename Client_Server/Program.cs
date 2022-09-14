// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Sockets;
using System.Text;

static void Server_1(string adress, int port)
{
	IPAddress iPAddress = IPAddress.Parse(adress);
    IPEndPoint localEP = new IPEndPoint(iPAddress, port);
    Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	try
	{
		listenSocket.Bind(localEP);
		listenSocket.Listen(10);
		Console.WriteLine("Server was started. Waiting for connection...");

		while (true)
		{
			Socket handler = listenSocket.Accept();
			StringBuilder stringBuilder = new StringBuilder();
			int bytes = 0; //count of bytes
			byte[] dataBuff = new byte[256]; //buffer for getting datas

			do
			{
				bytes = handler.Receive(dataBuff);
				stringBuilder.Append(Encoding.Unicode.GetString(dataBuff, 0, bytes));

			} while (handler.Available > 0);

			Console.WriteLine($"In {DateTime.Now.ToShortTimeString()} from {adress} has been got the string: {stringBuilder}");

			//Answer
			//string message = "Your message has been sent";
			string message = $"In {DateTime.Now.ToShortTimeString()} from {adress} has been got the string: Hello, client!";
			dataBuff = Encoding.Unicode.GetBytes(message);
			handler.Send(dataBuff);
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

Server_1("127.0.0.1", 8005);