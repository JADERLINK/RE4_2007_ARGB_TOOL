using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace RE4_2007_ARGB_TOOL
{
    internal static class Repack
    {
        public unsafe static void RepackFile(FileInfo fileInfo) 
        {
            var diretory = Path.GetDirectoryName(fileInfo.FullName);
            var name = Path.GetFileNameWithoutExtension(fileInfo.Name);
            var outputpath = Path.Combine(diretory, name + ".argb");

            Bitmap bitmap = new Bitmap(fileInfo.FullName);
            var lockBits = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            var source = (byte*)lockBits.Scan0.ToPointer();

            byte[] Pixel = new byte[bitmap.Width * bitmap.Height * 4];

            fixed (byte* p = Pixel)
            {
                for (var i = 0; i < Pixel.Length; i += 4)
                {
                    p[i + 0] = source[i + 0];
                    p[i + 1] = source[i + 1];
                    p[i + 2] = source[i + 2];
                    p[i + 3] = source[i + 3];
                }
            }
            bitmap.UnlockBits(lockBits);

            BinaryWriter bw = new BinaryWriter(new FileInfo(outputpath).Create());
            bw.Write((uint)0x41524742);
            bw.Write((uint)0x08080808);
            bw.Write((uint)bitmap.Width);
            bw.Write((uint)bitmap.Height);
            bw.Write(Pixel);
            bw.Close();
        }
    }
}
