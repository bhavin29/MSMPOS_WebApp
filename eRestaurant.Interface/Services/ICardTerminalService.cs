using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;
using System.Collections.Generic;


namespace RocketPOS.Interface.Services
{
    public interface ICardTerminalService
    {
        CardTerminalModel GetCardTerminalById(int cardTErminalId);
        List<CardTerminalModel> GetCardTerminalList();

        int InsertCardTerminal(CardTerminalModel cardTerminlaModel);

        int UpdateCardTerminal(CardTerminalModel cardTerminlaModel);

        int DeleteCardTerminal(int cardTErminalId);
    }
}
