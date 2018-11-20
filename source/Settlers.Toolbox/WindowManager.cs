using System;

using Settlers.Toolbox.Interfaces;
using Settlers.Toolbox.Views;

namespace Settlers.Toolbox
{
    public class WindowManager : IWindowManager
    {
        private ReadmeWindow _ReadmeWindow;

        public bool ReadmeWindowIsOpen => _ReadmeWindow != null;

        // https://stackoverflow.com/questions/25845689/opening-new-window-in-mvvm-wpf
        public void OpenReadmeWindow()
        {
            if (_ReadmeWindow == null)
            {
                _ReadmeWindow = new ReadmeWindow();
                _ReadmeWindow.Closed += ReadmeWindowOnClosed;
                _ReadmeWindow.Show();
            }
        }

        private void ReadmeWindowOnClosed(object sender, EventArgs e)
        {
            _ReadmeWindow = null;
        }
    }
}