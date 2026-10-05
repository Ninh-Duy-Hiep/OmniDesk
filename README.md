# OmniDesk - Backend Service

Dự án Backend cho hệ thống OmniDesk, được xây dựng trên nền tảng **ASP.NET Core** và **MySQL**, áp dụng mô hình kiến trúc **Onion Architecture / Clean Architecture** nhằm đảm bảo tính độc lập, dễ mở rộng và thuận tiện cho việc viết kiểm thử tự động.

---

## 📁 Cấu Trúc Thư Mục Dự Án

Cây thư mục hoàn chỉnh của Solution tương ứng với cấu trúc hiện tại[cite: 3]:

```text
OmniDesk/
├── OmniDesk.API/                                # [Lớp Presentation] Điểm tiếp nhận request từ Client, quản lý vòng đời HTTP[cite: 3]
│   ├── Controllers/                             # Tiếp nhận HTTP Request, gọi tầng Application và trả về HTTP Response[cite: 3]
│   ├── Extensions/                              # Các extension methods mở rộng cấu hình DI, CORS, Swagger, Auth[cite: 3]
│   ├── Middlewares/                             # Custom middlewares xử lý luồng Request (Exception handling, Logging)[cite: 3]
│   ├── Properties/                              # Cấu hình môi trường khởi chạy máy chủ phát triển (launchSettings.json)[cite: 3]
│   ├── appsettings.json                         # Cấu hình chuỗi kết nối MySQL và thông số môi trường
│   └── Program.cs                               # Composition Root: Khởi chạy app, cấu hình Pipeline và ráp nối DI container
│
├── OmniDesk.Application/                        # [Lớp Application] Điều phối Use Cases và chứa toàn bộ logic ứng dụng[cite: 3]
│   ├── Common/                                  # Thành phần dùng chung nội bộ tầng Application (Models phân trang, Exceptions)[cite: 3]
│   ├── DTOs/                                    # Data Transfer Objects: Đối tượng định dạng dữ liệu vào/ra cho các API[cite: 3]
│   ├── Interfaces/                              # Hợp đồng trừu tượng (Contracts) cho Repositories và các Service ngoại vi[cite: 3]
│   ├── Services/                                # Hiện thực hóa logic nghiệp vụ chính (Business / Application Services)[cite: 3]
│   └── Validators/                              # Bộ quy tắc kiểm tra tính hợp lệ của dữ liệu đầu vào (FluentValidation)[cite: 3]
│
├── OmniDesk.Domain/                             # [Lớp Domain Core] Lõi nghiệp vụ độc lập nhất, không phụ thuộc vào bất kỳ thư viện nào[cite: 3]
│   ├── Common/                                  # Lớp thực thể dùng chung (BaseEntity, BaseAuditableEntity chứa Id, thời gian tạo)[cite: 3]
│   ├── Entities/                                # Các đối tượng thực thể đại diện cho dữ liệu cốt lõi (User, Desk, Booking,...)[cite: 3]
│   ├── Enums/                                   # Định nghĩa các trạng thái dạng liệt kê (DeskStatus, BookingStatus,...)[cite: 3]
│   └── Exceptions/                              # Các ngoại lệ riêng biệt khi vi phạm quy tắc nghiệp vụ tầng Domain[cite: 3]
│
└── OmniDesk.Infrastructure/                     # [Lớp Infrastructure] Hiện thực hóa kỹ thuật, giao tiếp Database và bên thứ ba[cite: 3]
    ├── Data/                                    # Quản lý tầng truy xuất dữ liệu MySQL thông qua Entity Framework Core[cite: 3]
    │   ├── Configurations/                      # Cấu hình lược đồ bảng bằng Fluent API (Mapping thuộc tính, khóa chính, quan hệ)[cite: 3]
    │   ├── Context/                             # Quản lý DbContext kết nối cơ sở dữ liệu MySQL (AppDbContext)[cite: 3]
    │   └── Repositories/                        # Triển khai thực tế các interface truy vấn dữ liệu từ Application[cite: 3]
    └── Services/                                # Triển khai các dịch vụ hạ tầng thực tế (EmailSender, CloudStorage, Payment,...)[cite: 3]
