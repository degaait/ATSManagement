using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblAdractivitiesReport
{
    public Guid Id { get; set; }

    public Guid? TypeId { get; set; }

    public string? Womens { get; set; }

    public string? Childrens { get; set; }

    public string? WomeElders { get; set; }

    public string? Hivpositives { get; set; }

    public string? Mens { get; set; }

    public string? OtherServantType { get; set; }

    public string? OutofResponsibilty { get; set; }

    public string? Family { get; set; }

    public string? Property { get; set; }

    public string? AsserinaSerategna { get; set; }

    public string? LelochGudayAyine { get; set; }

    public string? YediridirGenzebMeten { get; set; }

    public string? YeteWosenewuGenzebMeten { get; set; }

    public Guid? YearId { get; set; }

    public Guid? MonthId { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblMonth? Month { get; set; }

    public virtual TblAdreventType? Type { get; set; }

    public virtual TblYear? Year { get; set; }
}
