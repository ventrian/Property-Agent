Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Security.Roles

Namespace Ventrian.PropertyAgent

    Public Class AgentController

#Region " Private Members "

        Dim _portalSettings As PortalSettings
        Dim _portalID As Integer
        Dim _propertySettings As PropertySettings

#End Region

#Region " Constructors "

        Public Sub New(ByVal portalSettings As PortalSettings, ByVal propertySettings As PropertySettings, ByVal portalID As Integer)
            _portalSettings = portalSettings
            _portalID = portalID
            _propertySettings = propertySettings
        End Sub

#End Region

#Region " Public Methods "

        Public Function List() As ArrayList

            ' Get list of users with Submit, Approve  permissions  + administrator (portal + host) users

            Dim availableUsers As New ArrayList
            Dim strRoles As String = _propertySettings.PermissionSubmit()
            'NOTE: A user with “approve” permission only, cannot submit properties
            'strRoles += ";" + _propertySettings.PermissionApprove()
            If strRoles <> String.Empty Then
                Dim arrRoles As String() = strRoles.Split(";"c)
                Dim objRoleController As New DotNetNuke.Security.Roles.RoleController
                For Each roleName As String In arrRoles
                    If roleName <> String.Empty Then
                        Dim arrUsersInRole As ArrayList = objRoleController.GetUsersByRoleName(_portalID, roleName)
                        AddUsersToList(availableUsers, arrUsersInRole)
                    End If
                Next
            End If

            Return availableUsers

        End Function

        Public Function ListActive(ByVal portalID As Integer, ByVal moduleID As Integer) As ArrayList

            Return FillUserCollection(portalID, DataProvider.Instance().ListAgentActive(moduleID))

        End Function

        Public Function ListAvailable(ByVal portalID As Integer, ByVal moduleID As Integer, ByVal roles As String, ByVal brokerID As Integer) As ArrayList

            Dim rolesList As String = ""
            Dim objRoleController As New RoleController()

            For Each role As String In roles.Split(";"c)
                Dim objRole As RoleInfo = objRoleController.GetRoleByName(portalID, role)
                If Not (objRole Is Nothing) Then
                    If (rolesList = "") Then
                        rolesList = objRole.RoleID.ToString()
                    Else
                        rolesList = rolesList & "," & objRole.RoleID.ToString()
                    End If
                End If
            Next
            Return FillUserCollection(portalID, DataProvider.Instance().ListAgentAvailable(portalID, moduleID, rolesList, brokerID))

        End Function

        Public Function ListSelected(ByVal portalID As Integer, ByVal moduleID As Integer, ByVal brokerID As Integer) As ArrayList

            Return FillUserCollection(portalID, DataProvider.Instance().ListAgentSelected(moduleID, brokerID))

        End Function

        Public Sub AddBroker(ByVal userID As Integer, ByVal brokerID As Integer, ByVal moduleID As Integer)
            DataProvider.Instance().AddBroker(userID, brokerID, moduleID)
        End Sub

        Public Sub DeleteBroker(ByVal userID As Integer, ByVal brokerID As Integer, ByVal moduleID As Integer)
            DataProvider.Instance().DeleteBroker(userID, brokerID, moduleID)
        End Sub

        Public Function ListApprovers() As ArrayList

            ' Get list of users with Submit, Approve  permissions  + administrator (portal + host) users

            Dim availableUsers As New ArrayList
            Dim strRoles As String = _propertySettings.PermissionApprove()
            'NOTE: A user with “approve” permission only, cannot submit properties
            'strRoles += ";" + _propertySettings.PermissionApprove()
            If strRoles <> String.Empty Then
                Dim arrRoles As String() = strRoles.Split(";"c)
                Dim objRoleController As New DotNetNuke.Security.Roles.RoleController
                For Each roleName As String In arrRoles
                    If roleName <> String.Empty Then
                        Dim arrUsersInRole As ArrayList = objRoleController.GetUsersByRoleName(_portalID, roleName)
                        AddUsersToList(availableUsers, arrUsersInRole)
                    End If
                Next
            End If

            Return availableUsers

        End Function

        Public Function ListOwners(ByVal portalID As Integer, ByVal moduleID As Integer, ByVal roles As String) As ArrayList

            Dim rolesList As String = ""
            Dim objRoleController As New RoleController()

            For Each role As String In roles.Split(";"c)
                Dim objRole As RoleInfo = objRoleController.GetRoleByName(portalID, role)
                If Not (objRole Is Nothing) Then
                    If (rolesList = "") Then
                        rolesList = objRole.RoleID.ToString()
                    Else
                        rolesList = rolesList & "," & objRole.RoleID.ToString()
                    End If
                End If
            Next

            Return FillUserCollection(portalID, DataProvider.Instance().ListBrokers(portalID, moduleID, rolesList))

        End Function

        Public Function ListBrokers() As ArrayList

            Dim availableUsers As New ArrayList
            Dim strRoles As String = _propertySettings.PermissionBroker()
            If strRoles <> String.Empty Then
                Dim arrRoles As String() = strRoles.Split(";"c)
                Dim objRoleController As New DotNetNuke.Security.Roles.RoleController
                For Each roleName As String In arrRoles
                    If roleName <> String.Empty Then
                        Dim arrUsersInRole As ArrayList = objRoleController.GetUsersByRoleName(_portalID, roleName)
                        AddUsersToList(availableUsers, arrUsersInRole)
                    End If
                Next
            End If

            Return availableUsers

        End Function

