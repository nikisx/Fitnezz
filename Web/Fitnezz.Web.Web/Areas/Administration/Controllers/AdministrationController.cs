namespace Fitnezz.Web.Web.Areas.Administration.Controllers
{
    using Fitnezz.Web.Common;
    using Fitnezz.Web.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
