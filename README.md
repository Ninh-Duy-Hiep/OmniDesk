# OmniDesk - Backend Service

Dự án Backend cho hệ thống OmniDesk, được xây dựng trên nền tảng **ASP.NET Core** và **MySQL**, áp dụng mô hình kiến trúc **Onion Architecture / Clean Architecture** nhằm đảm bảo tính độc lập, dễ mở rộng và thuận tiện cho việc viết kiểm thử tự động.

---

```text
OmniDesk/
├── OmniDesk.Domain/                                    # [Core] Lõi nghiệp vụ độc lập, không phụ thuộc tầng khác
│   ├── Common/                                         # Entity cơ sở, Auditable, Domain Events
│   │   ├── BaseEntity.cs
│   │   └── BaseAuditableEntity.cs
│   ├── Entities/                                       # Thực thể nghiệp vụ (User, Desk, Message, ...)
│   │   └── User.cs
│   ├── Enums/                                          # Kiểu liệt kê trạng thái, vai trò
│   │   └── UserRole.cs
│   └── Exceptions/                                     # Exception nghiệp vụ riêng của Domain
│       └── DomainException.cs
│
├── OmniDesk.Application/                               # [Core] Điều phối Use Cases, CQRS và quy tắc ứng dụng
│   ├── Common/                                         # Thành phần dùng chung nội bộ Application
│   │   ├── Behaviors/                                  # MediatR Pipeline Behaviors (Cross-cutting concerns)
│   │   │   ├── LoggingBehavior.cs                      # Ghi log request/response
│   │   │   ├── PerformanceBehavior.cs                  # Cảnh báo request chạy chậm
│   │   │   └── ValidationBehavior.cs                   # Tự động bắt lỗi FluentValidation trước khi vào Handler
│   │   ├── Exceptions/                                 # App-level exceptions (AppValidationException, BusinessException, NotFoundException)
│   │   │   ├── BusinessException.cs
│   │   │   └── AppValidationException.cs
│   │   └── Models/                                     # Định dạng đóng gói dữ liệu và Response chuẩn
│   │       ├── ApiResponse.cs                          # Wrapper response chuẩn cho toàn hệ thống
│   │       ├── ApiError.cs                             # Chi tiết lỗi theo từng field
│   │       └── Pagination/                             # Models phân trang
│   │           ├── IPagination.cs
│   │           ├── PagePagination.cs                   # Phân trang offset-based
│   │           └── CursorPagination.cs                 # Phân trang cursor-based
│   ├── DTOs/                                           # Data Transfer Objects chia sẻ
│   │   └── UserDtos.cs
│   ├── Interfaces/                                     # Contracts trừu tượng cho hạ tầng triển khai
│   │   ├── IApplicationDbContext.cs                    # Trừu tượng DbContext
│   │   ├── IJwtTokenGenerator.cs                       # Hợp đồng phát sinh JWT
│   │   └── Services/                                   # Giao diện dịch vụ ngoại vi & Realtime
│   │       └── INotificationHubService.cs              # Hợp đồng gửi thông báo socket sang Client
│   └── Modules/                                        # Phân cụm Use Cases theo Domain Module (Feature Folders)
│       ├── Auth/                                       # Module xác thực
│       │   ├── Commands/
│       │   │   └── Login/
│       │   │       ├── LoginCommand.cs
│       │   │       ├── LoginCommandHandler.cs
│       │   │       └── LoginCommandValidator.cs
│       │   └── Queries/
│       └── Users/                                      # Module người dùng
│           ├── Commands/
│           │   ├── CreateUser/
│           │   │   ├── CreateUserCommand.cs
│           │   │   ├── CreateUserCommandHandler.cs
│           │   │   └── CreateUserCommandValidator.cs
│           │   ├── DeleteUser/
│           │   │   ├── DeleteUserCommand.cs
│           │   │   └── DeleteUserCommandHandler.cs
│           │   └── UpdateUser/
│           │       ├── UpdateUserCommand.cs
│           │       ├── UpdateUserCommandHandler.cs
│           │       └── UpdateUserCommandValidator.cs
│           └── Queries/
│               ├── GetAllUsers/
│               │   ├── GetAllUsersQuery.cs
│               │   └── GetAllUsersQueryHandler.cs
│               └── GetUserById/
│                   ├── GetUserByIdQuery.cs
│                   └── GetUserByIdQueryHandler.cs
│
├── OmniDesk.Infrastructure/                            # [Outer] Hiện thực hóa kỹ thuật Database & Dịch vụ ngoài
│   ├── Data/                                           # Tầng truy xuất dữ liệu (EF Core)
│   │   ├── Configurations/                             # Cấu hình EntityTypeConfiguration (Fluent API)
│   │   │   └── UserConfiguration.cs
│   │   └── Context/                                    # DbContext kế thừa IApplicationDbContext
│   │       └── AppDbContext.cs
│   ├── Migrations/                                     # EF Core Migrations
│   ├── Realtime/                                       # Triển khai tầng gửi tin SignalR phía Infrastructure
│   │   └── NotificationHubService.cs
│   └── Services/                                       # Triển khai các dịch vụ kỹ thuật[cite: 1]
│       └── JwtTokenGenerator.cs
│
└── OmniDesk.API/                                       # [Outer] Điểm tiếp nhận request từ Client (Presentation Layer)[cite: 1]
    ├── Controllers/                                    # API Endpoints tiếp nhận HTTP Request[cite: 1]
    │   ├── BaseApiController.cs                        # Controller cơ sở trợ giúp trả về ApiResponse chuẩn
    │   ├── AuthController.cs
    │   └── UsersController.cs
    ├── Extensions/                                     # Extension methods cấu hình DI, Swagger, Auth, DB[cite: 1]
    │   ├── AuthenticationExtensions.cs
    │   ├── CorsExtensions.cs
    │   ├── DatabaseExtensions.cs
    │   ├── MediatRExtensions.cs
    │   ├── SerilogExtensions.cs
    │   └── SwaggerExtensions.cs
    ├── Filters/                                        # Action/Result Filters chuẩn hóa Response & Validation
    │   └── ApiResponseFilter.cs                        # (Tùy chọn) Bọc response tự động
    ├── Hubs/                                           # SignalR Hubs phục vụ kết nối WebSocket thời gian thực
    │   ├── NotificationHub.cs
    │   └── ChatHub.cs
    ├── Middlewares/                                    # Custom Middlewares xử lý request pipeline[cite: 1]
    │   ├── ExceptionHandlingMiddleware.cs              # Bắt toàn bộ Exception và trả ra payload chuẩn JSON
    │   └── TraceIdMiddleware.cs                        # Gán TraceId xuyên suốt request
    ├── Logs/                                           # Thư mục chứa log định kỳ (Serilog)
    ├── Properties/                                     # Cấu hình launchSettings.json[cite: 1]
    ├── appsettings.json                                # Cấu hình ConnectionString, JWT Secret, v.v.[cite: 1]
    └── Program.cs                                      # Entry point, đăng ký DI và Pipeline middleware[cite: 1]
