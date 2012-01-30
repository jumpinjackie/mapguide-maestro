' Demo application for compiled runtime expressions.
' Generates an image by evaluating expressions for the r,g,b components of each pixel.
' Author: Eugene Ciloci
' Original demo concept by Pascal Ganaye (http://www.codeproject.com/dotnet/eval3.asp)

Imports Ciloci.Flee
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices

Public Class MainForm

	' Each expression will be attached to this class and will have access to the X and Y fields
	Private Class ExpressionOwner
		Public X As Double
		Public Y As Double
		' Allow the use of the random number generator too
		Public Rand As Random

		Public Sub New()
			Me.Rand = New Random()
		End Sub
	End Class

	Private Const IMAGE_HEIGHT As Integer = 256
	Private Const IMAGE_WIDTH As Integer = 256
	Private Const ORIGINAL_CONCEPT_URL As String = "http://www.codeproject.com/dotnet/eval3.asp"

	' The red, green, and blue expressions
	Private MyRedExpression, MyGreenExpression, MyBlueExpression As IGenericExpression(Of Double)
	' Our raw image data
	Private MyImageData As Byte()
	' Use one expression owner instance for all expressions
	Private MyExpressionOwner As ExpressionOwner
	' Use same options for all expressions
	Private MyContext As ExpressionContext

	Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
		MyBase.OnLoad(e)

		MyExpressionOwner = New ExpressionOwner

		' Create the context with our owner
		MyContext = New ExpressionContext(MyExpressionOwner)
		' Import math so we have access to its functions in expressions
		MyContext.Imports.AddType(GetType(Math))

		Me.CreateInitialImage()
		MyImageData = New Byte((IMAGE_HEIGHT * IMAGE_WIDTH * 3) - 1) {}

		Me.cmdGenerate.Enabled = False
		Me.PopulatePresets()
		Me.HookTextboxes()
		AddHandler ddPresets.SelectedIndexChanged, AddressOf ddPresets_FirstSelectedIndexChanged
	End Sub

	' Create the initial image
	Private Sub CreateInitialImage()
		Dim img As Image = New Bitmap(IMAGE_WIDTH, IMAGE_HEIGHT, Imaging.PixelFormat.Format24bppRgb)
		Dim g As Graphics = Graphics.FromImage(img)

		Dim imgRect As New Rectangle(0, 0, IMAGE_WIDTH, IMAGE_HEIGHT)
		g.FillRectangle(Brushes.White, imgRect)
		g.DrawString("Select a preset or enter expressions for the red, green, and blue components.", New Font("Tahoma", 10, FontStyle.Bold), Brushes.Orange, imgRect)

		g.Dispose()
		Me.PictureBox1.Image = img
	End Sub

	' Populate all the preset formulas
	Private Sub PopulatePresets()
		Dim info As PresetInfo
		Dim items As ComboBox.ObjectCollection = Me.ddPresets.Items

		info = New PresetInfo("Blinds", "(round(4*x-y*2) % 2) - x", "(abs(x+2*y) % 0{0}75)*10+y/5", "round(sin(sqrt(x*x+y*y))*3/5)+x/3")
		items.Add(info)

		info = New PresetInfo("Bullseye", "1-round(x/y*0{0}5)", "1-round(y/x*0{0}4)", "round(sin(sqrt(x*x+y*y)*10))")
		items.Add(info)

		info = New PresetInfo("Wave", "cos(x/2)/2", "cos(y/2)/3", "round(sin(sqrt(x*x*x+y*y)*10))")
		items.Add(info)

		info = New PresetInfo("Swirls", "x*15", "cos(x*y*4900)", "y*15")
		items.Add(info)

		info = New PresetInfo("Semi-Random", "cos(x) * rand.nextdouble()", "y^2", "x^2")
		items.Add(info)

		info = New PresetInfo("Mod", "(x ^2) % y", "y % x", "x % y")
		items.Add(info)
	End Sub

	Private Sub HookTextboxes()
		AddHandler tbRed.TextChanged, AddressOf ExpressionTextbox_TextChanged
		AddHandler tbGreen.TextChanged, AddressOf ExpressionTextbox_TextChanged
		AddHandler tbBlue.TextChanged, AddressOf ExpressionTextbox_TextChanged
	End Sub

	Private Sub UnHookTextboxes()
		RemoveHandler tbRed.TextChanged, AddressOf ExpressionTextbox_TextChanged
		RemoveHandler tbGreen.TextChanged, AddressOf ExpressionTextbox_TextChanged
		RemoveHandler tbBlue.TextChanged, AddressOf ExpressionTextbox_TextChanged
	End Sub

	' Function that actually generates the image
	Private Sub GenerateImage()
		Const mult As Double = 2 * Math.PI / 256.0
		Dim index As Integer

		For Yi As Integer = 0 To IMAGE_HEIGHT - 1
			' Update the y coordinate
			MyExpressionOwner.Y = (Yi - 128) * mult

			For Xi As Integer = 0 To IMAGE_WIDTH - 1
				' Update the x coordinate
				MyExpressionOwner.X = (Xi - 128) * mult

				' Evaluate the expressions
				Me.SetColorComponent(MyBlueExpression.Evaluate(), index)
				Me.SetColorComponent(MyGreenExpression.Evaluate(), index + 1)
				Me.SetColorComponent(MyRedExpression.Evaluate(), index + 2)
				index += 3
			Next
		Next
	End Sub

	Private Sub SetColorComponent(ByVal value As Double, ByVal index As Integer)
		If value < 0 Then
			value = 0
		ElseIf value > 1 Then
			value = 1
		ElseIf Double.IsNaN(value) = True Then
			value = 0
		End If

		MyImageData(index) = CByte(value * 255)
	End Sub

	Private Sub Generate()
		Dim sw As New Stopwatch()

		sw.Start()
		' Time the image generation
		Me.GenerateImage()
		sw.Stop()

		' Fast transfer of all the raw values to the image
		Dim rect As New Rectangle(0, 0, IMAGE_WIDTH, IMAGE_HEIGHT)
		Dim bmp As Bitmap = Me.PictureBox1.Image
		Dim data As BitmapData = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat)
		Marshal.Copy(MyImageData, 0, data.Scan0, MyImageData.Length)
		bmp.UnlockBits(data)

		Me.PictureBox1.Invalidate()

		' Show timing results
		Dim seconds As Double = sw.ElapsedMilliseconds / 1000
		Const NUM_EVALS As Integer = IMAGE_WIDTH * IMAGE_HEIGHT * 3
		Me.ShowStatusMessage("Evaluations: {1:n0}{0}Time: {2:n2} seconds{0}Speed: {3:n0} evaluations/sec", Environment.NewLine, NUM_EVALS, seconds, NUM_EVALS / seconds)
	End Sub

	Private Sub SetExpressionFromTextbox(ByVal e As IGenericExpression(Of Double), ByVal tb As TextBox)
		If tb Is Me.tbRed Then
			MyRedExpression = e
		ElseIf tb Is Me.tbGreen Then
			MyGreenExpression = e
		Else
			MyBlueExpression = e
		End If
	End Sub

	Private Sub CreateExpressionsFromPreset(ByVal preset As PresetInfo)
		Dim sw As New Stopwatch()
		sw.Start()

		MyRedExpression = MyContext.CompileGeneric(Of Double)(preset.MyRed)
		MyGreenExpression = MyContext.CompileGeneric(Of Double)(preset.MyGreen)
		MyBlueExpression = MyContext.CompileGeneric(Of Double)(preset.MyBlue)

		sw.Stop()

		Me.ShowStatusMessage("Presets compiled in {0:n0}ms", sw.ElapsedMilliseconds)
	End Sub

	Private Sub CreateSingleExpression(ByVal source As TextBox)
		Dim expr As IGenericExpression(Of Double)

		Try
			Dim sw As New Stopwatch()
			sw.Start()
			' Try to create the expression
			expr = MyContext.CompileGeneric(Of Double)(source.Text)

			' If we get here, then the expression was compiled successfully
			sw.Stop()
			source.BackColor = Color.Empty
			Me.ShowStatusMessage("Expression compiled in {0:n0}ms", sw.ElapsedMilliseconds)
		Catch ex As ExpressionCompileException
			' Could not compile the exception so show error 
			Me.ShowStatusMessage(ex.Message)
			expr = Nothing
			source.BackColor = Color.Tomato
		End Try

		Me.SetExpressionFromTextbox(expr, source)
		Me.cmdGenerate.Enabled = Not MyRedExpression Is Nothing And Not MyGreenExpression Is Nothing And Not MyBlueExpression Is Nothing
	End Sub

	Private Sub SaveImage()
		Dim sfd As New SaveFileDialog()
		sfd.Filter = "PNG files (*.png)|*.png"

		If sfd.ShowDialog(Me) = DialogResult.OK Then
			Dim bmp As Bitmap = Me.PictureBox1.Image
			bmp.Save(sfd.FileName, ImageFormat.Png)
		End If

		sfd.Dispose()
	End Sub

	Private Sub LoadPreset(ByVal preset As PresetInfo)
		Me.UnHookTextboxes()
		Me.tbRed.Text = preset.MyRed
		Me.tbGreen.Text = preset.MyGreen
		Me.tbBlue.Text = preset.MyBlue
		Me.HookTextboxes()
		Me.CreateExpressionsFromPreset(preset)
		Me.cmdGenerate.Enabled = True
		Me.tbRed.BackColor = Color.Empty
		Me.tbGreen.BackColor = Color.Empty
		Me.tbBlue.BackColor = Color.Empty
	End Sub

	Private Sub ShowStatusMessage(ByVal msg As String, ByVal ParamArray args As Object())
		Me.lblStatus.Text = String.Format(msg, args)
	End Sub

#Region "EventHandlers"

	Private Sub cmdGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGenerate.Click
		Me.Generate()
	End Sub

	' Save the generated image to a png file
	Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
		Me.SaveImage()
	End Sub

	Private Sub ExpressionTextbox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Me.CreateSingleExpression(sender)
	End Sub

	Private Sub ddPresets_FirstSelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
		RemoveHandler ddPresets.SelectedIndexChanged, AddressOf ddPresets_FirstSelectedIndexChanged
		Me.ddPresets.DropDownStyle = ComboBoxStyle.DropDownList
	End Sub

	Private Sub ddPresets_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddPresets.SelectedIndexChanged
		Dim preset As PresetInfo = Me.ddPresets.SelectedItem
		Me.LoadPreset(preset)
	End Sub

	Private Sub LinkLabel1_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
		Process.Start(ORIGINAL_CONCEPT_URL)
	End Sub

#End Region

End Class