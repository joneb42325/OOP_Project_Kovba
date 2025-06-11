using System.Data;

namespace OOP_Project_Kovba.Interfaces
{
    public interface IDatabaseConnection
    {
        IDbConnection CreateConnection();
    }
}
