using System;
using System.Runtime.Versioning;
using System.Windows;

namespace PdfAnalyzer
{
    /// <summary>
    /// Application entry point.
    /// </summary>
    public class Program
    {
        [STAThread]
        [SupportedOSPlatform("windows")]
        public static void Main()
        {
            var app = new App();
            var window = new MainWindow();
            app.Run(window);
        }
    }
}
