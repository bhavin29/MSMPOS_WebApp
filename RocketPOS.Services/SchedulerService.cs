using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Interface.Services;
using RocketPOS.Interface.Repository;

namespace RocketPOS.Services
{
    public class SchedulerService : ISchedulerService
    {
        private readonly ISchedulerRepository _schedulerRepository;

        public SchedulerService(ISchedulerRepository schedulerRepository)
        {
            _schedulerRepository = schedulerRepository;
        }

        public int SalesPOSInventorySync()
        {
            return _schedulerRepository.SalesPOSInventorySync();

        }
    }
}
