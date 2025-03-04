using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marquise_Web.Data.IRepository
{
    public interface IMessageRepository
    {
        void Add(Message message);
    }
}
