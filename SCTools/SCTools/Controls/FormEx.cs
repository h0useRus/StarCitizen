using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NSW.StarCitizen.Tools.Controls
{
    public class FormEx : Form
    {
        private static readonly Dictionary<Type, FormPropsStorage> _propsStorages = new Dictionary<Type, FormPropsStorage>();
        private FormPropsStorage? _propsStorage;

        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Restore window position, size and state from previous instance.")]
        public bool RestoreLocation { get; set; }

        private FormPropsStorage PropsStorage
        {
            get
            {
                if (_propsStorage != null)
                    return _propsStorage;
                return _propsStorage = GetFormPropsStorage();
            }
        }

        public FormEx()
        {
            Load += OnLoad;
            FormClosing += OnFormClosing;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Load -= OnLoad;
                FormClosing -= OnFormClosing;
                LocationChanged -= OnLocationChanged;
                SizeChanged -= OnSizeChanged;
            }
            base.Dispose(disposing);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            if (RestoreLocation)
            {
                PropsStorage.Restore(this);
            }
            LocationChanged += OnLocationChanged;
            SizeChanged += OnSizeChanged;
        }

        private void OnFormClosing(object sender, EventArgs e) => PropsStorage.StoreState(this);

        private void OnLocationChanged(object sender, EventArgs e) => PropsStorage.StoreBounds(this);

        private void OnSizeChanged(object sender, EventArgs e) => PropsStorage.StoreBounds(this);

        private FormPropsStorage GetFormPropsStorage()
        {
            var type = GetType();
            if (_propsStorages.TryGetValue(type, out var storage))
            {
                return storage;
            }
            var result = new FormPropsStorage();
            _propsStorages.Add(type, result);
            return result;
        }

        private sealed class FormPropsStorage
        {
            public FormWindowState WindowState { get; private set; }
            public int Left { get; private set; }
            public int Top { get; private set; }
            public int Width { get; private set; }
            public int Height { get; private set; }
            public bool StateSaved { get; private set; }
            public bool BoundsSaved { get; private set; }

            public void StoreState(Form form)
            {
                WindowState = form.WindowState;
                StateSaved = true;
            }

            public void StoreBounds(Form form)
            {
                if (form.WindowState == FormWindowState.Normal)
                {
                    Left = form.Location.X;
                    Top = form.Location.Y;
                    Width = form.Size.Width;
                    Height = form.Size.Height;
                    BoundsSaved = true;
                }
            }

            public void Restore(Form form)
            {
                if (BoundsSaved)
                {
                    var workArea = Screen.GetWorkingArea(new Rectangle(Left, Top, Width, Height));
                    form.Left = Clamp(Left, workArea.X, workArea.X + workArea.Width - Width);
                    form.Top = Clamp(Top, workArea.Y, workArea.Y + workArea.Height - Height);
                    form.Width = Width;
                    form.Height = Height;
                }
                if (StateSaved)
                {
                    form.WindowState = WindowState;
                }
            }

            private static int Clamp(int value, int min, int max) => value < min ? min : (value > max ? max : value);
        }
    }
}
