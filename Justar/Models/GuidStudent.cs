using System;
using System.Collections.Generic;
using System.Text;

namespace Justar.Models
{
    public class GuidStudent
    {
        public Guid Guid { get; set; }
        public string Fio { get; set; }
        public Dictionary<DateTime, bool[]> Action { get; set; }
    }
}
