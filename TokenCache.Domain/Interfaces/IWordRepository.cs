using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenCache.Domain.Entities;

namespace TokenCache.Domain.Interfaces
{
    public interface IWordRepository
    {
       
        Task CreateAsync(Word word);
        Task<Word> GetByIdAsync(int Id);        

    }
}
