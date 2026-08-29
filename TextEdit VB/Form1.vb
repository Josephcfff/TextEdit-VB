Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "TextEdit VB - Editor de Texto"
        Me.WindowState = FormWindowState.Maximized
        Me.IsMdiContainer = True

        tsslEstado.Text = "Listo"
        tsslContador.Text = "Caracteres: 0 | Palabras: 0"
    End Sub

    Public Sub ActualizarContadorEstado(caracteres As Integer, palabras As Integer)
        tsslContador.Text = "Caracteres: " & caracteres & " | Palabras: " & palabras
    End Sub

    ' MENÚ: ARCHIVO
    Private Sub mnuNuevo_Click(sender As Object, e As EventArgs) Handles mnuNuevo.Click, tsbNuevo.Click
        Dim frmEditor As New frmEditorTextEdit()
        frmEditor.MdiParent = Me
        frmEditor.Show()
        tsslEstado.Text = "Nuevo documento"
    End Sub

    Private Sub mnuAbrir_Click(sender As Object, e As EventArgs) Handles mnuAbrir.Click, tsbAbrir.Click
        Using openFileDialog As New OpenFileDialog()
            openFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*"
            openFileDialog.Title = "Abrir archivo de texto"

            If openFileDialog.ShowDialog() = DialogResult.OK Then
                Dim frmEditor As New frmEditorTextEdit()
                frmEditor.MdiParent = Me
                frmEditor.RutaArchivo = openFileDialog.FileName ' Corregido
                frmEditor.rtbDocumento.Text = IO.File.ReadAllText(openFileDialog.FileName)
                frmEditor.ArchivoModificado = False
                frmEditor.Text = IO.Path.GetFileName(frmEditor.RutaArchivo) & " - TextEdit VB"
                frmEditor.Show()
                tsslEstado.Text = "Archivo abierto: " & openFileDialog.SafeFileName
            End If
        End Using
    End Sub

    Private Sub mnuGuardar_Click(sender As Object, e As EventArgs) Handles mnuGuardar.Click, tsbGuardar.Click
        If TypeOf Me.ActiveMdiChild Is frmEditorTextEdit Then
            DirectCast(Me.ActiveMdiChild, frmEditorTextEdit).GuardarDocumento()
            tsslEstado.Text = "Documento guardado"
        Else
            MessageBox.Show("No hay ningún documento activo para guardar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub mnuGuardarComo_Click(sender As Object, e As EventArgs) Handles mnuGuardarComo.Click
        If TypeOf Me.ActiveMdiChild Is frmEditorTextEdit Then
            DirectCast(Me.ActiveMdiChild, frmEditorTextEdit).GuardarComoDocumento()
            tsslEstado.Text = "Documento guardado como..."
        End If
    End Sub

    Private Sub mnuSalir_Click(sender As Object, e As EventArgs) Handles mnuSalir.Click
        Application.Exit()
    End Sub

    ' MENÚ: EDICIÓN
    Private Sub mnuDeshacer_Click(sender As Object, e As EventArgs) Handles mnuDeshacer.Click
        If TypeOf Me.ActiveMdiChild Is frmEditorTextEdit Then
            Dim editor = DirectCast(Me.ActiveMdiChild, frmEditorTextEdit)
            If editor.rtbDocumento.CanUndo Then editor.rtbDocumento.Undo()
        End If
    End Sub

    Private Sub mnuRehacer_Click(sender As Object, e As EventArgs) Handles mnuRehacer.Click
        If TypeOf Me.ActiveMdiChild Is frmEditorTextEdit Then
            Dim editor = DirectCast(Me.ActiveMdiChild, frmEditorTextEdit)
            If editor.rtbDocumento.CanRedo Then editor.rtbDocumento.Redo()
        End If
    End Sub

    Private Sub mnuCortar_Click(sender As Object, e As EventArgs) Handles mnuCortar.Click, tsbCortar.Click
        If TypeOf Me.ActiveMdiChild Is frmEditorTextEdit Then
            DirectCast(Me.ActiveMdiChild, frmEditorTextEdit).rtbDocumento.Cut()
        End If
    End Sub

    Private Sub mnuCopiar_Click(sender As Object, e As EventArgs) Handles mnuCopiar.Click, tsbCopiar.Click
        If TypeOf Me.ActiveMdiChild Is frmEditorTextEdit Then
            DirectCast(Me.ActiveMdiChild, frmEditorTextEdit).rtbDocumento.Copy()
        End If
    End Sub

    Private Sub mnuPegar_Click(sender As Object, e As EventArgs) Handles mnuPegar.Click, tsbPegar.Click
        If TypeOf Me.ActiveMdiChild Is frmEditorTextEdit Then
            DirectCast(Me.ActiveMdiChild, frmEditorTextEdit).rtbDocumento.Paste()
        End If
    End Sub

    Private Sub mnuSeleccionarTodo_Click(sender As Object, e As EventArgs) Handles mnuSeleccionarTodo.Click
        If TypeOf Me.ActiveMdiChild Is frmEditorTextEdit Then
            DirectCast(Me.ActiveMdiChild, frmEditorTextEdit).rtbDocumento.SelectAll()
        End If
    End Sub

    ' MENÚ: FORMATO
    Private Sub mnuFuente_Click(sender As Object, e As EventArgs) Handles mnuFuente.Click
        If TypeOf Me.ActiveMdiChild Is frmEditorTextEdit Then
            Dim editor = DirectCast(Me.ActiveMdiChild, frmEditorTextEdit)
            Using fontDialog As New FontDialog()
                fontDialog.Font = editor.rtbDocumento.SelectionFont
                If fontDialog.ShowDialog() = DialogResult.OK Then
                    editor.rtbDocumento.SelectionFont = fontDialog.Font
                End If
            End Using
        End If
    End Sub

    Private Sub mnuColorTexto_Click(sender As Object, e As EventArgs) Handles mnuColorTexto.Click
        If TypeOf Me.ActiveMdiChild Is frmEditorTextEdit Then
            Dim editor = DirectCast(Me.ActiveMdiChild, frmEditorTextEdit)
            Using colorDialog As New ColorDialog()
                colorDialog.Color = editor.rtbDocumento.SelectionColor
                If colorDialog.ShowDialog() = DialogResult.OK Then
                    editor.rtbDocumento.SelectionColor = colorDialog.Color
                End If
            End Using
        End If
    End Sub

    ' MENÚ: HERRAMIENTAS
    Private Sub mnuBuscar_Click(sender As Object, e As EventArgs) Handles mnuBuscar.Click, tsbBuscar.Click
        If TypeOf Me.ActiveMdiChild Is frmEditorTextEdit Then
            Dim editor = DirectCast(Me.ActiveMdiChild, frmEditorTextEdit)
            Dim textoABuscar As String = InputBox("Ingrese la palabra o frase a buscar:", "Buscar Texto")

            If Not String.IsNullOrEmpty(textoABuscar) Then
                Dim posicion As Integer = editor.rtbDocumento.Find(textoABuscar, editor.rtbDocumento.SelectionStart + editor.rtbDocumento.SelectionLength, RichTextBoxFinds.None)
                If posicion <> -1 Then
                    editor.rtbDocumento.Select(posicion, textoABuscar.Length)
                    editor.rtbDocumento.Focus()
                    tsslEstado.Text = "Texto encontrado."
                Else
                    MessageBox.Show("No se encontró el texto especificado.", "Buscar", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    tsslEstado.Text = "Texto no encontrado."
                End If
            End If
        End If
    End Sub

    Private Sub mnuContarPalabras_Click(sender As Object, e As EventArgs) Handles mnuContarPalabras.Click
        If TypeOf Me.ActiveMdiChild Is frmEditorTextEdit Then
            Dim editor = DirectCast(Me.ActiveMdiChild, frmEditorTextEdit)
            Dim palabras As Integer = 0
            If Not String.IsNullOrWhiteSpace(editor.rtbDocumento.Text) Then
                palabras = editor.rtbDocumento.Text.Split(New Char() {" "c, ControlChars.Cr, ControlChars.Lf}, StringSplitOptions.RemoveEmptyEntries).Length
            End If
            MessageBox.Show("El documento actual contiene " & palabras & " palabras.", "Contador de Palabras", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub mnuContarCaracteres_Click(sender As Object, e As EventArgs) Handles mnuContarCaracteres.Click
        If TypeOf Me.ActiveMdiChild Is frmEditorTextEdit Then
            Dim editor = DirectCast(Me.ActiveMdiChild, frmEditorTextEdit)
            Dim caracteres As Integer = editor.rtbDocumento.Text.Length
            MessageBox.Show("El documento actual contiene " & caracteres & " caracteres.", "Contador de Caracteres", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    ' MENÚ: AYUDA
    Private Sub mnuAcercaDe_Click(sender As Object, e As EventArgs) Handles mnuAcercaDe.Click
        MessageBox.Show("TextEdit VB - Editor de Texto de Escritorio" & vbCrLf &
                        "Desarrollado con Visual Basic .NET y Windows Forms." & vbCrLf &
                        "Programación de Aplicaciones de Escritorio - CUR Estelí",
                        "Acerca de TextEdit VB", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

End Class