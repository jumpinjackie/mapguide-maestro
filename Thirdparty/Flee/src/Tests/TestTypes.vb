Imports ciloci.Flee
Imports NUnit.Framework
Imports System.ComponentModel
Imports System.Globalization

Public Structure Mouse
	Public S As String
	Public I As Integer
	Public DT As DateTime
	Public Shared SharedDT As DateTime

	Public Sub New(ByVal s As String, ByVal i As Integer)
		Me.S = s
		Me.I = i
		DT = New DateTime(2007, 1, 1)
	End Sub

	Public Function GetI() As Integer
		Return I
	End Function

	Public Function GetYear(ByVal dt As DateTime) As Integer
		Return dt.Year
	End Function

	Default Public ReadOnly Property Item(ByVal i As Integer) As DateTime
		Get
			Return Me.DT
		End Get
	End Property

	Default Public ReadOnly Property Item(ByVal i As Integer, ByVal s As String) As DateTime
		Get
			Return Me.DT
		End Get
	End Property

	Default Public ReadOnly Property Item(ByVal s As String, ByVal i As Integer) As Integer
		Get
			Return i * 2
		End Get
	End Property
End Structure

Public Class Monitor
	Public I As Integer
	Public S As String
	Public DT As DateTime
	Public Shared SharedString As String = "string"

	Public Sub New()
		I = 900
		S = "monitor"
		DT = New DateTime(2007, 1, 1)
	End Sub

	Public Function GetI() As Integer
		Return I
	End Function

	Public Shared Widening Operator CType(ByVal value As Monitor) As Double
		Return 1.0
	End Operator

	Default Public ReadOnly Property Item(ByVal i As Integer) As DateTime
		Get
			Return Me.DT
		End Get
	End Property

	Default Public ReadOnly Property Item(ByVal d As Double, ByVal s As String) As DateTime
		Get
			Return Me.DT
		End Get
	End Property

	Default Public ReadOnly Property Item(ByVal s As String, ByVal ParamArray args As Integer()) As Integer
		Get
			Return -100
		End Get
	End Property
End Class

Public Structure Keyboard
	Public StructA As Mouse
	Public ClassA As Monitor
End Structure

