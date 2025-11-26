using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Domain.Statistics
{
    public class TrackFilter
    {
        public static TrackFilter Empty => new TrackFilter();

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
