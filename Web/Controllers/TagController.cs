using Microsoft.AspNetCore.Mvc;
using Web.Services.Abstract;
using Web.ViewModels.Tag;

namespace Web.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _tagService.GetAllAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await _tagService.GetTagAddCategoriesAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TagCreateVM model)
        {
            var isSucceded = await _tagService.TagCreateAsync(model);
            if (isSucceded) return RedirectToAction(nameof(Index));
            model = await _tagService.GetTagAddCategoriesAsync();
            return View(model);
        }
    }
}
