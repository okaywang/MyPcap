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
    public class PacketMonitor:IDisposable
    {
        private PacketDevice _device;
        private PacketCommunicator _communicator;
        private Thread _grabTthread;

        public event Action<Packet> PacketRecieved;
        public event Action<PacketSampleStatistics> StatisticsRecieved;
        
        private System.ComponentModel.BackgroundWorker _backgroundWorkerCapture;
        private System.ComponentModel.BackgroundWorker _backgroundWorkerStatistics;
        public PacketMonitor(PacketDevice device,PacketCommunicatorMode mode)
        {
            _device = device;
            _communicator = _device.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000);
            _communicator.Mode = mode;
            _grabTthread = new Thread(_GrapPacket);
            _grabTthread.Name = "thread used to grap packet";
            _backgroundWorkerCapture = new System.ComponentModel.BackgroundWorker();
            _backgroundWorkerCapture.DoWork += new System.ComponentModel.DoWorkEventHandler(GrabPacket);

            _backgroundWorkerStatistics = new BackgroundWorker();
            _backgroundWorkerStatistics.DoWork += new System.ComponentModel.DoWorkEventHandler(StatisticsTraffic);
        }

        internal void SetFilter(string filterString)
        {
            var filter = _communicator.CreateFilter(filterString);
            _communicator.SetFilter(filter);
        }

        
         
        internal void StatisticsTraffic()
        {
            _backgroundWorkerStatistics.RunWorkerAsync();
        } 
        internal void StatisticsTraffic(object sender, DoWorkEventArgs e)
        {
            _StatisticsTraffic();
        }
        private void _StatisticsTraffic()
        {
            _communicator.ReceiveStatistics(0, OnStatisticsRecieved);
        }

       
        internal void GrabPacket()
        {  
            _backgroundWorkerCapture.RunWorkerAsync();
        } 
        internal void GrabPacket(object sender, DoWorkEventArgs e)
        {
            _GrapPacket();
        } 
        private void _GrapPacket()
        {
            _communicator.ReceivePackets(0, OnPacketRecieved);
        }

        internal void StopGrab()
        {
            _grabTthread.Suspend();
        }

        
        private void OnPacketRecieved(Packet packet)
        {
            if (PacketRecieved != null)
            {
                PacketRecieved(packet);
            }
        }

        private void OnStatisticsRecieved(PacketSampleStatistics statistics)
        {
            if (StatisticsRecieved != null)
            {
                StatisticsRecieved(statistics);
            }
        }

        

        public void Dispose()
        {
            _grabTthread.Abort();
            _communicator.Dispose();
            _backgroundWorkerCapture.Dispose();
        }

        
    }
}
