﻿namespace prueba_backend.Models.Services
{
    public interface IUsuarioService
    {
        object ValidateUser(string username, string password);
        object CreateUser(string username, string password);
    }
}
