Imports System.IO
Imports System.Drawing.Imaging

Public Class Form1
    Dim _path As String
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        openConnection()
        LoadRootDrives()
    End Sub

    Public Sub LoadRootDrives()
        For Each drive As DriveInfo In DriveInfo.GetDrives
            Dim node As New TreeNode(drive.Name)
            With node
                .Tag = drive.Name
                .ImageKey = "folder"
                .ImageIndex = 4
                .SelectedImageKey = "folder"
                .Nodes.Add("Empty")
            End With

            TreeView1.Nodes.Add(node)
        Next
    End Sub

    Public Sub LoadChildren(nd As TreeNode, dir As String)
        Dim DirectoryInformation As New DirectoryInfo(dir)

        Dim SubItems() As ListViewItem.ListViewSubItem
        Dim Item As ListViewItem = Nothing

        Try
            'Load all sub folders into the node
            For Each d As DirectoryInfo In DirectoryInformation.GetDirectories

                If Not (d.Attributes And FileAttributes.Hidden) = FileAttributes.Hidden Then
                    Dim folder As New TreeNode(d.Name)

                    With folder
                        .Tag = d.FullName
                        .ImageKey = "folder"
                        .SelectedImageKey = "folder"
                        .Nodes.Add("*Empty*")
                    End With

                    nd.Nodes.Add(folder)

                    Item = New ListViewItem(d.Name, 4)
                    SubItems = New ListViewItem.ListViewSubItem() {New ListViewItem.ListViewSubItem(Item, 0), New ListViewItem.ListViewSubItem(Item, "Directory"), New ListViewItem.ListViewSubItem(Item, d.LastAccessTime.ToShortDateString())}

                    Item.SubItems.AddRange(SubItems)
                End If

            Next

            'load all files into the child nodes
            For Each file As FileInfo In DirectoryInformation.GetFiles
                If Not (file.Attributes And FileAttributes.Hidden) = FileAttributes.Hidden Then
                    Dim FN As New TreeNode(file.Name)
                    With FN
                        .Tag = file.FullName
                        .ImageKey = "file"
                        .SelectedImageKey = "file"
                    End With



                    Select Case file.FullName.Split(".").LastOrDefault().ToLower()

                        'Image Formats
                        Case "bin"
                            nd.Nodes.Add(FN)
                        Case "iso"
                            nd.Nodes.Add(FN)
                        Case "img"
                            nd.Nodes.Add(FN)
                        Case "dmg"
                            nd.Nodes.Add(FN)
                            'Picture Formats
                        Case "bmp"
                            nd.Nodes.Add(FN)
                        Case "jpg"
                            nd.Nodes.Add(FN)
                        Case "png"
                            nd.Nodes.Add(FN)
                        Case "gif"
                            nd.Nodes.Add(FN)
                        Case "tiff"
                            nd.Nodes.Add(FN)
                        Case "jpeg"
                            nd.Nodes.Add(FN)
                        Case "ico"
                            nd.Nodes.Add(FN)
                        Case "jfif"
                            nd.Nodes.Add(FN)

                    End Select

                End If
            Next
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub

    Private Sub TreeView1_BeforeExpand(sender As Object, e As TreeViewCancelEventArgs) Handles TreeView1.BeforeExpand
        Dim IsDriveReady As Boolean = (From d As DriveInfo In DriveInfo.GetDrives Where d.Name = e.Node.Tag Select d.IsReady).FirstOrDefault()

        If (e.Node.Tag <> "Desktop" AndAlso Not e.Node.Tag.Contains(":\")) OrElse IsDriveReady OrElse Directory.Exists(e.Node.Tag) Then
            e.Node.Nodes.Clear()
            LoadChildren(e.Node, e.Node.Tag.ToString)

        ElseIf e.Node.ImageKey = "Dekstop" Then
            e.Node.Nodes.Clear()

            Dim DesktopFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory)
            Dim UserDesktopFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)

            LoadChildren(e.Node, UserDesktopFolder)
            LoadChildren(e.Node, DesktopFolder)
        Else
            e.Cancel = True
            MessageBox.Show("Error drive is empty, " + e.Node.ImageKey.ToString())
        End If

    End Sub


    Private Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
        _path = e.Node.Tag.ToString()
        'MsgBox(_path)
    End Sub

    Private Sub TreeView1_AfterCollapse(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterCollapse
        e.Node.Nodes.Clear()
        e.Node.Nodes.Add("Empty")
    End Sub

    Private Sub TreeView1_DoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseClick
        If e.Button = MouseButtons.Left AndAlso File.Exists(e.Node.Tag.ToString) Then
            Try
                img.Image = Nothing
                img.Image = Image.FromFile(e.Node.Tag)
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        ' ============================================ est5dam elError Provider 3shan elError yb2a bshkl Professional 3la elControlers ely bst5dmha 
        If txt_Name.Text.Trim = "" Then
            ErrorProvider1.SetError(txt_Name, "Please Enter The Picture Name")
            Label1.ForeColor = Color.Red
            Exit Sub
        End If

        '============================================ 3shan a3ml Insert 
        Try
            Dim sql = "select * from TB_ImageData where Name=N'" & txt_Name.Text & "'"
            Dim adp As New SqlClient.SqlDataAdapter(sql, sqlConn)
            Dim ds As New DataSet
            adp.Fill(ds)
            Dim dt = ds.Tables(0)
            If dt.Rows.Count > 0 Then
                MsgBox("This Name is already used, Please Change the Name before return saving ", MsgBoxStyle.Information, "Add Name") : Exit Sub
            Else
                Dim dr = dt.NewRow
                dr!Name = txt_Name.Text
                dr!Address = txt_Address.Text
                dr!Email = txt_Email.Text

                '======================= tshfer elImages ===================
                If Not img.Image Is Nothing Then
                    Dim imgByteArray() As Byte
                    Dim stream As New MemoryStream
                    img.Image.Save(stream, ImageFormat.Jpeg)
                    imgByteArray = stream.ToArray()
                    stream.Close()
                    dr!img = imgByteArray
                End If
                    '=================================================
                    dt.Rows.Add(dr)
                    Dim cmd As New SqlClient.SqlCommandBuilder(adp)
                    adp.Update(dt)
                MsgBox("Data Saved Successful", MsgBoxStyle.Information, "Save")
                End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Save Failed")
        End Try
    End Sub

    Private Sub txt_Name_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_Name.TextChanged
        ErrorProvider1.SetError(txt_Name, "")
        Label1.ForeColor = Color.Black
    End Sub
End Class
