using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myshop.DataAccess.data;
using System.Security.Claims;
using Utilities;


namespace Myshop.Web.Areas.admin.Controllers;

	[Area("Admin")]
	[Authorize(Roles = SD.AdminRole)]

	public class UsersController : Controller
{
    private readonly ApplicationDbContext _context;
    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        string userid = claim.Value;
        return View(_context.ApplicationUsers.Where(x => x.Id != userid).ToList());
    }
}
