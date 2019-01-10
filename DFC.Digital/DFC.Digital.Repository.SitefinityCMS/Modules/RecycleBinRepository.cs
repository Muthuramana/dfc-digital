﻿using DFC.Digital.Data.Interfaces;
using System.Linq;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.RecycleBin;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.SitefinityExceptions;
using Telerik.Sitefinity.Utilities.TypeConverters;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RecycleBinRepository : IRecyleBinRepository
    {
        private const string ApprenticeVacancyDeleteTypeName = "Telerik.Sitefinity.DynamicTypes.Model.JobProfile.ApprenticeshipVacancy";

        public void DeleteVacanciesPermanently(int itemCount)
        {
            using (var recycleBinItemsManager = RecycleBinManagerFactory.GetManager())
            {
                SystemManager.RunWithElevatedPrivilege(d =>
                {
                    var recycleBinItems = recycleBinItemsManager.GetRecycleBinItems()
                        .Where(di => di.DeletedItemTypeName.Equals(ApprenticeVacancyDeleteTypeName)).Take(itemCount).ToList();
                    var providerName = DynamicModuleManager.GetDefaultProviderName(DynamicTypes.JobProfileModuleName);

                    using (var dynamicModuleManager = DynamicModuleManager.GetManager(providerName))
                    {
                        var dynamicModuleContentType = TypeResolutionService.ResolveType(ApprenticeVacancyDeleteTypeName);
                        foreach (var recycleBinDataItem in recycleBinItems)
                        {
                            try
                            {
                                var dataItem =
                                    dynamicModuleManager.GetItem(dynamicModuleContentType, recycleBinDataItem.DeletedItemId) as
                                        IRecyclableDataItem;
                                dynamicModuleManager.RecycleBin.PermanentlyDeleteFromRecycleBin(dataItem);
                                dynamicModuleManager.SaveChanges();
                            }
                            catch (ItemNotFoundException)
                            {
                                recycleBinItemsManager.Delete(recycleBinDataItem);
                            }
                        }
                    }
                });
                recycleBinItemsManager.SaveChanges();
            }
        }
    }
}