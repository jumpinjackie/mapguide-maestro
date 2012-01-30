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

''' <summary>
''' Resource keys for compile error messages
''' </summary>
''' <remarks></remarks>
Friend Class CompileErrorResourceKeys

	Public Const CouldNotResolveType As String = "CouldNotResolveType"
	Public Const CannotConvertType As String = "CannotConvertType"
	Public Const FirstArgNotBoolean As String = "FirstArgNotBoolean"
	Public Const NeitherArgIsConvertibleToTheOther As String = "NeitherArgIsConvertibleToTheOther"
	Public Const ValueNotRepresentableInType As String = "ValueNotRepresentableInType"
	Public Const SearchArgIsNotKnownCollectionType As String = "SearchArgIsNotKnownCollectionType"
	Public Const OperandNotConvertibleToCollectionType As String = "OperandNotConvertibleToCollectionType"
	Public Const TypeNotArrayAndHasNoIndexerOfType As String = "TypeNotArrayAndHasNoIndexerOfType"
	Public Const ArrayIndexersMustBeOfType As String = "ArrayIndexersMustBeOfType"
	Public Const AmbiguousCallOfFunction As String = "AmbiguousCallOfFunction"
	Public Const NamespaceCannotBeUsedAsType As String = "NamespaceCannotBeUsedAsType"
	Public Const TypeCannotBeUsedAsAnExpression As String = "TypeCannotBeUsedAsAnExpression"
	Public Const StaticMemberCannotBeAccessedWithInstanceReference As String = "StaticMemberCannotBeAccessedWithInstanceReference"
	Public Const ReferenceToNonSharedMemberRequiresObjectReference As String = "ReferenceToNonSharedMemberRequiresObjectReference"
	Public Const FunctionHasNoReturnValue As String = "FunctionHasNoReturnValue"
	Public Const OperationNotDefinedForType As String = "OperationNotDefinedForType"
	Public Const OperationNotDefinedForTypes As String = "OperationNotDefinedForTypes"
	Public Const CannotConvertTypeToExpressionResult As String = "CannotConvertTypeToExpressionResult"
	Public Const AmbiguousOverloadedOperator As String = "AmbiguousOverloadedOperator"
	Public Const NoIdentifierWithName As String = "NoIdentifierWithName"
	Public Const NoIdentifierWithNameOnType As String = "NoIdentifierWithNameOnType"
	Public Const IdentifierIsAmbiguous As String = "IdentifierIsAmbiguous"
	Public Const IdentifierIsAmbiguousOnType As String = "IdentifierIsAmbiguousOnType"
	Public Const CannotReferenceCalcEngineAtomWithoutCalcEngine As String = "CannotReferenceCalcEngineAtomWithoutCalcEngine"
	Public Const CalcEngineDoesNotContainAtom As String = "CalcEngineDoesNotContainAtom"
	Public Const UndefinedFunction As String = "UndefinedFunction"
	Public Const UndefinedFunctionOnType As String = "UndefinedFunctionOnType"
	Public Const NoAccessibleMatches As String = "NoAccessibleMatches"
	Public Const NoAccessibleMatchesOnType As String = "NoAccessibleMatchesOnType"
	Public Const CannotParseType As String = "CannotParseType"
	Public Const MultiArrayIndexNotSupported As String = "MultiArrayIndexNotSupported"

	' Grammatica
	Public Const UnexpectedToken As String = "UNEXPECTED_TOKEN"
	Public Const IO As String = "IO"
	Public Const UnexpectedEof As String = "UNEXPECTED_EOF"
	Public Const UnexpectedChar As String = "UNEXPECTED_CHAR"
	Public Const InvalidToken As String = "INVALID_TOKEN"
	Public Const Analysis As String = "ANALYSIS"
	Public Const LineColumn As String = "LineColumn"

	Public Const SyntaxError As String = "SyntaxError"

	Private Sub New()

	End Sub
End Class

Friend Class GeneralErrorResourceKeys

	Public Const TypeNotAccessibleToExpression As String = "TypeNotAccessibleToExpression"
	Public Const VariableWithNameAlreadyDefined As String = "VariableWithNameAlreadyDefined"
	Public Const UndefinedVariable As String = "UndefinedVariable"
	Public Const InvalidVariableName As String = "InvalidVariableName"
	Public Const CannotDetermineNewVariableType As String = "CannotDetermineNewVariableType"
	Public Const VariableValueNotAssignableToType As String = "VariableValueNotAssignableToType"
	Public Const CouldNotFindPublicStaticMethodOnType As String = "CouldNotFindPublicStaticMethodOnType"
	Public Const OnlyPublicStaticMethodsCanBeImported As String = "OnlyPublicStaticMethodsCanBeImported"
	Public Const InvalidNamespaceName As String = "InvalidNamespaceName"
	Public Const NewOwnerTypeNotAssignableToCurrentOwner As String = "NewOwnerTypeNotAssignableToCurrentOwner"

	Private Sub New()

	End Sub
End Class