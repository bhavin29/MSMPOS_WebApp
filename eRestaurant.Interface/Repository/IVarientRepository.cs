using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IVarientRepository
    {

        List<VarientModel> GetVarientList();

        int InsertVarient(VarientModel VarientModel);   

        int UpdateVarient(VarientModel VarientModel);

        int DeleteVarient(int VarientID);

    }
}
