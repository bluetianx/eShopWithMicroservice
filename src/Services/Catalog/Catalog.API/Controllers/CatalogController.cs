using System;
using Catalog.API.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Catalog.API.Controllers
{
    public class CatalogController : Controller
    {
        private readonly CatalogContext _catalogContext;
        private readonly CatalogSettings _settings;
        
        public CatalogController(CatalogContext context, IOptionsSnapshot<CatalogSettings> settings )
        {
            _catalogContext = context ?? throw new ArgumentNullException(nameof(context));
            _settings = settings.Value;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        
    }
}