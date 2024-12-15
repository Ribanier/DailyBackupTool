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
    public partial class GunlukKullanilanDosyalariYedekleme : Form
    {
        public GunlukKullanilanDosyalariYedekleme()
        {
            InitializeComponent();
        }
        string settingsfilepath = "settings.txt";
        string sourcedirectorypath = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string targetdirectorypath = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        int startmin = 00;
        int starthour = 10;


        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(settingsfilepath))
                {
                    // Dosya adı ve oluşturulacağı yer
                    settingsfilepath = "settings.txt";

                    // Dosyaya yazılacak içerik
                    string[] settingvalues = {sourcedirectorypath,
                        targetdirectorypath,
                        startmin.ToString(),
                        starthour.ToString()
        };


                    // İçeriği dosyaya satır satır yaz
                    File.WriteAllLines(settingsfilepath, settingvalues);
                    textBox1.Text = sourcedirectorypath;
                    textBox4.Text = targetdirectorypath;
                    textBox2.Text = starthour.ToString();
                    textBox3.Text = startmin.ToString();


                }
                else
                {
                    string[] settingvalues = File.ReadAllLines(settingsfilepath);
                    sourcedirectorypath = settingvalues[0];
                    targetdirectorypath = settingvalues[1];
                    startmin = int.Parse(settingvalues[2]);
                    starthour = int.Parse(settingvalues[3]);

                }
                textBox1.Text = sourcedirectorypath;
                textBox4.Text = targetdirectorypath;
                textBox2.Text = starthour.ToString();
                textBox3.Text = startmin.ToString();


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
            ListAndCopyFilesAsync();
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
        private async void ListAndCopyFilesAsync()
        {
            string sourcePath = sourcedirectorypath; // Kaynak klasör
            string targetPath = targetdirectorypath;// Hedef klasör
            DateTime startDate = dateTimePicker1.Value.Date;
            DateTime endDate = dateTimePicker2.Value.Date;

            if (Directory.Exists(sourcePath))
            {
                // Hedef klasörde tarih-saat isimli bir klasör oluştur
                string dateTimeFolderName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string targetRootFolder = Path.Combine(targetPath, dateTimeFolderName);

                Directory.CreateDirectory(targetRootFolder); // Klasörü oluştur

                listBox1.Items.Clear(); // Başarıyla listelenen dosyalar
                listBox2.Items.Clear(); // Hata oluşan dosyalar

                await Task.Run(() =>
                {
                    try
                    {
                        var fileArray = Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories);

                        foreach (var file in fileArray)
                        {
                            try
                            {
                                DateTime lastWriteTime = File.GetLastWriteTime(file).Date;

                                // Tarih aralığını kontrol et
                                if (lastWriteTime >= startDate && lastWriteTime <= endDate)
                                {
                                    // Listeye ekle
                                    Invoke((Action)(() => listBox1.Items.Add(file)));

                                    // Kopyalama işlemi
                                    string relativePath = GetRelativePath(sourcePath, file); // Özel yöntem kullanılıyor
                                    string targetFilePath = Path.Combine(targetRootFolder, relativePath); // Tarih klasörüne göre kopyala
                                    string targetDirectory = Path.GetDirectoryName(targetFilePath);

                                    if (!Directory.Exists(targetDirectory))
                                    {
                                        Directory.CreateDirectory(targetDirectory); // Alt klasörleri oluştur
                                    }

                                    File.Copy(file, targetFilePath, overwrite: true); // Dosyayı kopyala
                                }
                            }
                            catch (Exception copyEx)
                            {
                                // Hata oluşursa, dosyayı listBox2'ye ekle
                                Invoke((Action)(() =>
                                {
                                    listBox2.Items.Add($"{file} - Hata: {copyEx.Message}");
                                }));
                            }
                        }

                        Invoke((Action)(() =>
                        {
                            MessageBox.Show("Listeleme ve kopyalama işlemi tamamlandı!");
                        }));
                    }
                    catch (Exception ex)
                    {
                        Invoke((Action)(() =>
                        {
                            MessageBox.Show($"Genel bir hata oluştu: {ex.Message}");
                        }));
                    }
                });
            }
            else
            {
                MessageBox.Show("Kaynak klasör bulunamadı!");
            }
        }

        public static string GetRelativePath(string basePath, string filePath)
        {
            Uri baseUri = new Uri(basePath.EndsWith("\\") ? basePath : basePath + "\\");
            Uri fileUri = new Uri(filePath);
            return Uri.UnescapeDataString(baseUri.MakeRelativeUri(fileUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }
        private void button2_Click(object sender, EventArgs e)
        {

            // Dosya adı ve oluşturulacağı yer
            try
            {
                sourcedirectorypath = textBox1.Text;
                targetdirectorypath = textBox4.Text;
                starthour = int.Parse(textBox2.Text);
                startmin = int.Parse(textBox3.Text);


                // Dosyaya yazılacak içerik
                string[] settingvalues = {sourcedirectorypath,targetdirectorypath,startmin.ToString(), starthour.ToString()
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

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();

            textBox4.Text = dialog.SelectedPath;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:sadikyildirim@ribanier.com");
        }
    }
}
