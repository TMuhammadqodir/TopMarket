using AutoMapper;
using Castle.Core.Logging;
using Data.Contexts;
using Data.IRepositories;
using Data.Repositories;
using Domain.Entities.Addresses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Service.Interfaces;
using Service.Mappers;
using Service.Services;

namespace TopMarket.IntegrationTests.ServiceTests;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private IMapper mapper;
    private AppDbContext dbContext;
    private IRepository<Address> addressRepository;
    private Mock<ILogger<AddressService>> addressServiceLoggerMock;

    public IAddressService AddressService;
    
    public CustomWebApplicationFactory()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase("TestDb")
                    .Options;
        
        this.dbContext = new AppDbContext(options);
        this.mapper = new MapperConfiguration(c => c.AddProfile<MappingProfile>()).CreateMapper();
        this.addressServiceLoggerMock = new Mock<ILogger<AddressService>>();
        this.addressRepository = new Repository<Address>(dbContext);

        this.AddressService = new AddressService(addressServiceLoggerMock.Object, mapper, addressRepository);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(this.AddressService);
        });
    }
}
