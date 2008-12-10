using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accipio.Inservio.Framework;

namespace Accipio.Inservio.Server
{
    public class ServicesRepository
    {
        private Dictionary<string, ServicePlugInRegistration> plugIns = new Dictionary<string, ServicePlugInRegistration>();
    }
}
