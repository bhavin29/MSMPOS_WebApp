using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface ISectionRepository
    {
        List<SectionModel> GetSectionList();

        int InsertSection(SectionModel sectionModel);

        int UpdateSection(SectionModel sectionModel);

        int DeleteSection(int id);
    }
}
