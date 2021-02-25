using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface IRawMaterialRepository
    {
        List<RawMaterialModel> GetRawMaterialList();

        int InsertRawMaterial(RawMaterialModel rawMaterialModel);

        int UpdateRawMaterial(RawMaterialModel rawMaterialModel);

        int DeleteRawMaterial(int id);
    }
}
