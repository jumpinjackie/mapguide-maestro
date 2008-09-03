#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
// 
#endregion
using System;
using OSGeo.MapGuide.MaestroAPI.ApplicationDefinition;

namespace OSGeo.MapGuide.Maestro.FusionEditor
{
	/// <summary>
	/// Summary description for BasisWidgetEditor.
	/// </summary>
	public class BasisWidgetEditor : System.Windows.Forms.UserControl
	{
		protected WidgetType m_w;
		protected bool m_isUpdating = false;
		public event ValueChangedDelegate ValueChanged;
		protected WidgetTypeCollection m_defaultWidgets;
		public WidgetTypeCollection DefaultWidgets
		{
			get { return m_defaultWidgets; }
			set { m_defaultWidgets = value; }
		}

		public virtual void SetItem(WidgetType w)
		{
			throw new MissingMethodException(w == null ? "<null>" : w.Type);
		}

		protected void RaiseValueChanged()
		{
			if (ValueChanged != null)
				ValueChanged(this, m_w);
		}

		protected string GetSettingValue(string name)
		{
			if (m_w == null || m_w.Extension == null)
				return "";

			string v = m_w.Extension[name];
			if (v == null)
				if (m_defaultWidgets != null)
					foreach(WidgetType w in m_defaultWidgets)
						if (w.Type == m_w.Type)
						{
							if (w.Extension != null)
								v = w.Extension[name];
							break;
						}

			return v == null ? "" : v;
		}

		protected void SetSettingValue(string name, string value)
		{
			if (m_w == null)
				return;
			if (m_w.Extension == null)
				m_w.Extension = new CustomContentType();
			m_w.Extension[name] = value;
		}
	}
}
