using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace grupo_rojo.Models
{
    [Table("t_contacto")]
    public class Contacto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]	
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Message { get; set; }  
    }
}