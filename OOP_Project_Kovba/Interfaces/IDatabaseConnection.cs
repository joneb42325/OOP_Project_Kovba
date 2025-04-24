using System.Data;

namespace MyMVC.Interfaces
{
    public interface IDatabaseConnection
    {
        IDbConnection CreateConnection();
    }
}
