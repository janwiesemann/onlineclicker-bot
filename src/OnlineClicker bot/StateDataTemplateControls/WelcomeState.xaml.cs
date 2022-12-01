using Markdig;
using Markdig.Wpf;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xaml;

namespace OnlineClicker_bot.StateDataTemplateControls
{
    /// <summary>
    /// Interaktionslogik für WelcomeState.xaml
    /// </summary>
    public partial class WelcomeState : UserControl
    {
        public WelcomeState()
        {
            InitializeComponent();
        }

        protected override async void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            try
            {
                string markdown;
                using (WebClient client = new WebClient())
                    markdown = await client.DownloadStringTaskAsync("https://raw.githubusercontent.com/janwiesemann/onlineclicker-bot/main/README.MD");

                //Pretty much the sample code from https://github.com/Kryptos-FR/markdig.wpf/blob/master/src/Markdig.Xaml.SampleApp/MainWindow.xaml.cs
                //Never used markdig.wpf before.

                string xaml = Markdig.Wpf.Markdown.ToXaml(markdown, new MarkdownPipelineBuilder().UseSupportedExtensions().Build());

                using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(xaml)))
                using (XamlXmlReader reader = new XamlXmlReader(stream, new XamlSchemaContext()))
                {
                    if (System.Windows.Markup.XamlReader.Load(reader) is FlowDocument document)
                        readmeDocumentViewer.Document = document;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
        }

        private void OpenHyperlink(object sender, ExecutedRoutedEventArgs e)
        {
            Process.Start(e.Parameter.ToString());
        }

        private class MyXamlSchemaContext : XamlSchemaContext
        {
            public override bool TryGetCompatibleXamlNamespace(string xamlNamespace, out string compatibleNamespace)
            {
                if (xamlNamespace.Equals("clr-namespace:Markdig.Wpf", StringComparison.Ordinal))
                {
                    compatibleNamespace = $"clr-namespace:Markdig.Wpf;assembly={Assembly.GetAssembly(typeof(Markdig.Wpf.Styles)).FullName}";
                    return true;
                }
                return base.TryGetCompatibleXamlNamespace(xamlNamespace, out compatibleNamespace);
            }
        }
    }
}