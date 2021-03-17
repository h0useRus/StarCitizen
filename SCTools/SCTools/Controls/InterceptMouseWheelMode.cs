namespace NSW.StarCitizen.Tools.Controls
{
    public enum InterceptMouseWheelMode
    {
        /// <summary>MouseWheel always works (defauld behavior)</summary>
        Always,
        /// <summary>MouseWheel works only when mouse is over the (focused) control</summary>
        WhenMouseOver,
        /// <summary>MouseWheel never works</summary>
        Never
    }
}
