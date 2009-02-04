using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KillXml
{
    /// <summary>
    /// Main form of the KillXml application.
    /// </summary>
    public partial class MainForm : Form, IMainView
    {
        public MainForm()
        {
            InitializeComponent();
            presenter = new MainFormPresenter(this);
            listViewXPathResults.Hide();
            textBoxXPathError.Hide();
        }

        public string XmlSource
        {
            get { return richTextBoxXmlContent.Text; }
            set { richTextBoxXmlContent.Text = value; }
        }

        public string XPathExpression
        {
            get { return textBoxXPathExpression.Text; }
            set { textBoxXPathExpression.Text = value; }
        }

        public string XsltSource
        {
            get { return richTextBoxXsltSource.Text; }
            set { richTextBoxXsltSource.Text = value; }
        }

        public void ListNamespaces(IDictionary<string, string> namespaces)
        {
            listViewNamespaces.Items.Clear();

            foreach (KeyValuePair<string, string> pair in namespaces)
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Text = pair.Key;
                listViewItem.SubItems.Add(pair.Value);
                listViewNamespaces.Items.Add(listViewItem);
            }
        }

        public void ShowXPathError(Exception ex)
        {
            textBoxXPathError.Text = ex.Message;
            listViewXPathResults.Hide();
            textBoxXPathError.Show();
        }

        public void ShowXPathResults(IList<XPathResultItem> results)
        {
            listViewXPathResults.Items.Clear();

            if (results.Count > 0)
            {
                foreach (XPathResultItem item in results)
                {
                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.Text = item.NodeName;
                    listViewItem.SubItems.Add(item.NodeInnerXml);
                    listViewItem.SubItems.Add(item.NodePath);

                    listViewXPathResults.Items.Add(listViewItem);
                }
            }
            else
            {
                ListViewItem listViewItem = new ListViewItem();
                //listViewItem.ForeColor = Color.Red;
                listViewItem.Text = "Query did not match any nodes";

                listViewXPathResults.Items.Add(listViewItem);
            }

            textBoxXPathError.Hide();
            listViewXPathResults.Show();
        }

        public void ShowDocumentInTransformBrowser(Uri transformedXmlUrl)
        {
            webBrowserTransformedXml.Navigate(transformedXmlUrl);
        }

        private MainFormPresenter presenter;

        private void ButtonQuery_Click(object sender, EventArgs e)
        {
            presenter.ExecuteXPathQuery();
        }

        private void ButtonTransform_Click(object sender, EventArgs e)
        {
            presenter.OnTransformButtonClicked();
        }

        private void TextBoxXPathExpression_Enter(object sender, EventArgs e)
        {
            textBoxXPathExpression.SelectAll();
        }

        private void TextBoxXPathExpression_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                presenter.ExecuteXPathQuery();
        }

        private void TabControlLower_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPageIndex == 2)
                presenter.OnNamespacesTabShown();
        }
    }
}
