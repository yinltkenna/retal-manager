# Hướng dẫn triển khai chức năng mới (Service)

> Tài liệu này mô tả cách triển khai một chức năng mới trên hệ thống theo từng tầng (layer) chuẩn. Mục tiêu là đảm bảo một chức năng được triển khai đầy đủ từ API (Web) → Application → Domain → Infrastructure, và sau này có thể tách mỗi tầng thành project riêng.

---

## 1. Cấu trúc thư mục (Layered Architecture)
```
/Services
  /[ServiceName]
    /src
      /Application      <-- Business logic + DTO + Mapping + Validation 
        /Definitions    <-- (Optional) seed data / config / constant definitions
        /DTOs
          /Request
          /Response
        /Services
          /Interfaces
          /Implementations
        /Mapping
        /Validator
      /Domain           <-- Domain models & nguyên tắc nghiệp vụ thuần túy
        /Entities
        /Enums
      /Infrastructure   <-- Data access, Caching, Messaging, Clients
        /Caching
        /Clients
        /Data
          /Seed
          AppDbContext.cs
        /Messaging
          /Consumers
          /Outbox
          /Publishers
        /Repositories
          /Interfaces
          /Implementations
      /Web              <-- Web/API layer
        /Common
          /Constants
          /Extensions
          /Helpers
          /TemplateResponses
        /Configurations
        /Controllers
        /Middleware
      appsettings.json
      Dockerfile
      Program.cs
```
---

## 2. Luồng phát triển cho một chức năng mới (end‑to‑end)

### 2.1. Bước 0: Xác định yêu cầu

Trước khi viết code, xác định rõ:
- API sẽ nhận input (request) ra sao (fields, required, optional)
- Output (response) cần chứa gì
- Nghiệp vụ chính là gì (validation, quyền, tương tác DB, gửi event, v.v.)
- Quyền hạn (permission) cần kiểm tra (view/create/update/delete…)

---

## 3. Triển khai theo tầng (từng bước)

### 3.1. Tầng Domain (Entities + Enums)

**Mục đích:** Định nghĩa model và quy tắc nghiệp vụ thuần túy, không phụ thuộc vào framework.

- **Entities**: kế thừa `BaseEntity` (Id/CreatedAt/UpdatedAt/IsDeleted) nếu phù hợp.
- **Enums**: nếu là trạng thái thì nên dùng `public const string ...` (dễ mở rộng) hoặc `public const int ...` với tài liệu rõ ràng.

Ví dụ:
```csharp
public class Amenity : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public static class AmenityStatus
{
    public const string Active = "active";
    public const string Inactive = "inactive";
}
```

> Lưu ý: Nếu sau này tách Domain sang project riêng, chỉ cần tham chiếu namespace mà không phụ thuộc vào Entity Framework hay ASP.NET.

---

### 3.2. Tầng Infrastructure (Data + Repositories + Messaging)

#### 3.2.1. DbContext & cấu hình

- Định nghĩa `AppDbContext` với `DbSet<TEntity>` cho entity mới
- Cấu hình model trong `OnModelCreating` (indexes, relations, constraints, soft‑delete filter nếu cần)

#### 3.2.2. Repository chung (Generic Repository)

Sử dụng generic repository để tránh lặp code. Mẫu:
```csharp
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _db;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext db)
    {
        _db = db;
        _dbSet = db.Set<T>();
    }

    public IQueryable<T> Query() => _dbSet.AsQueryable();

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbSet.FindAsync([id], cancellationToken);

    public async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
        => await _dbSet.ToListAsync(cancellationToken);

    public async Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await _dbSet.Where(predicate).ToListAsync(cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        => await _dbSet.AddAsync(entity, cancellationToken);

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        var prop = typeof(T).GetProperty("IsDeleted");
        if (prop != null && prop.PropertyType == typeof(bool))
        {
            prop.SetValue(entity, true);
            _dbSet.Update(entity);
        }
        else
        {
            _dbSet.Remove(entity);
        }

        return Task.CompletedTask;
    }
}
```

