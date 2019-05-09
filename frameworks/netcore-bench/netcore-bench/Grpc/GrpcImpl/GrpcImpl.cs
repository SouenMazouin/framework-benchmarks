using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Netcorebench;
using NetCoreBench.Models;
using World = Netcorebench.World;

namespace NetCoreBench.Grpc.GrpcImpl
{
    public class GrpcImpl : NetCoreBenchService.NetCoreBenchServiceBase
    {
        private readonly IDb _iDb;

        public GrpcImpl(IDb iDb)
        {
            _iDb = iDb;
        }

        public override Task<StringValue> InMemory(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new StringValue {Value = "Hello World"});
        }

        public override Task<World> SingleQuery(Empty request, ServerCallContext context)
        {
            return _iDb.LoadSingleQueryRow()
                .ContinueWith(ToProto, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public override Task<WorldList> MultiQueries(Queries request, ServerCallContext context)
        {
            return _iDb.LoadMultipleQueriesRows(request.Number)
                .ContinueWith(ToProtoList, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public override async Task ServerStream(Queries request, IServerStreamWriter<World> responseStream,
            ServerCallContext context)
        {
            await _iDb.LoadMultipleQueriesRows(request.Number)
                .ContinueWith(ToProtoList, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public override async Task BidiStream(IAsyncStreamReader<Queries> requestStream,
            IServerStreamWriter<World> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext(CancellationToken.None))
                await _iDb.LoadMultipleQueriesRows(requestStream.Current.Number)
                    .ContinueWith(ToProtoList, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private static World ToProto(Task<Models.World> row)
        {
            return new World
            {
                Id = row.Result.Id,
                RandomNumber = row.Result.RandomNumber
            };
        }

        private static WorldList ToProtoList(Task<Models.World[]> row)
        {
            var worldList = new WorldList();
            foreach (var world in row.Result)
                worldList.World.Add(new World
                {
                    Id = world.Id,
                    RandomNumber = world.RandomNumber
                });
            return worldList;
        }
    }
}