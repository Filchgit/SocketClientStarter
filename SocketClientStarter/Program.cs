using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientStarter
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket client = null;
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // TcpClient tcpClient = new TcpClient();      could just do it this was as well I guess

            IPAddress ipaddr = null;

            try
            {
                Console.WriteLine("Please enter a Valid Server Address and Press Enter:");
                string strIPAddress = Console.ReadLine();
                Console.WriteLine("Please enter a Valid Port number 0 to 65535 and press enter,");
                string strPortInput = Console.ReadLine();
                int portInput = 0;

                if (!IPAddress.TryParse(strIPAddress, out ipaddr))
                //this returns a true/false bool as well as the out of the tryparse if sucessful.
                {
                    Console.WriteLine("IP Address entered was invalid.");
                    return;
                }

                if (!int.TryParse(strPortInput.Trim(), out portInput))
                //note that the input was in a string so first need to trim and convert to integer
                {
                    Console.WriteLine("Invalid Port number supplied.");
                    return;
                }
                if (portInput <= 0 || portInput > 65535)
                {
                    Console.WriteLine("Port number must be between  0 and 65535.");
                    return;
                }
                System.Console.WriteLine(string.Format("IPaddress: {0} - Prt {1}", ipaddr.ToString(), portInput));

                client.Connect(ipaddr, portInput);
                // tcpClient.Connect(ipaddr, portInput);
            
                Console.WriteLine("Connected to the server, type text and press enter to send it to the server, type <EXIT> to close.");
                string inputCommand = string.Empty;
                while (true)
                {
                    inputCommand = Console.ReadLine();
                    if (inputCommand.Equals("<EXIT>"))
                     { break; }
                    byte[] buffSend = Encoding.ASCII.GetBytes(inputCommand);
                    client.Send(buffSend);
                    //note that I could just use client.Send(Encoding.ASCII.GetBytes(inputCommand));
                    //  client.Send(Encoding.ASCII.GetBytes(inputCommand));

                    byte[] buffReceived = new byte[128];
                    int recInt = client.Receive(buffReceived);

                    Console.WriteLine("Data received: {0}", Encoding.ASCII.GetString(buffReceived, 0, recInt));
                }
            }


            catch (Exception excp)
            {
                Console.WriteLine(excp.ToString());
            }
            finally 
            {  if (client != null)
                    // otherwise these commands would cause errors and make program crash
                {
                    if (client.Connected)
                    {
                        client.Shutdown(SocketShutdown.Both);
                    }
                    client.Close();
                    client.Dispose();
                }
            }
            Console.WriteLine("Press a key to exit. . . ");
            Console.ReadKey();
        }

    }
}
