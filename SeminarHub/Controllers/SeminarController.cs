using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeminarHub.Data;
using SeminarHub.Data.Models;
using SeminarHub.Models;
using System.Globalization;
using System.Security.Claims;
using static SeminarHub.Data.Models.DataConstants;

namespace SeminarHub.Controllers
{
    [Authorize]
    public class SeminarController : Controller
    {
        private readonly SeminarHubDbContext data;
        public SeminarController(SeminarHubDbContext context)
        {
            data = context;
        }
        public async Task<IActionResult> All()
        {
            var info = await data.Seminars
                .AsNoTracking()
                .Select(s => new AllViewModel(
                    s.Id,
                    s.Topic,
                    s.Lecturer,
                    s.Category.Name,
                    s.DateAndTime,
                    s.Organizer.UserName
                ))
                .ToListAsync();
            return View(info);
        }
        public async Task<IActionResult> Join(int id)
        {
            var seminar = await data.Seminars
                .Where(s => s.Id == id)
                .Include(s => s.SeminarsParticipants)
                .FirstOrDefaultAsync();
            if (seminar == null)
            {
                return BadRequest();
            }

            var userId = GetUserId();
            if (!seminar.SeminarsParticipants.Any(p => p.ParticipantId == userId))
            {
                seminar.SeminarsParticipants.Add(new SeminarParticipant()
                {
                    SeminarId = seminar.Id,
                    ParticipantId = userId
                });
                await data.SaveChangesAsync();
                return RedirectToAction(nameof(Joined));
            }
            return RedirectToAction(nameof(All));
        }
        public async Task<IActionResult> Joined()
        {
            string userId = GetUserId();
            var info = await data.SeminarParticipants
                .Where(p => p.ParticipantId == userId)
                .AsNoTracking()
                .Select(s => new AllViewModel(
                    s.SeminarId,
                    s.Seminar.Topic,
                    s.Seminar.Lecturer,
                    s.Seminar.Category.Name,
                    s.Seminar.DateAndTime,
                    s.Seminar.Organizer.UserName
                ))
                .ToListAsync();
            return View(info);
        }
        public async Task<IActionResult> Leave(int id)
        {
            var seminar = await data.Seminars
                .Where(s => s.Id == id)
                .Include(s => s.SeminarsParticipants)
                .FirstOrDefaultAsync();
            if (seminar == null)
            {
                return BadRequest();
            }
            var userId = GetUserId();
            var participant = seminar.SeminarsParticipants
                .FirstOrDefault(p => p.ParticipantId == userId);
            if (participant == null)
            {
                return BadRequest();
            }
            seminar.SeminarsParticipants.Remove(participant);
            await data.SaveChangesAsync();
            return RedirectToAction(nameof(Joined));
        }
        public async Task<IActionResult> Details(int id)
        {
            var seminar = await data.Seminars
                .Where(s => s.Id == id)
                .Select(s => new DetailsViewModel(
                s.Id,
                s.Topic,
                s.Lecturer,
                s.Category.Name,
                s.DateAndTime,
                s.Organizer.UserName,
                s.Details,
                s.Duration
                ))
                .FirstOrDefaultAsync();
            if (seminar == null)
            {
                return BadRequest();
            }
            return View(seminar);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AddViewModel();
            model.Categories = await GetCategories();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddViewModel seminar)
        {
            DateTime date = DateTime.Now;

            if (!DateTime.TryParseExact(
                seminar.DateAndTime,
                DateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date))
            {
                ModelState
                    .AddModelError(nameof(seminar.DateAndTime), $"Invalid date! Format must be: {DateTimeFormat}");
            }
            if (!ModelState.IsValid)
            {
                seminar.Categories = await GetCategories();
                return View(seminar);
            }
            var userId = GetUserId();
            var seminarData = new Seminar
            {
                Topic = seminar.Topic,
                Lecturer = seminar.Lecturer,
                CategoryId = seminar.CategoryId,
                DateAndTime = DateTime.Parse(seminar.DateAndTime),
                Details = seminar.Details,
                Duration = seminar.Duration,
                OrganizerId = userId
            };
            await data.Seminars.AddAsync(seminarData);
            await data.SaveChangesAsync();
            return RedirectToAction(nameof(All));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var seminar = await data.Seminars.FindAsync(id);
            if(seminar == null)
            {
                return BadRequest();
            }
            if (seminar.OrganizerId != GetUserId())
            {
                return Unauthorized();
            }
            var model = new AddViewModel
            {
                Topic = seminar.Topic,
                Lecturer = seminar.Lecturer,
                CategoryId = seminar.CategoryId,
                DateAndTime = seminar.DateAndTime.ToString(DateTimeFormat),
                Details = seminar.Details,
                Duration = seminar.Duration,
                Categories = await GetCategories()
            };
            model.Categories = await GetCategories();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AddViewModel seminar, int id)
        {
            var s = await data.Seminars
                .FindAsync(id);
            if (s == null)
            {
                return BadRequest();
            }
            if (s.OrganizerId != GetUserId())
            {
                return Unauthorized();
            }
            DateTime date = DateTime.Now;
            if (!DateTime.TryParseExact(
                  seminar.DateAndTime,
                  DateTimeFormat,
                  CultureInfo.InvariantCulture,
                  DateTimeStyles.None,
                  out date))
            {
                ModelState
                    .AddModelError(nameof(seminar.DateAndTime), $"Invalid date! Format must be: {DateTimeFormat}");
            }
            if (!ModelState.IsValid)
            {
                seminar.Categories = await GetCategories();
                return View(seminar);
            }
            s.Topic = seminar.Topic;
            s.Lecturer = seminar.Lecturer;
            s.CategoryId = seminar.CategoryId;
            s.DateAndTime = DateTime.Parse(seminar.DateAndTime);
            s.Details = seminar.Details;
            s.Duration = seminar.Duration;
            await data.SaveChangesAsync();
            return RedirectToAction(nameof(All));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var seminar = await data.Seminars.FindAsync(id);
            if (seminar == null)
            {
                return BadRequest();
            }
            if (seminar.OrganizerId != GetUserId())
            {
                return Unauthorized();
            }
            var model = new ConfirmDeleteViewModel
            {
                Id = seminar.Id,
                Topic = seminar.Topic,
                DateAndTime = seminar.DateAndTime.ToString(DateTimeFormat)
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(ConfirmDeleteViewModel seminar)
        {
            var s = await data.Seminars.FindAsync(seminar.Id);
            if (s == null)
            {
                return BadRequest();
            }
            if (s.OrganizerId != GetUserId())
            {
                return Unauthorized();
            }
            data.Seminars.Remove(s);
            await data.SaveChangesAsync();
            return RedirectToAction(nameof(All));
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }
        private async Task<IEnumerable<CategoriesViewModel>> GetCategories()
        {
            return await data.Categories
                .AsNoTracking()
                .Select(c => new CategoriesViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }
    }
}
