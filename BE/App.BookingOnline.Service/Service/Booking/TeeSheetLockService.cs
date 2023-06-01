using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service.DTO;
using App.Core.Helper;
using App.Core.Service;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Service
{
    public class TeeSheetLockService : BaseGridService<TeeSheetLockDTO, TeeSheetLock, TeeSheetLockPagingModel, ITeeSheetLockRepository>, ITeeSheetLockService
    {
        ITeeSheetLockLineRepository _lineRepo;
        public TeeSheetLockService(ITeeSheetLockRepository repo, ITeeSheetLockLineRepository lineRepo ) : base(repo)
        {
            _lineRepo = lineRepo;
        }
        public override TeeSheetLockDTO Get(Guid Id)
        {
            var result = _repo.SingleOrDefaultAsync(x => x.Id == Id).Result;
            var lines = result.TeeSheetLockLines;
            result.TeeSheetLockLines = null;
            var dto = AutoMapperHelper.Map<TeeSheetLock, TeeSheetLockDTO>(result);
            dto.TeeSheetLockLines = AutoMapperHelper.Map<TeeSheetLockLine, TeeSheetLockLineDTO, List<TeeSheetLockLine>, List<TeeSheetLockLineDTO>>(lines);
            return dto;
        }

        private DateTime BuildTime(DateTime date, string time)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;
            int hour = 0;
            int minute = 0;
            int second = 0;

            var splitTime = time.Split(":");
            int.TryParse(splitTime[0], out hour);
            int.TryParse(splitTime[1], out minute);

            return new DateTime(year, month, day, hour, minute, second);
        }

        public override void Update(TeeSheetLockDTO entityDTO)
        {
            var lines = entityDTO.TeeSheetLockLines;
            entityDTO.TeeSheetLockLines = null;
            base.Update(entityDTO);
            var lineEntities = AutoMapperHelper.Map<TeeSheetLockLineDTO, TeeSheetLockLine, List<TeeSheetLockLineDTO>, List<TeeSheetLockLine>>(lines);
            foreach (var line in lineEntities)
            {
                line.C_Org_Id = entityDTO.C_Org_Id;
                line.IsActive = entityDTO.IsActive;
                
                if (line.IsDeleted.Value)
                {
                    _lineRepo.Remove(line);
                } else
                {
                    line.StartTimeValue = BuildTime(entityDTO.StartDate, line.StartTime);
                    line.EndTimeValue = BuildTime(entityDTO.EndDate, line.EndTime);
                    if (line.Id == null || line.Id == Guid.Empty)
                    {
                        line.CreatedDate = DateTime.Now;
                        line.CreatedUser = entityDTO.UpdatedUser;
                        _lineRepo.AddAsync(line);
                    }
                    else
                    {
                        line.UpdatedDate = DateTime.Now;
                        line.UpdatedUser = entityDTO.UpdatedUser;
                        _lineRepo.Update(line);
                    }
                }
               
            }
        }


        public override TeeSheetLockDTO Add(TeeSheetLockDTO entityDTO)
        {
            var lines = entityDTO.TeeSheetLockLines;
            entityDTO.TeeSheetLockLines = null;
            var result = base.Add(entityDTO);
            var lineEntities = AutoMapperHelper.Map<TeeSheetLockLineDTO, TeeSheetLockLine, List<TeeSheetLockLineDTO>, List<TeeSheetLockLine>>(lines);
            foreach (var line in lineEntities)
            {
                line.C_Org_Id = entityDTO.C_Org_Id;
                line.GF_TeeSheetLock_Id = result.Id;
                line.IsActive = entityDTO.IsActive;
                line.StartTimeValue = BuildTime(entityDTO.StartDate, line.StartTime);
                line.EndTimeValue = BuildTime(entityDTO.EndDate, line.EndTime);
            }
            _lineRepo.AddRangeAsync(lineEntities);

            result.TeeSheetLockLines = lines;
            return result;
        }

    }
    public class TeeSheetLockLineService : BaseGridService<TeeSheetLockLineDTO, TeeSheetLockLine, TeeSheetLockLinePagingModel, ITeeSheetLockLineRepository>, ITeeSheetLockLineService
    {
        public TeeSheetLockLineService(ITeeSheetLockLineRepository repo) : base(repo)
        {

        }
    }
}

