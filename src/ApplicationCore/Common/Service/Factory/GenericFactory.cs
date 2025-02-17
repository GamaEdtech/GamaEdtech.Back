﻿namespace GamaEdtech.Backend.Common.Service.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using GamaEdtech.Backend.Common.Data.Enumeration;

    public class GenericFactory<TProvider, TProviderType>(IEnumerable<TProvider> providers) : IGenericFactory<TProvider, TProviderType>
        where TProvider : IProvider<TProviderType>
        where TProviderType : Enumeration<byte>
    {
        public TProvider? GetProvider(TProviderType providerType, bool returnFirstItemIfNotMatch = false)
        {
            ArgumentNullException.ThrowIfNull(providers);

            var provider = providers.FirstOrDefault(t => t.ProviderType.Equals(providerType));
            if (provider is null && returnFirstItemIfNotMatch)
            {
                provider = providers.First();
            }

            return provider;
        }
    }
}
