using System.Windows;

namespace OnlineClicker_bot
{
    /// <summary>
    /// A ProxyObject for some DataBinding issues.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is necessary if you want to achieve <see cref="System.Windows.Data.RelativeSource" /> like binding on a different VisualTree. A <see cref="System.Windows.Controls.ToolTip" /> is one such example.
    /// </para>
    /// <para>I stole this code from <see href="https://code.4noobz.net/wpf-mvvm-proxy-binding/" /></para>
    /// </remarks>
    internal class DataContextProxy : Freezable
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("DataSource", typeof(object), typeof(DataContextProxy), new PropertyMetadata(null));

        public object DataSource
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        protected override Freezable CreateInstanceCore() => new DataContextProxy();
    }
}