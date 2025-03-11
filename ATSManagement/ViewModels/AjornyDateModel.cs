using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ViewModels
{
    public class AjornyDateModel
    {
        public Guid AdjoryId { get; set; }

        public Guid? RequestId { get; set; }
        //ቀጣይ ቀጠሮ
        [Required(ErrorMessage = "*")]
        public DateTime? AdjorneyDate { get; set; }
        [Required(ErrorMessage = "*")]
        public string? WhatIsDone { get; set; }

        public Guid? CreatedBy { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime? CreatedDate { get; set; }
        //ጉዳዩን የያዘው ባለሞያ ስም
        [Required(ErrorMessage = "*")]
        public string? ExpertHanlingCase { get; set; }
        //ከሳሽ/ተከሳሽ፤ይግባኝ ባይ/አመልካች/መልስ ሰጭ
        [Required(ErrorMessage = "*")]
        public string? Plaintiff_Defendant { get; set; }
        //ጉዳዩ የሚታይበት ፍ/ቤት እና መ/ቁ
        [Required(ErrorMessage = "*")]
        public string? TheCourtCaseHanled { get; set; }
        //የተቀጠረበት ምክንያት
        [Required(ErrorMessage = "*")]
        public string? AppointmentReason { get; set; }
        public string? Defendant_info { get; set; }
        public string? CaseNumber { get; set; }


    }
}
