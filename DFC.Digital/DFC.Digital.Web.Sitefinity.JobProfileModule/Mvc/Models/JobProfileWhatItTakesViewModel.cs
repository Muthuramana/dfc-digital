﻿namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    /// <summary>
    /// Job Profile Section View
    /// </summary>
    public class JobProfileWhatItTakesViewModel
    {
        public JobProfileWhatItTakesSkillsViewModel JobProfileWhatItTakesSkillsViewModel { get; set; }

        /// <summary>
        /// Gets or sets the restrictions other requirements.
        /// </summary>
        /// <value>
        /// The restrictions other requirements.
        /// </value>
        public RestrictionsOtherRequirements RestrictionsOtherRequirements { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is what it takes view.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is what it takes view; otherwise, <c>false</c>.
        /// </value>
        public bool IsWhatItTakesCadView { get; set; }

        /// <summary>
        /// Gets or sets the section identifier.
        /// </summary>
        /// <value>
        /// The section identifier.
        /// </value>
        public string SectionId { get; set; }

        public string Title { get; set; }
    }
}