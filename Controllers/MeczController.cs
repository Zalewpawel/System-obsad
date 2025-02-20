using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sedziowanie.Data;
using Sedziowanie.Models;
using System;
using System.Linq;

namespace Sedziowanie.Controllers
{
    public class MeczController : Controller
    {
        private readonly DBObsadyContext _context;

        public MeczController(DBObsadyContext context)
        {
            _context = context;
        }

        public IActionResult Add()
        {
            var sedziowie = _context.Sedziowie
                .Select(s => new
                {
                    Id = s.Id,
                    FullName = $"{s.Imie} {s.Nazwisko}",
                    Klasa = s.Klasa
                })
                .ToList();

            var rozgrywki = _context.Rozgrywki
                .Select(r => new
                {
                    Id = r.Id,
                    Nazwa = r.Nazwa
                })
                .ToList();

            ViewBag.Sedziowie = sedziowie;
            ViewBag.Rozgrywki = rozgrywki;

            return View();
        }

        [HttpPost]
        public IActionResult Add(string numerMeczu, DateTime data, int rozgrywkiId, string gospodarz, string gosc,
                                 int sedziaIId, int sedziaIIId, int sedziaSekretarzId)
        {
            if (data < DateTime.Now)
            {
                ModelState.AddModelError("", "Data meczu nie może być wcześniejsza niż aktualna data.");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Add");
            }

            var mecz = new Mecz
            {
                NumerMeczu = numerMeczu,
                Data = data,
                Gospodarz = gospodarz,
                Gosc = gosc,
                RozgrywkiId = rozgrywkiId,
                SedziaIId = sedziaIId,
                SedziaIIId = sedziaIIId,
                SedziaSekretarzId = sedziaSekretarzId
            };

            _context.Mecze.Add(mecz);
            _context.SaveChanges();

            return RedirectToAction("ListaMeczy");
        }

      
        public IActionResult ListaMeczy()
        {
            var mecze = _context.Mecze
                .Include(m => m.Rozgrywki)
                .Include(m => m.SedziaI)
                .Include(m => m.SedziaII)
                .Include(m => m.SedziaSekretarz)
                .ToList();

            return View(mecze);
        }


        // Akcja: Edycja meczu (GET - formularz)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var mecz = _context.Mecze
                .Include(m => m.Rozgrywki)
                .Include(m => m.SedziaI)
                .Include(m => m.SedziaII)
                .Include(m => m.SedziaSekretarz)
                .FirstOrDefault(m => m.Id == id);

            if (mecz == null)
            {
                return NotFound();
            }

            ViewBag.Sedziowie = _context.Sedziowie
                .Select(s => new
                {
                    Id = s.Id,
                    FullName = $"{s.Imie} {s.Nazwisko}"
                })
                .ToList();

            ViewBag.Rozgrywki = _context.Rozgrywki
                .Select(r => new
                {
                    Id = r.Id,
                    Nazwa = r.Nazwa
                })
                .ToList();

            return View(mecz);
        }

        // Akcja: Edycja meczu (POST - zapis zmian)
        [HttpPost]
        public IActionResult Edit(Mecz mecz)
        {
            if (!ModelState.IsValid)
            {
                return View(mecz);
            }

            _context.Mecze.Update(mecz);
            _context.SaveChanges();

            return RedirectToAction("ListaMeczy");
        }

        // Akcja: Usunięcie meczu (GET - potwierdzenie)
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var mecz = _context.Mecze
                .Include(m => m.Rozgrywki)
                .FirstOrDefault(m => m.Id == id);

            if (mecz == null)
            {
                return NotFound();
            }

            return View(mecz);
        }

        // Akcja: Usunięcie meczu (POST - potwierdzenie usunięcia)
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var mecz = _context.Mecze.FirstOrDefault(m => m.Id == id);
            if (mecz == null)
            {
                return NotFound();
            }

            _context.Mecze.Remove(mecz);
            _context.SaveChanges();

            return RedirectToAction("ListaMeczy");
        }
    }
}
