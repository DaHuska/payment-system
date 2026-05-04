using System;
using System.Security.Cryptography;
using System.Text;
using is_payment_system.Model.Enums;
using is_payment_system.Service;
using MySqlConnector;

namespace is_payment_system.SampleData
{
    /// <summary>
    /// Populates the database with a curated set of test data covering every table
    /// (Users, Merchants, MerchantOwners, Cards, Transactions, Logs) so the full
    /// admin/client/merchant flow plus the DB Tables and Logs windows can be exercised
    /// end-to-end without manually creating records.
    ///
    /// Idempotent: if the Users table already has rows, Populate() is a no-op. Use
    /// Repopulate() to wipe and reseed (handy during development).
    /// </summary>
    public static class SampleDataPopulator
    {
        public static void Populate()
        {
            if (IsAlreadyPopulated())
            {
                Console.WriteLine("[SampleData] Database already contains data, skipping.");
                return;
            }

            Console.WriteLine("[SampleData] Populating database with test data...");
            Seed();
        }

        public static void Repopulate()
        {
            Console.WriteLine("[SampleData] Wiping and reseeding database...");
            using (var con = DbConnection.CreateOpen())
            {
                ClearAll(con);
            }

            Seed();
        }

        private static bool IsAlreadyPopulated()
        {
            using var con = DbConnection.CreateOpen();
            using var cmd = new MySqlCommand("SELECT COUNT(*) FROM Users;", con);
            var count = Convert.ToInt32(cmd.ExecuteScalar());
            return count > 0;
        }

        private static void Seed()
        {
            using var con = DbConnection.CreateOpen();
            using var tx = con.BeginTransaction();

            try
            {
                SeedUsers(con, tx);
                SeedMerchants(con, tx);
                SeedMerchantOwners(con, tx);
                SeedCards(con, tx);
                SeedTransactions(con, tx);
                SeedLogs(con, tx);

                tx.Commit();
                Console.WriteLine("[SampleData] Done.");
                PrintSummary(con);
            }
            catch (Exception ex)
            {
                tx.Rollback();
                Console.WriteLine($"[SampleData] FAILED: {ex.Message}");
                throw;
            }
        }

        private static void ClearAll(MySqlConnection con)
        {
            // Delete in FK-safe order: leaves first, roots last.
            Execute(con, "DELETE FROM Logs;");
            Execute(con, "DELETE FROM Transactions;");
            Execute(con, "DELETE FROM Cards;");
            Execute(con, "DELETE FROM MerchantOwners;");
            Execute(con, "DELETE FROM Merchants;");
            Execute(con, "DELETE FROM Users;");

            // Reset auto-increment counters so seeded ids are predictable (1, 2, 3, ...).
            Execute(con, "ALTER TABLE Users AUTO_INCREMENT = 1;");
            Execute(con, "ALTER TABLE Merchants AUTO_INCREMENT = 1;");
            Execute(con, "ALTER TABLE Cards AUTO_INCREMENT = 1;");
            Execute(con, "ALTER TABLE Transactions AUTO_INCREMENT = 1;");
            Execute(con, "ALTER TABLE Logs AUTO_INCREMENT = 1;");
        }

        // ──────────────────────────────────────────────────────────────────
        // Users — 1 admin + 4 regular users with mixed active/inactive state.
        // ──────────────────────────────────────────────────────────────────
        private static void SeedUsers(MySqlConnection con, MySqlTransaction tx)
        {
            const string sql = @"
                INSERT INTO Users (Id, FirstName, LastName, Email, Password, Role, DateCreated, IsActive)
                VALUES (@id, @firstName, @lastName, @email, @password, @role, @dateCreated, @isActive);";

            // (Id, FirstName, LastName, Email, Password, Role, daysAgo, IsActive)
            var users = new (int, string, string, string, string, UserRole, int, bool)[]
            {
                (1, "Иван",   "Петров",     "ivan.petrov@payment.bg",        hashPass("admin123"),   UserRole.ADMIN, 120, true),
                (2, "Мария",  "Георгиева",  "maria.georgieva@example.com",   hashPass("1234567"),    UserRole.USER,   90, true),
                (3, "Георги", "Димитров",   "georgi.dimitrov@example.com",   hashPass("12345678"),   UserRole.USER,   60, true),
                (4, "Петър",  "Иванов",     "petar.ivanov@example.com",      hashPass("qwerty"),     UserRole.USER,   45, false),
                (5, "Анна",   "Стоянова",   "anna.stoyanova@example.com",    hashPass("annapass"),   UserRole.USER,   10, true),
            };

            foreach (var (id, first, last, email, password, role, daysAgo, isActive) in users)
            {
                using var cmd = new MySqlCommand(sql, con, tx);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@firstName", first);
                cmd.Parameters.AddWithValue("@lastName", last);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@role", (int)role);
                cmd.Parameters.AddWithValue("@dateCreated", DateTime.Now.AddDays(-daysAgo));
                cmd.Parameters.AddWithValue("@isActive", isActive);
                cmd.ExecuteNonQuery();
            }
        }

