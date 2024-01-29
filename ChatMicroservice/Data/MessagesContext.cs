using ChatMicroservice.Model;

namespace ChatMicroservice.Data
{
    public class MessagesContext
    {
        private List<SingleMessage> Messages { get; set;} = new List<SingleMessage>();


        public void InsertMessage(SingleMessage message)
        {
            //Console.WriteLine("SE INSEREAZA ACUMA");
            //Console.WriteLine(message.From);
            //Console.WriteLine(message.To);
            //Console.WriteLine(message.Message);
            Messages.Add(message);
        }


        public List<SingleMessage> GetMessages(int clientId)
        {
			return Messages
                .Where(message => message.To == clientId || message.From == clientId)
                .ToList();
		}

		public List<SingleMessage> GetMessages(int adminId, int clientId)
		{
            return Messages
                .Where(message => (message.To == clientId && message.From == adminId) || (message.To == adminId && message.From == clientId))
                .ToList();
		}


        public void DeleteMessages(int userId)
        {
            Messages.RemoveAll(m => m.To == userId || m.From == userId);
        }
	}
}
