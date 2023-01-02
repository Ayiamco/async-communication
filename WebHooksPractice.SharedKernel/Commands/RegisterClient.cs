using AutoMapper;
using DapperHelper;
using MediatR;
using System.Security.Cryptography.X509Certificates;
using WebHooks.SharedKernel.Base;
using WebHooks.SharedKernel.Entities;
using WebHooks.SharedKernel.Repositories.Interfaces;

namespace WebHooks.SharedKernel.Commands
{
    public static class RegisterClient
    {
        public class Request : IRequest<ApiResponse<Response>>
        {
            public string Name { get; set; }

            public string HandlerUrl { get; set; }
        }

        public class Response 
        {

        }

        public class Handler : IRequestHandler<Request, ApiResponse<Response>>
        {
            private readonly IMapper mapper;
            private readonly IClientRepo client;

            public Handler(IMapper mapper,IClientRepo client)
            {
                this.mapper = mapper;
                this.client = client;
            }
            public async Task<ApiResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var parameters = mapper.Map<Client>(request);
                    var resp=await client.CreateClient(parameters);
                    
                    if(resp == CommandResp.Success) return new ApiResponse<Response>()
                    {
                        Message="Client registration was successful"
                    };

                    return new ApiResponse<Response>
                    {
                        Message = "Failed to register client",
                        ErrorMessage = "Some internal error occurred."
                    };
                }

                return new ApiResponse<Response>();
            }

            public class RegisterClientMapper: Profile
            {
                public RegisterClientMapper()
                {
                    CreateMap<Request, Client>().
                        ForMember(dest => dest.ClientName, src=> src.MapFrom(src => src.Name));
                }
            }
        }
    }
}
