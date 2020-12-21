using System.Collections.Generic;

namespace AuthRefreshApi.Configuration
{
    public class CorsDomainsConfigSection
    {
        public IEnumerable<string> Values {get;set;}
    }
}