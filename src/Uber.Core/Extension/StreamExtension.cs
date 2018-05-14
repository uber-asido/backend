using System;
using System.IO;

namespace Uber.Core
{
    public static class StreamExtension
    {
        public static byte[] ReadToEnd(this Stream stream)
        {
            long original_position = 0;

            if (stream.CanSeek)
            {
                original_position = stream.Position;
                stream.Position = 0;
            }

            try
            {
                var read_buffer = new byte[4096];

                var total_bytes_read = 0;
                int bytes_read;

                while ((bytes_read = stream.Read(read_buffer, total_bytes_read, read_buffer.Length - total_bytes_read)) > 0)
                {
                    total_bytes_read += bytes_read;

                    if (total_bytes_read == read_buffer.Length)
                    {
                        var next_byte = stream.ReadByte();
                        if (next_byte != -1)
                        {
                            byte[] temp = new byte[read_buffer.Length * 2];
                            Buffer.BlockCopy(read_buffer, 0, temp, 0, read_buffer.Length);
                            Buffer.SetByte(temp, total_bytes_read, (byte)next_byte);
                            read_buffer = temp;
                            total_bytes_read++;
                        }
                    }
                }

                var buffer = read_buffer;
                if (read_buffer.Length != total_bytes_read)
                {
                    buffer = new byte[total_bytes_read];
                    Buffer.BlockCopy(read_buffer, 0, buffer, 0, total_bytes_read);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = original_position;
                }
            }
        }
    }
}
