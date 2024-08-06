using eSya.ConfigStores.DL.Entities;
using eSya.ConfigStores.DO;
using eSya.ConfigStores.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigStores.DL.Repository
{
    public class SACClassRepository: ISACClassRepository
    {
        private readonly IStringLocalizer<SACClassRepository> _localizer;
        public SACClassRepository(IStringLocalizer<SACClassRepository> localizer)
        {
            _localizer = localizer;
        }

        #region Define SAC Class
        public async Task<List<DO_SACClass>> GetSACClasses(int ISDCode)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var ds = db.GtSacclas.Where(x=>x.Isdcode==ISDCode)
                                 .Select(x => new DO_SACClass
                                 {
                                     Isdcode=x.Isdcode,
                                     Sacclass = x.Sacclass,
                                     SacclassDesc=x.SacclassDesc,
                                     UsageStatus = x.UsageStatus,
                                     ActiveStatus = x.ActiveStatus
                                 }
                        ).OrderBy(o => o.Sacclass).ToListAsync();
                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_SACClass> GetSACClassByClassID(int ISDCode,string SACClassID)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var ds = db.GtSacclas
                        .Where(i => i.Isdcode == ISDCode && i.Sacclass.ToUpper().Replace(" ", "") == SACClassID.ToUpper().Replace(" ", ""))
                                 .Select(x => new DO_SACClass
                                 {
                                     Isdcode = x.Isdcode,
                                     Sacclass = x.Sacclass,
                                     SacclassDesc = x.SacclassDesc,
                                     UsageStatus = x.UsageStatus,
                                     ActiveStatus = x.ActiveStatus
                                 }
                        ).FirstOrDefaultAsync();
                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertIntoSACClass(DO_SACClass obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var _isExists = db.GtSacclas.Where(x =>x.Isdcode==obj.Isdcode 
                        && x.Sacclass.ToUpper().Replace(" ", "") == obj.Sacclass.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if(_isExists!=null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0091", Message = string.Format(_localizer[name: "W0091"]) };

                        }
                        else
                        {
                            var desc = db.GtSacclas.Where(w =>w.Isdcode==obj.Isdcode
                            && w.SacclassDesc.ToUpper().Replace(" ", "") == obj.SacclassDesc.ToUpper().Replace(" ", "")).Count();
                            if(desc > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0092", Message = string.Format(_localizer[name: "W0092"]) };

                            }
                            var SACClass = new GtSaccla
                            {
                                Isdcode = obj.Isdcode,
                                Sacclass = obj.Sacclass,
                                SacclassDesc = obj.SacclassDesc,
                                UsageStatus = false,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtSacclas.Add(SACClass);

                        }
                        //var usage = db.GtSacclas.Where(x =>x.Isdcode==obj.Isdcode && x.Sacclass == obj.Sacclass && obj.ActiveStatus).FirstOrDefault();
                        //if(usage != null)
                        //{
                        //    usage.UsageStatus = true;
                        //}
                       
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public async Task<DO_ReturnParameter> UpdateSACClass(DO_SACClass obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var _isSac = db.GtSacclas.Where(x =>x.Isdcode==obj.Isdcode 
                        && x.Sacclass.ToUpper().Replace(" ", "") == obj.Sacclass.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (_isSac == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0093", Message = string.Format(_localizer[name: "W0093"]) };
                        }
                        else
                        {
                            var desc = db.GtSacclas.Where(w =>w.Isdcode==obj.Isdcode 
                            && w.SacclassDesc.ToUpper().Replace(" ", "") == obj.SacclassDesc.ToUpper().Replace(" ", "") 
                            && w.Sacclass.ToUpper().Replace(" ", "") != obj.Sacclass.ToUpper().Replace(" ", "")).Count();
                            if (desc > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0092", Message = string.Format(_localizer[name: "W0092"]) };

                            }

                            _isSac.SacclassDesc = obj.SacclassDesc;
                            _isSac.UsageStatus = false;
                            _isSac.ActiveStatus = obj.ActiveStatus;
                            _isSac.ModifiedBy = obj.UserID;
                            _isSac.ModifiedOn = System.DateTime.Now;
                            _isSac.ModifiedTerminal = obj.TerminalID;
                           
                        }
                        //var usage = db.GtSacclas.Where(x => x.Isdcode == obj.Isdcode && x.Sacclass == obj.Sacclass && obj.ActiveStatus).FirstOrDefault();
                        //if (usage != null)
                        //{
                        //    usage.UsageStatus = true;
                        //}
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public async Task<DO_ReturnParameter> DeleteSACClass(int ISDCode,string SACClassID)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var LinkExist = db.GtSaccats.Where(w =>w.Isdcode== ISDCode 
                        && w.Sacclass.ToUpper().Replace(" ", "") == SACClassID.ToUpper().Replace(" ", "")
                        && w.ActiveStatus).Count();
                        if (LinkExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0094", Message = string.Format(_localizer[name: "W0094"]) };
                        }

                        GtSaccla SacClass = db.GtSacclas.Where(w =>w.Isdcode== ISDCode && w.Sacclass.ToUpper().Replace(" ", "") == SACClassID.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (SacClass != null)
                        {
                            db.GtSacclas.Remove(SacClass);
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0005", Message = string.Format(_localizer[name: "S0005"]) };
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion

    }
}
