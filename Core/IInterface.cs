using System;

namespace OpenWealth
{
    public interface IInterface
    {
        System.Windows.Forms.Form GetMainForm();
        void AddMenuItem(string text, string mainMenuItemText, string subMenuItemText, EventHandler callback);
    }
}
