using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PcapDotNet.Core;

namespace PacketAnalyst
{
    public partial class FrmPacketStatistics : Form
    {
        PacketMonitor _monitor;
        public FrmPacketStatistics()
        {
            InitializeComponent();

            var device = LivePacketDevice.AllLocalMachine.FirstOrDefault();
            _monitor = new PacketMonitor(device, PacketCommunicatorMode.Statistics);
            _monitor.StatisticsRecieved += DisplayStatistics;

            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _monitor.StatisticsTraffic();
        }
        private static DateTime _lastTimestamp;
        private void DisplayStatistics(PacketSampleStatistics statistics)
        {
            // Current sample time
            DateTime currentTimestamp = statistics.Timestamp;

            // Previous sample time
            DateTime previousTimestamp = _lastTimestamp;

            // Set _lastTimestamp for the next iteration
            _lastTimestamp = currentTimestamp;

            // If there wasn't a previous sample than skip this iteration (it's the first iteration)
            if (previousTimestamp == DateTime.MinValue)
                return;

            // Calculate the delay from the last sample
            double delayInSeconds = (currentTimestamp - previousTimestamp).TotalSeconds;

            // Calculate bits per second
            int bitsPerSecond = (Int32)( statistics.AcceptedBytes * 8 / delayInSeconds);

            // Calculate packets per second
            int packetsPerSecond = (Int32)(statistics.AcceptedPackets / delayInSeconds);

            // Print timestamp and samples
            var line = statistics.Timestamp + " BPS: " + bitsPerSecond + " PPS: " + packetsPerSecond;

            this.lstStatistics.Items.Add(line);
        } 
    }
}
