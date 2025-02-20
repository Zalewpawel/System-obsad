using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sedziowanie.Data;
using Sedziowanie.Models;
using System.Linq;

namespace Sedziowanie.Controllers
{
    public class RozgrywkiController : Controller
    {
        private readonly DBObsadyContext _context;

        public RozgrywkiController(DBObsadyContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Add()
        {
            // Wyświetlenie pustego formularza do dodania rozgrywki
            return View();
        }

        [HttpPost]
        public IActionResult Add(Rozgrywki rozgrywki)
        {
            // Sprawdzenie poprawności danych
            if (!ModelState.IsValid)
            {
                return View(rozgrywki);
            }

            // Dodanie nowej rozgrywki do bazy danych
            _context.Rozgrywki.Add(rozgrywki);
            _context.SaveChanges();

            // Przekierowanie do listy rozgrywek po dodaniu
            return RedirectToAction("ListaRozgrywek");
        }

        // Akcja do wyświetlania listy rozgrywek
        public IActionResult ListaRozgrywek()
        {
            var rozgrywki = _context.Rozgrywki.ToList();
            return View(rozgrywki);
        }

        // Nowa akcja do wyświetlania meczów dla danej rozgrywki
        [HttpGet]
        public IActionResult MeczeRozgrywek(int rozgrywkiId)
        {
            // Pobieramy mecze dla wybranej rozgrywki
            var mecze = _context.Mecze
                .Include(m => m.SedziaI)
                .Include(m => m.SedziaII)
                .Include(m => m.SedziaSekretarz)
                .Where(m => m.RozgrywkiId == rozgrywkiId)
                .Select(m => new
                {
                    m.NumerMeczu,
                    m.Data,
                    m.Gospodarz,
                    m.Gosc,
                    SedziaI = m.SedziaI.Imie + " " + m.SedziaI.Nazwisko,
                    SedziaII = m.SedziaII.Imie + " " + m.SedziaII.Nazwisko,
                    SedziaSekretarz = m.SedziaSekretarz.Imie + " " + m.SedziaSekretarz.Nazwisko
                })
                .ToList();

            // Pobieramy nazwę rozgrywki
            ViewBag.Rozgrywka = _context.Rozgrywki
                .Where(r => r.Id == rozgrywkiId)
                .Select(r => r.Nazwa)
                .FirstOrDefault();

            return View(mecze);
        }
    }
}
