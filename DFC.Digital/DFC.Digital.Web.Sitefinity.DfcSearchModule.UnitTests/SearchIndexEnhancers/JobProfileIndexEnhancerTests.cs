﻿using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.DfcSearchModule.UnitTests
{
    public class JobProfileIndexEnhancerTests
    {
        [Theory]
        [InlineData(false, false, 1000, 2000)]
        [InlineData(false, true, 1000, 2000)]
        [InlineData(true, false, 10, 20)]
        [InlineData(true, true, 10, 20)]
        public async Task GetSalaryRangeAsyncTestAsync(bool isSalaryOverriden, bool isPublishing, double salaryStarterExpected, double salaryExperiencedExpected)
        {
            var fakeJobProfileRepo = A.Fake<IJobProfileRepository>();
            var fakeJobProfileCategoryRepo = A.Fake<IJobProfileCategoryRepository>();
            var salaryService = A.Fake<ISalaryService>();
            var salaryCalculator = A.Fake<ISalaryCalculator>();
            var dummyJobProfileIndex = A.Dummy<JobProfileIndex>();
            var dummyJobProfile = new JobProfile
            {
                IsLMISalaryFeedOverriden = isSalaryOverriden,
                SalaryStarter = (decimal)salaryStarterExpected,
                SalaryExperienced = (decimal)salaryExperiencedExpected,
                SOCCode = nameof(JobProfile.SOCCode)
            };
            var dummySalary = new JobProfileSalary
            {
                Deciles = new Dictionary<int, decimal>
                {
                    { 10, 100 },
                    { 20, 200 }
                }
            };

            A.CallTo(() => fakeJobProfileRepo.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => fakeJobProfileRepo.GetByUrlNameForSearchIndex(A<string>._, A<bool>._)).Returns(dummyJobProfile);
            A.CallTo(() => salaryService.GetSalaryBySocAsync(A<string>._)).Returns(Task.FromResult(dummySalary));
            A.CallTo(() => salaryCalculator.GetStarterSalary(A<JobProfileSalary>._)).Returns(1000);
            A.CallTo(() => salaryCalculator.GetExperiencedSalary(A<JobProfileSalary>._)).Returns(2000);

            var enhancer = new JobProfileIndexEnhancer(fakeJobProfileRepo, fakeJobProfileCategoryRepo, salaryService, salaryCalculator);
            enhancer.Initialise(dummyJobProfileIndex, isPublishing);

            await enhancer.PopulateSalary();

            dummyJobProfileIndex.SalaryExperienced.Should().Be(salaryExperiencedExpected);
            dummyJobProfileIndex.SalaryStarter.Should().Be(salaryStarterExpected);
            A.CallTo(() => fakeJobProfileRepo.GetByUrlNameForSearchIndex(A<string>._, isPublishing)).MustHaveHappened();
            A.CallTo(() => fakeJobProfileRepo.GetByUrlName(A<string>._)).MustNotHaveHappened();

            if (isSalaryOverriden)
            {
                A.CallTo(() => salaryService.GetSalaryBySocAsync(A<string>._)).MustNotHaveHappened();
                A.CallTo(() => salaryCalculator.GetStarterSalary(A<JobProfileSalary>._)).MustNotHaveHappened();
                A.CallTo(() => salaryCalculator.GetExperiencedSalary(A<JobProfileSalary>._)).MustNotHaveHappened();
            }
            else
            {
                A.CallTo(() => salaryService.GetSalaryBySocAsync(A<string>._)).MustHaveHappened();
                A.CallTo(() => salaryCalculator.GetStarterSalary(A<JobProfileSalary>._)).MustHaveHappened();
                A.CallTo(() => salaryCalculator.GetExperiencedSalary(A<JobProfileSalary>._)).MustHaveHappened();
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetRelatedFieldsWithUrlTest(bool isPublishing)
        {
            var fakeJobProfileRepo = A.Fake<IJobProfileRepository>();
            var fakeJobProfileCategoryRepo = A.Fake<IJobProfileCategoryRepository>();
            var salaryService = A.Fake<ISalaryService>();
            var salaryCalculator = A.Fake<ISalaryCalculator>();
            var dummyJobProfileIndex = A.Dummy<JobProfileIndex>();
            var dummyJobProfile = new JobProfile
            {
                RelatedInterests = new[] { "1|one", "2|two" }.AsQueryable(),
                RelatedEnablers = new[] { "1|one", "2|two" }.AsQueryable(),
                RelatedEntryQualifications = new[] { "1|one", "2|two" }.AsQueryable(),
                RelatedTrainingRoutes = new[] { "1|one", "2|two" }.AsQueryable(),
                RelatedPreferredTaskTypes = new[] { "1|one", "2|two" }.AsQueryable(),
                RelatedJobAreas = new[] { "1|one", "2|two" }.AsQueryable()
            };

            var dummyCategories = new List<JobProfileCategory>
            {
                new JobProfileCategory
                {
                    Title = "one",
                    Url = "url1"
                },
                new JobProfileCategory
                {
                    Title = "two",
                    Url = "url2"
                }
            };

            var expectedCategories = dummyCategories.Select(c => $"{c.Title}|{c.Url}");

            A.CallTo(() => fakeJobProfileRepo.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => fakeJobProfileRepo.GetByUrlNameForSearchIndex(A<string>._, A<bool>._)).Returns(dummyJobProfile);
            A.CallTo(() => fakeJobProfileCategoryRepo.GetByIds(A<IList<Guid>>._)).Returns(dummyCategories);

            var enhancer = new JobProfileIndexEnhancer(fakeJobProfileRepo, fakeJobProfileCategoryRepo, salaryService, salaryCalculator);
            enhancer.Initialise(dummyJobProfileIndex, isPublishing);
            enhancer.PopulateRelatedFieldsWithUrl();

            A.CallTo(() => fakeJobProfileRepo.GetByUrlNameForSearchIndex(A<string>._, isPublishing)).MustHaveHappened();
            A.CallTo(() => fakeJobProfileRepo.GetByUrlName(A<string>._)).MustNotHaveHappened();

            A.CallTo(() => fakeJobProfileCategoryRepo.GetByIds(A<IList<Guid>>._)).MustHaveHappened();

            dummyJobProfileIndex.JobProfileCategoriesWithUrl.Should().BeEquivalentTo(expectedCategories);
            dummyJobProfileIndex.Interests.Should().BeEquivalentTo(dummyJobProfile.RelatedInterests);
            dummyJobProfileIndex.Enablers.Should().BeEquivalentTo(dummyJobProfile.RelatedEnablers);
            dummyJobProfileIndex.EntryQualifications.Should().BeEquivalentTo(dummyJobProfile.RelatedEntryQualifications);
            dummyJobProfileIndex.TrainingRoutes.Should().BeEquivalentTo(dummyJobProfile.RelatedTrainingRoutes);
            dummyJobProfileIndex.JobAreas.Should().BeEquivalentTo(dummyJobProfile.RelatedJobAreas);
            dummyJobProfileIndex.PreferredTaskTypes.Should().BeEquivalentTo(dummyJobProfile.RelatedPreferredTaskTypes);
        }
    }
}