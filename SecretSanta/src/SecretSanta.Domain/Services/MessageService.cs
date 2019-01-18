using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
    public class MessageService
    {
        private ApplicationDbContext DbContext { get; }

        public MessageService(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public Message AddOrUpdateMessage(Message message)
        {
            if (message.Id == default(int))
                DbContext.Messages.Add(message);
            else
                DbContext.Messages.Update(message);
            DbContext.SaveChanges();

            return message;
        }

        public void RemoveMessage(Message message)
        {
            DbContext.Messages.Remove(message);
            DbContext.SaveChanges();
        }

        public ICollection<Message> AddMessages(ICollection<Message> messages)
        {
            DbContext.AddRange(messages);
            DbContext.SaveChanges();
            return messages;
        }

        public Message Find(int id)
        {
            return DbContext.Messages.Include(message => message.Pairing).ThenInclude(pairing => pairing.Santa)
                    .Include(message => message.Pairing).ThenInclude(pairing => pairing.Recipient)
                .SingleOrDefault(message => message.Id == id);
        }

        public List<Message> FetchAll()
        {
            var messageTask = DbContext.Messages
                .Include(message => message.Pairing)
                    .ThenInclude(pairing => pairing.Recipient)
                .Include(message => message.Pairing)
                    .ThenInclude(pairing => pairing.Santa)
                .ToListAsync();
            messageTask.Wait();
            return messageTask.Result;
        }
    }
}