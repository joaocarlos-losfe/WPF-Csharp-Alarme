using System;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Windows;
using NAudio.Wave;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using Data;

namespace Alarmer
{
    public partial class MainWindow : Window
    {
        private string audio_directory = @"A:\Programação\Projetos do visual studio\Testes\Alarmer\sound\padrao.mp3";
        OpenFileDialog openFile = new OpenFileDialog();

        List<ItemData> itemData = new List<ItemData>();

        IWavePlayer device = new WaveOut();
        //AudioFileReader audioFile;

        public MainWindow()
        {
            InitializeComponent();
            cb_mes.SelectedIndex = int.Parse(DateTime.Now.ToString("MM")) - 1;
            cb_dia.SelectedIndex = int.Parse(DateTime.Now.ToString("dd")) - 1;
            
            lbl_info_horario.Content = DateTime.Now.ToString("HH:mm");
            lbl_info_data.Content = DateTime.Now.ToString("dd/MM/yyy");

            Thread T = new Thread(new ThreadStart(alarmer_observer));
            T.SetApartmentState(ApartmentState.STA);
            T.Start();

        }

        private void alarmer_observer()
        {
            while (true)
            {
                for (int i = 0; i < itemData.Count; i++)
                {
                    if (itemData[i].Data_e_hora_do_alarme.Equals(DateTime.Now.ToString("dd/MM") + " " + DateTime.Now.ToString("HH:mm")) && DateTime.Now.ToString("ss").Equals("00"))
                    {
                        window_alarm_show alarm_Show = new window_alarm_show(itemData[i].Arquivo_de_audio);
                        alarm_Show.ShowDialog();
                    }  
                }
            }
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
            lbl_info_hora_atual.Content = DateTime.Now.ToString("HH:mm:ss") + " " + DateTime.Now.ToString("dd/MM/yyy");
        }

        private void btn_selecionar_arquivo_Click(object sender, RoutedEventArgs e)
        {
            openFile.Filter = "Arquivos de audio | *.mp3; *.aac; *.wav";
            DialogResult dialogButtonResult = openFile.ShowDialog();

            if(dialogButtonResult != System.Windows.Forms.DialogResult.Cancel)
            {
                audio_directory = openFile.FileName;
                openFile.Title = "Selecione um arquivo de audio";
                lbl_info_som_do_alarme.Content = Path.GetFileName(audio_directory);
            }   
        }

        private void btn_salvar_alarme_Click(object sender, RoutedEventArgs e)
        {
            string temp_hour = cb_hora.Text.Remove(2, 3);
            string temp_mensage = "Alarme...";

            if(! txt_box_mensagem.Text.Equals(""))
            {
                temp_mensage = txt_box_mensagem.Text;
            }

            string data_hora = cb_dia.Text + "/" + cb_mes.Text + " " + temp_hour + ":" + cb_minuto.Text;

            listviewAlarmer_Update(data_hora, temp_mensage, audio_directory);
        }

        private void listviewAlarmer_Update(string data_hora, string mensagem, string diretorio_arquivo_de_audio)
        {
            
            ItemData item = new ItemData();
            item.Data_e_hora_do_alarme = data_hora;
            item.Mensagem = mensagem;
            item.Arquivo_de_audio = diretorio_arquivo_de_audio;

            bool contains = false;

            for(int i = 0; i < itemData.Count; i++)
            {
                if( itemData[i].Data_e_hora_do_alarme.Equals(item.Data_e_hora_do_alarme) )
                {
                    contains = true;
                }
            }

            if(!contains)
            {
                itemData.Add(item);
                list_alarmes_salvos.Items.Add(item);
                Console.WriteLine(item.Data_e_hora_do_alarme);
            }
            else
            {
                System.Windows.MessageBox.Show("Alarme definido anteriormente...");
            }
              
        }
    }
}
