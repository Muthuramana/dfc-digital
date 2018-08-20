﻿using System;
using System.ComponentModel;
using System.Web.Mvc;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.CmsExtensions.MVC.Controllers
{
    /// <summary>
    /// Custom Widget for Admin Panel
    /// </summary>
    /// <seealso cref="BaseDfcController" />
    [ControllerToolboxItem(Name = "SkillsFrameworkImport", Title = "Framework Skills Data Import Widget", SectionName = SitefinityConstants.CustomAdminWidgetSection)]
    public class SkillsFrameworkDataImportController : BaseDfcController
    {
        #region Private Members

        private readonly IImportSkillsFrameworkDataService importSkillsFrameworkDataService;
        private readonly IReportAuditRepository reportAuditRepository;
        private readonly IWebAppContext webAppContext;

        #endregion Private Members

        #region Constructors

        public SkillsFrameworkDataImportController(IApplicationLogger applicationLogger, IImportSkillsFrameworkDataService importSkillsFrameworkDataService, IReportAuditRepository reportAuditRepository, IWebAppContext webAppContext) : base(applicationLogger)
        {
            this.importSkillsFrameworkDataService = importSkillsFrameworkDataService;
            this.reportAuditRepository = reportAuditRepository;
            this.webAppContext = webAppContext;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the Page Title.
        /// </summary>
        /// <value>
        /// The Page Title.
        /// </value>
        [DisplayName("Page Title")]
        public string PageTitle { get; set; } = "Onet Data Import";

        /// <summary>
        /// Gets or sets the First Paragraph.
        /// </summary>
        /// <value>
        /// The Page Title.
        /// </value>
        [DisplayName("First Paragraph")]
        public string FirstParagraph { get; set; } = "Select the Onet data import process you want to perform.";

        /// <summary>
        /// Gets or sets the First Paragraph.
        /// </summary>
        /// <value>
        /// The Page Title.
        /// </value>
        [DisplayName("Not Allowed Message")]
        public string NotAllowedMessage { get; set; } = "You are not allowed to use this functionality. Only Administrators are.";

        #endregion Public Properties

        #region Actions

        // GET: AdminPanel
        public ActionResult Index()
        {
            var model = new SkillsFrameworkImportViewModel
            {
                PageTitle = PageTitle,
                FirstParagraph = FirstParagraph,
                NotAllowedMessage = NotAllowedMessage,
                IsAdmin = webAppContext.IsUserAdministrator
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string importMode)
        {
            var importResult = new SkillsFrameworkResultsViewModel
            {
                PageTitle = PageTitle,
                FirstParagraph = FirstParagraph,
                NotAllowedMessage = NotAllowedMessage,
                IsAdmin = webAppContext.IsUserAdministrator
            };

            var otherMessage = string.Empty;

            if (webAppContext.IsUserAdministrator)
            {
                try
                {
                    switch (importMode?.ToUpperInvariant().Trim())
                    {
                        case "IMPORTSKILLS":
                            importSkillsFrameworkDataService.ImportFrameworkSkills();
                            importResult.ActionCompleted = "Import Onet Skills";
                            break;
                        case "UPDATESOCOCCUPATIONALCODES":
                            importSkillsFrameworkDataService.UpdateSocCodesOccupationalCode();
                            importResult.ActionCompleted = "Update Soc Codes with Onet Occupational Codes";
                            break;
                        case "UPDATEJPDIGITALSKILLS":
                            importSkillsFrameworkDataService.UpdateJobProfilesDigitalSkills();
                            importResult.ActionCompleted = "Update Job Profiles With Digital Skill levels";
                            break;
                        case "BUILDSOCMATRIX":
                            importSkillsFrameworkDataService.BuildSocMatrixData();
                            importResult.ActionCompleted = "Build Soc Skill Matrix";
                            break;
                        case "UPDATEJPSKILLS":
                            importSkillsFrameworkDataService.UpdateJpSocSkillMatrix();
                            importResult.ActionCompleted = "Update Job Profiles with related Soc skill Matrices";
                            break;
                    }
                   }
                catch (Exception ex)
                {
                    otherMessage = $"{ex.Message} <br /> {ex.InnerException} <br /> {ex.StackTrace}";
                }
            }
            else
            {
                otherMessage = NotAllowedMessage;
            }

            importResult.AuditRecords = reportAuditRepository.GetAllAuditRecords();
            importResult.OtherMessage = otherMessage;

            return View("ImportResults", importResult);
        }

        #endregion Actions
    }
}