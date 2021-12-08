using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using web_lab2.Abstractions;
using web_lab2.Models;

namespace web_lab2.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private readonly IServiceProvider _serviceProvider;

        private IBookRepository _bookRepository;
        private IOrderRepository _orderRepository;
        private ISageRepository _sageRepository;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;

        public UnitOfWork(DatabaseContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public IBookRepository Books =>
            _bookRepository ??= _serviceProvider.GetService<IBookRepository>();

        public IOrderRepository Orders =>
            _orderRepository ??= _serviceProvider.GetService<IOrderRepository>();

        public ISageRepository Sages =>
            _sageRepository ??= _serviceProvider.GetService<ISageRepository>();

        public IUserRepository Users =>
            _userRepository ??= _serviceProvider.GetService<IUserRepository>();

        public IRoleRepository Roles =>
            _roleRepository ??= _serviceProvider.GetService<IRoleRepository>();

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}