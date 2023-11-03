// Script for Driver: Parallel Lines HUD files
// Integrated from Visual Edit by BuilderDemo7 for Zenda
// "Hey, this is... cool"
// Written by BuilderDemo7

using System;
using System.IO;
using System.Diagnostics;

namespace Zenda.DriverPL
{
	public class HUDElement {
		// Main values
		public byte Type; // 1 byte (byte)
		public byte Pad; // 1 byte (byte)
		public short Group; // 2 bytes (int16)
		public short Input; // 2 bytes (int16)
		public short Id; // 2 bytes (int16)
		
		// Drawing
		public float X,Y; // 8 bytes (2 floats)
		public float Width,Height; // 8 bytes (2 floats)
		public float SizeX,SizeY; // 8 bytes (2 floats)
	    public float R,G,B,A; // 16 bytes (4 floats)	

        // Other
        private HUD _hud;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public HUD GetHUD() {
        	return _hud;
        }
	    // Useful function (took from HUDElement class)
	    /// <summary>
	    /// Returns a byte[] join of A with B like table.concat() from Lua
	    /// </summary>		
	    private static byte[] byteJoin(byte[] a,byte[] b) {
	    	byte[] j = new byte[a.Length+b.Length];
	    	for (int i=0;i<a.Length+b.Length;i++) {
	    		if (i<b.Length-1) {
	    			j[i] = a[i];
	    		}
	    		else {
	    			j[i] = b[i-b.Length];
	    		}
	    	}
	    	return j;
	    }		
	    // Useful function
	    /// <summary>
	    /// Returns a block of byte[size] from given offset from a array like string.sub() from Lua
	    /// </summary>		
		private static byte[] byteSub(byte[] array,int off,int size) {
			byte[] ret = new byte[size];
			for (int i=0;i<size;i++) {
				ret[i] = array[i+off];
			}
			return ret;
		}
	    // Useful function
	    /// <summary>
	    /// Overwrites existing bytes from array with bytes
	    /// </summary>		
	    private static byte[] byteWrite(byte[] array,byte[] bytes,int off,int size) {
	    	byte[] a = byteSub(array,0,size-off);
	    	byte[] m = byteSub(array,off,size);
	    	byte[] b = byteSub(array,(off+size),size-(off+size));
	    	byte[] am = byteJoin(a,m);
	    	byte[] amb = byteJoin(am,b);
	    	return amb;
	    }
	    
	    private void fromBinary(byte[] buffer) {
	    	// 2 bytes (byte)
	    	this.Type = buffer[0];
	    	Pad = buffer[1];
	    	// 6 bytes (3 int16)
	    	Group = BitConverter.ToInt16(buffer,2);
	    	Input = BitConverter.ToInt16(buffer,4);
	    	Id = BitConverter.ToInt16(buffer,6);
	        // 40 bytes (10 floats)
	    	X = BitConverter.ToSingle(buffer,8); Y = BitConverter.ToSingle(buffer,12);
	    	Width = BitConverter.ToSingle(buffer,16); Height = BitConverter.ToSingle(buffer,20);
	        SizeX = BitConverter.ToSingle(buffer,24); SizeY = BitConverter.ToSingle(buffer,28);
	        R = BitConverter.ToSingle(buffer,32); G = BitConverter.ToSingle(buffer,36); B = BitConverter.ToSingle(buffer,40); A = BitConverter.ToSingle(buffer,44);
	        // Binary conversion done	    	
	    }
	    
