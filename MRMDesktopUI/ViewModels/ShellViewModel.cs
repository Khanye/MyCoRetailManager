using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MRMDesktopUI.EventModels;

namespace MRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private IEventAggregator _events;
        private SalesViewModel _salesVM;
        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM)
        {           
            _salesVM = salesVM;
            _events = events;
            _events.Subscribe(this);

            //ActivateItem(_container.GetInstance<LoginViewModel>());
            // Better way 
            ActivateItem(IoC.Get<LoginViewModel>());
        }

       public  void Handle(LogOnEvent message)
        {
            ActivateItem(_salesVM);
        }
    }
}
