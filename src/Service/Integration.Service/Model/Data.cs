using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integration.Service.Model
{
    [Serializable]
    public class Data
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public string Country { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal TotalVatAmount { get; set; }
    }
}
