using Microsoft.EntityFrameworkCore;
using MoqExercise.Models;

namespace MoqExercise.Repositories;

public class RegisterRepository(RegisterDbContext context) : IRegisterRepository
{
    private readonly RegisterDbContext _context = context;

    public Task CreateAsync(Register register)
    {
        _context.Register.Add(register);
        return _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var register = await GetByIdAsync(id);
        if (register is not null)
        {
            _context.Remove(register);
            await _context.SaveChangesAsync();
        }
    }

    public Task<Register?> GetByIdAsync(int id) =>
        _context.Register.FirstOrDefaultAsync(r => r.Id == id);

    public Task<List<Register>> ListAsync() =>
        _context.Register.ToListAsync();

    public Task UpdateAsync(Register register)
    {
        _context.Entry(register).State = EntityState.Modified;
        return _context.SaveChangesAsync();
    }
}
