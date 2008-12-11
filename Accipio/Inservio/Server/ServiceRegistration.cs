using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Accipio.Inservio.Server
{
    public class ServiceRegistration
    {
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private Dictionary<string, MethodRegistration> methods = new Dictionary<string, MethodRegistration>();
    }
}
