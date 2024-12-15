using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DegistirilmeTarihineGoreYedekleme.Properties;

namespace DegistirilmeTarihineGoreYedekleme
{
    public partial class GunlukKullanılanDosyalariYedekleme : Form
    {
        public GunlukKullanılanDosyalariYedekleme()
        {
            InitializeComponent();
        }
        string settingsfilepath = "settings.txt";
        string directorypath = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        int startmin = 00;
        int starthour = 10;
        int sondegisim = 1;
        int sonerisim = 0;

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(settingsfilepath))
                {
                    // Dosya adı ve oluşturulacağı yer
                    settingsfilepath = "settings.txt";

                    // Dosyaya yazılacak içerik
                    string[] settingvalues = {directorypath,
                        startmin.ToString(),
                        starthour.ToString(),
                        sondegisim.ToString(),
                        sonerisim.ToString()
        };


                    // İçeriği dosyaya satır satır yaz
                    File.WriteAllLines(settingsfilepath, settingvalues);
                    textBox1.Text = directorypath;
                    textBox2.Text = starthour.ToString();
                    textBox3.Text = startmin.ToString();
                    sonerisimrb.Checked = sonerisim == 1 ? true : false;
                    sondegistirmerb.Checked = sondegisim == 1 ? true : false;

                }
                else
                {
                    string[] settingvalues = File.ReadAllLines(settingsfilepath);
                    directorypath = settingvalues[0];
                    startmin = int.Parse(settingvalues[1]);
                    starthour = int.Parse(settingvalues[2]);
                    sondegisim = int.Parse(settingvalues[3]);
                    sonerisim = int.Parse(settingvalues[4]);
                }
                textBox1.Text = directorypath;
                textBox2.Text = starthour.ToString();
                textBox3.Text = startmin.ToString();
                sonerisimrb.Checked = sonerisim == 1 ? true : false;
                sondegistirmerb.Checked = sondegisim == 1 ? true : false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
            // Form yüklendiğinde zamanlayıcı başlasın
            await ScheduleDailyTask(starthour, startmin, ExecuteTask);
        }
        private async Task ScheduleDailyTask(int hour, int minute, Action task)
        {
            while (true)
            {
                DateTime now = DateTime.Now;
                DateTime nextRun = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);

                // Eğer belirtilen saat geçtiyse, ertesi güne planla
                if (now > nextRun)
                {
                    nextRun = nextRun.AddDays(1);
                }

                TimeSpan timeToWait = nextRun - now;

                // Kullanıcıya bilgi vermek için Label gibi bir UI bileşeni güncellenebilir
                UpdateStatusLabel($"Bir sonraki çalıştırma: {nextRun}");

                // Zaman dolana kadar bekle
                await Task.Delay(timeToWait);

                // Görev çalıştır
                task.Invoke();
            }
        }

        private void ExecuteTask()
        {
            // Görevinizi burada çalıştırabilirsiniz
            MessageBox.Show($"Görev çalıştı: {DateTime.Now}", "Bilgilendirme");

            // Örnek: Bir log yazabilir veya başka bir işlem yapabilirsiniz
        }

        private void UpdateStatusLabel(string message)
        {
            // Bir Label bileşeni varsa durumu göstermek için bu metodu kullanabilirsiniz
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateStatusLabel(message)));
            }
            else
            {
                label6.Text = message; // lblStatus, Form üzerindeki bir Label bileşeni
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            // Dosya adı ve oluşturulacağı yer
            try
            {
                directorypath = textBox1.Text;
                starthour = int.Parse(textBox2.Text);
                startmin = int.Parse(textBox3.Text);
                sondegisim = sondegistirmerb.Checked ? 1 : 0;
                sonerisim = sonerisimrb.Checked ? 1 : 0;

                // Dosyaya yazılacak içerik
                string[] settingvalues = {directorypath,startmin.ToString(), starthour.ToString(), sondegisim.ToString(), sonerisim.ToString()
        };


                // İçeriği dosyaya satır satır yaz
                File.WriteAllLines(settingsfilepath, settingvalues);
                DialogResult dialogResult = MessageBox.Show("Program çalıştıktan sonra ayarlarınız aktif edilecektir. \rHemen aktif olması için program yeniden başlatılsın mı?", "Ayarlar Kaydedildi", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Application.Restart();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();

            textBox1.Text = dialog.SelectedPath;
        }
    }
}
