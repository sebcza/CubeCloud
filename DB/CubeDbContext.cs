using System;
using DB.Models;
using Microsoft.EntityFrameworkCore;

namespace DB
{
    public class CubeDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public CubeDbContext() : base()
        {
        }
        public DbSet<Cube> Cubes { get; set; }

        public DbSet<OutputCube> OutputCubes { get; set; }
    }
}
