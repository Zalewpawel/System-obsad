using Microsoft.AspNetCore.Mvc;
using Sedziowanie.Models;
using Sedziowanie.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sedziowanie.Controllers
{
    public class NiedyspozycjaController : Controller
    {
        private readonly DBObsadyContext _context;

        public NiedyspozycjaController(DBObsadyContext context)
        {
            _context = context;
        }

        // Widok do wyświetlania listy niedyspozycji
        public IActionResult Show()
        {
            var niedyspozycje = _context.Niedyspozycje
                .Include(n => n.Sedzia) // Łączenie z tabelą Sedzia
                .ToList();
            return View(niedyspozycje);
        }

        [HttpGet]
        public IActionResult Add()
        {
            // Tworzymy listę rozwijaną z imieniem i nazwiskiem sędziów
            var sedziowie = _context.Sedziowie
                .Select(s => new
                {
                    FullName = s.Imie + " " + s.Nazwisko,
                    s.Id
                })
                .ToList();

            ViewBag.Sedziowie = new SelectList(sedziowie, "Id", "FullName");
            return View();
        }


        [HttpPost]
        public IActionResult Add(int sedziaId, DateTime poczatek, DateTime koniec)
        {
            if (poczatek >= koniec)
            {
                ModelState.AddModelError("", "Data początku musi być wcześniejsza niż data końca.");
                return View();
            }

            // Sprawdzamy, czy sędzia istnieje
            var sedzia = _context.Sedziowie.FirstOrDefault(s => s.Id == sedziaId);
            if (sedzia == null)
            {
                ModelState.AddModelError("", "Wybrany sędzia nie istnieje.");
                return View();
            }

            // Tworzymy nowy obiekt Niedyspozycja
            var niedyspozycja = new Niedyspozycja
            {
                SedziaId = sedzia.Id,
                Poczatek = poczatek,
                Koniec = koniec
            };

            // Dodajemy do bazy danych
            _context.Niedyspozycje.Add(niedyspozycja);
            _context.SaveChanges();

            return RedirectToAction("Show");
        }


    }
}
