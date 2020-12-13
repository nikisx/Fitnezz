using System.Threading.Tasks;
using Fitnezz.Web.Data.Models;

namespace Fitnezz.Web.Services.Data
{
    public interface IChatService
    {
        Chat GetChat(string id);

        Task<MessageViewModel> CreateMessage(string chatId, string content, string username);

        bool ChatExist(string chatId);

        Task CreateChat(string chatId);
    }
}