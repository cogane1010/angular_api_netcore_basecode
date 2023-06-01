using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using App.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using App.Core.Helper;
using Dapper;
using System.Data;
using System.Linq;
using System;
using App.Core;

namespace App.BookingOnline.Data.Repositories
{
    public class SequenceRepository : GridRepository<Sequence, SequencePagingModel>, ISequenceRepository
    {
       
        public SequenceRepository(BookingOnlineDbContext context)
            : base(context)
        { }

        public string GetCode(string DocumentType, int SeqMod)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                var code = "";
                var seq = this.dbSet.Where(x => x.DocumentType == DocumentType && x.IsActive).FirstOrDefault();
                if (seq != null)
                {
                    var dbSeqLine = this.Context.Set<SequenceLine>();
                    var query = dbSeqLine.Where(x => x.App_Sequence_Id == seq.Id && x.IsActive);
                    var curIndex = 1;
                    var prefix = "";
                    if (SeqMod == SequenceModes.Year)
                    {
                        query = query.Where(x => x.YearValue == DateTime.Today.Year % 1000);
                        var seqline = query.FirstOrDefault();
                        if (seqline == null)
                        {
                            dbSeqLine.Add(new SequenceLine
                            {
                                Id = Guid.NewGuid(),
                                App_Sequence_Id = seq.Id,
                                CreatedDate = DateTime.Now,
                                CreatedUser = "admin",
                                YearValue = DateTime.Today.Year % 1000,
                                SeqValue = 1,
                                IsActive = true,
                            });
                        }
                        else
                        {
                            curIndex = seqline.SeqValue + 1;
                            seqline.SeqValue = curIndex;
                            dbSeqLine.Update(seqline);
                        }

                        prefix = (DateTime.Today.Year % 1000).ToString("00");

                    }
                    else if (SeqMod == SequenceModes.Month)
                    {
                        query = query.Where(x => x.MonthValue == DateTime.Today.Month && x.YearValue == DateTime.Today.Year % 1000);
                        var seqline = query.FirstOrDefault();
                        if (seqline == null)
                        {
                            dbSeqLine.Add(new SequenceLine
                            {
                                Id = Guid.NewGuid(),
                                App_Sequence_Id = seq.Id,
                                CreatedDate = DateTime.Now,
                                CreatedUser = "admin",
                                YearValue = DateTime.Today.Year % 1000,
                                MonthValue = DateTime.Today.Month,
                                SeqValue = 1,
                                IsActive = true,
                            });
                        }
                        else
                        {
                            curIndex = seqline.SeqValue;
                            seqline.SeqValue = curIndex;
                            dbSeqLine.Update(seqline);
                        }
                        prefix = (DateTime.Today.Year % 1000).ToString("00") + DateTime.Today.Month.ToString("00");
                    }
                    else if (SeqMod == SequenceModes.Day)
                    {
                        query = query.Where(x => x.DateId == DateTime.Today);
                        var seqline = query.FirstOrDefault();
                        if (seqline == null)
                        {
                            dbSeqLine.Add(new SequenceLine
                            {
                                Id = Guid.NewGuid(),
                                App_Sequence_Id = seq.Id,
                                CreatedDate = DateTime.Now,
                                CreatedUser = "admin",
                                DateId = DateTime.Today,
                                SeqValue = 1,
                                IsActive = true,
                            });
                        }
                        else
                        {
                            curIndex = seqline.SeqValue;
                            seqline.SeqValue = curIndex;
                            dbSeqLine.Update(seqline);
                        }
                        prefix = DateTime.Today.ToString("yyMMdd");
                    }
                    else
                    {
                        query = query.Where(x => x.YearValue == null && x.MonthValue == null && x.DateId == null);
                        var seqline = query.FirstOrDefault();
                        if (seqline == null)
                        {
                            dbSeqLine.Add(new SequenceLine
                            {
                                Id = Guid.NewGuid(),
                                App_Sequence_Id = seq.Id,
                                CreatedDate = DateTime.Now,
                                CreatedUser = "admin",
                                SeqValue = 1,
                                IsActive = true,
                            });
                        }
                        else
                        {
                            curIndex = seqline.SeqValue;
                            seqline.SeqValue = curIndex;
                            dbSeqLine.Update(seqline);
                        }
                    }
                    var length = seq.MaxLength.HasValue ? seq.MaxLength : 6;
                    var formatIndex = "";
                    for (int i = 0; i < length; i++)
                    {
                        formatIndex += "0";
                    }
                    this.Context.SaveChanges();
                    code = seq.Prefix + prefix + curIndex.ToString(formatIndex);
                    transaction.Commit();
                    return code;
                }
                else
                {
                    throw new Exception("Sequence not exist!");
                }
            }
            
        }
        

    }

    public class SequenceLineRepository : GridRepository<SequenceLine, SequenceLinePagingModel>, ISequenceLineRepository
    {
        public SequenceLineRepository(BookingOnlineDbContext context)
            : base(context)
        { }


    }
}