	    /// <summary>
	    /// HUD Element class (Driv3r)
	    /// MAIN PARAMETERS:
	    /// t: Type of the element, pad: Drawing order of element, gr: Group of the element, id: Choosen Id to be formatted in the element (texture, text, etc.)
	    /// DRAWING PARAMETERS:
	    /// x: X of the element, y: Y of the element,
	    /// w: Width, h: Height,
	    /// sx: Size X, sy: Size Y,
	    /// r,g,b,a: Red, green, blue and alpha of the element
	    /// </summary>		
	    // Used to instantiate a new HUD Element without a buffer
		public HUDElement(byte t,byte pad,short grp,short input,short id,float x,float y,float w,float h,float sx,float sy,float r,float g,float b,float a) {
		    this.Type = t;
		    Pad = pad; Group = grp; Input = input; Id = id; 
		    X = x; Y = y;
		    Width = w;
		}
	    // Same code as above but indexing the HUD
		public HUDElement(HUD parenthud,byte t,byte pad,short grp,short input,short id,float x,float y,float w,float h,float sx,float sy,float r,float g,float b,float a) {
		    _hud = parenthud;
	    	this.Type = t;
		    Pad = pad; Group = grp; Input = input; Id = id; 
		    X = x; Y = y;
		    Width = w;
		}
	    // Used to get a new HUD Element from a buffer
	    public HUDElement(byte[] buffer) {
	    	fromBinary(buffer);
	    }
	    // Same as code above but indexing the HUD x2
	    public HUDElement(HUD parenthud,byte[] buffer) {
	    	_hud = parenthud;
	    	fromBinary(buffer);
	    }
	    
	    // Function to convert this HUD element to binary
	    public byte[] ToBinary() {
	    	// Work in progress
	    	int HUDElementBufferSize = 112; // just in case the HUD doesn't exist
	    	if (_hud!=null) {
	    	   HUDElementBufferSize = _hud.HUDElementBufferSize; // alright, the HUD exists. get the buffer size already!
	    	}
	    	byte[] buffer = new byte[HUDElementBufferSize];
	    	// bytes
	    	buffer[0] = Type;
	    	buffer[1] = Pad;
	    	// "Everything is going my way"
	    	// shorts
	    	buffer = byteWrite(buffer,BitConverter.GetBytes(Group),2,2);
	    	buffer = byteWrite(buffer,BitConverter.GetBytes(Input),4,2);
	    	buffer = byteWrite(buffer,BitConverter.GetBytes(Id),6,2);
	    	// floats
	    	buffer = byteWrite(buffer,BitConverter.GetBytes(X),8,4);
	    	buffer = byteWrite(buffer,BitConverter.GetBytes(Y),12,4);
	    	buffer = byteWrite(buffer,BitConverter.GetBytes(Width),16,4);
	    	buffer = byteWrite(buffer,BitConverter.GetBytes(Height),20,4);
	    	buffer = byteWrite(buffer,BitConverter.GetBytes(SizeX),24,4);
	    	buffer = byteWrite(buffer,BitConverter.GetBytes(SizeY),28,4);
	    	// the last floats
	    	buffer = byteWrite(buffer,BitConverter.GetBytes(R),32,4);
	    	buffer = byteWrite(buffer,BitConverter.GetBytes(G),36,4);
	    	buffer = byteWrite(buffer,BitConverter.GetBytes(B),40,4);
	    	buffer = byteWrite(buffer,BitConverter.GetBytes(A),44,4);
	    	return buffer;
	    }
	}
	/// <summary>
	/// HUD (.bin) file (Driv3r)
	/// </summary>
	public class HUD
	{
		// For HUD elements
		public readonly int HUDElementBufferSize = 112;
		
		// For header
		public ushort Version; // 2 bytes (int16) (no check as DriverPL/HUD.cs probably exists)
		private ushort Magic1; // 2 bytes (int16)
		private int Count; // 4 bytes (int32)
		
		// Other...
		private long Align; // 8 bytes (int64)
		private ushort Magic2; // 2 bytes (int16)
		
		// HUD elements are indexed here
		public HUDElement[] Elements;

