namespace WebHooks.SharedKernel.Entities
{
    public record Client
    {
        public Guid Id { get; set; }

        public string ClientName { get; set; }

        public string HandlerUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
