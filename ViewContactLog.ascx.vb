Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.PropertyAgent

    Partial Public Class ViewContactLog
        Inherits PropertyAgentBase

        Private _fieldID As Integer = Null.NullInteger
        Private _showCustomField As Boolean = False

#Region " Private Methods "

        Private Sub BindContactLog()


            Dim objContactLogController As New ContactLogController()

            Dim log As List(Of ContactLogInfo) = objContactLogController.List(ModuleId, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text).AddDays(1).AddMinutes(-1))

            Dim dt As New DataTable

            dt.Columns.Add("DateSent")
            dt.Columns.Add("Subject")
            dt.Columns.Add("Body")

            If (PropertySettings.ContactCustomField <> Null.NullInteger) Then

                Dim objCustomFieldController As New CustomFieldController()
                Dim objCustomField As CustomFieldInfo = objCustomFieldController.Get(PropertySettings.ContactCustomField)

                If (objCustomField IsNot Nothing) Then
                    dt.Columns.Add(objCustomField.Name)
                    _showCustomField = True
                End If
            End If

            Dim objContactFieldController As New ContactFieldController()
            Dim fields As List(Of ContactFieldInfo) = objContactFieldController.List(Me.ModuleId)

            Dim fieldShow As String = ""
            For Each field As ContactFieldInfo In fields
                If (field.ContactFieldID = PropertySettings.ContactLogField) Then
                    fieldShow = field.Caption
                End If
                dt.Columns.Add(field.Caption)
            Next

            For Each objLog As ContactLogInfo In log

                Dim dr As DataRow = dt.NewRow()

                dr(0) = objLog.DateSent
                dr(1) = objLog.Subject
                dr(2) = objLog.Body

                Dim counter As Integer = 3

                If (PropertySettings.ContactCustomField <> Null.NullInteger) Then
                    If (objLog.PropertyID <> Null.NullInteger) Then
                        Dim objPropertyController As New PropertyController
                        Dim objProperty As PropertyInfo = objPropertyController.Get(objLog.PropertyID)

                        If (objProperty IsNot Nothing) Then
                            If (objProperty.PropertyList.Contains(PropertySettings.ContactCustomField)) Then
                                dr(counter) = objProperty.PropertyList(PropertySettings.ContactCustomField).ToString()
                            Else
                                dr(counter) = ""
                            End If
                            counter = counter + 1
                        End If
                    Else
                        dr(counter) = ""
                        counter = counter + 1
                    End If
                End If

                For Each field As ContactFieldInfo In fields
                    If (objLog.FieldValues <> "") Then
                        For Each entry As String In objLog.FieldValues.Split("|||")
                            If (entry <> "") Then
                                Dim arr() As String = entry.Split(":::")
                                Dim caption As String = arr(0)
                                Dim value As String = arr(3)
                                If (caption.ToLower() = field.Caption.ToLower()) Then
                                    dr(counter) = value
                                    If (caption.ToLower() = fieldShow.ToLower()) Then
                                        _fieldID = counter
                                    End If
                                End If
                            End If
                        Next
                    End If
                    counter = counter + 1
                Next

                dt.Rows.Add(dr)

            Next

            grdContactLog.DataSource = dt
            grdContactLog.DataBind()

            If (grdContactLog.Items.Count = 0) Then
                grdContactLog.Visible = False
                lblNoContactLogs.Visible = True
                cmdExport.visible = False
            Else
                grdContactLog.Visible = True
                lblNoContactLogs.Visible = False
                cmdExport.Visible = True
            End If

        End Sub

        Private Sub BindCrumbs()

            Dim crumbs As New ArrayList

            Dim objCrumbMain As New CrumbInfo
            objCrumbMain.Caption = PropertySettings.MainLabel
            objCrumbMain.Url = NavigateURL()
            crumbs.Add(objCrumbMain)

            Dim objTemplateDefinitions As New CrumbInfo
            objTemplateDefinitions.Caption = Localization.GetString("ContactLog", LocalResourceFile)
            objTemplateDefinitions.Url = NavigateURL(Me.TabId, "", PropertySettings.SEOAgentType & "=ContactLog")
            crumbs.Add(objTemplateDefinitions)

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

#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                BindCrumbs()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try

                If (Page.IsPostBack = False) Then
                    txtStartDate.Text = DateTime.Now.AddMonths(-1).ToShortDateString()
                    txtEndDate.Text = DateTime.Now.ToShortDateString()
                    Localization.LocalizeDataGrid(grdContactLog, Me.LocalResourceFile)
                End If
                cmdStartDate.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtStartDate)
                cmdEndDate.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtEndDate)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub DataGrid1_ItemDataBound(ByVal s As Object, ByVal e As DataGridItemEventArgs) Handles grdContactLog.ItemDataBound

            If (_hideColumns) Then

                Dim i As Integer = 0
                For Each tc As TableCell In e.Item.Cells
                    If ((i > 3 And _showCustomField) Or (i > 2 And _showCustomField = False)) Then
                        If (_fieldID <> Null.NullInteger) Then
                            If (i = _fieldID) Then
                                e.Item.Cells(i).Visible = True
                            Else
                                e.Item.Cells(i).Visible = False
                            End If
                        Else
                            e.Item.Cells(i).Visible = False
                        End If
                    Else
                        e.Item.Cells(i).Visible = True
                    End If
                    i = i + 1
                Next

            Else
                Dim i As Integer = 0
                For Each tc As TableCell In e.Item.Cells
                    e.Item.Cells(i).Visible = True
                    i = i + 1
                Next
            End If
        End Sub


        Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click

            Try

                If (Page.IsValid) Then
                    BindContactLog()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

        Private _hideColumns As Boolean = True

        Protected Sub cmdExport_Click(ByVal sender As Object, ByVal e As EventArgs)

            Response.Clear()
            Response.AddHeader("content-disposition", "attachment;filename=ExportContactLog.xls")
            Response.Charset = ""
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.ContentType = "application/vnd.xls"
            Dim stringWrite As New System.IO.StringWriter()
            Dim htmlWrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringWrite)
            _hideColumns = False
            BindContactLog()
            grdContactLog.RenderControl(htmlWrite)
            Response.Write(stringWrite.ToString())
            Response.End()

        End Sub
    End Class

End Namespace