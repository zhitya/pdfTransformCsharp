using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace PdfAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _pdfPath = string.Empty;
        private string _outputFolder = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSelectPdf_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                Title = "Select PDF document"
            };
            if (ofd.ShowDialog() == true)
            {
                _pdfPath = ofd.FileName;
                var info = new FileInfo(_pdfPath);
                txtPdfInfo.Text = $"{info.FullName} | {info.Length / 1024}KB | {info.CreationTime}";
                if (!string.IsNullOrEmpty(_outputFolder))
                {
                    Analyze();
                }
            }
        }

        private void BtnSelectOutput_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _outputFolder = dlg.SelectedPath;
                txtOutputFolder.Text = _outputFolder;
                if (!string.IsNullOrEmpty(_pdfPath))
                {
                    Analyze();
                }
            }
        }

        private async void Analyze()
        {
            if (string.IsNullOrEmpty(_pdfPath) || string.IsNullOrEmpty(_outputFolder))
            {
                return;
            }

            string outputFile = System.IO.Path.Combine(_outputFolder,
                System.IO.Path.GetFileNameWithoutExtension(_pdfPath) + ".txt");

            progress.Value = 0;
            txtProgress.Text = "0%";

            var analyzer = new AnalyzePDF();
            analyzer.ProgressChanged += (p) => Dispatcher.Invoke(() =>
            {
                progress.Value = p;
                txtProgress.Text = $"{p}%";
            });
            await Task.Run(() => analyzer.Process(_pdfPath, outputFile));

            Dispatcher.Invoke(() => txtResult.Text = $"Result saved to: {outputFile}");
        }
    }
}
