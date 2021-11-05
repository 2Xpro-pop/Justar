using System;
using System.IO;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using Justar.Models;

namespace Justar.Services
{
    public static class BinaryDatabase
    {
        [Serializable]
        class Database
        {
            public List<GuidStudent> students = new List<GuidStudent>();
            public List<DateTime> dateTimes = new List<DateTime>();
            public DataTable actions = new DataTable();
        }

        private static Database _database;

        public static void Init()
        {

            var formatter = new BinaryFormatter();
            
            using(var file = new FileStream(App.dbPath+".dat", FileMode.OpenOrCreate))
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
            return _database.students;
        }

        public static void AddStudent(string fio)
        {
            var student = new GuidStudent()
            {
                Fio = fio,
                Guid = Guid.NewGuid(),
            };
            _database.students.Add(student);

            var column = new DataColumn(student.Guid.ToString(), typeof(bool[]));
            _database.actions.Columns.Add(column);
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

    }
}
