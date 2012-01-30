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
Imports System.Reflection.Emit
Imports System.Reflection

' Handles implicit conversions
Friend Class ImplicitConverter

	' Table of results for binary operations using primitives
	Private Shared OurBinaryResultTable As Type(,)
	' Primitive types we support
	Private Shared OurBinaryTypes As Type()

	Shared Sub New()
		' Create a table with all the primitive types
		Dim types As Type() = {GetType(Char), GetType(Byte), GetType(SByte), GetType(Int16), GetType(UInt16), GetType(Int32), GetType(UInt32), GetType(Int64), GetType(UInt64), GetType(Single), GetType(Double)}
		OurBinaryTypes = types
		Dim table(types.Length - 1, types.Length - 1) As Type
		OurBinaryResultTable = table
		FillIdentities(types, table)

		' Fill the table
		AddEntry(GetType(UInt32), GetType(UInt64), GetType(UInt64))
		AddEntry(GetType(Int32), GetType(Int64), GetType(Int64))
		AddEntry(GetType(UInt32), GetType(Int64), GetType(Int64))
		AddEntry(GetType(Int32), GetType(UInt32), GetType(Int64))
		AddEntry(GetType(UInt32), GetType(Single), GetType(Single))
		AddEntry(GetType(UInt32), GetType(Double), GetType(Double))
		AddEntry(GetType(Int32), GetType(Single), GetType(Single))
		AddEntry(GetType(Int32), GetType(Double), GetType(Double))
		AddEntry(GetType(Int64), GetType(Single), GetType(Single))
		AddEntry(GetType(Int64), GetType(Double), GetType(Double))
		AddEntry(GetType(UInt64), GetType(Single), GetType(Single))
		AddEntry(GetType(UInt64), GetType(Double), GetType(Double))
		AddEntry(GetType(Single), GetType(Double), GetType(Double))

		' Byte
		AddEntry(GetType(Byte), GetType(Byte), GetType(Int32))
		AddEntry(GetType(Byte), GetType(SByte), GetType(Int32))
		AddEntry(GetType(Byte), GetType(Int16), GetType(Int32))
		AddEntry(GetType(Byte), GetType(UInt16), GetType(Int32))
		AddEntry(GetType(Byte), GetType(Int32), GetType(Int32))
		AddEntry(GetType(Byte), GetType(UInt32), GetType(UInt32))
		AddEntry(GetType(Byte), GetType(Int64), GetType(Int64))
		AddEntry(GetType(Byte), GetType(UInt64), GetType(UInt64))
		AddEntry(GetType(Byte), GetType(Single), GetType(Single))
		AddEntry(GetType(Byte), GetType(Double), GetType(Double))

		' SByte
		AddEntry(GetType(SByte), GetType(SByte), GetType(Int32))
		AddEntry(GetType(SByte), GetType(Int16), GetType(Int32))
		AddEntry(GetType(SByte), GetType(UInt16), GetType(Int32))
		AddEntry(GetType(SByte), GetType(Int32), GetType(Int32))
		AddEntry(GetType(SByte), GetType(UInt32), GetType(Long))
		AddEntry(GetType(SByte), GetType(Int64), GetType(Int64))
		'invalid -- AddEntry(GetType(SByte), GetType(UInt64), GetType(UInt64))
		AddEntry(GetType(SByte), GetType(Single), GetType(Single))
		AddEntry(GetType(SByte), GetType(Double), GetType(Double))

		' int16
		AddEntry(GetType(Int16), GetType(Int16), GetType(Int32))
		AddEntry(GetType(Int16), GetType(UInt16), GetType(Int32))
		AddEntry(GetType(Int16), GetType(Int32), GetType(Int32))
		AddEntry(GetType(Int16), GetType(UInt32), GetType(Long))
		AddEntry(GetType(Int16), GetType(Int64), GetType(Int64))
		'invalid -- AddEntry(GetType(Int16), GetType(UInt64), GetType(UInt64))
		AddEntry(GetType(Int16), GetType(Single), GetType(Single))
		AddEntry(GetType(Int16), GetType(Double), GetType(Double))

		' Uint16
		AddEntry(GetType(UInt16), GetType(UInt16), GetType(Int32))
		AddEntry(GetType(UInt16), GetType(Int16), GetType(Int32))
		AddEntry(GetType(UInt16), GetType(Int32), GetType(Int32))
		AddEntry(GetType(UInt16), GetType(UInt32), GetType(UInt32))
		AddEntry(GetType(UInt16), GetType(Int64), GetType(Int64))
		AddEntry(GetType(UInt16), GetType(UInt64), GetType(UInt64))
		AddEntry(GetType(UInt16), GetType(Single), GetType(Single))
		AddEntry(GetType(UInt16), GetType(Double), GetType(Double))

		' Char
		AddEntry(GetType(Char), GetType(Char), GetType(Int32))
		AddEntry(GetType(Char), GetType(UInt16), GetType(UInt16))
		AddEntry(GetType(Char), GetType(Int32), GetType(Int32))
		AddEntry(GetType(Char), GetType(UInt32), GetType(UInt32))
		AddEntry(GetType(Char), GetType(Int64), GetType(Int64))
		AddEntry(GetType(Char), GetType(UInt64), GetType(UInt64))
		AddEntry(GetType(Char), GetType(Single), GetType(Single))
		AddEntry(GetType(Char), GetType(Double), GetType(Double))
	End Sub

	Private Sub New()

	End Sub

	Private Shared Sub FillIdentities(ByVal typeList As Type(), ByVal table As Type(,))
		For i As Integer = 0 To typeList.Length - 1
			Dim t As Type = typeList(i)
			table(i, i) = t
		Next
	End Sub

	Private Shared Sub AddEntry(ByVal t1 As Type, ByVal t2 As Type, ByVal result As Type)
		Dim index1 As Integer = GetTypeIndex(t1)
		Dim index2 As Integer = GetTypeIndex(t2)
		OurBinaryResultTable(index1, index2) = result
		OurBinaryResultTable(index2, index1) = result
	End Sub

	Private Shared Function GetTypeIndex(ByVal t As Type) As Integer
		Return System.Array.IndexOf(OurBinaryTypes, t)
	End Function

	Public Shared Function EmitImplicitConvert(ByVal sourceType As Type, ByVal destType As Type, ByVal ilg As FleeILGenerator) As Boolean
		If sourceType Is destType Then
			Return True
		ElseIf EmitOverloadedImplicitConvert(sourceType, destType, ilg) = True Then
			Return True
		ElseIf ImplicitConvertToReferenceType(sourceType, destType, ilg) = True Then
			Return True
		Else
			Return ImplicitConvertToValueType(sourceType, destType, ilg)
		End If
	End Function

	Private Shared Function EmitOverloadedImplicitConvert(ByVal sourceType As Type, ByVal destType As Type, ByVal ilg As FleeILGenerator) As Boolean
		' Look for an implicit operator on the destination type
		Dim mi As MethodInfo = Utility.GetSimpleOverloadedOperator("Implicit", sourceType, destType)

		If mi Is Nothing Then
			' No match
			Return False
		End If

		If Not ilg Is Nothing Then
			ilg.Emit(OpCodes.Call, mi)
		End If

		Return True
	End Function

	Private Shared Function ImplicitConvertToReferenceType(ByVal sourceType As Type, ByVal destType As Type, ByVal ilg As FleeILGenerator) As Boolean
		If destType.IsValueType = True Then
			Return False
		End If

		If sourceType Is GetType(Null) Then
			' Null is always convertible to a reference type
			Return True
		End If

		If destType.IsAssignableFrom(sourceType) = False Then
			Return False
		End If

		If sourceType.IsValueType = True Then
			If Not ilg Is Nothing Then
				ilg.Emit(OpCodes.Box, sourceType)
			End If
		End If

		Return True
	End Function

	Private Shared Function ImplicitConvertToValueType(ByVal sourceType As Type, ByVal destType As Type, ByVal ilg As FleeILGenerator) As Boolean
		' We only handle value types
		If sourceType.IsValueType = False And destType.IsValueType = False Then
			Return False
		End If

		' No implicit conversion to enum.  Have to do this check here since calling GetTypeCode on an enum will return the typecode
		' of the underlying type which screws us up.
		If sourceType.IsEnum = True Or destType.IsEnum = True Then
			Return False
		End If

		Return EmitImplicitNumericConvert(sourceType, destType, ilg)
	End Function

	' Emit an implicit conversion (if the ilg is not null) and returns a value that determines whether the implicit conversion
	' succeeded
	Public Shared Function EmitImplicitNumericConvert(ByVal sourceType As Type, ByVal destType As Type, ByVal ilg As FleeILGenerator) As Boolean
		Dim sourceTypeCode As TypeCode = Type.GetTypeCode(sourceType)
		Dim destTypeCode As TypeCode = Type.GetTypeCode(destType)

		Select Case destTypeCode
			Case TypeCode.Int16
				Return ImplicitConvertToInt16(sourceTypeCode, ilg)
			Case TypeCode.UInt16
				Return ImplicitConvertToUInt16(sourceTypeCode, ilg)
			Case TypeCode.Int32
				Return ImplicitConvertToInt32(sourceTypeCode, ilg)
			Case TypeCode.UInt32
				Return ImplicitConvertToUInt32(sourceTypeCode, ilg)
			Case TypeCode.Double
				Return ImplicitConvertToDouble(sourceTypeCode, ilg)
			Case TypeCode.Single
				Return ImplicitConvertToSingle(sourceTypeCode, ilg)
			Case TypeCode.Int64
				Return ImplicitConvertToInt64(sourceTypeCode, ilg)
			Case TypeCode.UInt64
				Return ImplicitConvertToUInt64(sourceTypeCode, ilg)
			Case Else
				Return False
		End Select
	End Function


	Private Shared Function ImplicitConvertToInt16(ByVal sourceTypeCode As TypeCode, ByVal ilg As FleeILGenerator) As Boolean
		Select Case sourceTypeCode
			Case TypeCode.Byte, TypeCode.SByte, TypeCode.Int16
				Return True
			Case Else
				Return False
		End Select
	End Function

	Private Shared Function ImplicitConvertToUInt16(ByVal sourceTypeCode As TypeCode, ByVal ilg As FleeILGenerator) As Boolean
		Select Case sourceTypeCode
			Case TypeCode.Char, TypeCode.Byte, TypeCode.UInt16
				Return True
			Case Else
				Return False
		End Select
	End Function

	Private Shared Function ImplicitConvertToInt32(ByVal sourceTypeCode As TypeCode, ByVal ilg As FleeILGenerator) As Boolean
		Select Case sourceTypeCode
			Case TypeCode.Char, TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32
				Return True
			Case Else
				Return False
		End Select
	End Function

	Private Shared Function ImplicitConvertToUInt32(ByVal sourceTypeCode As TypeCode, ByVal ilg As FleeILGenerator) As Boolean
		Select Case sourceTypeCode
			Case TypeCode.Char, TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16, TypeCode.UInt32
				Return True
			Case Else
				Return False
		End Select
	End Function

	Private Shared Function ImplicitConvertToDouble(ByVal sourceTypeCode As TypeCode, ByVal ilg As FleeILGenerator) As Boolean
		Select Case sourceTypeCode
			Case TypeCode.Char, TypeCode.SByte, TypeCode.Byte, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32, TypeCode.Single, TypeCode.Int64
				EmitConvert(ilg, OpCodes.Conv_R8)
			Case TypeCode.UInt32, TypeCode.UInt64
				EmitConvert(ilg, OpCodes.Conv_R_Un)
				EmitConvert(ilg, OpCodes.Conv_R8)
			Case TypeCode.Double
			Case Else
				Return False
		End Select

		Return True
	End Function

	Private Shared Function ImplicitConvertToSingle(ByVal sourceTypeCode As TypeCode, ByVal ilg As FleeILGenerator) As Boolean
		Select Case sourceTypeCode
			Case TypeCode.Char, TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32, TypeCode.Int64
				EmitConvert(ilg, OpCodes.Conv_R4)
			Case TypeCode.UInt32, TypeCode.UInt64
				EmitConvert(ilg, OpCodes.Conv_R_Un)
				EmitConvert(ilg, OpCodes.Conv_R4)
			Case TypeCode.Single
			Case Else
				Return False
		End Select

		Return True
	End Function

	Private Shared Function ImplicitConvertToInt64(ByVal sourceTypeCode As TypeCode, ByVal ilg As FleeILGenerator) As Boolean
		Select Case sourceTypeCode
			Case TypeCode.SByte, TypeCode.Int16, TypeCode.Int32
				EmitConvert(ilg, OpCodes.Conv_I8)
			Case TypeCode.Char, TypeCode.Byte, TypeCode.UInt16, TypeCode.UInt32
				EmitConvert(ilg, OpCodes.Conv_U8)
			Case TypeCode.Int64
			Case Else
				Return False
		End Select

		Return True
	End Function

	Private Shared Function ImplicitConvertToUInt64(ByVal sourceTypeCode As TypeCode, ByVal ilg As FleeILGenerator) As Boolean
		Select Case sourceTypeCode
			Case TypeCode.Char, TypeCode.Byte, TypeCode.UInt16, TypeCode.UInt32
				EmitConvert(ilg, OpCodes.Conv_U8)
			Case TypeCode.UInt64
			Case Else
				Return False
		End Select

		Return True
	End Function

	Private Shared Sub EmitConvert(ByVal ilg As FleeILGenerator, ByVal convertOpcode As OpCode)
		If Not ilg Is Nothing Then
			ilg.Emit(convertOpcode)
		End If
	End Sub

	' Get the result type for a binary operation
	Public Shared Function GetBinaryResultType(ByVal t1 As Type, ByVal t2 As Type) As Type
		Dim index1 As Integer = GetTypeIndex(t1)
		Dim index2 As Integer = GetTypeIndex(t2)

		If index1 = -1 Or index2 = -1 Then
			Return Nothing
		Else
			Return OurBinaryResultTable(index1, index2)
		End If
	End Function

	Public Shared Function GetImplicitConvertScore(ByVal sourceType As Type, ByVal destType As Type) As Integer
		If sourceType Is destType Then
			Return 0
		End If

		If sourceType Is GetType(Null) Then
			Return GetInverseDistanceToObject(destType)
		End If

		If Utility.GetSimpleOverloadedOperator("Implicit", sourceType, destType) IsNot Nothing Then
			' Implicit operator conversion, score it at 1 so it's just above the minimum
			Return 1
		End If

		If sourceType.IsValueType = True Then
			If destType.IsValueType = True Then
				' Value type -> value type
				Dim sourceScore As Integer = GetValueTypeImplicitConvertScore(sourceType)
				Dim destScore As Integer = GetValueTypeImplicitConvertScore(destType)

				Return destScore - sourceScore
			Else
				' Value type -> reference type
				Return GetReferenceTypeImplicitConvertScore(sourceType, destType)
			End If
		Else
			If destType.IsValueType = True Then
				' Reference type -> value type
				' Reference types can never be implicitly converted to value types
				Debug.Fail("No implicit conversion from reference type to value type")
			Else
				' Reference type -> reference type
				Return GetReferenceTypeImplicitConvertScore(sourceType, destType)
			End If
		End If
		
	End Function

	Private Shared Function GetValueTypeImplicitConvertScore(ByVal t As Type) As Integer
		Dim tc As TypeCode = Type.GetTypeCode(t)

		Select Case tc
			Case TypeCode.Byte
				Return 1
			Case TypeCode.SByte
				Return 2
			Case TypeCode.Char
				Return 3
			Case TypeCode.Int16
				Return 4
			Case TypeCode.UInt16
				Return 5
			Case TypeCode.Int32
				Return 6
			Case TypeCode.UInt32
				Return 7
			Case TypeCode.Int64
				Return 8
			Case TypeCode.UInt64
				Return 9
			Case TypeCode.Single
				Return 10
			Case TypeCode.Double
				Return 11
			Case TypeCode.Decimal
				Return 11
			Case TypeCode.Boolean
				Return 12
			Case TypeCode.DateTime
				Return 13
			Case Else
				Debug.Assert(False, "unknown value type")
				Return -1
		End Select
	End Function

	Private Shared Function GetReferenceTypeImplicitConvertScore(ByVal sourceType As Type, ByVal destType As Type) As Integer
		If destType.IsInterface = True Then
			Return 100
		Else
			Return GetInheritanceDistance(sourceType, destType)
		End If
	End Function

	Private Shared Function GetInheritanceDistance(ByVal sourceType As Type, ByVal destType As Type) As Integer
		Dim count As Integer = 0
		Dim current As Type = sourceType

		While Not current Is destType
			count += 1
			current = current.BaseType
		End While

		Return count * 1000
	End Function

	Private Shared Function GetInverseDistanceToObject(ByVal t As Type) As Integer
		Dim score As Integer = 1000
		Dim current As Type = t.BaseType

		While Not current Is Nothing
			score -= 100
			current = current.BaseType
		End While

		Return score
	End Function
End Class