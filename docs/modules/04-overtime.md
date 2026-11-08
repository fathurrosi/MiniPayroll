---
module: Overtime Management
code: M002004 / OT01
category: Core Operation & Master
last_updated: 2026-11-08
related_files:
  - App/App.UI.Web/Controllers/OvertimeTypeController.cs
  - App/App.UI.Web/Views/Overtime/Index.cshtml
  - App/App.UI.Web/Views/OvertimeType/Index.cshtml
  - App/App.Application/Interfaces/Services/Masters/IOvertimeTypeService.cs
  - App/App.Infrastructure/Services/Masters/OvertimeTypeService.cs
  - App/App.Infrastructure/Services/Payroll/OvertimeService.cs
  - App/App.Infrastructure/Services/Transactions/OvertimeService.cs
  - App/App.Domain/Entities/TblOvertime.cs
  - App/App.Domain/Entities/TblOvertimeType.cs
  - App/App.Domain/Entities/TblOvertimeCalculation.cs
  - App/App.Domain/Entities/TblOvertimeRate.cs
---

# ⏱️ Modul: Overtime Management (Lembur Karyawan)

## 1. Ringkasan & Tujuan
Modul **Overtime Management** mengelola pencatatan jam kerja lembur karyawan, penentuan tarif lembur berdasarkan regulasi ketenagakerjaan (Depnaker), dan kalkulasi otomatis nominal upah lembur yang akan dibayarkan melalui modul Payroll.

---

## 2. Struktur Komponen & File Terkait

```text
Overtime Module
├── UI (Views)
│   ├── Views/Overtime/Index.cshtml             # Riwayat & rekapitulasi lembur
│   └── Views/OvertimeType/Index.cshtml         # Master tipe lembur (hari kerja vs libur)
├── Controllers
│   └── Controllers/OvertimeTypeController.cs   # Kelola master jenis lembur
├── Services
│   ├── Application/Interfaces/Services/Masters/IOvertimeTypeService.cs
│   ├── Infrastructure/Services/Masters/OvertimeTypeService.cs
│   └── Infrastructure/Services/Payroll/OvertimeService.cs      # Mesin kalkulasi rate lembur
└── Domain (Entities & Models)
    ├── Entities/TblOvertime.cs                 # Catatan transaksi lembur
    ├── Entities/TblOvertimeType.cs             # Jenis lembur (Weekday, Weekend, Holiday)
    ├── Entities/TblOvertimeRate.cs             # Pengali upah per jam lembur (1.5x, 2x, 3x, 4x)
    └── Entities/TblOvertimeCalculation.cs      # Hasil hitung nominal upah lembur
```

---

## 3. Rumus & Aturan Regulasi Lembur (Kemenaker)

Upah lembur per jam dihitung dengan formula standar:

$$
\text{Upah Lembur per Jam} = \frac{1}{173} \times \text{Upah Sebulan (Gaji Pokok + Tunjangan Tetap)}
$$

### Skema Pengali Jam Lembur:
1. **Hari Kerja Normal**:
   - Jam pertama: **$1.5 \times$** upah per jam.
   - Jam kedua dan seterusnya: **$2.0 \times$** upah per jam.
2. **Hari Libur Resmi / Istirahat Mingguan**:
   - Jam ke-1 s.d ke-7 / ke-8: **$2.0 \times$** upah per jam.
   - Jam ke-8 / ke-9: **$3.0 \times$** upah per jam.
   - Jam ke-10 dan seterusnya: **$4.0 \times$** upah per jam.

---

## 4. Alur Integrasi Lembur ke Penggajian

```mermaid
flowchart LR
    Att[Log Presensi / Jam Lembur] --> Trans[TblOvertime Transaction]
    Trans --> Approval{Approval Atasan}
    Approval -->|Approved| Calc[OvertimeService Kalkulasi]
    Calc --> Rate[Lookup TblOvertimeRate]
    Rate --> Result[TblOvertimeCalculation]
    Result --> PayrollEngine[PayrollService: Tambahkan ke Penghasilan Bruto]
```

---

## 5. Endpoints Utama

| Controller | Route | Method | Keterangan |
| :--- | :--- | :---: | :--- |
| `OvertimeTypeController` | `/OvertimeType` | `GET` | Menampilkan tabel master jenis lembur. |
| `OvertimeTypeController` | `/OvertimeType/GetList` | `POST` | DataTables endpoint master tipe lembur. |
| `OvertimeTypeController` | `/OvertimeType/Save` | `POST` | Simpan/ubah master tipe lembur. |
| `OvertimeTypeController` | `/OvertimeType/Delete` | `POST` | Hapus master tipe lembur. |
| `Views/Overtime/Index` | `/Overtime` | `GET` | Dashboard monitoring transaksi lembur karyawan. |

