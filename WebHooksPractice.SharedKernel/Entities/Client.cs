using Dapper.BaseRepository.Attributes;

namespace WebHooks.SharedKernel.Entities
{
    public record Client : GetClientOutputParam
    {

        public string ClientName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public record GetClientParam : GetClientOutputParam
    {
        public Guid clientId { get; set; }
    }

    public record GetClientOutputParam
    {
        [SpOutputGuid]
        public Guid Id { get; set; }

        [SpOutputAnsiString(int.MaxValue)]
        public string HandlerUrl { get; set; }
    }
}
