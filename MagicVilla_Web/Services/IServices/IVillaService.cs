using MagicVilla_Web.Models.Dto;
using System.Linq.Expressions;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(VilaCreateDTO dto);
        Task<T> UpdateAsync<T>(VilaUpdateDTO dto);
        Task<T> DeleteAsync<T>(int id);
        
    }
}
