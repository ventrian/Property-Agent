Imports System.Threading

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Mail
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Entities.Tabs

Namespace Ventrian.PropertyAgent

    Public Class NotificationJob
        Inherits DotNetNuke.Services.Scheduling.SchedulerClient

#Region " Private Members "

        Private _portalName As String = ""

#End Region

#Region " Constructors "

        Public Sub New(ByVal objScheduleHistoryItem As DotNetNuke.Services.Scheduling.ScheduleHistoryItem)

            MyBase.new()
            Me.ScheduleHistoryItem = objScheduleHistoryItem

        End Sub

#End Region

        Public Overrides Sub DoWork()
            Try
                'notification that the event is progressing
                Me.Progressing()    'OPTIONAL
                SendReminders()
                Me.ScheduleHistoryItem.Succeeded = True    'REQUIRED

            Catch exc As Exception    'REQUIRED

                Me.ScheduleHistoryItem.Succeeded = False    'REQUIRED
                Me.ScheduleHistoryItem.AddLogNote("Property Agent -> Expiry Reminder failed. " + exc.ToString)     'OPTIONAL
                'notification that we have errored
                Me.Errored(exc)    'REQUIRED
                'log the exception
                LogException(exc)    'OPTIONAL

            End Try
        End Sub

        Private Sub SendReminders()

            Dim portalID As Integer = Convert.ToInt32(Me.ScheduleHistoryItem.GetSetting("PortalID"))
            Dim moduleID As Integer = Convert.ToInt32(Me.ScheduleHistoryItem.GetSetting("ModuleID"))
            Dim tabID As Integer = Convert.ToInt32(Me.ScheduleHistoryItem.GetSetting("TabID"))

            Dim objModuleController As New ModuleController

            Dim settings As Hashtable = objModuleController.GetModuleSettings(moduleID)

            Dim datePeriod As DateTime = DateTime.Now.AddDays(-1)
            If (settings.Contains(Constants.PROPERTY_LAST_DATE_TIME_SETTING)) Then
                datePeriod = DateTime.Parse(settings(Constants.PROPERTY_LAST_DATE_TIME_SETTING).ToString())
            End If

            Dim email As String = ""
            If (settings.Contains(Constants.PROPERTY_EMAIL_SETTING)) Then
                email = settings(Constants.PROPERTY_EMAIL_SETTING).ToString()
            Else
                Throw New Exception("Property Agent -> Portal Email not found.")
            End If

            If (settings.Contains(Constants.PROPERTY_PORTAL_NAME_SETTING)) Then
                _portalName = settings(Constants.PROPERTY_PORTAL_NAME_SETTING).ToString()
            End If

            Dim subject As String = ""
            If (settings.Contains(Constants.PROPERTY_SUBJECT_SETTING)) Then
                subject = settings(Constants.PROPERTY_SUBJECT_SETTING).ToString()
            Else
                Throw New Exception("Property Agent -> Subject not found.")
            End If

            Dim templateHeader As String = ""
            If (settings.Contains(Constants.PROPERTY_TEMPLATE_HEADER_SETTING)) Then
                templateHeader = settings(Constants.PROPERTY_TEMPLATE_HEADER_SETTING).ToString()
            Else
                Throw New Exception("Property Agent -> Template/Header not found.")
            End If

            Dim template As String = ""
            If (settings.Contains(Constants.PROPERTY_TEMPLATE_ITEM_SETTING)) Then
                template = settings(Constants.PROPERTY_TEMPLATE_ITEM_SETTING).ToString()
            Else
                Throw New Exception("Property Agent -> Template/Body not found.")
            End If

            Dim templateFooter As String = ""
            If (settings.Contains(Constants.PROPERTY_TEMPLATE_FOOTER_SETTING)) Then
                templateFooter = settings(Constants.PROPERTY_TEMPLATE_FOOTER_SETTING).ToString()
            Else
                Throw New Exception("Property Agent -> Template/Footer not found.")
            End If

            Dim toEmail As String = ""
            If (settings.Contains(Constants.PROPERTY_NOTIFICATION_TO_SETTING)) Then
                toEmail = settings(Constants.PROPERTY_NOTIFICATION_TO_SETTING).ToString().Trim()
            End If

            Dim bccEmail As String = ""
            If (settings.Contains(Constants.PROPERTY_NOTIFICATION_BCC_SETTING)) Then
                bccEmail = settings(Constants.PROPERTY_NOTIFICATION_BCC_SETTING).ToString().Trim()
            End If

            Dim emailsSent As Integer = 0

            Dim objPropertyController As New PropertyController
            Dim objProperties As List(Of PropertyInfo) = objPropertyController.List(moduleID, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, Null.NullBoolean, SortByType.Published, Null.NullInteger, SortDirectionType.Ascending, Null.NullString, Null.NullString, 0, 100000, Null.NullInteger, Null.NullBoolean, Null.NullInteger, Null.NullInteger, Null.NullInteger, Null.NullDouble, Null.NullDouble, datePeriod)

            If (objProperties.Count > 0) Then

                Dim body As String = ""
                For Each objProperty As PropertyInfo In objProperties
                    body = body & FormatTokens(template, objProperty, portalID, tabID, settings)
                Next

                For Each e As String In toEmail.Split(";"c)
                    Dim sendTo As String = e
                    Dim sendFrom As String = email

                    body = FormatHeaderTokens(templateHeader, portalID, tabID, settings) & body & FormatHeaderTokens(templateFooter, portalID, tabID, settings)

                    Try
                        DotNetNuke.Services.Mail.Mail.SendMail(sendFrom, sendTo, bccEmail, FormatHeaderTokens(subject, portalID, tabID, settings), body, "", "", "", "", "", "")
                        Me.ScheduleHistoryItem.AddLogNote("[SUCCESS] " & e & " sent notification for " & objProperties.Count & " properties.")
                        Me.ScheduleHistoryItem.AddLogNote(FormatHeaderTokens(subject, portalID, tabID, settings))
                        emailsSent = emailsSent + 1
                    Catch
                        Me.ScheduleHistoryItem.AddLogNote("[FAILURE] " & e)
                    End Try
                Next

            End If
            Me.ScheduleHistoryItem.AddLogNote(emailsSent.ToString() & " emails sent.")

            objModuleController.UpdateModuleSetting(moduleID, Constants.PROPERTY_LAST_DATE_TIME_SETTING, DateTime.Now.ToString())

        End Sub

        Private Function FormatHeaderTokens(ByVal text As String, ByVal portalID As Integer, ByVal tabID As Integer, ByVal settings As Hashtable) As String

            Dim formattedText As String = text

            If (settings.Contains(Constants.PROPERTY_LABEL_SETTING)) Then
                formattedText = formattedText.Replace("[PROPERTYLABEL]", settings(Constants.PROPERTY_LABEL_SETTING).ToString())
            Else
                formattedText = formattedText.Replace("[PROPERTYLABEL]", Constants.PROPERTY_LABEL_SETTING_DEFAULT)
            End If

            If (settings.Contains(Constants.PROPERTY_PLURAL_LABEL_SETTING)) Then
                formattedText = formattedText.Replace("[PROPERTYPLURALLABEL]", settings(Constants.PROPERTY_PLURAL_LABEL_SETTING).ToString())
            Else
                formattedText = formattedText.Replace("[PROPERTYPLURALLABEL]", Constants.PROPERTY_LABEL_SETTING_DEFAULT)
            End If

            Return formattedText

        End Function

        Public Function FormatTokens(ByVal text As String, ByVal objProperty As PropertyInfo, ByVal portalID As Integer, ByVal tabID As Integer, ByVal settings As Hashtable) As String

            Dim objCustomFieldController As New CustomFieldController
            Dim objCustomFields As List(Of CustomFieldInfo) = objCustomFieldController.List(objProperty.ModuleID, True)

            Dim objUser As UserInfo = UserController.GetUserById(portalID, objProperty.AuthorID)

            Dim delimStr As String = "[]"
            Dim delimiter As Char() = delimStr.ToCharArray()

            Dim layoutArray As String() = text.Split(delimiter)
            Dim formattedContent As String = ""

            For iPtr As Integer = 0 To layoutArray.Length - 1 Step 2

                formattedContent += layoutArray(iPtr).ToString()

                If iPtr < layoutArray.Length - 1 Then
                    Select Case layoutArray(iPtr + 1)

                        Case "AUTHORID"
                            If (objUser.UserID.ToString() IsNot Nothing) Then
                                formattedContent += objUser.UserID.ToString()
                            End If
                            Exit Select

                        Case "DISPLAYNAME"
                            If (objUser IsNot Nothing) Then
                                formattedContent += objUser.DisplayName
                            End If
                            Exit Select

                        Case "EMAIL"
                            If (objUser IsNot Nothing) Then
                                formattedContent += objUser.Email
                            End If
                            Exit Select

                        Case "EXPIRYDATE"
                            formattedContent += objProperty.DateExpired.ToString("MMMM, dd yyyy")
                            Exit Select

                        Case "FIRSTNAME"
                            If (objUser IsNot Nothing) Then
                                formattedContent += objUser.FirstName
                            End If
                            Exit Select

                        Case "LASTNAME"
                            If (objUser IsNot Nothing) Then
                                formattedContent += objUser.LastName
                            End If
                            Exit Select

                        Case "LINK"
                            Dim objTabController As New TabController
                            Dim objTab As TabInfo = objTabController.GetTab(tabID, portalID, False)

                            If (objTab IsNot Nothing) Then

                                Dim params As New List(Of String)

                                params.Add("agentType=View")
                                params.Add("PropertyID=" & objProperty.PropertyID.ToString())

                                Dim strURL As String = ApplicationURL(tabID)
                                For Each p As String In params
                                    strURL = strURL & "&" & p
                                Next

                                Dim objAliasController As New PortalAliasController
                                Dim col As ArrayList = objAliasController.GetPortalAliasArrayByPortalID(portalID)

                                If (col.Count > 0) Then
                                    strURL = strURL.Replace("~", CType(col(0), PortalAliasInfo).HTTPAlias)
                                    If (strURL.ToLower.StartsWith("http://") = False) Then
                                        strURL = "http://" & strURL
                                    End If
                                    formattedContent += strURL
                                End If
                            End If
                            Exit Select

                        Case "PORTALNAME"
                            formattedContent += _portalName
                            Exit Select

                        Case "PROPERTYID"
                            formattedContent += objProperty.PropertyID.ToString()
                            Exit Select

                        Case "PROPERTYLABEL"
                            If (settings.Contains(Constants.PROPERTY_LABEL_SETTING)) Then
                                formattedContent += settings(Constants.PROPERTY_LABEL_SETTING).ToString()
                            Else
                                formattedContent += Constants.PROPERTY_LABEL_SETTING_DEFAULT
                            End If
                            Exit Select

                        Case "PROPERTYPLURALLABEL"
                            If (settings.Contains(Constants.PROPERTY_PLURAL_LABEL_SETTING)) Then
                                formattedContent += settings(Constants.PROPERTY_PLURAL_LABEL_SETTING).ToString()
                            Else
                                formattedContent += Constants.PROPERTY_PLURAL_LABEL_SETTING_DEFAULT
                            End If
                            Exit Select

                        Case "USERNAME"
                            If (objUser IsNot Nothing) Then
                                formattedContent += objUser.Username
                            End If
                            Exit Select

                        Case Else

                            If (layoutArray(iPtr + 1).ToUpper().StartsWith("CUSTOM:")) Then

                                Dim field As String = layoutArray(iPtr + 1).Substring(7, layoutArray(iPtr + 1).Length - 7).ToLower()

                                Dim customFieldID As Integer = Null.NullInteger
                                Dim objCustomFieldSelected As New CustomFieldInfo
                                Dim isLink As Boolean = False

                                If (field.EndsWith("link")) Then
                                    Dim fieldWithoutLink As String = field.Remove(field.Length - 4, 4)
                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                        If (objCustomField.Name.ToLower() = fieldWithoutLink.ToLower()) Then
                                            customFieldID = objCustomField.CustomFieldID
                                            objCustomFieldSelected = objCustomField
                                            isLink = True
                                        End If
                                    Next
                                End If

                                Dim maxLength As Integer = Null.NullInteger
                                If (field.IndexOf(":"c) <> -1) Then
                                    Try
                                        maxLength = Convert.ToInt32(field.Split(":"c)(1))
                                    Catch
                                        maxLength = Null.NullInteger
                                    End Try
                                    field = field.Split(":"c)(0)
                                End If

                                If (customFieldID = Null.NullInteger) Then
                                    For Each objCustomField As CustomFieldInfo In objCustomFields
                                        If (objCustomField.Name.ToLower() = field.ToLower()) Then
                                            customFieldID = objCustomField.CustomFieldID
                                            objCustomFieldSelected = objCustomField
                                        End If
                                    Next
                                End If

                                If (customFieldID <> Null.NullInteger) Then

                                    Dim i As Integer = 0
                                    If (objProperty.PropertyList.Contains(customFieldID)) Then
                                        Dim objLiteral As New Literal
                                        Dim fieldValue As String = objProperty.PropertyList(customFieldID).ToString()
                                        If (maxLength <> Null.NullInteger) Then
                                            If (fieldValue.Length > maxLength) Then
                                                fieldValue = fieldValue.Substring(0, maxLength)
                                            End If
                                        End If
                                        formattedContent += fieldValue
                                        i = i + 1
                                    End If
                                End If

                                Exit Select

                            End If

                            formattedContent += "[" & layoutArray(iPtr + 1) & "]"
                    End Select
                End If

            Next

            Return formattedContent

        End Function

    End Class

End Namespace
