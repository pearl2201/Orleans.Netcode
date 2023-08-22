﻿using Microsoft.Extensions.Hosting;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netcode.Orleans.Config
{
 
    public class NetOrleansSiloHostConfigBuilder : NetOrleansConfigBaseBuilder
    {
        internal Action<ISiloBuilder> ConfigureBuilder { get; set; } = default!;

        /// <summary>
        /// Configure builder, such as providers.
        /// </summary>
        /// <param name="configure">Configure action. This may be called multiple times.</param>
        public NetOrleansSiloHostConfigBuilder Configure(Action<ISiloBuilder> configure)
        {
            ConfigureBuilder += configure;
            return this;
        }
    }

}
