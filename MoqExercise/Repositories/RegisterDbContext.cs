using Microsoft.EntityFrameworkCore;
using MoqExercise.Models;

namespace MoqExercise.Repositories;

public class RegisterDbContext(DbContextOptions<RegisterDbContext> options) : DbContext(options)
{
    public DbSet<Register> Register { get; set; }
}
