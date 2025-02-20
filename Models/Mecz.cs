using Sedziowanie.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Sedziowanie.Models
{
    public class Mecz
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(5)]
        public string NumerMeczu { get; set; }

        public DateTime Data { get; set; }

        
        public int RozgrywkiId { get; set; }
        public Rozgrywki Rozgrywki { get; set; }

        [MaxLength(100)]
        public string Gospodarz { get; set; }

        [MaxLength(100)]
        public string Gosc { get; set; }
       
        public int SedziaIId { get; set; }
       
        public Sedzia SedziaI { get; set; }

       
        public int SedziaIIId { get; set; }
        
        public Sedzia SedziaII { get; set; }

      
        public int SedziaSekretarzId { get; set; }
        
        public Sedzia SedziaSekretarz { get; set; }
    }
}
