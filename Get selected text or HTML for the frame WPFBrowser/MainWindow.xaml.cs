using DotNetBrowser.DOM;
using DotNetBrowser.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Get_selected_text_or_HTML_for_the_frame_WPFBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ManualResetEvent resetEvent = new ManualResetEvent(false);
            FinishLoadingFrameHandler listener = new FinishLoadingFrameHandler((object sender1, FinishLoadingEventArgs e1) =>
            {
                if (e1.IsMainFrame)
                {
                    resetEvent.Set();
                }
            });
            browserView.Browser.FinishLoadingFrameEvent += listener;
            try
            {
                browserView.Browser.LoadURL("http://frame.free.nanoquant.ru/");
                resetEvent.WaitOne(new TimeSpan(0, 0, 45));
            }
            finally
            {
                browserView.Browser.FinishLoadingFrameEvent -= listener;
            }


           
            
        }

        private void btnSelectedText_Click(object sender, RoutedEventArgs e)
        {
            label.Content = browserView.Browser.GetSelectedText();
        }

        private void btnSelectedHtml_Click(object sender, RoutedEventArgs e)
        {
            label.Content = "Frames count: " + browserView.Browser.GetFramesIds().Count;

            DOMDocument document = browserView.Browser.GetDocument();

            int i = 0;

            foreach (long frameId in browserView.Browser.GetFramesIds()) {
                DOMDocument frameDocument = browserView.Browser.GetDocument(frameId);
                i++;
                textBlock.Text += "\n\n" + "Frame #" + i + '\n' + frameDocument.DocumentElement.InnerHTML;
            }
        }
    }
}
