# OmniDesk - Backend Service

Dự án Backend cho hệ thống OmniDesk, được xây dựng trên nền tảng **ASP.NET Core** và **MySQL**, áp dụng mô hình kiến trúc **Onion Architecture / Clean Architecture** nhằm đảm bảo tính độc lập, dễ mở rộng và thuận tiện cho việc viết kiểm thử tự động.

---

```text
OmniDesk/
├── OmniDesk.Domain/                                      # [Lõi trung tâm] Không phụ thuộc thư viện ngoại vi
│   ├── Common/                                           # BaseEntity, BaseAuditableEntity, ValueObject
│   ├── Entities/                                         # User, Desk, Booking
│   ├── Enums/                                            # UserRole, DeskStatus, BookingStatus
│   └── Exceptions/                                       # DomainException (vi phạm quy tắc nghiệp vụ lõi)
│
├── OmniDesk.Application/                                 # [Orchestration] Điều phối nghiệp vụ, chia theo Feature CQRS[cite: 1, 2]
│   ├── Common/                                           # Thành phần dùng chung toàn tầng Application
│   │   ├── Behaviors/                                    # MediatR Pipeline: ValidationBehavior, LoggingBehavior
│   │   ├── Exceptions/                                   # ValidationException, NotFoundException
│   │   └── Models/                                       # PagedResult<T>, Result<T>
│   ├── Interfaces/                                       # Cổng giao tiếp ra ngoài (Ports): IApplicationDbContext, IEmailSender
│   └── Modules/ (hoặc Features/)                         # Đóng gói theo lát cắt chức năng (Vertical Slice)
│       ├── Users/
│       │   ├── Commands/                                 # Write operations: CreateUser, UpdateUser, DeleteUser[cite: 1, 2]
│       │   │   └── CreateUser/
│       │   │       ├── CreateUserCommand.cs
│       │   │       ├── CreateUserCommandHandler.cs
│       │   │       └── CreateUserCommandValidator.cs
│       │   └── Queries/                                  # Read-only operations: GetAllUsers, GetUserById[cite: 1, 2]
│       │       └── GetUserById/
│       │           ├── GetUserByIdQuery.cs
│       │           └── GetUserByIdQueryHandler.cs
│       ├── ...
│
├── OmniDesk.Infrastructure/                              # [Hạ tầng kỹ thuật] Triển khai database, cache, third-party[cite: 1]
│   ├── Data/
│   │   ├── Configurations/                               # Fluent API mapping cho từng Entity[cite: 1]
│   │   ├── Context/                                      # AppDbContext kế thừa IApplicationDbContext
│   │   └── Migrations/                                   # File migration của EF Core[cite: 1]
│   └── Services/                                         # JwtTokenGenerator, BCryptPasswordHasher, SmtpEmailService[cite: 1]
│
└── OmniDesk.API/                                         # [Presentation] Nhận HTTP request, mỏng tối đa[cite: 1]
    ├── Controllers/                                      # Gọi ISender để dispatch Command/Query[cite: 1]
    ├── Extensions/                                       # DependencyInjection, Swagger, Cors, Authentication
    ├── Middlewares/                                      # GlobalExceptionHandlingMiddleware[cite: 1]
    ├── Program.cs                                        # Đăng ký DI và pipeline[cite: 1]
    └── appsettings.json                                  # Connection strings và config hệ thống[cite: 1]
