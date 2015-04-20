using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Services.Import
{
    public static class PdfImporter
    {
        public static bool ImportPdf(string path)
        {
            string pdfText = ExtractTextFromPdf(path);
            if(string.IsNullOrWhiteSpace(pdfText))
                return false;
            return true;
        }

        private static string ExtractTextFromPdf(string path)
        {
            using (PdfReader reader = new PdfReader(path))
            {
                SACTextExtractionStrategy strategy = new SACTextExtractionStrategy();

                StringBuilder text = new StringBuilder();
                for (int i = 1; i <= reader.NumberOfPages; i++)
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i, strategy));
                return text.ToString();
            }
        } 
    }
}
