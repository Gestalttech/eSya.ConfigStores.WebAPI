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
                    var result = db.GtEcstrms.Where(x=>x.StoreType==0)

                                  .Select(s => new DO_StoreMaster
                                  {
                                      StoreCode = s.StoreCode,
                                      StoreType=s.StoreType,
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

        public async Task<DO_StoreMaster> GetStoreParameterList(int StoreCode, int StoreType)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    var ds = db.GtEcstrms
                        .Where(w => w.StoreCode == StoreCode && w.StoreType== StoreType)
                        .Select(r => new DO_StoreMaster
                        {
                            l_FormParameter =db.GtEcpasts.Where(x=>x.StoreCode==StoreCode).Select(p => new DO_eSyaParameter
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
                                StoreType=0,
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
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0084", Message = string.Format(_localizer[name: "W0084"]) };
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
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0084", Message = string.Format(_localizer[name: "W0084"]) };
                        }

                        GtEcstrm st_code = db.GtEcstrms.Where(s => s.StoreCode == storecodes.StoreCode && s.StoreType== storecodes.StoreType).FirstOrDefault();
                        if (st_code == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0085", Message = string.Format(_localizer[name: "W0085"]) };
                        }

                        if (!storecodes.ActiveStatus)
                        {
                            GtEastbl st_bl = db.GtEastbls.Where(s => s.StoreCode == storecodes.StoreCode && s.ActiveStatus).FirstOrDefault();
                            if (st_bl != null)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0086", Message = string.Format(_localizer[name: "W0086"]) };
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
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0087", Message = string.Format(_localizer[name: "W0087"]) };
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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveStoreCode(bool status,  int storecode, int StoreType)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcstrm strore_code = db.GtEcstrms.Where(w => w.StoreCode == storecode && w.StoreType== StoreType).FirstOrDefault();
                        if (strore_code == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0085", Message = string.Format(_localizer[name: "W0085"]) };
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
                                         StoreCode = Convert.ToInt32(m.StoreCode.ToString() + m.StoreType.ToString()),
                                         StoreType = m.StoreType,
                                         StoreDesc = m.StoreDesc,
                                         StoreTypeDesc = m.StoreType == 0 ? "Store" : "Department",
                                         ActiveStatus = false
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

        public async Task<DO_StoreBusinessLink> GetStoreBusinessLinkInfo(int BusinessKey, int StoreCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEastbls
                        .Where(w => w.BusinessKey == BusinessKey && w.StoreCode == StoreCode)
                        .Select(r => new DO_StoreBusinessLink
                        {
                            StoreCode=r.StoreCode,
                            StoreClass = r.StoreClass,
                            ActiveStatus = r.ActiveStatus
                        }).FirstOrDefaultAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_StoreBusinessLink>> GetPortfolioStoreBusinessLinkInfo(int BusinessKey, int StoreCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtEcspfms.Where(x => x.ActiveStatus)
                        .Select(r => new DO_StoreBusinessLink
                        {
                            BusinessKey = BusinessKey,
                            StoreCode = StoreCode,
                            ActiveStatus = false,
                            PortfolioId=r.PortfolioId,
                            PortfolioDesc=r.PortfolioDesc
                        }).ToListAsync();
                    
                    foreach (var obj in ds)
                    {
                        GtEcstpf pf = db.GtEcstpfs.Where(x => x.BusinessKey == BusinessKey && x.StoreCode==StoreCode && x.PortfolioId == obj.PortfolioId).FirstOrDefault();
                        if (pf != null)
                        {
                            obj.ActiveStatus = pf.ActiveStatus;
                        }
                        else
                        {
                            obj.ActiveStatus = false;
                            
                        }
                    }

                    return ds;
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
                        GtEastbl storelink = db.GtEastbls.Where(c => c.StoreCode == obj.StoreCode && c.BusinessKey == obj.BusinessKey && c.StoreClass==obj.StoreClass).FirstOrDefault(); 

                        if(storelink!=null)
                        {
                            storelink.ActiveStatus = obj.ActiveStatus;
                            storelink.ModifiedBy = obj.UserID;
                            storelink.ModifiedOn = DateTime.Now;
                            storelink.ModifiedTerminal = obj.TerminalID;
                            db.SaveChanges();
                        }
                        else
                        {
                            var stlink = new GtEastbl()
                            {
                                BusinessKey=obj.BusinessKey,
                                StoreCode=obj.StoreCode,
                                StoreClass=obj.StoreClass,
                                ActiveStatus=obj.ActiveStatus,
                                FormId=obj.FormId,
                                CreatedBy=obj.UserID,
                                CreatedOn=DateTime.Now,
                                CreatedTerminal=obj.TerminalID
                            };
                            db.GtEastbls.Add(stlink);
                            db.SaveChanges();
                        }
                        List<GtEcstpf> portfolio_links = db.GtEcstpfs.Where(c => c.StoreCode == obj.StoreCode && c.BusinessKey == obj.BusinessKey).ToList();
                        if (portfolio_links.Count > 0)
                        {
                            foreach (var store in portfolio_links)
                            {
                                db.GtEcstpfs.Remove(store);
                                db.SaveChanges();
                            }

                        }
                        if (obj.lst_businessLink != null)
                        {
                            foreach (var st_code in obj.lst_businessLink)
                            {
                                GtEcstpf objportf = new GtEcstpf
                                {
                                    StoreCode = st_code.StoreCode,
                                    BusinessKey = st_code.BusinessKey,
                                    PortfolioId = st_code.PortfolioId,
                                    ActiveStatus = st_code.ActiveStatus,
                                    FormId = obj.FormId,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = DateTime.Now,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtEcstpfs.Add(objportf);
                                await db.SaveChangesAsync();

                            }

                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }

                        else
                        {
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0088", Message = string.Format(_localizer[name: "W0088"]) };
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
                        .Join(db.GtEcfmpas,
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

        //public async Task<List<DO_StoreMaster>> GetStoreFormLinked(int formId)
        //{
        //    try
        //    {
        //        using (eSyaEnterprise db = new eSyaEnterprise())
        //        {
        //            var st =await db.GtEcstrms.Join(
        //                db.GtEcpasts.Where(w => w.ParamAction && w.ActiveStatus),
        //                s => s.StoreCode,
        //                p => p.StoreCode,
        //                (s, p) => new { s, p })
        //                .Join(db.GtEcfmpas.Where(w => w.FormId == formId),
        //                sp => new { sp.p.ParameterId },
        //                f => new { ParameterId = f.ParameterId },
        //                (sp, f) => new { sp, f })
        //                .Select(r => new 
        //                {
        //                    r.sp.s.StoreCode,
        //                    r.sp.s.StoreType,
        //                    r.sp.s.StoreDesc,
        //                }).ToListAsync();
        //            var distinctStores = st.GroupBy(x => new { x.StoreCode, x.StoreType }).Select(y => y.First());

        //            var result = distinctStores
        //           .GroupJoin(db.GtEcfmsts.Where(w => w.FormId == formId),
        //            s => s.StoreCode,
        //            f => f.StoreCode,
        //            (s, f) => new { s, f })
        //           .SelectMany(z => z.f.DefaultIfEmpty(),
        //            (a, b) => new DO_StoreMaster
        //            {
        //                StoreCode = a.s.StoreCode + a.s.StoreType,
        //                StoreType = a.s.StoreType,
        //                StoreDesc = a.s.StoreDesc,
        //                StoreTypeDesc =a. s.StoreType == 0 ? "Store" : "Department",
        //                ActiveStatus = b == null ? false : b.ActiveStatus

        //            }).ToList();

        //            return result.ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public async Task<List<DO_StoreMaster>> GetStoreFormLinked(int formId)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var st = await db.GtEcstrms.Join(
                        db.GtEcpasts.Where(w => w.ParamAction && w.ActiveStatus),
                        s => s.StoreCode,
                        p => p.StoreCode,
                        (s, p) => new { s, p })
                        .Join(db.GtEcfmpas.Where(w => w.FormId == formId),
                        sp => new { sp.p.ParameterId },
                        f => new { ParameterId = f.ParameterId },
                        (sp, f) => new { sp, f })
                        .Select(r => new DO_StoreMaster
                        {
                            StoreCode= Convert.ToInt32(r.sp.s.StoreCode.ToString() + r.sp.s.StoreType.ToString()),
                            StoreType=r.sp.s.StoreType,
                            StoreDesc=r.sp.s.StoreDesc,
                            StoreTypeDesc =  r.sp.s.StoreType == 0 ? "Store" : "Department",
                            ActiveStatus =false
                        }).ToListAsync();
                    var distinctStores = st.GroupBy(x => new { x.StoreCode, x.StoreType }).Select(y => y.First());

                    foreach (var obj in distinctStores)
                    {
                        GtEcfmst islink = db.GtEcfmsts.Where(c => c.FormId == formId && c.StoreCode == obj.StoreCode).FirstOrDefault();
                        if (islink != null)
                        {
                            obj.ActiveStatus = islink.ActiveStatus;
                        }
                        else
                        {
                            obj.ActiveStatus = false;
                        }
                    }
                    return distinctStores.ToList();

                    // var result = distinctStores
                    //.GroupJoin(db.GtEcfmsts.Where(w => w.FormId == formId),
                    // s => s.StoreCode,
                    // f => f.StoreCode,
                    // (s, f) => new { s, f })
                    //.SelectMany(z => z.f.DefaultIfEmpty(),
                    // (a, b) => new DO_StoreMaster
                    // {
                    //     StoreCode = a.s.StoreCode + a.s.StoreType,
                    //     StoreType = a.s.StoreType,
                    //     StoreDesc = a.s.StoreDesc,
                    //     StoreTypeDesc = a.s.StoreType == 0 ? "Store" : "Department",
                    //     ActiveStatus = b == null ? false : b.ActiveStatus

                    // }).ToList();

                    //return result.ToList();
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

        #region Portfolio Master
        public async Task<List<DO_PortfolioMaster>> GetAllPortfolios()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcspfms
                        .Select(p => new DO_PortfolioMaster
                        {
                            PortfolioId = p.PortfolioId,
                            PortfolioDesc = p.PortfolioDesc,
                            ActiveStatus = p.ActiveStatus,
                        }).OrderBy(o => o.PortfolioDesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertIntoPortfolio(DO_PortfolioMaster obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {


                        bool _ispfo = db.GtEcspfms.Any(a => a.PortfolioDesc.ToUpper().Replace(" ", "") == obj.PortfolioDesc.ToUpper().Replace(" ", ""));
                        if (_ispfo)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0089", Message = string.Format(_localizer[name: "W0089"]) };
                        }
                        int maxprtfoId = db.GtEcspfms.Select(c => c.PortfolioId).DefaultIfEmpty().Max();
                        maxprtfoId = maxprtfoId + 1;
                        var _pfolio = new GtEcspfm
                        {
                            PortfolioId = maxprtfoId,
                            PortfolioDesc = obj.PortfolioDesc,
                            ActiveStatus = obj.ActiveStatus,
                            FormId=obj.FormId,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };
                        db.GtEcspfms.Add(_pfolio);

                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
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
        public async Task<DO_ReturnParameter> UpdatePortfolio(DO_PortfolioMaster obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool is_portf = db.GtEcspfms.Any(a => a.PortfolioId != obj.PortfolioId && a.PortfolioDesc.ToUpper().Replace(" ", "") == obj.PortfolioDesc.ToUpper().Replace(" ", ""));
                        if (is_portf)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0089", Message = string.Format(_localizer[name: "W0089"]) };
                        }


                        GtEcspfm _prfolio = db.GtEcspfms.Where(w => w.PortfolioId == obj.PortfolioId).FirstOrDefault();
                        if (_prfolio == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0090", Message = string.Format(_localizer[name: "W0090"]) };
                        }

                        _prfolio.PortfolioDesc = obj.PortfolioDesc;
                        _prfolio.ActiveStatus = obj.ActiveStatus;
                        _prfolio.ModifiedBy = obj.UserID;
                        _prfolio.ModifiedOn = System.DateTime.Now;
                        _prfolio.ModifiedTerminal = obj.TerminalID;
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
        public async Task<DO_ReturnParameter> ActiveOrDeActivePortfolio(bool status, int PortfolioId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcspfm portfo = db.GtEcspfms.Where(w => w.PortfolioId == PortfolioId).FirstOrDefault();
                        if (portfo == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0090", Message = string.Format(_localizer[name: "W0090"]) };
                        }

                        portfo.ActiveStatus = status;
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
        #endregion
    }
}
