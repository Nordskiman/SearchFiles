using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchFiles
{
    public interface IView
    {
        void Show();
    }

    public interface ISearchView : IView
    {
        event Action StartSearch;
        event Action Closing;

        SynchronizationContext Context { get; }
        TreeView Tree { get; }
        ToolStripStatusLabel FileLabel { get; }
        ToolStripStatusLabel TimeLabel { get; }
        Button Start { get; }
        TextBox Directory { get; }
        TextBox Pattern { get; }
        TextBox SearchText { get; }
    }

    public partial class Search : Form, ISearchView
    {
        public Search()
        {
            Context = SynchronizationContext.Current;
            InitializeComponent();
            FormClosing += delegate { Closing?.Invoke(); };
        }

        public new void Show()
        {
            Application.Run(this);
        }

        public SynchronizationContext Context { get; }

        public TreeView Tree { get; private set; }

        public ToolStripStatusLabel FileLabel { get; private set; }

        public ToolStripStatusLabel TimeLabel { get; private set; }

        public Button Start { get; private set; }

        public TextBox Directory { get; private set; }

        public TextBox Pattern { get; private set; }

        public TextBox SearchText { get; private set; }

        public new event Action Closing;
        public event Action StartSearch;

        private void Start_Click_1(object sender, EventArgs e)
        {
            StartSearch?.Invoke();
        }

        private void Directory_Click_1(object sender, EventArgs e)
        {
            var diag = new FolderBrowserDialog();
            diag.ShowDialog();

            var path = diag.SelectedPath;

            if (!string.IsNullOrWhiteSpace(path))
                Directory.Text = path;
        }
    }
}
