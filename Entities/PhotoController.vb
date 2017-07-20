Imports DotNetNuke.Common.Utilities

Namespace Ventrian.PropertyAgent

    Public Class PhotoController

#Region " Private Members "

        Private _cacheKey As String = "Ventrian-PropertyAgent-"

#End Region

#Region " Public Methods "

        Public Function [Get](ByVal photoID As Integer) As PhotoInfo

            If (photoID = Null.NullInteger) Then
                Return Nothing
            End If

            Dim objPhoto As PhotoInfo = CType(DataCache.GetCache(_cacheKey & photoID.ToString()), PhotoInfo)

            If (objPhoto Is Nothing) Then
                objPhoto = CType(CBO.FillObject(DataProvider.Instance().GetPhoto(photoID), GetType(PhotoInfo)), PhotoInfo)
                DataCache.SetCache(_cacheKey & photoID.ToString(), objPhoto)
            End If

            Return objPhoto

        End Function

        Public Function List(ByVal propertyID As Integer) As ArrayList

            Return List(propertyID, Null.NullString)

        End Function

        Public Function List(ByVal propertyID As Integer, ByVal propertyGuid As String) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().ListPhoto(propertyID, propertyGuid), GetType(PhotoInfo))

        End Function

        Public Function Add(ByVal objPhoto As PhotoInfo) As Integer

            Return CType(DataProvider.Instance().AddPhoto(objPhoto.PropertyID, objPhoto.Title, objPhoto.Filename, objPhoto.DateCreated, objPhoto.Width, objPhoto.Height, objPhoto.SortOrder, objPhoto.PropertyGuid, objPhoto.PhotoType, objPhoto.ExternalUrl, objPhoto.Category), Integer)

        End Function

        Public Sub Update(ByVal objPhoto As PhotoInfo)

            DataCache.RemoveCache(_cacheKey & objPhoto.PhotoID.ToString())
            DataProvider.Instance().UpdatePhoto(objPhoto.PhotoID, objPhoto.PropertyID, objPhoto.Title, objPhoto.Filename, objPhoto.DateCreated, objPhoto.Width, objPhoto.Height, objPhoto.SortOrder, objPhoto.PropertyGuid, objPhoto.PhotoType, objPhoto.ExternalUrl, objPhoto.Category)

        End Sub

        Public Sub Delete(ByVal photoID As Integer)

            DataCache.RemoveCache(_cacheKey & photoID.ToString())
            DataProvider.Instance().DeletePhoto(photoID)

        End Sub

        Public Sub DeleteByPropertyID(ByVal propertyID As Integer)

            DataProvider.Instance().DeletePhotoByPropertyID(propertyID)

        End Sub

#End Region

    End Class

End Namespace
