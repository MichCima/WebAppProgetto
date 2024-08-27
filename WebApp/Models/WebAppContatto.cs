using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Areas.Identity.Data;

namespace WebApp.Models
{
    public class WebAppContatto
    {
        [Key]
        public int ContattoID { get; set; }

        [Column(TypeName ="nvarchar(50)")]
        [DisplayName("Nome")]
        [Required(ErrorMessage = "È richiesto il nome")]
        public string ContattoName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [DisplayName("Cognome")]
        [Required(ErrorMessage = "È richiesto il cognome")]
        public string ContattoCognome { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [DisplayName("Città")]
        [Required(ErrorMessage = "È richiesta la città")]
        public string ContattoCitta { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        [DisplayName("Telefono")]
        [Required(ErrorMessage = "È richiesto il numero telefonico")]
        [MaxLength(10, ErrorMessage = "Massimo 10 numeri")]
        public string ContattoTelefono { get; set; }

        // Foreign Key per Identity User
        [ForeignKey("User")]
        public string UserId { get; set; }
        public WebAppUser? User { get; set; }
    }
}
