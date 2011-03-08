// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="John Simons" email="johnsimons007@yahoo.com.au"/>
//     <version>$Revision: 1965 $</version>
// </file>

using System;

namespace ICSharpCode.Core
{
	public interface ITextBoxCommand : ICommand
	{
		bool IsEnabled {
			get;
			set;
		}
	}
}
