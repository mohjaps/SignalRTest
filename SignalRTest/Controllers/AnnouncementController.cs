using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRTest.Hubs;

namespace SignalRTest.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly IHubContext<ChatHub> _chat;

        public AnnouncementController(IHubContext<ChatHub> chat)
        {
            _chat = chat;
        }
        [HttpGet("/announcement")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("/announcement")]
        public async Task<IActionResult> Post([FromForm] string sender, [FromForm] string message)
        {
            await _chat.Clients.All.SendAsync("ReceiveMessage", sender, message);
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
