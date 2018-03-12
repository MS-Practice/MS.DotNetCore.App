using System;
using System.Collections.Generic;
using System.Text;

namespace ResilientHttpClientByPolly
{
    public interface IResilientHttpClientFactory
    {
        ResilientHttpClient CreateResilientHttpClient();
    }
}
