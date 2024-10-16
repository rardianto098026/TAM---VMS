using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TAM.VMS.Domain
{
    public partial interface IDbHelper : IDisposable
    {
        // Connection and Transaction Object  
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }

        // Model Repositories 
        IUserRepository UserRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        IUserRoleViewRepository UserRoleViewRepository { get; }
        IRoleRepository RoleRepository { get; }
        IRolePermissionRepository RolePermissionRepository { get; }
        IMenuGroupRepository MenuGroupRepository { get; }
        IMenuRepository MenuRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IGeneralCategoryRepository GeneralCategoryRepository { get; } 
        IConfigRepository ConfigRepository { get; }
        IEmailTemplateRepository EmailTemplateRepository { get; }


        ITaskListRepository TaskListRepository { get; }
        ITaskViewRepository TaskViewRepository { get; }
        IRequestViewRepository RequestViewRepository { get; }
        IDownloadVendorDatabaseRepository DownloadVendorDatabaseRepository { get; }

        IMasterModuleRepository MasterModuleRepository { get; }
        IMasterLevelModuleProcessRepository MasterLevelModuleProcessRepository { get; }
        IMasterBusinessCategoriesRepository MasterBusinessCategoriesRepository { get; }
        IBusinessCategoryDisplayViewRepository BusinessCategoryDisplayViewRepository { get; }
        IMasterDepartmentRepository MasterDepartmentRepository { get; }
        IMasterVATRepository MasterVATRepository { get; }
        IVATDisplayViewRepository VATDisplayViewRepository { get; }

    }
    public class DbHelper : DbContext, IDbHelper
    {
        public DbHelper(bool UseTransaction = false) : base(UseTransaction)
        {
        }

        public IDbConnection Connection
        {
            get
            {
                return base._connection;
            }
        }

        public IDbTransaction Transaction
        {
            get
            {
                return base._transaction;
            }
        }


        private IUserRepository _UserRepository;
        private IUserRoleRepository _UserRoleRepository;
        private IUserRoleViewRepository _UserRoleViewRepository;
        private IRoleRepository _RoleRepository;
        private IMenuGroupRepository _MenuGroupRepository;
        private IMenuRepository _MenuRepository;
        private IPermissionRepository _PermissionRepository;
        private IConfigRepository _ConfigRepository;
        private IEmailTemplateRepository _EmailTemplateRepository;
        private IGeneralCategoryRepository _GeneralCategoryRepository;
        private IRolePermissionRepository _RolePermissionRepository;
        private ITaskListRepository _TaskListRepository;
        private ITaskViewRepository _TaskViewRepository;
        private IRequestViewRepository _RequestViewRepository;
        private IDownloadVendorDatabaseRepository _DownloadVendorDatabaseRepository;
        private IMasterModuleRepository _MasterModuleRepository;
        private IMasterLevelModuleProcessRepository _MasterLevelModuleProcessRepository;
        private IMasterBusinessCategoriesRepository _MasterBusinessCategoriesRepository;
        private IBusinessCategoryDisplayViewRepository _BusinessCategoryDisplayViewRepository;
        private IMasterDepartmentRepository _MasterDepartmentRepository;
        private IMasterVATRepository _MasterVATRepository;
        private IVATDisplayViewRepository _VATDisplayViewRepository;

        public IUserRepository UserRepository
        {
            get { return _UserRepository ?? (_UserRepository = new UserRepository(_connection, _transaction)); }
        }
        public IUserRoleRepository UserRoleRepository
        {
            get { return _UserRoleRepository ?? (_UserRoleRepository = new UserRoleRepository(_connection, _transaction)); }
        }

        public IUserRoleViewRepository UserRoleViewRepository
        {
            get { return _UserRoleViewRepository ?? (_UserRoleViewRepository = new UserRoleViewRepository(_connection, _transaction)); }
        }
        public IRoleRepository RoleRepository
        {
            get { return _RoleRepository ?? (_RoleRepository = new RoleRepository(_connection, _transaction)); }
        }
        public IRolePermissionRepository RolePermissionRepository
        {
            get { return _RolePermissionRepository ?? (_RolePermissionRepository = new RolePermissionRepository(_connection, _transaction)); }
        }
        public IConfigRepository ConfigRepository
        {
            get { return _ConfigRepository ?? (_ConfigRepository = new ConfigRepository(_connection, _transaction)); }
        }
        public IEmailTemplateRepository EmailTemplateRepository
        {
            get { return _EmailTemplateRepository ?? (_EmailTemplateRepository = new EmailTemplateRepository(_connection, _transaction)); }
        }
        public IGeneralCategoryRepository GeneralCategoryRepository
        {
            get { return _GeneralCategoryRepository ?? (_GeneralCategoryRepository = new GeneralCategoryRepository(_connection, _transaction)); }
        }
        public IMenuGroupRepository MenuGroupRepository
        {
            get { return _MenuGroupRepository ?? (_MenuGroupRepository = new MenuGroupRepository(_connection, _transaction)); }
        }
        public IMenuRepository MenuRepository
        {
            get { return _MenuRepository ?? (_MenuRepository = new MenuRepository(_connection, _transaction)); }
        }
        public IPermissionRepository PermissionRepository
        {
            get { return _PermissionRepository ?? (_PermissionRepository = new PermissionRepository(_connection, _transaction)); }
        }

        public ITaskListRepository TaskListRepository
        {
            get { return _TaskListRepository ?? (_TaskListRepository = new TaskListRepository(_connection, _transaction)); }
        }
        public ITaskViewRepository TaskViewRepository
        {
            get { return _TaskViewRepository ?? (_TaskViewRepository = new TaskViewRepository(_connection, _transaction)); }
        }
        public IRequestViewRepository RequestViewRepository
        {
            get { return _RequestViewRepository ?? (_RequestViewRepository = new RequestViewRepository(_connection, _transaction)); }
        }
        public IDownloadVendorDatabaseRepository DownloadVendorDatabaseRepository
        {
            get { return _DownloadVendorDatabaseRepository ?? (_DownloadVendorDatabaseRepository = new DownloadVendorDatabaseRepository(_connection, _transaction)); }

        }
        public IMasterModuleRepository MasterModuleRepository
        {
            get { return _MasterModuleRepository ?? (_MasterModuleRepository = new MasterModuleRepository(_connection, _transaction)); }
        }
        public IMasterLevelModuleProcessRepository MasterLevelModuleProcessRepository
        {
            get { return _MasterLevelModuleProcessRepository ?? (_MasterLevelModuleProcessRepository = new MasterLevelModuleProcessRepository(_connection, _transaction)); }
        }
        public IMasterBusinessCategoriesRepository MasterBusinessCategoriesRepository
        {
            get { return _MasterBusinessCategoriesRepository ?? (_MasterBusinessCategoriesRepository = new MasterBusinessCategoriesRepository(_connection, _transaction)); }
        }
        public IBusinessCategoryDisplayViewRepository BusinessCategoryDisplayViewRepository
        {
            get { return _BusinessCategoryDisplayViewRepository ?? (_BusinessCategoryDisplayViewRepository = new BusinessCategoryDisplayViewRepository(_connection, _transaction)); }
        }
        public IMasterDepartmentRepository MasterDepartmentRepository
        {
            get { return _MasterDepartmentRepository ?? (_MasterDepartmentRepository = new MasterDepartmentRepository(_connection, _transaction)); }
        }
        public IMasterVATRepository MasterVATRepository
        {
            get { return _MasterVATRepository ?? (_MasterVATRepository = new MasterVATRepository(_connection, _transaction)); }
        }
        public IVATDisplayViewRepository VATDisplayViewRepository
        {
            get { return _VATDisplayViewRepository ?? (_VATDisplayViewRepository = new VATDisplayViewRepository(_connection, _transaction)); }
        }



        public override void resetRepositories()
        {
            _UserRepository = null;
            _UserRoleRepository = null;
            _UserRoleViewRepository = null;
            _RoleRepository = null;
            _RolePermissionRepository = null;
            _ConfigRepository = null;
            _EmailTemplateRepository = null;
            _GeneralCategoryRepository = null;
            _MenuGroupRepository = null;
            _PermissionRepository = null;
            _MenuRepository = null;
            _TaskListRepository = null;
            _TaskViewRepository = null;
            _RequestViewRepository = null;
            _DownloadVendorDatabaseRepository = null;
            _MasterModuleRepository = null;
            _MasterBusinessCategoriesRepository = null;
            _BusinessCategoryDisplayViewRepository = null;
            _MasterDepartmentRepository = null;
            _MasterVATRepository = null;
            _VATDisplayViewRepository = null;
        }
    }
}
