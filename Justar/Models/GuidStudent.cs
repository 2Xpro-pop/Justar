using System;
using System.Collections.Generic;
using System.Text;

namespace Justar.Models
{
    [Serizable]
    public class GuidStudent
    {
        public Guid Guid { get; set; }
        public string Fio { get; set; }
    }
}
