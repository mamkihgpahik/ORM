using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using ORM.Context;
using ORM.Entities;
using ORM.Repository;

Direction Direction = new Direction(3, "br");
string connectionString = "Server=ARSHAVA-A;Database=SeaBattle;Trusted_Connection=True;";
using (SqlConnection sqlConnection = new SqlConnection(connectionString))
{
    sqlConnection.Open();
    DbContext dbContext = new DbContext(sqlConnection);
    UnitOfWork uow = new UnitOfWork(dbContext);
    //uow.ShipRepository.Add(ship); 
    uow.DirectionRepository.Add(Direction);
    uow.Save();
}