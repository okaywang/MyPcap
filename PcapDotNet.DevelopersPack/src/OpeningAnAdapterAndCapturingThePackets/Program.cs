using System;
using System.Collections.Generic;
using PcapDotNet.Core;
using PcapDotNet.Packets;

namespace OpeningAnAdapterAndCapturingThePackets
{
    class Program
    {
        static void Main(string[] args)
        {
            // Retrieve the device list from the local machine
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;

            if (allDevices.Count == 0)
            {
                Console.WriteLine("No interfaces found! Make sure WinPcap is installed.");
                return;
            }

            // Print the list
            for (int i = 0; i != allDevices.Count; ++i)
            {
                LivePacketDevice device = allDevices[i];
                Console.Write((i + 1) + ". " + device.Name);
                if (device.Description != null)
                    Console.WriteLine(" (" + device.Description + ")");
                else
                    Console.WriteLine(" (No description available)");
            }

            int deviceIndex = 0;
            do
            {
                Console.WriteLine("Enter the interface number (1-" + allDevices.Count + "):");
                string deviceIndexString = 1.ToString();// Console.ReadLine();
                if (!int.TryParse(deviceIndexString, out deviceIndex) ||
                    deviceIndex < 1 || deviceIndex > allDevices.Count)
                {
                    deviceIndex = 0;
                }
            } while (deviceIndex == 0);

            // Take the selected adapter
            PacketDevice selectedDevice = allDevices[deviceIndex - 1];

            // Open the device
            using (PacketCommunicator communicator = 
                selectedDevice.Open(65536,                                  // portion of the packet to capture
                                                                            // 65536 guarantees that the whole packet will be captured on all the link layers
                                    PacketDeviceOpenAttributes.Promiscuous, // promiscuous mode
                                    1000))                                  // read timeout
            {
                Console.WriteLine("Listening on " + selectedDevice.Description + "...");

                // start the capture
                communicator.ReceivePackets(0, PacketHandler);
            }
        }
        private static int No = 1;
        // Callback function invoked by Pcap.Net for every incoming packet
        private static void PacketHandler(Packet packet)
        {
            if (No >6)
            {
                return;
            }
            var x = packet.IpV4.Source;

 

            Console.WindowWidth = Console.LargestWindowWidth;
            Console.Write(No++);
            Console.Write("\t");
            Console.Write("length:" + packet.Length);
            Console.Write("\t");
            Console.Write("Source:" + packet.IpV4.Source.ToString());
            Console.Write("\t");
            Console.Write("Destination:" + packet.IpV4.Destination.ToString());
            Console.Write("\t");
            Console.Write("Protocol:" + packet.IpV4.Protocol.ToString());
            Console.Write("\n");
        }
    }
}
