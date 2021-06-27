
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

namespace Business.Handlers.Products.Queries
{

    public class GetProductsQuery : IRequest<IDataResult<IEnumerable<Product>>>
    {
        public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IDataResult<IEnumerable<Product>>>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMediator _mediator;

            public GetProductsQueryHandler(IProductRepository productRepository, IMediator mediator)
            {
                _productRepository = productRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Product>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Product>>(await _productRepository.GetListAsync());
            }
        }
    }
}