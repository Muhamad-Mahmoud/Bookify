using Bookify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.DL.Repository.IRepository
{
    public interface IRoomImageRepository : IRepository<RoomImage>
    {
        void Update(RoomImage roomImage);
    }
}
