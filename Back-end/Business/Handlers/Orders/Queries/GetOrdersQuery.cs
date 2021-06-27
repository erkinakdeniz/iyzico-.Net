
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

namespace Business.Handlers.Orders.Queries
{

    public class GetOrdersQuery : IRequest<IDataResult<IEnumerable<Order>>>
    {
        public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IDataResult<IEnumerable<Order>>>
        {
            private readonly IOrderRepository _orderRepository;
            private readonly IMediator _mediator;

            public GetOrdersQueryHandler(IOrderRepository orderRepository, IMediator mediator)
            {
                _orderRepository = orderRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Order>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Order>>(await _orderRepository.GetListAsync());
            }
        }
    }
}