Public Class ExpressionOwner
	Private DoubleA As Double
	Private SingleA As Single
	Private Int32A As Int32
	Private StringA As String
	Private BoolA As Boolean
	Private TypeA As Type
	Private ByteA As Byte
	Private ByteB As Byte
	Private SByteA As SByte
	Private Int16A As Int16
	Private UInt16A As UInt16
	Private IntArr As Integer() = {100, 200, 300}
	Private StringArr As String() = {"a", "b", "c"}
	Private DoubleArr As Double() = {1.1, 2.2, 3.3}
	Private BoolArr As Boolean() = {True, False, True}
	Private CharArr As Char() = {"."}
	Private DateTimeArr As DateTime() = {New DateTime(2007, 7, 1)}
	Private List As IList
	Private StringDict As System.Collections.Specialized.StringDictionary
	Private GuidA As Guid
	Private DateTimeA As DateTime
	Private ICloneableA As ICloneable
	Private ICollectionA As ICollection
	Private VersionA As System.Version
	Private StructA As TestStruct
	Private IComparableA As IComparable
	Private ObjectIntA As Object
	Private ObjectStringA As Object
	Private ValueTypeStructA As ValueType
	Private ExceptionA As Exception
	Private ExceptionNull As Exception
	Private IComparableString As IComparable
	Private IComparableNull As IComparable
	Private ICloneableArray As ICloneable
	Private DelegateANull As System.Delegate
	Private ArrayA As System.Array
	Private DelegateA As AppDomainInitializer
	Private AsciiEncodingArr As System.Text.ASCIIEncoding() = {}
	Private EncodingA As System.Text.Encoding
	Private KeyboardA As Keyboard
	Private DecimalA As Decimal
	Private DecimalB As Decimal
	Private NullField As Object
	Private InstanceA As Object
	Private InstanceB As ArrayList
	Private Dict As Hashtable
	Private GenericDict As Dictionary(Of String, Integer)
	Private Row As DataRow

	Public Sub New()
		Me.InstanceB = New ArrayList()
		Me.InstanceA = Me.InstanceB
		Me.NullField = Nothing
		Me.DecimalA = 100
		Me.DecimalB = 0.25
		Me.KeyboardA = New Keyboard()
		Me.KeyboardA.StructA = New Mouse("mouse", 123)
		Me.KeyboardA.ClassA = New Monitor()
		Me.EncodingA = System.Text.Encoding.ASCII
		Me.DelegateA = AddressOf DoAction
		Me.ICloneableArray = New String() {}
		Me.ArrayA = New String() {}
		Me.DelegateANull = Nothing
		Me.IComparableNull = Nothing
		Me.IComparableString = "string"
		Me.ExceptionA = New ArgumentException
		Me.ExceptionNull = Nothing
		Me.ValueTypeStructA = New TestStruct
		Me.ObjectStringA = "string"
		Me.ObjectIntA = 100
		Me.IComparableA = 100.25
		Me.StructA = New TestStruct()
		Me.VersionA = New System.Version(1, 1, 1, 1)
		Me.ICloneableA = "abc"
		Me.GuidA = Guid.NewGuid()
		Me.List = New ArrayList()
		Me.List.Add("a")
		Me.List.Add(100)
		Me.StringDict = New Specialized.StringDictionary()
		Me.StringDict.Add("key", "value")
		Me.DoubleA = 100.25
		Me.SingleA = 100.25F
		Me.Int32A = 100000
		Me.StringA = "string"
		Me.BoolA = True
		Me.TypeA = GetType(String)
		Me.ByteA = 50
		Me.ByteB = 2
		Me.SByteA = -10
		Me.Int16A = -10
		Me.UInt16A = 100
		Me.DateTimeA = New DateTime(2007, 7, 1)
		Me.GenericDict = New Dictionary(Of String, Integer)()
		Me.GenericDict.Add("a", 100)
		Me.GenericDict.Add("b", 100)

		Me.Dict = New Hashtable()
		Me.Dict.Add(100, Nothing)
		Me.Dict.Add("abc", Nothing)

		Dim dt As New DataTable()
		dt.Columns.Add("ColumnA", GetType(Integer))

		dt.Rows.Add(100)

		Me.Row = dt.Rows(0)
	End Sub

	Private Sub DoAction(ByVal args As String())

	End Sub

	Public Sub DoStuff()

	End Sub

	Public Function DoubleIt(ByVal i As Integer) As Integer
		Return i * 2
	End Function

	Public Function FuncString() As String
		Return "abc"
	End Function

	Public Shared Function SharedFuncInt() As Integer
		Return 100
	End Function

	Private Function PrivateFuncString() As String
		Return "abc"
	End Function

	Public Shared Function PrivateSharedFuncInt() As Integer
		Return 100
	End Function

	Public Function GetDateTime() As DateTime
		Return Me.DateTimeA
	End Function

	Public Function ThrowException() As Integer
		Throw New InvalidOperationException("Should not be thrown!")
	End Function

	Public Function Func1(ByVal al As ArrayList) As ArrayList
		Return al
	End Function

	Public Function ReturnNullString() As String
		Return Nothing
	End Function

	Public Function Sum(ByVal i As Integer) As Integer
		Return 1
	End Function

	Public Function Sum(ByVal i1 As Integer, ByVal i2 As Integer) As Integer
		Return 2
	End Function

	Public Function Sum(ByVal i1 As Integer, ByVal i2 As Double) As Integer
		Return 3
	End Function

	Public Function Sum(ByVal ParamArray args As Integer()) As Integer
		Return 4
	End Function

	Public Function Sum2(ByVal i1 As Integer, ByVal i2 As Double) As Integer
		Return 3
	End Function

	Public Function Sum2(ByVal ParamArray args As Integer()) As Integer
		Return 4
	End Function

	Public Function Sum4(ByVal ParamArray args As Integer()) As Integer
		Dim sum As Integer = 0

		For Each i As Integer In args
			sum += i
		Next

		Return sum
	End Function

	Public Function ParamArray1(ByVal a As String, ByVal ParamArray args As Object()) As Integer
		Return 1
	End Function

	Public Function ParamArray2(ByVal ParamArray args As DateTime()) As Integer
		Return 1
	End Function

	Public Function ParamArray3(ByVal ParamArray args As DateTime()) As Integer
		Return 1
	End Function

	Public Function ParamArray3() As Integer
		Return 2
	End Function

	Public Function ParamArray4(ByVal ParamArray args As Integer()) As Integer
		Return 1
	End Function

	Public Function ParamArray4(ByVal ParamArray args As Object()) As Integer
		Return 2
	End Function

	Public ReadOnly Property DoubleAProp() As Double
		Get
			Return Me.DoubleA
		End Get
	End Property

	Private ReadOnly Property Int32AProp() As Int32
		Get
			Return Me.Int32A
		End Get
	End Property

	Friend Shared ReadOnly Property SharedPropA() As String
		Get
			Return "sharedprop"
		End Get
	End Property
