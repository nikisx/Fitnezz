using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Services.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitnezz.Web.Web.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IChatService chatService;
        private readonly IUsersService usersService;

        public ChatController(IChatService chatService, IUsersService usersService)
        {
            this.chatService = chatService;
            this.usersService = usersService;
        }

        public async Task<IActionResult> PrivateChat(string id)
        {//TODO If the chat dosent exist create it and set its id to trainer id plus user id

            var chatId = id;

            if (!this.chatService.ChatExist(chatId))
            {
                chatId = id + this.usersService.GetUserByUserName(this.User.Identity.Name).Id;
                if (!this.chatService.ChatExist(chatId))
                {
                    await this.chatService.CreateChat(chatId);
                }
            }

            var viewModel = this.chatService.GetChat(chatId);
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(string chatId, string message)
        {
            var username = this.User.Identity.Name;
            await this.chatService.CreateMessage(chatId, message, username);
            return this.RedirectToAction("PrivateChat", new { id = chatId });
        }
    }
}
