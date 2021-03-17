using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Resources;

namespace RocketPOS.Controllers.Master
{
    public class OutletRegisterController : Controller
    {
        private readonly ICommonService _iCommonService;
        private readonly IOutletRegisterService _iOutletRegisterService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public OutletRegisterController(IOutletRegisterService iOutletRegisterService, ICommonService iCommonService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iOutletRegisterService = iOutletRegisterService;
            _iDropDownService = idropDownService;
            _iCommonService = iCommonService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index()
        {
            _iCommonService.GetPageWiseRoleRigths("OutletRegister");
            List<OutletRegisterModel> outletRegisterModels = new List<OutletRegisterModel>();
            outletRegisterModels = _iOutletRegisterService.GetOutletRegisterList().ToList();

            return View(outletRegisterModels);
        }

        public ActionResult OutletRegister(int? id)
        {
            OutletRegisterModel outletRegisterModel = new OutletRegisterModel();
            if (UserRolePermissionForPage.Add == true || UserRolePermissionForPage.Edit == true)
            {
                outletRegisterModel.OpenDate = DateTime.Now;

                if (id > 0)
                {
                    int OutletRegisterId = Convert.ToInt32(id);
                    outletRegisterModel = _iOutletRegisterService.GetOutletRegisterById(OutletRegisterId);
                }
                outletRegisterModel.OutletList = _iDropDownService.GetOutletList();
                outletRegisterModel.UserList = _iDropDownService.GetUserList();

                return View(outletRegisterModel);
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OutletRegister(OutletRegisterModel outletRegisterModel)
        {
            outletRegisterModel.OutletList = _iDropDownService.GetOutletList();
            outletRegisterModel.UserList = _iDropDownService.GetUserList();

            var errors = ModelState
    .Where(x => x.Value.Errors.Count > 0)
    .Select(x => new { x.Key, x.Value.Errors })
    .ToArray();

            decimal value;
            string errorString;
            if (decimal.TryParse(outletRegisterModel.OpeningBalance.ToString(), out value) == true)
            {
                if (value <= 0)
                {
                    ModelState.AddModelError("OpeningBalance", "Opening balance should be more than zero");
                }
            }

            if (!ModelState.IsValid)
            {
                errorString = this.ValidationOutletRegister(outletRegisterModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(outletRegisterModel);
                }
            }


            if (outletRegisterModel.Id > 0)
            {
                var result = _iOutletRegisterService.UpdateOutletRegister(outletRegisterModel);
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iOutletRegisterService.InsertOutletRegister(outletRegisterModel);
                if (result == 0)
                {
                    errorString = "User with outlet already exists for the same open date";
                    ModelState.AddModelError("UserId", "User with outlet already exists for the same open date");
                    ViewBag.Validate = errorString;
                    return View(outletRegisterModel);
                }
                else
                {
                    ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
                }
            }
            return RedirectToAction("Index", "OutletRegister");
        }

        public ActionResult Delete(int id)
        {
            if (UserRolePermissionForPage.Delete == true)
            {
                var deletedid = _iOutletRegisterService.DeleteOutletRegister(id);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        private string ValidationOutletRegister(OutletRegisterModel outletRegisterModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(outletRegisterModel.OutletId.ToString()))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidAddOnesName");
                return ErrorString;
            }
            if (string.IsNullOrEmpty(outletRegisterModel.UserId.ToString()))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidAddOnesName");
                return ErrorString;
            }
            if (Math.Abs(outletRegisterModel.OpeningBalance) <= 0)
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidAddOnesName");
                return ErrorString;
            }

            return ErrorString;
        }


    }
}
