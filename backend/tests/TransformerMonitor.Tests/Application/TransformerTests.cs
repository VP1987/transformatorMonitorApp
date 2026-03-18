using AutoMapper;
using Moq;
using TransformerMonitor.Application.DTOs;
using TransformerMonitor.Application.Transformers.Handlers;
using TransformerMonitor.Application.Transformers.Queries;
using TransformerMonitor.Application.Transformers.Commands;
using TransformerMonitor.Application.Transformers.Validators;
using TransformerMonitor.Domain.Entities;
using TransformerMonitor.Domain.Interfaces;
using Xunit;
using FluentValidation.TestHelper;

namespace TransformerMonitor.Tests.Application;

public class TransformerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ITransformerRepository> _transformerRepoMock;

    public TransformerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _transformerRepoMock = new Mock<ITransformerRepository>();
        
        // Setup UnitOfWork to return the transformer repository mock
        _unitOfWorkMock.Setup(u => u.Transformers).Returns(_transformerRepoMock.Object);
    }

    [Fact]
    public async Task GetAllTransformers_ShouldReturnEmpty_WhenNoData()
    {
        _transformerRepoMock.Setup(u => u.GetAllAsync()).ReturnsAsync(new List<Transformer>());
        _mapperMock.Setup(m => m.Map<IEnumerable<TransformerDto>>(It.IsAny<IEnumerable<Transformer>>()))
                   .Returns(new List<TransformerDto>());

        var handler = new GetTransformersHandlers(_unitOfWorkMock.Object, _mapperMock.Object);

        var result = await handler.Handle(new GetAllTransformersQuery(), CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetTransformerById_ShouldReturnNull_WhenNotFound()
    {
        _transformerRepoMock.Setup(u => u.GetWithReadingsAsync(It.IsAny<int>(), 10))
                       .ReturnsAsync((Transformer?)null);

        var handler = new GetTransformersHandlers(_unitOfWorkMock.Object, _mapperMock.Object);

        var result = await handler.Handle(new GetTransformerByIdQuery(99), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateTransformer_ShouldCallAddAndComplete()
    {
        var command = new CreateTransformerCommand(101, "New Trans", "Region X", 22000);
        _mapperMock.Setup(m => m.Map<TransformerDto>(It.IsAny<Transformer>())).Returns(new TransformerDto());

        var handler = new TransformerCommandHandlers(_unitOfWorkMock.Object, _mapperMock.Object);

        await handler.Handle(command, CancellationToken.None);

        _transformerRepoMock.Verify(u => u.AddAsync(It.IsAny<Transformer>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public void Validator_ShouldHaveError_WhenNameIsEmpty()
    {
        var validator = new CreateTransformerCommandValidator();
        var command = new CreateTransformerCommand(1, "", "Region", 20000);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(v => v.Name);
    }

    [Fact]
    public void Validator_ShouldHaveError_WhenVoltageIsTooLow()
    {
        var validator = new CreateTransformerCommandValidator();
        var command = new CreateTransformerCommand(1, "Name", "Region", 500);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(v => v.BaseVoltage);
    }
}
