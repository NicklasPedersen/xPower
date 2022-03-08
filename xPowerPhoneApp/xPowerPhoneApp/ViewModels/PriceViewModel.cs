using System;
using System.Collections.Generic;
using System.Text;
using xPowerPhoneApp.Repositorys.Interfaces;

namespace xPowerPhoneApp.ViewModels
{
    internal class PriceViewModel
    {
        private IPowerRepository _powerRepository;
        public PriceViewModel(IPowerRepository powerRepository) : base()
        {
            _powerRepository = powerRepository;
        }
    }
}
