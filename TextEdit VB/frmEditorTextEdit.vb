Public Class frmEditorTextEdit

    ' Variables públicas libres de errores de serialización (WFO100)
    Public ArchivoModificado As Boolean = False
    Public RutaArchivo As String = ""

    Private Sub frmEditorTextEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CentrarHoja()
        ActualizarContadores()
    End Sub

    Private Sub CentrarHoja()
        Dim panelPadre As Control = rtbDocumento.Parent
        If panelPadre IsNot Nothing Then
            Dim nuevaX As Integer = (panelPadre.ClientSize.Width - rtbDocumento.Width) \ 2
            If nuevaX < 10 Then nuevaX = 10
            rtbDocumento.Left = nuevaX
            rtbDocumento.Top = 20
        End If
    End Sub

    Private Sub frmEditorTextEdit_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        CentrarHoja()
    End Sub

    Private Sub rtbDocumento_TextChanged(sender As Object, e As EventArgs) Handles rtbDocumento.TextChanged
        ArchivoModificado = True
        ActualizarContadores()

        Dim nombreVisible As String = If(String.IsNullOrEmpty(RutaArchivo), "Sin título.txt", IO.Path.GetFileName(RutaArchivo))
        Me.Text = nombreVisible & " * - TextEdit VB"
    End Sub

    Private Sub ActualizarContadores()
        Dim texto As String = rtbDocumento.Text
        Dim caracteres As Integer = texto.Length
        Dim palabras As Integer = 0

        If Not String.IsNullOrWhiteSpace(texto) Then
            palabras = texto.Split(New Char() {" "c, ControlChars.Cr, ControlChars.Lf}, StringSplitOptions.RemoveEmptyEntries).Length
        End If

        ' Actualiza directamente al formulario principal Form1
        If TypeOf Me.MdiParent Is Form1 Then
            Dim principal As Form1 = DirectCast(Me.MdiParent, Form1)
            principal.ActualizarContadorEstado(caracteres, palabras)
        End If
    End Sub

    Public Sub GuardarDocumento()
        If String.IsNullOrEmpty(RutaArchivo) Then
            GuardarComoDocumento()
        Else
            IO.File.WriteAllText(RutaArchivo, rtbDocumento.Text)
            ArchivoModificado = False
            Me.Text = IO.Path.GetFileName(RutaArchivo) & " - TextEdit VB"
            MessageBox.Show("Archivo guardado exitosamente.", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Public Sub GuardarComoDocumento()
        Using saveFileDialog As New SaveFileDialog()
            saveFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*"
            saveFileDialog.Title = "Guardar documento como"

            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                RutaArchivo = saveFileDialog.FileName
                IO.File.WriteAllText(RutaArchivo, rtbDocumento.Text)
                ArchivoModificado = False
                Me.Text = IO.Path.GetFileName(RutaArchivo) & " - TextEdit VB"
            End If
        End Using
    End Sub

    Private Sub frmEditorTextEdit_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If ArchivoModificado Then
            Dim resultado As DialogResult = MessageBox.Show(
                "El documento ha sido modificado. ¿Desea guardar los cambios?",
                "Control de Cambios",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning)

            If resultado = DialogResult.Yes Then
                GuardarDocumento()
                If ArchivoModificado Then e.Cancel = True
            ElseIf resultado = DialogResult.Cancel Then
                e.Cancel = True
            End If
        End If
    End Sub

End Class