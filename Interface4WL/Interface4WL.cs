using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace OpenWealth.WLProvider
{
    public class Interface4WL : IPlugin, IInterface
    {
        private static readonly ILog l = Core.GetLogger(typeof(Interface4WL).FullName);
        private Form mainForm;
        private object mainFormLocker = new object();
        private List<DynMenuItem> menuItemList = new List<DynMenuItem>();
        private ToolStripMenuItem openWealthItem;
        private System.Timers.Timer timer;

        public Interface4WL()
        {
            Core.SetGlobal("Interface", this);
            if (this.GetMainForm() == null)
            {
                this.timer = new System.Timers.Timer();
                this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
                this.timer.Interval = 10000;
                this.timer.Enabled = true;
            }
        }

        public void AddMenuItem(string textLevel1, string textLevel2, string textLevel3, EventHandler callback)
        {
            lock (this.mainFormLocker)
            {
                if (this.GetMainForm() == null)
                {
                    this.menuItemList.Add(new DynMenuItem(textLevel1, textLevel2, textLevel3, callback));
                }
                else
                {
                    this.AddMenuItem(this.GetMainForm(), textLevel1, textLevel2, textLevel3, callback);
                }
            }
        }

        private void AddMenuItem(Form form, string textLevel1, string textLevel2, string textLevel3, EventHandler callback)
        {
            if (form == null)
            {
                throw new ArgumentNullException("form");
            }
            if (textLevel1 == null)
            {
                throw new ArgumentNullException("textLevel1");
            }
            if (form.InvokeRequired)
            {
                AddMenuItemCallback addMenuItemCallback = new AddMenuItemCallback(this.AddMenuItem);
                form.Invoke(addMenuItemCallback, new object[] { form, textLevel1, textLevel2, textLevel3, callback });
            }
            else if (form.MainMenuStrip == null)
            {
                l.Error("form.MainMenuStrip == null");
            }
            else
            {
                if (this.openWealthItem == null)
                {
                    foreach (ToolStripMenuItem mainItem in form.MainMenuStrip.Items)
                    {
                        if (mainItem.Text == "O&penWealth")
                        {
                            this.openWealthItem = mainItem;
                            break;
                        }
                    }
                    if (this.openWealthItem == null)
                    {
                        int toolsIndex = 0;
                        while (toolsIndex < form.MainMenuStrip.Items.Count)
                        {
                            if (form.MainMenuStrip.Items[toolsIndex].Text == "W&orkspaces")
                            {
                                break;
                            }
                            toolsIndex++;
                        }

                        openWealthItem = new ToolStripMenuItem("O&penWealth");
                        form.MainMenuStrip.Items.Insert(toolsIndex, this.openWealthItem);
                    }
                }

                ToolStripMenuItem menuItem1 = GetMenuItem(openWealthItem, textLevel1, textLevel2, callback);
                ToolStripMenuItem menuItem2 = GetMenuItem(menuItem1, textLevel2, textLevel3, callback);
                                              GetMenuItem(menuItem2, textLevel3,       null, callback);

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

        private Form CheckForm(Form form)
        {
            if (form.InvokeRequired)
            {
                CheckFormCallback checkFormCallback = new CheckFormCallback(this.CheckForm);
                return (Form)form.Invoke(checkFormCallback, new object[] { form });
            }
            if (!form.IsMdiContainer || !form.Text.Contains("Wealth-Lab"))
                return null;

            foreach (DynMenuItem mi in this.menuItemList)
                this.AddMenuItem(form, mi.textLevel1, mi.textLevel2, mi.textLevel3, mi.callback);
            this.menuItemList.Clear();
            
            return form;
        }

        private Form FindMainForm()
        {
            for (int index = 0; index < Application.OpenForms.Count; index++)
            {
                try
                {
                    Form form = Application.OpenForms[index];
                    Form f = this.CheckForm(form);
                    if (f != null)
                    {
                        return f;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    l.Debug("ArgumentOutOfRangeException");
                }
            }
            return null;
        }

        public Form GetMainForm()
        {
            lock (this.mainFormLocker)
            {
                if (this.mainForm == null)
                {
                    this.mainForm = this.FindMainForm();
                }
                return this.mainForm;
            }
        }

        public void Init() { }
        public void Stop() { }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.GetMainForm() != null)
                this.timer.Enabled = false;
        }

        private delegate void AddMenuItemCallback(Form form, string text, string mainMenuItemText, string subMenuItemText, EventHandler callback);

        private delegate Form CheckFormCallback(Form form);

        private class DynMenuItem
        {
            public EventHandler callback;
            public string textLevel1;
            public string textLevel2;
            public string textLevel3;

            public DynMenuItem(string textLevel1, string textLevel2, string textLevel3, EventHandler callback)
            {
                this.textLevel1 = textLevel1;
                this.textLevel2 = textLevel2;
                this.textLevel3 = textLevel3;
                this.callback = callback;
            }
        }
    }
}