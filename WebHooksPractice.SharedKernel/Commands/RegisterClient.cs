using AutoMapper;
using Dapper.BaseRepository.Config;
using MediatR;
using WebHooks.SharedKernel.Base;
using WebHooks.SharedKernel.Entities;
using WebHooks.SharedKernel.Repositories.Interfaces;

namespace WebHooks.SharedKernel.Commands
{
    public static class RegisterClient
    {
        public class Command : IRequest<Response>
        {
            public string Name { get; set; }

            public string HandlerUrl { get; set; }
        }

        public class Response : ApiResponse
        {
            public Guid ClientId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly IMapper mapper;
            private readonly IClientRepo client;

            public Handler(IMapper mapper, IClientRepo client)
            {
                this.mapper = mapper;
                this.client = client;
            }
            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var parameters = mapper.Map<Client>(request);
                    var clientId = Guid.NewGuid();
                    parameters.Id = clientId;
                    var resp = await client.CreateClient(parameters);

                    if (resp == CommandResp.Success) return new Response()
                    {
                        Message = "Client registration was successful",
                        ClientId = clientId
                    };

                    return new Response
                    {
                        Message = "Failed to register client",
                        ErrorMessage = "Some internal error occurred."
                    };
                }

                return new Response();
            }

            public class RegisterClientMapper : Profile
            {
                public RegisterClientMapper()
                {
                    CreateMap<Command, Client>().
                        ForMember(dest => dest.ClientName, src => src.MapFrom(src => src.Name));
                }
            }
        }
    }
}
