using DataAccess.Models;

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.services
{
}
//    public class GonbadTalaService
//    {
//        private readonly IChildRepository _childRepository;
//        private readonly IVisitRepository _visitRepository;

//        // تزریق وابستگی از طریق سازنده (Constructor Injection)
//        public GonbadTalaService(IChildRepository childRepository, IVisitRepository visitRepository)
//        {
//            _childRepository = childRepository;
//            _visitRepository = visitRepository;
//        }
//        /// <summary>
//        /// ثبت‌نام اولیه
//        /// </summary>
//        public async Task<child1> RegisterNewChildAsync(string firstName, string lastName, string phoneNumber, int age)
//        {
//            // 1. بررسی تکراری بودن
//            bool isExisting = await _childRepository.ExistsByPhoneNumberAsync(phoneNumber);
//            if (isExisting)
//            {
//                throw new Exception("این شماره تماس قبلاً ثبت شده است.");
//            }

//            // 2. ایجاد و ذخیره
//            var newChild = new child1
//            {
//                FirstName = firstName,
//                LastName = lastName,
//                PhoneNumber = phoneNumber,
//                Age = age
//            };

//            await _childRepository.AddAsync(newChild);
//            await _childRepository.SaveChangesAsync();

//            return newChild;
//        }

//        /// <summary>
//        /// ثبت ورود روزانه
//        /// </summary>
//        public async Task<Visit> CheckInChildAsync(string phoneNumber, DateTime checkInTime)
//        {
//            // 1. پیدا کردن بچه
//            var child = await _childRepository.GetByPhoneNumberAsync(phoneNumber);
//            if (child == null)
//            {
//                throw new Exception("بچه‌ای با این شماره تماس یافت نشد.");
//            }

//            // 2. بررسی وضعیت فعال قبلی
//            bool hasActiveVisit = await _visitRepository.HasActiveVisitAsync(child.Id);
//            if (hasActiveVisit)
//            {
//                throw new Exception("این بچه در حال حاضر در خانه بازی حضور دارد.");
//            }

//            // 3. ایجاد بازدید جدید
//            var newVisit = new Visit
//            {
//                ChildId = child.Id,
//                VisitDate = checkInTime.Date,
//                CheckInTime = checkInTime,
//                Status = "Active"
//            };

//            await _visitRepository.AddAsync(newVisit);
//            await _visitRepository.SaveChangesAsync();

//            return newVisit;
//        }

//        /// <summary>
//        /// ثبت خروج
//        /// </summary>
//        public async Task<Visit> CheckOutChildAsync(int visitId, DateTime checkOutTime)
//        {
//            var visit = await _visitRepository.GetByIdAsync(visitId);
//            if (visit == null) throw new Exception("رکورد یافت نشد.");
//            if (visit.Status != "Active") throw new Exception("بازدید بسته شده است.");

//            visit.CheckOutTime = checkOutTime;
//            visit.Status = "Completed";

//            await _visitRepository.UpdateAsync(visit);
//            await _visitRepository.SaveChangesAsync();

//            return visit;
//        }
//    }
//}
