using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IStoreService
    {
        List<StoreModel> GetStoreList();

        StoreModel GetStoreById(int storeId);

        int InsertStore(StoreModel storeModel);

        int UpdateStore(StoreModel storeModel);

        int DeleteStore(int storeId);
    }
}
