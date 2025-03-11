using ATSManagementExternal.Models;
using System.ComponentModel.DataAnnotations;

namespace ATSManagementExternal.ViewModels
{
    public class AddionalDocument
    {
       
        public Guid RequestID { get; set; }
        [Required(ErrorMessage ="*")]
        public IFormFile? formFile { get; set; }
        [Required(ErrorMessage = "*")]
        public string? Title { get; set; }
        public List<TblDocumentHistory> tblDocuments { get; set; }=new List<TblDocumentHistory>();
    }
    public class RequestModel
    {
        public Guid? RequestId { get; set; }
        [Required(ErrorMessage = "*")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        [Display(Name = "Request Detail")]
        public string? RequestDetail { get; set; }

    }
}
