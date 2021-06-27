
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


namespace Business.Handlers.Products.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteProductCommand : IRequest<IResult>
    {
        public int ProductID { get; set; }

        public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, IResult>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMediator _mediator;

            public DeleteProductCommandHandler(IProductRepository productRepository, IMediator mediator)
            {
                _productRepository = productRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            {
                var productToDelete = _productRepository.Get(p => p.ProductID == request.ProductID);

                _productRepository.Delete(productToDelete);
                await _productRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

