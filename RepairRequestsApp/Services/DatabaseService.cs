using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Dapper;
using RepairRequestsApp.Models;

namespace RepairRequestsApp.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RepairRequests.db");
            _connectionString = $"Data Source={dbPath};Version=3;";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS RepairRequests (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        RoomNumber TEXT NOT NULL,
                        Description TEXT NOT NULL,
                        RequestedBy TEXT NOT NULL,
                        RequestDate TEXT NOT NULL,
                        Status TEXT NOT NULL,
                        Priority TEXT NOT NULL,
                        ContactPhone TEXT,
                        CompletionDate TEXT,
                        Notes TEXT
                    )";
                connection.Execute(createTableQuery);
            }
        }

        public List<RepairRequest> GetAllRequests()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                return connection.Query<RepairRequest>("SELECT * FROM RepairRequests ORDER BY RequestDate DESC").ToList();
            }
        }

        public RepairRequest GetRequestById(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<RepairRequest>("SELECT * FROM RepairRequests WHERE Id = @Id", new { Id = id });
            }
        }

        public void AddRequest(RepairRequest request)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string insertQuery = @"
                    INSERT INTO RepairRequests (RoomNumber, Description, RequestedBy, RequestDate, Status, Priority, ContactPhone, CompletionDate, Notes)
                    VALUES (@RoomNumber, @Description, @RequestedBy, @RequestDate, @Status, @Priority, @ContactPhone, @CompletionDate, @Notes)";
                connection.Execute(insertQuery, request);
            }
        }

        public void UpdateRequest(RepairRequest request)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string updateQuery = @"
                    UPDATE RepairRequests 
                    SET RoomNumber = @RoomNumber, 
                        Description = @Description, 
                        RequestedBy = @RequestedBy, 
                        RequestDate = @RequestDate, 
                        Status = @Status, 
                        Priority = @Priority, 
                        ContactPhone = @ContactPhone, 
                        CompletionDate = @CompletionDate, 
                        Notes = @Notes
                    WHERE Id = @Id";
                connection.Execute(updateQuery, request);
            }
        }

        public void DeleteRequest(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Execute("DELETE FROM RepairRequests WHERE Id = @Id", new { Id = id });
            }
        }

        public List<RepairRequest> SearchRequests(string searchTerm)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string searchQuery = @"
                    SELECT * FROM RepairRequests 
                    WHERE RoomNumber LIKE @SearchTerm 
                       OR Description LIKE @SearchTerm 
                       OR RequestedBy LIKE @SearchTerm
                       OR Status LIKE @SearchTerm
                    ORDER BY RequestDate DESC";
                return connection.Query<RepairRequest>(searchQuery, new { SearchTerm = $"%{searchTerm}%" }).ToList();
            }
        }

        public List<RepairRequest> FilterByStatus(string status)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                return connection.Query<RepairRequest>(
                    "SELECT * FROM RepairRequests WHERE Status = @Status ORDER BY RequestDate DESC", 
                    new { Status = status }).ToList();
            }
        }
    }
}
