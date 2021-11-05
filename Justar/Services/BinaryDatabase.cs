using System;
using System.IO;
using System.Linq;
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
            public Dictionary<Guid, GuidStudent> students = new Dictionary<Guid, GuidStudent>();
            public List<DateTime> dateTimes = new List<DateTime>();
        }

        private static Database _database;

        public static void Init()
        {

            var formatter = new BinaryFormatter();
            
            using(var file = new FileStream(App.dbPath+".dat", FileMode.OpenOrCreate))
            {
                _database = (Database)formatter.Deserialize(file);
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
                Action = DefaultDictionary(),
            };
            _database.students.Add(student.Guid, student);
        }

        public static void SetReport(DateTime date, Guid guid, bool[] state)
        {
            var _student = _database.students.Where(f => f.Key == guid);
            if (_student.Any())
            {
                if(_database.dateTimes.Contains(date))
                {
                    _database.students[guid].Action[date] = state;
                    return;
                }
                _database.students[guid].Action.Add(date, state);
                foreach(var  i in _database.students)
                {
                    if(!i.Value.Action.ContainsKey(date))
                        i.Value.Action.Add(date, new bool[] { true, true, true, true });
                }
                _database.dateTimes.Add(date);
            }
        }

        private static Dictionary<DateTime, bool[]> DefaultDictionary()
        {
            var dict = new Dictionary<DateTime, bool[]>();
            foreach(var date in _database.dateTimes)
            {
                dict.Add(date, new bool[] { true, true, true, true });
            }
            return dict;
        }
    }
}
