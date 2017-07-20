'
' Property Agent for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2005-2007
' by Ventrian Systems ( support@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.IO
Imports System.Xml

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.UI.UserControls

Namespace Ventrian.PropertyAgent

    Partial Public Class ChangeTemplate
        Inherits PropertyAgentBase

#Region " Private Methods "

        Private Sub BindCrumbs()

            Dim crumbs As New ArrayList

            Dim objCrumbMain As New CrumbInfo
            objCrumbMain.Caption = PropertySettings.MainLabel
            objCrumbMain.Url = NavigateURL()
            crumbs.Add(objCrumbMain)

            Dim objChangeTemplate As New CrumbInfo
            objChangeTemplate.Caption = Localization.GetString("ChangeTemplate", LocalResourceFile)
            objChangeTemplate.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=ChangeTemplate")
            crumbs.Add(objChangeTemplate)

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

        Private Sub BindTemplates()

            Dim objTemplateController As New TemplateController

            drpTemplate.DataSource = objTemplateController.List(Me.PortalId)
            drpTemplate.DataBind()

            If (drpTemplate.Items.Count <> 1) Then
                drpTemplate.Items.Insert(0, _
                                          New ListItem( _
                                                        Localization.GetString("SelectTemplateDropdown", _
                                                                                Me.LocalResourceFile), "-1"))
            End If

        End Sub

        Private Sub InitializeTemplate()

            Dim objCustomFieldController As New CustomFieldController
            Dim objPropertyTypeController As New PropertyTypeController
            Dim objModuleController As New ModuleController

            Dim objTemplateController As New TemplateController
            Dim objTemplate As TemplateInfo = objTemplateController.Get(Convert.ToInt32(drpTemplate.SelectedValue))

            If Not (objTemplate Is Nothing) Then

                If (chkCopyFiles.Checked = False) Then
                    If (Me.PropertySettings.Template <> "") Then

                        ' Delete existing

                        ' - Delete Files in Portal Directory
                        Dim _
                            path As String = Me.PortalSettings.HomeDirectoryMapPath & "PropertyAgent\" & _
                                             Me.ModuleId.ToString() & "\Templates\" & Me.PropertySettings.Template
                        If (Directory.Exists(path)) Then
                            Directory.Delete(path, True)
                        End If

                        ' - Delete Images in Portal Directory
                        Dim _
                            images As String = Me.PortalSettings.HomeDirectoryMapPath & "PropertyAgent\" & _
                                               Me.ModuleId.ToString() & "\Images"
                        If (Directory.Exists(images)) Then
                            Directory.Delete(images, True)
                        End If

                        ' - Delete Custom Values / Properties
                        Dim objPropertyController As New PropertyController
                        objPropertyController.DeleteByModuleID(Me.ModuleId)

                        ' - Delete Custom Fields
                        Dim objCustomFieldsToDelete As List(Of CustomFieldInfo) = objCustomFieldController.List(Me.ModuleId, False)
                        For Each objCustomFieldToDelete As CustomFieldInfo In objCustomFieldsToDelete
                            objCustomFieldController.Delete(objCustomFieldToDelete.CustomFieldID)
                        Next

                        ' - Delete Property Types
                        Dim objPropertyTypesToDelete As List(Of PropertyTypeInfo) = objPropertyTypeController.List(Me.ModuleId, False)
                        For Each objPropertyTypeToDelete As PropertyTypeInfo In objPropertyTypesToDelete
                            objPropertyTypeController.Delete(Me.ModuleId, objPropertyTypeToDelete.PropertyTypeID)
                        Next

                    End If
                End If

                ' Copy Files to Portal Directory
                Dim origin As String = ApplicationMapPath & "\DesktopModules\PropertyAgent\Templates\" & objTemplate.Folder
                Dim destination As String = ""

                If (Me.PropertySettings.Template <> "") Then
                    If (chkCopyFiles.Checked) Then
                        destination = Me.PortalSettings.HomeDirectoryMapPath & "PropertyAgent\" & Me.ModuleId.ToString() & _
                                      "\Templates\" & Me.PropertySettings.Template
                    Else
                        destination = Me.PortalSettings.HomeDirectoryMapPath & "PropertyAgent\" & Me.ModuleId.ToString() & _
                                      "\Templates\" & objTemplate.Folder
                    End If
                Else
                    destination = Me.PortalSettings.HomeDirectoryMapPath & "PropertyAgent\" & Me.ModuleId.ToString() & _
                                  "\Templates\" & objTemplate.Folder
                End If

                CopyDirectory(origin, destination)

                If (chkCopyFiles.Checked = False) Then

                    ' Add Custom Fields
                    Dim doc As New XmlDocument
                    doc.Load( _
                              ApplicationMapPath & "\DesktopModules\PropertyAgent\Templates\" & objTemplate.Folder & _
                              "\Template.xml")

                    Dim nodeRoot As XmlNode = doc.DocumentElement

                    Dim objCustomFields As XmlNodeList = nodeRoot.SelectNodes("CustomFields")

                    For Each objCustomField As XmlNode In objCustomFields
                        For Each objCustomFieldEntry As XmlNode In objCustomField.ChildNodes

                            Dim objCustomFieldInfo As New CustomFieldInfo
                            objCustomFieldInfo.FieldElementType = FieldElementType.Standard
                            CBO.InitializeObject(objCustomFieldInfo, GetType(CustomFieldInfo))
                            objCustomFieldInfo.InheritSecurity = True
                            For Each objCustomFieldValue As XmlNode In objCustomFieldEntry.ChildNodes
                                Select Case objCustomFieldValue.Name.ToLower
                                    Case "name"
                                        objCustomFieldInfo.Name = objCustomFieldValue.InnerXml
                                    Case "caption"
                                        objCustomFieldInfo.Caption = objCustomFieldValue.InnerXml
                                    Case "captionhelp"
                                        objCustomFieldInfo.CaptionHelp = objCustomFieldValue.InnerXml
                                    Case "fieldtype"
                                        objCustomFieldInfo.FieldType = _
                                            CType([Enum].Parse(GetType(CustomFieldType), objCustomFieldValue.InnerXml),  _
                                                CustomFieldType)
                                    Case "fieldelementtype"
                                        objCustomFieldInfo.FieldElementType = _
                                            CType([Enum].Parse(GetType(FieldElementType), objCustomFieldValue.InnerXml),  _
                                                FieldElementType)
                                    Case "fieldelements"
                                        objCustomFieldInfo.FieldElements = objCustomFieldValue.InnerXml
                                    Case "defaultvalue"
                                        objCustomFieldInfo.DefaultValue = objCustomFieldValue.InnerXml
                                    Case "length"
                                        objCustomFieldInfo.Length = Convert.ToInt32(objCustomFieldValue.InnerXml)
                                    Case "required"
                                        objCustomFieldInfo.IsRequired = Convert.ToBoolean(objCustomFieldValue.InnerXml)
                                    Case "validationtype"
                                        objCustomFieldInfo.ValidationType = _
                                            CType( _
                                                [Enum].Parse(GetType(CustomFieldValidationType), _
                                                              objCustomFieldValue.InnerXml), CustomFieldValidationType)
                                    Case "regularexpression"
                                        objCustomFieldInfo.RegularExpression = objCustomFieldValue.InnerXml
                                    Case "searchable"
                                        objCustomFieldInfo.IsSearchable = Convert.ToBoolean(objCustomFieldValue.InnerXml)
                                    Case "searchtype"
                                        objCustomFieldInfo.SearchType = _
                                            CType([Enum].Parse(GetType(SearchType), objCustomFieldValue.InnerXml), _
                                                SearchType)
                                    Case "fieldelementsfrom"
                                        objCustomFieldInfo.FieldElementsFrom = objCustomFieldValue.InnerXml
                                    Case "fieldelementsto"
                                        objCustomFieldInfo.FieldElementsTo = objCustomFieldValue.InnerXml
                                    Case "sortable"
                                        objCustomFieldInfo.IsSortable = Convert.ToBoolean(objCustomFieldValue.InnerXml)
                                    Case "featured"
                                        objCustomFieldInfo.IsFeatured = Convert.ToBoolean(objCustomFieldValue.InnerXml)
                                    Case "listing"
                                        objCustomFieldInfo.IsInListing = Convert.ToBoolean(objCustomFieldValue.InnerXml)
                                    Case "manager"
                                        objCustomFieldInfo.IsInManager = Convert.ToBoolean(objCustomFieldValue.InnerXml)
                                    Case "captionhidden"
                                        objCustomFieldInfo.IsCaptionHidden = _
                                            Convert.ToBoolean(objCustomFieldValue.InnerXml)
                                    Case "hidden"
                                        objCustomFieldInfo.IsHidden = Convert.ToBoolean(objCustomFieldValue.InnerXml)
                                    Case "lockdown"
                                        objCustomFieldInfo.IsLockDown = Convert.ToBoolean(objCustomFieldValue.InnerXml)
                                    Case "inheritsecurity"
                                        objCustomFieldInfo.InheritSecurity = Convert.ToBoolean(objCustomFieldValue.InnerXml)
                                End Select
                            Next
                            objCustomFieldInfo.IsPublished = True
                            objCustomFieldInfo.ModuleID = Me.ModuleId
                            objCustomFieldInfo.IncludeCount = False
                            objCustomFieldInfo.HideZeroCount = False
                            objCustomFieldController.Add(objCustomFieldInfo)
                        Next
                    Next

                    Dim objSettings As XmlNodeList = nodeRoot.SelectNodes("Settings")

                    For Each objSetting As XmlNode In objSettings
                        For Each objSettingEntry As XmlNode In objSetting.ChildNodes
                            Dim name As String = ""
                            Dim value As String = ""
                            For Each objSettingValue As XmlNode In objSettingEntry.ChildNodes
                                Select Case objSettingValue.Name.ToLower
                                    Case "name"
                                        name = objSettingValue.InnerXml
                                    Case "value"
                                        value = objSettingValue.InnerXml
                                End Select
                            Next
                            If _
                                (name <> Constants.PERMISSION_APPROVE_SETTING Or _
                                 name <> Constants.PERMISSION_AUTO_APPROVE_SETTING Or _
                                 name <> Constants.PERMISSION_DETAIL_URL_SETTING Or _
                                 name <> Constants.PERMISSION_SUBMIT_SETTING Or _
                                 name <> Constants.PERMISSION_VIEW_DETAIL_SETTING Or _
                                 name <> Constants.PERMISSION_LOCKDOWN_SETTING) Then
                                If (name = Constants.TEMPLATE_OLD_SETTING) Then
                                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.TEMPLATE_SETTING, value)
                                Else
                                    objModuleController.UpdateModuleSetting(Me.ModuleId, name, value)
                                End If
                            End If
                        Next
                    Next

                    Dim objPropertyTypes As XmlNodeList = nodeRoot.SelectNodes("PropertyTypes")
                    AddPropertyTypes(objPropertyTypes, Null.NullInteger)

                End If

                Response.Redirect(NavigateURL(), True)
            End If

        End Sub

        Private Sub AddPropertyTypes(ByRef objPropertyTypes As XmlNodeList, ByVal parentID As Integer)

            Dim objPropertyTypeController As New PropertyTypeController

            For Each objPropertyType As XmlNode In objPropertyTypes
                For Each objPropertyTypeEntry As XmlNode In objPropertyType.ChildNodes
                    Dim name As String = ""
                    Dim description As String = ""
                    Dim sortOrder As Integer = 0
                    For Each objPropertyTypeValue As XmlNode In objPropertyTypeEntry.ChildNodes
                        Select Case objPropertyTypeValue.Name.ToLower
                            Case "name"
                                name = objPropertyTypeValue.InnerXml
                            Case "description"
                                description = objPropertyTypeValue.InnerXml
                            Case "sortorder"
                                sortOrder = Convert.ToInt32(objPropertyTypeValue.InnerXml)
                        End Select
                    Next
                    Dim objPropertyTypeInfo As New PropertyTypeInfo
                    objPropertyTypeInfo.ParentID = parentID
                    objPropertyTypeInfo.Name = name
                    objPropertyTypeInfo.Description = description
                    objPropertyTypeInfo.SortOrder = sortOrder
                    objPropertyTypeInfo.ModuleID = ModuleId
                    objPropertyTypeInfo.IsPublished = True
                    objPropertyTypeInfo.AllowProperties = True
                    objPropertyTypeInfo.PropertyTypeID = objPropertyTypeController.Add(objPropertyTypeInfo)
                    AddPropertyTypes(objPropertyTypeEntry.SelectNodes("PropertyTypes"), objPropertyTypeInfo.PropertyTypeID)
                Next
            Next

        End Sub

        Public Sub CopyDirectory(ByVal Origin As String, ByVal Destination As String)
            Dim CarpetaActual As DirectoryInfo = New DirectoryInfo(Origin)
            Dim Archivo As FileInfo
            Dim Carpeta As DirectoryInfo

            For Each Archivo In CarpetaActual.GetFiles()
                If Not Directory.Exists(Destination) Then Directory.CreateDirectory(Destination)
                Try
                    Dim path As String = IO.Path.Combine(Destination, Archivo.Name)
                    If (File.Exists(path)) Then
                        File.Delete(path)
                    End If
                    Archivo.CopyTo(path)
                Catch
                End Try
            Next

            For Each Carpeta In CarpetaActual.GetDirectories()
                Dim subDirectory As String = Path.Combine(Destination, Carpeta.Name)
                Try
                    Directory.CreateDirectory(subDirectory)
                Catch
                End Try
                CopyDirectory(Carpeta.FullName, subDirectory)
            Next
        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Init

            Try

                BindCrumbs()

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            Try

                If (Me.PropertySettings.Template = "") Then
                    CType(dshChangeTemplate, SectionHeadControl).Text = Localization.GetString("SelectTemplate", Me.LocalResourceFile)
                    lblChangeTemplateHelp.Text = Localization.GetString("SelectTemplateDescription", Me.LocalResourceFile)
                    cmdCancel.Visible = False
                    trCopyFiles.Visible = False
                Else
                    CType(dshChangeTemplate, SectionHeadControl).Text = Localization.GetString("ChangeTemplate", Me.LocalResourceFile)
                    lblChangeTemplateHelp.Text = Localization.GetString("ChangeTemplateDescription", Me.LocalResourceFile)
                End If

                If (Page.IsPostBack = False) Then
                    If (Me.PropertySettings.Template <> "") Then
                        cmdInitTemplate.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("Confirmation", LocalResourceFile) & "');")
                    End If
                    cmdInitTemplate.ValidationGroup = "PropertyAgent-" & Me.TabModuleId
                    valTemplate.ValidationGroup = "PropertyAgent-" & Me.ModuleId
                    BindTemplates()
                End If

                cmdInitTemplate.CssClass = PropertySettings.ButtonClass
                cmdCancel.CssClass = PropertySettings.ButtonClass

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdInitTemplate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdInitTemplate.Click

            Try

                If (Page.IsValid) Then
                    InitializeTemplate()
                End If

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click

            Try

                Response.Redirect(NavigateURL(), True)

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace