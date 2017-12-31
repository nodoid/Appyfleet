using System;
using System.Threading.Tasks;
using mvvmframework.Models;

namespace mvvmframework.Interfaces
{
    public interface ISockets
    {
        bool SendMessage(string message, byte[] sReceivedData, byte[] sSuccess, int count);
    }
}
