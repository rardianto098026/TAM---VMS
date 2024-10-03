﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace TAM.VMS.Domain
{
    public abstract class DbContext
    {
        protected IDbConnection _connection;
        protected IDbTransaction _transaction;

        private IDbConnection OpenConnection()
        {
            var config = new AppConfiguration();
            var builder = new SqlConnectionStringBuilder(config.ConnectionString);
            var conn = new SqlConnection(builder.ConnectionString);

            return conn;
        }

        public DbContext()
        {
            _connection = OpenConnection();
        }

        public DbContext(bool UseTransaction)
        {
            _connection = OpenConnection();
            if (UseTransaction)
            {
                _connection.Open();
                _transaction = _connection.BeginTransaction();
            }
        }

        protected bool _disposed;

        public void Commit()
        {

            if (_transaction == null)
                return;
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                resetRepositories();
            }
        }

        public virtual void resetRepositories()
        {
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~DbContext()
        {
            dispose(false);
        }
    }
}
