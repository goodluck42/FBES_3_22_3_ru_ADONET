using Microsoft.Data.SqlClient;

// If u have Developer edition
var connection = new SqlConnection("Server=localhost,1433;Database=master;Trusted_Connection=True;TrustServerCertificate=True;");
// If u have SQLEXPRESS
//var connection = new SqlConnection(@"Server=.\SQLEXPRESS;Database=master;Trusted_Connection=True; User Id=admin; Password=admin");


connection.Open();

var dbName = "StoreDb";

var existsDbCommand = new SqlCommand()
{
    Connection = connection,
    CommandText = @$"
DECLARE @DbName NVARCHAR(256)

SET @DbName = N'{dbName}'

IF (EXISTS(SELECT D.name FROM sys.databases AS D WHERE D.name = @DbName))
BEGIN
   SELECT 1
END
ELSE
BEGIN
   SELECT 0
END"
};

if ((int)existsDbCommand.ExecuteScalar() == 1)
{
    Console.WriteLine("Database exists!");
    
}
else
{
    var createDbcommand = new SqlCommand
    {
        Connection = connection,
        CommandText = @"CREATE DATABASE [StoreDb]"
    };

    try
    {
        var count = createDbcommand.ExecuteNonQuery();

        Console.WriteLine(count);
    }
    catch (SqlException e)
    {
        Console.WriteLine(e.Message);
    }
}

var useCommand = new SqlCommand()
{
    Connection = connection,
    CommandText = @$"USE [{dbName}];"
};

useCommand.ExecuteNonQuery();

try
{
    var createTableCommand = new SqlCommand()
    {
        Connection = connection,
        CommandText = @"
CREATE TABLE [Items]
(
    [Id] INT PRIMARY KEY IDENTITY,
    [Name] NVARCHAR(450) UNIQUE,
    [Quantity] INT CHECK([Quantity] >= 0)
)"
    };
    
    createTableCommand.ExecuteNonQuery();

    Console.WriteLine("Table created!");
}
catch (SqlException e)
{
    Console.WriteLine(e.Message);
}


try
{
    var insertDataCommand = new SqlCommand()
    {
        Connection = connection,
        CommandText = @"INSERT INTO [Items]
VALUES (N'Cucumber', 42),
(N'Potato', 13),
(N'Tomato', 12);"
    };

    var result = insertDataCommand.ExecuteNonQuery();

    Console.WriteLine($"Added rows: {result}");
}
catch (SqlException e)
{
    Console.WriteLine(e.Message);
}

try
{
    var selectCommand = new SqlCommand
    {
        Connection = connection,
        CommandText = "SELECT * FROM [Items]"
    };

    using var reader = selectCommand.ExecuteReader();

    while (reader.Read())
    {
        //var id = reader.GetInt32(0);
        var id = (int)reader["Id"];
        var name = reader.GetString(1);
        var quantity = (int)reader.GetValue(2);

        Console.WriteLine($"Id: {id}; Name: {name}; Quantity: {quantity}");
    }
}
catch (SqlException e)
{
    Console.WriteLine(e.Message);
}

connection.Close();