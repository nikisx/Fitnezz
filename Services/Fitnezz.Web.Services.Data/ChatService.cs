using System;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Data.Common.Repositories;
using Fitnezz.Web.Data.Models;

namespace Fitnezz.Web.Services.Data
{
    public class ChatService : IChatService
    {
        private readonly IDeletableEntityRepository<Chat> chatRepository;
        private readonly IDeletableEntityRepository<Message> messageRepository;

        public ChatService(IDeletableEntityRepository<Chat> chatRepository, IDeletableEntityRepository<Message> messageRepository)
        {
            this.chatRepository = chatRepository;
            this.messageRepository = messageRepository;
        }

        public Chat GetChat(string id)
        {
            return this.chatRepository.All().Where(x => x.Id == id).Select(x => new Chat()
            {
                Id = x.Id,
                Messages = x.Messages.Where(a => a.ChatId == id).ToList(),
            }).FirstOrDefault();
        }

        public async Task<MessageViewModel> CreateMessage(string chatId, string content, string username)
        {
            var message = new Message()
            {
                ChatId = chatId,
                Content = content,
                UserName = username,
                Time = DateTime.Now,
            };

            await this.messageRepository.AddAsync(message);
            await this.messageRepository.SaveChangesAsync();
            var messageToReturn = new MessageViewModel()
            {
                ChatId = message.ChatId,
                Content = message.Content,
                Time = string.Format("{0:G}", message.Time),
                UserName = message.UserName,
            };
            return messageToReturn;
        }

        public bool ChatExist(string chatId)
        {
            return this.chatRepository.All().Any(x => x.Id == chatId);
        }

        public async Task CreateChat(string chatId)
        {
            var chat = new Chat()
            {
                Id = chatId,
            };

            await this.chatRepository.AddAsync(chat);
            await this.chatRepository.SaveChangesAsync();
        }
    }
}