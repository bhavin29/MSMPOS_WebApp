using RocketPOS.Interface.Services;
using RocketPOS.Interface.Repository;
using RocketPOS.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RocketPOS.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _iCustomerRepository;

        public CustomerService(ICustomerRepository iCustomerRepository)
        {
            _iCustomerRepository = iCustomerRepository;
        }

        public int DeleteCustomer(int customerId)
        {
            return _iCustomerRepository.DeleteCustomer(customerId);
        }

        public CustomerModel GetCustomerById(int customerId)
        {
            return _iCustomerRepository.GetCustomerList().Where(x => x.Id == customerId).FirstOrDefault();
        }

        public List<CustomerModel> GetCustomerList()
        {
            return _iCustomerRepository.GetCustomerList();
        }

        public int InsertCustomer(CustomerModel customerModel)
        {
            return _iCustomerRepository.InsertCustomer(customerModel);
        }

        public int UpdateCustomer(CustomerModel customerModel)
        {
            return _iCustomerRepository.UpdateCustomer(customerModel);
        }
    }
}
