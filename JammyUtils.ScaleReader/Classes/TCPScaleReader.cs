using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Net.Sockets;
using System.IO;

namespace JammyUtils.ScaleReader.Classes
{

    /// <summary>
    /// Queries a scale device by IP/port to return weight data.
    /// </summary>
    public class TCPScaleReader
    {
        /// <summary>
        /// Queries a ZM303 scale device by IP/port to get weight data.
        /// </summary>
        /// <param name="address">IP address of scale.</param>
        /// <param name="port">Port of scale.</param>
        /// <param name="sendchar">Character to send (TODO: change to string to support SMA). Defaults to ENQ character: ChrW(5)</param>
        /// <param name="length">Length of bytes for read buffer. Default is 128, HIGHER VALUES CAN CAUSE TO FAIL so throw argument range exception</param>
        /// <returns>Scale's response. If it's an exception, it'll return the exception.ToString for now.</returns>
        /// <remarks>This was the most reliable method tested.</remarks>
        public async static Task<string> SendWeighRequest(string address, int port, char? sendchar = null, int length = 128 )
        {
            sendchar = sendchar ?? Strings.ChrW(5);

            try
            {
                var theclient = new TcpClient() { ReceiveTimeout = 250, SendTimeout = 250, NoDelay = true };

                await theclient.ConnectAsync(address, port);

                var netstream = theclient.GetStream();

                var thewriter = new StreamWriter(netstream);
                thewriter.Write(sendchar);
                thewriter.Flush();

                byte[] bytes = new byte[length];

                await netstream.ReadAsync(bytes, 0, bytes.Length);

                theclient.Close();

                return Encoding.ASCII.GetString(bytes);
            }
            catch (Exception ex)
            {
                //TODO: 1v1 the scale.
                //I mean check if tcpclient closed if this happens.
                return ex.ToString();
            }

        }


    }
}
