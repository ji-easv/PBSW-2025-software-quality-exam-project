using BoxFactory.IntegrationTests.Data;
using BoxFactory.IntegrationTests.Setup;
using Models.DTOs;
using Models.Models;
using TestUtils;

namespace BoxFactory.IntegrationTests;

public class BoxTests(CustomWebApplicationFactory factory) : ApiTestBase(factory)
{
    [Fact]
    public async Task CreateBox_200()
    {
        var boxCreateDto = ModelUtils.ValidBoxCreateDto();
        var response = await Client.PostAsJsonAsync("/Box", boxCreateDto);

        response.EnsureSuccessStatusCode();
        var createdBox = await response.Content.ReadFromJsonAsync<Box>();
        Assert.NotNull(createdBox);
        Assert.Equal(boxCreateDto.Color, createdBox!.Color);
        Assert.Equal(boxCreateDto.Material, createdBox.Material);
        Assert.Equal(boxCreateDto.Price, createdBox.Price);
        Assert.Equal(boxCreateDto.Stock, createdBox.Stock);
        Assert.Equal(boxCreateDto.Weight, createdBox.Weight);
        Assert.Equal(boxCreateDto.DimensionsDto!.Length, createdBox.Dimensions?.Length);
        Assert.Equal(boxCreateDto.DimensionsDto.Width, createdBox.Dimensions?.Width);
        Assert.Equal(boxCreateDto.DimensionsDto.Height, createdBox.Dimensions?.Height);
    }

    [Theory]
    [ClassData(typeof(CreateBoxTestData))]
    public async Task CreateBox_InvalidData_400(BoxCreateDto boxCreateDto)
    {
        var response = await Client.PostAsJsonAsync("/Box", boxCreateDto);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetBox_200()
    {
        var existingBox = ModelUtils.ValidBox();
        var dbContext = NewDbContext;
        await dbContext.Boxes.AddAsync(existingBox);
        await dbContext.SaveChangesAsync();

        var response = await Client.GetAsync($"/Box/{existingBox.Id}");
        response.EnsureSuccessStatusCode();

        var retrievedBox = await response.Content.ReadFromJsonAsync<Box>();
        Assert.NotNull(retrievedBox);
        Assert.Equal(existingBox.Id, retrievedBox!.Id);
        Assert.Equal(existingBox.Color, retrievedBox.Color);
        Assert.Equal(existingBox.Material, retrievedBox.Material);
        Assert.Equal(existingBox.Price, retrievedBox.Price);
        Assert.Equal(existingBox.Stock, retrievedBox.Stock);
        Assert.Equal(existingBox.Weight, retrievedBox.Weight);
        Assert.Equal(existingBox.Dimensions!.Length, retrievedBox.Dimensions?.Length);
        Assert.Equal(existingBox.Dimensions.Width, retrievedBox.Dimensions?.Width);
        Assert.Equal(existingBox.Dimensions.Height, retrievedBox.Dimensions?.Height);
    }

    [Fact]
    public async Task GetBox_NotFound_404()
    {
        var response = await Client.GetAsync($"/Box/{Guid.Empty}");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteBox_204()
    {
        var existingBox = ModelUtils.ValidBox();
        var dbContext = NewDbContext;
        await dbContext.Boxes.AddAsync(existingBox);
        await dbContext.SaveChangesAsync();

        var response = await Client.DeleteAsync($"/Box/{existingBox.Id}");
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);

        dbContext = NewDbContext;
        var deletedBox = await dbContext.Boxes.FindAsync(existingBox.Id);
        Assert.Null(deletedBox);
    }

    [Fact]
    public async Task DeleteBox_NotFound_404()
    {
        var response = await Client.DeleteAsync($"/Box/{Guid.Empty}");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task UpdateBox_200()
    {
        var existingBox = ModelUtils.ValidBox();
        var dbContext = NewDbContext;
        await dbContext.Boxes.AddAsync(existingBox);
        await dbContext.SaveChangesAsync();

        var boxUpdateDto = ModelUtils.ValidBoxUpdateDto();
        var response = await Client.PutAsJsonAsync($"/Box/{existingBox.Id}", boxUpdateDto);
        response.EnsureSuccessStatusCode();

        var updatedBox = await response.Content.ReadFromJsonAsync<Box>();
        Assert.NotNull(updatedBox);
        Assert.Equal(existingBox.Id, updatedBox!.Id);
        Assert.Equal(boxUpdateDto.Color, updatedBox.Color);
        Assert.Equal(boxUpdateDto.Material, updatedBox.Material);
        Assert.Equal(boxUpdateDto.Price, updatedBox.Price);
        Assert.Equal(boxUpdateDto.Stock, updatedBox.Stock);
        Assert.Equal(boxUpdateDto.Weight, updatedBox.Weight);
        Assert.Equal(boxUpdateDto.DimensionsDto!.Length, updatedBox.Dimensions?.Length);
        Assert.Equal(boxUpdateDto.DimensionsDto.Width, updatedBox.Dimensions?.Width);
        Assert.Equal(boxUpdateDto.DimensionsDto.Height, updatedBox.Dimensions?.Height);
    }

    [Theory]
    [ClassData(typeof(UpdateBoxTestData))]
    public async Task UpdateBox_InvalidData_400(BoxUpdateDto boxUpdateDto)
    {
        var existingBox = ModelUtils.ValidBox();
        var dbContext = NewDbContext;
        await dbContext.Boxes.AddAsync(existingBox);
        await dbContext.SaveChangesAsync();

        var response = await Client.PutAsJsonAsync($"/Box/{existingBox.Id}", boxUpdateDto);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }
}