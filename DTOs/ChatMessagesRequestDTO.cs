namespace epp_be.DTOs
{
    public class ChatMessagesRequestDTO
    {
        public int SenderId { get; set; }
        public string SenderEmail { get; set; }
        public int ReceiverId { get; set; }
        public string ReceiverEmail { get; set; }
        public string Content { get; set; }
    }
}
