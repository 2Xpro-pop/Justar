using SQLite;
using Justar.Models;

using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Justar.Services
{
    public static class ReportDatabase
    {
        private static SQLiteAsyncConnection Connection
        {
            get
            {
                return connection == null ? connection = new SQLiteAsyncConnection(App.dbPath) : connection;
            }
        }
        private static SQLiteAsyncConnection connection;

        public static async Task<CreateTableResult> Init()
        {
            var result = await Connection.CreateTableAsync<Report>();
            var list = await GetReports();
            Debug.WriteLine($"количество репортов {list.Count}");
            return result;
        }

        public static Task<List<Report>> GetReports()
        {
            return Connection.Table<Report>().ToListAsync();
        }

        public static async Task Delete(Report t)
        {
            await Connection.DeleteAsync<Report>(t.Id);
        }

        public static async Task DeleteStudent(Student t)
        {
            await Connection.ExecuteAsync($"DELETE FROM Reports WHERE StudentId='{t.Id}'");
        }

        public static async Task DeleteStudent(int id)
        {
            await Connection.ExecuteAsync($"DELETE FROM Reports WHERE StudentId='{id}'");
        }

        public static async Task Update(Report t)
        {
            await Connection.UpdateAsync(t);
        }

        public static async Task Insert(Report report)
        {
            var id = await Connection.InsertAsync(report);
            report.Id = id;
        }

        public static async Task<List<Report>> SelectReports(DateTime date)
        {
            return await Connection.QueryAsync<Report>($"SELECT * FROM Reports WHERE Date='{date.Date.ToString(Report.DateReportFormat)}'");
        }

        public static async Task InsertDate(List<Report> reports)
        {
            await Connection.ExecuteAsync($"DELETE FROM Reports WHERE Date='{reports[0].Date}'");
            await Connection.InsertAllAsync(reports);
        }

        public static async void UpdateStudents(List<Report> reports)
        {
            foreach(var report in reports)
            {
                try
                {
                    var student = await StudentDatabase.Select(report.StudentId);
                    student.State = report.StudentState;
                    await StudentDatabase.Update(student);
                }
                catch
                {
                    await DeleteStudent(report.StudentId);
                }
            }
        }

    }
}
