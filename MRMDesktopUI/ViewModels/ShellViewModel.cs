using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MRMDesktopUI.EventModels;
using MRMDesktopUI.Library.Models;

namespace MRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private IEventAggregator _events;
        private SalesViewModel _salesVM;
        private ILoggedInUserModel _user;
        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM, ILoggedInUserModel loggedInUserModel)
        {           
            _salesVM = salesVM;
            _events = events;
            _events.Subscribe(this);
            _user = loggedInUserModel;

            //ActivateItem(_container.GetInstance<LoginViewModel>());
            // Better way 
            ActivateItem(IoC.Get<LoginViewModel>());
        }
        public bool IsLoggedIn
        {
            get
            {
                bool output = false;

                if (string.IsNullOrEmpty(_user.Token) == false)
                {
                    output = true;
                }
                return output;
            }

        }


        public void LogOut()
        {
            _user.LogOffUser();
            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
        public void ExitApplication()
        {
            TryClose();
        }

       public  void Handle(LogOnEvent message)
        {
            ActivateItem(_salesVM);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
