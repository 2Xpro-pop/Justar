using SQLite;
using System;

namespace Justar.Models
{
    [Table("Reports")]
    public class Report: Item
    {
        public const string DateReportFormat = "d";
        static string[] Actions = new string[] { "присутствовал", "опаздал", "отсутствовал" };

        public string Date { get; set; }
        public string Action { get; set; }
        public int StudentId { get; set; }
        public string Addition { get; set; }
        public StudentActionState StudentState { get; set; } 

        public Report()
        {

        }

        public Report(Student student, DateTime date, string addition)
        {
            addition = string.IsNullOrWhiteSpace(addition) ? "" : ". Дополнительная инофрмация:" + addition;
            Action = $"{student.Fio} {Actions[(int)student.State]}{addition}";
            StudentId = student.Id;
            Date = date.Date.ToString(DateReportFormat);
            Addition = addition;
            StudentState = student.State;
        }

        public DateTime DateFormat()
        {
            return DateTime.ParseExact(Date, DateReportFormat, null);
        }
    }
        
}
