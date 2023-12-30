using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenda.Driv3r
{
    class HUDElement
    {
        public struct Header
        {
            public uint Format { get; set; }
            public ulong HeaderSize { get; set; }
            public uint Flags { get; set; }
        }
    }
    class HUD : ModifiableFile
    {
        public struct Header
        {
            public uint Format { get; set; }
            public ulong HeaderSize { get; set; }
            public uint Flags { get; set; }
        }
    }
}
