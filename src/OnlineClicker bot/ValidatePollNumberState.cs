using System;
using System.Collections.Generic;
using System.Threading;

namespace OnlineClicker_bot
{
    internal sealed class ValidatePollNumberState : PollStateBase
    {
        public ValidatePollNumberState(MainWindowViewModel mainWindowViewModel, int pollNumber) : base(mainWindowViewModel, pollNumber)
        {
            cancellationTokenSource = new CancellationTokenSource();
        }

        private readonly CancellationTokenSource cancellationTokenSource;

        public override async void Start()
        {
            base.Start();

            try
            {
                List<int> res = await SendRequestAndGetResponse(RequestType.GetResults);
                if (res == null || 1 > res.Count)
                    throw new Exception("Response was invalid! This poll does not have any results!");

                if (cancellationTokenSource.IsCancellationRequested)
                    return;

                MainWindowViewModel.CurrentState = new PollState(MainWindowViewModel, PollNumber, res.Count - 1);
            }
            catch (Exception ex)
            {
                if (!cancellationTokenSource.IsCancellationRequested)
                    MainWindowViewModel.CurrentState = new ErrorState(ex);
            }
        }

        public override void Stop()
        {
            base.Stop();

            cancellationTokenSource.Cancel();
        }
    }
}