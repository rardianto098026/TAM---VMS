using Kendo.Mvc.UI;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TAM.VMS.Domain;
using Dapper;
using Kendo.Mvc.UI;
using TAM.VMS.Infrastructure.Session;
using System.Data;
using TAM.VMS.Domain.Object;
using TAM.VMS.Infrastructure.General;
using Microsoft.Data.SqlClient;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;

namespace TAM.VMS.Service
{
    public class DownloadVendorDatabaseService : DbService
    {
        public DownloadVendorDatabaseService(IDbHelper db) : base(db)
        {
        }

        public async Task<ObjectResult<APIUserAuthenticatedResponse>> AuthenticateAPI(APIUserLoginRequest model, bool isLoginSSO = false)
        {
            var result = new ObjectResult<APIUserAuthenticatedResponse>();
            try
            {
                if (!string.IsNullOrEmpty(model.Username) && !string.IsNullOrEmpty(model.Password) && !isLoginSSO || isLoginSSO)
                {
                    var USERNAME = model.Username;
                    var password = model.Password;
                    var user = new User();
                    if (isLoginSSO)
                        user = GetUserByNoReg(model.NoReg);
                    else
                        user = GetUserByUsername(USERNAME);

                    var roles = GetRolesByUsername(model.Username);

                    if (user == null) throw new ModelException("Password", "User or Password not match");

                    if (isLoginSSO == false)
                    {
                        if (password == null) throw new ModelException("Password", "User or Password not match");

                        if (MD5Helper.Encode(password) != user.Password)
                        {
                            if (password != user.Password)
                                throw new ModelException("Password", "User or Password not match");
                        }
                    }


                    var claims = new[]
                    {
                        new Claim("Name", user.Name),
                        new Claim("USERNAME", user.Username),
                        new Claim("Roles", string.Join(",", roles)),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };


                    var authSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(GeneralHelper.Config?.JWT?.Secret ?? string.Empty));

                    var token = new JwtSecurityToken(
                            issuer: GeneralHelper.Config?.JWT?.ValidIssuer,
                            audience: GeneralHelper.Config?.JWT?.ValidAudience,
                            expires: DateTime.Now.AddHours(GeneralHelper.Config?.JWT?.ExpireHour ?? 1),
                            claims: claims,
                            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    result.Obj = new APIUserAuthenticatedResponse
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        ValidTo = token.ValidTo
                    };
                    result.Status.OK();

                    return result;
                }
                result.Status.UnAuthorized("User or Password not match");
            }
            catch (Exception ex)
            {
                result.Status.UnAuthorized("Failed to Authenticate User");
                //result.Error("Failed to Authenticate User", ex.Message);
            }
            return result;
        }

        public DataSourceResult GetDataSourceResult(DataSourceRequest request)
        {
            var genericDataTableQuery = new GenericDataSourceQuery(Db.Connection, request);
            var result = genericDataTableQuery.GetData<DownloadVendorDBView>("SELECT * FROM [VW_DownloadVendorDB]");
            return result;
        }

