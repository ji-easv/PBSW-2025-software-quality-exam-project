using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Core.Mapping;
using Core.Services;
using Core.UnitTests.Utils;
using Infrastructure.Interfaces;
using Models.Models;
using Moq;

namespace Core.UnitTests.Services;

public class BoxServiceTests
{
    private readonly BoxService _boxService;
    private readonly Mock<IBoxRepository> _boxRepository;

    public BoxServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        var mapper = config.CreateMapper();
        _boxRepository = new Mock<IBoxRepository>();
        _boxService = new BoxService(_boxRepository.Object, mapper);
    }

    [Fact]
    public async Task GetBoxByIdAsync_BoxExists_ReturnsBox()
    {
        var boxId = BoxUtils.InsertBoxInDb(_boxRepository).Id;
        var result = await _boxService.GetBoxByIdAsync(boxId);

        Assert.NotNull(result);
        Assert.Equal(boxId, result.Id);
    }

    [Fact]
    public async Task GetBoxByIdAsync_BoxDoesNotExist_ThrowsNotFoundException()
    {
        var boxId = Guid.NewGuid();
        _boxRepository.Setup(repo => repo.GetBoxByIdAsync(boxId)).ReturnsAsync((Box?)null);
        await Assert.ThrowsAsync<Models.Exceptions.NotFoundException>(() => _boxService.GetBoxByIdAsync(boxId));
    }

    [Fact]
    public async Task CreateBox_ValidBox_ReturnsCreatedBox()
    {
        var boxCreateDto = BoxUtils.CreateBoxCreateDto();

        _boxRepository.Setup(repo => repo.CreateBoxAsync(It.IsAny<Box>()))
            .ReturnsAsync((Box box) => box);

        var createdBox = await _boxService.CreateBoxAsync(boxCreateDto);

        Assert.NotNull(createdBox);
        Assert.Equal(boxCreateDto.Color, createdBox.Color);
        Assert.Equal(boxCreateDto.Material, createdBox.Material);
        Assert.Equal(boxCreateDto.Price, createdBox.Price);
        Assert.Equal(boxCreateDto.Stock, createdBox.Stock);
        Assert.Equal(boxCreateDto.Weight, createdBox.Weight);
    }

    [Theory]
    [InlineData("Turquoise")]
    [InlineData("Transparent")]
    public async Task CreateBox_InvalidColor_ThrowsValidationException(string invalidColor)
    {
        var box = BoxUtils.CreateBoxCreateDto();
        box.Color = invalidColor;
        await Assert.ThrowsAsync<ValidationException>(() => _boxService.CreateBoxAsync(box));
    }

    [Theory]
    [InlineData("Glass")]
    [InlineData("Moss")]
    public async Task CreateBox_InvalidMaterial_ThrowsValidationException(string invalidMaterial)
    {
        var box = BoxUtils.CreateBoxCreateDto();
        box.Material = invalidMaterial;
        await Assert.ThrowsAsync<ValidationException>(() => _boxService.CreateBoxAsync(box));
    }

    [Fact]
    public async Task DeleteBoxAsync_BoxExists_DeletesBox()
    {
        var box = BoxUtils.InsertBoxInDb(_boxRepository);
        _boxRepository.Setup(repo => repo.DeleteBoxAsync(box)).Returns(Task.CompletedTask);

        await _boxService.DeleteBoxAsync(box.Id);
        _boxRepository.Verify(repo => repo.DeleteBoxAsync(box), Times.Once);
    }

    [Fact]
    public async Task DeleteBoxAsync_BoxDoesNotExist_ThrowsNotFoundException()
    {
        var boxId = Guid.NewGuid();
        _boxRepository.Setup(repo => repo.GetBoxByIdAsync(boxId)).ReturnsAsync((Box?)null);

        await Assert.ThrowsAsync<Models.Exceptions.NotFoundException>(() => _boxService.DeleteBoxAsync(boxId));
    }

    [Fact]
    public async Task UpdateBoxAsync_BoxExists_ReturnsUpdatedBox()
    {
        var existingBox = BoxUtils.InsertBoxInDb(_boxRepository);
        var boxUpdateDto = BoxUtils.CreateBoxUpdateDto();
        
        _boxRepository.Setup(repo => repo.UpdateBoxAsync(It.IsAny<Box>()))
            .ReturnsAsync((Box box) => box);

        var updatedBox = await _boxService.UpdateBoxAsync(existingBox.Id, boxUpdateDto);

        Assert.NotNull(updatedBox);
        Assert.Equal(boxUpdateDto.Color, updatedBox.Color);
        Assert.Equal(boxUpdateDto.Material, updatedBox.Material);
        Assert.Equal(boxUpdateDto.Price, updatedBox.Price);
        Assert.Equal(boxUpdateDto.Stock, updatedBox.Stock);
        Assert.Equal(boxUpdateDto.Weight, updatedBox.Weight);
    }
    
    [Fact]
    public async Task UpdateBoxAsync_BoxDoesNotExist_ThrowsNotFoundException()
    {
        var boxId = Guid.NewGuid();
        _boxRepository.Setup(repo => repo.GetBoxByIdAsync(boxId)).ReturnsAsync((Box?)null);
        var boxUpdateDto = BoxUtils.CreateBoxUpdateDto();

        await Assert.ThrowsAsync<Models.Exceptions.NotFoundException>(() => _boxService.UpdateBoxAsync(boxId, boxUpdateDto));
    }
}