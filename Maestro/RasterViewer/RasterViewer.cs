#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.RuntimeClasses;

namespace OSGeo.MapGuide.Maestro.RasterViewer
{
	/// <summary>
	/// Summary description for RasterViewer.
	/// </summary>
	public class RasterViewer : System.Windows.Forms.UserControl
	{

		private const int SCREEN_DPI = 96;
		public enum Tools
		{
			ZoomIn,
			ZoomOut,
			Pan,
			Select
		}

		private class View
		{
			double m_minx;
			double m_miny;
			double m_maxx;
			double m_maxy;

			public View(double minx, double miny, double maxx, double maxy)
			{
				m_minx = Math.Min(minx, maxx); 
				m_miny = Math.Min(miny, maxy);
				m_maxx = Math.Max(minx, maxx);
				m_maxy = Math.Max(miny, maxy);
			}

			public View(double x, double y, double scale, double unitsPerPixel, int w, int h)
			{
				double width = ((double)w * unitsPerPixel * scale) / 2;
				double height = ((double)h * unitsPerPixel * scale) / 2;

				m_minx = x - width;
				m_maxx = x + width;
				m_miny = y - height;
				m_maxy = y + height;
			}

			public double MinX
			{
				get { return m_minx; }
				set { m_minx = value; }
			}

			public double MinY
			{
				get { return m_miny; }
				set { m_miny = value; }
			}

			public double MaxX
			{
				get { return m_maxx; }
				set { m_maxx = value; }
			}

			public double MaxY
			{
				get { return m_maxy; }
				set { m_maxy = value; }
			}

			public double Width
			{
				get
				{
					return m_maxx - m_minx;
				}
				set
				{
					double diff = (this.Width - value) / 2;
					m_maxx += diff;
					m_minx -= diff;
				}
			}

			public double Height
			{
				get
				{
					return m_maxy - m_miny;
				}
				set
				{
					double diff = (this.Height - value) / 2;
					m_maxy -= diff;
					m_miny += diff;
				}
			}

			public double X
			{
				get 
				{
					return (this.Width / 2) + m_minx;
				}
				set 
				{
					double diff = this.X - value;
					m_maxx -= diff;
					m_minx -= diff;
				}
			}

			public double Y
			{
				get 
				{
					return (this.Height / 2) + m_miny;
				}
				set 
				{
					double diff = this.Y - value;
					m_maxy -= diff;
					m_miny -= diff;
				}
			}

			public PointF Center 
			{ 
				get 
				{ 
					return new PointF((float)this.X, (float)this.Y);
				}
				set
				{
					this.X = value.X;
					this.Y = value.Y;
				}
			}

			public double GetScale(int w, int h, double unitsPerPixel)
			{
				return Math.Max(1 / ((w * unitsPerPixel) / this.Width), 1 / ((h * unitsPerPixel) / this.Height));
			}

			public PointF CalculatePosition(int x, int y, int w, int h)
			{
				return new PointF((float)(this.Width * (x/(double)w) + m_minx), (float)(this.Height * ((h-y)/(double)h) + m_miny));
			}
		}

		

		public System.Windows.Forms.ImageList imageList;
		public System.Windows.Forms.ToolBar toolBar;
		public System.Windows.Forms.PictureBox picture;
		private System.Windows.Forms.Panel panel;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ToolBarButton ZoomInButton;
		private System.Windows.Forms.ToolBarButton ZoomOutButton;
		private System.Windows.Forms.ToolBarButton UnzoomButton;
		private System.Windows.Forms.StatusBar statusBar;
		private System.ComponentModel.IContainer components;


		private ServerConnectionI m_connection;
		private View m_current;
		private View m_initial;
		private System.Windows.Forms.ToolBarButton RefreshMapButton;
		private string m_currentmap;
		private Tools m_currentTool;
		private System.Windows.Forms.ToolBarButton PanButton;
		private double m_unitsPerPixel;
		private Point m_downPoint;
	
