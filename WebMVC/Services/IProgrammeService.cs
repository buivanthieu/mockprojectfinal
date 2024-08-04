using WebMVC.Models;

namespace WebMVC.Services;

public interface IProgrammeService
{
    Task<IEnumerable<Programme>> GetAllProgrammesAsync();
    Task<Programme> GetProgrammeByIdAsync(int id);
    Task<Programme> CreateProgrammeAsync(Programme programme);
    Task<Programme> UpdateProgrammeAsync(int id, Programme programme);
    Task DeleteProgrammeAsync(int id);
    Task<IEnumerable<Contact>> GetAllContactsAsync();
}