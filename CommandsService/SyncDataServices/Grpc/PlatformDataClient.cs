using AutoMapper;
using Grpc.Net.Client;
using PlatformService;
using CommandsService.Models;

namespace CommandsService.SyncDataServices.Grpc
{
    public class PlatformDataClient(IConfiguration configuration, IMapper mapper) : IPlatformDataClient
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IMapper _mapper = mapper;

        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            Console.WriteLine($"--> Calling gRPC Service {_configuration["GrpcPlatform"]}");

            var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"]);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var req = new GetAllRequest();

            try
            {
                var reply = client.GetAllPlatforms(req);
                return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Could not call gRPC Server: {e.Message} ");
                throw;
            }

            throw new NotImplementedException();
        }
    }
}