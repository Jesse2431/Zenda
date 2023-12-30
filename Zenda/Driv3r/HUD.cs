using System;
using System.IO;    
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenda.Driv3r
{
    public class HUDElement
    {
        public struct HUDElementData
        {
            public ushort Type { get; set; }
            public ushort Group { get; set; }
            public ushort Input { get; set; }
            public ushort LocalisedStringId { get; set; }

            public float X { get; set; }
            public float Y { get; set; }
            public float Width { get; set; }
            public float Height { get; set; }
            public float SizeX { get; set; }
            public float SizeY { get; set; }

            public float R { get; set; }
            public float G { get; set; }
            public float B { get; set; }
            public float A { get; set; }
        }

        public HUDElementData Data = new HUDElementData();

        public void Load(Stream stream)
        {
            using (var br = new BinaryReader(stream, Encoding.UTF8, true))
            {
                Data.Type = br.ReadUInt16();
                Data.Group = br.ReadUInt16();
                Data.Input = br.ReadUInt16();
                Data.LocalisedStringId = br.ReadUInt16();

                Data.X = br.ReadSingle();
                Data.Y = br.ReadSingle();
                Data.Width = br.ReadSingle();
                Data.Height = br.ReadSingle();
                Data.SizeX = br.ReadSingle();
                Data.SizeY = br.ReadSingle();

                Data.R = br.ReadSingle();
                Data.G = br.ReadSingle();
                Data.B = br.ReadSingle();
                Data.A = br.ReadSingle();
            }
        }

        public void Save(Stream stream)
        {
            using (var bw = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                bw.Write(Data.Type);
                bw.Write(Data.Group);
                bw.Write(Data.Input);
                bw.Write(Data.LocalisedStringId);

                bw.Write(Data.X);
                bw.Write(Data.Y);
                bw.Write(Data.Width);
                bw.Write(Data.Height);
                bw.Write(Data.SizeX);
                bw.Write(Data.SizeY);

                bw.Write(Data.R);
                bw.Write(Data.G);
                bw.Write(Data.B);
                bw.Write(Data.A);
            }
        }

        public HUDElement() { }
        public HUDElement(Stream stream) { Load(stream); }
    }
    public class HUD : ModifiableFile
    {
        public struct HUDHeader
        {
            public uint Format { get; set; }
            public ulong HeaderSize { get; set; }
            public uint Flags { get; set; }
        }

        public HUDHeader Header = new HUDHeader();
        public List<HUDElement> Elements;

        public override void Load()
        {
            Stream stream = GetStream();
            Header = new HUDHeader();
            using (var br = new BinaryReader(stream, Encoding.UTF8, true))
            {
                Header.Format = br.ReadUInt32();
                int count = (int)br.ReadUInt32();
                Header.HeaderSize = br.ReadUInt64();
                Header.Flags = br.ReadUInt32();
                Elements = new List<HUDElement>(count);
                stream.Position += (int)( Header.HeaderSize - (ulong)System.Runtime.InteropServices.Marshal.SizeOf(typeof(HUDHeader)) );
                for(int id = 0; id<count; id++)
                {
                    Elements.Add(new HUDElement(stream));
                }
            }
        }

        public override void Save()
        {
            Stream stream = GetStream();
            stream.SetLength( 
                (long)Header.HeaderSize +
                (long)System.Runtime.InteropServices.Marshal.SizeOf(typeof(HUDElement.HUDElementData)) * (long)Elements.Count
                );
            using (var bw = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                bw.Write(Header.Format);
                bw.Write(Elements.Count);
                bw.Write(Header.HeaderSize);
                bw.Write(Header.Flags);
                bw.Write(new byte[((int)(Header.HeaderSize - (ulong)System.Runtime.InteropServices.Marshal.SizeOf(typeof(HUDHeader))))]);
                // will automatically write the element's content to the stream
                foreach(HUDElement element in Elements)
                {
                    element.Save(stream);
                }
            }
        }

        public HUD() { }
        public HUD(Stream stream) : base(stream) { }
        public HUD(string fileName) : base(fileName) { }
    }
}
