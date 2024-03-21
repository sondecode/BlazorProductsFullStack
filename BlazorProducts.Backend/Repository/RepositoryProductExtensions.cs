using Entities.Models;

namespace BlazorProducts.Backend.Repository
{
	public static class RepositoryProductExtensions
	{
		public static IQueryable<Product> Search (this IQueryable<Product> products, string? searchTerm )
		{
			if (string.IsNullOrWhiteSpace(searchTerm)) return products;

			var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

			return products.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm));
		}
	}
}
