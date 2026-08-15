using AutoMapper;
using PreschoolManagement.Application.Common;
using PreschoolManagement.Application.DTOs;
using PreschoolManagement.Application.Interfaces;
using PreschoolManagement.Domain.Common;
using PreschoolManagement.Domain.Entities;
using PreschoolManagement.Infrastructure.Identity;

namespace PreschoolManagement.Infrastructure.Services;

public class CrudService<TEntity, TDto, TCreateDto, TUpdateDto> : ICrudService<TDto, TCreateDto, TUpdateDto>
    where TEntity : BaseEntity
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly Func<IUnitOfWork, IGenericRepository<TEntity>> _repositoryFactory;

    public CrudService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        Func<IUnitOfWork, IGenericRepository<TEntity>> repositoryFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _repositoryFactory = repositoryFactory;
    }

    public async Task<PagedResult<TDto>> GetAllAsync(QueryParameters query, CancellationToken cancellationToken = default)
    {
        var result = await _repositoryFactory(_unitOfWork).GetPagedAsync(query, cancellationToken);
        return new PagedResult<TDto>
        {
            Items = _mapper.Map<IEnumerable<TDto>>(result.Items),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repositoryFactory(_unitOfWork).GetByIdAsync(id, cancellationToken);
        return entity is null ? default : _mapper.Map<TDto>(entity);
    }

    public async Task<TDto> CreateAsync(TCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<TEntity>(dto);
        if (entity is User user && dto is UserCreateDto userDto)
        {
            user.PasswordHash = PasswordHasherUtility.HashPassword(userDto.Password);
        }

        await AddAddressIfPresentAsync(dto!, entity, cancellationToken);
        await _repositoryFactory(_unitOfWork).AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<TDto>(entity);
    }

    public async Task<TDto?> UpdateAsync(Guid id, TUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var repository = _repositoryFactory(_unitOfWork);
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            return default;
        }

        _mapper.Map(dto, entity);
        await AddAddressIfPresentAsync(dto!, entity, cancellationToken);
        repository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TDto>(entity);
    }

    private async Task AddAddressIfPresentAsync(object dto, TEntity entity, CancellationToken cancellationToken)
    {
        if (entity is not IAddressOwner addressOwner || dto is not SchoolCreateDto and not TeacherCreateDto and not StudentCreateDto)
        {
            return;
        }

        var address = dto switch
        {
            SchoolCreateDto school => _mapper.Map<Address>(school.Address),
            TeacherCreateDto teacher => _mapper.Map<Address>(teacher.Address),
            StudentCreateDto student => _mapper.Map<Address>(student.Address),
            _ => throw new InvalidOperationException("An address is required for this resource.")
        };

        addressOwner.AddressId = address.Id;
        await _unitOfWork.Addresses.AddAsync(address, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var repository = _repositoryFactory(_unitOfWork);
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            return false;
        }

        repository.SoftDelete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
