
using Business.Handlers.Baskets.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Baskets.Queries.GetBasketQuery;
using Entities.Concrete;
using static Business.Handlers.Baskets.Queries.GetBasketsQuery;
using static Business.Handlers.Baskets.Commands.CreateBasketCommand;
using Business.Handlers.Baskets.Commands;
using Business.Constants;
using static Business.Handlers.Baskets.Commands.UpdateBasketCommand;
using static Business.Handlers.Baskets.Commands.DeleteBasketCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class BasketHandlerTests
    {
        Mock<IBasketRepository> _basketRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _basketRepository = new Mock<IBasketRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Basket_GetQuery_Success()
        {
            //Arrange
            var query = new GetBasketQuery();

            _basketRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Basket, bool>>>())).ReturnsAsync(new Basket()
//propertyler buraya yazılacak
//{																		
//BasketId = 1,
//BasketName = "Test"
//}
);

            var handler = new GetBasketQueryHandler(_basketRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.BasketId.Should().Be(1);

        }

        [Test]
        public async Task Basket_GetQueries_Success()
        {
            //Arrange
            var query = new GetBasketsQuery();

            _basketRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Basket, bool>>>()))
                        .ReturnsAsync(new List<Basket> { new Basket() { /*TODO:propertyler buraya yazılacak BasketId = 1, BasketName = "test"*/ } });

            var handler = new GetBasketsQueryHandler(_basketRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Basket>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Basket_CreateCommand_Success()
        {
            Basket rt = null;
            //Arrange
            var command = new CreateBasketCommand();
            //propertyler buraya yazılacak
            //command.BasketName = "deneme";

            _basketRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Basket, bool>>>()))
                        .ReturnsAsync(rt);

            _basketRepository.Setup(x => x.Add(It.IsAny<Basket>())).Returns(new Basket());

            var handler = new CreateBasketCommandHandler(_basketRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _basketRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Basket_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateBasketCommand();
            //propertyler buraya yazılacak 
            //command.BasketName = "test";

            _basketRepository.Setup(x => x.Query())
                                           .Returns(new List<Basket> { new Basket() { /*TODO:propertyler buraya yazılacak BasketId = 1, BasketName = "test"*/ } }.AsQueryable());

            _basketRepository.Setup(x => x.Add(It.IsAny<Basket>())).Returns(new Basket());

            var handler = new CreateBasketCommandHandler(_basketRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Basket_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateBasketCommand();
            //command.BasketName = "test";

            _basketRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Basket, bool>>>()))
                        .ReturnsAsync(new Basket() { /*TODO:propertyler buraya yazılacak BasketId = 1, BasketName = "deneme"*/ });

            _basketRepository.Setup(x => x.Update(It.IsAny<Basket>())).Returns(new Basket());

            var handler = new UpdateBasketCommandHandler(_basketRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _basketRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Basket_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteBasketCommand();

            _basketRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Basket, bool>>>()))
                        .ReturnsAsync(new Basket() { /*TODO:propertyler buraya yazılacak BasketId = 1, BasketName = "deneme"*/});

            _basketRepository.Setup(x => x.Delete(It.IsAny<Basket>()));

            var handler = new DeleteBasketCommandHandler(_basketRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _basketRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

