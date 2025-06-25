using System;
using System.IO;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace PdfAnalyzer
{
    /// <summary>
    /// Provides functionality to analyze PDF documents.
    /// </summary>
    public class AnalyzePDF
    {
        /// <summary>
        /// Event raised when progress changes.
        /// </summary>
        public event Action<int>? ProgressChanged;

        /// <summary>
        /// Processes the specified PDF and writes information to a text file.
        /// </summary>
        /// <param name="pdfPath">Input PDF file path.</param>
        /// <param name="outputPath">Output text file path.</param>
        public void Process(string pdfPath, string outputPath)
        {
            using var document = PdfDocument.Open(pdfPath);
            int pageCount = document.NumberOfPages;
            using var writer = new StreamWriter(outputPath);

            for (int i = 1; i <= pageCount; i++)
            {
                Page page = document.GetPage(i);
                writer.WriteLine($"--- Page {i} ---");
                foreach (var word in page.GetWords())
                {
                    var font = word.FontName;
                    writer.WriteLine($"Text: {word.Text} | Font: {font}");
                }
                int progress = (int)((i / (double)pageCount) * 100);
                ProgressChanged?.Invoke(progress);
            }
            ProgressChanged?.Invoke(100);
        }
    }
}
