using System.Globalization;
using System.Windows.Forms;

namespace NSW.StarCitizen.Tools.Helpers
{
    public static class RtlAwareMessageBox
    {
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
            => Show(text, caption, buttons, icon, MessageBoxDefaultButton.Button1, 0);

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
            => Show(text, caption, buttons, icon, defaultButton, 0);

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
            {
                options |= MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign;
            }
            return MessageBox.Show(text, caption, buttons, icon, defaultButton, options);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
            => Show(owner, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, 0);

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
            => Show(owner, text, caption, buttons, icon, defaultButton, 0);

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            if ((owner is Control control && control.RightToLeft == RightToLeft.Yes) ||
                CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
            {
                options |= MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign;
            }
            return MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options);
        }
    }
}
