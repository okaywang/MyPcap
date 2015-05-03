using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacketAnalyst
{
    public class PacketDigest
    {
        public string SourceMac { get; set; }
        public string DestinationMac { get; set; }
        public string SourceIP { get; set; }
        public string DestinationIP { get; set; }
        public string Protocal { get; set; }
        public int Length { get; set; }
        public string Memo { get; set; }
    }
}
