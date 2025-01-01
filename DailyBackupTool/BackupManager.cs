using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DailyBackupManager
{
    public partial class BackupManager : Form
    {
        public BackupManager()
        {
            InitializeComponent();
        }

        string settingsFilePath = "settings.txt";
        string sourceDirectories = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string targetDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        int startMinute = 00;
        int startHour = 10;
        private void LogError(string message, string path = "")
        {
            File.AppendAllText("error_log.txt", $"{DateTime.Now}: {message}\n");
        }

        private async void BackupManager_Load(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(settingsFilePath))
                {
                    MessageBox.Show("Başlangıç için gerekli yerleri doldurup kaydediniz.");

                }
                else
                {
                    LoadSettings();
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
                LogError(ex.Message);
            }
            // Form yüklendiğinde zamanlayıcı başlasın
            await ScheduleDailyTask(startHour, startMinute, ExecuteTask);
        }
        void LoadSettings()
        {
            try
            {
                string[] settings = File.ReadAllLines(settingsFilePath);
                sourceDirectories = settings[0];
                targetDirectory = settings[1];
                startMinute = int.Parse(settings[2]);
                startHour = int.Parse(settings[3]);
                listBoxSourceDirectories.Items.AddRange(sourceDirectories.Split(','));
                textBoxTargetDirectory.Text = targetDirectory;
                textBoxStartHour.Text = startHour.ToString();
                textBoxStartMinute.Text = startMinute.ToString();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
                MessageBox.Show("Ayarlar yüklenirken bir hata ile karşılaşıldı, lütfen ayarları kontrol edip kaydediniz.");
            }
        }

        void SaveSettings()
        {
            try
            {
                if (listBoxSourceDirectories.Items.Count == 0 || textBoxTargetDirectory.Text == "" || textBoxStartHour.Text == "" || textBoxStartMinute.Text == "")
                { MessageBox.Show("Gerekli yerleri doldurunuz."); }
                else
                {
                    sourceDirectories = string.Join(",", listBoxSourceDirectories.Items.Cast<string>());
                    targetDirectory = textBoxTargetDirectory.Text;
                    startHour = int.Parse(textBoxStartHour.Text);
                    startMinute = int.Parse(textBoxStartMinute.Text);
                    string[] settings = { sourceDirectories, targetDirectory, startMinute.ToString(), startHour.ToString() };
                    File.WriteAllLines(settingsFilePath, settings);
                    DialogResult dialogResult = MessageBox.Show("Ayarlar kaydedildi.\rHemen aktif olması için program yeniden başlatılsın mı?", "Success!", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Application.Restart();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
                MessageBox.Show("Ayarlar kaydedilirken bir hata ile karşılaşıldı, lütfen ayarları kontrol edip tekrar kaydediniz.");
            }
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
            label6.Text = $"Görev çalışıyor: {DateTime.Now}";
            label6.ForeColor = Color.Green;


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
                label6.ForeColor = Color.Black;
            }
        }
        private async void ListAndCopyFilesAsync()
        {
            int prstatus = 0;
            listBoxProcessedFiles.Items.Clear(); // Başarıyla listelenen dosyalar
            listBoxErrorFiles.Items.Clear(); // Hata oluşan dosyalar
            foreach (string sourceDirectories in listBoxSourceDirectories.Items)
            {

                string sourcePath = sourceDirectories;
                string targetPath = targetDirectory;// Hedef klasör
                DateTime startDate = dateTimePickerStartDate.Value.Date;
                DateTime endDate = dateTimePickerEndDate.Value.Date;


                if (Directory.Exists(sourcePath))
                {
                    // Hedef klasörde tarih-saat isimli bir klasör oluştur
                    // string dateTimeFolderName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss--ffff");
                    string dateTimeFolderName = DateTime.Now.ToString("yyyy-M-d_s-ffff") + "-" + sourcePath.Split('\\').Last();
                    string targetRootFolder = Path.Combine(targetPath, dateTimeFolderName);

                    Directory.CreateDirectory(targetRootFolder); // Klasörü oluştur



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
                                    DateTime creationDate = File.GetCreationTime(file).Date;
                                    int index = 0;

                                    // Tarih aralığını kontrol et
                                    if ((lastWriteTime >= startDate && lastWriteTime <= endDate) || (creationDate >= startDate && creationDate <= endDate))
                                    {
                                        // Listeye ekle
                                        Invoke((Action)(() => listBoxProcessedFiles.Items.Add(file)));

                                        // Kopyalama işlemi
                                        string relativePath = GetRelativePath(sourcePath, file); // Özel yöntem kullanılıyor
                                        string targetFilePath = Path.Combine(targetRootFolder, relativePath); // Tarih klasörüne göre kopyala
                                        string targetDirectory = Path.GetDirectoryName(targetFilePath);

                                        if (!Directory.Exists(targetDirectory))
                                        {
                                            Directory.CreateDirectory(targetDirectory); // Alt klasörleri oluştur
                                        }
                                        if (Path.GetFileName(file).Length >= 100)
                                        {
                                            string directory = Path.GetDirectoryName(targetFilePath); // Hedef dosya dizinini al
                                            string extension = Path.GetExtension(file); // Dosya uzantısı

                                            // Yeni dosya adını oluştur
                                            string newFileName = $"degistirilmis{index++.ToString()}{extension}";

                                            // Eski ve yeni dosya isimlerini log.txt'ye yaz
                                            string logFilePath = Path.Combine(directory, "log.txt");
                                            using (StreamWriter writer = new StreamWriter(logFilePath, true)) // Append mode
                                            {
                                                writer.WriteLine($"Eski Dosya Adı: {Path.GetFileName(file)}, Yeni Dosya Adı: {newFileName}");
                                            }

                                            // Hedef dosya yolunu güncelle
                                            targetFilePath = Path.Combine(directory, newFileName);
                                        }

                                        File.Copy(file, targetFilePath, overwrite: true); // Dosyayı kopyala
                                    }
                                }
                                catch (Exception copyEx)
                                {
                                    LogError(copyEx.Message, file);
                                    // Hata oluşursa, dosyayı listBox2'ye ekle
                                    Invoke((Action)(() =>
                                    {
                                        listBoxErrorFiles.Items.Add($"{file} - Hata: {copyEx.Message}");

                                    }));
                                }
                            }


                        }
                        catch (Exception ex)
                        {
                            LogError(ex.Message);
                            Invoke((Action)(() =>
                            {
                                MessageBox.Show($"Genel bir hata oluştu: {ex.Message}");
                                prstatus++;

                            }));
                        }
                    });
                }

            }
            if (prstatus == 0)
                shutDownPC(600);
        }

        private void shutDownPC(int time = 300)
        {
            Invoke((Action)(() =>
            {
                Process.Start("shutdown", $"/s /t {time}");
                DialogResult dialogResult = MessageBox.Show("Listeleme ve kopyalama işlemi tamamlandı!\r Bilgisayar " + time.ToString() + " saniye sonra kapanacak. Kapatmayı iptal etmek istiyor musun", "İşlem Tamam!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes) { Process.Start("shutdown", "/a"); }
                // Shutdown komutunu çağır
            }));
        }

        public static string GetRelativePath(string basePath, string filePath)
        {
            Uri baseUri = new Uri(basePath.EndsWith("\\") ? basePath : basePath + "\\");
            Uri fileUri = new Uri(filePath);
            return Uri.UnescapeDataString(baseUri.MakeRelativeUri(fileUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }
        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void buttonAddSource_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            if (dialog.SelectedPath != "")
                listBoxSourceDirectories.Items.Add(dialog.SelectedPath);
        }

        private void buttonSetTarget_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();

            textBoxTargetDirectory.Text = dialog.SelectedPath;
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:sadikyildirim@ribanier.com");
        }

        private void listBoxSourceDirectories_DoubleClick(object sender, EventArgs e)
        {
            listBoxSourceDirectories.Items.Remove(listBoxSourceDirectories.SelectedItem);
        }

    }
}
