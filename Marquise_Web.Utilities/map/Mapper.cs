
using AutoMapper;
using Marquise_Web.Data;
using Marquise_Web.Model.SiteModel;
using MArquise_Web.Model.CRM;


namespace Utilities.Map
{
    public class Mapper 
    {
        private readonly IMapper mapper;

        public Mapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public MessageModel ModelToMessage(Message message)
        {
            return mapper.Map<MessageModel>(message);
        }
        
        public Message MessageToModel(MessageModel messageModel)
        {
            return mapper.Map<Message>(messageModel);
        }

        public UserModel ModelToUser(User user)
        {
            return mapper.Map<UserModel>(user);
        }

        public Message UserToModel(UserModel userModel)
        {
            return mapper.Map<Message>(userModel);
        }
    }
}
