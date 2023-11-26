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
        private SimpleContainer _container;
        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM, SimpleContainer container)
        {           
            _salesVM = salesVM;
            _events = events;
            _events.Subscribe(this);

            _container = container;
            ActivateItem(_container.GetInstance<LoginViewModel>());
        }

       public  void Handle(LogOnEvent message)
        {
            ActivateItem(_salesVM);
        }
    }
}
