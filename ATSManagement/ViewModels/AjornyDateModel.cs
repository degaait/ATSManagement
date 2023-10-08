namespace ATSManagement.ViewModels
{
    public class AjornyDateModel
    {
        public Guid AdjoryId { get; set; }

        public Guid? RequestId { get; set; }

        public DateTime? AdjorneyDate { get; set; }

        public string? WhatIsDone { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

    }
}
