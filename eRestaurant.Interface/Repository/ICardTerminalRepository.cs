using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface ICardTerminalRepository
    {
        List<CardTerminalModel> GetCardTerminalList();

        int InsertCardTerminal(CardTerminalModel CardTerminalModel);

        int UpdateCardTerminal(CardTerminalModel CardTerminalModel);

        int DeleteCardTerminal(int cardTerminalID);

    }
}
