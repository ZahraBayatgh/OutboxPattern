namespace OutboxPattern.Infrastructure.Outbox
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public DateTime OccuredOnUtc { get; set; }
        public DateTime? ProcessedOnUtc { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
