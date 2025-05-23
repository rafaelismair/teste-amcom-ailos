﻿namespace Questao5.Infrastructure.Sqlite
{
    public interface IDatabaseConfig
    {
        string Name { get; }
    }


    public class DatabaseConfig : IDatabaseConfig
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Name { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
