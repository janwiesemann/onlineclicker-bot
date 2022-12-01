using System.Windows.Input;

namespace OnlineClicker_bot
{
    internal class MainWindowViewModel : NotifyBase
    {
        public MainWindowViewModel()
        {
            FullReloadCommand = new RelayICommand(p =>
            {
                if (PollNumber != null)
                    CurrentState = new ValidatePollNumberState(this, PollNumber.Value);
                else
                    CurrentState = null;
            }); //This will 'Reload' the whole state stack.
        }

        private StateBase _CurrentState;
        private int? _PollNumber;

        public StateBase CurrentState
        {
            get
            {
                if (_CurrentState == null)
                    _CurrentState = new WelcomeState();

                return _CurrentState;
            }
            set
            {
                _CurrentState?.Stop(); //Stop old stuff

                _CurrentState = value;
                RaisePropertyChanged();

                _CurrentState?.Start(); //Start new stuff
            }
        }

        public ICommand FullReloadCommand { get; }

        public ICommand OpenSettingsCommand { get; }

        public int? PollNumber
        {
            get
            {
                return _PollNumber;
            }
            set
            {
                _PollNumber = value;

                RaisePropertyChanged();
            }
        }
    }
}