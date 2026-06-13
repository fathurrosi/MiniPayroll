using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Interfaces.Services
{
    public interface IHasherService
    {
        string Hash(string password);
        bool Verify(string password, string passwordHash);

    }
}
