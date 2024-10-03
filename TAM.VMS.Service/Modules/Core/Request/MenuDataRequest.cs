using System;

namespace TAM.VMS.Service
{
    public class MenuDataRequest
    {
        public Guid ID { get; set; }
        public Guid MenuGroupID { get; set; }
        public Guid? ParentID { get; set; }
        public int OrderIndex { get; set; }
        public string Title { get; set; }
    }
}
