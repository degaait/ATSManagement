﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ViewModels
{
    public class DocumentHistoryModel
    {      
            public Guid HistoryId { get; set; }
            public Guid? RequestId { get; set; }
            [Display(Name = "Document")]
            public IFormFile? DocPath { get; set; }
            [Display(Name = "Round")]
            public int Round { get; set; }
            [Display(Name = "Description")]
            public string? Description { get; set; }
            [Display(Name = "Description")]
            public Guid? InternalReplyId { get; set; }
            [Display(Name = "Replied")]
            public Guid? ExternalRepliedBy { get; set; }
            [Display(Name = "File Description")]
            public string? FileDescription { get; set; }


        [Required(ErrorMessage = "*")]
        public Guid? InistId { get; set; }
        [Display(Name = "Institutions")]
        public List<SelectListItem>? Intitutions { get; set; }
    }
}
