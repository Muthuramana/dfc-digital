﻿using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Microsoft.Azure.Search;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Digital.Service.AzureSearch
{
    public class DfcSearchQueryService<T> : AzSearchQueryService<T>, ISearchQueryService<T>
        where T : class
    {
        private readonly ISearchQueryBuilder queryBuilder;
        private readonly ISearchManipulator<T> searchManipulator;

        public DfcSearchQueryService(
            ISearchIndexClient indexClient,
            IAzSearchQueryConverter queryConverter,
            ISearchQueryBuilder queryBuilder,
            ISearchManipulator<T> searchManipulator,
            IApplicationLogger applicationLogger)
            : base(indexClient, queryConverter, applicationLogger)
        {
            this.queryBuilder = queryBuilder;
            this.searchManipulator = searchManipulator;
        }

        public override SearchResult<T> Search(string searchTerm, SearchProperties properties)
        {
            var cleanedSearchTerm = queryBuilder.RemoveSpecialCharactersFromTheSearchTerm(searchTerm, properties);
            var trimmedSearchTerm = queryBuilder.TrimCommonWordsAndSuffixes(cleanedSearchTerm, properties);
            var partialTermToSearch = queryBuilder.BuildContainPartialSearch(trimmedSearchTerm, properties);
            var finalComputedSearchTerm = searchManipulator.BuildSearchExpression(searchTerm, cleanedSearchTerm, partialTermToSearch, properties);

            var searchProperties = properties ?? new SearchProperties();
            var res = base.Search(finalComputedSearchTerm, searchProperties);
            var orderedResult = searchManipulator.Reorder(res, searchTerm, searchProperties);

            return orderedResult;
        }

        public override async Task<SearchResult<T>> SearchAsync(string searchTerm, SearchProperties properties)
        {
            var cleanedSearchTerm = queryBuilder.RemoveSpecialCharactersFromTheSearchTerm(searchTerm, properties);
            var trimmedSearchTerm = queryBuilder.TrimCommonWordsAndSuffixes(cleanedSearchTerm, properties);
            var partialTermToSearch = queryBuilder.BuildContainPartialSearch(trimmedSearchTerm, properties);
            var finalComputedSearchTerm = searchManipulator.BuildSearchExpression(searchTerm, cleanedSearchTerm, partialTermToSearch, properties);

            var searchProperties = properties ?? new SearchProperties();
            var res = await base.SearchAsync(finalComputedSearchTerm, searchProperties);
            var orderedResult = searchManipulator.Reorder(res, searchTerm, searchProperties);

            return orderedResult;
        }
    }
}