        public List<UserRoleView> GetAllUser()
        {

            List<UserRoleView> results = new List<UserRoleView>();
            try
            {
                using (var sqlConnection = new SqlConnection(Db.Connection.ConnectionString))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                        sqlConnection.Open();
                    var sql = "SELECT * FROM [VW_UserRole]";
                    results = sqlConnection.Query<UserRoleView>(sql).ToList();
                    return results;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return results;
        }

        public User GetUserByUsername(string username)
        {
            return Db.UserRepository.Find(new { Username = username, RowStatus = true }).FirstOrDefault();
        }

        public User GetUserByName(string name)
        {
            return Db.UserRepository.Find(new { Name = name, RowStatus = true }).FirstOrDefault();
        }

        public User GetUserByID(Guid id)
        {
            return Db.UserRepository.Find(new { ID = id }).FirstOrDefault();
        }

        public IEnumerable<User> GetUser()
        {
            var result = Db.UserRepository.FindAll().Where(x => x.RowStatus == true).ToList();
            return result;
        }

        public User GetUserByNoReg(string noReg)
        {
            return Db.UserRepository.Find(new { NIP = noReg }).FirstOrDefault();
        }

        public IEnumerable<UserRoleView> GetUsersRoleViewByRole(string roleName)
        {
            return Db.UserRoleViewRepository.Find(new { Role = roleName, RowStatus = 1 }).ToList();
        }

        public UserRoleView GetUserRoleViewByRole(string roleName)
        {
            return Db.UserRoleViewRepository.Find(new { Role = roleName, RowStatus = 1 }).FirstOrDefault();
        }

        public IEnumerable<string> GetRolesByUsername(string username)
        {
            var userRole = Db.UserRoleRepository.FindByUsername(username);
            var userRoleString = userRole.Select(x => x.RoleName).ToList();

            return userRoleString.Count() > 0 ? userRoleString : new[] { "User" }.ToList();
        }
        public bool IsUseMasterPassword()
        {
            return SessionManager.IsUseMasterPassword;
        }


        //public string Save(User user, IEnumerable<string> roles)
        //{
        //    //var result = true;
        //    string result = string.Empty;

        //    var getUsers = GetUser().Where(x => x.Username.ToLower() == user.Username.ToLower() && x.RowStatus == true).FirstOrDefault();
        //    var editAble = GetUser().Where(x => x.Username.ToLower() == user.Username.ToLower() && x.ID == user.ID && x.RowStatus == true).FirstOrDefault();

        //    using (DbHelper db = new DbHelper(true))
        //    {
        //        if (getUsers == null || editAble != null)
        //        {
        //            if (user.ID == default)
        //            {
        //                var password = "123"; //ApplicationCacheManager.GetConfig<string>("DefaultPassword");

        //                user.Password = MD5Helper.Encode(password);
        //                user.Username = user.Username.ToLower();

        //                user.CreatedBy = SessionManager.Current;
        //                user.CreatedOn = DateTime.Now;
        //                user.RowStatus = true;
        //                user = db.UserRepository.Add(user, new string[] {
        //                "Username",
        //                "Password",
        //                "Name",
        //                "Surname",
        //                "Email",
        //                "NIP",
        //                "CreatedOn",
        //                "CreatedBy",
        //                "RowStatus"
        //            });
        //            }
        //            else
        //            {
        //                user.ModifiedBy = SessionManager.Current;
        //                user.ModifiedOn = DateTime.Now;

        //                db.UserRepository.Update(user, new string[] {
        //                "Username",
        //                "Name",
        //                "Surname",
        //                "Email",
        //                "NIP",
        //                "ModifiedOn",
        //                "ModifiedBy"
        //            });
        //            }

        //            // UPDATE ROLES
        //            var parametersRole = new DynamicParameters();
        //            parametersRole.Add("@UserID", user.ID);
        //            parametersRole.Add("@RoleID", roles == null ? "" : string.Join(",", roles));
        //            parametersRole.Add("@By", SessionManager.Current);
        //            db.Connection.Execute("usp_Core_UpdateUserRole", parametersRole, db.Transaction, commandType: System.Data.CommandType.StoredProcedure);

        //            db.Commit();
        //        }
        //        else
        //        {
        //            result = user.Username + " is already exist.";
        //        }

        //    }
        //    return result;
        //}

        public string AddRequest()
        {
            string result = string.Empty;

            using (var db = new DbHelper(true))
            {
                try
                {
                    Guid idModule = GetIdModule();
                    Guid idModuleProcess = GetIdModuleProcess(idModule);

                    var vendorDB = new DownloadVendorDatabase
                    {
                        FileName = "Database Vendor " + SessionManager.Department,
                        ReqDate = DateTime.Now,
                        DepartmentId = SessionManager.DepartmentID,
                        StatusId = "1", // 1 = "Requested"
                        CreatedBy = SessionManager.Current,
                        CreatedOn = DateTime.Now,
                        IdModuleProcess = idModuleProcess
                    };

                    vendorDB = db.DownloadVendorDatabaseRepository.Add(vendorDB, new[]
                    {
                        "FileName",
                        "ReqDate",
                        "DepartmentId",
                        "StatusId",
                        "CreatedOn",
                        "CreatedBy",
                        "IdModuleProcess",
                    });

                    var task = new TaskList
                    {
                        VendorName = "ini vendor static", // This should be replaced with dynamic input
                        IdModule = idModule,
                        IdDataByModule = vendorDB.ID,
                        IdModuleProcess = idModuleProcess,
                        StatusID = 1,
                        CreatedBy = SessionManager.Current,
                        CreatedOn = DateTime.Now
                    };

                    task = db.TaskListRepository.Add(task, new[]
                    {
                        "VendorName",
                        "IdModule",
                        "IdDataByModule",
                        "IdModuleProcess",
                        "StatusID",
                        "CreatedBy",
                        "CreatedOn"
                    });

                    db.Commit();
                }
                catch (Exception ex)
                {
                    // Handle exception (e.g., log the error)
                    result = "Error: " + ex.Message;
                }
            }

            return result;
        }

        public Guid GetIdModule()
        {
            using (var dbContext = new DbHelper(true))
            {
                // Check if the module exists
                var existingModule = dbContext.MasterModuleRepository.Find(new {Module = "DownloadVendorDatabase"}).FirstOrDefault();

                if (existingModule != null)
                {
                    // Module exists, return its ID
                    return existingModule.ID; // Assuming Id is of type Guid
                }
                else
                {
                    // Module does not exist, create a new one
                    var newModule = new MasterModule // Assuming your entity class is named Module
                    {
                        Module = "DownloadVendorDatabase",
                        Desc = "Download Vendor Database",
                        CreatedBy = "System",
                        CreatedDate = DateTime.Now
                    };

                    dbContext.MasterModuleRepository.Add(newModule);
                    dbContext.Commit(); // Save changes to the database

                    // Return the new module's ID
                    return newModule.ID; // Assuming Id is of type Guid
                }
            }
        }

        public Guid GetIdModuleProcess(Guid idmodule)
        {
            using (var dbContext = new DbHelper(true))
            {
                // Check if the module exists
                var existingModule = dbContext.MasterLevelModuleProcessRepository.Find(new { IDModule = idmodule, Level = 1 }).FirstOrDefault();

                if (existingModule != null)
                {
                    // Module exists, return its ID
                    return existingModule.ID; // Assuming Id is of type Guid
                }
                else
                {
                    // Module does not exist, create a new one
                    var newModule = new MasterLevelModuleProcess // Assuming your entity class is named Module
                    {
                        IDModule = idmodule,
                        Level = 1,
                        IdRole = GetIdRole(),
                        Desc = "-",
                        CreatedBy = "System",
                        CreatedOn = DateTime.Now
                    };

                    dbContext.MasterLevelModuleProcessRepository.Add(newModule);
                    dbContext.Commit(); // Save changes to the database

                    // Return the new module's ID
                    return newModule.ID; // Assuming Id is of type Guid
                }
            }
        }

        public Guid GetIdRole()
        {
            using (var dbContext = new DbHelper(true))
            {
                // Check if the module exists
                var existingModule = dbContext.RoleRepository.Find(new { Name = "VM_Staff" }).FirstOrDefault();

                if (existingModule != null)
                {
                    // Module exists, return its ID
                    return existingModule.ID; // Assuming Id is of type Guid
                }
                else
                {
                    // Module does not exist, create a new one
                    var newModule = new Role // Assuming your entity class is named Module
                    {
                        Name = "VM_Staff",
                        Description = "VM Staff",
                        CreatedBy = "System",
                        CreatedOn = DateTime.Now
                    };

                    dbContext.RoleRepository.Add(newModule);
                    dbContext.Commit(); // Save changes to the database

                    // Return the new module's ID
                    return newModule.ID; // Assuming Id is of type Guid
                }
            }
        }

        public void Delete(Guid id)
        {
            User user = Db.UserRepository.Find(new { ID = id }).FirstOrDefault();
            user.RowStatus = false;
            user.ModifiedOn = DateTime.Now;
            user.ModifiedBy = SessionManager.Current;
            string[] columns = new string[] { "RowStatus", "ModifiedOn", "ModifiedBy" };
            Db.UserRepository.Update(user, columns);
        }

        public string ResetPassword(ResetPassword data)
        {
            try
            {
                User user = Db.UserRepository.Find(new { ID = data.IdResetPW }).FirstOrDefault();
                user.Password = data.ResetNewPW;
                user.ModifiedOn = DateTime.Now;
                user.ModifiedBy = SessionManager.Current;
                string[] columns = new string[] { "Password", "ModifiedOn", "ModifiedBy" };
                Db.UserRepository.Update(user, columns);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string ChangePassword(ChangePassword data)
        {
            try
            {
                User user = Db.UserRepository.Find(new { ID = data.Id }).FirstOrDefault();
                user.Password = data.NewPW;
                user.ModifiedOn = DateTime.Now;
                user.ModifiedBy = SessionManager.Current;
                string[] columns = new string[] { "Password", "ModifiedOn", "ModifiedBy" };
                Db.UserRepository.Update(user, columns);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Claim[] Authenticate(LoginViewModel model)
        {
            if (string.IsNullOrEmpty(model.Username))
                throw new ModelException("Username", "Username not found");
            var username = model.Username;
            var password = model.TamUserPwd;
            var x = MD5Helper.Encode(password);

            var user = GetUserByUsername(username);
            var roles = GetRolesByUsername(model.Username);

            if (user == null) throw new ModelException("Username", "Username not found");
            if (password == null) throw new ModelException("Password", "Password not found");
            if (MD5Helper.Encode(password) != user.Password) throw new ModelException("Password", "Password not match");
            if (roles.Contains("User") && roles.Count() == 1) throw new ModelException("Username", "User has no valid Role");

            //if (user.NoReg == null)
            //    user.NoReg = "";

            var claims = new[]
            {
                new Claim("Name", user.Name),
                new Claim("Username", user.Username),
                new Claim("Roles", string.Join(",", roles)),
                //new Claim("NoReg", user.NoReg),
                //new Claim("Permission", "test,123,App.Core.User")
            };

            return claims;
        }

        public Claim[] AuthenticateWithoutPW(LoginViewModel model)
        {
            if (string.IsNullOrEmpty(model.Username))
                throw new ModelException("Username", "Username not found");
            var username = model.Username;
            var password = model.TamUserPwd;
            var x = MD5Helper.Encode(password);

            var user = GetUserByUsername(username);
            var roles = GetRolesByUsername(model.Username);

            if (user == null) throw new ModelException("Username", "Username not found");
            if (password == null) throw new ModelException("Password", "Password not found");
            if (roles.Contains("User") && roles.Count() == 1) throw new ModelException("Username", "User has no valid Role");

            //if (user.NoReg == null)
            //    user.NoReg = "";

            var claims = new[]
            {
                new Claim("Name", user.Name),
                new Claim("Username", user.Username),
                new Claim("Roles", string.Join(",", roles)),
                //new Claim("NoReg", user.NoReg),
                //new Claim("Permission", "test,123,App.Core.User")
            };

            return claims;
        }
    }
}
