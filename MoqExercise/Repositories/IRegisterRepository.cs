using MoqExercise.Models;

namespace MoqExercise.Repositories;

public interface IRegisterRepository
{
    Task<Register?> GetByIdAsync(int id);
    Task<List<Register>> ListAsync();
    Task CreateAsync(Register register);
    Task UpdateAsync(Register register);
    Task DeleteAsync(int id);
}
