using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Interfaces
{
    public interface IDeletedInfo
    {
        //could be null
        public DateTime? DeletedAt { get; set; }

        bool IsDeleted { get; set; }
    }
}
