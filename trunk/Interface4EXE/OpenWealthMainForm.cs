using System;
using System.Windows.Forms;

namespace OpenWealth.Interface4EXE
{
    public partial class OpenWealthMainForm : Form, IInterface, IPlugin
    {
        public OpenWealthMainForm()
        {
            InitializeComponent();

            Core.SetGlobal("Interface", this);
        }

        public Form GetMainForm()
        {
            return this;
        }

        public void AddMenuItem(string textLevel1, string textLevel2, string textLevel3, EventHandler callback)
        {
            if (textLevel1 == null)
            {
                throw new ArgumentNullException("textLevel1");
            }
            if (this.InvokeRequired)
            {
                AddMenuItemCallback addMenuItemCallback = new AddMenuItemCallback(this.AddMenuItem);
                Invoke(addMenuItemCallback, new object[] { textLevel1, textLevel2, textLevel3, callback });
            }
            else
            {
                ToolStripMenuItem menuItemLevel1 = null;
                foreach (ToolStripMenuItem item in mainMenuStrip.Items)
                    if (textLevel1 == item.Text)
                    {
                        menuItemLevel1 = item;
                        break;
                    }
                if (menuItemLevel1 == null)
                {
                    menuItemLevel1 = new ToolStripMenuItem(textLevel1);
                    mainMenuStrip.Items.Add(menuItemLevel1);
                    if ((textLevel2 == null) || (textLevel2 == string.Empty))
                    {
                        menuItemLevel1.Click += callback;
                        return;
                    }
                }

                ToolStripMenuItem menuItemLevel2 = GetMenuItem(menuItemLevel1, textLevel2, textLevel3, callback);
                                                   GetMenuItem(menuItemLevel2, textLevel3, null, callback);
            }
        }

        private ToolStripMenuItem GetMenuItem(ToolStripMenuItem parentMenu, string text, string nextText, EventHandler callback)
        {
            if (text == null)
                return null;
            if (parentMenu.DropDown == null)
                parentMenu.DropDown = new ToolStripDropDown();
            foreach (ToolStripMenuItem item in parentMenu.DropDown.Items)
                if (text == item.Text)
                    return item;
            ToolStripMenuItem menuItem = new ToolStripMenuItem(text);
            parentMenu.DropDown.Items.Add(menuItem);
            if ((nextText == null) || (nextText == string.Empty))
                menuItem.Click += callback;
            return menuItem;
        }

        private delegate void AddMenuItemCallback(string text, string mainMenuItemText, string subMenuItemText, EventHandler callback);

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Core.Stop();
        }

        public void Init() { }
        public void Stop() { }
    }
}
