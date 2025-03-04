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
        private readonly SiteDataEntities context;

        public MessageRepository(SiteDataEntities context)
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
