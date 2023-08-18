using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace MRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        private LoginViewModel _loginVM;
        public ShellViewModel(LoginViewModel loginvm)
        {
             _loginVM = loginvm;
            ActivateItem(_loginVM);
        }
    }
}
