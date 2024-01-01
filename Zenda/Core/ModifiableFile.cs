// Script for class for modifiying all file formats
// Written by BD7

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenda
{
    public abstract class ModifiableFile
    {
        private Stream stream;

        /// <summary>
        /// Gets the file's stream
        /// </summary>
        /// <returns>file's stream</returns>
        public Stream GetStream()
        {
            return stream;
        }
        /// <summary>
        /// Appends a new stream to the file's old stream
        /// </summary>
        /// <param name="newStream">The new stream to append to the old stream</param>
        public void SetStream(Stream newStream)
        {
            stream = newStream;
        }

        /// <summary>
        /// Loads the file's information from the file's stream.
        /// </summary>
        public virtual void Load()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Saves the file's information to the file's stream.
        /// </summary>
        public virtual void Save()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Disposes the current modifiable file
        /// </summary>
        public void Dispose()
        {
            stream.Close();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Starts a new modifiable file from a inherited class.
        /// </summary>
        /// <typeparam name="T">The inherited type to create the modifiable file</typeparam>
        /// <param name="stream">The stream to load</param>
        /// <returns></returns>
        public static ModifiableFile Create<T>(Stream stream)
            where T : ModifiableFile
        {
            ModifiableFile f = Activator.CreateInstance(typeof(ModifiableFile), true) as T;
            f.SetStream(stream);
            f.Load();
            return f;
        }

        public ModifiableFile() { }
        /// <summary>
        /// Constructs this modifiable file from a file path.
        /// </summary>
        /// <param name="fileName">The path to the file to load</param>
        public ModifiableFile(string fileName, bool readOnly = true)
        {
            // loads the file
            FileStream file = new FileStream(fileName, FileMode.Open, readOnly ? FileAccess.Read : FileAccess.ReadWrite);

            SetStream(file); // prepares the stream to this file
            Load(); // loads the file
        }
        /// <summary>
        /// Constructs this modifiable file from a stream.
        /// </summary>
        /// <param name="stream">The stream to load</param>
        public ModifiableFile(Stream stream)
        {
            SetStream(stream); // prepares the stream to this file
            Load(); // loads the file
        }
    }
}
