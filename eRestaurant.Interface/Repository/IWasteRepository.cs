using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;
namespace RocketPOS.Interface.Repository
{
    public interface IWasteRepository
    {
        List<WasteListModel> GetWasteList();
        int InsertWaste(WasteModel purchaseModel);
        int UpdateWaste(WasteModel purchaseModel);
        int DeleteWaste(long WasteId);
        List<WasteDetailModel> GetWasteDetails(long purchaseId);
        List<WasteModel> GetWasteById(long purchaseId);
        int DeleteWasteDetails(long WasteDetailsId);
        long ReferenceNumber();
    }
}
