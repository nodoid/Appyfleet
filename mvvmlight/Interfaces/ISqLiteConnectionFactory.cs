using System;
using SQLite.Net;
namespace mvvmframework
{
    public interface ISqLiteConnectionFactory
    {
        SQLiteConnection GetConnection();
    }
}
