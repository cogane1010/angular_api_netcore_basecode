using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Data.Identity;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Common;
using App.Core.Domain;
using App.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BookingOnline.Data
{
    public interface IPromotionRepository : IBaseGridRepository<M_Promotion, PromotionFilterModel>
    {
        ValueTask<IEnumerable<AspRole>> GetRolesExample();
        ValueTask<IEnumerable<CustomerGroup>> GetCustomerGroups(string userId);
        IEnumerable<M_Promotion_CustomerGroup> GetPromotionCustomerGroups(Guid promotionId, Guid orgId, Guid customerGroupId);
        IEnumerable<M_Promotion_Course> GetPromotionCourse(Guid promotionId, Guid CourseId);
        void DeletePromotionCourse(Guid id, IEnumerable<Guid> courseId);
        void DeletepromotionCustom(Guid id, IEnumerable<Guid> cusGroups);
        IEnumerable<M_Promotion_Course> GetPromotionCourse(Guid id);
        IEnumerable<M_Promotion_CustomerGroup> GetPromotionCusGroup(Guid id);
        PagingResponseEntity<M_Promotion> GetPagingData(PromotionFilterModel pagingModel);
        string GetPromotionCusGroupName(Guid id);
        string GetPromotionCourseName(Guid id);
    }
}