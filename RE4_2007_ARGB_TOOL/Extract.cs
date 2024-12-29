using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace RE4_2007_ARGB_TOOL
{
    internal static class Extract
    {
        public static void ExtractFile(FileInfo fileInfo) 
        {
            var diretory = Path.GetDirectoryName(fileInfo.FullName);
            var name = Path.GetFileNameWithoutExtension(fileInfo.Name);
            var outputpath = Path.Combine(diretory, name + ".png");

            var br = new BinaryReader(fileInfo.OpenRead());

            uint magic = br.ReadUInt32();
            if (magic != 0x41524742) //ARGB
            {
                Console.WriteLine("Invalid file!");
                br.Close();
                return;
            }

            br.ReadUInt32(); // 08 08 08 08

            int width = br.ReadInt32();
            int height = br.ReadInt32();

            byte[] colors = br.ReadBytes(width * height * 4);
            br.Close();

            Bitmap bitmap = new Bitmap(width, height, width * 4,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb,
                    System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(colors, 0));

            bitmap.Save(outputpath);
        }
    }
}
