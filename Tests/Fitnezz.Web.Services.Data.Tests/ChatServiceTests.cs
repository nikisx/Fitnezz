using System.Linq;

namespace Fitnezz.Web.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Fitnezz.Web.Data.Common.Repositories;
    using Fitnezz.Web.Data.Models;
    using Moq;
    using Xunit;

    public class ChatServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Chat>> chatRepository;
        private readonly Mock<IDeletableEntityRepository<Message>> messageRepository;
        private readonly List<Chat> dbChats;
        private readonly List<Message> dbMessages;

        public ChatServiceTests()
        {
            this.dbChats = new List<Chat>();
            this.dbMessages = new List<Message>();
            this.chatRepository = new Mock<IDeletableEntityRepository<Chat>>();
            this.messageRepository = new Mock<IDeletableEntityRepository<Message>>();
        }

        [Fact]
        public async Task CreateMessageTest()
        {
            this.messageRepository.Setup(x => x.AddAsync(It.IsAny<Message>())).Callback((Message message) => dbMessages.Add(message));
            var service = new ChatService(this.chatRepository.Object, this.messageRepository.Object);

            await service.CreateMessage("1","Test", "TestUser");

            Assert.Single(this.dbMessages);
        }

        [Fact]
        public async Task CreateChatTest()
        {
            this.chatRepository.Setup(x => x.AddAsync(It.IsAny<Chat>())).Callback((Chat chat) => dbChats.Add(chat));
            var service = new ChatService(this.chatRepository.Object, this.messageRepository.Object);

            await service.CreateChat("1");

            Assert.Single(this.dbChats);
        }

        [Fact]
        public async Task ChatExistTest()
        {
            this.chatRepository.Setup(x => x.AddAsync(It.IsAny<Chat>())).Callback((Chat chat) => this.dbChats.Add(chat));
            this.chatRepository.Setup(x => x.All()).Returns(this.dbChats.AsQueryable());
            var service = new ChatService(this.chatRepository.Object, this.messageRepository.Object);

            await service.CreateChat("1");
            var chat = service.GetChat("1");

            Assert.NotNull(chat);
        }

        [Fact]
        public void ChatDosentExistTest()
        {
            this.chatRepository.Setup(x => x.AddAsync(It.IsAny<Chat>())).Callback((Chat chat) => this.dbChats.Add(chat));
            this.chatRepository.Setup(x => x.All()).Returns(this.dbChats.AsQueryable());
            var service = new ChatService(this.chatRepository.Object, this.messageRepository.Object);

            var actual = service.ChatExist("1");

            Assert.False(actual);
        }
    }
}