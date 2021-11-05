using SQLite;
using Justar.Models;

using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Justar.Services
{
    public static class StudentDatabase
    {
        public static ObservableCollection<Student> Items { get; } = new ObservableCollection<Student>();

        private static SQLiteAsyncConnection Connection
        {
            get
            {
                return connection == null ? connection = new SQLiteAsyncConnection(App.dbPath): connection;
            }
        }
        private static SQLiteAsyncConnection connection;

        public static async Task<CreateTableResult> Init()
        {
            var result = await Connection.CreateTableAsync<Student>();

            Items.Clear();

            foreach(var student in  await Connection.Table<Student>().ToListAsync())
            {
                Items.Add(student);
            }

            return result;
        }

        public static Task<List<Student>> GetStudents()
        {
            return Connection.Table<Student>().ToListAsync();
        }

        public static async Task Delete(Student t)
        {
            await Connection.DeleteAsync<Student>(t.Id);
            var index = Items.IndexOf(Items.First(f => f.Id == t.Id));
            Items.RemoveAt(index);
        }

        public static async Task Update(Student t)
        {
            await Connection.UpdateAsync(t);
            var index = Items.IndexOf(Items.First(f => f.Id == t.Id));
            Items[index] = t;
        }

        public static async Task<int> Insert(Student student)
        {
            var id = await Connection.InsertAsync(student);
            student.Id = id;
            Items.Add(student);
            return id;
        }

        public static async Task<Student> Select(int id)
        {
            var list = await Connection.QueryAsync<Student>($"SELECT * from Students WHERE Id='{id}'");
            Debug.WriteLine($"need id is {id}");
            return list.Where(student => student.Id == id).First();
        }


    }
}
