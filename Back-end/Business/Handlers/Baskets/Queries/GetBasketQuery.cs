
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Baskets.Queries
{
    public class GetBasketQuery : IRequest<IDataResult<Basket>>
    {
        public int BasketId { get; set; }

        public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, IDataResult<Basket>>
        {
            private readonly IBasketRepository _basketRepository;
            private readonly IMediator _mediator;

            public GetBasketQueryHandler(IBasketRepository basketRepository, IMediator mediator)
            {
                _basketRepository = basketRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Basket>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
            {
                var basket = await _basketRepository.GetAsync(p => p.BasketId == request.BasketId);
                return new SuccessDataResult<Basket>(basket);
            }
        }
    }
}
