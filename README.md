# 💼 MiniPayroll - HR & Payroll Management System

Sistem manajemen Human Resource (HR), absensi karyawan, dan penggajian (Payroll) berbasis **ASP.NET Core (.NET 10)** dengan arsitektur **Clean Architecture**.

---

## 🏛️ Arsitektur Proyek

Solusi proyek dibagi menjadi 4 layer utama di dalam folder `App/`:

| Layer | Proyek | Deskripsi & Tanggung Jawab |
| :--- | :--- | :--- |
| **Domain** | `App.Domain` | Entities (EF Core), Enums, DTOs, Request/Response ViewModels. Bebas dependensi eksternal. |
| **Application** | `App.Application` | Abstraksi interface (`Services`, `Repositories`, `Procedures`), kontrak bisnis logic, dan helper security (`PasswordHasher`). |
| **Infrastructure** | `App.Infrastructure` | Implementasi konkret EF Core DbContext, Service logic, Dapper query, integrasi Database, JWT Service, dan Logging. |
| **Presentation (Web)** | `App.UI.Web` | ASP.NET Core MVC Controllers, Razor Views, static assets (JS/CSS/Vendor libraries), dan konfigurasi Dependency Injection (`Program.cs`). |

---

## 📚 Modul Sistem & Katalog Dokumentasi

Setiap modul didokumentasikan secara rinci di dalam direktori [`docs/modules/`](./docs/modules/). Klik tautan pada modul di bawah ini untuk melihat detail teknis, entitas, alur proses, dan daftar tugas pengembangan:

| No | Modul | Kode Menu | File Dokumentasi | Fokus Utama |
| :---: | :--- | :---: | :--- | :--- |
| 1 | **Attendance (Presensi)** | `M002001` | [01-attendance.md](./docs/modules/01-attendance.md) | Log kehadiran, clock in/out, filter data table, status kehadiran. |
| 2 | **Payroll & Payslip** | `M002` | [02-payroll-payslip.md](./docs/modules/02-payroll-payslip.md) | Payroll run engine, hitung take home pay, closing periode, slip gaji. |
| 3 | **Leave Management** | `M002005` | [03-leave-management.md](./docs/modules/03-leave-management.md) | Pengajuan cuti/izin, kuota cuti tahunan, dan persetujuan bertingkat. |
| 4 | **Overtime Management** | `M002004` | [04-overtime.md](./docs/modules/04-overtime.md) | Pengajuan lembur, master rate lembur (Depnaker), kalkulasi upah lembur. |
| 5 | **Shift & Schedule** | `M004 / SH` | [05-shift-schedule.md](./docs/modules/05-shift-schedule.md) | Jam kerja, pola shift (pattern), jadwal bulanan, dan hari libur nasional. |
| 6 | **Employee & Organization** | `M004 / EMP` | [06-employee-organization.md](./docs/modules/06-employee-organization.md) | Data induk karyawan, profil cabang, departemen, jabatan, dan struktur. |
| 7 | **Salary Components & Tax** | `M004 / SC` | [07-salary-components-tax.md](./docs/modules/07-salary-components-tax.md) | Komponen tunjangan/potongan, tarif BPJS Kes & TK, Biaya Jabatan, PPh 21 & PTKP. |
| 8 | **User & Access Security** | `M005 / ST` | [08-user-access-security.md](./docs/modules/08-user-access-security.md) | RBAC (Role-Based Access Control), hak akses menu, password hashing, JWT. |
| 9 | **Reports & Analytics** | `M006 / RP` | [09-reporting.md](./docs/modules/09-reporting.md) | Rekapitulasi absensi, cuti, lembur, ringkasan dan detail payroll per periode. |
| 10 | **Workflow & Approval** | `M003` | [10-workflow-approval.md](./docs/modules/10-workflow-approval.md) | Matriks alur pengesahan dokumen dan approval berjenjang multi-level. |

---

## 🗄️ Database & Skrip SQL

Skrip basis data SQL Server tersimpan pada folder [`Db/`](./Db/):
- `DB_Structure_Latest.sql`: Struktur tabel lengkap, stored procedures, dan triggers terbaru.
- `dbo.tbl_Menus.Table.sql`: Skrip master data navigasi menu dan sub-menu aplikasi.
- `dbo.tbl_Users.Table.sql`: Skrip data inisialisasi akun user default.

---

## 🚀 Panduan Memulai Cepat (Quickstart)

### 1. Kebutuhan Sistem
- [.NET 10 SDK](https://dotnet.microsoft.com/)
- SQL Server 2019 / 2022 atau Azure SQL Database
- Visual Studio 2022 / VS Code / Antigravity

### 2. Konfigurasi Database
Sesuaikan connection string pada file `App/App.UI.Web/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MANDE_03;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Menjalankan Aplikasi
```powershell
cd App/App.UI.Web
dotnet restore
dotnet build
dotnet run
```
Aplikasi dapat diakses melalui browser pada `http://localhost:5000` (atau port HTTPS yang dikonfigurasikan).

---

## 📝 Konvensi Dokumentasi (*Docs-as-Code*)
- Setiap perubahan fitur wajib memperbarui file dokumentasi modul terkait pada folder `docs/modules/`.
- Format penulisan mengikuti standar linter yang didefinisikan pada [`.markdownlint.json`](./.markdownlint.json).

