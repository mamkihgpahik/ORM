using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using ORM.Context;
using ORM.Entities;
using ORM.Repository;

Ship ship = new Ship();
Direction Direction = new Direction(3, "br");
string connectionString = "Server=ARSHAVA-A;Database=SeaBattle;Trusted_Connection=True;";
string DataBase = "SeaBattle";
using (SqlConnection sqlConnection = new SqlConnection(connectionString))
{
    sqlConnection.Open();
    DbContext dbContext = new DbContext(sqlConnection, DataBase);
    UnitOfWork uow = new UnitOfWork(dbContext);
    //uow.ShipRepository.Add(ship); 
    uow.DirectionRepository.Add(Direction);
    uow.Save();
    
}