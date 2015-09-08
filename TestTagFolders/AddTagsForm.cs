using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestTagFolders
{
    public partial class AddTagsForm : Form
    {
        public List<string> _addedTags = new List<string>();

        public AddTagsForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text != "")
            {
                this.listBox1.Items.Add(this.textBox1.Text);
                _addedTags.Add(this.textBox1.Text);
                this.textBox1.Text = "";
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            _callback(_addedTags);
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private Action<IEnumerable<string>> _callback;
        public void SetApplyCallback(Action<IEnumerable<string>> callback)
        {
            _callback = callback;
        }

        private void AddTagsForm_Load(object sender, EventArgs e)
        {
            this.textBox1.Focus();
        }
    }
}
