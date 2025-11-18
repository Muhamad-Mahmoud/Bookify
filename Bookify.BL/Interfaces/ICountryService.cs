using Bookify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BL.Interfaces
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task<Country?> GetCountryByIdAsync(int id);
        Task<Country> GetCountryAsync(Expression<Func<Country, bool>> filter);
        Task<bool> AddCountryAsync(Country country);
        Task<bool> UpdateCountryAsync(Country country);
        Task<bool> DeleteCountryAsync(int id);
    }
}
