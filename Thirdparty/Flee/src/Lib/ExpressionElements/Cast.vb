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

' Elements that look like normal functions but behave in a special way

Imports System.Reflection.Emit
Imports System.Reflection

' Implements explicit casts and conversions
Friend Class CastElement
	Inherits ExpressionElement

	Private MyCastExpression As ExpressionElement
	' The type we are casting to
	Private MyDestType As Type

	Public Sub New(ByVal castExpression As ExpressionElement, ByVal destTypeParts As String(), ByVal isArray As Boolean, ByVal services As IServiceProvider)
		MyCastExpression = castExpression

		MyDestType = GetDestType(destTypeParts, services)

		If MyDestType Is Nothing Then
			MyBase.ThrowCompileException(CompileErrorResourceKeys.CouldNotResolveType, CompileExceptionReason.UndefinedName, GetDestTypeString(destTypeParts, isArray))
		End If

		If isArray = True Then
			MyDestType = MyDestType.MakeArrayType()
		End If

		If Me.IsValidCast(MyCastExpression.ResultType, MyDestType) = False Then
			Me.ThrowInvalidCastException()
		End If
	End Sub

	Private Shared Function GetDestTypeString(ByVal parts As String(), ByVal isArray As Boolean) As String
		Dim s As String = String.Join(".", parts)

		If isArray = True Then
			s = s & "[]"
		End If

		Return s
	End Function

	' Resolve the type we are casting to
	Private Shared Function GetDestType(ByVal destTypeParts As String(), ByVal services As IServiceProvider) As Type
		Dim context As ExpressionContext = services.GetService(GetType(ExpressionContext))

		Dim t As Type = Nothing

		' Try to find a builtin type with the name
		If destTypeParts.Length = 1 Then
			t = ExpressionImports.GetBuiltinType(destTypeParts(0))
		End If

		If Not t Is Nothing Then
			Return t
		End If

		' Try to find the type in an import
		t = context.Imports.FindType(destTypeParts)

		If Not t Is Nothing Then
			Return t
		End If

		Return Nothing
	End Function

	Private Function IsValidCast(ByVal sourceType As Type, ByVal destType As Type) As Boolean
		If sourceType Is destType Then
			' Identity cast always succeeds
			Return True
		ElseIf destType.IsAssignableFrom(sourceType) = True Then
			' Cast is already implicitly valid
			Return True
		ElseIf ImplicitConverter.EmitImplicitConvert(sourceType, destType, Nothing) = True Then
			' Cast is already implicitly valid
			Return True
		ElseIf IsCastableNumericType(sourceType) And IsCastableNumericType(destType) Then
			' Explicit cast of numeric types always succeeds
			Return True
		ElseIf sourceType.IsEnum = True Or destType.IsEnum = True Then
			Return Me.IsValidExplicitEnumCast(sourceType, destType)
		ElseIf Not Me.GetExplictOverloadedOperator(sourceType, destType) Is Nothing Then
			' Overloaded explict cast exists
			Return True
		End If

		If sourceType.IsValueType = True Then
			' If we get here then the cast always fails since we are either casting one value type to another
			' or a value type to an invalid reference type
			Return False
		Else
			If destType.IsValueType = True Then
				' Reference type to value type
				' Can only succeed if the reference type is a base of the value type or
				' it is one of the interfaces the value type implements
				Dim interfaces As Type() = destType.GetInterfaces()
				Return IsBaseType(destType, sourceType) = True Or System.Array.IndexOf(interfaces, sourceType) <> -1
			Else
				' Reference type to reference type
				Return Me.IsValidExplicitReferenceCast(sourceType, destType)
			End If
		End If
	End Function

	Private Function GetExplictOverloadedOperator(ByVal sourceType As Type, ByVal destType As Type) As MethodInfo
		Dim binder As New ExplicitOperatorMethodBinder(destType, sourceType)

		' Look for an operator on the source type and dest types
		Dim miSource As MethodInfo = Utility.GetOverloadedOperator("Explicit", sourceType, binder, sourceType)
		Dim miDest As MethodInfo = Utility.GetOverloadedOperator("Explicit", destType, binder, sourceType)

		If miSource Is Nothing And miDest Is Nothing Then
			Return Nothing
		ElseIf miSource Is Nothing Then
			Return miDest
		ElseIf miDest Is Nothing Then
			Return miSource
		Else
			MyBase.ThrowAmbiguousCallException(sourceType, destType, "Explicit")
			Return Nothing
		End If
	End Function

	Private Function IsValidExplicitEnumCast(ByVal sourceType As Type, ByVal destType As Type) As Boolean
		sourceType = GetUnderlyingEnumType(sourceType)
		destType = GetUnderlyingEnumType(destType)
		Return Me.IsValidCast(sourceType, destType)
	End Function

	Private Function IsValidExplicitReferenceCast(ByVal sourceType As Type, ByVal destType As Type) As Boolean
		Debug.Assert(sourceType.IsValueType = False And destType.IsValueType = False, "expecting reference types")

		If sourceType Is GetType(Object) Then
			' From object to any other reference-type
			Return True
		ElseIf sourceType.IsArray = True And destType.IsArray = True Then
			' From an array-type S with an element type SE to an array-type T with an element type TE,
			' provided all of the following are true:

			' S and T have the same number of dimensions
			If sourceType.GetArrayRank() <> destType.GetArrayRank() Then
				Return False
			Else
				Dim SE As Type = sourceType.GetElementType()
				Dim TE As Type = destType.GetElementType()

				' Both SE and TE are reference-types
				If SE.IsValueType = True Or TE.IsValueType = True Then
					Return False
				Else
					' An explicit reference conversion exists from SE to TE
					Return Me.IsValidExplicitReferenceCast(SE, TE)
				End If
			End If
		ElseIf sourceType.IsClass = True And destType.IsClass = True Then
			' From any class-type S to any class-type T, provided S is a base class of T
			Return IsBaseType(destType, sourceType)
		ElseIf sourceType.IsClass = True And destType.IsInterface = True Then
			' From any class-type S to any interface-type T, provided S is not sealed and provided S does not implement T
			Return sourceType.IsSealed = False And ImplementsInterface(sourceType, destType) = False
		ElseIf sourceType.IsInterface = True And destType.IsClass = True Then
			' From any interface-type S to any class-type T, provided T is not sealed or provided T implements S.
			Return destType.IsSealed = False Or ImplementsInterface(destType, sourceType) = True
		ElseIf sourceType.IsInterface = True And destType.IsInterface = True Then
			' From any interface-type S to any interface-type T, provided S is not derived from T
			Return ImplementsInterface(sourceType, destType) = False
		Else
			Debug.Assert(False, "unknown explicit cast")
		End If
	End Function

	Private Shared Function IsBaseType(ByVal target As Type, ByVal potentialBase As Type) As Boolean
		Dim current As Type = target
		While Not current Is Nothing
			If current Is potentialBase Then
				Return True
			End If
			current = current.BaseType
		End While
		Return False
	End Function

	Private Shared Function ImplementsInterface(ByVal target As Type, ByVal interfaceType As Type) As Boolean
		Dim interfaces As Type() = target.GetInterfaces()
		Return System.Array.IndexOf(interfaces, interfaceType) <> -1
	End Function

	Private Sub ThrowInvalidCastException()
		MyBase.ThrowCompileException(CompileErrorResourceKeys.CannotConvertType, CompileExceptionReason.InvalidExplicitCast, MyCastExpression.ResultType.Name, MyDestType.Name)
	End Sub

	Private Shared Function IsCastableNumericType(ByVal t As Type) As Boolean
		Return t.IsPrimitive = True And Not t Is GetType(Boolean)
	End Function

	Private Shared Function GetUnderlyingEnumType(ByVal t As Type) As Type
		If t.IsEnum = True Then
			Return System.Enum.GetUnderlyingType(t)
		Else
			Return t
		End If
	End Function

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		MyCastExpression.Emit(ilg, services)

		Dim sourceType As Type = MyCastExpression.ResultType
		Dim destType As Type = MyDestType

		Me.EmitCast(ilg, sourceType, destType, services)
	End Sub

	Private Sub EmitCast(ByVal ilg As FleeILGenerator, ByVal sourceType As Type, ByVal destType As Type, ByVal services As IServiceProvider)
		Dim explicitOperator As MethodInfo = Me.GetExplictOverloadedOperator(sourceType, destType)

		If sourceType Is destType Then
			' Identity cast; do nothing
			Return
		ElseIf Not explicitOperator Is Nothing Then
			ilg.Emit(OpCodes.Call, explicitOperator)
		ElseIf sourceType.IsEnum = True Or destType.IsEnum = True Then
			Me.EmitEnumCast(ilg, sourceType, destType, services)
		ElseIf ImplicitConverter.EmitImplicitConvert(sourceType, destType, ilg) = True Then
			' Implicit numeric cast; do nothing
			Return
		ElseIf IsCastableNumericType(sourceType) And IsCastableNumericType(destType) Then
			' Explicit numeric cast
			EmitExplicitNumericCast(ilg, sourceType, destType, services)
		ElseIf sourceType.IsValueType = True Then
			Debug.Assert(destType.IsValueType = False, "expecting reference type")
			ilg.Emit(OpCodes.Box, sourceType)
		Else
			If destType.IsValueType = True Then
				' Reference type to value type
				ilg.Emit(OpCodes.Unbox_Any, destType)
			Else
				' Reference type to reference type
				If destType.IsAssignableFrom(sourceType) = False Then
					' Only emit cast if it is an explicit cast
					ilg.Emit(OpCodes.Castclass, destType)
				End If
			End If
		End If
	End Sub

	Private Sub EmitEnumCast(ByVal ilg As FleeILGenerator, ByVal sourceType As Type, ByVal destType As Type, ByVal services As IServiceProvider)
		If destType.IsValueType = False Then
			ilg.Emit(OpCodes.Box, sourceType)
		ElseIf sourceType.IsValueType = False Then
			ilg.Emit(OpCodes.Unbox_Any, destType)
		Else
			sourceType = GetUnderlyingEnumType(sourceType)
			destType = GetUnderlyingEnumType(destType)
			Me.EmitCast(ilg, sourceType, destType, services)
		End If
	End Sub

	Private Shared Sub EmitExplicitNumericCast(ByVal ilg As FleeILGenerator, ByVal sourceType As Type, ByVal destType As Type, ByVal services As IServiceProvider)
		Dim desttc As TypeCode = Type.GetTypeCode(destType)
		Dim sourcetc As TypeCode = Type.GetTypeCode(sourceType)
		Dim unsigned As Boolean = IsUnsignedType(sourceType)
		Dim options As ExpressionOptions = services.GetService(GetType(ExpressionOptions))
		Dim checked As Boolean = options.Checked
		Dim op As OpCode = OpCodes.Nop

		Select Case desttc
			Case TypeCode.SByte
				If unsigned = True And checked = True Then
					op = OpCodes.Conv_Ovf_I1_Un
				ElseIf checked = True Then
					op = OpCodes.Conv_Ovf_I1
				Else
					op = OpCodes.Conv_I1
				End If
			Case TypeCode.Byte
				If unsigned = True And checked = True Then
					op = OpCodes.Conv_Ovf_U1_Un
				ElseIf checked = True Then
					op = OpCodes.Conv_Ovf_U1
				Else
					op = OpCodes.Conv_U1
				End If
			Case TypeCode.Int16
				If unsigned = True And checked = True Then
					op = OpCodes.Conv_Ovf_I2_Un
				ElseIf checked = True Then
					op = OpCodes.Conv_Ovf_I2
				Else
					op = OpCodes.Conv_I2
				End If
			Case TypeCode.UInt16
				If unsigned = True And checked = True Then
					op = OpCodes.Conv_Ovf_U2_Un
				ElseIf checked = True Then
					op = OpCodes.Conv_Ovf_U2
				Else
					op = OpCodes.Conv_U2
				End If
			Case TypeCode.Int32
				If unsigned = True And checked = True Then
					op = OpCodes.Conv_Ovf_I4_Un
				ElseIf checked = True Then
					op = OpCodes.Conv_Ovf_I4
				ElseIf sourcetc <> TypeCode.UInt32 Then
					' Don't need to emit a convert for this case since, to the CLR, it is the same data type
					op = OpCodes.Conv_I4
				End If
			Case TypeCode.UInt32
				If unsigned = True And checked = True Then
					op = OpCodes.Conv_Ovf_U4_Un
				ElseIf checked = True Then
					op = OpCodes.Conv_Ovf_U4
				ElseIf sourcetc <> TypeCode.Int32 Then
					op = OpCodes.Conv_U4
				End If
			Case TypeCode.Int64
				If unsigned = True And checked = True Then
					op = OpCodes.Conv_Ovf_I8_Un
				ElseIf checked = True Then
					op = OpCodes.Conv_Ovf_I8
				ElseIf sourcetc <> TypeCode.UInt64 Then
					op = OpCodes.Conv_I8
				End If
			Case TypeCode.UInt64
				If unsigned = True And checked = True Then
					op = OpCodes.Conv_Ovf_U8_Un
				ElseIf checked = True Then
					op = OpCodes.Conv_Ovf_U8
				ElseIf sourcetc <> TypeCode.Int64 Then
					op = OpCodes.Conv_U8
				End If
			Case TypeCode.Single
				op = OpCodes.Conv_R4
			Case Else
				Debug.Assert(False, "Unknown cast dest type")
		End Select

		If op.Equals(OpCodes.Nop) = False Then
			ilg.Emit(op)
		End If
	End Sub

	Private Shared Function IsUnsignedType(ByVal t As Type) As Boolean
		Dim tc As TypeCode = Type.GetTypeCode(t)
		Select Case tc
			Case TypeCode.Byte, TypeCode.UInt16, TypeCode.UInt32, TypeCode.UInt64
				Return True
			Case Else
				Return False
		End Select
	End Function

	Public Overrides ReadOnly Property ResultType() As System.Type
		Get
			Return MyDestType
		End Get
	End Property
End Class