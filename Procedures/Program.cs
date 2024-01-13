using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;


var configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .Build();

using var connection = new SqlConnection(configuration["ConnectionString"]);

connection.Open();

//// Insert using parameters
// var itemName = Console.ReadLine()!;
// var quantity = Console.ReadLine()!;
//
// using var insertCommand = new SqlCommand
// {
//     Connection = connection,
//     CommandText = "USE [StoreDb]; INSERT INTO [Items] VALUES (@itemName, @quantity)"
// };
//
// insertCommand.Parameters.Add(new SqlParameter()
// {
//     SqlDbType = SqlDbType.NVarChar,
//     Direction = ParameterDirection.Input,
//     Size = 450,
//     Value = itemName,
//     ParameterName = "@itemName"
// });
//
// insertCommand.Parameters.AddWithValue("@quantity", quantity);
//
// insertCommand.ExecuteNonQuery();
    


//// Executing procedures

{
    var executeCommand = new SqlCommand
    {
        Connection = connection,
        CommandType = CommandType.StoredProcedure,
        CommandText = "sp_GetAllItems"
    };

    using var reader = executeCommand.ExecuteReader();

    if (reader.HasRows)
    {
        var items = new List<Item>();
        while (reader.Read())
        {
            items.Add(new Item
            {
                Id = reader.GetInt32("Id"),
                Name = reader.GetString("Name"),
                Quantity = reader.GetInt32("Quantity")
            });
        }

        foreach (var item in items)
        {
            Console.WriteLine(item);
        }
    }
}

{
    var executeCommand = new SqlCommand
    {
        Connection = connection,
        CommandType = CommandType.StoredProcedure,
        CommandText = "sp_GetAverageQuantity"
    };

    var outputParameter = new SqlParameter
    {
        SqlDbType = SqlDbType.Float,
        Direction = ParameterDirection.Output,
        ParameterName = "@Result"
    };
    
    executeCommand.Parameters.Add(outputParameter);

    var affected = executeCommand.ExecuteNonQuery();

    Console.WriteLine($"Average quantity: {outputParameter.Value}");
    Console.WriteLine($"Affected rows: {affected}");
}

{
    Console.WriteLine("Enter id: ");
    int id = int.Parse(Console.ReadLine()!);
    
    var executeCommand = new SqlCommand
    {
        Connection = connection,
        CommandText = "sp_GetItemById",
        CommandType = CommandType.StoredProcedure
    };
    
    executeCommand.Parameters.Add(new SqlParameter
    {
        Direction = ParameterDirection.Input,
        SqlDbType = SqlDbType.Int,
        ParameterName = "@Id",
        Value = id
    });

    using var reader = executeCommand.ExecuteReader();

    if (reader.HasRows)
    {
        _ = reader.Read();

        var item = new Item()
        {
            Id = reader.GetInt32("Id"),
            Name = reader.GetString("Name"),
            Quantity = reader.GetInt32("Quantity")
        };

        Console.WriteLine(item);
    }
    else
    {
        Console.WriteLine("Item with such id not found!");
    }
}


record Item
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
}