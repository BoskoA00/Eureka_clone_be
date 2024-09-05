using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace epp_be.Data
{
    public class ChatMessage
    {
        public int Id { get; set; }

        public string SenderEmail { get; set; }

        public string ReceiverEmail { get; set; }

        public string Content { get; set; }

        public int Status { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int SenderId { get; set; }
        [ForeignKey(nameof(SenderId))]
        public User SenderUser { get; set; }

        public int ReceiverId { get; set; }
        [ForeignKey(nameof(ReceiverId))]
        public User ReceiverUser { get; set; }
    }
}
