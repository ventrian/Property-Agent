Imports DotNetNuke.Services.Journal
Imports DotNetNuke.Security.Roles.Internal

Namespace Ventrian.PropertyAgent.Social


    Public Class Journal

        Public Const ContentTypeName As String = "Ventrian_PropertyAgent_"

#Region "Internal Methods"

        Friend Shared Sub AddPropertyToJournal(ByVal objProperty As PropertyInfo, ByVal portalId As Integer, ByVal tabId As Integer, ByVal journalUserId As Integer, ByVal journalGroupID As Integer, ByVal url As String, ByVal summary As String)
            Dim objectKey As String = "Ventrian_Article_" + objProperty.PropertyID.ToString() + "_" + journalGroupID.ToString()
            Dim ji As JournalItem = JournalController.Instance.GetJournalItemByKey(portalId, objectKey)

            If Not ji Is Nothing Then
                JournalController.Instance.DeleteJournalItemByKey(portalId, objectKey)
            End If

            ji = New JournalItem

            ji.PortalId = portalId
            ji.ProfileId = journalUserId
            ji.UserId = journalUserId
            ji.ContentItemId = objProperty.PropertyID
            ji.Title = ""
            ji.ItemData = New ItemData()
            ji.ItemData.Url = url
            ji.Summary = summary
            ji.Body = Nothing
            ji.JournalTypeId = 1
            ji.ObjectKey = objectKey
            ji.SecuritySet = "E,"
            If (journalGroupID <> -1) Then
                ji.SocialGroupId = journalGroupID
            End If

            JournalController.Instance.SaveJournalItem(ji, tabId)
        End Sub

        Friend Shared Sub AddCommentToJournal(ByVal objProperty As PropertyInfo, ByVal objComment As CommentInfo, ByVal portalId As Integer, ByVal tabId As Integer, ByVal journalUserId As Integer, ByVal url As String, ByVal title As String)
            Dim objectKey As String = "Ventrian_PropertyAgent_Comment_" + objProperty.PropertyID.ToString() + ":" + objComment.CommentID.ToString()
            Dim ji As JournalItem = JournalController.Instance.GetJournalItemByKey(portalId, objectKey)

            If Not ji Is Nothing Then
                JournalController.Instance.DeleteJournalItemByKey(portalId, objectKey)
            End If

            ji = New JournalItem

            ji.PortalId = portalId
            ji.ProfileId = journalUserId
            ji.UserId = journalUserId
            ji.ContentItemId = objComment.CommentID
            ji.Title = title
            ji.ItemData = New ItemData()
            ji.ItemData.Url = url
            ji.Summary = objComment.Comment
            ji.Body = Nothing
            ji.JournalTypeId = 18
            ji.ObjectKey = objectKey
            ji.SecuritySet = "E,"

            JournalController.Instance.SaveJournalItem(ji, tabId)
        End Sub

        'Friend Sub AddRatingToJournal(ByVal objArticle As ArticleInfo, ByVal objRating As RatingInfo, ByVal portalId As Integer, ByVal tabId As Integer, ByVal journalUserId As Integer, ByVal url As String)
        '    Dim objectKey As String = "Ventrian_Article_Rating_" + objArticle.ArticleID.ToString() + ":" + objRating.RatingID.ToString()
        '    Dim ji As JournalItem = JournalController.Instance.GetJournalItemByKey(portalId, objectKey)

        '    If Not ji Is Nothing Then
        '        JournalController.Instance.DeleteJournalItemByKey(portalId, objectKey)
        '    End If

        '    ji = New JournalItem

        '    ji.PortalId = portalId
        '    ji.ProfileId = journalUserId
        '    ji.UserId = journalUserId
        '    ji.ContentItemId = objRating.RatingID
        '    ji.Title = objArticle.Title
        '    ji.ItemData = New ItemData()
        '    ji.ItemData.Url = url
        '    ji.Summary = objRating.Rating.ToString()
        '    ji.Body = Nothing
        '    ji.JournalTypeId = 17
        '    ji.ObjectKey = objectKey
        '    ji.SecuritySet = "E,"

        '    JournalController.Instance.SaveJournalItem(ji, tabId)
        'End Sub

#End Region

    End Class

End Namespace
