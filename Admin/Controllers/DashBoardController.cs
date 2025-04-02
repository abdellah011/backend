using Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Admin.Data;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly UserContext _context;

    public DashboardController(UserContext context)
    {
        _context = context;
    }

    [HttpGet("user-stats")]
    public IActionResult GetUserStats()
    {
        var totalUsers = _context.Users.Count();
        // Exemple de logique, à adapter selon comment tu gères les connexions
        var connectedUsers = _context.Users.Count(u => !string.IsNullOrEmpty(u.Token));

        var stats = new UserStatsDto
        {
            Date = DateTime.Now,
            TotalUsers = totalUsers,
            ConnectedUsers = connectedUsers
        };

        return Ok(stats);
    }
    [HttpGet("user-stats-history")]
    public IActionResult GetUserStatsHistory()
    {
        var stats = new List<UserStatsDto>
    {
        new UserStatsDto { Date = DateTime.Now.AddDays(-4), TotalUsers = 5, ConnectedUsers = 2 },
        new UserStatsDto { Date = DateTime.Now.AddDays(-3), TotalUsers = 6, ConnectedUsers = 3 },
        new UserStatsDto { Date = DateTime.Now.AddDays(-2), TotalUsers = 7, ConnectedUsers = 2 },
        new UserStatsDto { Date = DateTime.Now.AddDays(-1), TotalUsers = 8, ConnectedUsers = 1 },
        new UserStatsDto { Date = DateTime.Now, TotalUsers = 8, ConnectedUsers = 1 },
    };

        return Ok(stats);

    }
    [HttpGet("user-roles")]
    public IActionResult GetUserRoles()
    {
        var admins = _context.Users.Count(u => u.Role == "admin");
        var users = _context.Users.Count(u => u.Role != "admin");

        var roles = new[]
        {
        new { name = "Administrateurs", value = admins },
        new { name = "Utilisateurs", value = _context.Users.Count() - admins }
    };

        return Ok(roles);
    }
    [HttpGet("count-users-admins")]
    public IActionResult GetUsersAdminsCount()
    {
        var totalUsers = _context.Users.Count();
        var totalAdmins = _context.Users.Count(u => u.Role == "admin");

        return Ok(new
        {
            totalUsers,
            totalAdmins
        });
    }
}
