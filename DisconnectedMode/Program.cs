using System.Data;
using Microsoft.Data.SqlClient;

//// Transactions
// var sqlConnection = new SqlConnection("Server=localhost,1433;Database=StoreDb;Trusted_Connection=True;TrustServerCertificate=True;User ID=admin;Password=123");
// var sqlTransaction = sqlConnection.BeginTransaction();
//
// var sqlCommand = sqlConnection.CreateCommand();
//
// sqlCommand.CommandText = "INSERT INTO [Items] VALUES(N'Peach', -1);";
//
// try
// {
//     sqlCommand.ExecuteNonQuery();
// }
// catch (SqlException e)
// {
//     sqlTransaction.Rollback();
// }
//
// sqlCommand.CommandText = "INSERT INTO [Items] VALUES(N'Apricot', -1);";
//
// try
// {
//     sqlCommand.ExecuteNonQuery();
// }
// catch (SqlException e)
// {
//     sqlTransaction.Rollback();
// }
//
// sqlTransaction.Commit();


//// Disconnected mode
var sqlConnection =
    new SqlConnection(
        "Server=localhost,1433;Database=StoreDb;Trusted_Connection=True;TrustServerCertificate=True;User ID=admin;Password=123");
var sqlCommand = sqlConnection.CreateCommand();

sqlCommand.CommandText = "SELECT * FROM [Items]";

var adapter = new SqlDataAdapter(sqlCommand);
var commandBuilder = new SqlCommandBuilder(adapter);

var dataSet = new DataSet();

adapter.Fill(dataSet);

var itemsTable = dataSet.Tables[0];

itemsTable.PrimaryKey = new[] { itemsTable.Columns[0] };

Console.WriteLine("PKeys: ");
foreach (DataColumn dataColumn in itemsTable.PrimaryKey)
{
    Console.WriteLine(dataColumn.ColumnName);
}

foreach (DataColumn column in itemsTable.Columns)
{
    Console.Write(column.ColumnName);
    Console.Write('\t');
}

Console.WriteLine();

foreach (DataRow row in itemsTable.Rows)
{
    Console.WriteLine($"Id: {row["Id"]}");
    Console.WriteLine($"Name: {row["Name"]}");
    Console.WriteLine($"Quantity: {row["Quantity"]}");
}

//// INSERT
// var newRow = itemsTable.NewRow();
//
// newRow.ItemArray = new object?[] { null, "Peach", 10 };
//
// itemsTable.Rows.Add(newRow);
//
// adapter.Update(dataSet);




//// UPDATE
// DataRow? updateRow = itemsTable.Rows.Find(12);
//
// if (updateRow != null)
// {
//     updateRow["Name"] = "Apricot";
//
//     adapter.Update(dataSet);
// }

/// DELETE

var deleteRow = itemsTable.Rows.Find(1);

if (deleteRow != null)
{
    Console.WriteLine("Found item!");
    deleteRow.Delete();

    adapter.Update(dataSet);
}





