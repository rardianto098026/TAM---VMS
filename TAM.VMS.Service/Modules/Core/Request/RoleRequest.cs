using System;
using System.Collections.Generic;

namespace TAM.VMS.Service
{
    public class RoleRequest
    {
        public IEnumerable<string> Permissions { get; set; }
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