#### 3.2.3. Unit of Work

Đảm bảo mọi thao tác được gộp vào cùng một `SaveChangesAsync`:
```csharp
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(AppDbContext db) => _db = db;

    public IGenericRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T);
        if (_repositories.TryGetValue(type, out var value))
            return (IGenericRepository<T>)value;

        var repo = new GenericRepository<T>(_db);
        _repositories[type] = repo;
        return repo;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _db.SaveChangesAsync(cancellationToken);
}
```

> Tip: Nếu bạn muốn tách Infrastructure ra project riêng, giữ các interface trong Application và implement trong Infrastructure.

---

### 3.3. Tầng Application (Business logic + DTOs + Mapping + Validation)

#### 3.3.1. DTOs (Request/Response)

Tạo DTOs cho request/response theo tên rõ ràng.

Ví dụ:
```csharp
public class CreateAmenityRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class AmenityResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

#### 3.3.2. Mapping (AutoMapper)

Nếu dùng AutoMapper, định nghĩa profile mapping:
```csharp
public class AmenityMappingProfile : Profile
{
    public AmenityMappingProfile()
    {
        CreateMap<CreateAmenityRequest, Amenity>();
        CreateMap<Amenity, AmenityResponse>();
    }
}
```

#### 3.3.3. Validator (FluentValidation hoặc thủ công)

Validator giúp đảm bảo dữ liệu hợp lệ trước khi xử lý:
```csharp
public class CreateAmenityRequestValidator : AbstractValidator<CreateAmenityRequest>
{
    public CreateAmenityRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
```

#### 3.3.4. Service (business logic)

Service chứa logic chính, gọi repository + mapping + permission + validation.

**Permission check (định nghĩa trong EventContracts):**
- Định nghĩa permission string trong `EventContracts/Authorization/Permissions/[Service]/...`
- Seed permission trong `EventContracts/Authorization/Definitions/PermissionDefinitions` và `DefId`.

**Mẫu PermissionChecker** (sử dụng `IAuthorizationService` & `PermissionRequirement`):
```csharp
public class PermissionChecker : IPermissionChecker
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthorizationService _authorizationService;

    public PermissionChecker(IHttpContextAccessor httpContextAccessor,
                             IAuthorizationService authorizationService)
    {
        _httpContextAccessor = httpContextAccessor;
        _authorizationService = authorizationService;
    }

    public async Task<bool> HasPermissionAsync(string permission)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null || !user.Identity?.IsAuthenticated == true)
            return false;

        var result = await _authorizationService.AuthorizeAsync(user, null, new PermissionRequirement(permission));
        return result.Succeeded;
    }
}
```

**Mẫu Service method:**
```csharp
public async Task<ApiResponse<AmenityResponse>> CreateAsync(CreateAmenityRequest request)
{
    if (!await _permissionChecker.HasPermissionAsync(AmenityPermissions.CREATE))
        return ApiResponse<AmenityResponse>.FailResponse("Unauthorized");

    var entity = _mapper.Map<Amenity>(request);
    entity.Id = Guid.NewGuid();
    entity.CreatedAt = DateTime.UtcNow;
    entity.UpdatedAt = DateTime.UtcNow;

    await _unitOfWork.Repository<Amenity>().AddAsync(entity);
    await _unitOfWork.SaveChangesAsync();

    return ApiResponse<AmenityResponse>.SuccessResponse(_mapper.Map<AmenityResponse>(entity));
}
```

> **Quy ước DateTime (VN timezone)**
> Nếu cần trả về UTC chuyển sang VN, có thể dùng converter hoặc mapping helper để quy đổi trước khi gửi.

---

### 3.4. Tầng Web (API layer)

- **Controller**: xây dựng endpoint, gọi service, trả về `ApiResponse<T>` (khi có)
- **Route**: luôn dùng `[Route("api/[controller]")]` và `ApiController`.

Ví dụ:
```csharp
[ApiController]
[Route("api/[controller]")]
public class AmenitiesController : ControllerBase
{
    private readonly IAmenityService _service;

