using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PcapDotNet.Packets;
using PcapDotNet.Core;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;

namespace PacketAnalyst
{
    public class PacketStatistics:IDisposable
    {
        private PacketDevice _device;
        private PacketCommunicator _communicator;
        private Thread _grabTthread;

        public event Action<Packet> PacketRecieved;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        public PacketStatistics(PacketDevice device)
        {
            _device = device;
            _communicator = _device.Open(100, PacketDeviceOpenAttributes.Promiscuous, 1000);
            _grabTthread = new Thread(_GrapPacket);
            _grabTthread.Name = "thread used to grap packet";
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(GrabPacket);
        }

        internal void SetFilter(string filterString)
        {
            var filter = _communicator.CreateFilter(filterString);
            _communicator.SetFilter(filter);
        }
        internal void GrabPacket(object sender, DoWorkEventArgs e)
        {
            _GrapPacket();
        }
        internal void GrabPacket()
        { 
            //_grabTthread.Start();
            //_GrapPacket();
            backgroundWorker1.RunWorkerAsync();
        }
        internal void StopGrab()
        {
            _grabTthread.Suspend();
        }

        private void _GrapPacket() {
            _communicator.ReceivePackets(0, PacketHandler); 
        }
        
        private void PacketHandler(Packet packet)
        {
            if (PacketRecieved != null)
            {
                PacketRecieved(packet);
            }
        }


        

        public void Dispose()
        {
            _grabTthread.Abort();
            _communicator.Dispose();
            backgroundWorker1.Dispose();
        }

        
    }
}
