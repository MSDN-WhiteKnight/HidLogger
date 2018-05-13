namespace HidLogger
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbSessionName = new System.Windows.Forms.TextBox();
            this.tbFile = new System.Windows.Forms.TextBox();
            this.bStartCapture = new System.Windows.Forms.Button();
            this.bStopCapture = new System.Windows.Forms.Button();
            this.bViewEvents = new System.Windows.Forms.Button();
            this.tbEvents = new System.Windows.Forms.TextBox();
            this.bBrowse = new System.Windows.Forms.Button();
            this.bSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Session name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 50);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "File";
            // 
            // tbSessionName
            // 
            this.tbSessionName.Location = new System.Drawing.Point(116, 20);
            this.tbSessionName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbSessionName.Name = "tbSessionName";
            this.tbSessionName.Size = new System.Drawing.Size(179, 20);
            this.tbSessionName.TabIndex = 1;
            this.tbSessionName.Text = "UsbLoggerSession";
            // 
            // tbFile
            // 
            this.tbFile.Location = new System.Drawing.Point(116, 48);
            this.tbFile.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbFile.Name = "tbFile";
            this.tbFile.Size = new System.Drawing.Size(337, 20);
            this.tbFile.TabIndex = 1;
            this.tbFile.Text = "c:\\test\\events.etl";
            // 
            // bStartCapture
            // 
            this.bStartCapture.Location = new System.Drawing.Point(24, 86);
            this.bStartCapture.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bStartCapture.Name = "bStartCapture";
            this.bStartCapture.Size = new System.Drawing.Size(129, 26);
            this.bStartCapture.TabIndex = 2;
            this.bStartCapture.Text = "Start capture";
            this.bStartCapture.UseVisualStyleBackColor = true;
            this.bStartCapture.Click += new System.EventHandler(this.bStartCapture_Click);
            // 
            // bStopCapture
            // 
            this.bStopCapture.Location = new System.Drawing.Point(165, 86);
            this.bStopCapture.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bStopCapture.Name = "bStopCapture";
            this.bStopCapture.Size = new System.Drawing.Size(129, 26);
            this.bStopCapture.TabIndex = 2;
            this.bStopCapture.Text = "Stop capture";
            this.bStopCapture.UseVisualStyleBackColor = true;
            this.bStopCapture.Click += new System.EventHandler(this.bStopCapture_Click);
            // 
            // bViewEvents
            // 
            this.bViewEvents.Location = new System.Drawing.Point(298, 86);
            this.bViewEvents.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bViewEvents.Name = "bViewEvents";
            this.bViewEvents.Size = new System.Drawing.Size(129, 26);
            this.bViewEvents.TabIndex = 2;
            this.bViewEvents.Text = "View events";
            this.bViewEvents.UseVisualStyleBackColor = true;
            this.bViewEvents.Click += new System.EventHandler(this.bViewEvents_Click);
            // 
            // tbEvents
            // 
            this.tbEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEvents.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbEvents.Location = new System.Drawing.Point(26, 136);
            this.tbEvents.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbEvents.Multiline = true;
            this.tbEvents.Name = "tbEvents";
            this.tbEvents.ReadOnly = true;
            this.tbEvents.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbEvents.Size = new System.Drawing.Size(533, 234);
            this.tbEvents.TabIndex = 3;
            // 
            // bBrowse
            // 
            this.bBrowse.Location = new System.Drawing.Point(458, 44);
            this.bBrowse.Name = "bBrowse";
            this.bBrowse.Size = new System.Drawing.Size(65, 26);
            this.bBrowse.TabIndex = 4;
            this.bBrowse.Text = "Browse";
            this.bBrowse.UseVisualStyleBackColor = true;
            this.bBrowse.Click += new System.EventHandler(this.bBrowse_Click);
            // 
            // bSave
            // 
            this.bSave.Enabled = false;
            this.bSave.Location = new System.Drawing.Point(431, 86);
            this.bSave.Margin = new System.Windows.Forms.Padding(2);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(129, 26);
            this.bSave.TabIndex = 2;
            this.bSave.Text = "Save to file";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 388);
            this.Controls.Add(this.bBrowse);
            this.Controls.Add(this.tbEvents);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.bViewEvents);
            this.Controls.Add(this.bStopCapture);
            this.Controls.Add(this.bStartCapture);
            this.Controls.Add(this.tbFile);
            this.Controls.Add(this.tbSessionName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "USB HID Logger example";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbSessionName;
        private System.Windows.Forms.TextBox tbFile;
        private System.Windows.Forms.Button bStartCapture;
        private System.Windows.Forms.Button bStopCapture;
        private System.Windows.Forms.Button bViewEvents;
        private System.Windows.Forms.TextBox tbEvents;
        private System.Windows.Forms.Button bBrowse;
        private System.Windows.Forms.Button bSave;
    }
}

