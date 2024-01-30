using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PetConnect.Controllers;
using PetConnect.Data;
using PetConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PetConnectTest
{
    public class UsersControllerTests
    {

        private Mock<ApplicationDbContext> _dbContextMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private UsersController _controller;

        [SetUp]
        public void Setup()
        {
            _dbContextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());

            // Se creeaza mock pentru UserManager<ApplicationUser>
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            // Se creeaza mock pentru RoleManager<IdentityRole>
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                new IRoleValidator<IdentityRole>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);

            _controller = new UsersController(
                _dbContextMock.Object,
                _userManagerMock.Object,
                _roleManagerMock.Object
            );

            
            _userManagerMock
                .Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser { });

        }
         [Test]
        public async Task ShowProfile_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var userId = "d8025b90-4b6f-421e-8b47-719fbfeca83d";
            var user = new ApplicationUser { Id = userId };
            _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _controller.ShowProfile(userId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual("ShowProfile", viewResult.ViewName);
            Assert.AreEqual(user, viewResult.Model);
        }
        [Test]
        public async Task EditProfile_UserDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var claimsPrincipal = new Mock<ClaimsPrincipal>(); 
            _userManagerMock.Setup(um => um.GetUserAsync(claimsPrincipal.Object)).ReturnsAsync((ApplicationUser)null);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal.Object }
            };

            // Act
            var result = await _controller.EditProfile();

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task EditProfile_UserExists_ReturnsViewResultWithUser()
        {
            // Arrange
            var userId = "d8025b90-4b6f-421e-8b47-719fbfeca83d";
            var user = new ApplicationUser { Id = userId };
            var claimsPrincipal = new Mock<ClaimsPrincipal>(); 
            _userManagerMock.Setup(um => um.GetUserAsync(claimsPrincipal.Object)).ReturnsAsync(user);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal.Object }
            };

            // Act
            var result = await _controller.EditProfile();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(user, viewResult.Model);
        }
        [Test]
        public async Task ShowProfile_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var invalidUserId = "invalidUserId";
            _userManagerMock.Setup(um => um.FindByIdAsync(invalidUserId)).ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _controller.ShowProfile(invalidUserId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
       

    }
}
