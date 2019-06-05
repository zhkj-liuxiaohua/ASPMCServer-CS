/*
 * 由SharpDevelop创建。
 * 用户： admin
 * 日期: 2019/3/18
 * 时间: 9:45
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace PassEncode
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.TextBox filepathtxt;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btshowfile;
		private System.Windows.Forms.Button btencode;
		private System.Windows.Forms.Button btdecode;
		private System.Windows.Forms.TextBox content;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.filepathtxt = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btshowfile = new System.Windows.Forms.Button();
			this.btencode = new System.Windows.Forms.Button();
			this.btdecode = new System.Windows.Forms.Button();
			this.content = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "filename";
			this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenFileDialog1FileOk);
			// 
			// filepathtxt
			// 
			this.filepathtxt.Location = new System.Drawing.Point(26, 37);
			this.filepathtxt.Name = "filepathtxt";
			this.filepathtxt.Size = new System.Drawing.Size(235, 21);
			this.filepathtxt.TabIndex = 0;
			this.filepathtxt.Click += new System.EventHandler(this.Label1Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(26, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(57, 22);
			this.label1.TabIndex = 1;
			this.label1.Text = "选择文件";
			this.label1.Click += new System.EventHandler(this.Label1Click);
			// 
			// btshowfile
			// 
			this.btshowfile.Location = new System.Drawing.Point(47, 108);
			this.btshowfile.Name = "btshowfile";
			this.btshowfile.Size = new System.Drawing.Size(120, 21);
			this.btshowfile.TabIndex = 2;
			this.btshowfile.Text = "显示文本内容";
			this.btshowfile.UseVisualStyleBackColor = true;
			this.btshowfile.Click += new System.EventHandler(this.BtshowfileClick);
			// 
			// btencode
			// 
			this.btencode.Location = new System.Drawing.Point(65, 145);
			this.btencode.Name = "btencode";
			this.btencode.Size = new System.Drawing.Size(75, 23);
			this.btencode.TabIndex = 3;
			this.btencode.Text = "加密";
			this.btencode.UseVisualStyleBackColor = true;
			this.btencode.Click += new System.EventHandler(this.BtencodeClick);
			// 
			// btdecode
			// 
			this.btdecode.Location = new System.Drawing.Point(65, 186);
			this.btdecode.Name = "btdecode";
			this.btdecode.Size = new System.Drawing.Size(74, 24);
			this.btdecode.TabIndex = 4;
			this.btdecode.Text = "解密";
			this.btdecode.UseVisualStyleBackColor = true;
			this.btdecode.Click += new System.EventHandler(this.BtdecodeClick);
			// 
			// content
			// 
			this.content.Location = new System.Drawing.Point(277, 31);
			this.content.Multiline = true;
			this.content.Name = "content";
			this.content.Size = new System.Drawing.Size(205, 179);
			this.content.TabIndex = 5;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(527, 304);
			this.Controls.Add(this.content);
			this.Controls.Add(this.btdecode);
			this.Controls.Add(this.btencode);
			this.Controls.Add(this.btshowfile);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.filepathtxt);
			this.Name = "MainForm";
			this.Text = "PassEncode";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
