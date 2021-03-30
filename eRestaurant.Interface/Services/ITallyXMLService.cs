using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Services
{
    public interface ITallyXMLService
    {
        void GenerateSalesVoucher(string fromDate, string toDate,int outletId, string path);
        void SerializeToXml<T>(T anyobject, string xmlFilePath);
    }
}
