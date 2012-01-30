' This library is free software; you can redistribute it and/or
' modify it under the terms of the GNU Lesser General Public License
' as published by the Free Software Foundation; either version 2.1
' of the License, or (at your option) any later version.
' 
' This library is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
' Lesser General Public License for more details.
' 
' You should have received a copy of the GNU Lesser General Public
' License along with this library; if not, write to the Free
' Software Foundation, Inc., 59 Temple Place, Suite 330, Boston,
' MA 02111-1307, USA.
' 
' Flee - Fast Lightweight Expression Evaluator
' Copyright © 2007 Eugene Ciloci
'

Imports System.Reflection
Imports System.Reflection.Emit

''' <summary>
''' Holds various shared utility methods
''' </summary>
''' <remarks></remarks>
Friend Class Utility

	Private Sub New()

	End Sub

	Public Shared Sub AssertNotNull(ByVal o As Object, ByVal paramName As String)
		If o Is Nothing Then
			Throw New ArgumentNullException(paramName)
		End If
	End Sub

	Public Shared Sub EmitStoreLocal(ByVal ilg As FleeILGenerator, ByVal index As Integer)
		If index >= 0 And index <= 3 Then
			Select Case index
				Case 0
					ilg.Emit(OpCodes.Stloc_0)
				Case 1
					ilg.Emit(OpCodes.Stloc_1)
				Case 2
					ilg.Emit(OpCodes.Stloc_2)
				Case 3
					ilg.Emit(OpCodes.Stloc_3)
			End Select
		Else
			Debug.Assert(index < 256, "local index too large")
			ilg.Emit(OpCodes.Stloc_S, CByte(index))
		End If
	End Sub

	Public Shared Sub EmitLoadLocal(ByVal ilg As FleeILGenerator, ByVal index As Integer)
		Debug.Assert(index >= 0, "Invalid index")

		If index >= 0 And index <= 3 Then
			Select Case index
				Case 0
					ilg.Emit(OpCodes.Ldloc_0)
				Case 1
					ilg.Emit(OpCodes.Ldloc_1)
				Case 2
					ilg.Emit(OpCodes.Ldloc_2)
				Case 3
					ilg.Emit(OpCodes.Ldloc_3)
			End Select
		Else
			Debug.Assert(index < 256, "local index too large")
			ilg.Emit(OpCodes.Ldloc_S, CByte(index))
		End If
	End Sub

	Public Shared Sub EmitLoadLocalAddress(ByVal ilg As FleeILGenerator, ByVal index As Integer)
		Debug.Assert(index >= 0, "Invalid index")

		If index <= Byte.MaxValue Then
			ilg.Emit(OpCodes.Ldloca_S, CByte(index))
		Else
			ilg.Emit(OpCodes.Ldloca, index)
		End If
	End Sub

	Public Shared Sub EmitArrayLoad(ByVal ilg As FleeILGenerator, ByVal elementType As Type)
		Dim tc As TypeCode = Type.GetTypeCode(elementType)

		Select Case tc
			Case TypeCode.Byte
				ilg.Emit(OpCodes.Ldelem_U1)
			Case TypeCode.SByte, TypeCode.Boolean
				ilg.Emit(OpCodes.Ldelem_I1)
			Case TypeCode.Int16
				ilg.Emit(OpCodes.Ldelem_I2)
			Case TypeCode.UInt16
				ilg.Emit(OpCodes.Ldelem_U2)
			Case TypeCode.Int32
				ilg.Emit(OpCodes.Ldelem_I4)
			Case TypeCode.UInt32
				ilg.Emit(OpCodes.Ldelem_U4)
			Case TypeCode.Int64, TypeCode.UInt64
				ilg.Emit(OpCodes.Ldelem_I8)
			Case TypeCode.Single
				ilg.Emit(OpCodes.Ldelem_R4)
			Case TypeCode.Double
				ilg.Emit(OpCodes.Ldelem_R8)
			Case TypeCode.Object, TypeCode.String
				ilg.Emit(OpCodes.Ldelem_Ref)
			Case Else
				' Must be a non-primitive value type
				ilg.Emit(OpCodes.Ldelema, elementType)
				ilg.Emit(OpCodes.Ldobj, elementType)
				Return
		End Select
	End Sub

	Public Shared Sub EmitArrayStore(ByVal ilg As FleeILGenerator, ByVal elementType As Type)
		Dim tc As TypeCode = Type.GetTypeCode(elementType)

		Select Case tc
			Case TypeCode.Byte, TypeCode.SByte, TypeCode.Boolean
				ilg.Emit(OpCodes.Stelem_I1)
			Case TypeCode.Int16, TypeCode.UInt16
				ilg.Emit(OpCodes.Stelem_I2)
			Case TypeCode.Int32, TypeCode.UInt32
				ilg.Emit(OpCodes.Stelem_I4)
			Case TypeCode.Int64, TypeCode.UInt64
				ilg.Emit(OpCodes.Stelem_I8)
			Case TypeCode.Single
				ilg.Emit(OpCodes.Stelem_R4)
			Case TypeCode.Double
				ilg.Emit(OpCodes.Stelem_R8)
			Case TypeCode.Object, TypeCode.String
				ilg.Emit(OpCodes.Stelem_Ref)
			Case Else
				' Must be a non-primitive value type
				ilg.Emit(OpCodes.Stelem, elementType)
		End Select
	End Sub

	Public Shared Sub SyncFleeILGeneratorLabels(ByVal source As FleeILGenerator, ByVal target As FleeILGenerator)
		While source.LabelCount <> target.LabelCount
			target.DefineLabel()
		End While
	End Sub

	Public Shared Function IsIntegralType(ByVal t As Type) As Boolean
		Dim tc As TypeCode = Type.GetTypeCode(t)
		Select Case tc
			Case TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32, TypeCode.UInt32, TypeCode.Int64, TypeCode.UInt64
				Return True
			Case Else
				Return False
		End Select
	End Function

	Public Shared Function GetBitwiseOpType(ByVal leftType As Type, ByVal rightType As Type) As Type
		If IsIntegralType(leftType) = False OrElse IsIntegralType(rightType) = False Then
			Return Nothing
		Else
			Return ImplicitConverter.GetBinaryResultType(leftType, rightType)
		End If
	End Function

	''' <summary>
	''' Find a simple (unary) overloaded operator
	''' </summary>
	''' <param name="name">The name of the operator</param>
	''' <param name="sourceType">The type to convert from</param>
	''' <param name="destType">The type to convert to</param>
	''' <returns>The operator's method or null of no match is found</returns>
	Public Shared Function GetSimpleOverloadedOperator(ByVal name As String, ByVal sourceType As Type, ByVal destType As Type) As MethodInfo
		Dim data As New Hashtable()
		data.Add("Name", String.Concat("op_", name))
		data.Add("sourceType", sourceType)
		data.Add("destType", destType)

		Const flags As BindingFlags = BindingFlags.Public Or BindingFlags.Static

		' Look on the source type
		Dim members As MemberInfo() = sourceType.FindMembers(MemberTypes.Method, flags, AddressOf SimpleOverloadedOperatorFilter, data)

		If members.Length = 0 Then
			' Look on the dest type
			members = destType.FindMembers(MemberTypes.Method, flags, AddressOf SimpleOverloadedOperatorFilter, data)
		End If

		Debug.Assert(members.Length < 2, "Multiple overloaded operators found")

		If members.Length = 0 Then
			' No match
			Return Nothing
		Else
			Return members(0)
		End If
	End Function

	''' <summary>
	''' Matches simple overloaded operators
	''' </summary>
	''' <param name="member"></param>
	''' <param name="value"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Private Shared Function SimpleOverloadedOperatorFilter(ByVal member As MemberInfo, ByVal value As Object) As Boolean
		Dim data As IDictionary = value
		Dim method As MethodInfo = DirectCast(member, MethodInfo)

		Dim nameMatch As Boolean = method.IsSpecialName = True AndAlso method.Name.Equals(DirectCast(data("Name"), String), StringComparison.OrdinalIgnoreCase)

		If nameMatch = False Then
			Return False
		End If

		Dim returnTypeMatch As Boolean = method.ReturnType Is DirectCast(data("destType"), Type)

		If returnTypeMatch = False Then
			Return False
		End If

		Dim parameters As ParameterInfo() = method.GetParameters()
		Dim argumentMatch As Boolean = parameters.Length > 0 AndAlso parameters(0).ParameterType Is DirectCast(data("sourceType"), Type)

		Return argumentMatch
	End Function

	Public Shared Function GetOverloadedOperator(ByVal name As String, ByVal sourceType As Type, ByVal binder As Binder, ByVal ParamArray argumentTypes As Type()) As MethodInfo
		name = String.Concat("op_", name)
		Dim mi As MethodInfo = sourceType.GetMethod(name, BindingFlags.Public Or BindingFlags.Static, binder, CallingConventions.Any, argumentTypes, Nothing)

		If mi Is Nothing OrElse mi.IsSpecialName = False Then
			Return Nothing
		Else
			Return mi
		End If
	End Function

	Public Shared Function GetILGeneratorLength(ByVal ilg As ILGenerator) As Integer
		Dim fi As System.Reflection.FieldInfo = GetType(ILGenerator).GetField("m_length", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
		Return DirectCast(fi.GetValue(ilg), Integer)
	End Function

	Public Shared Function IsLongBranch(ByVal startPosition As Integer, ByVal endPosition As Integer) As Boolean
		Return (endPosition - startPosition) > SByte.MaxValue
	End Function

	Public Shared Function FormatList(ByVal items As String()) As String
		Dim separator As String = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator + " "
		Return String.Join(separator, items)
	End Function

	Public Shared Function GetGeneralErrorMessage(ByVal key As String, ByVal ParamArray args As Object()) As String
		Dim msg As String = FleeResourceManager.Instance.GetGeneralErrorString(key)
		Return String.Format(msg, args)
	End Function

	Public Shared Function GetCompileErrorMessage(ByVal key As String, ByVal ParamArray args As Object()) As String
		Dim msg As String = FleeResourceManager.Instance.GetCompileErrorString(key)
		Return String.Format(msg, args)
	End Function
End Class