using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using Entities.Models;

namespace BlazorProducts.Backend.Repository.RepoExtensions
{
    public static class RepositoryOrderExtensions
    {
        public static IQueryable<Order> Search(this IQueryable<Order> products, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return products;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return products.Where(p => p.CustomerName.ToLower().Contains(lowerCaseSearchTerm));
        }

        public static IQueryable<Order> Sort(this IQueryable<Order> products, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return products.OrderBy(e => e.CustomerName);

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Order).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    continue;

                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name} {direction}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            if (string.IsNullOrWhiteSpace(orderQuery))
                return products.OrderBy(e => e.CustomerName);

            return products.OrderBy(orderQuery);
        }
    }
}
