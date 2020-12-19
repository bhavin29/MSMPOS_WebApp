using RocketPOS.Interface.Services;
using RocketPOS.Interface.Repository;
using RocketPOS.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace RocketPOS.Services
{
    public class CardTerminalService : ICardTerminalService
    {
        private readonly ICardTerminalRepository _IcardTerminalRepository;

        public CardTerminalService(ICardTerminalRepository iCardTerminalRepository)
        {
            _IcardTerminalRepository = iCardTerminalRepository;
        }

        public CardTerminalModel GetCardTerminalById(int cardTerminalId)
        {
            return _IcardTerminalRepository.GetCardTerminalList().Where(x => x.Id == cardTerminalId).FirstOrDefault();
        }

        public int InsertCardTerminal(CardTerminalModel cardTerminalModel)
        {
            return _IcardTerminalRepository.InsertCardTerminal(cardTerminalModel);
        }

        public int UpdateCardTerminal(CardTerminalModel cardTerminalModel)
        {
            return _IcardTerminalRepository.UpdateCardTerminal(cardTerminalModel);
        }

        public int DeleteCardTerminal(int cardTerminalId)
        {
            return _IcardTerminalRepository.DeleteCardTerminal(cardTerminalId);
        }

        public List<CardTerminalModel> GetCardTerminalList()
        {
            return _IcardTerminalRepository.GetCardTerminalList();
        }
    }
}
