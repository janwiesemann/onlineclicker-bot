using MahApps.Metro.Controls;
using System;

namespace OnlineClicker_bot
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PollNumberTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                viewModel.FullReloadCommand.Execute(null);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // try { HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, OnlineClickerClickerPHPURI);

            // MultipartFormDataContent content = new MultipartFormDataContent(); request.Content = content;

            // content.Add(new StringContent(pollID.ToString()), "pollNumber"); content.Add(new StringContent(awnserNumber.ToString()), "answersNumber"); content.Add(new
            // StringContent(((int)type).ToString()), "requestType");

            // HttpResponseMessage response = httpClient.Send(request); string responseString = response.Content.ReadAsStringAsync().Result;

            // const string successColon = "Success:"; if (!responseString.StartsWith(successColon, StringComparison.OrdinalIgnoreCase)) throw new Exception(responseString);

            // responseString = responseString.Substring(successColon.Length);

            // List<int> ret = new List<int>(4); foreach (string item in responseString.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)) { if (int.TryParse(item, out
            // int i)) ret.Add(i); } return ret;

            // }
        }
    }
}