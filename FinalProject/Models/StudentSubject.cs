using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class StudentSubject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Student Id")]
        public int StudentId { get; set; }
        [Required]
        [Display(Name = "Subject Id")]
        public int SubjectId { get; set; }
        [Display(Name = "Student Name")]
        public Student? Student { get; set; }
        [Display(Name = "Subject Name")]
        public Subject? Subject { get; set; }
    }
}
