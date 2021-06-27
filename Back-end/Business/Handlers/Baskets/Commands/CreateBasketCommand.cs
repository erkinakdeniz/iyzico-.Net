
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.Baskets.ValidationRules;

namespace Business.Handlers.Baskets.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateBasketCommand : IRequest<IResult>
    {

        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int? OrderId { get; set; }


        public class CreateBasketCommandHandler : IRequestHandler<CreateBasketCommand, IResult>
        {
            private readonly IBasketRepository _basketRepository;
            private readonly IMediator _mediator;
            public CreateBasketCommandHandler(IBasketRepository basketRepository, IMediator mediator)
            {
                _basketRepository = basketRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateBasketValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
            {
                var isThereBasketRecord = _basketRepository.Query().Any(u => u.ProductID == request.ProductID);

                if (isThereBasketRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedBasket = new Basket
                {
                    ProductID = request.ProductID,
                    Quantity = request.Quantity,
                    Price = request.Price,
                    OrderId = request.OrderId,

                };

                _basketRepository.Add(addedBasket);
                await _basketRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}