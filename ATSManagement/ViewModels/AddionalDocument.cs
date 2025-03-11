using ATSManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ViewModels
{
    public class AddionalDocument
    {
       
        public Guid? RequestID { get; set; }
        [Required(ErrorMessage ="*")]
        public IFormFile? formFile { get; set; }
        [Required(ErrorMessage = "*")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "*")]
        public Guid? InistId { get; set; }
        [Display(Name = "Institutions")]
        public List<SelectListItem>? Intitutions { get; set; }

        public List<TblDocumentHistory> tblDocuments { get; set; }=new List<TblDocumentHistory>();
    }
  
}
