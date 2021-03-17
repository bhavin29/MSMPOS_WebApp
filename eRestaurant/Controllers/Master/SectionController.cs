using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketPOS.Controllers.Master
{
    public class SectionController : Controller
    {
        private readonly ICommonService _iCommonService;
        private readonly ISectionService _iSectionService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private LocService _locService;

        public SectionController(ISectionService iSectionService, ICommonService iCommonService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _iSectionService = iSectionService;
            _iCommonService = iCommonService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;
        }

        public ActionResult Index(int? noDelete)
        {
            _iCommonService.GetPageWiseRoleRigths("Section");
            List<SectionModel> sectionModel = new List<SectionModel>();
            sectionModel = _iSectionService.GetSectionList().ToList();
            if (noDelete != null)
            {
                ViewBag.Result = _locService.GetLocalizedHtmlString("Can not delete reference available.");
            }
            return View(sectionModel);
        }

        public ActionResult Section(int? id)
        {
            SectionModel sectionModel = new SectionModel();
            if (UserRolePermissionForPage.Add == true || UserRolePermissionForPage.Edit == true)
            {
                if (id > 0)
                {
                    sectionModel = _iSectionService.GetSectionById(Convert.ToInt32(id));
                }

                return View(sectionModel);
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Section(SectionModel sectionModel)
        {

            if (!ModelState.IsValid)
            {
                string errorString = this.ValidationSection(sectionModel);
                if (!string.IsNullOrEmpty(errorString))
                {
                    ViewBag.Validate = errorString;
                    return View(sectionModel);
                }
            }

            if (sectionModel.Id > 0)
            {
                var result = _iSectionService.UpdateSection(sectionModel);
                if (result == -1)
                {
                    ModelState.AddModelError("SectionName", "Section already exists");
                    return View(sectionModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("EditSuccss");
            }
            else
            {
                var result = _iSectionService.InsertSection(sectionModel);
                if (result == -1)
                {
                    ModelState.AddModelError("SectionName", "Section already exists");
                    return View(sectionModel);
                }
                ViewBag.Result = _locService.GetLocalizedHtmlString("SaveSuccess");
            }

            return RedirectToAction("Index", "Section");
        }

        public ActionResult Delete(int id)
        {
            int result = 0;
            if (UserRolePermissionForPage.Delete == true)
            {
                result = _iCommonService.GetValidateReference("Section", id.ToString());
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index), new { noDelete = result });
                }
                else
                {
                    var deletedid = _iSectionService.DeleteSection(id);
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        private string ValidationSection(SectionModel sectionModel)
        {
            string ErrorString = string.Empty;
            if (string.IsNullOrEmpty(sectionModel.SectionName))
            {
                ErrorString = _locService.GetLocalizedHtmlString("ValidSectionName");
                return ErrorString;
            }
            return ErrorString;
        }
    }
}
