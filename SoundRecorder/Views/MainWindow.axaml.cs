using Avalonia.Controls;
using Avalonia.ReactiveUI;
using SoundRecorder.ViewModels;
using ReactiveUI;
using System.Threading.Tasks;
using NAudio;
using System.IO;
using System;
using NAudio.Wave;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Runtime.InteropServices;
using Avalonia.Controls.Templates;

namespace SoundRecorder.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            var outputFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Recordings");
            Directory.CreateDirectory(outputFolder);
            var outputFilePath = Path.Combine(outputFolder, $"recorded at {DateTime.Now.ToString("MMM dd yyy, H-mm-ss")}.wav");

            StopRecBut.IsEnabled = false;
            var capture = new WasapiLoopbackCapture();
            WaveFileWriter writer = null;

            RecBut.Click += (s, a) =>
            {
                writer = new WaveFileWriter(outputFilePath, capture.WaveFormat);
                capture.StartRecording();
                RecBut.IsEnabled = false;
                StopRecBut.IsEnabled = true;
                this.RecMonitoring.Text = $"Идет запись в \n{outputFilePath}";
            };

            StopRecBut.Click += (s, a) => capture.StopRecording();

            capture.DataAvailable += (s, a) =>
            {
                writer.Write(a.Buffer, 0, a.BytesRecorded);
                //ограничение по времени
                //if (writer.Position > capture.WaveFormat.AverageBytesPerSecond * 160)
                //{
                //    capture.StopRecording();
                //}
            };

            capture.RecordingStopped += (s, a) =>
            {
                writer?.Dispose();
                writer = null;
                RecBut.IsEnabled = true;
                StopRecBut.IsEnabled = false;
                this.Closing += (s, a) => { capture.Dispose(); };
                this.RecMonitoring.Text = $"Записано в \n{outputFilePath}";
            };

            this.Closing += (s, a) => { capture.StopRecording(); };
        }

        
    }
}