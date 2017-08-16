using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace bOS.Commons.IO
{
    public class StreamHelper
    {
        //
        // CopyStream is from 
        // http://stackoverflow.com/questions/411592/how-do-i-save-a-stream-to-a-file
        //
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

        public static void CopyStream2File(Stream stream, String filename)
        {
            using (FileStream localfs = File.OpenWrite(filename))
            {
                StreamHelper.CopyStream(stream, localfs);
            }
        }

    }
}
