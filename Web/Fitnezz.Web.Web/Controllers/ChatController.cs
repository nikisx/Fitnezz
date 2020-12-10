using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Common;
using Fitnezz.Web.Data.Models;
using Fitnezz.Web.Services.Data;
using Fitnezz.Web.Web.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Fitnezz.Web.Web.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IChatService chatService;
        private readonly IUsersService usersService;
        private readonly IHubContext<ChatHub> chat;

        public ChatController(IChatService chatService, IUsersService usersService, IHubContext<ChatHub> chat)
        {
            this.chatService = chatService;
            this.usersService = usersService;
            this.chat = chat;
        }

        public async Task<IActionResult> PrivateChat(string id)
        {

            var chatId = id;

            if (!this.chatService.ChatExist(chatId))
            {
                if (this.User.IsInRole(GlobalConstants.TrainerRoleName))
                {
                    chatId = this.usersService.GetTrainer(this.User.Identity.Name).Id + id;
                }
                else
                {
                    chatId = id + this.usersService.GetUserByUserName(this.User.Identity.Name).Id;
                }

                if (!this.chatService.ChatExist(chatId))
                {
                    await this.chatService.CreateChat(chatId);
                }
            }

            var viewModel = this.chatService.GetChat(chatId);
            return this.View(viewModel);
        }

        [HttpPost("[controller]/[action]")]
        public async Task<IActionResult> CreateMessage(string chatId, string message, string connectionId)
        {
            var username = this.User.Identity.Name;
            var messageToSent = await this.chatService.CreateMessage(chatId, message, username);
            await this.chat.Groups.AddToGroupAsync(connectionId, chatId);
            await this.chat.Clients.Group(chatId).SendAsync("RecieveMessage", new Message()
            {
                UserName = messageToSent.UserName,
                ChatId = messageToSent.ChatId,
                Content = messageToSent.Content,
                Time = DateTime.Parse(messageToSent.Time.ToString("dd/MM/yyyy hh:mm:ss")),
            });
            return this.RedirectToAction("PrivateChat", new { id = chatId });
        }

        [HttpPost("[controller]/[action]/{connectionId}/{chatId}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string chatId)
        {
            await this.chat.Groups.AddToGroupAsync(connectionId, chatId);
            return this.Ok();
        }
    }
}
