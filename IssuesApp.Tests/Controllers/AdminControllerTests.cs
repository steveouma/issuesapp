using AutoMapper;
using Castle.Core.Logging;
using FakeItEasy;
using FluentAssertions;
using IssuesApp.Controllers;
using IssuesApp.Dto;
using IssuesApp.Interfaces;
using IssuesApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssuesApp.Tests.Controllers
{
    public class AdminControllerTests
    {
        private readonly ILogger<AdminController> _fakeLogger;
        private readonly IAdminRepository _fakeAdminRepository;
        private readonly IMapper _fakeMapper;
        private AdminController _controller;
        public AdminControllerTests()
        {
            _fakeLogger = A.Fake<ILogger<AdminController>>();
            _fakeAdminRepository = A.Fake<IAdminRepository>();
            _fakeMapper = A.Fake<IMapper>();
            _controller = new AdminController(_fakeLogger, _fakeAdminRepository, _fakeMapper);
        }

        [Fact]
        public void AdminController_GetAdmins_ReturnOK()
        {
            // Arrange

            var admins = A.Fake<ICollection<AdminDto>>();
            var adminList = A.Fake<List<AdminDto>>();

            A.CallTo(()=> _fakeMapper.Map<List<AdminDto>>(admins)).Returns(adminList);

            // Act

            var result = _controller.GetAdmins();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void AdminController_CreateAdmin_ReturnsSuccess()
        {
            // Arrange

            var admin = A.Fake<Admin>();
            var adminCreate = A.Fake<AdminDto>();
            var adminMap = A.Fake<Admin>();

            A.CallTo(() => _fakeAdminRepository.CheckAdminEmail(adminCreate)).Returns(admin);
            A.CallTo(() => _fakeMapper.Map<Admin>(adminCreate)).Returns(admin);
            A.CallTo(() => _fakeAdminRepository.CreateAdmin(adminMap)).Returns(true);

            // Act

            var result = _controller.CreateAdmin(adminCreate);

            // Assert

            result.Should().NotBeNull();
        }

        [Fact]
        public void AdminController_GetAdmin_ShouldNotBeNull()
        {
            // Arrange

            var testAdminId = 1;
            var adminDto = A.Fake<AdminDto>();
            A.CallTo(() => _fakeMapper.Map<AdminDto>(_fakeAdminRepository.GetAdmin(testAdminId))).Returns(adminDto);

            // Act

            var result = _controller.GetAdmin(testAdminId);

            // Assert

            result.Should().NotBeNull();
        }

        [Fact]
        public void AdminController_UpdateAdmin_ShouldNotBeNull()
        {
            var testAdminId = 1;
            var admin = A.Fake<Admin>();
            var updateAdmin = A.Fake<AdminDto>();
            var adminMap = A.Fake<Admin>();

            A.CallTo(() => _fakeAdminRepository.AdminExists(testAdminId)).Returns(true);
            A.CallTo(() => _fakeMapper.Map<Admin>(updateAdmin)).Returns(admin);
            A.CallTo(() => _fakeAdminRepository.UpdateAdmin(adminMap)).Returns(true);

            var result = _controller.UpdateAdmin(testAdminId, updateAdmin);

            result.Should().NotBeNull();
        }
    }
}
