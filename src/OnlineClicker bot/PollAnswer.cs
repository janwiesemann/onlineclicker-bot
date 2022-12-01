namespace OnlineClicker_bot
{
    internal class PollAnswer : NotifyBase
    {
        public PollAnswer(int answerNumber)
        {
            AnswerNumber = answerNumber;
        }

        private int _Value;
        public int AnswerNumber { get; }

        public int Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;

                RaisePropertyChanged();
            }
        }
    }
}