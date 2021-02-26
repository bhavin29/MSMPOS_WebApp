using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface IRawMaterialService
    {
        List<RawMaterialModel> GetRawMaterialList();

        RawMaterialModel GetRawMaterialById(int id);

        int InsertRawMaterial(RawMaterialModel rawMaterialModel);

        int UpdateRawMaterial(RawMaterialModel rawMaterialModel);

        int DeleteRawMaterial(int id);
    }
}
