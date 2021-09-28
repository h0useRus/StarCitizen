using System;
using System.ComponentModel;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Helpers;

namespace NSW.StarCitizen.Tools.Controls
{
    public class ComboboxEx : ComboBox
    {
        [DefaultValue(typeof(InterceptMouseWheelMode), "Always")]
        [Category("Behavior")]
        [Description("Enables MouseWheel only under certain conditions.")]
        public InterceptMouseWheelMode InterceptMouseWheel { get; set; } = InterceptMouseWheelMode.Always;

        // these events will be raised correctly, when mouse enters on the textbox
        public new event EventHandler<EventArgs>? MouseEnter;
        public new event EventHandler<EventArgs>? MouseLeave;

        private bool _mouseOver;
        private bool _inWndProc;

        public ComboboxEx()
        {
            base.MouseEnter += OnMouseEnterLeave;
            base.MouseLeave += OnMouseEnterLeave;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                base.MouseEnter -= OnMouseEnterLeave;
                base.MouseLeave -= OnMouseEnterLeave;
            }
        }

        public new object DataSource
        {
            get => base.DataSource;
            set
            {
                base.DataSource = value;
                DetermineDropDownWidth();
            }
        }

        public new string DisplayMember
        {
            get => base.DisplayMember;
            set
            {
                base.DisplayMember = value;
                DetermineDropDownWidth();
            }
        }

        public new string ValueMember
        {
            get => base.ValueMember;
            set
            {
                base.ValueMember = value;
                DetermineDropDownWidth();
            }
        }

        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case WinApi.WM_MOUSEWHEEL:
                    switch (InterceptMouseWheel)
                    {
                        case InterceptMouseWheelMode.Always:
                            base.WndProc(ref message);
                            break;
                        case InterceptMouseWheelMode.WhenMouseOver:
                            if (_mouseOver)
                            {
                                base.WndProc(ref message);
                            }
                            else
                            {
                                WinApi.SendControlMessage(Parent, ref message, ref _inWndProc);
                            }
                            break;
                        case InterceptMouseWheelMode.Never:
                            WinApi.SendControlMessage(Parent, ref message, ref _inWndProc);
                            return;
                    }
                    break;
                default:
                    base.WndProc(ref message);
                    break;
            }
        }

        private void DetermineDropDownWidth()
        {
            int widestStringInPixels = 0;
            foreach (object o in Items)
            {
                string toCheck;
                if (string.IsNullOrEmpty(DisplayMember))
                {
                    toCheck = o.ToString();
                }
                else
                {
                    var pinfo = o.GetType().GetProperty(DisplayMember);
                    toCheck = pinfo.GetValue(o, null).ToString();
                }
                if (TextRenderer.MeasureText(toCheck, Font).Width > widestStringInPixels)
                {
                    widestStringInPixels = TextRenderer.MeasureText(toCheck, Font).Width;
                }
            }
            Width = widestStringInPixels + 24;
            DropDownWidth = widestStringInPixels + 15;
        }

        private void OnMouseEnterLeave(object sender, EventArgs e)
        {
            bool isOver = RectangleToScreen(ClientRectangle).Contains(MousePosition);
            if (_mouseOver != isOver)
            {
                _mouseOver = isOver;
                if (_mouseOver)
                {
                    MouseEnter?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MouseLeave?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
