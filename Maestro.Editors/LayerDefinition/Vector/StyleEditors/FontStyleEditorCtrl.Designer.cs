namespace Maestro.Editors.LayerDefinition.Vector.StyleEditors
{
    partial class FontStyleEditorCtrl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label11 = new System.Windows.Forms.Label();
            chkAdvancedPlacement = new System.Windows.Forms.CheckBox();
            grpAdvancedPlacement = new System.Windows.Forms.GroupBox();
            numScaleLimit = new System.Windows.Forms.NumericUpDown();
            previewPicture = new System.Windows.Forms.PictureBox();
            previewGroup = new System.Windows.Forms.GroupBox();
            rotationCombo = new System.Windows.Forms.ComboBox();
            verticalCombo = new System.Windows.Forms.ComboBox();
            horizontalCombo = new System.Windows.Forms.ComboBox();
            label10 = new System.Windows.Forms.Label();
            verticalLabel = new System.Windows.Forms.Label();
            horizontalLabel = new System.Windows.Forms.Label();
            alignmentGroup = new System.Windows.Forms.GroupBox();
            label12 = new System.Windows.Forms.Label();
            cmbLabelJustification = new System.Windows.Forms.ComboBox();
            colorGroup = new System.Windows.Forms.GroupBox();
            backgroundColor = new ColorExpressionField();
            textColor = new ColorExpressionField();
            backgroundTypeCombo = new System.Windows.Forms.ComboBox();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            boldCheck = new System.Windows.Forms.CheckBox();
            DisplayLabel = new System.Windows.Forms.CheckBox();
            fontGroup = new System.Windows.Forms.GroupBox();
            sizeCombo = new System.Windows.Forms.ComboBox();
            unitsCombo = new System.Windows.Forms.ComboBox();
            sizeContextCombo = new System.Windows.Forms.ComboBox();
            fontCombo = new System.Windows.Forms.ComboBox();
            propertyCombo = new System.Windows.Forms.ComboBox();
            label6 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            underlineCheck = new System.Windows.Forms.CheckBox();
            italicCheck = new System.Windows.Forms.CheckBox();
            grpAdvancedPlacement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numScaleLimit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)previewPicture).BeginInit();
            previewGroup.SuspendLayout();
            alignmentGroup.SuspendLayout();
            colorGroup.SuspendLayout();
            fontGroup.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label11.Location = new System.Drawing.Point(5, 44);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(64, 15);
            label11.TabIndex = 2;
            label11.Text = "Scale Limit";
            // 
            // chkAdvancedPlacement
            // 
            chkAdvancedPlacement.AutoSize = true;
            chkAdvancedPlacement.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            chkAdvancedPlacement.Location = new System.Drawing.Point(13, 19);
            chkAdvancedPlacement.Name = "chkAdvancedPlacement";
            chkAdvancedPlacement.Size = new System.Drawing.Size(176, 19);
            chkAdvancedPlacement.TabIndex = 0;
            chkAdvancedPlacement.Text = "Enable Advanced Placement";
            chkAdvancedPlacement.UseVisualStyleBackColor = true;
            chkAdvancedPlacement.CheckedChanged += chkAdvancedPlacement_CheckedChanged;
            // 
            // grpAdvancedPlacement
            // 
            grpAdvancedPlacement.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            grpAdvancedPlacement.Controls.Add(label11);
            grpAdvancedPlacement.Controls.Add(numScaleLimit);
            grpAdvancedPlacement.Controls.Add(chkAdvancedPlacement);
            grpAdvancedPlacement.Location = new System.Drawing.Point(3, 559);
            grpAdvancedPlacement.Name = "grpAdvancedPlacement";
            grpAdvancedPlacement.Size = new System.Drawing.Size(376, 73);
            grpAdvancedPlacement.TabIndex = 24;
            grpAdvancedPlacement.TabStop = false;
            grpAdvancedPlacement.Text = "Advanced Placement";
            // 
            // numScaleLimit
            // 
            numScaleLimit.DecimalPlaces = 2;
            numScaleLimit.Location = new System.Drawing.Point(126, 42);
            numScaleLimit.Maximum = new decimal(new int[] { 10, 0, 0, 65536 });
            numScaleLimit.Name = "numScaleLimit";
            numScaleLimit.Size = new System.Drawing.Size(153, 23);
            numScaleLimit.TabIndex = 1;
            numScaleLimit.ValueChanged += numScaleLimit_ValueChanged;
            // 
            // previewPicture
            // 
            previewPicture.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            previewPicture.BackColor = System.Drawing.Color.White;
            previewPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            previewPicture.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            previewPicture.Location = new System.Drawing.Point(8, 16);
            previewPicture.Name = "previewPicture";
            previewPicture.Size = new System.Drawing.Size(365, 24);
            previewPicture.TabIndex = 0;
            previewPicture.TabStop = false;
            previewPicture.Paint += previewPicture_Paint;
            // 
            // previewGroup
            // 
            previewGroup.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            previewGroup.Controls.Add(previewPicture);
            previewGroup.Location = new System.Drawing.Point(3, 638);
            previewGroup.Name = "previewGroup";
            previewGroup.Size = new System.Drawing.Size(379, 48);
            previewGroup.TabIndex = 22;
            previewGroup.TabStop = false;
            previewGroup.Text = "Preview";
            // 
            // rotationCombo
            // 
            rotationCombo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            rotationCombo.DisplayMember = "Display";
            rotationCombo.Location = new System.Drawing.Point(129, 78);
            rotationCombo.Name = "rotationCombo";
            rotationCombo.Size = new System.Drawing.Size(244, 23);
            rotationCombo.TabIndex = 11;
            rotationCombo.ValueMember = "Value";
            rotationCombo.SelectedIndexChanged += rotationCombo_SelectedIndexChanged;
            rotationCombo.TextChanged += rotationCombo_TextChanged;
            // 
            // verticalCombo
            // 
            verticalCombo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            verticalCombo.DisplayMember = "Display";
            verticalCombo.Location = new System.Drawing.Point(129, 46);
            verticalCombo.Name = "verticalCombo";
            verticalCombo.Size = new System.Drawing.Size(244, 23);
            verticalCombo.TabIndex = 10;
            verticalCombo.ValueMember = "Value";
            verticalCombo.SelectedIndexChanged += verticalCombo_SelectedIndexChanged;
            verticalCombo.TextChanged += verticalCombo_TextChanged;
            // 
            // horizontalCombo
            // 
            horizontalCombo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            horizontalCombo.DisplayMember = "Display";
            horizontalCombo.Location = new System.Drawing.Point(129, 14);
            horizontalCombo.Name = "horizontalCombo";
            horizontalCombo.Size = new System.Drawing.Size(244, 23);
            horizontalCombo.TabIndex = 9;
            horizontalCombo.ValueMember = "Value";
            horizontalCombo.SelectedIndexChanged += horizontalCombo_SelectedIndexChanged;
            horizontalCombo.TextChanged += horizontalCombo_TextChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label10.Location = new System.Drawing.Point(8, 80);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(52, 15);
            label10.TabIndex = 5;
            label10.Text = "Rotation";
            // 
            // verticalLabel
            // 
            verticalLabel.AutoSize = true;
            verticalLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            verticalLabel.Location = new System.Drawing.Point(8, 48);
            verticalLabel.Name = "verticalLabel";
            verticalLabel.Size = new System.Drawing.Size(45, 15);
            verticalLabel.TabIndex = 4;
            verticalLabel.Text = "Vertical";
            // 
            // horizontalLabel
            // 
            horizontalLabel.AutoSize = true;
            horizontalLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            horizontalLabel.Location = new System.Drawing.Point(8, 16);
            horizontalLabel.Name = "horizontalLabel";
            horizontalLabel.Size = new System.Drawing.Size(62, 15);
            horizontalLabel.TabIndex = 3;
            horizontalLabel.Text = "Horizontal";
            // 
            // alignmentGroup
            // 
            alignmentGroup.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            alignmentGroup.Controls.Add(label12);
            alignmentGroup.Controls.Add(cmbLabelJustification);
            alignmentGroup.Controls.Add(rotationCombo);
            alignmentGroup.Controls.Add(verticalCombo);
            alignmentGroup.Controls.Add(horizontalCombo);
            alignmentGroup.Controls.Add(label10);
            alignmentGroup.Controls.Add(verticalLabel);
            alignmentGroup.Controls.Add(horizontalLabel);
            alignmentGroup.Location = new System.Drawing.Point(3, 411);
            alignmentGroup.Name = "alignmentGroup";
            alignmentGroup.Size = new System.Drawing.Size(379, 142);
            alignmentGroup.TabIndex = 21;
            alignmentGroup.TabStop = false;
            alignmentGroup.Text = "Alignment";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label12.Location = new System.Drawing.Point(8, 110);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(101, 15);
            label12.TabIndex = 13;
            label12.Text = "Label Justification";
            // 
            // cmbLabelJustification
            // 
            cmbLabelJustification.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbLabelJustification.DisplayMember = "Display";
            cmbLabelJustification.Location = new System.Drawing.Point(129, 107);
            cmbLabelJustification.Name = "cmbLabelJustification";
            cmbLabelJustification.Size = new System.Drawing.Size(244, 23);
            cmbLabelJustification.TabIndex = 12;
            cmbLabelJustification.ValueMember = "Value";
            cmbLabelJustification.SelectedIndexChanged += cmbLabelJustification_SelectedIndexChanged;
            cmbLabelJustification.TextChanged += cmbLabelJustification_TextChanged;
            // 
            // colorGroup
            // 
            colorGroup.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            colorGroup.Controls.Add(backgroundColor);
            colorGroup.Controls.Add(textColor);
            colorGroup.Controls.Add(backgroundTypeCombo);
            colorGroup.Controls.Add(label7);
            colorGroup.Controls.Add(label8);
            colorGroup.Controls.Add(label9);
            colorGroup.Location = new System.Drawing.Point(3, 243);
            colorGroup.Name = "colorGroup";
            colorGroup.Size = new System.Drawing.Size(379, 160);
            colorGroup.TabIndex = 20;
            colorGroup.TabStop = false;
            colorGroup.Text = "Colors";
            // 
            // backgroundColor
            // 
            backgroundColor.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            backgroundColor.ColorExpression = "";
            backgroundColor.Location = new System.Drawing.Point(129, 72);
            backgroundColor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            backgroundColor.Name = "backgroundColor";
            backgroundColor.Size = new System.Drawing.Size(244, 49);
            backgroundColor.TabIndex = 13;
            backgroundColor.CurrentColorChanged += backgroundColor_SelectedIndexChanged;
            backgroundColor.RequestExpressionEditor += BackgroundColor_RequestExpressionEditor;
            // 
            // textColor
            // 
            textColor.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textColor.ColorExpression = "";
            textColor.Location = new System.Drawing.Point(129, 16);
            textColor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textColor.Name = "textColor";
            textColor.Size = new System.Drawing.Size(244, 49);
            textColor.TabIndex = 12;
            textColor.CurrentColorChanged += textColor_SelectedIndexChanged;
            textColor.RequestExpressionEditor += TextColor_RequestExpressionEditor;
            // 
            // backgroundTypeCombo
            // 
            backgroundTypeCombo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            backgroundTypeCombo.DisplayMember = "Display";
            backgroundTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            backgroundTypeCombo.Location = new System.Drawing.Point(129, 128);
            backgroundTypeCombo.Name = "backgroundTypeCombo";
            backgroundTypeCombo.Size = new System.Drawing.Size(244, 23);
            backgroundTypeCombo.TabIndex = 11;
            backgroundTypeCombo.ValueMember = "Value";
            backgroundTypeCombo.SelectedIndexChanged += backgroundTypeCombo_SelectedIndexChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label7.Location = new System.Drawing.Point(8, 130);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(97, 15);
            label7.TabIndex = 5;
            label7.Text = "Background type";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label8.Location = new System.Drawing.Point(8, 72);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(71, 15);
            label8.TabIndex = 4;
            label8.Text = "Background";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label9.Location = new System.Drawing.Point(8, 16);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(28, 15);
            label9.TabIndex = 3;
            label9.Text = "Text";
            // 
            // boldCheck
            // 
            boldCheck.Appearance = System.Windows.Forms.Appearance.Button;
            boldCheck.FlatStyle = System.Windows.Forms.FlatStyle.System;
            boldCheck.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            boldCheck.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            boldCheck.Location = new System.Drawing.Point(0, 0);
            boldCheck.Name = "boldCheck";
            boldCheck.Size = new System.Drawing.Size(24, 24);
            boldCheck.TabIndex = 3;
            boldCheck.Text = "B";
            boldCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            boldCheck.CheckedChanged += boldCheck_CheckedChanged;
            // 
            // DisplayLabel
            // 
            DisplayLabel.AutoSize = true;
            DisplayLabel.Checked = true;
            DisplayLabel.CheckState = System.Windows.Forms.CheckState.Checked;
            DisplayLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            DisplayLabel.Location = new System.Drawing.Point(3, 3);
            DisplayLabel.Name = "DisplayLabel";
            DisplayLabel.Size = new System.Drawing.Size(137, 19);
            DisplayLabel.TabIndex = 23;
            DisplayLabel.Text = "Display feature labels";
            DisplayLabel.UseVisualStyleBackColor = true;
            DisplayLabel.CheckedChanged += DisplayLabel_CheckedChanged;
            // 
            // fontGroup
            // 
            fontGroup.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            fontGroup.Controls.Add(sizeCombo);
            fontGroup.Controls.Add(unitsCombo);
            fontGroup.Controls.Add(sizeContextCombo);
            fontGroup.Controls.Add(fontCombo);
            fontGroup.Controls.Add(propertyCombo);
            fontGroup.Controls.Add(label6);
            fontGroup.Controls.Add(label5);
            fontGroup.Controls.Add(label4);
            fontGroup.Controls.Add(label3);
            fontGroup.Controls.Add(label2);
            fontGroup.Controls.Add(label1);
            fontGroup.Controls.Add(panel1);
            fontGroup.Location = new System.Drawing.Point(3, 27);
            fontGroup.Name = "fontGroup";
            fontGroup.Size = new System.Drawing.Size(379, 208);
            fontGroup.TabIndex = 19;
            fontGroup.TabStop = false;
            fontGroup.Text = "Font";
            // 
            // sizeCombo
            // 
            sizeCombo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            sizeCombo.Items.AddRange(new object[] { "Expression..." });
            sizeCombo.Location = new System.Drawing.Point(129, 144);
            sizeCombo.Name = "sizeCombo";
            sizeCombo.Size = new System.Drawing.Size(244, 23);
            sizeCombo.TabIndex = 10;
            sizeCombo.SelectedIndexChanged += sizeCombo_SelectedIndexChanged;
            sizeCombo.TextChanged += sizeCombo_TextChanged;
            // 
            // unitsCombo
            // 
            unitsCombo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            unitsCombo.DisplayMember = "Display";
            unitsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            unitsCombo.Location = new System.Drawing.Point(129, 112);
            unitsCombo.Name = "unitsCombo";
            unitsCombo.Size = new System.Drawing.Size(244, 23);
            unitsCombo.TabIndex = 9;
            unitsCombo.ValueMember = "Value";
            unitsCombo.SelectedIndexChanged += unitsCombo_SelectedIndexChanged;
            // 
            // sizeContextCombo
            // 
            sizeContextCombo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            sizeContextCombo.DisplayMember = "Display";
            sizeContextCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            sizeContextCombo.Location = new System.Drawing.Point(129, 80);
            sizeContextCombo.Name = "sizeContextCombo";
            sizeContextCombo.Size = new System.Drawing.Size(244, 23);
            sizeContextCombo.TabIndex = 8;
            sizeContextCombo.ValueMember = "Value";
            sizeContextCombo.SelectedIndexChanged += sizeContextCombo_SelectedIndexChanged;
            // 
            // fontCombo
            // 
            fontCombo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            fontCombo.Location = new System.Drawing.Point(129, 48);
            fontCombo.Name = "fontCombo";
            fontCombo.Size = new System.Drawing.Size(244, 23);
            fontCombo.TabIndex = 7;
            fontCombo.SelectedIndexChanged += fontCombo_SelectedIndexChanged;
            fontCombo.TextChanged += fontCombo_TextChanged;
            // 
            // propertyCombo
            // 
            propertyCombo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            propertyCombo.Location = new System.Drawing.Point(129, 16);
            propertyCombo.Name = "propertyCombo";
            propertyCombo.Size = new System.Drawing.Size(244, 23);
            propertyCombo.TabIndex = 6;
            propertyCombo.SelectedIndexChanged += propertyCombo_SelectedIndexChanged;
            propertyCombo.TextChanged += propertyCombo_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label6.Location = new System.Drawing.Point(8, 176);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(32, 15);
            label6.TabIndex = 5;
            label6.Text = "Style";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label5.Location = new System.Drawing.Point(8, 144);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(27, 15);
            label5.TabIndex = 4;
            label5.Text = "Size";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label4.Location = new System.Drawing.Point(8, 112);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(34, 15);
            label4.TabIndex = 3;
            label4.Text = "Units";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label3.Location = new System.Drawing.Point(8, 80);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(70, 15);
            label3.TabIndex = 2;
            label3.Text = "Size context";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label2.Location = new System.Drawing.Point(8, 48);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(31, 15);
            label2.TabIndex = 1;
            label2.Text = "Font";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label1.Location = new System.Drawing.Point(8, 16);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(52, 15);
            label1.TabIndex = 0;
            label1.Text = "Property";
            // 
            // panel1
            // 
            panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel1.Controls.Add(underlineCheck);
            panel1.Controls.Add(italicCheck);
            panel1.Controls.Add(boldCheck);
            panel1.Location = new System.Drawing.Point(129, 176);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(244, 24);
            panel1.TabIndex = 11;
            // 
            // underlineCheck
            // 
            underlineCheck.Appearance = System.Windows.Forms.Appearance.Button;
            underlineCheck.FlatStyle = System.Windows.Forms.FlatStyle.System;
            underlineCheck.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            underlineCheck.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            underlineCheck.Location = new System.Drawing.Point(64, 0);
            underlineCheck.Name = "underlineCheck";
            underlineCheck.Size = new System.Drawing.Size(24, 24);
            underlineCheck.TabIndex = 5;
            underlineCheck.Text = "U";
            underlineCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            underlineCheck.CheckedChanged += underlineCheck_CheckedChanged;
            // 
            // italicCheck
            // 
            italicCheck.Appearance = System.Windows.Forms.Appearance.Button;
            italicCheck.FlatStyle = System.Windows.Forms.FlatStyle.System;
            italicCheck.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            italicCheck.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            italicCheck.Location = new System.Drawing.Point(32, 0);
            italicCheck.Name = "italicCheck";
            italicCheck.Size = new System.Drawing.Size(24, 24);
            italicCheck.TabIndex = 4;
            italicCheck.Text = "I";
            italicCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            italicCheck.CheckedChanged += italicCheck_CheckedChanged;
            // 
            // FontStyleEditorCtrl
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            AutoScroll = true;
            AutoScrollMinSize = new System.Drawing.Size(385, 694);
            Controls.Add(grpAdvancedPlacement);
            Controls.Add(previewGroup);
            Controls.Add(alignmentGroup);
            Controls.Add(colorGroup);
            Controls.Add(DisplayLabel);
            Controls.Add(fontGroup);
            Name = "FontStyleEditorCtrl";
            Size = new System.Drawing.Size(385, 694);
            grpAdvancedPlacement.ResumeLayout(false);
            grpAdvancedPlacement.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numScaleLimit).EndInit();
            ((System.ComponentModel.ISupportInitialize)previewPicture).EndInit();
            previewGroup.ResumeLayout(false);
            alignmentGroup.ResumeLayout(false);
            alignmentGroup.PerformLayout();
            colorGroup.ResumeLayout(false);
            colorGroup.PerformLayout();
            fontGroup.ResumeLayout(false);
            fontGroup.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkAdvancedPlacement;
        private System.Windows.Forms.GroupBox grpAdvancedPlacement;
        private System.Windows.Forms.NumericUpDown numScaleLimit;
        private System.Windows.Forms.PictureBox previewPicture;
        private System.Windows.Forms.GroupBox previewGroup;
        private System.Windows.Forms.ComboBox rotationCombo;
        internal System.Windows.Forms.ComboBox verticalCombo;
        internal System.Windows.Forms.ComboBox horizontalCombo;
        private System.Windows.Forms.Label label10;
        internal System.Windows.Forms.Label verticalLabel;
        internal System.Windows.Forms.Label horizontalLabel;
        private System.Windows.Forms.GroupBox alignmentGroup;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cmbLabelJustification;
        private System.Windows.Forms.GroupBox colorGroup;
        private ColorExpressionField backgroundColor;
        private ColorExpressionField textColor;
        private System.Windows.Forms.ComboBox backgroundTypeCombo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox boldCheck;
        private System.Windows.Forms.CheckBox DisplayLabel;
        private System.Windows.Forms.GroupBox fontGroup;
        private System.Windows.Forms.ComboBox sizeCombo;
        private System.Windows.Forms.ComboBox unitsCombo;
        private System.Windows.Forms.ComboBox sizeContextCombo;
        private System.Windows.Forms.ComboBox fontCombo;
        private System.Windows.Forms.ComboBox propertyCombo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox underlineCheck;
        private System.Windows.Forms.CheckBox italicCheck;
    }
}
