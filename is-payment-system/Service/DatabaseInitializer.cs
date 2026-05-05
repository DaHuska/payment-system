    using MySqlConnector;

    namespace is_payment_system.Service
    {
        public static class DatabaseInitializer
        {
            public static void Initialize()
            {
                using var con = DbConnection.CreateOpen();
                
                Execute(con, @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        FirstName VARCHAR(100) NOT NULL,
                        LastName VARCHAR(100) NOT NULL,
                        Email VARCHAR(255) NOT NULL UNIQUE,
                        Password VARCHAR(255) NOT NULL,
                        Role INT NOT NULL,
                        DateCreated DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        IsActive BOOLEAN NOT NULL DEFAULT TRUE
                    ) ENGINE=InnoDB;
                ");

                Execute(con, @"
                    CREATE TABLE IF NOT EXISTS Merchants (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        BusinessName VARCHAR(150) NOT NULL,
                        Email VARCHAR(255) NOT NULL,
                        Phone VARCHAR(30) NULL,
                        Balance DECIMAL(18,2) NOT NULL DEFAULT 0.00,
                        Status INT NOT NULL,
                        CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                    ) ENGINE=InnoDB;
                ");

                Execute(con, @"
                    CREATE TABLE IF NOT EXISTS MerchantOwners (
                        MerchantId INT NOT NULL,
                        UserId INT NOT NULL,
                        PRIMARY KEY (MerchantId, UserId),
                        CONSTRAINT FK_MerchantOwners_Merchants
                            FOREIGN KEY (MerchantId) REFERENCES Merchants(Id)
                            ON DELETE CASCADE
                            ON UPDATE CASCADE,
                        CONSTRAINT FK_MerchantOwners_Users
                            FOREIGN KEY (UserId) REFERENCES Users(Id)
                            ON DELETE CASCADE
                            ON UPDATE CASCADE
                    ) ENGINE=InnoDB;
                ");

                Execute(con, @"
                    CREATE TABLE IF NOT EXISTS Cards (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        UserId INT NOT NULL,
                        CardNumber VARCHAR(32) NOT NULL,
                        Balance DECIMAL(18,2),
                        CVV VARCHAR(10) NOT NULL,
                        Iban VARCHAR(34) NOT NULL,
                        CreatedDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        ExpirationDate DATETIME NOT NULL,
                        CONSTRAINT FK_Cards_Users
                            FOREIGN KEY (UserId) REFERENCES Users(Id)
                            ON DELETE CASCADE
                            ON UPDATE CASCADE
                    ) ENGINE=InnoDB;
                ");

                Execute(con, @"
                    CREATE TABLE IF NOT EXISTS Transactions (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        Amount DECIMAL(18,2) NOT NULL,
                        Timestamp DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        Status INT NOT NULL,
                        SenderId INT NOT NULL,
                        MerchantId INT NOT NULL,
                        CONSTRAINT FK_Transactions_Sender
                            FOREIGN KEY (SenderId) REFERENCES Users(Id)
                            ON DELETE CASCADE
                            ON UPDATE CASCADE,
                        CONSTRAINT FK_Transactions_Merchant
                            FOREIGN KEY (MerchantId) REFERENCES Merchants(Id)
                            ON DELETE CASCADE
                            ON UPDATE CASCADE
                    ) ENGINE=InnoDB;
                ");
                
                Execute(con, @"
                    CREATE TABLE IF NOT EXISTS Logs (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        EventId VARCHAR(100) NOT NULL,
                        Message TEXT NOT NULL,
                        Timestamp DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        LoggerType VARCHAR(50) NOT NULL
                    ) ENGINE=InnoDB;
                ");
            }

            private static void Execute(MySqlConnection con, string sql)
            {
                using var cmd = new MySqlCommand(sql, con);
                cmd.ExecuteNonQuery();
            }
        }
    }