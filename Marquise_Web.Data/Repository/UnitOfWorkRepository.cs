using Marquise_Web.Data.IRepository;
using System.Threading.Tasks;

namespace Marquise_Web.Data.Repository
{
    public class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        private readonly Marquise_WebEntities _context;

        public IMessageRepository MessageRepository { get; private set; }

        public IUserRepository UserRepository { get; private set; }

        // Constructor
        public UnitOfWorkRepository(Marquise_WebEntities context,
            IMessageRepository messageRepository,
            IUserRepository userRepository)
        {
            _context = context;
            MessageRepository = messageRepository;
            UserRepository = userRepository;
        }

        // ذخیره‌سازی تغییرات در پایگاه داده
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Dispose برای مدیریت منابع
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
