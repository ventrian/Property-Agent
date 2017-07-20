Imports System
Imports System.Data

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Framework

Namespace Ventrian.PropertyAgent

    Public Class PropertyValueController

#Region " Private Members "

        Private Const CACHE_KEY = "-PropertyAgent-ProperyValues-All"

#End Region

#Region " Public Methods "

        Public Function GetByCustomField(ByVal propertyID As Integer, ByVal customFieldID As Integer) As PropertyValueInfo

            Dim objPropertyController As New PropertyController
            Dim objProperty As PropertyInfo = objPropertyController.Get(propertyID)

            If (objProperty IsNot Nothing) Then
                Return GetByCustomField(propertyID, customFieldID, objProperty.ModuleID)
            End If

            Return Nothing

        End Function

        Public Function GetByCustomField(ByVal propertyID As Integer, ByVal customFieldID As Integer, ByVal moduleID As Integer) As PropertyValueInfo

            Dim objPropertyValues As List(Of PropertyValueInfo) = List(propertyID, moduleID)

            For Each objPropertyValue As PropertyValueInfo In objPropertyValues
                If (objPropertyValue.CustomFieldID = customFieldID) Then
                    Return objPropertyValue
                End If
            Next

            Return Nothing

        End Function

        Public Function List(ByVal propertyID As Integer, ByVal moduleID As Integer) As List(Of PropertyValueInfo)

            Dim key As String = propertyID.ToString() & CACHE_KEY

            Dim objPropertyValues As List(Of PropertyValueInfo) = CType(DataCache.GetCache(key), List(Of PropertyValueInfo))

            If (objPropertyValues Is Nothing) Then
                If (HttpContext.Current IsNot Nothing) Then
                    If (HttpContext.Current.Items.Contains(key)) Then
                        Return CType(HttpContext.Current.Items(key), List(Of PropertyValueInfo))
                    End If
                End If
                objPropertyValues = CBO.FillCollection(Of PropertyValueInfo)(DataProvider.Instance().ListPropertyValue(propertyID))

                Dim objController As New DotNetNuke.Entities.Modules.ModuleController
                Dim settings As Hashtable = objController.GetModuleSettings(moduleID)

                Dim useCache As Boolean = Constants.CACHE_PROPERTY_VALUES_SETTING_DEFAULT
                If (settings.ContainsKey(Constants.CACHE_PROPERTY_VALUES_SETTING)) Then
                    useCache = Convert.ToBoolean(settings(Constants.CACHE_PROPERTY_VALUES_SETTING).ToString())
                End If

                If (useCache) Then
                    DataCache.SetCache(key, objPropertyValues)
                Else
                    If (HttpContext.Current IsNot Nothing) Then
                        HttpContext.Current.Items.Add(key, objPropertyValues)
                    End If
                End If
            End If

            Return objPropertyValues

        End Function

        Public Function ListByCustomField(ByVal customFieldID As Integer) As List(Of PropertyValueInfo)

            Return CBO.FillCollection(Of PropertyValueInfo)(DataProvider.Instance().ListPropertyValueByField(customFieldID))

        End Function

        Public Function Add(ByVal objPropertyValue As PropertyValueInfo) As Integer

            DataCache.RemoveCache(objPropertyValue.PropertyID.ToString() & CACHE_KEY)
            Return CType(DataProvider.Instance().AddPropertyValue(objPropertyValue.PropertyID, objPropertyValue.CustomFieldID, objPropertyValue.CustomValue), Integer)

        End Function

        Public Sub Update(ByVal objPropertyValue As PropertyValueInfo)

            DataCache.RemoveCache(objPropertyValue.PropertyID.ToString() & CACHE_KEY)
            DataProvider.Instance().UpdatePropertyValue(objPropertyValue.PropertyValueID, objPropertyValue.PropertyID, objPropertyValue.CustomFieldID, objPropertyValue.CustomValue)

        End Sub

        Public Sub Delete(ByVal propertyID As Integer, ByVal propertyValueID As Integer)

            DataCache.RemoveCache(propertyID.ToString() & CACHE_KEY)
            DataProvider.Instance().DeletePropertyValue(propertyValueID)

        End Sub

#End Region

    End Class

End Namespace
