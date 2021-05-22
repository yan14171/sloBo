
//#define Mine

using NAudio.Wave;
using System;
using DrawRectangle;
using Alexa_proj.Data_Control.Models;
using System.Threading.Tasks;

namespace Alexa_proj
{
    [Serializable]
    public class SoundCapturer : ExecutableModel
    {
        static WaveInEvent waveSource;
        static WaveFileWriter waveFile;
        public override async Task Execute()
        {
            string file = "RecordingFile.wav";
             CaptureSound(file);
            StartUp.OnEnterPressed += StartUp_OnEnterPressed;
        }

        private void StartUp_OnEnterPressed(object sender, EventArgs e)
        {

            waveSource.StopRecording();
            waveFile.Dispose();

        }

        private static void CaptureSound(string fileName)
        { 
                waveSource = new WaveInEvent();
                waveSource.WaveFormat = new WaveFormat(48000, 1);
                waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
                fileName = (@$"Resources/Files/{fileName}");
                waveFile = new WaveFileWriter(fileName, waveSource.WaveFormat);
                waveSource.StartRecording();

        StartUp.CurrentMenu.DynamicShow(
                     (
                     new ConsoleRectangle(38, 1,
                     new Point() { X = 12, Y = 0 },
                     ConsoleColor.Green, new[] { "Press any button to end the recording" }, 0)
                    ));
 
        } 

        static void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            waveFile.Write(e.Buffer, 0, e.BytesRecorded);
        }
    }
}