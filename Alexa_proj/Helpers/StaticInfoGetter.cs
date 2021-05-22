using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Alexa_proj.Helpers
{
    class StaticInfoGetter
    {
        public HttpRequestHeaders HttpRequestHeaders { get; init; } = default;

        public StaticInfoGetter()
        {

        }

        public StaticInfoGetter(HttpRequestHeaders httpRequestHeaders)
        {
            HttpRequestHeaders = httpRequestHeaders;
        }

        
    }
}
