
using Business.Handlers.Orders.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Orders.Queries.GetOrderQuery;
using Entities.Concrete;
using static Business.Handlers.Orders.Queries.GetOrdersQuery;
using static Business.Handlers.Orders.Commands.CreateOrderCommand;
using Business.Handlers.Orders.Commands;
using Business.Constants;
using static Business.Handlers.Orders.Commands.UpdateOrderCommand;
using static Business.Handlers.Orders.Commands.DeleteOrderCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class OrderHandlerTests
    {
        Mock<IOrderRepository> _orderRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _orderRepository = new Mock<IOrderRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Order_GetQuery_Success()
        {
            //Arrange
            var query = new GetOrderQuery();

            _orderRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Order, bool>>>())).ReturnsAsync(new Order()
//propertyler buraya yazılacak
//{																		
//OrderId = 1,
//OrderName = "Test"
//}
);

            var handler = new GetOrderQueryHandler(_orderRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.OrderId.Should().Be(1);

        }

        [Test]
        public async Task Order_GetQueries_Success()
        {
            //Arrange
            var query = new GetOrdersQuery();

            _orderRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Order, bool>>>()))
                        .ReturnsAsync(new List<Order> { new Order() { /*TODO:propertyler buraya yazılacak OrderId = 1, OrderName = "test"*/ } });

            var handler = new GetOrdersQueryHandler(_orderRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Order>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Order_CreateCommand_Success()
        {
            Order rt = null;
            //Arrange
            var command = new CreateOrderCommand();
            //propertyler buraya yazılacak
            //command.OrderName = "deneme";

            _orderRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Order, bool>>>()))
                        .ReturnsAsync(rt);

            _orderRepository.Setup(x => x.Add(It.IsAny<Order>())).Returns(new Order());

            var handler = new CreateOrderCommandHandler(_orderRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _orderRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Order_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateOrderCommand();
            //propertyler buraya yazılacak 
            //command.OrderName = "test";

            _orderRepository.Setup(x => x.Query())
                                           .Returns(new List<Order> { new Order() { /*TODO:propertyler buraya yazılacak OrderId = 1, OrderName = "test"*/ } }.AsQueryable());

            _orderRepository.Setup(x => x.Add(It.IsAny<Order>())).Returns(new Order());

            var handler = new CreateOrderCommandHandler(_orderRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Order_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateOrderCommand();
            //command.OrderName = "test";

            _orderRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Order, bool>>>()))
                        .ReturnsAsync(new Order() { /*TODO:propertyler buraya yazılacak OrderId = 1, OrderName = "deneme"*/ });

            _orderRepository.Setup(x => x.Update(It.IsAny<Order>())).Returns(new Order());

            var handler = new UpdateOrderCommandHandler(_orderRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _orderRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Order_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteOrderCommand();

            _orderRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Order, bool>>>()))
                        .ReturnsAsync(new Order() { /*TODO:propertyler buraya yazılacak OrderId = 1, OrderName = "deneme"*/});

            _orderRepository.Setup(x => x.Delete(It.IsAny<Order>()));

            var handler = new DeleteOrderCommandHandler(_orderRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _orderRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

