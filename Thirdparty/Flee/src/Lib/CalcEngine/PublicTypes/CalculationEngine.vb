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
Imports System.Text.RegularExpressions

Namespace CalcEngine

	''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="CalculationEngine"]/*' />	
	Public Class CalculationEngine

#Region "Fields"

		Private MyDependencies As DependencyManager(Of ExpressionResultPair)
		' Map of name to node
		Private MyNameNodeMap As Dictionary(Of String, ExpressionResultPair)

#End Region

#Region "Events"

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="CalculationEngine.NodeRecalculated"]/*' />	
		Public Event NodeRecalculated As EventHandler(Of NodeEventArgs)

#End Region

#Region "Constructor"

		Public Sub New()
			MyDependencies = New DependencyManager(Of ExpressionResultPair)(New PairEqualityComparer())
			MyNameNodeMap = New Dictionary(Of String, ExpressionResultPair)(StringComparer.OrdinalIgnoreCase)
		End Sub

#End Region

#Region "Methods - Private"

		Private Sub AddTemporaryHead(ByVal headName As String)
			Dim pair As New GenericExpressionResultPair(Of Integer)()
			pair.SetName(headName)

			If MyNameNodeMap.ContainsKey(headName) = False Then
				MyDependencies.AddTail(pair)
				MyNameNodeMap.Add(headName, pair)
			Else
				Throw New ArgumentException(String.Format("An expression already exists at '{0}'", headName))
			End If
		End Sub

		Private Sub DoBatchLoadAdd(ByVal info As BatchLoadInfo)
			Try
				Me.Add(info.Name, info.ExpressionText, info.Context)
			Catch ex As ExpressionCompileException
				Me.Clear()
				Throw New BatchLoadCompileException(info.Name, info.ExpressionText, ex)
			End Try
		End Sub

		Private Function GetTail(ByVal tailName As String) As ExpressionResultPair
			Utility.AssertNotNull(tailName, "name")
			Dim pair As ExpressionResultPair = Nothing
			MyNameNodeMap.TryGetValue(tailName, pair)
			Return pair
		End Function

		Private Function GetTailWithValidate(ByVal tailName As String) As ExpressionResultPair
			Utility.AssertNotNull(tailName, "name")
			Dim pair As ExpressionResultPair = Me.GetTail(tailName)

			If pair Is Nothing Then
				Throw New ArgumentException(String.Format("No expression is associated with the name '{0}'", tailName))
			Else
				Return pair
			End If
		End Function

		Private Function GetNames(ByVal pairs As IList(Of ExpressionResultPair)) As String()
			Dim names(pairs.Count - 1) As String

			For i As Integer = 0 To names.Length - 1
				names(i) = pairs.Item(i).Name
			Next

			Return names
		End Function

		Private Function GetRootTails(ByVal roots As String()) As ExpressionResultPair()
			' No roots supplied so get everything
			If roots.Length = 0 Then
				Return MyDependencies.GetTails()
			End If

			' Get the tail for each name
			Dim arr(roots.Length - 1) As ExpressionResultPair

			For i As Integer = 0 To arr.Length - 1
				arr(i) = Me.GetTailWithValidate(roots(i))
			Next

			Return arr
		End Function

#End Region

#Region "Methods - Internal"

		Friend Sub FixTemporaryHead(ByVal expression As IDynamicExpression, ByVal context As ExpressionContext, ByVal resultType As Type)
			Dim pairType As Type = GetType(GenericExpressionResultPair(Of ))
			pairType = pairType.MakeGenericType(resultType)

			Dim pair As ExpressionResultPair = Activator.CreateInstance(pairType)
			Dim headName As String = context.CalcEngineExpressionName
			pair.SetName(headName)
			pair.SetExpression(expression)

			Dim oldPair As ExpressionResultPair = MyNameNodeMap.Item(headName)
			MyDependencies.ReplaceDependency(oldPair, pair)
			MyNameNodeMap.Item(headName) = pair

			' Let the pair store the result of its expression
			pair.Recalculate()
		End Sub

		' Called by an expression when it references another expression in the engine
		Friend Sub AddDependency(ByVal tailName As String, ByVal context As ExpressionContext)
			Dim actualTail As ExpressionResultPair = Me.GetTail(tailName)
			Dim headName As String = context.CalcEngineExpressionName
			Dim actualHead As ExpressionResultPair = Me.GetTail(headName)

			' An expression could depend on the same reference more than once (ie: "a + a * a")
			MyDependencies.AddDepedency(actualTail, actualHead)
		End Sub

		Friend Function ResolveTailType(ByVal tailName As String) As Type
			Dim actualTail As ExpressionResultPair = Me.GetTail(tailName)
			Return actualTail.ResultType
		End Function

		Friend Function HasTail(ByVal tailName As String) As Boolean
			Return MyNameNodeMap.ContainsKey(tailName)
		End Function

		Friend Sub EmitLoad(ByVal tailName As String, ByVal ilg As FleeILGenerator)
			Dim pi As PropertyInfo = GetType(ExpressionContext).GetProperty("CalculationEngine")
			ilg.Emit(OpCodes.Callvirt, pi.GetGetMethod())

			' Load the tail
			Dim methods As MemberInfo() = GetType(CalculationEngine).FindMembers(MemberTypes.Method, BindingFlags.Instance Or BindingFlags.Public, Type.FilterNameIgnoreCase, "GetResult")
			Dim mi As MethodInfo = Nothing

			For Each method As MethodInfo In methods
				If method.IsGenericMethod = True Then
					mi = method
					Exit For
				End If
			Next

			Dim resultType As Type = Me.ResolveTailType(tailName)

			mi = mi.MakeGenericMethod(resultType)

			ilg.Emit(OpCodes.Ldstr, tailName)
			ilg.Emit(OpCodes.Call, mi)
		End Sub

#End Region

#Region "Methods - Public"

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="CalculationEngine.Add"]/*' />	
		Public Sub Add(ByVal atomName As String, ByVal expression As String, ByVal context As ExpressionContext)
			Utility.AssertNotNull(atomName, "atomName")
			Utility.AssertNotNull(expression, "expression")
			Utility.AssertNotNull(context, "context")

			Me.AddTemporaryHead(atomName)

			context.SetCalcEngine(Me, atomName)

			context.CompileDynamic(expression)
		End Sub

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="CalculationEngine.Remove"]/*' />	
		Public Function Remove(ByVal name As String) As Boolean
			Dim tail As ExpressionResultPair = Me.GetTail(name)

			If tail Is Nothing Then
				Return False
			End If

			Dim dependents As ExpressionResultPair() = MyDependencies.GetDependents(tail)
			MyDependencies.Remove(dependents)

			For Each pair As ExpressionResultPair In dependents
				MyNameNodeMap.Remove(pair.Name)
			Next

			Return True
		End Function

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="CalculationEngine.CreateBatchLoader"]/*' />
		Public Function CreateBatchLoader() As BatchLoader
			Dim loader As New BatchLoader()
			Return loader
		End Function

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="CalculationEngine.BatchLoad"]/*' />
		Public Sub BatchLoad(ByVal loader As BatchLoader)
			Utility.AssertNotNull(loader, "loader")
			Me.Clear()

			Dim infos As BatchLoadInfo() = loader.GetBachInfos()

			For Each info As BatchLoadInfo In infos
				Me.DoBatchLoadAdd(info)
			Next
		End Sub

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="CalculationEngine.GetResult"]/*' />	
		Public Function GetResult(Of T)(ByVal name As String) As T
			Dim tail As ExpressionResultPair = Me.GetTailWithValidate(name)

			If Not GetType(T) Is tail.ResultType Then
				Dim msg As String = String.Format("The result type of '{0}' ('{1}') does not match the supplied type argument ('{2}')", name, tail.ResultType.Name, GetType(T).Name)
				Throw New ArgumentException(msg)
			End If

			Dim actualTail As GenericExpressionResultPair(Of T) = tail
			Return actualTail.Result
		End Function

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="CalculationEngine.GetResult2"]/*' />	
		Public Function GetResult(ByVal name As String) As Object
			Dim tail As ExpressionResultPair = Me.GetTailWithValidate(name)
			Return tail.ResultAsObject
		End Function

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="CalculationEngine.GetExpression"]/*' />	
		Public Function GetExpression(ByVal name As String) As IExpression
			Dim tail As ExpressionResultPair = Me.GetTailWithValidate(name)
			Return tail.Expression
		End Function

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="CalculationEngine.GetDependents"]/*' />	
		Public Function GetDependents(ByVal name As String) As String()
			Dim pair As ExpressionResultPair = Me.GetTail(name)
			Dim dependents As New List(Of ExpressionResultPair)()

			If Not pair Is Nothing Then
				MyDependencies.GetDirectDependents(pair, dependents)
			End If

			Return Me.GetNames(dependents)
		End Function

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="CalculationEngine.GetPrecedents"]/*' />	
		Public Function GetPrecedents(ByVal name As String) As String()
			Dim pair As ExpressionResultPair = Me.GetTail(name)
			Dim dependents As New List(Of ExpressionResultPair)()

			If Not pair Is Nothing Then
				MyDependencies.GetDirectPrecedents(pair, dependents)
			End If

			Return Me.GetNames(dependents)
		End Function

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="CalculationEngine.HasDependents"]/*' />	
		Public Function HasDependents(ByVal name As String) As Boolean
			Dim pair As ExpressionResultPair = Me.GetTail(name)
			Return Not pair Is Nothing AndAlso MyDependencies.HasDependents(pair)
		End Function

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="CalculationEngine.HasPrecedents"]/*' />	
		Public Function HasPrecedents(ByVal name As String) As Boolean
			Dim pair As ExpressionResultPair = Me.GetTail(name)
			Return Not pair Is Nothing AndAlso MyDependencies.HasPrecedents(pair)
		End Function

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="CalculationEngine.Contains"]/*' />	
		Public Function Contains(ByVal name As String) As Boolean
			Utility.AssertNotNull(name, "name")
			Return MyNameNodeMap.ContainsKey(name)
		End Function

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="CalculationEngine.Recalculate"]/*' />	
		Public Sub Recalculate(ByVal ParamArray roots As String())
			' Get the tails corresponding to the names
			Dim rootTails As ExpressionResultPair() = Me.GetRootTails(roots)
			' Create a dependency list based on the tails
			Dim tempDependents As DependencyManager(Of ExpressionResultPair) = MyDependencies.CloneDependents(rootTails)
			' Get the sources (ie: nodes with no incoming edges) since that's what the sort requires
			Dim sources As Queue(Of ExpressionResultPair) = tempDependents.GetSources(rootTails)
			' Do the topological sort
			Dim calcList As IList(Of ExpressionResultPair) = tempDependents.TopologicalSort(sources)

			Dim args As New NodeEventArgs()

			' Recalculate the sorted expressions
			For Each pair As ExpressionResultPair In calcList
				pair.Recalculate()
				args.SetData(pair.Name, pair.ResultAsObject)
				RaiseEvent NodeRecalculated(Me, args)
			Next
		End Sub

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="CalculationEngine.Clear"]/*' />	
		Public Sub Clear()
			MyDependencies.Clear()
			MyNameNodeMap.Clear()
		End Sub

#End Region

#Region "Properties - Public"

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="CalculationEngine.Count"]/*' />	
		Public ReadOnly Property Count() As Integer
			Get
				Return MyDependencies.Count
			End Get
		End Property

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="CalculationEngine.DependencyGraph"]/*' />	
		Public ReadOnly Property DependencyGraph() As String
			Get
				Return MyDependencies.DependencyGraph
			End Get
		End Property

#End Region
	End Class
End Namespace