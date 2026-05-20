# 💰 MoneyApp — Hệ thống Quản lý Thu Chi Cá Nhân

Ứng dụng web giúp người dùng theo dõi thu nhập và chi tiêu cá nhân theo từng ví, danh mục và khoảng thời gian.

---

## 🚀 Tính năng

- Đăng ký, đăng nhập với xác thực JWT
- Quản lý nhiều ví (Wallet) độc lập
- Thêm, sửa, xóa giao dịch thu/chi — tự động cập nhật số dư ví
- Kiểm tra số dư trước khi cho phép chi tiêu
- Phân loại giao dịch theo danh mục (Category)
- Thống kê chi tiêu theo ngày/tháng

---

## 🛠️ Công nghệ sử dụng

**Back-end:**
- ASP.NET Core (C#)
- Entity Framework Core + LINQ
- SQL Server
- JWT Authentication, BCrypt

**Front-end:**
- Vue.js 3
- Vuetify 3
- Axios

---

## 📁 Cấu trúc dự án

```
MoneyApp/
├── MoneyApp.Server/          # Back-end ASP.NET Core
│   ├── Controllers/          # API endpoints (User, Wallet, Transaction, Category)
│   ├── Models/               # Entity models
│   ├── DTOs/                 # Data Transfer Objects
│   ├── Data/                 # DbContext
│   └── Program.cs            # Cấu hình middleware, DI
│
└── moneyapp.client/          # Front-end Vue.js
    ├── src/
    │   ├── components/       # Home, Transaction, Login, Register...
    │   ├── router/           # Vue Router
    │   └── utils/            # axios instance, API helpers
    └── package.json
```

---

## ⚙️ Hướng dẫn chạy dự án

### Yêu cầu
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) >= 16
- SQL Server

### 1. Chạy Back-end

```bash
cd MoneyApp.Server

# Cập nhật connection string trong appsettings.json
# "DefaultConnection": "Server=...;Database=MoneyApp;..."

# Tạo database
dotnet ef database update

# Chạy server
dotnet run --launch-profile https
# Server chạy tại: https://localhost:7029
```

### 2. Chạy Front-end

```bash
cd moneyapp.client

npm install
npm run serve
# Ứng dụng chạy tại: http://localhost:8080
```

---

## 📡 API Endpoints

| Method | Endpoint | Mô tả | Auth |
|--------|----------|-------|------|
| POST | `/User/login` | Đăng nhập | ❌ |
| POST | `/User/register` | Đăng ký | ❌ |
| GET | `/User` | Thông tin user | ✅ |
| GET | `/wallet` | Danh sách ví | ✅ |
| POST | `/wallet` | Tạo ví mới | ✅ |
| GET | `/Transaction` | Danh sách giao dịch | ✅ |
| POST | `/Transaction` | Thêm giao dịch | ✅ |
| PUT | `/Transaction/{id}` | Sửa giao dịch | ✅ |
| DELETE | `/Transaction/{id}` | Xóa giao dịch | ✅ |
| GET | `/Category` | Danh sách danh mục | ✅ |

> ✅ Yêu cầu Bearer Token trong header `Authorization`

---

## 🔐 Xác thực

Dự án sử dụng **JWT (JSON Web Token)**:
1. Đăng nhập thành công → nhận Token
2. Mỗi request tiếp theo gửi kèm header: `Authorization: Bearer <token>`
3. Token hết hạn → tự động chuyển về trang đăng nhập

---

## 📝 Ghi chú

- Đây là dự án cá nhân phục vụ mục đích học tập
- Chưa tối ưu cho môi trường production (CORS đang `AllowAnyOrigin`, JWT secret nên lưu trong biến môi trường)