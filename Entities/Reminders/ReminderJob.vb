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

    Public Class ReminderJob
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

            Dim period As Integer = Null.NullInteger
            If (settings.Contains(Constants.REMINDER_PERIOD_SETTING)) Then
                period = Convert.ToInt32(settings(Constants.REMINDER_PERIOD_SETTING).ToString())
            Else
                Throw New Exception("Property Agent -> Period not found.")
            End If

            Dim periodMeasurement As String = ""
            If (settings.Contains(Constants.REMINDER_PERIOD_MEASUREMENT_SETTING)) Then
                periodMeasurement = settings(Constants.REMINDER_PERIOD_MEASUREMENT_SETTING).ToString()
            Else
                Throw New Exception("Property Agent -> Period Measurement not found.")
            End If

            Dim frequency As Integer = Null.NullInteger
            If (settings.Contains(Constants.REMINDER_FREQUENCY_SETTING)) Then
                frequency = Convert.ToInt32(settings(Constants.REMINDER_FREQUENCY_SETTING).ToString())
            Else
                Throw New Exception("Property Agent -> Frequency not found.")
            End If

            Dim frequencyMeasurement As String = ""
            If (settings.Contains(Constants.REMINDER_FREQUENCY_MEASUREMENT_SETTING)) Then
                frequencyMeasurement = settings(Constants.REMINDER_FREQUENCY_MEASUREMENT_SETTING).ToString()
            Else
                Throw New Exception("Property Agent -> Frequency Measurement not found.")
            End If

            Dim datePeriod As DateTime = DateTime.Now

            Select Case periodMeasurement.ToLower()
                Case "d"
                    datePeriod = datePeriod.AddDays(Convert.ToInt32(period))
                    Exit Select

                Case "h"
                    datePeriod = datePeriod.AddHours(Convert.ToInt32(period))
                    Exit Select

                Case "m"
                    datePeriod = datePeriod.AddMinutes(Convert.ToInt32(period))
                    Exit Select

                Case "s"
                    datePeriod = datePeriod.AddSeconds(Convert.ToInt32(period))
                    Exit Select
            End Select

            Dim dateFrequency As DateTime = DateTime.Now

            Select Case frequencyMeasurement.ToLower()
                Case "d"
                    dateFrequency = dateFrequency.AddDays(Convert.ToInt32(frequency) * -1)
                    Exit Select

                Case "h"
                    dateFrequency = dateFrequency.AddHours(Convert.ToInt32(frequency) * -1)
                    Exit Select

                Case "m"
                    dateFrequency = dateFrequency.AddMinutes(Convert.ToInt32(frequency) * -1)
                    Exit Select

                Case "s"
                    dateFrequency = dateFrequency.AddSeconds(Convert.ToInt32(frequency) * -1)
                    Exit Select
            End Select

            Dim email As String = ""
            If (settings.Contains(Constants.REMINDER_EMAIL_SETTING)) Then
                email = settings(Constants.REMINDER_EMAIL_SETTING).ToString()
            Else
                Throw New Exception("Property Agent -> Portal Email not found.")
            End If

            If (settings.Contains(Constants.REMINDER_PORTAL_NAME_SETTING)) Then
                _portalName = settings(Constants.REMINDER_PORTAL_NAME_SETTING).ToString()
            End If

            Dim subject As String = ""
            If (settings.Contains(Constants.REMINDER_SUBJECT_SETTING)) Then
                subject = settings(Constants.REMINDER_SUBJECT_SETTING).ToString()
            Else
                Throw New Exception("Property Agent -> Subject not found.")
            End If

            Dim template As String = ""
            If (settings.Contains(Constants.REMINDER_TEMPLATE_SETTING)) Then
                template = settings(Constants.REMINDER_TEMPLATE_SETTING).ToString()
            Else
                Throw New Exception("Property Agent -> Template/Body not found.")
            End If

            Dim bccEmail As String = ""
            If (settings.Contains(Constants.REMINDER_BCC_SETTING)) Then
                bccEmail = settings(Constants.REMINDER_BCC_SETTING).ToString().Trim()
            End If

            Dim emailsSent As Integer = 0

            Dim objPropertyController As New PropertyController
            Dim objProperties As List(Of PropertyInfo) = objPropertyController.List(moduleID, Null.NullInteger, SearchStatusType.PublishedActive, Null.NullInteger, Null.NullInteger, False, Null.NullBoolean, SortByType.Expiry, Null.NullInteger, SortDirectionType.Ascending, Null.NullString, Null.NullString, 0, 100000, Null.NullBoolean)

            For Each objProperty As PropertyInfo In objProperties
                If (objProperty.DateExpired <> Null.NullDate) Then
                    If (datePeriod > objProperty.DateExpired Or objProperty.DateExpired < DateTime.Now) Then

                        Dim sendReminder As Boolean = True
                        If (settings.Contains("PA-Reminder-" & objProperty.PropertyID.ToString())) Then
                            Try
                                If (Convert.ToDateTime(settings("PA-Reminder-" & objProperty.PropertyID.ToString()).ToString()) > dateFrequency) Then
                                    sendReminder = False
                                End If
                            Catch
                            End Try
                        End If

                        If (sendReminder) Then

                            Dim sendTo As String = objProperty.Email
                            Dim sendFrom As String = email

                            Dim emailSubject As String = FormatTokens(subject, objProperty, portalID, tabID, settings)
                            Dim emailBody As String = FormatTokens(template, objProperty, portalID, tabID, settings)

                            Try
                                DotNetNuke.Services.Mail.Mail.SendMail(sendFrom, sendTo, bccEmail, emailSubject, emailBody, "", "", "", "", "", "")
                                objModuleController.UpdateModuleSetting(moduleID, "PA-Reminder-" & objProperty.PropertyID.ToString(), DateTime.Now.ToString())
                                Me.ScheduleHistoryItem.AddLogNote("[Success] Property " & objProperty.PropertyID.ToString() & " has been sent a reminder.")
                                emailsSent = emailsSent + 1
                            Catch
                                Me.ScheduleHistoryItem.AddLogNote("[FAILURE] Property " & objProperty.PropertyID.ToString() & " has thrown an exception when trying to send a reminder.")
                            End Try

                        End If

                    End If
                End If
            Next

            Me.ScheduleHistoryItem.AddLogNote(emailsSent.ToString() & " emails sent.")

        End Sub

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
