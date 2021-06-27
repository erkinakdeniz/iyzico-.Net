
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.Baskets.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteBasketCommand : IRequest<IResult>
    {
        public int BasketId { get; set; }

        public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand, IResult>
        {
            private readonly IBasketRepository _basketRepository;
            private readonly IMediator _mediator;

            public DeleteBasketCommandHandler(IBasketRepository basketRepository, IMediator mediator)
            {
                _basketRepository = basketRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
            {
                var basketToDelete = _basketRepository.Get(p => p.BasketId == request.BasketId);

                _basketRepository.Delete(basketToDelete);
                await _basketRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

