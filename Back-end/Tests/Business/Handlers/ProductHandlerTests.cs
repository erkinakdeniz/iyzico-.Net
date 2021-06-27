
using Business.Handlers.Products.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Products.Queries.GetProductQuery;
using Entities.Concrete;
using static Business.Handlers.Products.Queries.GetProductsQuery;
using static Business.Handlers.Products.Commands.CreateProductCommand;
using Business.Handlers.Products.Commands;
using Business.Constants;
using static Business.Handlers.Products.Commands.UpdateProductCommand;
using static Business.Handlers.Products.Commands.DeleteProductCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class ProductHandlerTests
    {
        Mock<IProductRepository> _productRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _productRepository = new Mock<IProductRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Product_GetQuery_Success()
        {
            //Arrange
            var query = new GetProductQuery();

            _productRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(new Product()
//propertyler buraya yazılacak
//{																		
//ProductId = 1,
//ProductName = "Test"
//}
);

            var handler = new GetProductQueryHandler(_productRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.ProductId.Should().Be(1);

        }

        [Test]
        public async Task Product_GetQueries_Success()
        {
            //Arrange
            var query = new GetProductsQuery();

            _productRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                        .ReturnsAsync(new List<Product> { new Product() { /*TODO:propertyler buraya yazılacak ProductId = 1, ProductName = "test"*/ } });

            var handler = new GetProductsQueryHandler(_productRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Product>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Product_CreateCommand_Success()
        {
            Product rt = null;
            //Arrange
            var command = new CreateProductCommand();
            //propertyler buraya yazılacak
            //command.ProductName = "deneme";

            _productRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                        .ReturnsAsync(rt);

            _productRepository.Setup(x => x.Add(It.IsAny<Product>())).Returns(new Product());

            var handler = new CreateProductCommandHandler(_productRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _productRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Product_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateProductCommand();
            //propertyler buraya yazılacak 
            //command.ProductName = "test";

            _productRepository.Setup(x => x.Query())
                                           .Returns(new List<Product> { new Product() { /*TODO:propertyler buraya yazılacak ProductId = 1, ProductName = "test"*/ } }.AsQueryable());

            _productRepository.Setup(x => x.Add(It.IsAny<Product>())).Returns(new Product());

            var handler = new CreateProductCommandHandler(_productRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Product_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateProductCommand();
            //command.ProductName = "test";

            _productRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                        .ReturnsAsync(new Product() { /*TODO:propertyler buraya yazılacak ProductId = 1, ProductName = "deneme"*/ });

            _productRepository.Setup(x => x.Update(It.IsAny<Product>())).Returns(new Product());

            var handler = new UpdateProductCommandHandler(_productRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _productRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Product_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteProductCommand();

            _productRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                        .ReturnsAsync(new Product() { /*TODO:propertyler buraya yazılacak ProductId = 1, ProductName = "deneme"*/});

            _productRepository.Setup(x => x.Delete(It.IsAny<Product>()));

            var handler = new DeleteProductCommandHandler(_productRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _productRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

