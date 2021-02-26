using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface ISectionService
    {
         List<SectionModel> GetSectionList();

        SectionModel GetSectionById(int id);

        int InsertSection(SectionModel sectionModel);

        int UpdateSection(SectionModel sectionModel);

        int DeleteSection(int id);
    }
}
