using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RocketPOS.Services
{
    public class RawMaterialService : IRawMaterialService
    {
        private readonly IRawMaterialRepository _iRawMaterialRepository;

        public RawMaterialService(IRawMaterialRepository iRawMaterialRepository)
        {
            _iRawMaterialRepository = iRawMaterialRepository;
        }

        public int DeleteRawMaterial(int id)
        {
            return _iRawMaterialRepository.DeleteRawMaterial(id);
        }

        public RawMaterialModel GetRawMaterialById(int id)
        {
            return _iRawMaterialRepository.GetRawMaterialList().Where(x => x.Id == id).FirstOrDefault();
        }

        public List<RawMaterialModel> GetRawMaterialList()
        {
            return _iRawMaterialRepository.GetRawMaterialList();
        }

        public int InsertRawMaterial(RawMaterialModel rawMaterialModel)
        {
            return _iRawMaterialRepository.InsertRawMaterial(rawMaterialModel);
        }

        public int UpdateRawMaterial(RawMaterialModel rawMaterialModel)
        {
            return _iRawMaterialRepository.UpdateRawMaterial(rawMaterialModel);
        }
    }
}
