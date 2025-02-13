using Grpc.Core;
using AutoMapper;
using PlatformService.Repositories;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService(IPlatformRepository repository, IMapper mapper) : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public override async Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var response = new PlatformResponse();
            var platforms = await _repository.GetAllPlatforms();

            foreach ( var platform in platforms)
            {
                response.Platform.Add(_mapper.Map<GrpcPlatformModel>(platform));
            }

            return response;
        }
    }
}