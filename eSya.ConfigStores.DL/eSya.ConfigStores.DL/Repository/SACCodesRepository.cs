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
    public class SACCodesRepository: ISACCodesRepository
    {
        private readonly IStringLocalizer<SACCodesRepository> _localizer;
        public SACCodesRepository(IStringLocalizer<SACCodesRepository> localizer)
        {
            _localizer = localizer;
        }
        #region SAC Codes
        public async Task<List<DO_SACCodes>> GetSACCodes(int ISDCode)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var ds = db.GtSaccods.Where(x => x.Isdcode == ISDCode)
                                 .Select(x => new DO_SACCodes
                                 {
                                     Isdcode = x.Isdcode,
                                     Sacclass = x.Sacclass,
                                     Saccategory = x.Saccategory,
                                     Saccode=x.Saccode,
                                     Sacdescription = x.Sacdescription,
                                     ActiveStatus = x.ActiveStatus,
                                     ParentId=x.Saccode
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
        public async Task<DO_SACCodes> GetSACCodeByCode(int ISDCode, string SACClassID, string SACCategoryID, string SACCodeID)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var ds = db.GtSaccods
                        .Where(w => w.Isdcode == ISDCode && w.Sacclass.ToUpper().Replace(" ", "") == SACClassID.ToUpper().Replace(" ", "") 
                        && w.Saccategory.ToUpper().Replace(" ", "") == SACCategoryID.ToUpper().Replace(" ", "")
                        && w.Saccode.ToUpper().Replace(" ", "") == SACCodeID.ToUpper().Replace(" ", ""))
                                 .Select(x => new DO_SACCodes
                                 {
                                     Isdcode = x.Isdcode,
                                     Sacclass = x.Sacclass,
                                     Saccategory = x.Saccategory,
                                     Saccode=x.Saccode,
                                     Sacdescription = x.Sacdescription,
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
        public async Task<DO_ReturnParameter> InsertIntoSACCode(DO_SACCodes obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var _isExists = db.GtSaccods.Where(x => x.Isdcode == obj.Isdcode && x.Sacclass.ToUpper().Replace(" ", "") == obj.Sacclass.ToUpper().Replace(" ", "")
                        && x.Saccategory.ToUpper().Replace(" ", "") == obj.Saccategory.ToUpper().Replace(" ", "")
                        && x.Saccode.ToUpper().Replace(" ", "") == obj.Saccode.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (_isExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0099", Message = string.Format(_localizer[name: "W0099"]) };

                        }
                        else
                        {
                            var desc = db.GtSaccods.Where(w => w.Isdcode == obj.Isdcode
                            && w.Sacclass.ToUpper().Replace(" ", "") == obj.Sacclass.ToUpper().Replace(" ", "")
                            && w.Sacdescription.ToUpper().Replace(" ", "") == obj.Sacdescription.ToUpper().Replace(" ", "")
                            && w.Saccode.ToUpper().Replace(" ", "") == obj.Saccode.ToUpper().Replace(" ", "")).Count();
                            if (desc > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0100", Message = string.Format(_localizer[name: "W0100"]) };

                            }
                            var SACCode = new GtSaccod
                            {
                                Isdcode = obj.Isdcode,
                                Sacclass = obj.Sacclass,
                                Saccategory = obj.Saccategory,
                                Saccode = obj.Isdcode.ToString() + obj.Sacclass.ToString() + obj.Saccode.ToString(),
                                Sacdescription = obj.Sacdescription,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtSaccods.Add(SACCode);
                           
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
        public async Task<DO_ReturnParameter> UpdateSACSACCode(DO_SACCodes obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var _isSaccode = db.GtSaccods.Where(x => x.Isdcode == obj.Isdcode
                        && x.Sacclass.ToUpper().Replace(" ", "") == obj.Sacclass.ToUpper().Replace(" ", "")
                        && x.Saccategory.ToUpper().Replace(" ", "") == obj.Saccategory.ToUpper().Replace(" ", "")
                        && x.Saccode.ToUpper().Replace(" ", "") == obj.Saccode.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (_isSaccode == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0101", Message = string.Format(_localizer[name: "W0101"]) };
                        }
                        else
                        {
                            var desc = db.GtSaccods.Where(w => w.Isdcode == obj.Isdcode
                            && w.Sacclass.ToUpper().Replace(" ", "") == obj.Sacclass.ToUpper().Replace(" ", "")
                            && w.Sacdescription.ToUpper().Replace(" ", "") == obj.Sacdescription.ToUpper().Replace(" ", "")
                            && w.Saccategory.ToUpper().Replace(" ", "") == obj.Saccategory.ToUpper().Replace(" ", "")
                            && w.Saccode.ToUpper().Replace(" ", "") != obj.Saccode.ToUpper().Replace(" ", "")).Count();
                            if (desc > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0100", Message = string.Format(_localizer[name: "W0100"]) };

                            }

                            _isSaccode.Sacdescription = obj.Sacdescription;
                            _isSaccode.ActiveStatus = obj.ActiveStatus;
                            _isSaccode.ModifiedBy = obj.UserID;
                            _isSaccode.ModifiedOn = System.DateTime.Now;
                            _isSaccode.ModifiedTerminal = obj.TerminalID;

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
        public async Task<DO_ReturnParameter> DeleteSACCode(int ISDCode, string SACClassID, string SACCategoryID,string SACCodeID)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                       
                        GtSaccod Saccacode = db.GtSaccods.Where(w => w.Isdcode == ISDCode && w.Sacclass.ToUpper().Replace(" ", "") == SACClassID.ToUpper().Replace(" ", "")
                        && w.Saccategory.ToUpper().Replace(" ", "") == SACCategoryID.ToUpper().Replace(" ", "")
                        && w.Saccode.ToUpper().Replace(" ", "") == SACCodeID.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (Saccacode != null)
                        {
                            db.GtSaccods.Remove(Saccacode);
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
