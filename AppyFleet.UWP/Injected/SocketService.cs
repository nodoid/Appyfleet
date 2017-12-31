using GalaSoft.MvvmLight.Ioc;
using mvvmframework;
using mvvmframework.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AppyFleet.UWP.Injected
{
    public class SocketService : ISockets
    {
        ILogFileService logService { get; set; } = SimpleIoc.Default.GetInstance<ILogFileService>();

        public bool SendMessage(string message, byte[] sReceivedData, byte[] sSuccess, int count)
        {
            var endPoint = new IPEndPoint(IPAddress.Parse(Constants.kIP), Constants.kPort);
            var rv = false;

            try
            {
                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp) { Blocking = true, ReceiveTimeout = 5000 })
                {
                    var data = Encoding.ASCII.GetBytes(message); // EncodingUtils.GetAsciiBytes
                    socket.SendTo(data, endPoint);
                    logService.WriteLog("JourneyManager:SendUDP", "Sent " + message);

                    int bytesReceived = socket.Receive(sReceivedData);
                    if (bytesReceived > 0) // if we receive anything, consider it an acknowledge (same as iOS)
                    {

                        if (sReceivedData.SequenceEqual(sSuccess))
                        {
                            logService.WriteLog("JourneyManager:SendUDP", $"Acknowledge received. Messages left to send: {count}");
                        }
                        else
                        {
                            try
                            {
                                var messageReceived = Encoding.ASCII.GetString(sReceivedData); // EncodingUtils.GetAsciiString(sReceivedData);
                                logService.WriteLog("JourneyManager:SendUDP", $"Received a UDP response, but it was not the expected response : {messageReceived}");
                            }
                            catch (Exception)
                            {
                                logService.WriteLog("JourneyManager:SendUDP", "Received a UDP response, but it was not the expected response and could not be parsed");
                            }

                            logService.WriteLog("JourneyManager:SendUDP", $"Send failed. Messages left to send: {count}");
                            rv = true;
                        }
                    }
                    else
                    {
                        logService.WriteLog("JourneyManager:SendUDP", "Failed to receive an acknowledge, postponing further sends");
                        rv = true;
                    }
                }
            }
            catch (SocketException sEx)
            {
                logService.WriteLog("JourneyManager:SendUDP", $"Send UDP failed for Socket Exception: {sEx.SocketErrorCode}");
                rv = true;
            }
            catch (Exception ex)
            {
                logService.WriteLog("JourneyManager:SendUDP", "Send UDP failed: " + ex.Message + ", Inner Exception = " + (ex.InnerException != null && ex.InnerException.Message != null ? ex.InnerException.Message : "NONE"));
                rv = true;
            }

            return rv;
        }
    }
}
