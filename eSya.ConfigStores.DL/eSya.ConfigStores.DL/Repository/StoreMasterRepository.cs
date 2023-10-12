using eSya.ConfigStores.DL.Entities;
using eSya.ConfigStores.DO;
using eSya.ConfigStores.DO.StaticVariables;
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
    public class StoreMasterRepository: IStoreMasterRepository
    {
        private readonly IStringLocalizer<StoreMasterRepository> _localizer;
        public StoreMasterRepository(IStringLocalizer<StoreMasterRepository> localizer)
        {
            _localizer = localizer;
        }

        #region Store Master

        public async Task<List<DO_StoreMaster>> GetStoreCodes()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcstrms

                                  .Select(s => new DO_StoreMaster
                                  {
                                      StoreType = s.StoreType,
                                      StoreCode = s.StoreCode,
                                      StoreDesc = s.StoreDesc,
                                      ActiveStatus = s.ActiveStatus
                                  }).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_StoreMaster> GetStoreParameterList(int StoreCode)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    var ds = db.GtEcstrms
                        .Where(w => w.StoreCode == StoreCode)
                        .Select(r => new DO_StoreMaster
                        {
                            l_FormParameter = r.GtEcpasts.Select(p => new DO_eSyaParameter
                            {
                                ParameterID = p.ParameterId,
                                ParmAction = p.ParamAction
                            }).ToList()
                        }).FirstOrDefaultAsync();

                    return await ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateStoreCodes(DO_StoreMaster storecodes)
        {
            try
            {
                if (storecodes.StoreCode != 0)
                {
                    return await UpdateStoreCodes(storecodes);
                }
                else
                {
                    return await InsertStoreCodes(storecodes);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DO_ReturnParameter> InsertStoreCodes(DO_StoreMaster storecodes)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcstrm isStorecodeExists = db.GtEcstrms.FirstOrDefault(c => c.StoreDesc.ToUpper().Replace(" ", "") == storecodes.StoreDesc.ToUpper().Replace(" ", ""));

                        if (isStorecodeExists == null)
                        {
                            int maxval = db.GtEcstrms.Select(c => c.StoreCode).DefaultIfEmpty().Max();
                            int storecode_ = maxval + 1;
                            var objstorecode = new GtEcstrm
                            {
                                StoreCode = storecode_,
                                StoreType = storecodes.StoreType,
                                StoreDesc = storecodes.StoreDesc,
                                FormId = storecodes.FormId,
                                ActiveStatus = storecodes.ActiveStatus,
                                CreatedBy = storecodes.UserID,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = storecodes.TerminalID
                            };
                            db.GtEcstrms.Add(objstorecode);

                            foreach (DO_eSyaParameter ip in storecodes.l_FormParameter)
                            {
                                var pMaster = new GtEcpast
                                {
                                    StoreCode = storecode_,
                                    ParameterId = ip.ParameterID,
                                    ParamAction = ip.ParmAction,
                                    ActiveStatus = ip.ActiveStatus,
                                    FormId = storecodes.FormId,
                                    CreatedBy = storecodes.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = storecodes.TerminalID,
                                };
                                db.GtEcpasts.Add(pMaster);
                            }

                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0100", Message = string.Format(_localizer[name: "W0100"]) };
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateStoreCodes(DO_StoreMaster storecodes)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcstrm isStorecodeExists = db.GtEcstrms.FirstOrDefault(c => c.StoreCode != storecodes.StoreCode && c.StoreDesc.ToUpper().Replace(" ", "") == storecodes.StoreDesc.ToUpper().Replace(" ", ""));
                        if (isStorecodeExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0100", Message = string.Format(_localizer[name: "W0100"]) };
                        }

                        GtEcstrm st_code = db.GtEcstrms.Where(s => s.StoreCode == storecodes.StoreCode).FirstOrDefault();
                        if (st_code == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0101", Message = string.Format(_localizer[name: "W0101"]) };
                        }

                        if (!storecodes.ActiveStatus)
                        {
                            GtEastbl st_bl = db.GtEastbls.Where(s => s.StoreCode == storecodes.StoreCode && s.ActiveStatus).FirstOrDefault();
                            if (st_bl != null)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0102", Message = string.Format(_localizer[name: "W0102"]) };
                            }
                        }

                        st_code.StoreDesc = storecodes.StoreDesc;
                        st_code.ActiveStatus = storecodes.ActiveStatus;
                        st_code.ModifiedBy = storecodes.UserID;
                        st_code.ModifiedOn = DateTime.Now;
                        st_code.ModifiedTerminal = storecodes.TerminalID;

                        //if (storecodes.ActiveStatus == true)
                        //{
                        foreach (DO_eSyaParameter ip in storecodes.l_FormParameter)
                        {
                            GtEcpast sPar = db.GtEcpasts.Where(x => x.StoreCode == storecodes.StoreCode && x.ParameterId == ip.ParameterID).FirstOrDefault();
                            if (sPar != null)
                            {
                                sPar.ParamAction = ip.ParmAction;
                                sPar.ActiveStatus = storecodes.ActiveStatus;
                                sPar.ModifiedBy = storecodes.UserID;
                                sPar.ModifiedOn = System.DateTime.Now;
                                sPar.ModifiedTerminal = storecodes.TerminalID;
                            }
                            else
                            {
                                var pMaster = new GtEcpast
                                {
                                    StoreCode = storecodes.StoreCode,
                                    ParameterId = ip.ParameterID,
                                    ParamAction = ip.ParmAction,
                                    ActiveStatus = ip.ActiveStatus,
                                    FormId = storecodes.FormId,
                                    CreatedBy = storecodes.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = storecodes.TerminalID,
                                };
                                db.GtEcpasts.Add(pMaster);
                            }
                        }
                        //}
                        //else
                        //{
                        //    var fa = await db.GtEcpast.Where(x => x.StoreCode == storecodes.StoreCode).ToListAsync();

                        //    if (fa != null)
                        //        db.GtEcpast.RemoveRange(fa);
                        //}

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> DeleteStoreCode(int Storecode)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcstrm store = db.GtEcstrms.FirstOrDefault(c => c.StoreCode == Storecode);

                        var paramstore = db.GtEcpasts.Where(c => c.StoreCode == Storecode).ToList();
                        if (store != null)
                        {
                            db.GtEcpasts.RemoveRange(paramstore);
                            db.GtEcstrms.Remove(store);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0005", Message = string.Format(_localizer[name: "S0005"]) };
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0103", Message = string.Format(_localizer[name: "W0103"]) };
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<List<DO_StoreMaster>> GetActiveStoreCodes()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcstrms.Where(x => x.ActiveStatus == true)

                                  .Select(s => new DO_StoreMaster
                                  {
                                      StoreCode = s.StoreCode,
                                      StoreDesc = s.StoreDesc
                                  }).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveStoreCode(bool status, string storetype, int storecode)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcstrm strore_code = db.GtEcstrms.Where(w => w.StoreType.ToUpper().Replace(" ", "") == storetype.ToUpper().Replace(" ", "") && w.StoreCode == storecode).FirstOrDefault();
                        if (strore_code == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0101", Message = string.Format(_localizer[name: "W0101"]) };
                        }

                        strore_code.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0003", Message = string.Format(_localizer[name: "S0003"]) };
                        else
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0004", Message = string.Format(_localizer[name: "S0004"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion  Store Master

        #region Store Business Link

        public async Task<List<DO_StoreMaster>> GetStoreList(int BusinessKey)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var sm = await db.GtEcstrms.Where(w => w.ActiveStatus == true)
                                     .Select(m => new DO_StoreMaster()
                                     {
                                         StoreCode = m.StoreCode,
                                         StoreDesc = m.StoreDesc
                                     }).ToListAsync();


                    foreach (var obj in sm)
                    {
                        GtEastbl getlocDesc = db.GtEastbls.Where(c => c.BusinessKey == BusinessKey && c.StoreCode == obj.StoreCode).FirstOrDefault();
                        if (getlocDesc != null)
                        {
                            obj.ActiveStatus = getlocDesc.ActiveStatus;
                        }
                        else
                        {
                            obj.ActiveStatus = false;
                        }
                    }
                    return sm;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_StoreBusinessLink>> GetStoreBusinessLinkInfo(int BusinessKey, int StoreCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEastbls
                        .Where(w => w.BusinessKey == BusinessKey && w.StoreCode == StoreCode)
                        .Select(r => new DO_StoreBusinessLink
                        {
                            StoreClass = r.StoreClass,
                            ActiveStatus = r.ActiveStatus
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertOrUpdateStoreBusinessLink(DO_StoreBusinessLink obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        List<GtEastbl> st_links = db.GtEastbls.Where(c => c.StoreCode == obj.StoreCode && c.BusinessKey == obj.BusinessKey).ToList();
                        if (st_links.Count > 0)
                        {
                            foreach (var store in st_links)
                            {
                                db.GtEastbls.Remove(store);
                                db.SaveChanges();
                            }

                        }
                        if (obj.lst_businessLink != null)
                        {
                            foreach (var st_code in obj.lst_businessLink)
                            {
                                GtEastbl objstrore = new GtEastbl
                                {
                                    StoreCode = st_code.StoreCode,
                                    BusinessKey = st_code.BusinessKey,
                                    StoreClass = st_code.StoreClass,
                                    ActiveStatus = st_code.ActiveStatus,
                                    FormId = obj.FormId,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = DateTime.Now,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtEastbls.Add(objstrore);
                                await db.SaveChangesAsync();

                            }

                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }

                        else
                        {
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0104", Message = string.Format(_localizer[name: "W0104"]) };
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion  Store Business Link

        #region Store Form Link

        public async Task<List<DO_Forms>> GetFormForStorelinking()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = await db.GtEcfmfds
                        .Join(db.GtEcfmps,
                            f => f.FormId,
                            p => p.FormId,
                            (f, p) => new { f, p })
                       .Where(w => w.f.ActiveStatus
                                  && w.p.ParameterId == ParameterIdValues.Form_isStoreLink
                                  && w.p.ActiveStatus)
                                  .Select(r => new DO_Forms
                                  {
                                      FormID = r.f.FormId,
                                      FormCode = r.f.FormCode,
                                      FormName = r.f.FormName
                                  }).OrderBy(o => o.FormCode).ToListAsync();
                    var Distinctforms = result.GroupBy(x => x.FormID).Select(y => y.First());
                    return Distinctforms.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_StoreMaster>> GetStoreFormLinked(int formId)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var st = db.GtEcstrms.Join(
                        db.GtEcpasts.Where(w => w.ParamAction && w.ActiveStatus),
                        s => s.StoreCode,
                        p => p.StoreCode,
                        (s, p) => new { s, p })
                        .Join(db.GtEcfmps.Where(w => w.FormId == formId),
                        sp => new { sp.p.ParameterId },
                        f => new { ParameterId = f.SubParameterId },
                        (sp, f) => new { sp, f })
                        .Select(r => new
                        {
                            r.sp.s.StoreType,
                            r.sp.s.StoreCode,
                            r.sp.s.StoreDesc,
                        });

                    //var result = await st
                    //    .GroupJoin(db.GtEcfmsts.Where(w => w.FormId == formId),
                    //        s => s.StoreCode,
                    //        f => f.StoreCode,
                    //  //(s, f) => new { s, f = f.FirstOrDefault() }).DefaultIfEmpty()
                    //  (s, f) => new { s, f = f.FirstOrDefault() })
                    //  .Select(s => new DO_StoreMaster
                    //  {
                    //      StoreType = s.s.StoreType,
                    //      StoreCode = s.s.StoreCode,
                    //      StoreDesc = s.s.StoreDesc,
                    //      ActiveStatus = s.f != null ? s.f.ActiveStatus : false
                    //  }).ToListAsync();

                    var result = await st
                   .GroupJoin(db.GtEcfmsts.Where(w => w.FormId == formId),
                    s => s.StoreCode,
                    f => f.StoreCode,
                    (s, f) => new { s, f })
                   .SelectMany(z => z.f.DefaultIfEmpty(),
                    (a, b) => new DO_StoreMaster
                    {
                        StoreType = a.s.StoreType,
                        StoreCode = a.s.StoreCode,
                        StoreDesc = a.s.StoreDesc,
                        ActiveStatus = b == null ? false : b.ActiveStatus
                    }).ToListAsync();
                    var Distinctstores = result.GroupBy(x => x.StoreCode).Select(y => y.First());
                    return Distinctstores.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoFormStoreLink(List<DO_StoreFormLink> l_obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var fa = db.GtEcfmsts.Where(w => w.FormId == l_obj.FirstOrDefault().FormId);
                        foreach (GtEcfmst f in fa)
                        {
                            f.ActiveStatus = false;
                            f.ModifiedBy = l_obj.FirstOrDefault().UserID;
                            f.ModifiedOn = System.DateTime.Now;
                            f.ModifiedTerminal = l_obj.FirstOrDefault().TerminalID;
                        }
                        await db.SaveChangesAsync();

                        foreach (DO_StoreFormLink s in l_obj)
                        {
                            var fs = db.GtEcfmsts.Where(w => w.FormId == s.FormId && w.StoreCode == s.StoreCode).FirstOrDefault();
                            if (fs != null)
                            {
                                fs.ActiveStatus = true;
                                fs.ModifiedBy = s.UserID;
                                fs.ModifiedOn = DateTime.Now;
                                fs.ModifiedTerminal = System.Environment.MachineName;
                            }
                            else
                            {
                                fs = new GtEcfmst();
                                fs.FormId = s.FormId;
                                fs.StoreCode = s.StoreCode;
                                fs.ActiveStatus = true;
                                fs.CreatedBy = s.UserID;
                                fs.CreatedOn = DateTime.Now;
                                fs.CreatedTerminal = System.Environment.MachineName;
                                db.GtEcfmsts.Add(fs);
                            }
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

        #endregion
    }
}
