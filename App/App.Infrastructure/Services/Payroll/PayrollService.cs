using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Models.Dto;
using App.Infrastructure.Data;
using App.Infrastructure.Services.Masters;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Infrastructure.Services.Payroll
{
    public class PayrollService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblOvertimeCalculation> _overtimeCalculationRepo;        
        private readonly IGenericRepository<TblEmployeeAllowance> _allowanceRepo;
        private readonly IGenericRepository<TblLeaveRequest> _LeaveRequestRepo;
        private readonly IGenericRepository<TblAttendance> _attendanceRepo;
        private readonly IGenericRepository<TblOvertime> _overtimeRepo;
        private readonly IGenericRepository<TblPayrollResult> _payrollResultRepo;
        private readonly IGenericRepository<TblPayrollRun> _payrollRunRepo;
        private readonly IGenericRepository<TblEmployeePayroll> _employeePayrollRepo;
        private readonly IGenericRepository<TblBpjskesehatanConfig> _BpjskesehatantRepo;
        private readonly IGenericRepository<TblBpjsketenagakerjaanConfig> _BpjsketenagakerjaanRepo;
        private readonly IGenericRepository<TblBiayaJabatanConfig> _BiayaJabatanRepo;
        private readonly IGenericRepository<TblPayrollPolicy> _payrollPolicyRepo;

        //var bpjsKes = await _db.tbl_BPJSKesehatanConfig
        //       .OrderByDescending(x => x.EffectiveDate)
        //       .FirstAsync();

        //var bpjsTk = await _db.tbl_BPJSKetenagakerjaanConfig
        //    .OrderByDescending(x => x.EffectiveDate)
        //    .FirstAsync();

        //var biayaCfg = await _db.tbl_BiayaJabatanConfig
        //    .OrderByDescending(x => x.EffectiveDate)
        //    .FirstAsync();
        private readonly IGenericRepository<TblEmployee> _employeeRepo;

        private readonly IGenericRepository<TblPtkp> _ptkpRepo;
        private readonly ILogger<ShiftPatternService> _logger;
        private readonly IContextService _context;
        public PayrollService(
            IGenericRepository<TblPtkp> ptkpRepo,
            IGenericRepository<TblOvertimeCalculation> overtimeCalculationRepo,
        IGenericRepository<TblEmployeeAllowance> allowanceRepo,
            IGenericRepository<TblLeaveRequest> LeaveRequestRepo,
            IGenericRepository<TblAttendance> attendanceRepo,
        IGenericRepository<TblOvertime> overtimeRepo,
             IGenericRepository<TblPayrollPolicy> payrollPolicyRepo,
                IGenericRepository<TblEmployee> empRepo,
                IGenericRepository<TblEmployeePayroll> employeePayrollRepo,
            ILogger<ShiftPatternService> logger,
            IGenericRepository<TblPayrollResult> payrollResultRepo,
            IContextService context,
            IGenericRepository<TblBpjskesehatanConfig> BpjskesehatantRepo,
            IGenericRepository<TblBpjsketenagakerjaanConfig> BpjsketenagakerjaanRepo,
            IGenericRepository<TblBiayaJabatanConfig> BiayaJabatanRepo,
            IGenericRepository<TblPayrollRun> payrollRunRepo,
            IMapper mapper)
        {
             _ptkpRepo= ptkpRepo;
            _overtimeCalculationRepo = overtimeCalculationRepo;
            _allowanceRepo = allowanceRepo;
            _LeaveRequestRepo = LeaveRequestRepo;
            _attendanceRepo = attendanceRepo;
            _overtimeRepo = overtimeRepo;
            _payrollPolicyRepo = payrollPolicyRepo;
            _mapper = mapper;
            _logger = logger;
            _payrollResultRepo = payrollResultRepo;
            _mapper = mapper;
            _context = context;
            _BpjskesehatantRepo = BpjskesehatantRepo;
            _BpjsketenagakerjaanRepo = BpjsketenagakerjaanRepo;
            _BiayaJabatanRepo = BiayaJabatanRepo;
            _payrollRunRepo = payrollRunRepo;
            _employeePayrollRepo = employeePayrollRepo;
            _employeeRepo = empRepo;

        }


        public async Task<PayrollResultDto> CalculateAsync(
            PayrollEmployeeDto input,
            int year,
            int month)
        {
            decimal gross =
                input.BasicSalary +
                input.Allowance +
                input.Overtime;

            // =========================
            // 1. Load Config
            // =========================


            var bpjsKesehatanList = await _BpjskesehatantRepo.GetListAsync();
            var bpjsKes = bpjsKesehatanList.OrderByDescending(x => x.EffectiveDate).FirstOrDefault();

            var bpjsTkList = await _BpjsketenagakerjaanRepo.GetListAsync();
            var bpjsTk = bpjsTkList.OrderByDescending(x => x.EffectiveDate).FirstOrDefault();

            var biayaJabatanList = await _BiayaJabatanRepo.GetListAsync();
            var biayaCfg = biayaJabatanList.OrderByDescending(x => x.EffectiveDate).FirstOrDefault();
            // =========================
            // 2. BPJS Kesehatan
            // =========================
            decimal baseSalary = Math.Min(gross, bpjsKes.SalaryCap);
            decimal bpjsKesehatan = baseSalary * bpjsKes.EmployeeRate;

            // =========================
            // 3. BPJS TK
            // =========================
            decimal jht = gross * bpjsTk.JhtEmployeeRate.Value;
            decimal jp = gross * bpjsTk.JpEmployeeRate.Value;

            decimal bpjsTkTotal = jht + jp;

            // =========================
            // 4. Biaya Jabatan
            // =========================
            decimal biayaJabatan =
                Math.Min(gross * biayaCfg.Rate.Value, biayaCfg.MaxMonthly.Value);

            // =========================
            // 5. Tax Base (Annual)
            // =========================
            decimal deductionMonthly =
                bpjsKesehatan +
                bpjsTkTotal +
                biayaJabatan;

            decimal netMonthlyBeforeTax =
                gross - deductionMonthly;

            decimal netAnnual =
                netMonthlyBeforeTax * 12;

            // =========================
            // 6. PKP
            // =========================
            decimal ptkp = await GetPTKPAsync(input.EmployeeId);

            decimal pkp = Math.Max(0, netAnnual - ptkp);

            // =========================
            // 7. PPh21
            // =========================
            decimal annualTax = CalculatePPh21(pkp);

            decimal monthlyTax = annualTax / 12;

            // =========================
            // 8. Net Salary
            // =========================
            decimal netSalary =
                netMonthlyBeforeTax - monthlyTax - input.AttendanceDeduction;

            return new PayrollResultDto
            {
                GrossSalary = gross,
                Bpjskesehatan = bpjsKesehatan,
                Bpjsketenagakerjaan = bpjsTkTotal,
                BiayaJabatan = biayaJabatan,
                Pph21 = monthlyTax,
                NetSalary = netSalary
            };
        }

        /// <summary>
        /// PTKP stands for Penghasilan Tidak Kena Pajak.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        private async Task<decimal> GetPTKPAsync(int employeeId)
        {
            var emp = await _employeeRepo.GetFirstOrDefaultAsync(t => t.EmployeeId == employeeId);
            var ptkp = await _ptkpRepo.GetFirstOrDefaultAsync(t => t.Ptkpcode == emp.Ptkpcode);
            //return emp?.PTKPCode switch
            //{
            //    "TK0" => 54_000_000,
            //    "K0" => 58_500_000,
            //    "K1" => 63_000_000,
            //    "K2" => 67_500_000,
            //    "K3" => 72_000_000,
            //    _ => 54_000_000
            //};

            return ptkp?.NominalYearly ?? 54000000;
        }


        /*
🏥 BPJS Kesehatan
var bpjsKes = configKes;

decimal baseSalary = Math.Min(salary, bpjsKes.SalaryCap);

decimal employee = baseSalary * bpjsKes.EmployeeRate;
decimal company  = baseSalary * bpjsKes.CompanyRate;
🧾 BPJS Ketenagakerjaan
var bpjsTk = configTk;

decimal jht = salary * bpjsTk.JHT_EmployeeRate;
decimal jp  = salary * bpjsTk.JP_EmployeeRate;

decimal employeeTotal = jht + jp;
💰 Biaya Jabatan
var biaya = configBiaya;

decimal monthly = salary * biaya.Rate;

decimal cappedMonthly = Math.Min(monthly, biaya.MaxMonthly);
         */
        public decimal CalculatePPh21(decimal pkpAnnual)
        {
            decimal tax = 0;

            if (pkpAnnual <= 0)
                return 0;

            // Layer 1: up to 60,000,000 → 5%
            if (pkpAnnual <= 60_000_000)
            {
                tax = pkpAnnual * 0.05m;
                return tax;
            }

            // Layer 2: 60M - 250M
            if (pkpAnnual <= 250_000_000)
            {
                tax =
                    (60_000_000 * 0.05m) +
                    ((pkpAnnual - 60_000_000) * 0.15m);

                return tax;
            }

            // Layer 3: 250M - 500M
            if (pkpAnnual <= 500_000_000)
            {
                tax =
                    (60_000_000 * 0.05m) +
                    (190_000_000 * 0.15m) +
                    ((pkpAnnual - 250_000_000) * 0.25m);

                return tax;
            }

            // Layer 4: 500M - 5B
            if (pkpAnnual <= 5_000_000_000)
            {
                tax =
                    (60_000_000 * 0.05m) +
                    (190_000_000 * 0.15m) +
                    (250_000_000 * 0.25m) +
                    ((pkpAnnual - 500_000_000) * 0.30m);

                return tax;
            }

            // Layer 5: above 5B
            tax =
                (60_000_000 * 0.05m) +
                (190_000_000 * 0.15m) +
                (250_000_000 * 0.25m) +
                (4_500_000_000 * 0.30m) +
                ((pkpAnnual - 5_000_000_000) * 0.35m);

            return tax;
        }
        public PayrollResultDto CalculatePayroll(PayrollEmployeeDto input)
        {
            decimal salary = input.BasicSalary;

            // BPJS
            decimal bpjsKes = Math.Min(salary, 12000000) * 0.01m;

            decimal bpjsTK =
                (salary * 0.02m) +
                (salary * 0.01m);

            decimal totalBpjs = bpjsKes + bpjsTK;

            // Biaya Jabatan
            decimal biayaJabatan = Math.Min(salary * 0.05m, 500000m);

            // Net annual
            decimal netAnnual =
                (salary - totalBpjs - biayaJabatan) * 12;

            decimal pkp = Math.Max(0, netAnnual - input.PTKP);

            // Tax
            decimal tax = CalculatePPh21(pkp);

            decimal monthlyTax = tax / 12;

            decimal netSalary =
                salary - totalBpjs - monthlyTax;

            return new PayrollResultDto
            {
                GrossSalary = salary,
                Bpjskesehatan = bpjsKes,
                Bpjsketenagakerjaan = bpjsTK,
                Pph21 = monthlyTax,
                NetSalary = netSalary
            };
        }

        public async Task RunPayroll(int year, int month)
        {
            var run = new TblPayrollRun
            {
                PeriodYear = year,
                PeriodMonth = month,
                Status = "DRAFT"
            };

            await _payrollRunRepo.AddAsync(run);

            var employees = await _employeeRepo.GetListAsync();

            foreach (var emp in employees)
            {
                var input = await BuildEmployeePayrollInput(emp, year, month);

                var result = await CalculateAsync(input, year, month);

                await _payrollResultRepo.AddAsync(new TblPayrollResult
                {
                    EmployeeId = emp.EmployeeId,
                    GrossSalary = result.GrossSalary,
                    Bpjskesehatan = result.Bpjskesehatan,
                    Bpjsketenagakerjaan = result.Bpjsketenagakerjaan,
                    BiayaJabatan = result.BiayaJabatan,
                    Pph21 = result.Pph21,
                    NetSalary = result.NetSalary
                });
            }

            //await _db.SaveChangesAsync();
        }

        public async Task LockPayroll(int payrollRunId)
        {
            var run = await _payrollRunRepo.FindAsync(t => t.Id == payrollRunId);

            run.Status = "LOCKED";

            await _payrollRunRepo.UpdateAsync(run);
        }


        //    private async Task<PayrollEmployeeDto> BuildEmployeePayrollInput(
        //TblEmployee employee,
        //int year,
        //int month)
        //    {
        //        var overtimeList = await _overtimeRepo.GetListAsync(x =>
        //                    x.EmployeeId == employee.EmployeeId &&
        //                    x.OvertimeDate.Year == year &&
        //                    x.OvertimeDate.Month == month &&
        //                    x.Status == "APPROVED");
        //        decimal overtime = overtimeList.Select(t=> t.amou).Sum(x => (decimal?)x.Amount) ?? 0;

        //        var allowanceList = await _allowanceRepo.GetListAsync(x =>
        //                x.EmployeeId == employee.EmployeeId &&
        //                x.IsActive);

        //        decimal allowance = allowanceList.Select(t=> t.Amount).Sum();
        //        decimal attendanceDeduction = await CalculateAttendanceDeduction(
        //            employee.EmployeeId,
        //            year,
        //            month);

        //        return new PayrollEmployeeDto
        //        {
        //            EmployeeId = employee.EmployeeId,
        //            BasicSalary = employee.BasicSalary,

        //            Allowance = allowance,

        //            Overtime = overtime,

        //            AttendanceDeduction = attendanceDeduction
        //        };
        //    }

        private async Task<PayrollEmployeeDto> BuildEmployeePayrollInput(
    TblEmployee employee,
    int year,
    int month)
        {
            // =====================================
            // Approved Overtime
            // =====================================

            var overtimeList = await _overtimeRepo.GetListAsync(x =>
                x.EmployeeId == employee.EmployeeId &&
                x.OvertimeDate.Year == year &&
                x.OvertimeDate.Month == month &&
                x.Status == "APPROVED");

            var overtimeIds = overtimeList
                .Select(x => x.Id)
                .ToList();

            decimal overtimeHours = overtimeList
                .Sum(x => x.TotalHours);

            decimal overtimeAmount = 0;

            if (overtimeIds.Any())
            {
                var overtimeCalculations =
                    await _overtimeCalculationRepo.GetListAsync(x =>
                        overtimeIds.Contains(x.OvertimeId));

                overtimeAmount = overtimeCalculations
                    .Sum(x => x.TotalAmount);
            }

            // =====================================
            // Allowances
            // =====================================

            var allowanceList = await _allowanceRepo.GetListAsync(x =>
                x.EmployeeId == employee.EmployeeId &&
                x.IsActive);

            decimal allowance = allowanceList
                .Sum(x => (decimal?)x.Amount) ?? 0;

            // =====================================
            // Attendance Deduction
            // =====================================

            decimal attendanceDeduction =
                await CalculateAttendanceDeduction(
                    employee.EmployeeId,
                    year,
                    month);

            // =====================================
            // Result
            // =====================================

            return new PayrollEmployeeDto
            {
                EmployeeId = employee.EmployeeId,

                BasicSalary = employee.BasicSalary,

                Allowance = allowance,

                //OvertimeHours = overtimeHours,
                Overtime = overtimeAmount,

                AttendanceDeduction = attendanceDeduction
            };
        }
        private async Task<decimal> CalculateAttendanceDeduction(
    int employeeId,
    int year,
    int month)
        {
            //var employeePayroll = await _db.tbl_EmployeePayroll
            //    .Where(x =>
            //        x.EmployeeId == employeeId &&
            //        x.IsActive)
            //    .OrderByDescending(x => x.EffectiveDate)
            //    .FirstOrDefaultAsync();

            var employeePayrollList = await _employeePayrollRepo
             .GetListAsync(x =>
                 x.EmployeeId == employeeId &&
                 x.IsActive);

            var employeePayroll = employeePayrollList.OrderByDescending(x => x.EffectiveDate).FirstOrDefault();
            if (employeePayroll == null)
                return 0;

            var policyList = await _payrollPolicyRepo.GetListAsync(x => x.IsActive);

            var policy = policyList.OrderByDescending(x => x.EffectiveDate).FirstOrDefault();

            decimal monthlySalary = employeePayroll.BasicSalary;

            decimal dailySalary =
                monthlySalary / policy.WorkingDaysPerMonth;

            var attendances = await _attendanceRepo.GetListAsync(x =>
                    x.EmployeeId == employeeId &&
                    x.AttendanceDate.Year == year &&
                    x.AttendanceDate.Month == month);

            //------------------------------------
            // ABSENT
            //------------------------------------

            int absentDays = attendances
                .Count(x => x.AttendanceStatus == AttendanceStatus.Absent.ToString());

            decimal absentDeduction =
                absentDays * dailySalary;

            //------------------------------------
            // LATE
            //------------------------------------

            decimal lateDeduction = 0;

            if (policy.DeductLate)
            {
                int totalLateMinutes =
                    attendances.Sum(x => x.LateMinutes);

                lateDeduction =
                    totalLateMinutes *
                    policy.LatePenaltyPerMinute;
            }

            //------------------------------------
            // EARLY OUT
            //------------------------------------

            decimal earlyOutDeduction = 0;

            if (policy.DeductEarlyOut)
            {
                int totalEarlyMinutes =
                    attendances.Sum(x => x.EarlyOutMinutes);

                earlyOutDeduction =
                    totalEarlyMinutes *
                    policy.EarlyOutPenaltyPerMinute;
            }

            //------------------------------------
            // UNPAID LEAVE
            //------------------------------------

            int unpaidLeaveDays = 0;
            //await _LeaveRequestRepo.GetListAsync(x =>
            //    x.EmployeeId == employeeId &&
            //    x.Status == "APPROVED")
            //.Join(
            //    _db.tbl_LeaveType,
            //    lr => lr.LeaveTypeId,
            //    lt => lt.Id,
            //    (lr, lt) => new
            //    {
            //        lr.StartDate,
            //        lr.EndDate,
            //        lt.IsPaid,
            //        lr.TotalDays
            //    })
            //.Where(x =>
            //    !x.IsPaid &&
            //    x.StartDate.Year == year &&
            //    x.StartDate.Month == month)
            //.SumAsync(x => (int?)x.TotalDays) ?? 0;

            decimal unpaidLeaveDeduction =
                unpaidLeaveDays * dailySalary;

            //------------------------------------
            // TOTAL
            //------------------------------------

            return
                absentDeduction +
                lateDeduction +
                earlyOutDeduction +
                unpaidLeaveDeduction;
        }
    }
}