		public RasterViewer()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.toolBar = new System.Windows.Forms.ToolBar();
			this.ZoomInButton = new System.Windows.Forms.ToolBarButton();
			this.ZoomOutButton = new System.Windows.Forms.ToolBarButton();
			this.UnzoomButton = new System.Windows.Forms.ToolBarButton();
			this.RefreshMapButton = new System.Windows.Forms.ToolBarButton();
			this.PanButton = new System.Windows.Forms.ToolBarButton();
			this.picture = new System.Windows.Forms.PictureBox();
			this.panel = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.SuspendLayout();
			// 
			// imageList
			// 
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// toolBar
			// 
			this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																					   this.ZoomInButton,
																					   this.ZoomOutButton,
																					   this.UnzoomButton,
																					   this.RefreshMapButton,
																					   this.PanButton});
			this.toolBar.DropDownArrows = true;
			this.toolBar.ImageList = this.imageList;
			this.toolBar.Location = new System.Drawing.Point(0, 0);
			this.toolBar.Name = "toolBar";
			this.toolBar.ShowToolTips = true;
			this.toolBar.Size = new System.Drawing.Size(616, 42);
			this.toolBar.TabIndex = 0;
			this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
			// 
			// ZoomInButton
			// 
			this.ZoomInButton.Text = "Zoom in";
			// 
			// ZoomOutButton
			// 
			this.ZoomOutButton.Text = "Zoom out";
			// 
			// UnzoomButton
			// 
			this.UnzoomButton.Text = "Unzoom";
			// 
			// RefreshMapButton
			// 
			this.RefreshMapButton.Text = "Refresh";
			// 
			// PanButton
			// 
			this.PanButton.Text = "Recenter";
			// 
			// picture
			// 
			this.picture.Dock = System.Windows.Forms.DockStyle.Fill;
			this.picture.Location = new System.Drawing.Point(96, 42);
			this.picture.Name = "picture";
			this.picture.Size = new System.Drawing.Size(520, 288);
			this.picture.TabIndex = 1;
			this.picture.TabStop = false;
			this.picture.Click += new System.EventHandler(this.picture_Click);
			this.picture.Resize += new System.EventHandler(this.picture_Resize);
			this.picture.VisibleChanged += new System.EventHandler(this.picture_VisibleChanged);
			this.picture.Paint += new System.Windows.Forms.PaintEventHandler(this.picture_Paint);
			this.picture.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picture_MouseUp);
			this.picture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picture_MouseMove);
			this.picture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picture_MouseDown);
			// 
			// panel
			// 
			this.panel.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel.Location = new System.Drawing.Point(0, 42);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(88, 288);
			this.panel.TabIndex = 2;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(88, 42);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(8, 288);
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 330);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(616, 22);
			this.statusBar.TabIndex = 4;
			// 
			// RasterViewer
			// 
			this.Controls.Add(this.picture);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel);
			this.Controls.Add(this.toolBar);
			this.Controls.Add(this.statusBar);
			this.Name = "RasterViewer";
			this.Size = new System.Drawing.Size(616, 352);
			this.ResumeLayout(false);

		}
		#endregion

		public void SetCurrentMap(ServerConnectionI connection, string resourceId)
		{
			m_connection = connection;
			RuntimeMap r;

			if (resourceId.EndsWith(".MapDefinition"))
			{
				m_currentmap = m_connection.GetResourceIdentifier(Guid.NewGuid().ToString(),  ResourceTypes.RuntimeMap, true);
				MapDefinition m = m_connection.GetMapDefinition(resourceId);
				r = new RuntimeMap(m);
				m_connection.CreateRuntimeMap(m_currentmap, r);
			}
			else
			{
				m_currentmap = resourceId;
				r = m_connection.GetRuntimeMap(m_currentmap);
			}

			m_initial = new View(r.Extents.MinX, r.Extents.MinY, r.Extents.MaxX, r.Extents.MaxY);
			m_current = new View(r.Extents.MinX, r.Extents.MinY, r.Extents.MaxX, r.Extents.MaxY);

			//Scaling code from server...
			m_unitsPerPixel = 0.0254 / (double)SCREEN_DPI / r.MetersPerUnit;
			/*double mapWidth2 = 0.5 * (double)width * unitsPerPixel * scale;
			double mapHeight2 = 0.5 * (double)height * unitsPerPixel * scale;*/

			this.Visible = true;
			this.Enabled = true;
			GoToScale(m_initial.X, m_initial.Y, m_initial.GetScale(picture.Width, picture.Height, m_unitsPerPixel));
		}

		public double CurrentScale
		{
			get 
			{ 
				if (m_current == null) 
					return 0;
				else 
					return m_current.GetScale(picture.Width, picture.Height, m_unitsPerPixel); 
			}
			set 
			{ 
				if (m_current != null)
					m_current = new View(m_current.X, m_current.Y, value, m_unitsPerPixel, picture.Width, picture.Height);
			}
		}

		public PointF CurrentCenter
		{
			get { return m_current.Center; }
			set { m_current.Center = value; }
		}

		public void GoToScale(double x, double y, double scale)
		{
			if (!this.Enabled || !this.Visible || m_connection == null)
				return;

			if (m_imageFetcher != null && m_imageFetcher.IsAlive)
			{
				try { m_imageFetcher.Abort(); }
				catch { }
				//Don't care if we can't cancel, but at least try
				m_imageFetcher.Join(500);
			}

			m_current = new View(x, y, scale, m_unitsPerPixel, picture.Width, picture.Height);

			m_imageFetcherParams = new FetchImageParameters();
			m_imageFetcherParams.x = m_current.X;
			m_imageFetcherParams.y = m_current.Y;
			m_imageFetcherParams.scale = m_current.GetScale(picture.Width, picture.Height, m_unitsPerPixel);
			m_imageFetcherParams.w = picture.Width;
			m_imageFetcherParams.h = picture.Height;
			m_imageFetcherParams.resid = m_currentmap;
			m_imageFetcherParams.evt = new System.Threading.ManualResetEvent(false);

			m_imageFetcher = new System.Threading.Thread(new System.Threading.ThreadStart(FetchImage));
			m_imageFetcher.IsBackground = true;
			m_imageFetcher.Start();
			
			m_imageFetcherParams.evt.WaitOne();
			m_imageFetcherParams = null;
		}

		System.Threading.Thread m_imageFetcher;
		FetchImageParameters m_imageFetcherParams;

		private class FetchImageParameters
		{
			public double x;
			public double y;
			public double scale;
			public string resid;
			public int w;
			public int h;
			public System.Threading.ManualResetEvent evt;
		}

		private void FetchImage()
		{
			//Copy pointer and release
			FetchImageParameters p = m_imageFetcherParams;
			p.evt.Set();

			Bitmap bmp = new Bitmap(m_connection.RenderRuntimeMap(p.resid, p.x, p.y, p.scale, p.w, p.h, SCREEN_DPI));
			this.Invoke(new SetImageDelegate(SetImage), new object[] { bmp } );
		}

		private delegate void SetImageDelegate(Bitmap bmp);
		private void SetImage(Bitmap bmp)
		{
			picture.Image = bmp;
		}

		private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == RefreshMapButton)
				GoToScale(m_current.X, m_current.Y, m_current.GetScale(picture.Width, picture.Height, m_unitsPerPixel));
			else if (e.Button == UnzoomButton)
				GoToScale(m_initial.X, m_initial.Y, m_initial.GetScale(picture.Width, picture.Height, m_unitsPerPixel));
			else if (e.Button == ZoomInButton)
				SetTool(Tools.ZoomIn);
			else if (e.Button == ZoomOutButton)
				SetTool(Tools.ZoomOut);
			else if (e.Button == PanButton)
				SetTool(Tools.Pan);
			/*else if (e.Button == SelectButton)
				SetTool(Tools.Select);*/
		}

		private void SetTool(Tools tool)
		{
			m_currentTool = tool;

		}

		private void picture_Click(object sender, System.EventArgs e)
		{
			Point p = picture.PointToClient(Cursor.Position);
			PointF m = m_current.CalculatePosition(p.X, p.Y, picture.Width, picture.Height);
			double mapCoordX = m.X; //Cursor.Position.X * (picture.Width);
			double mapCoordY = m.Y; //Cursor.Position.Y * (picture.Height);
			double scale = m_current.GetScale(picture.Width, picture.Height, m_unitsPerPixel);
			

			switch (m_currentTool)
			{
				case Tools.ZoomIn:
					GoToScale(mapCoordX, mapCoordY, scale / 1.5);
					break;
				case Tools.ZoomOut:
					GoToScale(mapCoordX, mapCoordY, scale * 1.5);
					break;
				case Tools.Pan:
					GoToScale(mapCoordX, mapCoordY, scale);
					break;
				case Tools.Select:
					//TODO: Select
					break;
			}
		}

		private void picture_Resize(object sender, System.EventArgs e)
		{
			if (picture.Visible && picture.Enabled && m_current != null)
				GoToScale(m_current.X, m_current.Y, m_current.GetScale(picture.Width, picture.Height, m_unitsPerPixel));
		}

		private void picture_VisibleChanged(object sender, System.EventArgs e)
		{
			if (picture.Visible && picture.Enabled && m_current != null)
				GoToScale(m_current.X, m_current.Y, m_current.GetScale(picture.Width, picture.Height, m_unitsPerPixel));
		}

		private void picture_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Point p = picture.PointToClient(Cursor.Position);
			PointF m = m_current.CalculatePosition(p.X, p.Y, picture.Width, picture.Height);
			statusBar.Text = "(" + m.X.ToString("0.0000") + ", " + m.Y.ToString("0.0000") + ")\t 1:" + m_current.GetScale(picture.Width, picture.Height, m_unitsPerPixel).ToString("0.0") + "\t (" + m_current.MinX.ToString("0.00") + ", " + m_current.MinY.ToString("0.00") + ") - (" + m_current.MaxX.ToString("0.00") + ", " + m_current.MaxY.ToString("0.00") + ")";

			if (e.Button == MouseButtons.Left)
			{
				if (m_currentTool == Tools.ZoomIn || m_currentTool == Tools.Select)
				{

				}
			}
		}

		private void picture_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			//Code for visually displaying the center, used for debugging
			/*using(Pen p = new Pen(Color.Black))
			{
				e.Graphics.DrawLine(p, 0, picture.Height / 2, picture.Width, picture.Height / 2);
				e.Graphics.DrawLine(p, picture.Width / 2, 0, picture.Width / 2, picture.Height);
				e.Graphics.DrawRectangle(p, 0, 0, picture.Width - 1, picture.Height - 1);
			}*/

		}

		private void picture_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				m_downPoint = picture.PointToClient(Cursor.Position);
		}

		private void picture_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Point p = picture.PointToClient(Cursor.Position);
			PointF m = m_current.CalculatePosition(p.X, p.Y, picture.Width, picture.Height);
			double scale = m_current.GetScale(picture.Width, picture.Height, m_unitsPerPixel);
			bool boxType = Math.Abs(p.X - m_downPoint.X) > 2 || Math.Abs(p.Y - m_downPoint.Y) > 2;
			PointF m2 = m_current.CalculatePosition(m_downPoint.X, m_downPoint.Y, picture.Width, picture.Height);

			switch (m_currentTool)
			{
				case Tools.ZoomIn:
					/*if (boxType)
					{
						View v = new View(m.X, m.Y, m2.X, m2.Y);
						GoToScale(v.X, v.Y, v.GetScale(picture.Width, picture.Height, m_unitsPerPixel));
					}
					else*/
						GoToScale(m.X, m.Y, scale / 1.5);
					break;
				case Tools.ZoomOut:
					GoToScale(m.X, m.Y, scale * 1.5);
					break;
				case Tools.Pan:
					GoToScale(m.X, m.Y, scale);
					break;
				case Tools.Select:
					//TODO: Select
					break;
			}

		}
	}
}
