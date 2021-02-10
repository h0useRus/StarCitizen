using System;
using System.ComponentModel;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Helpers;

namespace NSW.StarCitizen.Tools.Controls
{
    public class NumericUpDownEx : NumericUpDown
    {
        [DefaultValue(typeof(InterceptMouseWheelMode), "Always")]
        [Category("Behavior")]
        [Description("Enables MouseWheel only under certain conditions.")]
        public InterceptMouseWheelMode InterceptMouseWheel { get; set; } = InterceptMouseWheelMode.Always;

        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Enables validate text during editing.")]
        public bool ValidateTextDuringEditing { get; set; } = true;

        public enum InterceptMouseWheelMode
        {
            /// <summary>MouseWheel always works (defauld behavior)</summary>
            Always,
            /// <summary>MouseWheel works only when mouse is over the (focused) control</summary>
            WhenMouseOver,
            /// <summary>MouseWheel never works</summary>
            Never
        }

        // these events will be raised correctly, when mouse enters on the textbox
        public new event EventHandler<EventArgs>? MouseEnter;
        public new event EventHandler<EventArgs>? MouseLeave;

        private readonly TextBox _textbox;
        private readonly Control _upDownButtons;
        private bool _mouseOver;

        public NumericUpDownEx() : base()
        {
            _upDownButtons = Controls[0];
            if (_upDownButtons == null || _upDownButtons.GetType().FullName != "System.Windows.Forms.UpDownBase+UpDownButtons")
                throw new ArgumentNullException(GetType().FullName + ": Can't find internal UpDown buttons field.");
            if (Controls[1] is TextBox textBox)
                _textbox = textBox;
            else
                throw new ArgumentNullException(GetType().FullName + ": Can't find internal TextBox field.");
            _textbox.MouseEnter += OnMouseEnterLeave;
            _textbox.MouseLeave += OnMouseEnterLeave;
            _upDownButtons.MouseEnter += OnMouseEnterLeave;
            _upDownButtons.MouseLeave += OnMouseEnterLeave;
            base.MouseEnter += OnMouseEnterLeave;
            base.MouseLeave += OnMouseEnterLeave;
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (!ValidateTextDuringEditing)
            {
                base.ValidateEditText();
            }
            else if (UserEdit)
            {
                base.UpdateEditText();
            }
        }

        protected override void ValidateEditText()
        {
            if (ValidateTextDuringEditing)
            {
                base.ValidateEditText();
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
                            break;
                        case InterceptMouseWheelMode.Never:
                            // kill the message
                            return;
                    }
                    break;
                default:
                    base.WndProc(ref message);
                    break;
            }
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
