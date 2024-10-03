using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Middleware
{
    public enum EnumContext
    {
        Pessoa,
        Sistema
    }
    public interface IConexao
    {
        DbContext this [EnumContext enumContext] { get; }
        T GetDbContext<T>(EnumContext enumContext) where T : DbContext;
        IDbContextTransaction? BeginTransaction();
        IDbContextTransaction? BeginTransaction(DbContext context);
        Task<IDbContextTransaction?> BeginTransactionAsync();
        Task<IDbContextTransaction?> BeginTransactionAsync(DbContext context);
    } 
    public static class IConexaoExtension
    {
        public static void Confirmar(this IDbContextTransaction? transaction)
        {
            if (transaction != null)
                transaction.Commit();
        }
        public static Task ConfirmarAsync(this IDbContextTransaction? transaction)
        {
            if (transaction != null)
                return transaction.CommitAsync();

            return Task.CompletedTask;
        }
        public static void Reverter(this IDbContextTransaction? transaction)
        {
            if (transaction != null)
                transaction.Rollback();
        }
        public static Task ReverterAsync(this IDbContextTransaction? transaction)
        {
            if (transaction != null)
                return transaction.RollbackAsync();

            return Task.CompletedTask;
        }
    }
}