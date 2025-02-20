using Sedziowanie.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sedziowanie.Models
{
    public class Niedyspozycja
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Poczatek { get; set; }

        [Required]
        public DateTime Koniec { get; set; }

       
        public int SedziaId { get; set; }
        public Sedzia Sedzia { get; set; }
    }
}

