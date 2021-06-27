
using Business.Constants;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Aspects.Autofac.Validation;
using Business.Handlers.Baskets.ValidationRules;


namespace Business.Handlers.Baskets.Commands
{


    public class UpdateBasketCommand : IRequest<IResult>
    {
        public int BasketId { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int? OrderId { get; set; }

        public class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand, IResult>
        {
            private readonly IBasketRepository _basketRepository;
            private readonly IMediator _mediator;

            public UpdateBasketCommandHandler(IBasketRepository basketRepository, IMediator mediator)
            {
                _basketRepository = basketRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateBasketValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
            {
                var isThereBasketRecord = await _basketRepository.GetAsync(u => u.BasketId == request.BasketId);


                isThereBasketRecord.ProductID = request.ProductID;
                isThereBasketRecord.Quantity = request.Quantity;
                isThereBasketRecord.Price = request.Price;
                isThereBasketRecord.OrderId = request.OrderId;


                _basketRepository.Update(isThereBasketRecord);
                await _basketRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

