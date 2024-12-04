using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UtilityBot.Controllers;
using UtilityBot.Models;
using Telegram.Bot.Polling;



    public class Bot : BackgroundService
    {
        private readonly ILogger<Bot> _logger;
        private readonly ITelegramBotClient _botClient;
        private readonly Dictionary<long, UserState> _userStates;
        private readonly MainMenuController _mainMenuController;
        private readonly SymbolCounterController _symbolCounterController;
        private readonly NumberSumController _numberSumController;

        public Bot(ILogger<Bot> logger, ITelegramBotClient botClient)
        {
            _logger = logger;
            _botClient = botClient;
            _userStates = new Dictionary<long, UserState>();
            _mainMenuController = new MainMenuController(_botClient);
            _symbolCounterController = new SymbolCounterController(_botClient);
            _numberSumController = new NumberSumController(_botClient);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            _botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cts.Token);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                if (update.Message is not null && update.Message.Type == MessageType.Text)
                {
                    var chatId = update.Message.Chat.Id;
                    var message = update.Message;

                    if (!_userStates.ContainsKey(chatId))
                    {
                        _userStates.Add(chatId, new UserState());
                    }

                    var userState = _userStates[chatId];

                    switch (message.Text)
                    {
                        case "/start":
                            await StartCommandReceived(botClient, message);
                            break;
                        default:
                            if (userState.CurrentFunction == "symbols")
                            {
                                await _symbolCounterController.CountSymbolsAsync(message, userState);
                            }
                            else if (userState.CurrentFunction == "numbers")
                            {
                                await _numberSumController.SumNumbersAsync(message, userState);
                            }
                            break;
                    }
                }
                else if (update.CallbackQuery is not null)
            {
                var callbackQuery = update.CallbackQuery;
                var data = callbackQuery.Data;
                var chatId = callbackQuery.Message.Chat.Id;

                if (_userStates.TryGetValue(chatId, out var userState))
                {
                    if (data == "Подсчитать символы")
                    {
                        userState.CurrentFunction = "symbols";
                        await _botClient.SendMessage(chatId, "Введите текст для подсчета символов");
                    }
                    else if (data == "Подсчитать числа")
                    {
                        userState.CurrentFunction = "numbers";
                        await _botClient.SendMessage(chatId, "Введите числа через пробел для суммирования");
                    }
                }
            }
        }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обработки обновления: {0}", ex.Message);
            }
        }

        private async Task StartCommandReceived(ITelegramBotClient botClient, Message message)
        {
            var chatId = message.Chat.Id;
            if (_userStates.ContainsKey(chatId))
            {
                _userStates.Remove(chatId);
            }

            _userStates.Add(chatId, new UserState());

            await _mainMenuController.SendMainMenuAsync(message);
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => "Telegram API Error:\n" + apiRequestException.ErrorCode + "\n" + apiRequestException.Message,
                _ => exception.ToString()
            };

            _logger.LogError(ErrorMessage);
            return Task.CompletedTask;
        }
    }
