using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ShotoUAshotBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Замените на ваш токен  
            var botClient = new TelegramBotClient("8303370209:AAFSHUFdONwaPoub_0OLusxDitIs2TppA1Y");

            using var cts = new CancellationTokenSource();

            // Регистрируем обработчики  
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // все обновления  
            };

            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMe();
            Console.WriteLine($"Бот запущен @{me.Username}");
            Console.WriteLine("Нажмите Enter для остановки");
            Console.ReadLine();

          cts.Cancel();
        }

        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
                return;

            var chatId = update.Message.Chat.Id;
            var text = update.Message.Text.Trim().ToLower();

            if (text == "/start")
            {
                object value = await botClient.SendMessage(chatId,
                    "Привет! Я Шото у Ашота 🚗\n" +
                    "Я помогу с запросами по ремонту автомобиля.\n" +
                    "Напиши 'ремонт', чтобы начать.");
            }
            else if (text.Contains("ремонт"))
            {
                await botClient.SendMessage(chatId,
                    "Какая услуга вас интересует?\n" +
                    "1. Замена масла\n" +
                    "2. Диагностика двигателя\n" +
                    "3. Ремонт тормозной системы");
            }
            else if (text == "1" || text.Contains("масло"))
            {
                await botClient.SendMessage(chatId,
                    "Замена масла стоит 1500₽.");
            }
            else if (text == "2" || text.Contains("двигатель"))
            {
                await botClient.SendMessage(chatId,
                    "Диагностика двигателя — 2000₽.");
            }
            else if (text == "3" || text.Contains("тормоза"))
            {
                await botClient.SendMessage(chatId,
                    "Ремонт тормозной системы от 3000₽.");
            }
            else
            {
                await botClient.SendMessage(chatId,
                    "Извините, не понял запрос. Напишите 'ремонт' повторно.");
            }
        }

       static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ct)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiEx => $"Telegram API Error:\n[{apiEx.ErrorCode}]\n{apiEx.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }
}