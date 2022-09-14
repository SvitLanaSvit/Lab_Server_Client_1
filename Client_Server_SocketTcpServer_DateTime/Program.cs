// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;

static void Server_2(string adress, int port)
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
			Socket handler = listenSocket.Accept();
			StringBuilder stringBuilder = new StringBuilder();
			int bytes = 0;
			byte[] dataBuffer = new byte[256];

			do
			{
				bytes = handler.Receive(dataBuffer);
				stringBuilder.Append(Encoding.Unicode.GetString(dataBuffer, 0, bytes));

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

			dataBuffer = Encoding.Unicode.GetBytes(message);
			handler.Send(dataBuffer);
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

Server_2("127.0.0.1", 8005);