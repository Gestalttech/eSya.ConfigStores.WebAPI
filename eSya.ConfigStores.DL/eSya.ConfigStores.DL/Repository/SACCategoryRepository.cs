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
    public class SACCategoryRepository: ISACCategoryRepository
    {
        private readonly IStringLocalizer<SACCategoryRepository> _localizer;
        public SACCategoryRepository(IStringLocalizer<SACCategoryRepository> localizer)
        {
            _localizer = localizer;
        }
        #region SAC Category
        public async Task<List<DO_SACCategory>> GetSACCategories(int ISDCode)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var ds = db.GtSaccats.Where(x => x.Isdcode == ISDCode)
                                 .Select(x => new DO_SACCategory
                                 {
                                     Isdcode = x.Isdcode,
                                     Sacclass = x.Sacclass,
                                     Saccategory = x.Saccategory,
                                     SaccategoryDesc = x.SaccategoryDesc,
                                     ActiveStatus = x.ActiveStatus
                                 }
                        ).OrderBy(g => g.Saccategory).ToListAsync();
                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_SACCategory> GetSACCategoryByCategoryID(int ISDCode, string SACClassID, string SACCategoryID)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var ds = db.GtSaccats
                        .Where(w => w.Isdcode == ISDCode && w.Sacclass.ToUpper().Replace(" ", "") == SACClassID.ToUpper().Replace(" ", "") && w.Saccategory.ToUpper().Replace(" ", "") == SACCategoryID.ToUpper().Replace(" ", ""))
                                 .Select(x => new DO_SACCategory
                                 {
                                     Isdcode = x.Isdcode,
                                     Sacclass = x.Sacclass,
                                     Saccategory = x.Saccategory,
                                     SaccategoryDesc = x.SaccategoryDesc,
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
        public async Task<DO_ReturnParameter> InsertIntoSACCategory(DO_SACCategory obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var _isExists = db.GtSaccats.Where(x => x.Isdcode == obj.Isdcode && x.Sacclass.ToUpper().Replace(" ", "") == obj.Sacclass.ToUpper().Replace(" ", "")
                        && x.Saccategory.ToUpper().Replace(" ", "") == obj.Saccategory.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (_isExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0095", Message = string.Format(_localizer[name: "W0095"]) };

                        }
                        else
                        {
                            var desc = db.GtSaccats.Where(w =>w.Isdcode==obj.Isdcode 
                            && w.Sacclass.ToUpper().Replace(" ", "")==obj.Sacclass.ToUpper().Replace(" ", "")
                            && w.SaccategoryDesc.ToUpper().Replace(" ", "") == obj.SaccategoryDesc.ToUpper().Replace(" ", "")).Count();
                            if (desc > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0096", Message = string.Format(_localizer[name: "W0096"]) };

                            }
                            var SACCateg = new GtSaccat
                            {
                                Isdcode = obj.Isdcode,
                                Sacclass = obj.Sacclass,
                                Saccategory = obj.Saccategory,
                                SaccategoryDesc = obj.SaccategoryDesc,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtSaccats.Add(SACCateg);

                        }

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
        public async Task<DO_ReturnParameter> UpdateSACCategory(DO_SACCategory obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var _isSacateg = db.GtSaccats.Where(x => x.Isdcode == obj.Isdcode
                        && x.Sacclass.ToUpper().Replace(" ", "") == obj.Sacclass.ToUpper().Replace(" ", "")
                        && x.Saccategory.ToUpper().Replace(" ", "") == obj.Saccategory.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (_isSacateg == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0097", Message = string.Format(_localizer[name: "W0097"]) };
                        }
                        else
                        {
                            var desc = db.GtSaccats.Where(w =>w.Isdcode==obj.Isdcode 
                            && w.Sacclass.ToUpper().Replace(" ", "") == obj.Sacclass.ToUpper().Replace(" ", "")
                            && w.SaccategoryDesc.ToUpper().Replace(" ", "") == obj.SaccategoryDesc.ToUpper().Replace(" ", "")
                            && w.Saccategory.ToUpper().Replace(" ", "") != obj.Saccategory.ToUpper().Replace(" ", "")).Count();
                            if (desc > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0096", Message = string.Format(_localizer[name: "W0096"]) };

                            }

                            _isSacateg.SaccategoryDesc = obj.SaccategoryDesc;
                            _isSacateg.ActiveStatus = obj.ActiveStatus;
                            _isSacateg.ModifiedBy = obj.UserID;
                            _isSacateg.ModifiedOn = System.DateTime.Now;
                            _isSacateg.ModifiedTerminal = obj.TerminalID;

                        }
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
        public async Task<DO_ReturnParameter> DeleteSACCategory(int ISDCode, string SACClassID, string SACCategoryID)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var LinkExist = db.GtSaccods.Where(w => w.Isdcode == ISDCode && w.Sacclass.ToUpper().Replace(" ", "") == SACClassID.ToUpper().Replace(" ", "") 
                        && w.Saccategory.ToUpper().Replace(" ", "")== SACCategoryID.ToUpper().Replace(" ", "") && w.ActiveStatus).Count();
                        if (LinkExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0098", Message = string.Format(_localizer[name: "W0098"]) };
                        }

                        GtSaccat Saccateg = db.GtSaccats.Where(w => w.Isdcode == ISDCode && w.Sacclass.ToUpper().Replace(" ", "") == SACClassID.ToUpper().Replace(" ", "")
                        && w.Saccategory.ToUpper().Replace(" ", "")==SACCategoryID.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (Saccateg != null)
                        {
                            db.GtSaccats.Remove(Saccateg);
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
