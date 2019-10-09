using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Extensions;
using Catalog.API.Infrastructure;
using Catalog.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogContext _catalogContext;
        private readonly CatalogSettings _settings;
        
        public CatalogController(CatalogContext context, IOptionsSnapshot<CatalogSettings> settings )
        {
            _catalogContext = context ?? throw new ArgumentNullException(nameof(context));
            _settings = settings.Value;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        private async Task<List<CatalogItem>> GetItemsByIdsAsync(string ids)
        {
            var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));

            if (!numIds.All(nid => nid.Ok))
            {
                return new List<CatalogItem>();
            }

            var idsToSelect = numIds
                .Select(id => id.Value);

            var items = await _catalogContext.CatalogItems.Where(ci => idsToSelect.Contains(ci.Id)).ToListAsync();

            items = ChangeUriPlaceholder(items);

            return items;
        }
        
        private List<CatalogItem> ChangeUriPlaceholder(List<CatalogItem> items)
        {
            var baseUri = _settings.PicBaseUrl;
            var azureStorageEnabled = _settings.AzureStorageEnabled;

            foreach (var item in items)
            {
                item.FillProductUrl(baseUri, azureStorageEnabled: azureStorageEnabled);
            }

            return items;
        }
        
    }
}