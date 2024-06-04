using Service.DTOs.Addresses;
using Service.Exceptions;

namespace TopMarket.IntegrationTests.ServiceTests;

public class AddressServiceTests : IDisposable
{
    private CustomWebApplicationFactory factory;

    public AddressServiceTests()
    {
        this.factory = new CustomWebApplicationFactory();
    }

    [Fact]
    public async Task AddNewAddress_ShouldAdd()
    {
        //Arrange
        var addresses = this.getFakeAddresses();

        //Act
        var results = new List<AddressResultDto>();
        foreach (var address in addresses)
            results.Add(await this.factory.AddressService.CreateAsync(address));

        //Assert
        foreach (var result in results)
            Assert.NotNull(result);
    }

    [Fact]
    public async Task ModifyAddress_ShouldBeSuccessfull()
    {
        //Arrange
        var addedAddress = await this.factory.AddressService.CreateAsync(this.getFakeAddresses().First());
        var address = new AddressUpdateDto
        {
            Id = addedAddress.Id,
            CountryId = 1,
            DistrictId = 2,
            DoorCode = "3",
            Floor = "4",
            Home = "5",
            RegionId = 6,
            Street = "7th street"
        };

        //Act
        var result = await this.factory.AddressService.ModifyAsync(address);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task RemoveAddress_ShouldRemove()
    {
        //Arrange
        var addedAddress = await this.factory.AddressService.CreateAsync(this.getFakeAddresses().First());

        //Act
        var result = await this.factory.AddressService.RemoveAsync(addedAddress.Id);

        //Assert
        Assert.True(result);
    }

    public void Dispose()
    {
        this.factory.Dispose();
    }

    private IList<AddressCreationDto> getFakeAddresses()
    {
        var addresses = new List<AddressCreationDto>
        {
            new AddressCreationDto
            {
                Street = "Amir Temur",
                CountryId = 1,
                DistrictId = 1,
                DoorCode = "33",
                Floor = "Ground floor",
                Home = "33",
                RegionId = 1
            },

            new AddressCreationDto
            {
                CountryId = 1,
                DistrictId = 2,
                DoorCode = "3",
                Floor = "4",
                Home = "5",
                RegionId = 6,
                Street = "Ming o'rik"
            }
        };

        return addresses;
    }
}
