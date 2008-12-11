using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Accipio.Inservio.Framework;

namespace Accipio.Inservio.Server
{
    public class ServicesRepository
    {
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private Dictionary<string, ServicePlugInRegistration> plugIns = new Dictionary<string, ServicePlugInRegistration>();
    }
}
