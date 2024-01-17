using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

using var connection =
    new SqlConnection("Server=localhost,1433;Database=StoreDb;Trusted_Connection=True;TrustServerCertificate=True;");

connection.Open();

// {
//     connection.Execute(@"CREATE TABLE [Test] (
//                             [Id] INT IDENTITY,
//                     )");
//     connection.Execute("DROP TABLE [Test]");
// }

// INSERT
{
    // ORM
    // Object Relational Mapping
    // connection.Execute("INSERT INTO [Items] VALUES(@Name, @Quantity)", new
    // {
    //     Name = "Cherry",
    //     Quantity = 1
    // });
}

// Calling procedure with output param
{
    var @params = new DynamicParameters();
    
    @params.Add("Result", dbType: DbType.Single, direction: ParameterDirection.Output);
    connection.Query<float>("sp_GetAverageQuantity", @params, commandType: CommandType.StoredProcedure);
    Console.WriteLine(@params.Get<float>("Result"));
}

{
    var result = connection.Query<Item>("SELECT * FROM [Items]");

    foreach (var item in result)
    {
        Console.WriteLine(item.Id);
        Console.WriteLine(item.Name);
        Console.WriteLine(item.Quantity);
        Console.WriteLine(new string('-', 15));
    }
}


// Foo(new
// {
//     Data = 2
// });
//
// void Foo(object? obj)
// {
//     Console.WriteLine(obj.Data);
// }


class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
}