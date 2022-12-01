using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace OnlineClicker_bot
{
    internal class PollState : PollStateBase
    {
        public PollState(MainWindowViewModel mainWindowViewModel, int pollNumber, int numOfAnswers) : base(mainWindowViewModel, pollNumber)
        {
            PollAnswers = new PollAnswer[numOfAnswers];
            for (int i = 0; i < numOfAnswers; i++)
                PollAnswers[i] = new PollAnswer(i + 1);

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
            Timer_Tick(this, EventArgs.Empty);
        }

        private readonly DispatcherTimer timer;
        private RelayICommand _AddVoteCommand;
        private bool _BalanceModeIsEnabled;
        private bool _IsBalancing;
        private bool _IsExecuting;
        private int _TotalNumberOfVotes;

        public ICommand AddVoteCommand
        {
            get
            {
                if (_AddVoteCommand == null)
                    _AddVoteCommand = new RelayICommand(p => p is PollAnswer, async p =>
                    {
                        if (!(p is PollAnswer answer))
                            return;

                        try
                        {
                            await AddVoteToPollAnswer(answer);
                        }
                        catch (Exception)
                        { }
                    });

                return _AddVoteCommand;
            }
        }

        public bool BalanceModeIsEnabled
        {
            get
            {
                return _BalanceModeIsEnabled;
            }
            set
            {
                _BalanceModeIsEnabled = value;

                RaisePropertyChanged();
            }
        }

        public bool IsBalancing
        {
            get
            {
                return _IsBalancing;
            }
            private set
            {
                _IsBalancing = value;

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsLoading));
            }
        }

        public bool IsExecuting
        {
            get
            {
                return _IsExecuting;
            }
            private set
            {
                _IsExecuting = value;

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsLoading));
            }
        }

        public override bool IsLoading => base.IsLoading || IsExecuting || IsBalancing;

        public PollAnswer[] PollAnswers { get; }

        public int TotalNumberOfVotes
        {
            get
            {
                return _TotalNumberOfVotes;
            }
            set
            {
                _TotalNumberOfVotes = value;

                RaisePropertyChanged();
            }
        }

        public override void Start()
        {
            base.Start();

            timer.Start();
        }

        public override void Stop()
        {
            base.Stop();

            timer.Stop();
        }

        private async Task AddVotesToPollAnswer(PollAnswer answer, Predicate<PollAnswer> runUntillCondition)
        {
            while (runUntillCondition(answer))
                await AddVoteToPollAnswer(answer);
        }

        private async Task AddVoteToPollAnswer(PollAnswer answer)
        {
            await SendRequestAndGetResponse(RequestType.Vote, answer.AnswerNumber);
            answer.Value++;
            TotalNumberOfVotes++;
        }

        private async Task BalanceAsync(int maxNumberOfVotes)
        {
            IsBalancing = true;
            try
            {
                for (int i = 0; i < PollAnswers.Length; i++)
                    await AddVotesToPollAnswer(PollAnswers[i], a => maxNumberOfVotes > a.Value);
            }
            finally
            {
                IsBalancing = false;
            }
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (IsExecuting) //Stop, if one execution is running. No need to check for Thread safe access or locking, since all operation are executed on the main UI Thread.
                return;

            IsExecuting = true;
            try
            {
                List<int> res = await SendRequestAndGetResponse(RequestType.GetResults);
                if (res.Count != PollAnswers.Length + 1)
                    return;

                TotalNumberOfVotes = res[0];

                int maxNumberOfVotes = 0;
                for (int i = 0; i < PollAnswers.Length; i++)
                {
                    PollAnswers[i].Value = res[i + 1];

                    if (PollAnswers[i].Value > maxNumberOfVotes)
                        maxNumberOfVotes = PollAnswers[i].Value;
                }

                if (BalanceModeIsEnabled)
                    await BalanceAsync(maxNumberOfVotes);
            }
            catch (Exception)
            { }
            finally
            {
                IsExecuting = false;
            }
        }
    }
}