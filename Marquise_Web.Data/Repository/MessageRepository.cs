using Marquise_Web.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marquise_Web.Data.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly Marquise_WebEntities context;

        public MessageRepository(Marquise_WebEntities context)
        {
            this.context = context;
        }

        public void Add(Message message)
        {
                context.Messages.Add(message);
                context.SaveChanges();
        }
    }
}
