using System;
using System.Collections.Generic;
using System.Text;
using xPowerPhoneApp.Interfaces;

namespace xPowerPhoneApp.ViewModels
{
    abstract internal class PageViewModel : BaseViewModel
    {

        protected IChangePage _pageChanger;

        /// <summary>
        /// Base class for view models
        /// </summary>
        /// <param name="pageChanger">The object that can Change page</param>
        protected PageViewModel(IChangePage pageChanger)
        {
            _pageChanger = pageChanger;
        }
    }
}
