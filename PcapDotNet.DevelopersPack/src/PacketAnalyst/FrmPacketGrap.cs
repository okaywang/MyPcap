using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using PcapDotNet.Core;

namespace PacketAnalyst
{
    public partial class FrmPacketGrap : Form
    {
        private Canvas2 _canvas;
        PacketMonitor _monitor;

        public FrmPacketGrap()
        {
            InitializeComponent();

            var device = LivePacketDevice.AllLocalMachine.FirstOrDefault();
            _monitor = new PacketMonitor(device, PacketCommunicatorMode.Capture);
            _monitor.PacketRecieved += DisplayPacket;
            _canvas = new Canvas2(this.dgvPackets);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            var x = this.label2.Text;
        }

        private void Form1_Load(object sender, EventArgs e)
        { 
            this.dgvPackets.AllowUserToResizeRows = false;
            var properties = typeof(PacketDigest).GetProperties();
            foreach (var item in properties)
            { 
                this.dgvPackets.Columns.Add(item.Name,item.Name);
            } 

            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _canvas.Clear();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var filter = string.Empty;
            if (this.txtFilter.Text != string.Empty)
            {
                filter += this.txtFilter.Text; 
            }
            _monitor.SetFilter(filter);
            _monitor.GrabPacket();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _monitor.StopGrab();
        }

        private void DisplayPacket(Packet packet)
        {
            IpV4Datagram ip = packet.Ethernet.IpV4; 

            //var str = string.Format("{0}    {1}     {2}     {3}     protocal:{4}", packet.Timestamp.ToString("hh:mm:ss.fff"), packet.Length, ip.Source,ip.Destination,packet.ToString());
            //var str = string.Format("{0}",packet.Ethernet.IpV4.Protocol);

            //var digest = new PacketDigest();
            //digest.SourceIP = 
            //_canvas.WriteLine(str.ToString());
            PacketDigest digest = null;


            switch (packet.Ethernet.EtherType)
            {
                case PcapDotNet.Packets.Ethernet.EthernetType.AppleTalk:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.AppleTalkArp:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.Arp:
                    digest = GetDigest_Arp(packet);
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.AtaOverEthernet:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.AvbTransportProtocol:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.CircuitEmulationServicesOverEthernet:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.CobraNet:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.ConnectivityFaultManagementOrOperationsAdministrationManagement:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.EtherCatProtocol:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.ExtensibleAuthenticationProtocolOverLan:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.FibreChannelOverEthernet:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.FibreChannelOverEthernetInitializationProtocol:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.HomePlug:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.HyperScsi:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.IpV4:
                    digest = GetDigest_IPV4(packet);
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.IpV6:
                    return; 
                case PcapDotNet.Packets.Ethernet.EthernetType.MacControl:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.MacSecurity:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.MultiprotocolLabelSwitchingMulticast:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.MultiprotocolLabelSwitchingUnicast:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.None:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.Novell:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.NovellInternetworkPacketExchange:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.PointToPointProtocol:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.PointToPointProtocolOverEthernetDiscoveryStage:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.PointToPointProtocolOverEthernetSessionStage:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.PrecisionTimeProtocol:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.ProviderBridging:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.QInQ:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.ReverseArp:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.SerialRealTimeCommunicationSystemIii:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.VLanTaggedFrame:
                    break;
                case PcapDotNet.Packets.Ethernet.EthernetType.VeritasLowLatencyTransport:
                    break;
                default:
                    break;
            }

            _canvas.WriteLine(digest);
        }


        

