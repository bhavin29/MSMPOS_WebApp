using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IStoreRepository
    {
        List<StoreModel> GetStoreList();

        int InsertStore(StoreModel storeModel);

        int UpdateStore(StoreModel storeModel);

        int DeleteStore(int storeId);
    }
}
