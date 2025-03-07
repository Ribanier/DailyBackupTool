namespace DailyBackupManager
{
    partial class BackupManager
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackupManager));
            this.dateTimePickerStartDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePickerEndDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonAddSource = new System.Windows.Forms.Button();
            this.listBoxProcessedFiles = new System.Windows.Forms.ListBox();
            this.listBoxErrorFiles = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonSaveSettings = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxStartHour = new System.Windows.Forms.TextBox();
            this.textBoxStartMinute = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxTargetDirectory = new System.Windows.Forms.TextBox();
            this.buttonSetTarget = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.listBoxSourceDirectories = new System.Windows.Forms.ListBox();
            this.aboutlbl = new System.Windows.Forms.LinkLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // dateTimePickerStartDate
            // 
            this.dateTimePickerStartDate.Location = new System.Drawing.Point(12, 199);
            this.dateTimePickerStartDate.Name = "dateTimePickerStartDate";
            this.dateTimePickerStartDate.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerStartDate.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Kopyalanacak tarih aralığını seçiniz";
            // 
            // dateTimePickerEndDate
            // 
            this.dateTimePickerEndDate.Location = new System.Drawing.Point(241, 199);
            this.dateTimePickerEndDate.Name = "dateTimePickerEndDate";
            this.dateTimePickerEndDate.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerEndDate.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Arama yapılacak klasörü seçiniz";
            // 
            // buttonAddSource
            // 
            this.buttonAddSource.Location = new System.Drawing.Point(385, 53);
            this.buttonAddSource.Name = "buttonAddSource";
            this.buttonAddSource.Size = new System.Drawing.Size(56, 23);
            this.buttonAddSource.TabIndex = 7;
            this.buttonAddSource.Text = "Seç";
            this.buttonAddSource.UseVisualStyleBackColor = true;
            this.buttonAddSource.Click += new System.EventHandler(this.buttonAddSource_Click);
            // 
            // listBoxProcessedFiles
            // 
            this.listBoxProcessedFiles.FormattingEnabled = true;
            this.listBoxProcessedFiles.HorizontalScrollbar = true;
            this.listBoxProcessedFiles.Location = new System.Drawing.Point(12, 349);
            this.listBoxProcessedFiles.Name = "listBoxProcessedFiles";
            this.listBoxProcessedFiles.Size = new System.Drawing.Size(184, 199);
            this.listBoxProcessedFiles.TabIndex = 9;
            this.listBoxProcessedFiles.DoubleClick += new System.EventHandler(this.listBoxProcessedFiles_DoubleClick);
            // 
            // listBoxErrorFiles
            // 
            this.listBoxErrorFiles.FormattingEnabled = true;
            this.listBoxErrorFiles.HorizontalScrollbar = true;
            this.listBoxErrorFiles.Location = new System.Drawing.Point(258, 349);
            this.listBoxErrorFiles.Name = "listBoxErrorFiles";
            this.listBoxErrorFiles.Size = new System.Drawing.Size(183, 199);
            this.listBoxErrorFiles.TabIndex = 10;
            this.listBoxErrorFiles.DoubleClick += new System.EventHandler(this.listBoxErrorFiles_DoubleClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 324);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Başarılı kopyalamalar";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(327, 324);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Başarısız kopyalamalar";
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.Location = new System.Drawing.Point(13, 285);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(428, 23);
            this.buttonSaveSettings.TabIndex = 13;
            this.buttonSaveSettings.Text = "Ayarları kaydet";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            this.buttonSaveSettings.Click += new System.EventHandler(this.buttonSaveSettings_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Durum:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(56, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "...";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(322, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Made by Ribanier";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(9, 249);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(167, 19);
            this.label8.TabIndex = 17;
            this.label8.Text = "Programın çalışacağı saati seçiniz";
            // 
            // textBoxStartHour
            // 
            this.textBoxStartHour.Location = new System.Drawing.Point(196, 249);
            this.textBoxStartHour.Name = "textBoxStartHour";
            this.textBoxStartHour.Size = new System.Drawing.Size(100, 20);
            this.textBoxStartHour.TabIndex = 18;
            // 
            // textBoxStartMinute
            // 
            this.textBoxStartMinute.Location = new System.Drawing.Point(341, 249);
            this.textBoxStartMinute.Name = "textBoxStartMinute";
            this.textBoxStartMinute.Size = new System.Drawing.Size(100, 20);
            this.textBoxStartMinute.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(193, 233);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Saat(0-24)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(338, 233);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(68, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "Dakika(0-60)";
            // 
            // textBoxTargetDirectory
            // 
            this.textBoxTargetDirectory.Location = new System.Drawing.Point(12, 135);
            this.textBoxTargetDirectory.Name = "textBoxTargetDirectory";
            this.textBoxTargetDirectory.Size = new System.Drawing.Size(366, 20);
            this.textBoxTargetDirectory.TabIndex = 22;
            // 
            // buttonSetTarget
            // 
            this.buttonSetTarget.Location = new System.Drawing.Point(384, 135);
            this.buttonSetTarget.Name = "buttonSetTarget";
            this.buttonSetTarget.Size = new System.Drawing.Size(57, 23);
            this.buttonSetTarget.TabIndex = 23;
            this.buttonSetTarget.Text = "Seç";
            this.buttonSetTarget.UseVisualStyleBackColor = true;
            this.buttonSetTarget.Click += new System.EventHandler(this.buttonSetTarget_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 119);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(240, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Bulunan dosyaların kopyalanacağı klasörü seçiniz";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(305, 22);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(130, 13);
            this.linkLabel1.TabIndex = 25;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "sadikyildirim@ribanier.com";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // listBoxSourceDirectories
            // 
            this.listBoxSourceDirectories.FormattingEnabled = true;
            this.listBoxSourceDirectories.Location = new System.Drawing.Point(12, 53);
            this.listBoxSourceDirectories.Name = "listBoxSourceDirectories";
            this.listBoxSourceDirectories.Size = new System.Drawing.Size(367, 69);
            this.listBoxSourceDirectories.TabIndex = 26;
            this.listBoxSourceDirectories.DoubleClick += new System.EventHandler(this.listBoxSourceDirectories_DoubleClick);
            // 
            // aboutlbl
            // 
            this.aboutlbl.AutoSize = true;
            this.aboutlbl.Location = new System.Drawing.Point(388, 551);
            this.aboutlbl.Name = "aboutlbl";
            this.aboutlbl.Size = new System.Drawing.Size(53, 13);
            this.aboutlbl.TabIndex = 27;
            this.aboutlbl.TabStop = true;
            this.aboutlbl.Text = "Hakkında";
            this.aboutlbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.aboutlbl_LinkClicked);
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // BackupManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 565);
            this.Controls.Add(this.aboutlbl);
            this.Controls.Add(this.listBoxSourceDirectories);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.buttonSetTarget);
            this.Controls.Add(this.textBoxTargetDirectory);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxStartMinute);
            this.Controls.Add(this.textBoxStartHour);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonSaveSettings);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBoxErrorFiles);
            this.Controls.Add(this.listBoxProcessedFiles);
            this.Controls.Add(this.buttonAddSource);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePickerEndDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePickerStartDate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BackupManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Automatic Backup Manager";
            this.Load += new System.EventHandler(this.BackupManager_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePickerStartDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePickerEndDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonAddSource;
        private System.Windows.Forms.ListBox listBoxProcessedFiles;
        private System.Windows.Forms.ListBox listBoxErrorFiles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonSaveSettings;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxStartHour;
        private System.Windows.Forms.TextBox textBoxStartMinute;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxTargetDirectory;
        private System.Windows.Forms.Button buttonSetTarget;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ListBox listBoxSourceDirectories;
        private System.Windows.Forms.LinkLabel aboutlbl;
        private System.Windows.Forms.Timer timer1;
    }
}

