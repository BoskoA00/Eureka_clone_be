namespace epp_be.DTOs
{
    public class ChatMessagesResponseDTO
    {
        public int Id { get; set; }
        public string SenderEmail { get; set; }
        public string ReceiverEmail { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int status { get; set; }
    }
}
