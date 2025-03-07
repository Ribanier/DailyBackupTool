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
        ToolTip toolTip;
        private async void BackupManager_Load(object sender, EventArgs e)
        {
            toolTip = new ToolTip();
            toolTip.SetToolTip(this.listBoxSourceDirectories, "Silmek için çift tıklayın");
            toolTip.SetToolTip(this.listBoxProcessedFiles, "Kopyalamak için çift tıklayın");
            toolTip.SetToolTip(this.listBoxErrorFiles, "Kopyalamak için çift tıklayın");
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

                string sourcePath = @"\\?\" + sourceDirectories;
                string targetPath = @"\\?\" + targetDirectory;// Hedef klasör
                DateTime startDate = dateTimePickerStartDate.Value.Date;
                DateTime endDate = dateTimePickerEndDate.Value.Date;
                //  MessageBox.Show(sourcePath.ToString() + @"\p" + targetPath.ToString());


                if (Directory.Exists(sourcePath))
                {
                    // Hedef klasörde tarih-saat isimli bir klasör oluştur
                    // string dateTimeFolderName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss--ffff");
                    string dateTimeFolderName = DateTime.Now.ToString("yyyy-M-d_s-ffff") + "-" + sourcePath.Split('\\').Last();
                    string targetRootFolder = Path.Combine(targetPath, dateTimeFolderName);
                    //   MessageBox.Show(dateTimeFolderName.ToString() + @"\p" + targetRootFolder.ToString());
                    Directory.CreateDirectory(targetRootFolder); // Klasörü oluştur



                    await Task.Run(() =>
                    {
                        try
                        {
                            var fileArray = Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories);

                            foreach (var file in fileArray)
                            {
                                //MessageBox.Show(file.ToString());
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
                                        //  MessageBox.Show(relativePath.ToString() + @"\p" + targetFilePath.ToString()+@"\p"+targetDirectory.ToString());
                                        if (!Directory.Exists(targetDirectory))
                                        {
                                            Directory.CreateDirectory(targetDirectory); // Alt klasörleri oluştur
                                        }
                                        /*   if (Path.GetFileName(file).Length >= 100)
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
                                           }*/

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
            /*   Uri baseUri = new Uri(basePath.EndsWith("\\") ? basePath : basePath + "\\");
               Uri fileUri = new Uri(filePath);
               return Uri.UnescapeDataString(baseUri.MakeRelativeUri(fileUri).ToString().Replace('/', Path.DirectorySeparatorChar));*/
            basePath = basePath.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;

            // filePath'i normalleştir
            filePath = filePath.TrimEnd(Path.DirectorySeparatorChar);

            // Base path ile filePath arasındaki kısmı bul
            if (filePath.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
            {
                return filePath.Substring(basePath.Length);
            }

            // Eğer filePath, basePath'ten bir yol değilse, tam yol döndürülür.
            return filePath;
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

        private void aboutlbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Bu program, belirlenen bir klasördeki dosyaları son değiştirme veya oluşturma tarihine göre analiz ederek yedekleme işlemi gerçekleştirir. Kullanıcı tarafından belirlenen tarih kriterlerine uygun dosyalar, belirtilen yedekleme klasörüne kopyalanır veya taşınır. Böylece, belirli bir zaman diliminde güncellenen veya oluşturulan dosyaların güvenli bir şekilde saklanması sağlanır. Program, veri kaybını önlemek ve dosya yönetimini kolaylaştırmak için etkili bir yedekleme çözümü sunar.", "Hakkında", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void listBoxProcessedFiles_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxProcessedFiles.SelectedItem != null)
            {
                // Clipboard'a kopyala
                Clipboard.SetData(DataFormats.Text, (Object)listBoxProcessedFiles.SelectedItem);

                // Kopyalandı mesajını göster
                toolTip.Show("Kopyalandı!", listBoxProcessedFiles, 1000);
                timerVal = 0;
                timer1.Start();
            }
        }

        private void listBoxErrorFiles_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxProcessedFiles.SelectedItem != null)
            {
                // Clipboard'a kopyala
                Clipboard.SetData(DataFormats.Text, (Object)listBoxErrorFiles.SelectedItem);

                // Kopyalandı mesajını göster
                toolTip.Show("Kopyalandı!", listBoxProcessedFiles, 1000);
                timerVal = 0;
                timer1.Start();
            }

        }
        int timerVal = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timerVal == 2)
            {
                toolTip.SetToolTip(this.listBoxProcessedFiles, "Kopyalamak için çift tıklayın");
                toolTip.SetToolTip(this.listBoxErrorFiles, "Kopyalamak için çift tıklayın");
                timerVal = 0;
                timer1.Stop();
            }
            timerVal++;
        }
    }
}
