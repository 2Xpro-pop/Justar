using System;
using System.IO;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using Justar.Views;
using Justar.Models;

namespace Justar.Services
{
    public static class BinaryDatabase
    {
        [Serializable]
        class Database
        {
            public Dictionary<Guid,GuidStudent> students = new Dictionary<Guid,GuidStudent>();
            public List<DateTime> dateTimes = new List<DateTime>();
            public DataTable actions = new DataTable();
        }

        private static Database _database;

        public static void Init()
        {

            var formatter = new BinaryFormatter();

            using(var file = new FileStream(App.dbPath + ".dat", FileMode.OpenOrCreate))
            {
                _database = (Database)formatter.Deserialize(file);
            }

            if (!_database.actions.Columns.Contains("dates"))
            {
                var column = new DataColumn("dates", typeof(DateTime));
                column.Unique = true;
                _database.actions.Columns.Add(column);
                _database.actions.PrimaryKey = new DataColumn[] { _database.actions.Columns["dates"] };
            }
        }

        public static void Save()
        {
            var formatter = new BinaryFormatter();

            using(var file = new FileStream(App.dbPath+".dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(file, _database);
            }
        }

        public static IReadOnlyList<GuidStudent> GetStudents()
        {
            return _database.students.Values.ToList();
        }

        public static void AddStudent(string fio)
        {
            var student = new GuidStudent()
            {
                Fio = fio,
                Guid = Guid.NewGuid(),
            };
            _database.students.Add(student.Guid, student);

            var column = new DataColumn(student.Guid.ToString(), typeof(bool[]));
            _database.actions.Columns.Add(column);
        }

        public static void UpdateFio(Guid guid, string fio)
        {
            _database.students[guid].Fio = fio;
        }

        public static void DeleteStudent(Guid guid)
        {
            _database.students.Remove(guid);
            _database.actions.Columns.Remove(guid.ToString());
        }

        public static void SetReport(DateTime date, Guid guid, bool[] state)
        {
           if(_database.actions.Rows.Contains(date))
           {
                _database.actions.Rows.Find(date)[guid.ToString()] = state;
           }
           else
            {
                var row = _database.actions.NewRow();
                row["dates"] = date;
                row[guid.ToString()] = state;
                _database.actions.Rows.Add(row);
            }
        }

        public static GuidStudent GetStudent(Guid guid)
        {
            return _database.students[guid];
        }

        public static string TextInfo(DateTime date, AboutPage.ViewStudents viewStudents)
        {
            var builder = new System.Text.StringBuilder();
            var row = _database.actions.Rows.Find(date);
            foreach (var column in GetStudents())
            {
                if (viewStudents == AboutPage.ViewStudents.Absent)
                {
                    var value = (bool[])row[column.Guid.ToString()];
                    int summ = value.Sum(f => f ? 1 : 0);
                    if (summ > 0)
                    {
                        builder.AppendLine(column.Fio);
                        builder.Append(" пропустил пар ");
                        builder.Append(summ);
                    }
                }
                if(viewStudents == AboutPage.ViewStudents.Present)
                {
                    var value = (bool[])row[column.Guid.ToString()];
                    int summ = value.Sum(f => f ? 1 : 0);
                    if (summ == 0)
                    {
                        builder.AppendLine(column.Fio);
                        builder.Append(" был на парах ");
                    }
                }
                if (viewStudents == AboutPage.ViewStudents.AbsentAndPresent)
                {
                    var value = (bool[])row[column.Guid.ToString()];
                    int summ = value.Sum(f => f ? 1 : 0);
                    builder.AppendLine(column.Fio);
                    builder.Append( summ > 0 ? $" пропустил пар {summ}" : " был на парах ");
                }
            }
            return builder.ToString();
        }

    }
}
