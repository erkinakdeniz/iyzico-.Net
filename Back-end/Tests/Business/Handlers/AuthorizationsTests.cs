﻿using Business.Constants;
using Business.Handlers.Authorizations.Commands;
using Business.Handlers.Authorizations.Queries;
using Core.CrossCuttingConcerns.Caching;
using Core.Entities.Concrete;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tests.Helpers;
using static Business.Handlers.Authorizations.Commands.ForgotPasswordCommand;
using static Business.Handlers.Authorizations.Commands.RegisterUserCommand;
using static Business.Handlers.Authorizations.Queries.LoginUserQuery;

namespace Tests.Business.Handlers
{
    [TestFixture]
    public class AuthorizationsTests
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<ITokenHelper> _tokenHelper;
        private Mock<IMediator> _mediator;
        private Mock<ICacheManager> _cacheManager;

        private LoginUserQueryHandler _loginUserQueryHandler;
        private LoginUserQuery _loginUserQuery;
        private RegisterUserCommandHandler _registerUserCommandHandler;
        private RegisterUserCommand _command;
        private ForgotPasswordCommandHandler _forgotPasswordCommandHandler;
        private ForgotPasswordCommand _forgotPasswordCommand;

        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
            _tokenHelper = new Mock<ITokenHelper>();
            _mediator = new Mock<IMediator>();
            _cacheManager = new Mock<ICacheManager>();

            _loginUserQueryHandler = new LoginUserQueryHandler(_userRepository.Object, _tokenHelper.Object, _mediator.Object, _cacheManager.Object);
            _registerUserCommandHandler = new RegisterUserCommandHandler(_userRepository.Object);
            _forgotPasswordCommandHandler = new ForgotPasswordCommandHandler(_userRepository.Object);
        }

        [Test]
        public async Task Handler_Login()
        {
            var user = DataHelper.GetUser("test");
            HashingHelper.CreatePasswordHash("123456", out var passwordSalt, out var passwordHash);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;
            _userRepository.
                            Setup(x => x.GetAsync(It.IsAny<Expression<Func<User, bool>>>())).Returns(() => Task.FromResult(user));


            _userRepository.Setup(x => x.GetClaims(It.IsAny<int>()))
                            .Returns(new List<OperationClaim>() { new() { Id = 1, Name = "test" } });
            _loginUserQuery = new LoginUserQuery
            {
                Email = user.Email,
                Password = "123456"
            };

            var result = await _loginUserQueryHandler.Handle(_loginUserQuery, new System.Threading.CancellationToken());

            result.Success.Should().BeTrue();

        }

        [Test]
        public async Task Handler_Register()
        {
            var registerUser = new User { Email = "test@test.com", FullName = "test test" };
            _command = new RegisterUserCommand
            {
                Email = registerUser.Email,
                FullName = registerUser.FullName,
                Password = "123456"
            };
            var result = await _registerUserCommandHandler.Handle(_command, new System.Threading.CancellationToken());

            result.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Handler_ForgotPassword()
        {
            var user = DataHelper.GetUser("test");
            _userRepository.
                                    Setup(x => x.GetAsync(It.IsAny<Expression<Func<User, bool>>>())).Returns(() => Task.FromResult(user));
            _forgotPasswordCommand = new ForgotPasswordCommand
            {
                Email = user.Email,
                TcKimlikNo = Convert.ToString(user.CitizenId)
            };
            var result = await _forgotPasswordCommandHandler.Handle(_forgotPasswordCommand, new System.Threading.CancellationToken());
            result.Success.Should().BeTrue();
        }
    }
}