End Class

Friend Class AccessTestExpressionOwner
	Private PrivateField1 As Integer
	<Ciloci.Flee.ExpressionOwnerMemberAccess(True)> _
	Private PrivateField2 As Integer
	<Ciloci.Flee.ExpressionOwnerMemberAccess(False)> _
	Private PrivateField3 As Integer
	Public PublicField1 As Integer
End Class

Friend Class OverloadTestExpressionOwner

	Public A As System.IO.MemoryStream
	Public B As Object

	Public Function ValueType1(ByVal arg As Integer) As Integer
		Return 1
	End Function

	Public Function ValueType1(ByVal arg As Single) As Integer
		Return 2
	End Function

	Public Function ValueType1(ByVal arg As Double) As Integer
		Return 3
	End Function

	Public Function ValueType1(ByVal arg As Decimal) As Integer
		Return 4
	End Function

	Public Function ValueType2(ByVal arg As Single) As Integer
		Return 1
	End Function

	Public Function ValueType2(ByVal arg As Double) As Integer
		Return 2
	End Function

	Public Function ValueType3(ByVal arg As Double) As Integer
		Return 1
	End Function

	Public Function ValueType3(ByVal arg As Decimal) As Integer
		Return 2
	End Function

	Public Function ReferenceType1(ByVal arg As Object) As Integer
		Return 1
	End Function

	Public Function ReferenceType1(ByVal arg As String) As Integer
		Return 2
	End Function

	Public Function ReferenceType2(ByVal arg As Object) As Integer
		Return 1
	End Function

	Public Function ReferenceType2(ByVal arg As System.IO.MemoryStream) As Integer
		Return 2
	End Function

	Public Function ReferenceType3(ByVal arg As Object) As Integer
		Return 1
	End Function

	Public Function ReferenceType3(ByVal arg As IComparable) As Integer
		Return 2
	End Function

	Public Function ReferenceType4(ByVal arg As IFormattable) As Integer
		Return 1
	End Function

	Public Function ReferenceType4(ByVal arg As IComparable) As Integer
		Return 2
	End Function

	Public Function Value_ReferenceType1(ByVal arg As Integer) As Integer
		Return 1
	End Function

	Public Function Value_ReferenceType1(ByVal arg As Object) As Integer
		Return 2
	End Function

	Public Function Value_ReferenceType2(ByVal arg As ValueType) As Integer
		Return 1
	End Function

	Public Function Value_ReferenceType2(ByVal arg As Object) As Integer
		Return 2
	End Function

	Public Function Value_ReferenceType3(ByVal arg As IComparable) As Integer
		Return 1
	End Function

	Public Function Value_ReferenceType3(ByVal arg As Object) As Integer
		Return 2
	End Function

	Public Function Value_ReferenceType4(ByVal arg As IComparable) As Integer
		Return 1
	End Function

	Public Function Value_ReferenceType4(ByVal arg As IFormattable) As Integer
		Return 2
	End Function

	Public Function Access1(ByVal arg As Object) As Integer
		Return 1
	End Function

	<ExpressionOwnerMemberAccess(False)> _
	Public Function Access1(ByVal arg As String) As Integer
		Return 2
	End Function

	<ExpressionOwnerMemberAccess(False)> _
	Public Function Access2(ByVal arg As Object) As Integer
		Return 1
	End Function

	<ExpressionOwnerMemberAccess(False)> _
	Public Function Access2(ByVal arg As String) As Integer
		Return 2
	End Function

	Public Function Multiple1(ByVal arg1 As Single, ByVal arg2 As Double) As Integer
		Return 1
	End Function

	Public Function Multiple1(ByVal arg1 As Integer, ByVal arg2 As Double) As Integer
		Return 2
	End Function
