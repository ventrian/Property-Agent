Namespace Ventrian.PropertyAgent

    Public Class CustomFieldInfo

#Region " Private Members "

        Dim _customFieldID As Integer
        Dim _moduleID As Integer
        Dim _name As String
        Dim _fieldType As CustomFieldType
        Dim _fieldElements As String
        Dim _fieldElementType As FieldElementType
        Dim _fieldElementDropDown As Integer
        Dim _defaultValue As String
        Dim _caption As String
        Dim _captionHelp As String
        Dim _isInManager As Boolean
        Dim _isSortable As Boolean
        Dim _isInListing As Boolean
        Dim _isCaptionHidden As Boolean
        Dim _isFeatured As Boolean
        Dim _isLockDown As Boolean
        Dim _isPublished As Boolean
        Dim _isHidden As Boolean
        Dim _isSearchable As Boolean
        Dim _searchType As SearchType
        Dim _sortOrder As Integer
        Dim _isRequired As Boolean
        Dim _validationType As CustomFieldValidationType
        Dim _fieldElementsFrom As String
        Dim _fieldElementsTo As String
        Dim _length As Integer
        Dim _regularExpression As String
        Dim _includeCount As Boolean
        Dim _hideZeroCount As Boolean

        Dim _inheritSecurity As Boolean

#End Region

#Region " Public Properties "

        Public Property CustomFieldID() As Integer
            Get
                Return _customFieldID
            End Get
            Set(ByVal Value As Integer)
                _customFieldID = Value
            End Set
        End Property

        Public Property ModuleID() As Integer
            Get
                Return _moduleID
            End Get
            Set(ByVal Value As Integer)
                _moduleID = Value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal Value As String)
                _name = Value
            End Set
        End Property

        Public Property FieldElementType() As FieldElementType
            Get
                Return _fieldElementType
            End Get
            Set(ByVal Value As FieldElementType)
                _fieldElementType = Value
            End Set
        End Property

        Public Property FieldElementDropDown() As Integer
            Get
                Return _fieldElementDropDown
            End Get
            Set(ByVal Value As Integer)
                _fieldElementDropDown = Value
            End Set
        End Property

        Public Property FieldElements() As String
            Get
                Return _fieldElements
            End Get
            Set(ByVal Value As String)
                _fieldElements = Value
            End Set
        End Property

        Public ReadOnly Property FieldElementsSql() As List(Of String)
            Get
                Dim elements As New List(Of String)

                Try
                    If (FieldElementType = PropertyAgent.FieldElementType.SqlQuery) Then
                        Dim dr As IDataReader = DataProvider.Instance().ExecuteQuery(FieldElements)

                        While (dr.Read())
                            Dim element As String = dr(0).ToString()
                            elements.Add(element)
                        End While

                        dr.Close()
                    End If
                Catch
                End Try

                Return elements
            End Get
        End Property

        Public Property FieldType() As CustomFieldType
            Get
                Return _fieldType
            End Get
            Set(ByVal Value As CustomFieldType)
                _fieldType = Value
            End Set
        End Property

        Public Property DefaultValue() As String
            Get
                Return _defaultValue
            End Get
            Set(ByVal Value As String)
                _defaultValue = Value
            End Set
        End Property

        Public Property Caption() As String
            Get
                Return _caption
            End Get
            Set(ByVal Value As String)
                _caption = Value
            End Set
        End Property

        Public Property CaptionHelp() As String
            Get
                Return _captionHelp
            End Get
            Set(ByVal Value As String)
                _captionHelp = Value
            End Set
        End Property

        Public Property IsInManager() As Boolean
            Get
                Return _isInManager
            End Get
            Set(ByVal Value As Boolean)
                _isInManager = Value
            End Set
        End Property

        Public Property IsSortable() As Boolean
            Get
                Return _isSortable
            End Get
            Set(ByVal Value As Boolean)
                _isSortable = Value
            End Set
        End Property

        Public Property IsInListing() As Boolean
            Get
                Return _isInListing
            End Get
            Set(ByVal Value As Boolean)
                _isInListing = Value
            End Set
        End Property

        Public Property IsCaptionHidden() As Boolean
            Get
                Return _isCaptionHidden
            End Get
            Set(ByVal Value As Boolean)
                _isCaptionHidden = Value
            End Set
        End Property

        Public Property IsFeatured() As Boolean
            Get
                Return _isFeatured
            End Get
            Set(ByVal Value As Boolean)
                _isFeatured = Value
            End Set
        End Property

        Public Property IsLockDown() As Boolean
            Get
                Return _isLockDown
            End Get
            Set(ByVal Value As Boolean)
                _isLockDown = Value
            End Set
        End Property

        Public Property IsPublished() As Boolean
            Get
                Return _isPublished
            End Get
            Set(ByVal Value As Boolean)
                _isPublished = Value
            End Set
        End Property

        Public Property IsHidden() As Boolean
            Get
                Return _isHidden
            End Get
            Set(ByVal Value As Boolean)
                _isHidden = Value
            End Set
        End Property

        Public Property IsSearchable() As Boolean
            Get
                Return _isSearchable
            End Get
            Set(ByVal Value As Boolean)
                _isSearchable = Value
            End Set
        End Property

        Public Property SearchType() As SearchType
            Get
                Return _searchType
            End Get
            Set(ByVal Value As SearchType)
                _searchType = Value
            End Set
        End Property

        Public Property SortOrder() As Integer
            Get
                Return _sortOrder
            End Get
            Set(ByVal Value As Integer)
                _sortOrder = Value
            End Set
        End Property

        Public Property IsRequired() As Boolean
            Get
                Return _isRequired
            End Get
            Set(ByVal Value As Boolean)
                _isRequired = Value
            End Set
        End Property

        Public Property ValidationType() As CustomFieldValidationType
            Get
                Return _validationType
            End Get
            Set(ByVal Value As CustomFieldValidationType)
                _validationType = Value
            End Set
        End Property

        Public Property FieldElementsFrom() As String
            Get
                Return _fieldElementsFrom
            End Get
            Set(ByVal Value As String)
                _fieldElementsFrom = Value
            End Set
        End Property

        Public Property FieldElementsTo() As String
            Get
                Return _fieldElementsTo
            End Get
            Set(ByVal Value As String)
                _fieldElementsTo = Value
            End Set
        End Property

        Public Property Length() As Integer
            Get
                Return _length
            End Get
            Set(ByVal Value As Integer)
                _length = Value
            End Set
        End Property

        Public Property RegularExpression() As String
            Get
                Return _regularExpression
            End Get
            Set(ByVal Value As String)
                _regularExpression = Value
            End Set
        End Property

        Public Property IncludeCount() As Boolean
            Get
                Return _includeCount
            End Get
            Set(ByVal Value As Boolean)
                _includeCount = Value
            End Set
        End Property

        Public Property HideZeroCount() As Boolean
            Get
                Return _hideZeroCount
            End Get
            Set(ByVal Value As Boolean)
                _hideZeroCount = Value
            End Set
        End Property

        Public Property InheritSecurity() As Boolean
            Get
                Return _inheritSecurity
            End Get
            Set(ByVal Value As Boolean)
                _inheritSecurity = Value
            End Set
        End Property

#End Region

    End Class

End Namespace
