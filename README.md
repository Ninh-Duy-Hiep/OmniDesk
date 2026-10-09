# OmniDesk - Backend Service

Dự án Backend cho hệ thống OmniDesk, được xây dựng trên nền tảng **ASP.NET Core** và **MySQL**, áp dụng mô hình kiến trúc **Onion Architecture / Clean Architecture** nhằm đảm bảo tính độc lập, dễ mở rộng và thuận tiện cho việc viết kiểm thử tự động.

---

```text
OmniDesk/
+---OmniDesk.API                                          # [Tầng Presentation/Web API] Tiếp nhận HTTP Request và định tuyến phản hồi
|   |   appsettings.Development.json                      # Cấu hình môi trường phát triển (chuỗi kết nối DB dev, log debug)
|   |   appsettings.json                                  # Cấu hình chung của ứng dụng (JWT key/issuer, ConnectionStrings, Serilog)
|   |   OmniDesk.API.csproj                               # File cấu hình project API, quản lý SDK Web và các gói NuGet cài đặt
|   |   OmniDesk.API.csproj.user                          # File lưu cấu hình giao diện Visual Studio cá nhân của lập trình viên
|   |   OmniDesk.API.http                                 # File script kiểm thử gọi API nhanh trực tiếp trong IDE (thay thế Postman nhẹ)
|   |   Program.cs                                        # Entry point: Đăng ký DI container, nạp extensions và cấu hình HTTP pipeline[cite: 1]
|   |
|   +---Controllers                                       # Thư mục chứa các API Endpoints tiếp nhận HTTP request[cite: 1]
|   |       AuthController.cs                             # Controller xử lý các tác vụ xác thực (Login, cấp phát Token)[cite: 1]
|   |       BaseApiController.cs                          # Controller cha: Tích hợp sẵn MediatR Sender và helper xử lý ApiResponse[cite: 1]
|   |       UsersController.cs                            # Controller tiếp nhận CRUD người dùng, chuyển tiếp qua MediatR[cite: 1]
|   |
|   +---Extensions                                        # Các phương thức mở rộng giúp Program.cs gọn gàng, tách biệt cấu hình[cite: 1]
|   |       AuthenticationExtensions.cs                   # Cấu hình JWT Bearer Authentication và chính sách ủy quyền (Authorization)[cite: 1]
|   |       CorsExtensions.cs                             # Cấu hình CORS cho phép các domain client (frontend) truy cập API[cite: 1]
|   |       DatabaseExtensions.cs                         # Đăng ký AppDbContext kết nối cơ sở dữ liệu qua EF Core[cite: 1]
|   |       MediatRExtensions.cs                          # Đăng ký thư viện MediatR và quét các Pipeline Behaviors[cite: 1]
|   |       SerilogExtensions.cs                          # Cấu hình ghi log có cấu trúc qua Serilog (ghi ra console và file rolling)[cite: 1]
|   |       SwaggerExtensions.cs                          # Cấu hình tài liệu OpenAPI/Swagger kèm hỗ trợ truyền Bearer Token[cite: 1]
|   |
|   +---Filters                                           # Các bộ lọc can thiệp vào vòng đời xử lý action/result[cite: 1]
|   |       ApiResponseFilter.cs                          # Result Filter tự động bọc response thành công về schema ApiResponse[cite: 1]
|   |
|   +---logs                                              # Thư mục lưu trữ các tệp nhật ký thực thi hệ thống sinh ra từ Serilog[cite: 1]
|   |       omnidesk_log-20261008.txt                     # Tệp log xoay vòng của ngày 08/10/2026
|   |       omnidesk_log-20261009.txt                     # Tệp log xoay vòng của ngày 09/10/2026
|   |
|   +---Middlewares                                       # Các middleware tùy biến xử lý trong luồng HTTP request pipeline[cite: 1]
|   |       ExceptionHandlingMiddleware.cs                # Bắt ngoại lệ toàn cục (unhandled exception), serialize ra JSON chuẩn[cite: 1]
|   |       TraceIdMiddleware.cs                          # Cấp phát hoặc lấy X-Trace-Id từ header để gán vết cho request và log[cite: 1]
|   |
|   +---Properties
|   |       launchSettings.json                           # Cấu hình profile khởi chạy của ứng dụng (cổng port HTTP/HTTPS, IIS)[cite: 1]
|   |
|   \---Service
|           CurrentUserService.cs                         # Triển khai ICurrentUserService: Trích xuất UserId/UserName từ HttpContext.User
|
+---OmniDesk.Application                                  # [Tầng Ứng dụng] Chứa logic nghiệp vụ hệ thống, CQRS và điều phối Use Cases[cite: 1]
|   |   OmniDesk.Application.csproj                      # Quản lý thư viện MediatR, FluentValidation và tham chiếu đến Domain
|   |
|   +---Common                                            # Thành phần nền tảng dùng chung nội bộ tầng Application[cite: 1]
|   |   +---Behaviors                                     # MediatR Pipeline Behaviors đóng vai trò middleware cho request[cite: 1]
|   |   |       LoggingBehavior.cs                        # Ghi log tên request, tham số đầu vào và kết quả trả về[cite: 1]
|   |   |       PerformanceBehavior.cs                    # Đo thời gian thực thi, ghi log Warning nếu request chạy chậm (>500ms)[cite: 1]
|   |   |       ValidationBehavior.cs                     # Tự động quét và thực thi FluentValidation trước khi gọi Handler[cite: 1]
|   |   |
|   |   +---Exceptions                                    # Lớp ngoại lệ đặc thù ở tầng ứng dụng[cite: 1]
|   |   |       AppValidationException.cs                 # Đóng gói danh sách lỗi vi phạm dữ liệu từ ValidationBehavior[cite: 1]
|   |   |       BusinessException.cs                      # Ngoại lệ báo lỗi vi phạm quy tắc logic xử lý use case của hệ thống[cite: 1]
|   |   |
|   |   \---Models                                        # Chuẩn hóa cấu trúc dữ liệu trả về client[cite: 1]
|   |       |   ApiError.cs                               # Mô tả cấu trúc từng trường lỗi (Property, Message)[cite: 1]
|   |       |   ApiResponse.cs                            # Định dạng payload JSON chuẩn (Success, Data, Message, Errors, StatusCode)[cite: 1]
|   |       |
|   |       \---Pagination                                # Các model phục vụ chia trang danh sách dữ liệu[cite: 1]
|   |               CursorPagination.cs                   # Mô hình phân trang theo con trỏ (Cursor-based) cho tập dữ liệu lớn[cite: 1]
|   |               IPagination.cs                        # Interface hợp đồng chung cho các dạng phân trang[cite: 1]
|   |               PagePagination.cs                     # Mô hình phân trang theo số trang truyền thống (Offset-based)[cite: 1]
|   |
|   +---DTOs                                              # Data Transfer Objects chia sẻ giữa các tầng[cite: 1]
|   |       UserDtos.cs                                   # Định nghĩa các Record/Class truyền dữ liệu người dùng (UserResponse, UserDto)
|   |
|   +---Interfaces                                        # Bản thiết kế hợp đồng trừu tượng (Contracts)[cite: 1]
|   |       IApplicationDbContext.cs                      # Giao diện trừu tượng hóa DbContext giúp Handler thao tác dữ liệu mà không phụ thuộc EF Core[cite: 1]
|   |       ICurrentUserService.cs                        # Hợp đồng lấy định danh (Id, UserName) của người dùng đang gửi request
|   |       IJwtTokenGenerator.cs                         # Hợp đồng phát sinh chuỗi JWT token cho người dùng đăng nhập[cite: 1]
|   |
|   +---Modules                                           # Tổ chức use cases theo từng nghiệp vụ chức năng (Feature Folders)[cite: 1]
|   |   +---Auth                                          # Module chức năng đăng nhập, phân quyền[cite: 1]
|   |   |       LoginCommand.cs                           # Dữ liệu yêu cầu đăng nhập (Email/UserName, Password)[cite: 1]
|   |   |       LoginCommandHandler.cs                    # Logic kiểm tra mật khẩu, tạo token và trả về kết quả đăng nhập[cite: 1]
|   |   |       LoginCommandValidator.cs                  # Quy tắc kiểm tra định dạng email và mật khẩu không rỗng của LoginCommand[cite: 1]
|   |   |
|   |   \---Users                                         # Module quản lý tài khoản người dùng[cite: 1]
|   |       +---Commands                                  # Các tác vụ làm thay đổi trạng thái dữ liệu (Write operations)[cite: 1]
|   |       |   +---CreateUser                            # Thư mục use case tạo mới người dùng[cite: 1]
|   |       |   |       CreateUserCommand.cs              # Dữ liệu gửi lên để tạo người dùng (họ tên, username, password, role)[cite: 1]
|   |       |   |       CreateUserCommandHandler.cs       # Logic mã hóa mật khẩu, tạo thực thể User và lưu vào cơ sở dữ liệu[cite: 1]
|   |       |   |       CreateUserCommandValidator.cs     # Xác thực định dạng username, độ dài tên, role hợp lệ của lệnh tạo[cite: 1]
|   |       |   |
|   |       |   +---DeleteUser                            # Thư mục use case xóa người dùng[cite: 1]
|   |       |   |       DeleteUserCommand.cs              # Chứa UserId của người dùng cần xóa[cite: 1]
|   |       |   |       DeleteUserCommandHandler.cs       # Logic tìm kiếm và đánh dấu xóa (soft-delete hoặc hard-delete)[cite: 1]
|   |       |   |
|   |       |   \---UpdateUser                            # Thư mục use case cập nhật thông tin người dùng[cite: 1]
|   |       |           UpdateUserCommand.cs              # Dữ liệu cập nhật mới của tài khoản[cite: 1]
|   |       |           UpdateUserCommandHandler.cs       # Logic cập nhật trường dữ liệu vào DB[cite: 1]
|   |       |           UpdateUserCommandValidator.cs     # Xác thực dữ liệu đầu vào khi thực hiện cập nhật[cite: 1]
|   |       |
|   |       \---Queries                                   # Các tác vụ truy vấn dữ liệu chỉ đọc (Read operations)[cite: 1]
|   |               GetAllUsersQuery.cs                   # Truy vấn và Handler lấy danh sách tất cả người dùng (AsNoTracking)[cite: 1]
|   |               GetUserByIdQuery.cs                   # Truy vấn và Handler tìm kiếm chi tiết người dùng theo Id[cite: 1]
|   |
+---OmniDesk.Domain                                       # [Tầng Lõi Nghiệp Vụ] Độc lập hoàn toàn, không phụ thuộc bất kỳ thư viện ngoài nào[cite: 1]
|   |   OmniDesk.Domain.csproj                            # File cấu hình project Domain thuần C#[cite: 1]
|   |
|   +---Common                                            # Các lớp cơ sở nền tảng cho Domain Entities[cite: 1]
|   |       BaseAuditableEntity.cs                        # Kế thừa BaseEntity, bổ sung các trường theo dõi kiểm toán (CreatedAt, CreatedBy, UpdatedAt, LastModifiedBy)[cite: 1]
|   |       BaseEntity.cs                                 # Thực thể cơ sở: Chứa khóa chính Id (Guid) và danh sách Domain Events[cite: 1]
|   |
|   +---Entities                                          # Chứa các thực thể mô hình hóa dữ liệu cốt lõi[cite: 1]
|   |       User.cs                                       # Thực thể tài khoản người dùng kế thừa BaseAuditableEntity[cite: 1]
|   |
|   +---Enums                                             # Các kiểu liệt kê dữ liệu hệ thống[cite: 1]
|   |       UserRole.cs                                   # Liệt kê các vai trò trong hệ thống (Admin, User, Manager...)[cite: 1]
|   |
|   +---Exceptions                                        # Lỗi bất biến vi phạm quy tắc tầng Domain[cite: 1]
|   |       DomainException.cs                            # Ngoại lệ ném ra khi vi phạm quy tắc cốt lõi của Entity/Domain[cite: 1]
|   |
\---OmniDesk.Infrastructure                               # [Tầng Hạ Tầng] Hiện thực hóa kỹ thuật, kết nối Database và thư viện ngoài[cite: 1]
    |   OmniDesk.Infrastructure.csproj                    # Quản lý thư viện EF Core, Npgsql/SqlServer, BCrypt[cite: 1]
    |
    +---Data                                              # Cấu hình và thao tác lưu trữ dữ liệu qua Entity Framework Core[cite: 1]
    |   |   DbInitializer.cs                              # Logic khởi tạo dữ liệu mẫu ban đầu (Seed data: tài khoản Admin mặc định)
    |   |
    |   +---Configurations                                # Cấu hình ràng buộc bảng qua Fluent API[cite: 1]
    |   |       UserConfiguration.cs                      # Cấu hình chi tiết bảng Users (Index, độ dài cột, kiểu dữ liệu, quan hệ)[cite: 1]
    |   |
    |   \---Context
    |           AppDbContext.cs                           # Triển khai DbContext thực tế: Quản lý DbSet và tự động gán giá trị kiểm toán trong SaveChangesAsync[cite: 1]
    |
    +---Migrations                                        # Quản lý các phiên bản cấu trúc cơ sở dữ liệu của EF Core[cite: 1]
    |       20261008104332_AddUserTable.cs                # Mã lệnh tạo bảng Users và các cột tương ứng khi migrate
    |       20261008104332_AddUserTable.Designer.cs       # Metadata mô tả migration của EF Core
    |       AppDbContextModelSnapshot.cs                  # Ảnh chụp cấu trúc toàn bộ database hiện tại để EF Core so sánh thay đổi
    |
    \---Services                                          # Triển khai các dịch vụ kỹ thuật hạ tầng[cite: 1]
            JwtTokenGenerator.cs                          # Triển khai IJwtTokenGenerator: Sinh chuỗi mã hóa JWT Bearer Token[cite: 1]