        // ──────────────────────────────────────────────────────────────────
        // Merchants — three businesses with different statuses and balances.
        // ──────────────────────────────────────────────────────────────────
        private static void SeedMerchants(MySqlConnection con, MySqlTransaction tx)
        {
            const string sql = @"
                INSERT INTO Merchants (Id, BusinessName, Email, Phone, Balance, Status, CreatedAt)
                VALUES (@id, @businessName, @email, @phone, @balance, @status, @createdAt);";

            var merchants = new (int, string, string, string, decimal, MerchantStatus, int)[]
            {
                (1, "Кафе Аромат",       "contact@kafe-aromat.bg",   "+359 88 123 4567", 1250.50m, MerchantStatus.ACTIVE,     80),
                (2, "TechStore Online",   "support@techstore.bg",     "+359 2 987 6543",  8430.00m, MerchantStatus.ACTIVE,     70),
                (3, "Ресторант Море",     "info@restoran-more.bg",    "+359 88 555 7777",    0.00m, MerchantStatus.PENDING,    15),
            };

            foreach (var (id, name, email, phone, balance, status, daysAgo) in merchants)
            {
                using var cmd = new MySqlCommand(sql, con, tx);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@businessName", name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@balance", balance);
                cmd.Parameters.AddWithValue("@status", (int)status);
                cmd.Parameters.AddWithValue("@createdAt", DateTime.Now.AddDays(-daysAgo));
                cmd.ExecuteNonQuery();
            }
        }

        // ──────────────────────────────────────────────────────────────────
        // MerchantOwners — M2M between Users and Merchants.
        // Maria (user 2) owns merchants 1 and 2; Georgi (user 3) owns merchant 3.
        // ──────────────────────────────────────────────────────────────────
        private static void SeedMerchantOwners(MySqlConnection con, MySqlTransaction tx)
        {
            const string sql = @"
                INSERT INTO MerchantOwners (MerchantId, UserId)
                VALUES (@merchantId, @userId);";

            var links = new (int merchantId, int userId)[]
            {
                (1, 2),
                (2, 2),
                (3, 3),
            };

            foreach (var (merchantId, userId) in links)
            {
                using var cmd = new MySqlCommand(sql, con, tx);
                cmd.Parameters.AddWithValue("@merchantId", merchantId);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.ExecuteNonQuery();
            }
        }

        // ──────────────────────────────────────────────────────────────────
        // Cards — mix of active and expired cards for different users.
        // Card 4 is intentionally expired so DeleteExpiredCards() can be tested.
        // ──────────────────────────────────────────────────────────────────
        private static void SeedCards(MySqlConnection con, MySqlTransaction tx)
        {
            const string sql = @"
                INSERT INTO Cards (Id, UserId, CardNumber, CVV, Iban, CreatedDate, ExpirationDate)
                VALUES (@id, @userId, @cardNumber, @cvv, @iban, @createdDate, @expirationDate);";

            // (Id, UserId, CardNumber, CVV, Iban, createdDaysAgo, expiresInDays)
            var cards = new (int, int, string, string, string, int, int)[]
            {
                (1, 2, "4532 1234 5678 9012", "123", "BG80BNBG96611020345678", 60,  365 * 3),   // Maria's Visa
                (2, 2, "5555 9876 5432 1000", "456", "BG18RZBB91550123456789", 45,  365 * 2),   // Maria's Mastercard
                (3, 3, "3782 8224 6310 005",  "789", "BG12UNCR70001523456789", 30,  365 * 4),   // Georgi's AmEx
                (4, 2, "6011 2233 4455 6677", "321", "BG80BNBG96611020345678", 400, -10),       // Maria's expired Visa
                (5, 5, "4916 5577 1234 9876", "654", "BG24STSA93000026543210",  5,  365),       // Anna's Visa
            };

            foreach (var (id, userId, number, cvv, iban, createdDaysAgo, expiresInDays) in cards)
            {
                using var cmd = new MySqlCommand(sql, con, tx);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@cardNumber", number);
                cmd.Parameters.AddWithValue("@cvv", cvv);
                cmd.Parameters.AddWithValue("@iban", iban);
                cmd.Parameters.AddWithValue("@createdDate", DateTime.Now.AddDays(-createdDaysAgo));
                cmd.Parameters.AddWithValue("@expirationDate", DateTime.Now.AddDays(expiresInDays));
                cmd.ExecuteNonQuery();
            }
        }

