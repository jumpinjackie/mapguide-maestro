//---------------------------------------------------------------------------------
//IronTextBox.cs - version 2.0.2.0b
// TextBox control based class designed to be used with Microsoft's IronPython.
// Maybe useful for testing Python scripts with IronPython. 
//WHAT'S NEW: 
//      -Updated License from GNU to Expat/MIT
//      -Tested with IronPython 2.03B
//TO DO:
//      -Fix raw_input support: "s = raw_input('--> ')"
//      -Multiple arg support for "paths" command. eg. "paths -misc -python24"
//      -Intellisense ToolTip
//
//BY DOWNLOADING AND USING, YOU AGREE TO THE FOLLOWING TERMS:
//Copyright (c) 2006-2008 by Joseph P. Socoloski III
//LICENSE
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.
//the MIT License, given here: <http://www.opensource.org/licenses/mit-license.php> 
//---------------------------------------------------------------------------------

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;//ToolboxItem
using System.Drawing;       //ToolboxBitmap
using IronPython.Runtime;   //PythonDictionary
using IronPython.Hosting;   //PythonEngine
using Microsoft.Scripting;  //ScriptDomainManager
using Microsoft.Scripting.Hosting;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.AddIn.Scripting.UI
{
    #region IronTextBox Class
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(IronTextBox))]
    [DesignerAttribute(typeof(IronTextBoxControl))]
    internal class IronTextBox : TextBox
    {
        #region IronTextBox members
        /// <summary>
        /// Default prompt text.
        /// </summary>
        private string prompt = ">>>"; //NOXLATE

        /// <summary>
        /// Used for storing commands.
        /// </summary>
        private CommandHistory commandHistory = new CommandHistory();

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// Holds temporary defFunc lines.
        /// </summary>
        public System.Text.StringBuilder defStmtBuilder = new System.Text.StringBuilder();

        /// <summary>
        /// StringCollection of all MiscDirs
        /// </summary>
        public static StringCollection scMisc = new StringCollection();

        /// <summary>
        /// StringCollection of all Python24Dirs
        /// </summary>
        public static StringCollection scPython24 = new StringCollection();

        /// <summary>
        /// StringCollection of all IronPythonDirs
        /// </summary>
        public static StringCollection scIronPython = new StringCollection();

        /// <summary>
        /// Intellisense ToolTip.
        /// </summary>
        System.Windows.Forms.ToolTip intellisense = new System.Windows.Forms.ToolTip();

        /// <summary>
        /// True if currently processing raw_text()
        /// </summary>
        public static Boolean IsRawInput = false;

        /// <summary>
        /// Hold raw_input prompt by user
        /// </summary>
        public string rawprompt = "";

        #endregion IronTextBox members

        internal IronTextBox()
        {
            InitializeComponent();
            printPrompt();

            // Set up the delays for the ToolTip.
            intellisense.AutoPopDelay = 1000;
            intellisense.InitialDelay = 100;
            intellisense.ReshowDelay = 100;
            // Force the ToolTip text to be displayed whether or not the form is active.
            intellisense.ShowAlways = true;

        }

        #region Overrides
        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Overridden to protect against deletion of contents
        /// cutting the text and deleting it from the context menu
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0302: //WM_PASTE
                case 0x0300: //WM_CUT
                case 0x000C: //WM_SETTEXT
                    if (!IsCaretAtWritablePosition())
                        MoveCaretToEndOfText();
                    break;
                case 0x0303: //WM_CLEAR
                    return;
            }
            base.WndProc(ref m);
        }
        #endregion Overrides

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // consoleTextBox
            // 
            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;
            this.Location = new Point(0, 0);
            this.MaxLength = 0;
            this.Multiline = true;
            this.Name = "consoleTextBox";
            this.AcceptsTab = true;
            this.AcceptsReturn = true;    //for TextBox use
            this.ScrollBars = System.Windows.Forms.ScrollBars.Both;   //for TextBox use
            //this.ScrollBars = RichTextBoxScrollBars.Both;   //for RichTextBox use
            this.Size = new Size(400, 176);
            this.TabIndex = 0;
            this.Text = "";
            this.KeyPress += new KeyPressEventHandler(this.consoleTextBox_KeyPress);
            this.KeyDown += new KeyEventHandler(ConsoleControl_KeyDown);
            // 
            // IronTextBoxControl
            // 
            this.Name = "IronTextBox";
            this.Size = new Size(400, 176);
            this.ResumeLayout(false);

        }
        #endregion

        #region IronTextBox Base Methods
        /// <summary>
        /// Sends the prompt to the IronTextBox
        /// </summary>
        public void printPrompt()
        {
            string currentText = this.Text;

            //add newline if it does not exist
            if ((currentText.Length != 0) && (currentText[currentText.Length - 1] != '\n'))
                printLine();

            //add the prompt
            this.AddText(prompt);
        }

        /// <summary>
        /// Sends a newline character to the IronTextBox
        /// </summary>
        public void printLine()
        {
            this.AddText(System.Environment.NewLine);
        }

        /// <summary>
        /// Returns currentline's text string
        /// </summary>
        /// <returns>Returns currentline's text string</returns>
        public string GetTextAtPrompt()
        {
            if (GetCurrentLine() != "")
                return GetCurrentLine().Substring(prompt.Length);
            else
            {
                string mystring = (string)this.Lines.GetValue(this.Lines.Length - 2);
                return mystring.Substring(prompt.Length);
            }
        }

        /// <summary>
        /// Add a command to IronTextBox command history.
        /// </summary>
        /// <param name="currentCommand">IronTextBox command line</param>
        public void AddcommandHistory(string currentCommand)
        {
            commandHistory.Add(currentCommand);
        }

        /// <summary>
        /// Returns true if Keys.Enter
        /// </summary>
        /// <param name="key">Keys</param>
        /// <returns>Returns true if Keys.Enter</returns>
        private bool IsTerminatorKey(System.Windows.Forms.Keys key)
        {
            return key == Keys.Enter;
        }

        /// <summary>
        /// Returns true if (char)13 '\r'
        /// </summary>
        /// <param name="keyChar">char of keypressed</param>
        /// <returns>Returns true if (char)13 '\r'</returns>
        private bool IsTerminatorKey(char keyChar)
        {
            return ((int)keyChar) == 13;
        }

        /// <summary>
        /// Returns the current line, including prompt.
        /// </summary>
        /// <returns>Returns the current line, including prompt.</returns>
        private string GetCurrentLine()
        {
            if (this.Lines.Length > 0)
            {
                return (string)this.Lines.GetValue(this.Lines.GetLength(0) - 1);
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// Replaces the text at the current prompt.
        /// </summary>
        /// <param name="text">new text to replace old text.</param>
        private void ReplaceTextAtPrompt(string text)
        {
            string currentLine = GetCurrentLine();
            int charactersAfterPrompt = currentLine.Length - prompt.Length;

            if (charactersAfterPrompt == 0)
                this.AddText(text);
            else
            {
                this.Select(this.TextLength - charactersAfterPrompt, charactersAfterPrompt);
                this.SelectedText = text;
            }
        }

        /// <summary>
        /// Returns true if caret is positioned on the currentline.
        /// </summary>
        /// <returns>Returns true if caret is positioned on the currentline.</returns>
        private bool IsCaretAtCurrentLine()
        {
            return this.TextLength - this.SelectionStart <= GetCurrentLine().Length;
        }

        /// <summary>
        /// Adds text to the IronTextBox
        /// </summary>
        /// <param name="text">text to be added</param>
        private void AddText(string text)
        {
            //Optional////////////
            scollection.Add(text);  //Optional
            //this.Text = StringCollecttostring(scollection); //Optional
            //////////////////////

            this.Enabled = false;
            this.Text += text;
            MoveCaretToEndOfText();
            this.Enabled = true;
            this.Focus();
            this.Update();
        }

        /// <summary>
        /// Returns a string retrieved from a StringCollection.
        /// </summary>
        /// <param name="inCol">StringCollection to be searched.</param>
        public string StringCollecttostring(System.Collections.Specialized.StringCollection inCol)
        {
            string value = "";
            System.Collections.Specialized.StringEnumerator myEnumerator = inCol.GetEnumerator();
            while (myEnumerator.MoveNext())
            {
                value += myEnumerator.Current;
            }

            return value;
        }

        /// <summary>
        /// Move caret to the end of the current text.
        /// </summary>
        private void MoveCaretToEndOfText()
        {
            this.SelectionStart = this.TextLength;
            this.ScrollToCaret();
        }

        /// <summary>
        /// Returns true is the caret is just before the current prompt.
        /// </summary>
        /// <returns></returns>
        private bool IsCaretJustBeforePrompt()
        {
            return IsCaretAtCurrentLine() && GetCurrentCaretColumnPosition() == prompt.Length;
        }

        /// <summary>
        /// Returns the column position. Useful for selections.
        /// </summary>
        /// <returns></returns>
        private int GetCurrentCaretColumnPosition()
        {
            string currentLine = GetCurrentLine();
            int currentCaretPosition = this.SelectionStart;
            return (currentCaretPosition - this.TextLength + currentLine.Length);
        }

        /// <summary>
        /// Is the caret at a writable position.
        /// </summary>
        /// <returns></returns>
        private bool IsCaretAtWritablePosition()
        {
            return IsCaretAtCurrentLine() && GetCurrentCaretColumnPosition() >= prompt.Length;
        }

        /// <summary>
        /// Sets the text of the prompt.  Default is ">>>"
        /// </summary>
        /// <param name="val">string of new prompt</param>
        public void SetPromptText(string val)
        {
            string currentLine = GetCurrentLine();
            this.Select(0, prompt.Length);
            this.SelectedText = val;
            prompt = val;
        }

        /// <summary>
        /// Gets and sets the IronTextBox prompt.
        /// </summary>
        public string Prompt
        {
            get { return prompt; }
            set { SetPromptText(value); }
        }

        /// <summary>
        /// Returns the string array of the command history. 
        /// </summary>
        /// <returns></returns>
        public string[] GetCommandHistory()
        {
            return commandHistory.GetCommandHistory();
        }

        /// <summary>
        /// Adds text to the IronTextBox.
        /// </summary>
        /// <param name="text"></param>
        public void WriteText(string text)
        {
            this.AddText(text);
        }

        #region IronTextBox Events
        /// <summary>
        /// Handle KeyPress events here.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">KeyPressEventArgs</param>
        private void consoleTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            //If current key is a backspace and is just before prompt, then stay put!
            if (e.KeyChar == (char)8 && IsCaretJustBeforePrompt())
            {
                e.Handled = true;
                return;
            }

            //If current key is enter
            if (IsTerminatorKey(e.KeyChar))
            {
                //**ANY CHANGES HERE MUST ALSO BE COPIED TO SimEnter()**
                e.Handled = true;
                string currentCommand = GetTextAtPrompt();

                //Optional: add the command to the stringcollection
                scollection.Add(currentCommand);
                ///////////////////////////////////////////////////

                //If it is not an empty command, then "fire" the command
                if (currentCommand.Length != 0 && this.defStmtBuilder.Length == 0 && !IsRawInput)
                {
                    if (!currentCommand.Trim().Contains("raw_input")) //NOXLATE
                        printLine();
                    ((IronTextBoxControl)this.Parent).FireCommandEntered(currentCommand);
                    commandHistory.Add(currentCommand);
                }

                //if we are doing a def statement (currentCommand.EndsWith(":"))
                if (this.defStmtBuilder.Length != 0)
                {
                    if (currentCommand.EndsWith(":")) //NOXLATE
                    {
                        //we are in the first line of a def, it has already printed to console
                        
                        //autoindent the current autoindent value
                        //int asize = Parser.GetNextAutoIndentSize(this.defStmtBuilder.ToString()+"\r\n", 4);

                        //don't printPrompt();
                        ReplaceTextAtPrompt("..." + CreateIndentstring(4)); //NOXLATE
                        e.Handled = true;
                        return;

                    }
                    else//We are past the first line, and are indenting or ending a def
                    {
                        this.defStmtBuilder.Append(currentCommand + "\r\n"); //NOXLATE

                        //if it is an empty command let's see if we just finished a def statement
                        if (currentCommand.Trim().Equals(string.Empty)) //NOXLATE
                        {
                            ((IronTextBoxControl)this.Parent).FireCommandEntered(this.defStmtBuilder.ToString().Trim());
                            commandHistory.Add(this.defStmtBuilder.ToString());

                            //we just finished a def so clear the defbuilder
                            this.defStmtBuilder = this.defStmtBuilder.Remove(0, this.defStmtBuilder.Length);
                        }
                        else
                        {
                            //don't printPrompt();
                            AddText("\r\n..." + CreateIndentstring(4)); //NOXLATE
                            e.Handled = true;
                            return;
                        }
                    }
                }

                //raw_input support...
                if (currentCommand.Trim().Contains("raw_input(")) //NOXLATE
                {
                    IsRawInput = true;

                    //Note: if raw_input is in quotes this will not work
                    //fyi: IronPython.Modules.Builtin.RawInput();
                    //remove the "\r\n" from IPEWrapper
                    this.Text = this.Text.Remove(this.Text.Length - "\r\n".Length, "\r\n".Length); //NOXLATE
                    rawprompt = (string)this.Lines.GetValue(this.Lines.Length - 1);
                    MoveCaretToEndOfText();

                    //AddText(temp);
                    e.Handled = true;
                    return;
                }
                                
                if(IsRawInput)
                {
                    string rawcommand = (string)this.Lines.GetValue(this.Lines.Length - 2);
                    rawcommand = rawcommand.Replace(Prompt, string.Empty);
                    string tempprompt = (string)this.Lines.GetValue(this.Lines.Length - 1);

                    var iptemp = Python.CreateEngine();

                    //examine to see what type of raw_input
                    if (rawcommand.Trim().Equals("raw_input()")) //NOXLATE
                    {
                        IsRawInput = false;
                    }
                    else// s = raw_input('--> ')
                    {
                        IsRawInput = false;
                        rawprompt = string.Empty;
                        e.Handled = true;
                        printPrompt();
                        MoveCaretToEndOfText();
                        return;
                    }
                    

                }

                //if(GetTextAtPrompt().Trim().Equals(""))
                    printPrompt();
            }


            /*
            // Handle backspace and stringcollection to help the commandhistory accuracy and debugging.
            if (e.KeyChar == (char)8 && (GetStringCollectValue(scollection, scollection.Count - 1).Length == 1) && commandHistory.LastCommand.Contains(GetStringCollectValue(scollection, scollection.Count - 1)))
            {
                scollection.RemoveAt(scollection.Count - 1);
            }*/

        }

        /// <summary>
        /// Build a string of returning spaces for indenting
        /// </summary>
        /// <param name="indentsize"></param>
        /// <returns></returns>
        public string CreateIndentstring(int indentsize)
        {
            string r = string.Empty;
            for (int i = 0; i < indentsize; i++)
            {
                r += " "; //NOXLATE
            }
            return r;
        }

        /// <summary>
        /// KeyEvent control for staying inside the currentline and autocomplete features
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">KeyEventArgs</param>
        private void ConsoleControl_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // If the caret is anywhere else, set it back when a key is pressed.
            if (!IsCaretAtWritablePosition() && !(e.Control || IsTerminatorKey(e.KeyCode)))
            {
                MoveCaretToEndOfText();
            }

            // Prevent caret from moving before the prompt
            if (e.KeyCode == System.Windows.Forms.Keys.Left && IsCaretJustBeforePrompt() || e.KeyCode == System.Windows.Forms.Keys.Back && IsCaretJustBeforePrompt())
            {
                e.Handled = true;
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.Down)
            {
                if (commandHistory.DoesNextCommandExist())
                {
                    ReplaceTextAtPrompt(commandHistory.GetNextCommand());
                }
                e.Handled = true;
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.Up)
            {
                if (commandHistory.DoesPreviousCommandExist())
                {
                    ReplaceTextAtPrompt(commandHistory.GetPreviousCommand());
                }
                e.Handled = true;
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.Right)
            {
                // Performs command completion
                string currentTextAtPrompt = GetTextAtPrompt();
                string lastCommand = commandHistory.LastCommand;

                //If the last command is not nul and no text at the current prompt or lastcommand starts with the currenttext at the current prompt,
                //then autofill because the right arrow key was pressed.
                if (lastCommand != null && (currentTextAtPrompt.Length == 0 || lastCommand.StartsWith(currentTextAtPrompt)))
                {
                    if (lastCommand.Length > currentTextAtPrompt.Length)
                    {
                        int i = scollection.Count;
                        //scollection.Insert(scollection.Count, lastCommand[currentTextAtPrompt.Length].ToString());
                        this.AddText(lastCommand[currentTextAtPrompt.Length].ToString());
                    }
                }
            }
        }


        #endregion IronTextBox Events

        #endregion IronTextBox Base Methods

        #region IronTextBox IronPython Support
        /// <summary>
        /// Stores input commands from IronTextBox
        /// </summary>
        StringCollection input = new StringCollection();

        /// <summary>
        /// Stores output generated from IronPython
        /// </summary>
        StringCollection output = new StringCollection();

        #endregion IronTextBox IronPython Support

        #region StringCollection support
        /// <summary>
        /// Commands and strings from IronTextBox.AddText() gets stored here
        /// Status: Currently not used 3/12/06 11:16am
        /// </summary>
        System.Collections.Specialized.StringCollection scollection = new System.Collections.Specialized.StringCollection();

        /// <summary>
        /// Returns a string retrieved from a StringCollection.
        /// </summary>
        /// <param name="inCol">StringCollection to be searched.</param>
        /// <param name="index">index of StringCollection to retrieve.</param>
        public string GetStringCollectValue(System.Collections.Specialized.StringCollection inCol, int index)
        {
            string value = string.Empty;
            int count = 0;
            System.Collections.Specialized.StringEnumerator myEnumerator = inCol.GetEnumerator();
            while (myEnumerator.MoveNext())
            {
                if (index == count)
                {
                    value = myEnumerator.Current;
                }

                count = count + 1;
            }

            return value;
        }
        #endregion StringCollection support
    }
    #endregion IronTextBox Class

    #region IronTextBoxControl Class
    /// <summary>
    /// Summary description for IronTextBoxControl.
    /// </summary>
    public class IronTextBoxControl : UserControl
    {
        #region IronTextBoxControl members
        /// <summary>
        /// Main IronPython ScriptEngine
        /// </summary>
        public ScriptEngine engine;

        /// <summary>
        /// Main IronPython ScriptScope
        /// </summary>
        public ScriptScope scope;

        /// <summary>
        /// The IronTextBox member.
        /// </summary>
        private IronTextBox consoleTextBox;

        /// <summary>
        /// The CommandEntered event
        /// </summary>
        public event EventCommandEntered CommandEntered;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        /// <summary>
        /// Adds def lines one by one.
        /// </summary>
        public StringBuilder defBuilder
        {
            get { return consoleTextBox.defStmtBuilder; }
            set
            {
                if (consoleTextBox != null)
                    consoleTextBox.defStmtBuilder = value;
            }
        }

        /// <summary>
        /// Returns the string array of the command history.
        /// </summary>
        /// <returns></returns>
        public string[] GetCommandHistory()
        {
            return consoleTextBox.GetCommandHistory();
        }

        /// <summary>
        /// Gets and sets console text ForeColor. 
        /// </summary>
        public Color ConsoleTextForeColor
        {
            get { return consoleTextBox != null ? consoleTextBox.ForeColor : Color.Black; }
            set
            {
                if (consoleTextBox != null)
                    consoleTextBox.ForeColor = value;
            }
        }

        /// <summary>
        /// Gets and sets console text BackColor. 
        /// </summary>
        public Color ConsoleTextBackColor
        {
            get { return consoleTextBox != null ? consoleTextBox.BackColor : Color.White; }
            set
            {
                if (consoleTextBox != null)
                    consoleTextBox.BackColor = value;
            }
        }

        /// <summary>
        /// Gets and sets console Font. 
        /// </summary>
        public Font ConsoleTextFont
        {
            get { return consoleTextBox != null ? consoleTextBox.Font : new Font("Lucida Console", 8); }
            set
            {
                if (consoleTextBox != null)
                    consoleTextBox.Font = value;
            }
        }

        /// <summary>
        /// Gets and sets string to be used for the Prompt.
        /// </summary>
        public string Prompt
        {
            get { return consoleTextBox.Prompt; }
            set { consoleTextBox.Prompt = value; }
        }
        #endregion IronTextBoxControl members

        /// <summary>
        /// IronTextBoxControl
        /// </summary>
        public IronTextBoxControl()
        {
            InitializeComponent();

            //Create the ScriptRuntime
            engine = Python.CreateEngine(); //ScriptRuntime.Create().GetEngine("py");
            //Create the scope for the ScriptEngine
            scope = engine.CreateScope();
            //Expose our host application
            scope.SetVariable("app", new Services.HostApplication()); //NOXLATE
            //IronTextBox's CommandEntered event
            CommandEntered += new EventCommandEntered(irontextboxControl_CommandEntered);
        }

        /// <summary>
        /// Executes the Python file within the IronTextBox environment.
        /// A nice way to quickly get a Python module in CLI to test or use.
        /// </summary>
        /// <param name="pyfile">Python file (.py)</param>
        /// <returns>object</returns>
        object DoIPExecuteFile(string pyfile)
        {
            ScriptSource source = engine.CreateScriptSourceFromFile(pyfile);
            return source.Execute(scope);
        }

        /// <summary>
        /// Executes the code in SourceCodeKind.SingleStatement to fire the command event
        /// Use DoIPEvaluate if you do not wish to fire the command event
        /// </summary>
        /// <param name="pycode">python statement</param>
        /// <returns>object</returns>
        object DoIPExecute(string pycode)
        {
            ScriptSource source = engine.CreateScriptSourceFromString(pycode, SourceCodeKind.SingleStatement);
            return source.Execute(scope);
        }

        /// <summary>
        /// Executes the code in SourceCodeKind.Expression not to fire the command event
        /// Use DoIPExecute if you do wish to fire the command event
        /// </summary>
        /// <param name="pycode">Python expression</param>
        /// <returns>object</returns>
        object DoIPEvaluate(string pycode)
        {
            ScriptSource source = engine.CreateScriptSourceFromString(pycode, SourceCodeKind.Expression);
            return source.Execute(scope);
        }

        /// <summary>
        /// irontextboxControl_CommandEntered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void irontextboxControl_CommandEntered(object sender, CommandEnteredEventArgs e)
        {
            string command = e.Command.TrimEnd();

            engine.Runtime.IO.SetOutput(new IPEStreamWrapper(IPEStreamWrapper.IPEngineResponse), engine.Runtime.IO.InputEncoding);

            //Create a temp object to use
            //object tempobject;

            //Begin IronTextBox evaluation if something there....
            if (command != string.Empty)
            {
                if (command == "cls") //NOXLATE
                    this.Clear();
                else if (command.StartsWith("refl ")) //NOXLATE
                    this.ReflectIdentifier(command.Substring(5), false);
                else if (command.StartsWith("reflw ")) //NOXLATE
                    this.ReflectIdentifier(command.Substring(6), true);
                else if (command.StartsWith("printw ")) //NOXLATE
                    this.PrintIdentifier(command.Substring(7), true);
                else if (command == "history") //NOXLATE
                {
                    string[] commands = this.GetCommandHistory();
                    StringBuilder stringBuilder = new StringBuilder(commands.Length);
                    foreach (string s in commands)
                    {
                        stringBuilder.Append(s);
                        stringBuilder.Append(System.Environment.NewLine);
                    }
                    this.WriteText(stringBuilder.ToString());
                }
                else if (command == "help") //NOXLATE
                {
                    this.WriteText(GetHelpText());
                }
                else if (command == "newconsole") //NOXLATE
                {
                    //consoleTextBox.global_eng = new PythonEngine();
                    this.WriteText("Not currently supported\r\n"); //NOXLATE
                }
                else if (command.StartsWith("prompt") && command.Length == 6)
                {
                    string[] parts = command.Split(new char[] { '=' }); //NOXLATE
                    if (parts.Length == 2 && parts[0].Trim() == "prompt") //NOXLATE
                        this.Prompt = parts[1].Trim();
                }
                else if (command == "btaf") //NOXLATE
                {
                    //btaf = Browse To Append File....
                    //Check to see if sys is loaded
                    if (!DoIPEvaluate("dir()").ToString().Contains("sys")) //NOXLATE
                    {
                        consoleTextBox.printPrompt();
                        consoleTextBox.WriteText("import sys"); //NOXLATE
                        this.SimEnter();
                    }

                    System.Windows.Forms.FolderBrowserDialog ofd = new System.Windows.Forms.FolderBrowserDialog();
                    ofd.SelectedPath = Paths.MiscDirs.vs_Projects;
                    ofd.ShowDialog();
                    consoleTextBox.printPrompt();
                    consoleTextBox.WriteText("sys.path.append(\"" + ofd.SelectedPath + "\")"); //NOXLATE
                    this.SimEnter();
                }
                else if (command == "runfile") //NOXLATE
                {
                    //runfile - Run a .Py file.  Calls OpenFileDialog to PythonEngine.RunFile....
                    //  goodfor debuging .y file within IDE
                    this.Runfile();
                }
                else if (command == "btwfi") //NOXLATE
                {
                    //btwfi - Browse To Walk FIle. Calls OpenFileDialog.
                    this.WalkPythonFile();
                }
                else if (command == "rew") //NOXLATE
                {
                    //btwfi - Browse To Walk FIle. Calls OpenFileDialog.
                    StringBuilder SBCode = new StringBuilder();
                    this.RewritePyFiletoSB(out SBCode);
                    DoIPExecute(SBCode.ToString()); //transformed object code from a .py
                }
                else if (command.StartsWith("paths")) //NOXLATE
                {
                    //Appends all hardcoded common paths stored in Paths
                    //paths [-arg] - [args: -misc, -python24, -ironpython, -all] (-all=default)
                    if (command.Contains(" -")) //NOXLATE
                    {
                        string[] splitcommand = command.Split('-'); //NOXLATE
                        splitcommand[1] = splitcommand[1].Trim();
                        this.ImportPaths(splitcommand[1]);
                    }
                    else
                        this.ImportPaths(command.Trim());

                }
                else if (command.TrimEnd().EndsWith(":") == true) //NOXLATE
                {
                    //Need to do a ReadStatement...
                    try
                    {
                        //bool isMultiLine = false;
                        int numberOfBlankLines = 0;
                        object ExecWrapper = null;
                        //bool result;

                        string line = command;
                        if (line == null)
                        {
                            if (ExecWrapper != null)
                            {
                                //Ops.Call(ExecWrapper, new object[] { null });//not needed for IP2?
                            }
                            //result = false;
                        }

                        defBuilder.Append(line);
                        defBuilder.Append("\r\n"); //NOXLATE

                        bool endOfInput = (line.Length == 0);

                        //old//s = ParsetheText(consoleTextBox.global_eng.Sys, new CompilerContext(), defBuilder.ToString(), endOfInput, out parsingMultiLineString, out isMultiLine);
                        string[] seperators = new string[] { "\r" }; //NOXLATE
                        string[] allpieces = defBuilder.ToString().Split(seperators, StringSplitOptions.None);

                        if (/*Options.AutoIndentSize != 0 &&*/ line.Trim().Length == 0)
                            numberOfBlankLines++;
                        else
                            numberOfBlankLines = 0;


                        if (allpieces.Length > 1)
                        {
                            // Note that splitting a string literal over multiple lines does not 
                            // constitute a multi-line statement.
                            //isMultiLine = true;
                        }

                        //autoIndentSize = Parser.GetNextAutoIndentSize(defBuilder.ToString(), autoIndentSize);//Not needed in IP2?
                        //result = true;
                    }
                    catch
                    {
                    }
                }

                else //misc commands...
                {
                    try
                    {
                        DoIPExecute(command);
                        this.WriteText(Environment.NewLine + IPEStreamWrapper.sbOutput.ToString());
                        //added to fix "rearviewmirror" (IPEStreamWrapper.sbOutput not clearing) bug.
                        IPEStreamWrapper.sbOutput.Remove(0, IPEStreamWrapper.sbOutput.Length);        //Clear
                    }

                    catch (Exception err)//catch any errors
                    {
                        this.WriteText("\r\nERROR: " + err.Message); //NOXLATE
                    }
                }
            }
        }

        private void PrintIdentifier(string identifier, bool newWindow)
        {
            try
            {
                dynamic obj = scope.GetVariable(identifier);
                if (newWindow)
                    new TextWindow(obj.ToString()).ShowDialog();
                else
                    WriteText(obj.ToString());
            }
            catch (Exception ex)
            {
                WriteText("ERROR: " + ex.Message); //NOXLATE
            }
        }

        private void ReflectIdentifier(string identifier, bool newWindow)
        {
            try
            {
                dynamic obj = scope.GetVariable(identifier);
                var sb = new StringBuilder();
                sb.Append("========= Reflecting " + identifier + " ==========" + Environment.NewLine); //NOXLATE
                Type t = obj.GetType();
                sb.Append("CLR Type: " + t.Name + Environment.NewLine); //NOXLATE
                sb.Append("Methods:" + Environment.NewLine); //NOXLATE
                foreach (var method in t.GetMethods())
                {
                    sb.Append(" - " + method.ToString() + Environment.NewLine); //NOXLATE
                }
                sb.Append("Properties:" + Environment.NewLine); //NOXLATE
                foreach (var prop in t.GetProperties())
                {
                    sb.Append(" - " + prop.Name + " (" + prop.PropertyType.Name + ")" + Environment.NewLine); //NOXLATE
                }
                sb.Append("========= Reflection complete ==========" + Environment.NewLine); //NOXLATE
                if (newWindow)
                    new TextWindow(sb.ToString()).ShowDialog();
                else
                    WriteText(sb.ToString());
            }
            catch (Exception ex)
            {
                WriteText("ERROR: " + ex.Message); //NOXLATE
            }
        }

        /// <summary>
        /// Displays information about IronTextBox and user's IronPython version.
        /// </summary>
        /// <returns>Returns string information about IronTextBox and user's IronPython version.</returns>
        public string GetHelpText()
        {
            string helpText;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("*******************************************"); //NOXLATE
            stringBuilder.Append(System.Environment.NewLine);
            stringBuilder.Append("**   Maestro IronPython Console Help     **"); //NOXLATE
            stringBuilder.Append(System.Environment.NewLine);
            stringBuilder.Append("*******************************************"); //NOXLATE
            stringBuilder.Append(System.Environment.NewLine);
            //stringBuilder.Append("You are using " + engine.LanguageDisplayName + " (" + engine.LanguageVersion + ")"); //NOXLATE
            stringBuilder.Append("You are using " + engine.Setup.DisplayName + " (" + engine.LanguageVersion + ")"); //NOXLATE
            stringBuilder.Append(System.Environment.NewLine);
            stringBuilder.Append(System.Environment.NewLine);
            stringBuilder.Append("Commands Available:"); //NOXLATE
            stringBuilder.Append(System.Environment.NewLine);
            stringBuilder.Append(" prompt - Changes prompt. Usage: prompt=<desired_prompt>"); //NOXLATE
            stringBuilder.Append(System.Environment.NewLine);
            stringBuilder.Append(" history - prints history of entered commands."); //NOXLATE
            stringBuilder.Append(System.Environment.NewLine);
            stringBuilder.Append(" cls - Clears the screen."); //NOXLATE
            stringBuilder.Append(System.Environment.NewLine);
            //stringBuilder.Append(" newconsole - Clears the current PythonEngine.");
            stringBuilder.Append(" refl <variable> - Displays all members/properties of a given variable"); //NOXLATE
            stringBuilder.Append(System.Environment.NewLine);
            stringBuilder.Append(" reflw <variable> - Displays all members/properties of a given variable to a new window"); //NOXLATE
            stringBuilder.Append(System.Environment.NewLine);
            stringBuilder.Append(" printw <variable> - Prints the value of the given variable to a new window"); //NOXLATE
            stringBuilder.Append(System.Environment.NewLine);
            stringBuilder.Append(" btaf - Browse To Append Folder. Calls FolderBrowserDialog."); //NOXLATE
            stringBuilder.Append(System.Environment.NewLine);
            stringBuilder.Append(" btwfi - Browse To Walk FIle. Calls OpenFileDialog."); //NOXLATE
            stringBuilder.Append(System.Environment.NewLine);
            stringBuilder.Append(" paths [-arg] - [args: -misc, -python24, -ironpython, -all] (-all=default)"); //NOXLATE
            stringBuilder.Append(System.Environment.NewLine);
            stringBuilder.Append(" rew - Re-Write a Python file into a StringBuilder.(for testing)"); //NOXLATE
            stringBuilder.Append(System.Environment.NewLine);
            stringBuilder.Append(" runfile - Run a .Py file.  Calls OpenFileDialog to PythonEngine.RunFile."); //NOXLATE
            stringBuilder.Append(System.Environment.NewLine);
            helpText = stringBuilder.ToString();
            return helpText;
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.consoleTextBox = new IronTextBox();
            this.SuspendLayout();
            // 
            // consoleTextBox
            // 
            //	this.consoleTextBox.AcceptsReturn = true;
            this.consoleTextBox.AcceptsTab = true;
            this.consoleTextBox.BackColor = Color.White;
            this.consoleTextBox.Dock = DockStyle.Fill;
            this.consoleTextBox.Location = new Point(0, 0);
            this.consoleTextBox.Multiline = true;
            this.consoleTextBox.Name = "consoleTextBox";
            this.consoleTextBox.Prompt = ">>>";
            this.consoleTextBox.ScrollBars = ScrollBars.Both; //for TextBox use
            //this.consoleTextBox.ScrollBars = RichTextBoxScrollBars.Both; //for RichTextBox use
            this.consoleTextBox.Font = new Font("Lucida Console", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((Byte)(0)));
            this.consoleTextBox.Size = new Size(232, 216);
            this.consoleTextBox.TabIndex = 0;
            this.consoleTextBox.Text = "";
            // 
            // IronTextBoxControl
            // 
            this.Controls.Add(this.consoleTextBox);
            this.Name = "IronTextBoxControl";
            this.Size = new Size(232, 216);
            this.ResumeLayout(false);

        }
        #endregion

        #region Overides
        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        #endregion Overides

        /// <summary>
        /// Run the command.
        /// </summary>
        /// <param name="command">Command line string.</param>
        internal void FireCommandEntered(string command)
        {
            OnCommandEntered(command);
        }

        /// <summary>
        /// Creates new EventCommandEntered event.
        /// </summary>
        /// <param name="command">Command line string.</param>
        protected virtual void OnCommandEntered(string command)
        {
            if (CommandEntered != null)
                CommandEntered(command, new CommandEnteredEventArgs(command));
        }

        /// <summary>
        /// Clear the current text in the IronTextBox.
        /// </summary>
        public void Clear()
        {
            consoleTextBox.Clear();
        }

        /// <summary>
        /// Send text to the IronTextBox.
        /// </summary>
        /// <param name="text"></param>
        public void WriteText(string text)
        {
            consoleTextBox.WriteText(text);
        }

        /// <summary>
        /// Simulate the Enter KeyPress event.
        /// </summary>
        public void SimEnter()
        {
            string currentCommand = consoleTextBox.GetTextAtPrompt();
            consoleTextBox.Focus();

            //Optional: add the command to the stringcollection
            //consoleTextBox.scollection.Add(currentCommand);
            ///////////////////////////////////////////////////

            //If it is not an empty command, then "fire" the command
            if (currentCommand.Length != 0)
            {
                //consoleTextBox.printLine();
                ((IronTextBoxControl)consoleTextBox.Parent).FireCommandEntered(currentCommand);
                consoleTextBox.AddcommandHistory(currentCommand);
            }
            else
            {
                //if it is an empty command let's see if we just finished a def statement
                if (consoleTextBox.defStmtBuilder.Length != 0)
                {
                    ((IronTextBoxControl)consoleTextBox.Parent).FireCommandEntered(consoleTextBox.defStmtBuilder.ToString());
                    consoleTextBox.AddcommandHistory(consoleTextBox.defStmtBuilder.ToString());

                    //we just finished a def so clear the defbuilder
                    consoleTextBox.defStmtBuilder = consoleTextBox.defStmtBuilder.Remove(0, consoleTextBox.defStmtBuilder.Length);
                }
            }
            consoleTextBox.printPrompt();
        }

        /// <summary>
        /// Opens a Python files and reads line by line into IronTextBox.
        /// </summary>
        /// <param name="fullpathfilename">fullpathfilename</param>
        public void WalkPythonFile(string fullpathfilename)
        {
            try
            {
                string filetext = File.ReadAllText(fullpathfilename);
                //tabs create a problem when trying to remove comments
                filetext = filetext.Replace("\t", "    "); //NOXLATE

                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StringReader sr = new StringReader(filetext))
                {
                    String line;
                    StringBuilder sb = new StringBuilder();
                    int pos = 0;
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                       //if the line is a # comment line, or a single line do not add...
                        if (!line.StartsWith("#") && !line.StartsWith("    #") && !line.StartsWith("        #") && !line.StartsWith("            #") && !line.Equals("\r\n") && line != "") //NOXLATE
                        {
                            //catch """ comments
                            if (line.StartsWith("\"\"\"") || line.StartsWith("    \"\"\"") || line.StartsWith("        \"\"\"") || line.StartsWith("            \"\"\"")) //NOXLATE
                            {
                                //the line may also end with """, so if it is not read until end
                                if (!IsSingleCommentLine(line))
                                {
                                    //get to the end of the comments
                                    while (!sr.ReadLine().TrimEnd().EndsWith("\"\"\"")) //NOXLATE
                                    {
                                        //do nothing
                                    }
                                }

                                //reassign line
                                line = sr.ReadLine();
                            }

                            //the line may also end with """, so if it is not read until end
                            if (!IsSingleCommentLine(line))
                            {
                                //if the line ends with """, then delete """ and read until end
                                if (line.TrimEnd().EndsWith("\"\"\"") && line.IndexOf("\"\"\"") == line.Length - 3) //NOXLATE
                                {
                                    //remove """ and reassign line
                                    line = line.Remove(line.Length - 3);

                                    //get to the end of the comments
                                    while (!sr.ReadLine().TrimEnd().EndsWith("\"\"\"")) //NOXLATE
                                    {
                                        //do nothing
                                    }
                                }
                                //then append line
                                sb.AppendLine(line);

                                consoleTextBox.WriteText(line);
                                SimEnter();
                            }
                        }


                        pos = sb.Length;
                        //if a blank line, enter previous text as a FuncDef
                        if (line == string.Empty && sb.Length != 0)
                        {
                            try //try to find last ""
                            {
                                //consoleTextBox.WriteText(sb.ToString());
                                SimEnter();
                            }
                            catch
                            {

                            }

                        }
                        //consoleTextBox.WriteText(line);

                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                consoleTextBox.WriteText("The file could not be read:"); //NOXLATE
                consoleTextBox.WriteText(e.Message);
            }

        }

        /// <summary>
        /// Opens a FolderBrowserDialog to load a Python file to read it line by line into IronTextBox.
        /// </summary>
        public void WalkPythonFile()
        {
            try
            {
                //See if sys is imported...
                if (!DoIPEvaluate("dir()").ToString().Contains("sys")) //NOXLATE
                {
                    consoleTextBox.printPrompt();
                    consoleTextBox.WriteText("import sys"); //NOXLATE
                    this.SimEnter();
                }

                //Browse to the file...
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = Paths.MiscDirs.vs_Projects;
                ofd.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickPy, "py") + "|" + StringConstants.AllFilesFilter; //NOXLATE
                ofd.ShowDialog();

                //Ask the user if they would like to append the path
                string message = "Do you need to append the folder:\r\n" + Path.GetDirectoryName(Path.GetFullPath(ofd.FileName)) + "\r\n\r\nto the PythonEngine?"; //NOXLATE
                string caption = "Append Folder Path"; //NOXLATE
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(this, message, caption, buttons);
                if (result == DialogResult.Yes)
                {
                    consoleTextBox.printPrompt();
                    consoleTextBox.WriteText("sys.path.append(\"" + Path.GetDirectoryName(Path.GetFullPath(ofd.FileName)) + "\")"); //NOXLATE
                    this.SimEnter();

                    //Keep asking until No
                    while (result.Equals(DialogResult.Yes))
                    {
                        //Ask the user if more folders are needed to be appended
                        message = "Do you need to append another folder?"; //NOXLATE
                        result = MessageBox.Show(this, message, caption, buttons);
                        if (result == DialogResult.Yes)
                        {
                            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                            fbd.SelectedPath = Path.GetDirectoryName(Path.GetFullPath(ofd.FileName));
                            fbd.ShowDialog();
                            consoleTextBox.printPrompt();
                            consoleTextBox.WriteText("sys.path.append(\"" + fbd.SelectedPath + "\")"); //NOXLATE
                            this.SimEnter();
                        }
                    }
                }

                WalkPythonFile(ofd.FileName);
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                consoleTextBox.WriteText("The file could not be read:"); //NOXLATE
                consoleTextBox.WriteText(e.Message);
            }
        }

        /// <summary>
        /// Run a .Py file.  Calls OpenFileDialog to PythonEngine.RunFile.
        /// </summary>
        public void Runfile()
        {
            try
            {
            //Browse to the file...
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Paths.MiscDirs.vs_Projects;
            ofd.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickPy, "py") + "|" + StringConstants.AllFilesFilter; //NOXLATE
            ofd.ShowDialog();

            DoIPExecuteFile(ofd.FileName);
                
            }
            catch (Exception ex)
            {
                consoleTextBox.WriteText("The file could not be read:"); //NOXLATE
                consoleTextBox.WriteText(ex.Message);
            }

        }

        /// <summary>
        /// Opens a Python file and reads line by line into a StringBuilder.
        /// </summary>
        /// <param name="sbCode">out StringBuilder</param>
        public void RewritePyFiletoSB(out StringBuilder sbCode)
        {
            StringBuilder sb = new StringBuilder();

            //See if sys is imported...
            if (!DoIPEvaluate("dir()").ToString().Contains("sys")) //NOXLATE
            {
                consoleTextBox.printPrompt();
                consoleTextBox.WriteText("import sys"); //NOXLATE
                this.SimEnter();
            }

            //Browse to the file...
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Paths.MiscDirs.vs_Projects;
            ofd.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickPy, "py") + "|" + StringConstants.AllFilesFilter; //NOXLATE
            ofd.ShowDialog();

            try
            {
                string filetext = File.ReadAllText(ofd.FileName);
                //tabs create a problem when trying to remove comments
                filetext = filetext.Replace("\t", "    "); //NOXLATE

                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StringReader sr = new StringReader(filetext))
                {
                    String line;
                    int pos = 0;
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {

                        /////temp testing
                        /// "        # unpp augmented predicate"
                        /// "        """ "
                        ///if (line == "    def chunk_lemmatised(self,lemmatised_text):")
                        ///{
                        ///    int temp = pos;
                        ///}
                        /////temp testing

                        //if the line is a # comment line, or a single line do not add...
                        if (!line.StartsWith("#") && !line.StartsWith("    #") && !line.StartsWith("        #") && !line.StartsWith("            #") && !line.Equals("\r\n") && line != "") //NOXLATE
                        {
                            //catch """ comments
                            if (line.StartsWith("\"\"\"") || line.StartsWith("    \"\"\"") || line.StartsWith("        \"\"\"") || line.StartsWith("            \"\"\"")) //NOXLATE
                            {
                                //the line may also end with """, so if it is not read until end
                                if (!IsSingleCommentLine(line))
                                {
                                    //get to the end of the comments
                                    while (!sr.ReadLine().TrimEnd().EndsWith("\"\"\"")) //NOXLATE
                                    {
                                        //do nothing
                                    }
                                }

                                //reassign line
                                line = sr.ReadLine();
                            }

                            //the line may also end with """, so if it is not read until end
                            if (!IsSingleCommentLine(line))
                            {
                                //if the line ends with """, then delete """ and read until end
                                if (line.TrimEnd().EndsWith("\"\"\"") && line.IndexOf("\"\"\"") == line.Length - 3) //NOXLATE
                                {
                                    //remove """ and reassign line
                                    line = line.Remove(line.Length - 3);

                                    //get to the end of the comments
                                    while (!sr.ReadLine().TrimEnd().EndsWith("\"\"\"")) //NOXLATE
                                    {
                                        //do nothing
                                    }
                                }
                                //then append line
                                sb.AppendLine(line);
                            }
                        }
                        pos = sb.Length;
                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                consoleTextBox.WriteText("The file could not be read:"); //NOXLATE
                consoleTextBox.WriteText(e.Message);
            }

            sbCode = sb;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <returns>true if line begins with #, or begins with """ and endwith """</returns>
        public bool IsSingleCommentLine(string line)
        {
            //Trim the end of the line because sometimes whitespace after """
            line = line.TrimEnd();

            if (line.StartsWith("#") || line.StartsWith("    #") || line.StartsWith("        #") || line.StartsWith("            #") && line != "") //NOXLATE
            {
                return true;
            }
            else if (line.StartsWith("\"\"\"") || line.StartsWith("    \"\"\"") || line.StartsWith("        \"\"\"") || line.StartsWith("            \"\"\"")) //NOXLATE
            {
                if (line.TrimEnd().EndsWith("\"\"\"") && line.IndexOf("\"\"\"") != line.Length - 3) //NOXLATE
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// Returns aa ArrayList from a StringCollection  
        /// </summary>
        /// <param name="StringColin">Incoming StringCollection.</param>
        public ArrayList Convert_StringCollectiontoArrayList(StringCollection StringColin)
        {
            ArrayList newArrayList = new ArrayList();

            StringEnumerator myEnumerator = StringColin.GetEnumerator();
            while (myEnumerator.MoveNext())
                newArrayList.Add(myEnumerator.Current.ToString());

            return newArrayList;
        }

        /// <summary>
        /// ImportPaths
        /// </summary>
        /// <param name="arg"></param>
        public void ImportPaths(string arg)
        {
            StringCollection scMiscDirs = new StringCollection();
            scMiscDirs.Add(Paths.MiscDirs.ConceptNet);
            scMiscDirs.Add(Paths.MiscDirs.montylingua);
            scMiscDirs.Add(Paths.MiscDirs.vs_Projects);
            StringEnumerator SCEMiscDirs = scMiscDirs.GetEnumerator();

            StringCollection scPython24Dirs = new StringCollection();
            scPython24Dirs.Add(Paths.Python24Dirs.Python24_DLLs);
            scPython24Dirs.Add(Paths.Python24Dirs.Python24_Lib);
            scPython24Dirs.Add(Paths.Python24Dirs.Python24_Lib_lib_tk);
            scPython24Dirs.Add(Paths.Python24Dirs.Python24_libs);
            scPython24Dirs.Add(Paths.Python24Dirs.Python24_Tools);
            scPython24Dirs.Add(Paths.Python24Dirs.Python24_Tools_Scripts);
            StringEnumerator SCEPython24Dirs = scPython24Dirs.GetEnumerator();

            StringCollection scIronPythonDirs = new StringCollection();
            scIronPythonDirs.Add(Paths.IronPythonDirs.IronPython_Tutorial);
            //scIronPythonDirs.Add(Paths.IronPythonDirs.Runtime);
            StringEnumerator SCEIronPythonDirs = scIronPythonDirs.GetEnumerator();

            //Create All SC
            StringCollection scAll = new StringCollection();
            while (SCEMiscDirs.MoveNext())
            {
                scAll.Add(SCEMiscDirs.Current);
            }
            while (SCEPython24Dirs.MoveNext())
            {
                scAll.Add(SCEPython24Dirs.Current);
            }
            while (SCEIronPythonDirs.MoveNext())
            {
                scAll.Add(SCEIronPythonDirs.Current);
            }
            StringEnumerator SCEAll = scAll.GetEnumerator();

            //Reset Enums
            SCEMiscDirs.Reset();
            SCEPython24Dirs.Reset();
            SCEIronPythonDirs.Reset();

            //Check to see if sys is loaded
            if (!DoIPEvaluate("dir()").ToString().Contains("sys")) //NOXLATE
            {
                consoleTextBox.printPrompt();
                consoleTextBox.WriteText("import sys"); //NOXLATE
                this.SimEnter();
            }
            else
                consoleTextBox.printPrompt();
            

            try
            {
                switch (arg)
                {
                    case "misc": //NOXLATE
                        {
                            while (SCEMiscDirs.MoveNext())
                            {
                                //consoleTextBox.printPrompt();
                                consoleTextBox.WriteText("sys.path.append(\"" + SCEMiscDirs.Current + "\")"); //NOXLATE
                                this.SimEnter();
                            }
                            break;
                        }
                    case "python24": //NOXLATE
                        {
                            while (SCEPython24Dirs.MoveNext())
                            {
                                //consoleTextBox.printPrompt();
                                consoleTextBox.WriteText("sys.path.append(\"" + SCEPython24Dirs.Current + "\")"); //NOXLATE
                                this.SimEnter();
                            }
                            break;
                        }
                    case "ironpython": //NOXLATE
                        {
                            while (SCEIronPythonDirs.MoveNext())
                            {
                                //consoleTextBox.printPrompt();
                                consoleTextBox.WriteText("sys.path.append(\"" + SCEIronPythonDirs.Current + "\")"); //NOXLATE
                                this.SimEnter();
                            }
                            break;
                        }
                    case "all": //NOXLATE
                        {
                            while (SCEAll.MoveNext())
                            {
                                //consoleTextBox.printPrompt();
                                consoleTextBox.WriteText("sys.path.append(\"" + SCEAll.Current + "\")"); //NOXLATE
                                this.SimEnter();
                            }
                            break;
                        }
                    case "paths": //NOXLATE
                        {
                            while (SCEAll.MoveNext())
                            {
                                //consoleTextBox.printPrompt();
                                consoleTextBox.WriteText("sys.path.append(\"" + SCEAll.Current + "\")"); //NOXLATE
                                this.SimEnter();
                            }
                            break;
                        }
                    default:
                        consoleTextBox.WriteText("Invalid arg. Only: -misc, -python24, -ironpython, -all"); //NOXLATE
                        break;
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                consoleTextBox.WriteText("ImportPaths error: "); //NOXLATE
                consoleTextBox.WriteText(e.Message);
            }
        }
    }
    #endregion IronTextBoxControl Class

    #region CommandHistory Class
    internal class CommandHistory
    {
        private int currentPosn;
        private string lastCommand;
        private ArrayList commandHistory = new ArrayList();

        internal CommandHistory()
        {
        }

        internal void Add(string command)
        {
            if (command != lastCommand)
            {
                commandHistory.Add(command);
                lastCommand = command;
                currentPosn = commandHistory.Count;
            }
        }

        internal bool DoesPreviousCommandExist()
        {
            return currentPosn > 0;
        }

        internal bool DoesNextCommandExist()
        {
            return currentPosn < commandHistory.Count - 1;
        }

        internal string GetPreviousCommand()
        {
            lastCommand = (string)commandHistory[--currentPosn];
            return lastCommand;
        }

        internal string GetNextCommand()
        {
            lastCommand = (string)commandHistory[++currentPosn];
            return LastCommand;
        }

        internal string LastCommand
        {
            get { return lastCommand; }
        }

        internal string[] GetCommandHistory()
        {
            return (string[])commandHistory.ToArray(typeof(string));
        }
    }
    #endregion CommandHistory Class

    #region Utils
    namespace Utils
    {
        /// <summary>
        /// 
        /// </summary>
        public class Converts
        {
            /// <summary>
            /// Custom MessageBox call. Excepts some random objects from IronPython and converts to string.
            /// </summary>
            /// <param name="inobject">Output object from IronPython.</param>
            public static void MessageBoxIronPy(Object inobject)
            {
                Type itstype = inobject.GetType();

                switch (itstype.FullName)
                {
                    case "IronPython.Runtime.PythonDictionary": //NOXLATE
                        PythonDictionary IPDict = new PythonDictionary();
                        IPDict = (PythonDictionary)inobject;
                        MessageBox.Show(IPDict.ToString());
                        break;
                    case "IronPython.Runtime.List": //NOXLATE
                        List IPList = new List();
                        IPList = (List)inobject;
                        MessageBox.Show(IPList.ToString());
                        break;
                    case "System.String": //NOXLATE
                        MessageBox.Show(inobject.ToString());
                        break;
                    case "System.Int32": //NOXLATE
                        MessageBox.Show(Convert.ToString(inobject));
                        break;
                    case "System.Collections.Specialized.StringCollection": //NOXLATE
                        StringCollection IPSC = new StringCollection();
                        IPSC = (StringCollection)inobject;
                        StringEnumerator SCE = IPSC.GetEnumerator();
                        string output = string.Empty;
                        while (SCE.MoveNext())
                            output += SCE.Current.ToString();
                        MessageBox.Show(output);
                        break;
                    default:
                        MessageBox.Show(inobject.GetType().ToString() + " not yet implemented."); //NOXLATE
                        break;
                }
            }
        }
    }
    #endregion Utils

    #region CommandEnteredEventArgs Class
    /// <summary>
    /// Command argument class.
    /// </summary>
    public class CommandEnteredEventArgs : EventArgs
    {
        string command;
        public CommandEnteredEventArgs(string command)
        {
            this.command = command;
        }

        public string Command
        {
            get { return command; }
        }
    }
    #endregion CommandEnteredEventArgs Class

    public delegate void EventCommandEntered(object sender, CommandEnteredEventArgs e);
}