#End Region

#Region " Private Methods "

        Private Sub AddUsersToList(ByRef arrUsers As ArrayList, ByVal arrUsersToAdd As ArrayList)
            'Avoid duplicated users
            'TODO: optimize?
            If Not arrUsersToAdd Is Nothing Then ' just in case, to avoid unlikely Object reference not set to an instance of an object
                For Each objUser As UserInfo In arrUsersToAdd
                    If Not objUser Is Nothing Then
                        'TODO: understand why I have to tho this, for "Membership.Approved" return the REAL value
                        Dim objUserAux As UserInfo = UserController.GetUserById(_portalID, objUser.UserID)
                        If Not objUserAux Is Nothing Then ' just in case, to avoid unlikely Object reference not set to an instance of an object
                            'If objUserAux.Membership.Approved Then
                            Dim addUser As Boolean = True
                            For Each userinList As UserInfo In arrUsers
                                If objUser.UserID = userinList.UserID Then
                                    addUser = False
                                    Exit For
                                End If
                            Next
                            If addUser Then
                                arrUsers.Add(objUser)
                            End If
                            'End If
                        End If
                    End If
                Next
            End If
        End Sub

        Private Overloads Function FillUserCollection(ByVal portalId As Integer, ByVal dr As IDataReader) As ArrayList
            'Note:  the DataReader returned from this method should contain 2 result sets.  The first set
            '       contains the TotalRecords, that satisfy the filter, the second contains the page
            '       of data

            Dim arrUsers As New ArrayList
            Try
                Dim obj As UserInfo
                While dr.Read
                    ' fill business object
                    obj = FillUserInfo(portalId, dr, False)
                    ' add to collection
                    arrUsers.Add(obj)
                End While

            Catch
            Finally
                ' close datareader
                If Not dr Is Nothing Then
                    dr.Close()
                End If
            End Try

            Return arrUsers

        End Function

        Private Function FillUserInfo(ByVal portalId As Integer, ByVal dr As IDataReader, ByVal CheckForOpenDataReader As Boolean) As UserInfo

            Dim objUserInfo As UserInfo = Nothing
            Dim userName As String = Null.NullString
            Dim email As String = Null.NullString
            Dim updatePassword As Boolean
            Dim isApproved As Boolean

            Try
                ' read datareader
                Dim bContinue As Boolean = True

                If CheckForOpenDataReader Then
                    bContinue = False
                    If dr.Read Then
                        bContinue = True
                    End If
                End If
                If bContinue Then
                    objUserInfo = New UserInfo
                    objUserInfo.PortalID = portalId
                    objUserInfo.UserID = Convert.ToInt32(dr("UserID"))
                    objUserInfo.FirstName = Convert.ToString(dr("FirstName"))
                    objUserInfo.LastName = Convert.ToString(dr("LastName"))
                    Try
                        objUserInfo.DisplayName = Convert.ToString(dr("DisplayName"))
                    Catch
                    End Try
                    objUserInfo.IsSuperUser = Convert.ToBoolean(dr("IsSuperUser"))
                    Try
                        objUserInfo.AffiliateID = Convert.ToInt32(Null.SetNull(dr("AffiliateID"), objUserInfo.AffiliateID))
                    Catch
                    End Try

                    'store username and email in local variables for later use
                    'as assigning them now will trigger a GetUser membership call
                    userName = Convert.ToString(dr("Username"))
                    Try
                        email = Convert.ToString(dr("Email"))
                    Catch
                    End Try
                    Try
                        updatePassword = Convert.ToBoolean(dr("UpdatePassword"))
                    Catch
                    End Try
                    If Not objUserInfo.IsSuperUser Then
                        'For Users the approved/authorised info is stored in UserPortals
                        Try
                            isApproved = Convert.ToBoolean(dr("Authorised"))
                        Catch
                        End Try
                    End If
                End If
            Finally
                If CheckForOpenDataReader And Not dr Is Nothing Then
                    dr.Close()
                End If
            End Try

            If Not objUserInfo Is Nothing Then

                objUserInfo.Username = userName
                objUserInfo.Email = email
                objUserInfo.Membership.UpdatePassword = updatePassword
                If Not objUserInfo.IsSuperUser Then
                    'SuperUser authorisation is managed in aspnet Membership
                    objUserInfo.Membership.Approved = isApproved
                End If
            End If

            Return objUserInfo

        End Function

#End Region

    End Class

End Namespace

