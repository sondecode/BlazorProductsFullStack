﻿using BlazorProducts.Client.HttpRepository;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorProducts.Client.Pages
{
    public partial class Products
    {
        private MudTable<Product> _table;
        private ProductParameters _productParameters = new ProductParameters();
        private readonly int[] _pageSizeOption = { 4, 6, 10 };
        [Inject]
        public IProductHttpRepository Repository { get; set; }
        private async Task<TableData<Product>> GetServerData(TableState state)
        {
            _productParameters.PageSize = state.PageSize;
            _productParameters.PageNumber = state.Page + 1;

            _productParameters.OrderBy = state.SortDirection == SortDirection.Descending ?
                state.SortLabel + " desc" :
                state.SortLabel;

            var response = await Repository.GetProducts(_productParameters);
            return new TableData<Product>
            {
                Items = response.Items,
                TotalItems = response.MetaData.TotalCount
            };
        }
        private void OnSearch(string searchTerm)
        {
            _productParameters.SearchTerm = searchTerm;
            _table.ReloadServerData();
        }
    }
}
