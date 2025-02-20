using System.ComponentModel.DataAnnotations;
using Sedziowanie.Models;

namespace Sedziowanie.Models
{
    public class Sedzia
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Imie { get; set; }

        [MaxLength(50)]
        public string Nazwisko { get; set; }
        [MaxLength(2)]
        public string Klasa { get; set; }

        [MaxLength(15)]
        public string Telefon { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
       
       
        public ICollection<Niedyspozycja> Niedyspozycje { get; set; } = new List<Niedyspozycja>();
        public ICollection<Mecz> MeczeJakoSedzia1 { get; set; } = new List<Mecz>();
        public ICollection<Mecz> MeczeJakoSedzia2 { get; set; } = new List<Mecz>();
        public ICollection<Mecz> MeczeJakoSedziaSekretarz { get; set; } = new List<Mecz>();

    }
}
