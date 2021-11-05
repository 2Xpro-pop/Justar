using System;
using SQLite;

namespace Justar.Models
{
    [Table("Students")]
    public class Student: Item
    {

        public string Fio { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [Ignore]
        public StudentActionState State { get; set; } = StudentActionState.Absent;

        public Report MakeReport(DateTime date)
        {
            return new Report(this, date.Date, " ");
        }
    }

    public enum StudentActionState
    {
        Present,
        Late,
        Absent,
    }
}
