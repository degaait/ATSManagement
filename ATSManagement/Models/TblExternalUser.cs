﻿using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblExternalUser
{
    public Guid ExterUserId { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public Guid? InistId { get; set; }

    public bool? IsActive { get; set; }

    public virtual TblInistitution? Inist { get; set; }
}