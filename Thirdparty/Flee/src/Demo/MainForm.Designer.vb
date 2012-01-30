<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
	Inherits System.Windows.Forms.Form

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		Try
			If disposing AndAlso components IsNot Nothing Then
				components.Dispose()
			End If
		Finally
			MyBase.Dispose(disposing)
		End Try
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
		Me.Label2 = New System.Windows.Forms.Label
		Me.Label3 = New System.Windows.Forms.Label
		Me.Label4 = New System.Windows.Forms.Label
		Me.tbRed = New System.Windows.Forms.TextBox
		Me.tbGreen = New System.Windows.Forms.TextBox
		Me.tbBlue = New System.Windows.Forms.TextBox
		Me.PictureBox1 = New System.Windows.Forms.PictureBox
		Me.cmdGenerate = New System.Windows.Forms.Button
		Me.cmdSave = New System.Windows.Forms.Button
		Me.lblStatus = New System.Windows.Forms.Label
		Me.Label5 = New System.Windows.Forms.Label
		Me.ddPresets = New System.Windows.Forms.ComboBox
		Me.LinkLabel1 = New System.Windows.Forms.LinkLabel
		Me.Label1 = New System.Windows.Forms.Label
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Location = New System.Drawing.Point(5, 35)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(30, 13)
		Me.Label2.TabIndex = 1
		Me.Label2.Text = "Red:"
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Location = New System.Drawing.Point(5, 60)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(39, 13)
		Me.Label3.TabIndex = 2
		Me.Label3.Text = "Green:"
		'
		'Label4
		'
		Me.Label4.AutoSize = True
		Me.Label4.Location = New System.Drawing.Point(5, 85)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(31, 13)
		Me.Label4.TabIndex = 3
		Me.Label4.Text = "Blue:"
		'
		'tbRed
		'
		Me.tbRed.Location = New System.Drawing.Point(53, 32)
		Me.tbRed.Name = "tbRed"
		Me.tbRed.Size = New System.Drawing.Size(256, 20)
		Me.tbRed.TabIndex = 2
		'
		'tbGreen
		'
		Me.tbGreen.Location = New System.Drawing.Point(53, 57)
		Me.tbGreen.Name = "tbGreen"
		Me.tbGreen.Size = New System.Drawing.Size(256, 20)
		Me.tbGreen.TabIndex = 3
		'
		'tbBlue
		'
		Me.tbBlue.Location = New System.Drawing.Point(53, 82)
		Me.tbBlue.Name = "tbBlue"
		Me.tbBlue.Size = New System.Drawing.Size(256, 20)
		Me.tbBlue.TabIndex = 4
		'
		'PictureBox1
		'
		Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.PictureBox1.Location = New System.Drawing.Point(53, 170)
		Me.PictureBox1.Name = "PictureBox1"
		Me.PictureBox1.Size = New System.Drawing.Size(256, 256)
		Me.PictureBox1.TabIndex = 7
		Me.PictureBox1.TabStop = False
		'
		'cmdGenerate
		'
		Me.cmdGenerate.Location = New System.Drawing.Point(315, 32)
		Me.cmdGenerate.Name = "cmdGenerate"
		Me.cmdGenerate.Size = New System.Drawing.Size(85, 23)
		Me.cmdGenerate.TabIndex = 5
		Me.cmdGenerate.Text = "Generate"
		Me.cmdGenerate.UseVisualStyleBackColor = True
		'
		'cmdSave
		'
		Me.cmdSave.Location = New System.Drawing.Point(315, 61)
		Me.cmdSave.Name = "cmdSave"
		Me.cmdSave.Size = New System.Drawing.Size(85, 23)
		Me.cmdSave.TabIndex = 6
		Me.cmdSave.Text = "Save Image"
		Me.cmdSave.UseVisualStyleBackColor = True
		'
		'lblStatus
		'
		Me.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lblStatus.Location = New System.Drawing.Point(53, 113)
		Me.lblStatus.Name = "lblStatus"
		Me.lblStatus.Size = New System.Drawing.Size(256, 54)
		Me.lblStatus.TabIndex = 10
		'
		'Label5
		'
		Me.Label5.AutoSize = True
		Me.Label5.Location = New System.Drawing.Point(5, 114)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(40, 13)
		Me.Label5.TabIndex = 11
		Me.Label5.Text = "Status:"
		'
		'ddPresets
		'
		Me.ddPresets.FormattingEnabled = True
		Me.ddPresets.Location = New System.Drawing.Point(53, 5)
		Me.ddPresets.Name = "ddPresets"
		Me.ddPresets.Size = New System.Drawing.Size(214, 21)
		Me.ddPresets.TabIndex = 1
		Me.ddPresets.Text = "Select a preset"
		'
		'LinkLabel1
		'
		Me.LinkLabel1.AutoSize = True
		Me.LinkLabel1.LinkArea = New System.Windows.Forms.LinkArea(25, 13)
		Me.LinkLabel1.Location = New System.Drawing.Point(5, 429)
		Me.LinkLabel1.Name = "LinkLabel1"
		Me.LinkLabel1.Size = New System.Drawing.Size(217, 17)
		Me.LinkLabel1.TabIndex = 12
		Me.LinkLabel1.TabStop = True
		Me.LinkLabel1.Text = "Original Demo Concept by Pascal Ganaye"
		Me.LinkLabel1.UseCompatibleTextRendering = True
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(5, 8)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(42, 13)
		Me.Label1.TabIndex = 13
		Me.Label1.Text = "Presets"
		'
		'MainForm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(406, 449)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.LinkLabel1)
		Me.Controls.Add(Me.ddPresets)
		Me.Controls.Add(Me.Label5)
		Me.Controls.Add(Me.lblStatus)
		Me.Controls.Add(Me.cmdSave)
		Me.Controls.Add(Me.cmdGenerate)
		Me.Controls.Add(Me.PictureBox1)
		Me.Controls.Add(Me.tbBlue)
		Me.Controls.Add(Me.tbGreen)
		Me.Controls.Add(Me.tbRed)
		Me.Controls.Add(Me.Label4)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.Label2)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = False
		Me.Name = "MainForm"
		Me.Text = "Fast Lightweight Expression Evaluator Demo"
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents Label3 As System.Windows.Forms.Label
	Friend WithEvents Label4 As System.Windows.Forms.Label
	Friend WithEvents tbRed As System.Windows.Forms.TextBox
	Friend WithEvents tbGreen As System.Windows.Forms.TextBox
	Friend WithEvents tbBlue As System.Windows.Forms.TextBox
	Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
	Friend WithEvents cmdGenerate As System.Windows.Forms.Button
	Friend WithEvents cmdSave As System.Windows.Forms.Button
	Friend WithEvents lblStatus As System.Windows.Forms.Label
	Friend WithEvents Label5 As System.Windows.Forms.Label
	Friend WithEvents ddPresets As System.Windows.Forms.ComboBox
	Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
	Friend WithEvents Label1 As System.Windows.Forms.Label

End Class
