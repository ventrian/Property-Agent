Imports DotNetNuke.Common
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports ICSharpCode.SharpZipLib.Zip

Imports System.IO
Imports System.Xml

Namespace Ventrian.PropertyAgent

    Partial Public Class ImportTemplateDefinition
        Inherits PropertyAgentBase

#Region " Private Methods "

        Private Sub BindCrumbs()

            Dim crumbs As New ArrayList

            Dim objCrumbMain As New CrumbInfo
            objCrumbMain.Caption = PropertySettings.MainLabel
            objCrumbMain.Url = NavigateURL()
            crumbs.Add(objCrumbMain)

            Dim objTemplateDefinitions As New CrumbInfo
            objTemplateDefinitions.Caption = Localization.GetString("EditTemplateDefinitions", LocalResourceFile)
            objTemplateDefinitions.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditTemplateDefinitions")
            crumbs.Add(objTemplateDefinitions)

            Dim objImport As New CrumbInfo
            objImport.Caption = Localization.GetString("ImportNewTemplate", LocalResourceFile)
            objImport.Url = Request.RawUrl.ToString()
            crumbs.Add(objImport)

            If (PropertySettings.BreadcrumbPlacement = BreadcrumbType.Portal) Then
                For i As Integer = 0 To crumbs.Count - 1
                    Dim objCrumb As CrumbInfo = crumbs(i)
                    If (i > 0) Then
                        Dim objTab As New DotNetNuke.Entities.Tabs.TabInfo
                        objTab.TabID = -8888 + i
                        objTab.TabName = objCrumb.Caption
                        objTab.Url = objCrumb.Url
                        PortalSettings.ActiveTab.BreadCrumbs.Add(objTab)
                    End If
                Next
            End If

            If (PropertySettings.BreadcrumbPlacement = BreadcrumbType.Module) Then
                rptBreadCrumbs.DataSource = crumbs
                rptBreadCrumbs.DataBind()
            End If

        End Sub

        Private Sub ImportTemplate()

            Dim fileName As String = ExtractFileName(cmdBrowse.PostedFile.FileName)
            Dim folder As String = fileName.Replace(".zip", "")

            Dim unzip As New ZipInputStream(cmdBrowse.PostedFile.InputStream)

            Dim entry As ZipEntry = unzip.GetNextEntry()

            While Not (entry Is Nothing)
                If Not entry.IsDirectory Then

                    Dim name As String = ""
                    Dim path As String = ""
                    Dim buffer() As Byte

                    Dim s As String = entry.Name
                    Dim i As Integer = s.LastIndexOf("\"c)
                    If (i < 0) Then
                        i = s.LastIndexOf("/"c)
                    End If
                    If i < 0 Then
                        name = s.Substring(0, s.Length)
                        path = ""
                    Else
                        name = s.Substring(i + 1, s.Length - (i + 1))
                        path = s.Substring(0, i)
                    End If

                    buffer = New [Byte](Convert.ToInt32(entry.Size) - 1) {}
                    Dim size As Integer = 0
                    While size < buffer.Length
                        size += unzip.Read(buffer, size, buffer.Length - size)
                    End While

                    ' create the actual file folder which includes any relative filepath info
                    Dim fileFolder As String = System.IO.Path.Combine(Globals.ApplicationMapPath & "\DesktopModules\PropertyAgent\Templates\" & folder, path)
                    If Not Directory.Exists(fileFolder) Then
                        Directory.CreateDirectory(fileFolder)
                    End If

                    ' save file
                    Dim FullFileName As String = System.IO.Path.Combine(fileFolder, name)

                    If System.IO.File.Exists(FullFileName) Then
                        System.IO.File.SetAttributes(FullFileName, FileAttributes.Normal)
                    End If
                    Dim fs As New FileStream(FullFileName, FileMode.Create, FileAccess.Write)
                    fs.Write(buffer, 0, buffer.Length)
                    fs.Close()

                End If
                entry = unzip.GetNextEntry
            End While

            Dim objTemplate As New TemplateInfo

            Dim doc As New XmlDocument
            doc.Load(Globals.ApplicationMapPath & "\DesktopModules\PropertyAgent\Templates\" & folder & "\Template.xml")

            Dim nodeRoot As XmlNode = doc.DocumentElement

            Dim objNodes As XmlNodeList = nodeRoot.SelectNodes("Name")
            If (objNodes.Count = 1) Then
                objTemplate.Title = objNodes.Item(0).InnerXml
            Else
                Throw New Exception("Missing Node Value!")
            End If

            objNodes = nodeRoot.SelectNodes("Description")
            If (objNodes.Count = 1) Then
                objTemplate.Description = objNodes.Item(0).InnerXml
            Else
                Throw New Exception("Missing Node Value!")
            End If

            objTemplate.IsPremium = False
            objTemplate.Folder = folder

            Dim objTemplateController As New TemplateController
            objTemplateController.Add(objTemplate)

            Response.Redirect(NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=EditTemplateDefinitions"), True)

        End Sub

        Private Function ExtractFileName(ByVal path As String) As String

            Dim extractPos As Integer = path.LastIndexOf("\") + 1
            Return path.Substring(extractPos, path.Length - extractPos).Replace("/", "_").Replace("..", ".")

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                BindCrumbs()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub


        Private Sub cmdUploadFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUploadFile.Click

            Try

                If (Page.IsValid) Then
                    ImportTemplate()
                End If

                cmdCancel.CssClass = PropertySettings.ButtonClass
                cmdUploadFile.CssClass = PropertySettings.ButtonClass

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                Response.Redirect(NavigateURL(TabId, "", PropertySettings.SEOAgentType & "=EditTemplateDefinitions"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub valFile_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles valFile.ServerValidate

            Try

                If Not (cmdBrowse.PostedFile Is Nothing) Then
                    If (cmdBrowse.PostedFile.ContentLength > 0) Then
                        args.IsValid = True
                        Return
                    End If
                End If
                args.IsValid = False

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub valType_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles valType.ServerValidate

            Try

                If Not (cmdBrowse.PostedFile Is Nothing) Then

                    Dim arr As String() = cmdBrowse.PostedFile.FileName.Split(Convert.ToChar("."))
                    Dim fileType As String = arr(arr.Length - 1).ToLower()

                    If (fileType = "zip") Then
                        args.IsValid = True
                        Return
                    End If

                End If
                args.IsValid = False

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub


        Private Sub valFolderAlreadyExists_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles valFolderAlreadyExists.ServerValidate

            Try

                If Not (cmdBrowse.PostedFile Is Nothing) Then

                    Dim fileName As String = ExtractFileName(cmdBrowse.PostedFile.FileName)
                    Dim folder As String = fileName.Replace(".zip", "")

                    Dim objTemplateController As New TemplateController
                    Dim objTemplate As TemplateInfo = objTemplateController.GetByFolder(folder)

                    If (objTemplate Is Nothing) Then
                        args.IsValid = True
                        Return
                    End If

                End If
                args.IsValid = False

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace