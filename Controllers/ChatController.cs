using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HelloFuture.Helpers;
using HelloFuture.Hubs;
using HelloFuture.Models;
using HelloFuture.Data;

namespace HelloFuture.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.Customer + "," + Roles.Agent)]
    public class ChatController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(UserManager<IdentityUser> userManager, ApplicationDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            var CurrrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Model = _context.Headers.Where(m => m.RecieverId == CurrrentUserId || m.SenderId == CurrrentUserId).OrderByDescending(x => x.Time).Select(x => new HeaderUser
            {
                Id = x.Id,
                SenderId = x.SenderId,
                SenderUsername = _userManager.Users.FirstOrDefault(u => u.Id == x.SenderId).UserName,
                RecieverId = x.RecieverId,
                RecieverUsername = _userManager.Users.FirstOrDefault(u => u.Id == x.RecieverId).UserName,
                //Subject = x.Subject,
                Time = x.Time,
                Messages = _context.Messages.Where(y => y.HeaderId == x.Id).ToList()
            });

            return View(Model);
        }

        [HttpGet]
        public async Task<IActionResult> FindAgent()
        {
            var agent = await _context.CallAgents.FirstOrDefaultAsync(p => p.available == true);
            if (agent == null)
            {
                return View("NoFreeAgentAvailable");
            }

            return RedirectToAction("Message", "Chat", new { id = agent.Id });
        }

        //[HttpPost]
        //public async Task<IActionResult> FindAgent(int id)
        //{
        //    var agent = _context.CallAgents.FirstOrDefault(p => p.available == true);

        //    return RedirectToAction("Message", new { id = agent.Id });
        //}

        [HttpGet]
        public async Task<IActionResult> Message(int id)
        {
            var agent = _context.CallAgents.FirstOrDefault(p => p.available == true);
            var CurrrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Header header = new Header()
            {
                RecieverId = agent.UserId,
                SenderId = CurrrentUserId,
                //Subject = Prod,
                Time = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GTB Standard Time"))
            };

            var temp = _context.Headers.FirstOrDefault(h => (h.RecieverId == header.RecieverId) && (h.SenderId == header.SenderId) /*&& (h.Subject == header.Subject)*/);

            if (temp == null)
            {
                //new header to db
                //go to Messages 
                await _context.Headers.AddAsync(header);
                await _context.SaveChangesAsync();



                var NewHeader = _context.Headers.FirstOrDefault(h => (h.RecieverId == header.RecieverId) && (h.SenderId == header.SenderId) /*&& (h.Subject == header.Subject)*/).Id;

                //await this.Create(NewHeader, "");

                return RedirectToAction("Messages", new { id = NewHeader });
            }
            else
            {
                var NewHeader = temp.Id;
                return RedirectToAction("Messages", new { id = NewHeader });
            }

        }


        public async Task<IActionResult> Messages(int id)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var header = new Header();
            try
            {
                header = _context.Headers.FirstOrDefault(h => h.Id == id);
                header.Messages = _context.Messages.Where(m => m.HeaderId == header.Id).OrderByDescending(x => x.Time).ToList();
                foreach (var m in header.Messages)
                {
                    if (m.SenderId != userid)
                    {
                        m.Read = true;
                        _context.Messages.Update(m);
                        await _context.SaveChangesAsync();
                        m.IsFromSender = true;
                    }
                    else
                    {
                        m.IsFromSender = false;
                    }

                }
            }
            catch
            {

            }

            return View(header);
        }

        [HttpPost]
        public async Task Create(int HeaderId, string message)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var header = _context.Headers.FirstOrDefault(h => h.Id == HeaderId);
            var NewMessage = new Message()
            {
                HeaderId = HeaderId,
                Content = message,
                SenderId = userid,
                Time = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GTB Standard Time"))
            };
            await _context.Messages.AddAsync(NewMessage);
            await _context.SaveChangesAsync();


            //await _hubContext.Clients.All.SendAsync("Notify", $"Добавлено: {message} - {DateTime.Now.ToShortTimeString()}");
        }

        [HttpPost]
        [HttpGet]
        public async Task<string> MessageStatus()
        {
            int count = 0;
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var headers = await _context.Headers.Where(x => x.SenderId == userid || x.RecieverId == userid).ToListAsync();
            foreach (var header in headers)
            {
                header.Messages = await _context.Messages.Where(x => x.HeaderId == header.Id).ToListAsync();
                foreach (var message in header.Messages)
                {
                    if (message.SenderId != userid && message.Read == false)
                    {
                        count++;
                    }
                }
            }
            return count.ToString();
        }
    }
}
