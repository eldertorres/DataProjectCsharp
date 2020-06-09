﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProjectCsharp.Models.Repository
{

    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _db;

        public Repository(ApplicationDbContext db)
        {
            this._db = db;
        }

        public bool PortfoliosExists(string userId)
        {
            return (_db.Portfolios.FirstOrDefault(p => p.UserId == userId) != null);
        }

        public void AddPortfolio(Portfolio portfolio)
        {
            _db.Portfolios.Add(portfolio);
        }

        public void AddTrade(Trade trade)
        {
            _db.Trades.Add(trade);
        }

        public void UpdateTrade(Trade trade)
        {
            _db.Trades.Update(trade);
        }
        public void RemoveTrade(Trade trade)
        {
            _db.Trades.Remove(trade);
        }

        public List<Portfolio> GetAllUserPortfolios(string userId)
        {
           return  _db.Portfolios
                    .Where(p => p.UserId == userId)
                    .Include(p => p.Trades)
                    .ToList();
        }

        public List<Trade> GetAllUserTrades(int portfolioId, string userId)
        {
            return _db.Trades
                    .Where(t => t.PortfolioId == portfolioId && t.UserId == userId)
                    .OrderBy(t => t.TradeDate)
                    .ToList();
        }

        public Trade GetTrade(int? tradeId)
        {
            return _db.Trades.Find(tradeId);
        }

        public List<SecurityPrices> GetSecurityPrices(string symbol)
        {
            return _db.SecurityPrices.Where(t => t.ticker == symbol).OrderBy(t => t.date).ToList();
        }

        public Portfolio GetUserPortfolio(int? portfolioId, string userId)
        {
            return _db.Portfolios
                    .Where(p => p.PortfolioId == portfolioId && p.UserId == userId)
                    .Include(p => p.Trades)
                    .FirstOrDefault();
        }

        public bool IsDuplicatePortfolio(string name, string userId)
        {
            return _db.Portfolios.Any(p => p.Name == name && p.UserId == userId);
        }


        public async Task<bool> SaveChangesAsync()
        {
            return (await _db.SaveChangesAsync() > 0);
        }

        public void AddSecurityPrice(SecurityPrices price)
        {
            _db.SecurityPrices.Add(price);
        }

        public bool IsSecurityStored(string symbol)
        {
            return (_db.SecurityPrices.Where(sp => sp.ticker == symbol).FirstOrDefault() != null);
        }
    }
}
