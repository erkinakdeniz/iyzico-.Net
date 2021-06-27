
using Business.BusinessAspects;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;

namespace Business.Handlers.Baskets.Queries
{

    public class GetBasketsQuery : IRequest<IDataResult<IEnumerable<Basket>>>
    {
        public class GetBasketsQueryHandler : IRequestHandler<GetBasketsQuery, IDataResult<IEnumerable<Basket>>>
        {
            private readonly IBasketRepository _basketRepository;
            private readonly IMediator _mediator;

            public GetBasketsQueryHandler(IBasketRepository basketRepository, IMediator mediator)
            {
                _basketRepository = basketRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Basket>>> Handle(GetBasketsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Basket>>(await _basketRepository.GetListAsync());
            }
        }
    }
}