        private PacketDigest GetDigest_IPV4(Packet packet)
        {
            PacketDigest digest = null;
            #region in the payload of IPV4
            switch (packet.Ethernet.IpV4.Protocol)
            {
                case IpV4Protocol.ActiveNetworks:
                    break;
                case IpV4Protocol.Any0HopProtocol:
                    break;
                case IpV4Protocol.AnyDistributedFileSystem:
                    break;
                case IpV4Protocol.AnyHostInternal:
                    break;
                case IpV4Protocol.AnyLocalNetwork:
                    break;
                case IpV4Protocol.AnyPrivateEncryptionScheme:
                    break;
                case IpV4Protocol.Argus:
                    break;
                case IpV4Protocol.Aris:
                    break;
                case IpV4Protocol.AuthenticationHeader:
                    break;
                case IpV4Protocol.Ax25:
                    break;
                case IpV4Protocol.BackroomSatMon:
                    break;
                case IpV4Protocol.BbnRccMonitoring:
                    break;
                case IpV4Protocol.Bna:
                    break;
                case IpV4Protocol.BulkDataTransferProtocol:
                    break;
                case IpV4Protocol.Cbt:
                    break;
                case IpV4Protocol.Cftp:
                    break;
                case IpV4Protocol.Chaos:
                    break;
                case IpV4Protocol.CombatRadioTransportProtocol:
                    break;
                case IpV4Protocol.CombatRadioUserDatagram:
                    break;
                case IpV4Protocol.CompaqPeer:
                    break;
                case IpV4Protocol.ComputerProtocolHeartbeat:
                    break;
                case IpV4Protocol.ComputerProtocolNetworkExecutive:
                    break;
                case IpV4Protocol.CrossNetDebugger:
                    break;
                case IpV4Protocol.DatagramCongestionControlProtocol:
                    break;
                case IpV4Protocol.DatagramDeliveryProtocol:
                    break;
                case IpV4Protocol.DcnMeasurement:
                    break;
                case IpV4Protocol.DiiDataExchange:
                    break;
                case IpV4Protocol.DissimilarGatewayProtocol:
                    break;
                case IpV4Protocol.Emcon:
                    break;
                case IpV4Protocol.EncapsulationHeader:
                    break;
                case IpV4Protocol.EnhancedInteriorGatewayRoutingProtocol:
                    break;
                case IpV4Protocol.Esp:
                    break;
                case IpV4Protocol.EtherIp:
                    break;
                case IpV4Protocol.ExteriorGatewayProtocol:
                    break;
                case IpV4Protocol.FibreChannel:
                    break;
                case IpV4Protocol.Fire:
                    break;
                case IpV4Protocol.FragmentHeaderForIpV6:
                    break;
                case IpV4Protocol.GatewayToGateway:
                    break;
                case IpV4Protocol.Gmtp:
                    break;
                case IpV4Protocol.Gre:
                    break;
                case IpV4Protocol.Hip:
                    break;
                case IpV4Protocol.HostMonitoringProtocol:
                    break;
                case IpV4Protocol.Il:
                    break;
                case IpV4Protocol.IntegratedNetLayerSecurityProtocol:
                    break;
                case IpV4Protocol.InterDomainPolicyRoutingProtocol:
                    break;
                case IpV4Protocol.InterDomainPolicyRoutingProtocolControlMessageTransportProtocol:
                    break;
                case IpV4Protocol.InterDomainRoutingProtocol:
                    break;
                case IpV4Protocol.InteractiveAgentTransferProtocol:
                    break;
                case IpV4Protocol.InteriorGatewayProtocol:
                    break;
                case IpV4Protocol.InternetControlMessageProtocol:
                    digest = GetDigest_ICMP(packet);
                    break;
                case IpV4Protocol.InternetControlMessageProtocolForIpV6:
                    break;
                case IpV4Protocol.InternetGroupManagementProtocol:
                    break;
                case IpV4Protocol.InternetPacketCoreUtility:
                    break;
                case IpV4Protocol.InternetPluribusPacketCore:
                    break;
                case IpV4Protocol.InternetReliableTransactionProtocol:
                    break;
                case IpV4Protocol.InternetworkPacketExchangeInIp:
                    break;
                case IpV4Protocol.Ip:
                    break;
                case IpV4Protocol.IpComp:
                    break;
                case IpV4Protocol.IpIp:
                    break;
                case IpV4Protocol.IpV6:
                    break;
                case IpV4Protocol.IpV6HopByHopOption:
                    break;
                case IpV4Protocol.IpV6Opts:
                    break;
                case IpV4Protocol.IpV6Route:
                    break;
                case IpV4Protocol.Iplt:
                    break;
                case IpV4Protocol.IpsilonFlowManagementProtocol:
                    break;
                case IpV4Protocol.IsIsOverIpV4:
                    break;
                case IpV4Protocol.IsoIp:
                    break;
                case IpV4Protocol.IsoTransportProtocolClass4:
                    break;
                case IpV4Protocol.Kryptolan:
                    break;
                case IpV4Protocol.LArp:
                    break;
                case IpV4Protocol.LayerTwoTunnelingProtocol:
                    break;
                case IpV4Protocol.Leaf1:
                    break;
                case IpV4Protocol.Leaf2:
                    break;
                case IpV4Protocol.MagneticFusionEnergyNetworkServicesProtocol:
                    break;
                case IpV4Protocol.MeritInternodalProtocol:
                    break;
                case IpV4Protocol.Mobile:
                    break;
                case IpV4Protocol.MobileAdHocNetwork:
                    break;
                case IpV4Protocol.MobileHostRoutingProtocol:
                    break;
                case IpV4Protocol.MobileInternetworkingControlProtocol:
                    break;
                case IpV4Protocol.MobilityHeader:
                    break;
                case IpV4Protocol.MulticastTransportProtocol:
                    break;
                case IpV4Protocol.Multiplexing:
                    break;
                case IpV4Protocol.MultiprotocolLabelSwitchingInIp:
                    break;
                case IpV4Protocol.NArp:
                    break;
                case IpV4Protocol.NationalScienceFoundationNetworkInteriorGatewayProtocol:
                    break;
                case IpV4Protocol.NetworkVoice:
                    break;
                case IpV4Protocol.NoNextHeaderForIpV6:
                    break;
                case IpV4Protocol.OpenShortestPathFirst:
                    break;
                case IpV4Protocol.PacketRadioMeasurement:
                    break;
                case IpV4Protocol.PacketVideoProtocol:
                    break;
                case IpV4Protocol.PerformanceTransparencyProtocol:
                    break;
                case IpV4Protocol.Pin:
                    break;
                case IpV4Protocol.Pipe:
                    break;
                case IpV4Protocol.PragmaticGeneralMulticastTransportProtocol:
                    break;
                case IpV4Protocol.PrivateNetworkToNetworkInterface:
                    break;
                case IpV4Protocol.Pup:
                    break;
                case IpV4Protocol.Qnx:
                    break;
                case IpV4Protocol.ReliableDatagramProtocol:
                    break;
                case IpV4Protocol.RemoteVirtualDiskProtocol:
                    break;
                case IpV4Protocol.Rsvp:
                    break;
                case IpV4Protocol.RsvpE2EIgnore:
                    break;
                case IpV4Protocol.SatMon:
                    break;
                case IpV4Protocol.SatnetAndBackroomExpak:
                    break;
                case IpV4Protocol.ScheduleTransferProtocol:
                    break;
                case IpV4Protocol.SecurePacketShield:
                    break;
                case IpV4Protocol.SecureVersatileMessageTransactionProtocol:
                    break;
                case IpV4Protocol.SemaphoreCommunicationsSecondProtocol:
                    break;
                case IpV4Protocol.ServiceSpecificConnectionOrientedProtocolInAMultilinkAndConnectionlessEnvironment:
                    break;
                case IpV4Protocol.SimpleMessageProtocol:
                    break;
                case IpV4Protocol.SitaraNetworksProtocol:
                    break;
                case IpV4Protocol.Skip:
                    break;
                case IpV4Protocol.Sm:
                    break;
                case IpV4Protocol.SourceDemandRoutingProtocol:
                    break;
                case IpV4Protocol.SpaceCommunicationsProtocolStandards:
                    break;
                case IpV4Protocol.SpectraLinkRadioProtocol:
                    break;
                case IpV4Protocol.SpriteRpc:
                    break;
                case IpV4Protocol.Stream:
                    break;
                case IpV4Protocol.StreamControlTransmissionProtocol:
                    break;
                case IpV4Protocol.SunNd:
                    break;
                case IpV4Protocol.Swipe:
                    break;
                case IpV4Protocol.Tcf:
                    break;
                case IpV4Protocol.Tcp:
                    digest = this.GetDigest_Tcp(packet);
                    break;
                case IpV4Protocol.ThirdPartyConnect:
                    break;
                case IpV4Protocol.TransportLayerSecurityProtocol:
                    break;
                case IpV4Protocol.TransportProtocolPlusPlus:
                    break;
                case IpV4Protocol.Trunk1:
                    break;
                case IpV4Protocol.Trunk2:
                    break;
                case IpV4Protocol.Ttp:
                    break;
                case IpV4Protocol.Udp:
                    break;
                case IpV4Protocol.UdpLite:
                    break;
                case IpV4Protocol.Uti:
                    break;
                case IpV4Protocol.VersatileMessageTransactionProtocol:
                    break;
                case IpV4Protocol.Vines:
                    break;
                case IpV4Protocol.VirtualRouterRedundancyProtocol:
                    break;
                case IpV4Protocol.Visa:
                    break;
                case IpV4Protocol.WangSpanNetwork:
                    break;
                case IpV4Protocol.WidebandExpak:
                    break;
                case IpV4Protocol.WidebandMonitoring:
                    break;
                case IpV4Protocol.XeroxNsInternetDatagramProtocol:
                    break;
                case IpV4Protocol.XpressTransportProtocol:
                    break;
                default:
                    break;
            }
            #endregion
            return digest;
        }

