using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlineClicker_bot
{
    internal abstract class PollStateBase : StateBase
    {
        public PollStateBase(MainWindowViewModel mainWindowViewModel, int pollNumber)
        {
            MainWindowViewModel = mainWindowViewModel;
            PollNumber = pollNumber;

            httpClient = new HttpClient();
        }

        private bool _IsSendingRequest;
        private HttpClient httpClient;

        public override bool IsLoading => base.IsLoading || IsSendingRequest;

        public bool IsSendingRequest
        {
            get
            {
                return _IsSendingRequest;
            }
            private set
            {
                _IsSendingRequest = value;

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsLoading));
            }
        }

        public MainWindowViewModel MainWindowViewModel { get; }

        public int PollNumber { get; }

        protected Task<List<int>> SendRequestAndGetResponse(RequestType type) => SendRequestAndGetResponse(type, 1);

        protected async Task<List<int>> SendRequestAndGetResponse(RequestType type, int awnserNumber)
        {
            IsSendingRequest = true;
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://onlineclicker.org/clicker.php");

                MultipartFormDataContent content = new MultipartFormDataContent();
                request.Content = content;

                content.Add(new StringContent(PollNumber.ToString()), "pollNumber");
                content.Add(new StringContent(awnserNumber.ToString()), "answersNumber");
                content.Add(new StringContent(((int)type).ToString()), "requestType");

                HttpResponseMessage response = await httpClient.SendAsync(request);
                string responseString = await response.Content.ReadAsStringAsync();

                const string successColon = "Success:";
                if (!responseString.StartsWith(successColon, StringComparison.OrdinalIgnoreCase))
                    throw new Exception(responseString);

                responseString = responseString.Substring(successColon.Length);

                List<int> ret = new List<int>(4);
                foreach (string item in responseString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (int.TryParse(item, out int i))
                        ret.Add(i);
                }
                return ret;
            }
            finally
            {
                IsSendingRequest = false;
            }
        }
    }
}