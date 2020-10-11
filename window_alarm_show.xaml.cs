using System;
using System.Windows;
using NAudio.Wave;
using System.Windows.Threading;


namespace Alarmer
{
  
    public partial class window_alarm_show : Window
    {
        string file_audio_directory;
        IWavePlayer device = new WaveOut();
        AudioFileReader audioFile;
        int tick = 1;

        public window_alarm_show(string audio_directory)
        {
            InitializeComponent();
            file_audio_directory = audio_directory;
            sound_play();
        }

        public void sound_play()
        {
            audioFile = new AudioFileReader(file_audio_directory);
            device.Init(audioFile);
            device.Play();
        }

        public void sound_stop()
        {  
            device.Stop();
            audioFile.Dispose();
            device.Dispose();
        }

        private void btn_parar_Click(object sender, RoutedEventArgs e)
        {
            sound_stop();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            get_current_time();
        }

        private void get_current_time()
        {
            var Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(updateTime_tick);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
            
        }

        private void updateTime_tick(object sender, EventArgs e)
        {
            tick++;
            if (tick == 59)
            {
                tick = 0;
                sound_stop();
                this.Close();
            }
        }
    }
}
