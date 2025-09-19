using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using UsingEventAggregator.Core;

namespace ModuleB.ViewModels
{
    public class MessageListViewModel : BindableBase
    {
        MessageSentEvent _event;

        IEventAggregator _ea;

        private ObservableCollection<string> _messages;
        

        public ObservableCollection<string> Messages
        {
            get { return _messages; }
            set { SetProperty(ref _messages, value); }
        }


        private bool _isSubscribed = true;
        public bool IsSubscribed 
        { 
            get { return _isSubscribed; }
            set 
            {
                SetProperty(ref _isSubscribed, value);

                HandleSubscribe(_isSubscribed);
            } 
        }

        
        public MessageListViewModel(IEventAggregator ea)
        {
            _ea = ea;
            Messages = new ObservableCollection<string>();

            _event = ea.GetEvent<MessageSentEvent>();

            HandleSubscribe(true);

            //_ea.GetEvent<MessageSentEvent>().Subscribe(MessageReceived,
            //   ThreadOption.PublisherThread, false,
            //   message => message.Contains("Keyword") || message.Contains("Sent"));
        }

        SubscriptionToken _token;


        private void HandleSubscribe(bool isSubscribed)
        {
            if (_isSubscribed)
               _token = _event.Subscribe(MessageReceived);
            else
                _event.Unsubscribe(_token);

        }



        private void MessageReceived(string message)
        {
            Messages.Add(message);
        }
    }
}