    public AmenitiesController(IAmenityService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create(CreateAmenityRequest request)
    {
        var result = await _service.CreateAsync(request);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
```

---

## 4. Cấu hình chung (appsettings / Program.cs)

Các service nên luôn cấu hình sẵn (dù dev chưa dùng ngay):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=...;User Id=sa;Password=123;TrustServerCertificate=True;"
},
"RabbitMq": {
  "Host": "localhost",
  "Username": "guest",
  "Password": "guest"
},
"Redis": {
  "Connection": "localhost:6379"
},
"JwtSettings": {
  "Key": "...",
  "Issuer": "...",
  "Audience": "...",
  "ExpiresInMinutes": 60
}
```

> Trong `Program.cs`, đăng ký các service chung: DbContext, Redis, RabbitMQ, Authentication/JWT, AutoMapper, FluentValidation, Authorization.

---

## 5. Permission (Authorization) – seed & check

### 5.1. Định nghĩa permission string

- Tạo file `EventContracts/Authorization/Permissions/[Service]/[Feature]Permissions.cs`
- Định nghĩa constants (string) theo dạng: `service.feature.action`

Ví dụ:
```csharp
namespace EventContracts.Authorization.Permissions.PropertyService
{
    public static class AmenityPermissions
    {
        public const string VIEW = "property.amenity.view";
        public const string CREATE = "property.amenity.create";
        public const string UPDATE = "property.amenity.update";
        public const string DELETE = "property.amenity.delete";
    }
}
```

### 5.2. Seed Permission vào DB

- Tạo hoặc cập nhật class trong `EventContracts/Authorization/Definitions/PermissionDefinitions`:

```csharp
public static class AmenityPermissionDefinitions
{
    public static IEnumerable<BasePermissionDefinition> Get()
    {
        yield return new BasePermissionDefinition
        {
            Id = DefId.AmenityViewId,
            Code = AmenityPermissions.VIEW,
            Name = "View Amenity",
            Group = "Amenity",
            SortOrder = 1
        };
        // Thêm các quyền khác khi cần
    }
}
```

- Thêm Guid vào `EventContracts/Authorization/DefId.cs` để phục vụ seed.

---

## 6. Notes khi tách sang project riêng (future-proof)

- **Domain**: chỉ chứa Entities/Enums/ValueObjects/DomainEvents.
- **Application**: chứa Interfaces + DTOs + Services + Mappings. Không tham chiếu trực tiếp đến EF.
- **Infrastructure**: chứa EF Core, Repositories, Messaging, Clients.
- **Web**: chỉ chứa Controller + Middleware + cấu hình API.

> Nếu bạn chuyển sang cấu trúc multi-project, giữ nguyên tên folder/scope như hiện tại; chỉ cần tách thành các project con và giữ pattern tổ chức.

---

## 7. Quy ước chung

- Luôn dùng `ApiResponse<T>` (hoặc kiểu wrapper thống nhất) để trả kết quả từ service.
- Mọi endpoint nên trả `200` khi logic thành công, `400/401/403/404` khi lỗi hợp lệ.
- Không để business logic trong Controller.
- Checker quyền phải nằm trong Application (hoặc một tầng riêng) và được gọi trước khi thực thi logic quan trọng.

---

✅ **Kết luận**: Khi làm chức năng mới, cứ làm theo flow: **Request DTO → Validator → Service → Repository / Domain → Commit (UnitOfWork) → Response DTO → Controller**. Nếu cần permission thì bổ sung bước **permission check** ở đầu service. Buộc phải đi qua cả 4 layer (Web/Application/Domain/Infrastructure) để giữ cấu trúc rõ ràng.
