using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RocketPOS.Services
{
    public class SectionService : ISectionService
    {
        private readonly ISectionRepository _iSectionRepository;

        public SectionService(ISectionRepository iSectionRepository)
        {
            _iSectionRepository = iSectionRepository;
        }

        public int DeleteSection(int id)
        {
            return _iSectionRepository.DeleteSection(id);
        }

        public SectionModel GetSectionById(int id)
        {
            return _iSectionRepository.GetSectionList().Where(x => x.Id == id).FirstOrDefault();
        }

        public List<SectionModel> GetSectionList()
        {
            return _iSectionRepository.GetSectionList();
        }

        public int InsertSection(SectionModel sectionModel)
        {
            return _iSectionRepository.InsertSection(sectionModel);
        }

        public int UpdateSection(SectionModel sectionModel)
        {
            return _iSectionRepository.UpdateSection(sectionModel);
        }
    }
}
