using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;
using bOS.Commons.IO;

namespace bOS.Commons
{
    public static class Printer
    {
        public const string HtmlToPdfExePath = "wkhtmltopdf.exe";
        public const string HtmlToImageExePath = "wkhtmltoimage.exe";

        //sfortunatamente spesso la generazione diretta in TIFF ha qualche problemino per cui 
        //è necessario usare prima il png e poi convertirlo.
        public static bool GenerateTiff(string commandLocation, String url, String pngFile, String tiffFile)
        {
            Process p;
            ProcessStartInfo psi = new ProcessStartInfo();

            psi.FileName = Path.Combine(commandLocation, HtmlToImageExePath);
            psi.WorkingDirectory = Path.GetDirectoryName(psi.FileName);

            // run the conversion utility
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            // note: that we tell wkhtmltopdf to be quiet and not run scripts
            psi.Arguments = "\"" + url + "\" \"" + pngFile + "\"";

            p = Process.Start(psi);

            try
            {
                p.WaitForExit(20000);

                //convert png to tiff
                BitmapSource image = new BitmapImage(new Uri(pngFile));

                using (FileStream stream = new FileStream(tiffFile, FileMode.Create))
                {
                    TiffBitmapEncoder encoder = new TiffBitmapEncoder();
                    encoder.Compression = TiffCompressOption.None;
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    encoder.Save(stream);
                }

                return true;
            }
            catch
            {
                return false;

            }
            finally
            {
                p.Dispose();
            }
        }

        public static bool GeneratePdf(string commandLocation, String url,  String pdfFile)
        {
            Process p;
            ProcessStartInfo psi = new ProcessStartInfo();

            psi.FileName = Path.Combine(commandLocation, HtmlToPdfExePath);
            psi.WorkingDirectory = Path.GetDirectoryName(psi.FileName);

            // run the conversion utility
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            
            // note: that we tell wkhtmltopdf to be quiet and not run scripts
            psi.Arguments = "-q -n --disable-smart-shrinking --zoom 0.75 \"" + url + "\" \"" + pdfFile + "\"";

            p = Process.Start(psi);

            try
            {
                p.WaitForExit(10000);

                return true;
            }
            catch
            {
                return false;

            }
            finally
            {
                p.Dispose();
            }
        }

        public static bool GeneratePdf(string commandLocation, StreamReader html, Stream pdf, Size pageSize)
        {
            Process p;
            StreamWriter stdin;
            ProcessStartInfo psi = new ProcessStartInfo();

            psi.FileName = Path.Combine(commandLocation, HtmlToPdfExePath);
            psi.WorkingDirectory = Path.GetDirectoryName(psi.FileName);

            // run the conversion utility
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            // note: that we tell wkhtmltopdf to be quiet and not run scripts
            psi.Arguments = "-q -n --disable-smart-shrinking " + (pageSize.IsEmpty ? "" : "--page-width " + pageSize.Width + "mm --page-height " + pageSize.Height + "mm") + " - -";

            p = Process.Start(psi);

            try
            {
                stdin = p.StandardInput;
                stdin.AutoFlush = true;
                stdin.Write(html.ReadToEnd());
                stdin.Dispose();

                StreamHelper.CopyStream(p.StandardOutput.BaseStream, pdf);
                p.StandardOutput.Close();
                pdf.Position = 0;

                p.WaitForExit(10000);

                return true;
            }
            catch
            {
                return false;

            }
            finally
            {
                p.Dispose();
            }
        }
    }
}