End Class

Friend Class TestImport
	Public Shared Function DoStuff() As Integer
		Return 100
	End Function
End Class

Friend Structure TestStruct
	Implements IComparable

	Private MyA As Integer

	Public Sub New(ByVal a As Integer)
		MyA = a
	End Sub

	Public Function DoStuff() As Integer
		Return 100
	End Function

	Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo

	End Function
End Structure

''' <summary> 
''' This is our custom provider. It simply provides a custom type descriptor 
''' and delegates all its other tasks to its parent 
''' </summary> 
Friend NotInheritable Class UselessTypeDescriptionProvider
	Inherits TypeDescriptionProvider
	''' <summary> 
	''' Constructor 
	''' </summary> 
	Friend Sub New(ByVal parent As TypeDescriptionProvider)
		MyBase.New(parent)
	End Sub

	''' <summary> 
	''' Create and return our custom type descriptor and chain it with the original 
	''' custom type descriptor 
	''' </summary> 
	Public Overloads Overrides Function GetTypeDescriptor(ByVal objectType As Type, ByVal instance As Object) As ICustomTypeDescriptor
		Return New UselessCustomTypeDescriptor(MyBase.GetTypeDescriptor(objectType, instance))
	End Function
End Class

''' <summary> 
''' This is our custom type descriptor. It creates a new property and returns it along 
''' with the original list 
''' </summary> 
Friend NotInheritable Class UselessCustomTypeDescriptor
	Inherits CustomTypeDescriptor
	''' <summary> 
	''' Constructor 
	''' </summary> 
	Friend Sub New(ByVal parent As ICustomTypeDescriptor)
		MyBase.New(parent)
	End Sub

	''' <summary> 
	''' This method add a new property to the original collection 
	''' </summary> 
	Public Overloads Overrides Function GetProperties() As PropertyDescriptorCollection
		' Enumerate the original set of properties and create our new set with it 
		Dim originalProperties As PropertyDescriptorCollection = MyBase.GetProperties()
		Dim newProperties As New List(Of PropertyDescriptor)()
		For Each pd As PropertyDescriptor In originalProperties
			newProperties.Add(pd)
		Next

		' Create a new property and add it to the collection 
		newProperties.Add(New CustomPropertyDescriptor())

		' Finally return the list 
		Return New PropertyDescriptorCollection(newProperties.ToArray(), True)
	End Function
End Class

Friend Class CustomPropertyDescriptor
	Inherits PropertyDescriptor

	Public Sub New()
		MyBase.New("Name", Nothing)
	End Sub

	Public Overrides Function CanResetValue(ByVal component As Object) As Boolean

	End Function

	Public Overrides ReadOnly Property ComponentType() As System.Type
		Get
			Return GetType(Integer)
		End Get
	End Property

	Public Overrides Function GetValue(ByVal component As Object) As Object
		Return "prop!"
	End Function

	Public Overrides ReadOnly Property IsReadOnly() As Boolean
		Get

		End Get
	End Property

	Public Overrides ReadOnly Property PropertyType() As System.Type
		Get
			Return GetType(String)
		End Get
	End Property

	Public Overrides Sub ResetValue(ByVal component As Object)

	End Sub

	Public Overrides Sub SetValue(ByVal component As Object, ByVal value As Object)

	End Sub

	Public Overrides Function ShouldSerializeValue(ByVal component As Object) As Boolean

	End Function
End Class

Public Class NestedA

	Public Class NestedPublicB

		Public Shared Function DoStuff() As Integer
			Return 100
		End Function
	End Class

	Friend Class NestedInternalB

		Public Shared Function DoStuff() As Integer
			Return 100
		End Function
	End Class
End Class