namespace KillXml
{
    /// <summary>
    /// Main form of the KillXml application.
    /// </summary>
    public partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.tabControlLower = new System.Windows.Forms.TabControl();
            this.tabPageXPath = new System.Windows.Forms.TabPage();
            this.listViewXPathResults = new System.Windows.Forms.ListView();
            this.buttonQuery = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxXPathExpression = new System.Windows.Forms.TextBox();
            this.tabPageXslt = new System.Windows.Forms.TabPage();
            this.richTextBoxXmlContent = new System.Windows.Forms.RichTextBox();
            this.textBoxXPathError = new System.Windows.Forms.TextBox();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.tabPageNamespaces = new System.Windows.Forms.TabPage();
            this.listViewNamespaces = new System.Windows.Forms.ListView();
            this.columnHeaderPrefix = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderUrl = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tabControlLower.SuspendLayout();
            this.tabPageXPath.SuspendLayout();
            this.tabPageNamespaces.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.richTextBoxXmlContent);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tabControlLower);
            this.splitContainerMain.Size = new System.Drawing.Size(919, 486);
            this.splitContainerMain.SplitterDistance = 222;
            this.splitContainerMain.SplitterWidth = 6;
            this.splitContainerMain.TabIndex = 0;
            // 
            // tabControlLower
            // 
            this.tabControlLower.Controls.Add(this.tabPageXPath);
            this.tabControlLower.Controls.Add(this.tabPageXslt);
            this.tabControlLower.Controls.Add(this.tabPageNamespaces);
            this.tabControlLower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlLower.Location = new System.Drawing.Point(0, 0);
            this.tabControlLower.Margin = new System.Windows.Forms.Padding(4);
            this.tabControlLower.Name = "tabControlLower";
            this.tabControlLower.SelectedIndex = 0;
            this.tabControlLower.Size = new System.Drawing.Size(919, 258);
            this.tabControlLower.TabIndex = 0;
            this.tabControlLower.Selected += new System.Windows.Forms.TabControlEventHandler(this.TabControlLower_Selected);
            // 
            // tabPageXPath
            // 
            this.tabPageXPath.Controls.Add(this.listViewXPathResults);
            this.tabPageXPath.Controls.Add(this.buttonQuery);
            this.tabPageXPath.Controls.Add(this.label1);
            this.tabPageXPath.Controls.Add(this.textBoxXPathExpression);
            this.tabPageXPath.Controls.Add(this.textBoxXPathError);
            this.tabPageXPath.Location = new System.Drawing.Point(4, 27);
            this.tabPageXPath.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageXPath.Name = "tabPageXPath";
            this.tabPageXPath.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageXPath.Size = new System.Drawing.Size(911, 227);
            this.tabPageXPath.TabIndex = 0;
            this.tabPageXPath.Text = "XPath";
            this.tabPageXPath.UseVisualStyleBackColor = true;
            // 
            // listViewXPathResults
            // 
            this.listViewXPathResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewXPathResults.BackColor = System.Drawing.SystemColors.Menu;
            this.listViewXPathResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewXPathResults.FullRowSelect = true;
            this.listViewXPathResults.GridLines = true;
            this.listViewXPathResults.Location = new System.Drawing.Point(8, 47);
            this.listViewXPathResults.Name = "listViewXPathResults";
            this.listViewXPathResults.Size = new System.Drawing.Size(890, 172);
            this.listViewXPathResults.TabIndex = 3;
            this.listViewXPathResults.UseCompatibleStateImageBehavior = false;
            this.listViewXPathResults.View = System.Windows.Forms.View.Details;
            // 
            // buttonQuery
            // 
            this.buttonQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonQuery.Location = new System.Drawing.Point(798, 3);
            this.buttonQuery.Margin = new System.Windows.Forms.Padding(4);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(100, 32);
            this.buttonQuery.TabIndex = 2;
            this.buttonQuery.Text = "Query";
            this.buttonQuery.UseVisualStyleBackColor = true;
            this.buttonQuery.Click += new System.EventHandler(this.ButtonQuery_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "XPath expression:";
            // 
            // textBoxXPathExpression
            // 
            this.textBoxXPathExpression.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxXPathExpression.Location = new System.Drawing.Point(143, 6);
            this.textBoxXPathExpression.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxXPathExpression.Name = "textBoxXPathExpression";
            this.textBoxXPathExpression.Size = new System.Drawing.Size(646, 26);
            this.textBoxXPathExpression.TabIndex = 0;
            // 
            // tabPageXslt
            // 
            this.tabPageXslt.Location = new System.Drawing.Point(4, 27);
            this.tabPageXslt.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageXslt.Name = "tabPageXslt";
            this.tabPageXslt.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageXslt.Size = new System.Drawing.Size(911, 227);
            this.tabPageXslt.TabIndex = 1;
            this.tabPageXslt.Text = "XSLT";
            this.tabPageXslt.UseVisualStyleBackColor = true;
            // 
            // richTextBoxXmlContent
            // 
            this.richTextBoxXmlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxXmlContent.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxXmlContent.Name = "richTextBoxXmlContent";
            this.richTextBoxXmlContent.Size = new System.Drawing.Size(919, 222);
            this.richTextBoxXmlContent.TabIndex = 0;
            this.richTextBoxXmlContent.Text = string.Empty;
            // 
            // textBoxXPathError
            // 
            this.textBoxXPathError.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxXPathError.ForeColor = System.Drawing.Color.Firebrick;
            this.textBoxXPathError.Location = new System.Drawing.Point(8, 47);
            this.textBoxXPathError.Multiline = true;
            this.textBoxXPathError.Name = "textBoxXPathError";
            this.textBoxXPathError.Size = new System.Drawing.Size(890, 173);
            this.textBoxXPathError.TabIndex = 4;
            this.textBoxXPathError.Visible = false;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Node";
            // 
            // tabPageNamespaces
            // 
            this.tabPageNamespaces.Controls.Add(this.listViewNamespaces);
            this.tabPageNamespaces.Location = new System.Drawing.Point(4, 27);
            this.tabPageNamespaces.Name = "tabPageNamespaces";
            this.tabPageNamespaces.Size = new System.Drawing.Size(911, 227);
            this.tabPageNamespaces.TabIndex = 2;
            this.tabPageNamespaces.Text = "Namespaces";
            this.tabPageNamespaces.UseVisualStyleBackColor = true;
            // 
            // listViewNamespaces
            // 
            this.listViewNamespaces.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderPrefix,
            this.columnHeaderUrl});
            this.listViewNamespaces.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewNamespaces.GridLines = true;
            this.listViewNamespaces.Location = new System.Drawing.Point(0, 0);
            this.listViewNamespaces.Name = "listViewNamespaces";
            this.listViewNamespaces.Size = new System.Drawing.Size(911, 227);
            this.listViewNamespaces.TabIndex = 0;
            this.listViewNamespaces.UseCompatibleStateImageBehavior = false;
            this.listViewNamespaces.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderPrefix
            // 
            this.columnHeaderPrefix.Text = "prefix";
            this.columnHeaderPrefix.Width = 154;
            // 
            // columnHeaderUrl
            // 
            this.columnHeaderUrl.Text = "URL";
            this.columnHeaderUrl.Width = 577;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Path";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 486);
            this.Controls.Add(this.splitContainerMain);
            this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "KillXml";
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.tabControlLower.ResumeLayout(false);
            this.tabPageXPath.ResumeLayout(false);
            this.tabPageXPath.PerformLayout();
            this.tabPageNamespaces.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TabControl tabControlLower;
        private System.Windows.Forms.TabPage tabPageXPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxXPathExpression;
        private System.Windows.Forms.TabPage tabPageXslt;
        private System.Windows.Forms.Button buttonQuery;
        private System.Windows.Forms.ListView listViewXPathResults;
        private System.Windows.Forms.RichTextBox richTextBoxXmlContent;
        private System.Windows.Forms.TextBox textBoxXPathError;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TabPage tabPageNamespaces;
        private System.Windows.Forms.ListView listViewNamespaces;
        private System.Windows.Forms.ColumnHeader columnHeaderPrefix;
        private System.Windows.Forms.ColumnHeader columnHeaderUrl;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}