        private PacketDigest GetDigest_Arp(Packet packet)
        {
            var digest = new PacketDigest();
            digest.SourceMac = packet.Ethernet.Source.ToString();
            digest.DestinationMac = packet.Ethernet.Destination.ToString();
            digest.Length = packet.Length;
            digest.Protocal = packet.Ethernet.EtherType.ToString();
            digest.SourceIP = string.Empty;
            digest.DestinationIP = string.Empty;
            digest.Memo = packet.ToString();
            return digest;
        }

        private PacketDigest GetDigest_ICMP(Packet packet)
        { 
            var digest = new PacketDigest();
            digest.SourceMac = packet.Ethernet.Source.ToString();
            digest.DestinationMac = packet.Ethernet.Destination.ToString();
            digest.Length = packet.Length;
            digest.Protocal = packet.Ethernet.IpV4.Protocol.ToString();
            digest.SourceIP = packet.Ethernet.IpV4.Source.ToString();
            digest.DestinationIP = packet.Ethernet.IpV4.Destination.ToString();
            digest.Memo = packet.ToString();
            return digest;
        }
        private PacketDigest GetDigest_Tcp(Packet packet)
        { 

            var digest = new PacketDigest();
            digest.SourceMac = packet.Ethernet.Source.ToString();
            digest.DestinationMac = packet.Ethernet.Destination.ToString();
            digest.Length = packet.Length;
            digest.Protocal = packet.Ethernet.IpV4.Protocol.ToString();
            digest.SourceIP = packet.Ethernet.IpV4.Source.ToString();
            digest.DestinationIP = packet.Ethernet.IpV4.Destination.ToString();
            digest.Memo = packet.ToString();
            return digest;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._monitor.Dispose();
        }
    }
}
