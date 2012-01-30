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
''' Wraps a regular IL generator and provides additional functionality we need
''' </summary>
''' <remarks></remarks>
Friend Class FleeILGenerator

	Private MyILGenerator As ILGenerator
	Private MyLength As Integer
	Private MyLabelCount As Integer
	Private MyTempLocals As Dictionary(Of Type, LocalBuilder)
	Private MyIsTemp As Boolean

	Public Sub New(ByVal ilg As ILGenerator, Optional ByVal startLength As Integer = 0, Optional ByVal isTemp As Boolean = False)
		MyILGenerator = ilg
		MyTempLocals = New Dictionary(Of Type, LocalBuilder)
		MyIsTemp = isTemp
		MyLength = startLength
	End Sub

	Public Function GetTempLocalIndex(ByVal localType As Type) As Integer
		Dim local As LocalBuilder = Nothing

		If MyTempLocals.TryGetValue(localType, local) = False Then
			local = MyILGenerator.DeclareLocal(localType)
			MyTempLocals.Add(localType, local)
		End If

		Return local.LocalIndex
	End Function

	Public Sub Emit(ByVal op As OpCode)
		Me.RecordOpcode(op)
		MyILGenerator.Emit(op)
	End Sub

	Public Sub Emit(ByVal op As OpCode, ByVal arg As Type)
		Me.RecordOpcode(op)
		MyILGenerator.Emit(op, arg)
	End Sub

	Public Sub Emit(ByVal op As OpCode, ByVal arg As ConstructorInfo)
		Me.RecordOpcode(op)
		MyILGenerator.Emit(op, arg)
	End Sub

	Public Sub Emit(ByVal op As OpCode, ByVal arg As MethodInfo)
		Me.RecordOpcode(op)
		MyILGenerator.Emit(op, arg)
	End Sub

	Public Sub Emit(ByVal op As OpCode, ByVal arg As FieldInfo)
		Me.RecordOpcode(op)
		MyILGenerator.Emit(op, arg)
	End Sub

	Public Sub Emit(ByVal op As OpCode, ByVal arg As Byte)
		Me.RecordOpcode(op)
		MyILGenerator.Emit(op, arg)
	End Sub

	Public Sub Emit(ByVal op As OpCode, ByVal arg As SByte)
		Me.RecordOpcode(op)
		MyILGenerator.Emit(op, arg)
	End Sub

	Public Sub Emit(ByVal op As OpCode, ByVal arg As Short)
		Me.RecordOpcode(op)
		MyILGenerator.Emit(op, arg)
	End Sub

	Public Sub Emit(ByVal op As OpCode, ByVal arg As Integer)
		Me.RecordOpcode(op)
		MyILGenerator.Emit(op, arg)
	End Sub

	Public Sub Emit(ByVal op As OpCode, ByVal arg As Long)
		Me.RecordOpcode(op)
		MyILGenerator.Emit(op, arg)
	End Sub

	Public Sub Emit(ByVal op As OpCode, ByVal arg As Single)
		Me.RecordOpcode(op)
		MyILGenerator.Emit(op, arg)
	End Sub

	Public Sub Emit(ByVal op As OpCode, ByVal arg As Double)
		Me.RecordOpcode(op)
		MyILGenerator.Emit(op, arg)
	End Sub

	Public Sub Emit(ByVal op As OpCode, ByVal arg As String)
		Me.RecordOpcode(op)
		MyILGenerator.Emit(op, arg)
	End Sub

	Public Sub Emit(ByVal op As OpCode, ByVal arg As Label)
		Me.RecordOpcode(op)
		MyILGenerator.Emit(op, arg)
	End Sub

	Public Sub MarkLabel(ByVal lbl As Label)
		MyILGenerator.MarkLabel(lbl)
	End Sub

	Public Function DefineLabel() As Label
		MyLabelCount += 1
		Return MyILGenerator.DefineLabel()
	End Function

	Public Function DeclareLocal(ByVal localType As Type) As LocalBuilder
		Return MyILGenerator.DeclareLocal(localType)
	End Function

	Private Sub RecordOpcode(ByVal op As OpCode)
		'Trace.WriteLine(String.Format("{0:x}: {1}", MyLength, op.Name))
		Dim operandLength As Integer = GetOpcodeOperandSize(op.OperandType)
		MyLength += op.Size + operandLength
	End Sub

	Private Shared Function GetOpcodeOperandSize(ByVal operand As OperandType) As Integer
		Select Case operand
			Case OperandType.InlineNone
				Return 0
			Case OperandType.ShortInlineBrTarget, OperandType.ShortInlineI, OperandType.ShortInlineVar
				Return 1
			Case OperandType.InlineVar
				Return 2
			Case OperandType.InlineBrTarget, OperandType.InlineField, OperandType.InlineI, OperandType.InlineMethod, OperandType.InlineSig, OperandType.InlineString, OperandType.InlineTok, OperandType.InlineType, OperandType.ShortInlineR
				Return 4
			Case OperandType.InlineI8, OperandType.InlineR
				Return 8
			Case Else
				Debug.Fail("Unknown operand type")
		End Select
	End Function

	<Conditional("DEBUG")> _
	Public Sub ValidateLength()
		Debug.Assert(Me.Length = Me.ILGeneratorLength, "ILGenerator length mismatch")
	End Sub

	Public ReadOnly Property Length() As Integer
		Get
			Return MyLength
		End Get
	End Property

	Public ReadOnly Property LabelCount() As Integer
		Get
			Return MyLabelCount
		End Get
	End Property

	Private ReadOnly Property ILGeneratorLength() As Integer
		Get
			Return Utility.GetILGeneratorLength(MyILGenerator)
		End Get
	End Property

	Public ReadOnly Property IsTemp() As Boolean
		Get
			Return MyIsTemp
		End Get
	End Property
End Class