        // ──────────────────────────────────────────────────────────────────
        // Transactions — different statuses, senders, merchants and ages.
        // Transaction 6 is old enough to test DeleteOldTransactions().
        // ──────────────────────────────────────────────────────────────────
        private static void SeedTransactions(MySqlConnection con, MySqlTransaction tx)
        {
            const string sql = @"
                INSERT INTO Transactions (Id, Amount, Timestamp, Status, SenderId, MerchantId)
                VALUES (@id, @amount, @timestamp, @status, @senderId, @merchantId);";

            // (Id, Amount, hoursAgo, Status, SenderId, MerchantId)
            var transactions = new (int, decimal, int, TransactionStatus, int, int)[]
            {
                (1,  150.00m,    24 * 5, TransactionStatus.COMPLETED, 2, 1),  // Maria → Кафе Аромат, 5 days ago
                (2,   45.50m,    24 * 3, TransactionStatus.COMPLETED, 2, 2),  // Maria → TechStore, 3 days ago
                (3,  200.00m,    24 * 1, TransactionStatus.PENDING,   3, 2),  // Georgi → TechStore, 1 day ago
                (4,   99.99m,         2, TransactionStatus.FAILED,    5, 1),  // Anna → Кафе Аромат, 2 hours ago
                (5, 1250.00m,        12, TransactionStatus.COMPLETED, 2, 2),  // Maria → TechStore, 12 hours ago
                (6,   35.00m,   24 * 30, TransactionStatus.COMPLETED, 3, 1),  // Georgi → Кафе Аромат, 30 days ago
            };

            foreach (var (id, amount, hoursAgo, status, senderId, merchantId) in transactions)
            {
                using var cmd = new MySqlCommand(sql, con, tx);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@timestamp", DateTime.Now.AddHours(-hoursAgo));
                cmd.Parameters.AddWithValue("@status", (int)status);
                cmd.Parameters.AddWithValue("@senderId", senderId);
                cmd.Parameters.AddWithValue("@merchantId", merchantId);
                cmd.ExecuteNonQuery();
            }
        }

        // ──────────────────────────────────────────────────────────────────
        // Logs — sample events from both FileLogger and HashLogger.
        // Lets the LogsWindow be tested without needing to manually trigger logs.
        // ──────────────────────────────────────────────────────────────────
        private static void SeedLogs(MySqlConnection con, MySqlTransaction tx)
        {
            const string sql = @"
                INSERT INTO Logs (EventId, Message, Timestamp, LoggerType)
                VALUES (@eventId, @message, @timestamp, @loggerType);";

            // (EventId, Message, minutesAgo, LoggerType)
            var logs = new (string, string, int, string)[]
            {
                ("INFO",         "Application started successfully.",                                  60, "FileLogger"),
                ("INFO",         "User 'ivan.petrov@payment.bg' logged in.",                            50, "FileLogger"),
                ("TRANSACTION",  "New transaction recorded: 150.00 BGN, sender=2, merchant=1.",        45, "HashLogger"),
                ("WARNING",      "Failed login attempt for 'maria.georgieva@example.com'.",            30, "FileLogger"),
                ("ERROR",        "Database connection timeout, retrying... | Exception: TimeoutException: Connection timed out after 30 seconds.", 20, "FileLogger"),
                ("TRANSACTION",  "Transaction 5 marked as COMPLETED.",                                 12, "HashLogger"),
                ("INFO",         "Card 4 detected as expired and flagged for cleanup.",                  5, "FileLogger"),
            };

            foreach (var (eventId, message, minutesAgo, loggerType) in logs)
            {
                using var cmd = new MySqlCommand(sql, con, tx);
                cmd.Parameters.AddWithValue("@eventId", eventId);
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters.AddWithValue("@timestamp", DateTime.Now.AddMinutes(-minutesAgo));
                cmd.Parameters.AddWithValue("@loggerType", loggerType);
                cmd.ExecuteNonQuery();
            }
        }

        private static void PrintSummary(MySqlConnection con)
        {
            Console.WriteLine($"  Users:           {Count(con, "Users"),3}");
            Console.WriteLine($"  Merchants:       {Count(con, "Merchants"),3}");
            Console.WriteLine($"  MerchantOwners:  {Count(con, "MerchantOwners"),3}");
            Console.WriteLine($"  Cards:           {Count(con, "Cards"),3}");
            Console.WriteLine($"  Transactions:    {Count(con, "Transactions"),3}");
            Console.WriteLine($"  Logs:            {Count(con, "Logs"),3}");
        }

        private static int Count(MySqlConnection con, string table)
        {
            using var cmd = new MySqlCommand($"SELECT COUNT(*) FROM {table};", con);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private static void Execute(MySqlConnection con, string sql)
        {
            using var cmd = new MySqlCommand(sql, con);
            cmd.ExecuteNonQuery();
        }
        
        private static string hashPass(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }
    }
}
