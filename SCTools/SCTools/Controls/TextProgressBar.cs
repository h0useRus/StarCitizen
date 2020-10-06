using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NSW.StarCitizen.Tools.Controls
{
    public enum ProgressBarDisplayMode
    {
        NoText,
        Percentage,
        CurrentProgress,
        CustomText,
        TextAndPercentage,
        TextAndCurrentProgress
    }

    public class TextProgressBar : ProgressBar
    {
        [Description("Font of the text on ProgressBar"), Category("Additional Options")]
        public Font TextFont { get; set; } = new Font(FontFamily.GenericSerif, 8);

        private SolidBrush _textColorBrush = (SolidBrush)SystemBrushes.ControlText;
        [Category("Additional Options")]
        public Color TextColor
        {
            get => _textColorBrush.Color;
            set
            {
                _textColorBrush.Dispose();
                _textColorBrush = new SolidBrush(value);
            }
        }

        private SolidBrush _progressColorBrush = (SolidBrush)SystemBrushes.Highlight;
        [Category("Additional Options"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public Color ProgressColor
        {
            get => _progressColorBrush.Color;
            set
            {
                _progressColorBrush.Dispose();
                _progressColorBrush = new SolidBrush(value);
            }
        }

        private ProgressBarDisplayMode _visualMode = ProgressBarDisplayMode.CurrentProgress;
        [Category("Additional Options"), Browsable(true)]
        public ProgressBarDisplayMode VisualMode
        {
            get => _visualMode;
            set
            {
                _visualMode = value;
                Invalidate();//redraw component after change value from VS Properties section
            }
        }

        private string _text = string.Empty;

        [Description("If it's empty, % will be shown"), Category("Additional Options"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public string CustomText
        {
            get => _text;
            set
            {
                _text = value;
                Invalidate();//redraw component after change value from VS Properties section
            }
        }

        private string TextToDraw()
        {
            string text = CustomText;
            switch (VisualMode)
            {
                case (ProgressBarDisplayMode.Percentage):
                    text = GetPercentageString();
                    break;
                case (ProgressBarDisplayMode.CurrentProgress):
                    text = GetCurrentProgressString();
                    break;
                case (ProgressBarDisplayMode.TextAndCurrentProgress):
                    text = $"{CustomText}: {GetCurrentProgressString()}";
                    break;
                case (ProgressBarDisplayMode.TextAndPercentage):
                    text = $"{CustomText}: {GetPercentageString()}";
                    break;
            }

            return text;
        }
        private string GetPercentageString() => $"{(int)((float)Value - Minimum) / ((float)Maximum - Minimum) * 100 } %";
        private string GetCurrentProgressString() => $"{Value}/{Maximum}";

        public TextProgressBar()
        {
            Value = Minimum;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            DrawProgressBar(g);
            DrawStringIfNeeded(g);
        }

        private void DrawProgressBar(Graphics g)
        {
            var rect = ClientRectangle;
            ProgressBarRenderer.DrawHorizontalBar(g, rect);
            rect.Inflate(-3, -3);
            if (Value > 0)
            {
                var clip = new Rectangle(rect.X, rect.Y, (int)Math.Round(((float)Value / Maximum) * rect.Width), rect.Height);
                g.FillRectangle(_progressColorBrush, clip);
            }
        }

        private void DrawStringIfNeeded(Graphics g)
        {
            if (VisualMode != ProgressBarDisplayMode.NoText)
            {
                string text = TextToDraw();
                var len = g.MeasureString(text, TextFont);
                var location = new Point(((Width / 2) - (int)len.Width / 2), ((Height / 2) - (int)len.Height / 2));
                g.DrawString(text, TextFont, _textColorBrush, location);
            }
        }

        public new void Dispose()
        {
            _textColorBrush.Dispose();
            _progressColorBrush.Dispose();
            base.Dispose();
        }
    }
}