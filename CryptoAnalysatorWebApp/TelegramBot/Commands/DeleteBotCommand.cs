﻿using CryptoAnalysatorWebApp.TelegramBot.Commands.Common;
using CryptoAnalysatorWebApp.TradeBots;
using CryptoAnalysatorWebApp.TradeBots.Common;
using CryptoAnalysatorWebApp.TradeBots.Common.Objects;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoAnalysatorWebApp.TelegramBot.Commands {
    public class DeleteBotCommand : CommonCommand {
        public override string Name { get; } = "deleteBot";

        public override void Execute(Message message, TelegramBotClient client, string channelId = null) {
            var chatId = message.Chat.Id;

            string market = GetAuthData(message, client, chatId);
            if (market != "bittrex") {
                client.SendTextMessageAsync(chatId, "You can have bots only on bittrex");
                return;
            }

            bool deleted = TradeBotsStorage<ResponseWrapper>.DeleteTradeBot(chatId, "bittrex");
            if (deleted) {
                client.SendTextMessageAsync(chatId, "[DELETION] Your trade bot on bittrex will be deleted after trades are stopped");
            } else {
                client.SendTextMessageAsync(chatId, "You don't have trade bot on bittrex");
            }
        }

        private string GetAuthData(Message message, TelegramBotClient client, long chatId) {
            string[] splitedWords = message.Text.Split(' ');
            if (splitedWords.Length < 2) {
                client.SendTextMessageAsync(chatId, "Provide crypto market, please");
                return "";
            }

            return splitedWords[1];
        }
    }
}