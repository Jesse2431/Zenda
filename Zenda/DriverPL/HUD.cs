using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenda.DriverPL
{
    public class HUDElement
    {
        public struct HUDElementData
        {
            public float R { get; set; }
            public float G { get; set; }
            public float B { get; set; }
            public float A { get; set; }

            public float X { get; set; }
            public float Y { get; set; }
            public float Width { get; set; }
            public float Height { get; set; }

            // Driver: Parallel Lines HUD element contains a useful quad system to select a part of the texture
            public float QuadX { get; set; }
            public float QuadY { get; set; }
            
            // I'm not sure if this is part of the quad.
            public float SizeX { get; set; }
            public float SizeY { get; set; }

            public float QuadWidth { get; set; }
            public float QuadHeight { get; set; }

            public uint TextureId { get; set; }
            public int Flags { get; set; }
            public int Unk1 { get; set; }
            public int Unk2 { get; set; }
            public int Unk3 { get; set; }
            public int Unk4 { get; set; }
            public int Unk5 { get; set; }
            public int Unk6 { get; set; }
            public int Unk7 { get; set; }
            public int Unk8 { get; set; }
            public int Unk9 { get; set; }
            public int Unk10 { get; set; }
            public int Unk11 { get; set; }
            public int Unk12 { get; set; }
        }

        public HUDElementData Data = new HUDElementData();

        public void Load(Stream stream)
        {
            using (var br = new BinaryReader(stream, Encoding.UTF8, true))
            {
                Data.R = br.ReadSingle();
                Data.G = br.ReadSingle();
                Data.B = br.ReadSingle();
                Data.A = br.ReadSingle();

                Data.X = br.ReadSingle();
                Data.Y = br.ReadSingle();
                Data.Width = br.ReadSingle();
                Data.Height = br.ReadSingle();

                Data.QuadX = br.ReadSingle();
                Data.QuadY = br.ReadSingle();

                Data.SizeX = br.ReadSingle();
                Data.SizeY = br.ReadSingle();

                Data.QuadWidth = br.ReadSingle();
                Data.QuadHeight = br.ReadSingle();

                Data.TextureId = br.ReadUInt32();
                Data.Flags = br.ReadInt32();

                // Unknown
                Data.Unk1 = br.ReadInt32();
                Data.Unk2 = br.ReadInt32();
                Data.Unk3 = br.ReadInt32();
                Data.Unk4 = br.ReadInt32();
                Data.Unk5 = br.ReadInt32();
                Data.Unk6 = br.ReadInt32();
                Data.Unk7 = br.ReadInt32();
                Data.Unk8 = br.ReadInt32();
                Data.Unk9 = br.ReadInt32();
                Data.Unk10 = br.ReadInt32();
                Data.Unk11 = br.ReadInt32();
                Data.Unk12 = br.ReadInt32();
            }
        }

        public void Save(Stream stream)
        {
            using (var bw = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                bw.Write(Data.R);
                bw.Write(Data.G);
                bw.Write(Data.B);
                bw.Write(Data.A);

                bw.Write(Data.X);
                bw.Write(Data.Y);
                bw.Write(Data.Width);
                bw.Write(Data.Height);

                bw.Write(Data.QuadX);
                bw.Write(Data.QuadY);

                bw.Write(Data.SizeX);
                bw.Write(Data.SizeY);

                bw.Write(Data.QuadWidth);
                bw.Write(Data.QuadHeight);

                bw.Write(Data.TextureId);
                bw.Write(Data.Flags);

                // Unknown
                bw.Write(Data.Unk1);
                bw.Write(Data.Unk2);
                bw.Write(Data.Unk3);
                bw.Write(Data.Unk4);
                bw.Write(Data.Unk5);
                bw.Write(Data.Unk6);
                bw.Write(Data.Unk7);
                bw.Write(Data.Unk8);
                bw.Write(Data.Unk9);
                bw.Write(Data.Unk10);
                bw.Write(Data.Unk11);
                bw.Write(Data.Unk12);
            }
        }

        public HUDElement() { }
        public HUDElement(Stream stream) { Load(stream); }
    }
    // NOTE: Not inherited from Driv3r's HUD class because using expression "hud is Zenda.DriverPL.HUD" won't work right
    public class HUD : ModifiableFile
    {
        public struct HUDHeader
        {
            public uint Format { get; set; }
            public ulong HeaderSize { get; set; }
            public uint Flags { get; set; }

            // Used to fix aspect ratio problems in other computer monitors?
            public float ScreenSizeX { get; set; }
            public float ScreenSizeY { get; set; }
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

                Header.ScreenSizeX = br.ReadSingle();
                Header.ScreenSizeY = br.ReadSingle();

                Elements = new List<HUDElement>(count);
                stream.Position += (int)(Header.HeaderSize - (ulong)System.Runtime.InteropServices.Marshal.SizeOf(typeof(HUDHeader)));
                for (int id = 0; id < count; id++)
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

                bw.Write(Header.ScreenSizeX);
                bw.Write(Header.ScreenSizeY);

                bw.Write(new byte[((int)(Header.HeaderSize - (ulong)System.Runtime.InteropServices.Marshal.SizeOf(typeof(HUDHeader))))]);
                // will automatically write the element's content to the stream
                foreach (HUDElement element in Elements)
                {
                    element.Save(stream);
                }
            }
        }

        public HUD() { }
        public HUD(Stream stream) : base(stream) { }
        public HUD(string fileName, bool readOnly = true) : base(fileName, readOnly) { }
    }
}
