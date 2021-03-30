using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using RocketPOS.Framework;
using RocketPOS.Models;
using RocketPOS.Interface.Services;
using RocketPOS.Framework;
    using RocketPOS.Interface.Repository.Reports;

namespace RocketPOS.Services
{
    public class TallyXMLService : ITallyXMLService
    {
        GeneralClass clsGeneral = new GeneralClass();
        private readonly IReportRepository _iReportRepository;

        public TallyXMLService(IReportRepository iReportRepository)
        {
            _iReportRepository = iReportRepository;
        }
        public void GenerateSalesVoucher(string fromDate, string toDate, int outletId,string path)
        {
            List<TallySetupModel> tallySetupModel = new List<TallySetupModel>();
            List<TallySalesVoucherModel> tallySalesVoucherModels = new List<TallySalesVoucherModel>();
            XmlDocument xmldoc = new XmlDocument();
            List<XmlDocument> xmldocVoucher = new List<XmlDocument>();

            tallySetupModel = _iReportRepository.GetTallySetup(outletId);
            tallySalesVoucherModels = _iReportRepository.GetSalesVoucherData(fromDate, toDate, outletId);
            int i = 1;

            foreach (var salesVoucher in tallySalesVoucherModels)
            {
                var clsSalesFields = new SalesFields();
                clsSalesFields.VoucherType = "Sales";
                clsSalesFields.VoucherUniqueID = salesVoucher.BillDate.Replace("/", "");
                clsSalesFields.VoucherNumber = tallySetupModel.Find(x => x.Keyname.Contains("BillPrefix")).LedgerName + salesVoucher.BillDate.Replace("/", "") + salesVoucher.TallyBillPostfix;
                clsSalesFields.VoucherDate = Convert.ToDateTime(salesVoucher.BillDate);
                clsSalesFields.PartyLedgerName = salesVoucher.TallyLedgerName;
                clsSalesFields.EffectiveDate = Convert.ToDateTime(salesVoucher.BillDate);
                clsSalesFields.IsInvoice = "No";
                clsSalesFields.VoucherNarration = "";

                var pcledgerParty = new ALLLedgerEntries();
                pcledgerParty.LedgerName = salesVoucher.TallyLedgerName;
                pcledgerParty.IsDeemedPositive = "Yes";
                pcledgerParty.LedgerFromItem = "No";
                pcledgerParty.RemoveZeroEntries = "No";
                pcledgerParty.IsPartyLedger = "Yes";
                pcledgerParty.Amount = Convert.ToDouble(salesVoucher.Sales) * -1;
                clsSalesFields.SalesAllLedgerEntriesList.Add(salesVoucher.TallyLedgerName, pcledgerParty);

                pcledgerParty = new ALLLedgerEntries();
                pcledgerParty.LedgerName = salesVoucher.TallyLedgerNamePark; ;
                pcledgerParty.IsDeemedPositive = "No";
                pcledgerParty.LedgerFromItem = "No";
                pcledgerParty.RemoveZeroEntries = "No";
                pcledgerParty.IsPartyLedger = "No";
                pcledgerParty.Amount = Convert.ToDouble(salesVoucher.CashSales);
                clsSalesFields.SalesAllLedgerEntriesList.Add(salesVoucher.TallyLedgerNamePark, pcledgerParty);

                pcledgerParty = new ALLLedgerEntries();
                pcledgerParty.LedgerName = tallySetupModel.Find(x => x.Keyname.Contains("ExemptedSales")).LedgerName;
                pcledgerParty.IsDeemedPositive = "No";
                pcledgerParty.LedgerFromItem = "No";
                pcledgerParty.RemoveZeroEntries = "No";
                pcledgerParty.IsPartyLedger = "No";
                pcledgerParty.Amount = Convert.ToDouble(salesVoucher.ExemptedSales);
                clsSalesFields.SalesAllLedgerEntriesList.Add(tallySetupModel.Find(x => x.Keyname.Contains("ExemptedSales")).LedgerName, pcledgerParty);

                pcledgerParty = new ALLLedgerEntries();
                pcledgerParty.LedgerName = tallySetupModel.Find(x => x.Keyname.Contains("OutputVAT ")).LedgerName;
                pcledgerParty.IsDeemedPositive = "No";
                pcledgerParty.LedgerFromItem = "No";
                pcledgerParty.RemoveZeroEntries = "No";
                pcledgerParty.IsPartyLedger = "No";
                pcledgerParty.Amount = Convert.ToDouble(salesVoucher.OutputVAT);
                clsSalesFields.SalesAllLedgerEntriesList.Add(tallySetupModel.Find(x => x.Keyname.Contains("OutputVAT")).LedgerName, pcledgerParty);

                //var pcBillalloc = new BillAllocation();
                //pcBillalloc.Name = "AccountID";
                //pcBillalloc.BillType = "New Ref";
                //pcBillalloc.Amount = Val("Total") * -1;
                //pcledgerParty.BillAllocationList.Add("PartyLedger", pcBillalloc);
                //clsSalesFields.SalesAllLedgerEntriesList.Add("PartyLedger", pcledgerParty);

                var SalesLedgerCount = new List<string>(new string[] {

                salesVoucher.TallyLedgerName,
                salesVoucher.TallyLedgerNamePark,
                tallySetupModel.Find(x => x.Keyname.Contains("ExemptedSales")).LedgerName,
                tallySetupModel.Find(x => x.Keyname.Contains("OutputVAT ")).LedgerName
                 });

                var clsSalesVoucher = new TallySaleVoucher();
                xmldocVoucher.Add(clsSalesVoucher.CreateSaleVoucherXML(clsSalesFields, SalesLedgerCount, tallySetupModel.Find(x => x.Keyname.Contains("CompanyName")).LedgerName));

                i += 1;
            }
            SerializeToXml(xmldocVoucher, path);
        }

        public void SerializeToXml<T>(T anyobject, string xmlFilePath)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(anyobject.GetType());

            using (StreamWriter writer = new StreamWriter(xmlFilePath))
            {
                xmlSerializer.Serialize(writer, anyobject);
            }
        }
    }
}