	    // Useful function (took from HUDElement class)
	    /// <summary>
	    /// Returns a block of byte[size] from given offset from a array like string.sub() from Lua
	    /// </summary>		
		private static byte[] byteSub(byte[] array,int off,int size) {
			byte[] ret = new byte[size];
			for (int i=0;i<size;i++) {
				ret[i] = array[i+off];
			}
			return ret;
		}		
	    // Useful function (took from HUDElement class)
	    /// <summary>
	    /// Returns a byte[] join of A with B like table.concat() from Lua
	    /// </summary>		
	    private static byte[] byteJoin(byte[] a,byte[] b) {
	    	byte[] j = new byte[a.Length+b.Length];
	    	for (int i=0;i<a.Length+b.Length;i++) {
	    		if (i<b.Length-1) {
	    			j[i] = a[i];
	    		}
	    		else {
	    			j[i] = b[i-b.Length];
	    		}
	    	}
	    	return j;
	    }
	    // Useful function
	    /// <summary>
	    /// Overwrites existing bytes from array with bytes
	    /// </summary>		
	    private static byte[] byteWrite(byte[] array,byte[] bytes,int off,int size) {
	    	byte[] a = byteSub(array,0,size-off);
	    	byte[] m = byteSub(array,off,size);
	    	byte[] b = byteSub(array,(off+size),size-(off+size));
	    	byte[] am = byteJoin(a,m);
	    	byte[] amb = byteJoin(am,b);
	    	return amb;
	    }
	    
	    // Used in Driver: Parallel Lines only...
		//private int Width;
		//private int Height;
		// Easy function to get from binary for both functions below
		private void fromBinary(byte[] buffer) {
			// 4 bytes (2 int16)
			this.Version = BitConverter.ToUInt16(buffer,0);
			Magic1 = BitConverter.ToUInt16(buffer,2);
			// 4 bytes (1 int32)
			Count = BitConverter.ToInt32(buffer,4);
			// 8 bytes (1 int64)
			Align = BitConverter.ToInt64(buffer,8);
			// 2 bytes (1 int16)
			Magic2 = BitConverter.ToUInt16(buffer,16);	

            // time to get the elements now
            Elements = new HUDElement[Count]; // index
            // binary > HUDElement
            int off = 64; //(int)Convert.ToDecimal(Align); 
            #if DEBUG
                Debug.WriteLine(String.Format("HUD HEADER: Version = {0}, Magic 1 = {1} Count = {2}, Align = {3}, Magic 2 = {4}",this.Version,Magic1,Count,Align,Magic2));
            #endif
            for (int id=0;id<Count;id++) {
            	if (buffer.Length-off==0) {
            		throw new Exception("Attempt to process more bytes out of the buffer matrix");
            	}
            	// debug
            	#if DEBUG
            	     Debug.WriteLine(String.Format("{0} --> {1}, ID: {2}",off,buffer.Length-off,id));
            	#endif
            	Elements[id] = new HUDElement( this, byteSub(buffer,off,HUDElementBufferSize) );
            	off = off+HUDElementBufferSize;
            }
            #if DEBUG
                 Debug.WriteLine("HUD Done loading!");
            #endif
		}
		
		// Get binary HUD from given file name
		public HUD(string filename)
		{
			FileStream file = new FileStream(filename,FileMode.Open);
			if (file!=null) {
				fromBinary(file.ReadAllBytes());
			}
		}
		// Get binary HUD from given FileStream
		public HUD(FileStream file)
		{
			if (file!=null) {
				fromBinary(file.ReadAllBytes());
			}			
		}
		// Function to convert this HUD file to binary 
		// (a FileStream is required after the buffer is get to save it as a file)
		public byte[] Save() {
			byte[] buffer = new byte[64];
			// shorts
			buffer = byteWrite(buffer,BitConverter.GetBytes(this.Version),0,2);
			buffer = byteWrite(buffer,BitConverter.GetBytes(Magic1),2,2);
			// int32
			buffer = byteWrite(buffer,BitConverter.GetBytes(Elements.Length),4,4);
			// long
			buffer = byteWrite(buffer,BitConverter.GetBytes(Align),8,8);
			// short
			buffer = byteWrite(buffer,BitConverter.GetBytes(Magic2),16,8);
			
			// The rest is 0s because there is nothing else
			
			for (int id=0;id<Elements.Length;id++) {
				buffer = byteJoin(buffer,Elements[id].ToBinary());
			}
			
			return buffer;
			//throw new NotImplementedException(); // placeholder
			//BitConverter.GetBytes( // A'ight, I'm goin' to sleep, see ya tomorrow
		}
	}
}
