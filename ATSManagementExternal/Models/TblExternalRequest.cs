﻿using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblExternalRequest
{
    public Guid RequestId { get; set; }

    public string? RequestDetail { get; set; }

    public DateTime? RequestedDate { get; set; }

    public Guid? IntId { get; set; }

    public Guid? ExterUserId { get; set; }

    public Guid? ExternalRequestStatusId { get; set; }

    public virtual TblExternalUser? ExterUser { get; set; }

    public virtual TblExternalRequestStatus? ExternalRequestStatus { get; set; }

    public virtual TblInistitution? Int { get; set; }
}