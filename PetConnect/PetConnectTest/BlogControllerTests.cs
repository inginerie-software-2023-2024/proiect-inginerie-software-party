using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PetConnect.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnectTest
{
    public class BlogControllerTests
    {
        [Test]
        public void Blog1_ReturnsViewResult()
        {
            // Arrange
            var controller = new BlogsController();

            // Act
            var result = controller.Blog1() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Blog2_ReturnsViewResult()
        {
            // Arrange
            var controller = new BlogsController();

            // Act
            var result = controller.Blog2() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Blog3_ReturnsViewResult()
        {
            // Arrange
            var controller = new BlogsController();

            // Act
            var result = controller.Blog3() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
