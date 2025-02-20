using Microsoft.AspNetCore.Mvc;
using Sedziowanie.Models;
using Sedziowanie.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Sedziowanie.Controllers
{
    public class SedziaController : Controller
    {
        private readonly DBObsadyContext _context;

        public SedziaController(DBObsadyContext context)
        {
            _context = context;
        }

        // Akcja: Wyświetlenie listy sędziów
        [HttpGet]
        public IActionResult Show()
        {
            var sedziowie = _context.Sedziowie.ToList();
            return View(sedziowie);
        }

        // Akcja: Wyświetlenie formularza dodania sędziego
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // Akcja: Obsługa dodawania sędziego
        [HttpPost]
        public IActionResult Add(Sedzia sedzia)
        {
            if (!ModelState.IsValid)
            {
                return View(sedzia);
            }

            _context.Sedziowie.Add(sedzia);
            _context.SaveChanges();

            return RedirectToAction("Show");
        }

        // Akcja: Edycja sędziego (GET - formularz)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var sedzia = _context.Sedziowie.FirstOrDefault(s => s.Id == id);
            if (sedzia == null)
            {
                return NotFound();
            }
            return View(sedzia);
        }

        // Akcja: Edycja sędziego (POST - zapis zmian)
        [HttpPost]
        public IActionResult Edit(Sedzia sedzia)
        {
            if (!ModelState.IsValid)
            {
                return View(sedzia);
            }

            _context.Sedziowie.Update(sedzia);
            _context.SaveChanges();

            return RedirectToAction("Show");
        }

        // Akcja: Usunięcie sędziego (GET - potwierdzenie)
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var sedzia = _context.Sedziowie.FirstOrDefault(s => s.Id == id);
            if (sedzia == null)
            {
                return NotFound();
            }
            return View(sedzia);
        }

        // Akcja: Usunięcie sędziego (POST - potwierdzenie usunięcia)
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var sedzia = _context.Sedziowie.FirstOrDefault(s => s.Id == id);
            if (sedzia == null)
            {
                return NotFound();
            }

            _context.Sedziowie.Remove(sedzia);
            _context.SaveChanges();

            return RedirectToAction("Show");
        }

        // Akcja: Mecze sędziego
        [HttpGet]
        public IActionResult MeczeSedziego(int sedziaId)
        {
            var mecze = _context.Mecze
                .Include(m => m.Rozgrywki)
                .Include(m => m.SedziaI)
                .Include(m => m.SedziaII)
                .Include(m => m.SedziaSekretarz)
                .Where(m => m.SedziaIId == sedziaId || m.SedziaIIId == sedziaId || m.SedziaSekretarzId == sedziaId)
                .Select(m => new
                {
                    m.NumerMeczu,
                    m.Data,
                    m.Gospodarz,
                    m.Gosc,
                    Rozgrywki = m.Rozgrywki.Nazwa,
                    SedziaI = m.SedziaI.Imie + " " + m.SedziaI.Nazwisko,
                    SedziaII = m.SedziaII.Imie + " " + m.SedziaII.Nazwisko,
                    SedziaSekretarz = m.SedziaSekretarz.Imie + " " + m.SedziaSekretarz.Nazwisko
                })
                .ToList();

            ViewBag.Sedzia = _context.Sedziowie
                .Where(s => s.Id == sedziaId)
                .Select(s => s.Imie + " " + s.Nazwisko)
                .FirstOrDefault();

            return View(mecze);
        }
    }
}
