﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModels.Sales;
using System.Data.Entity;

namespace DataAccessLayer.Providers
{
    public class InvoicesRepo : IInvoicesRepo
    {
        private readonly DataContext dataContext;
        internal InvoicesRepo(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<int> Add(InvoiceMaster invoice)
        {
            this.dataContext.Invoices.Add(invoice);
            return await this.dataContext.SaveChangesAsync();
        }

        public async Task<ICollection<InvoiceMaster>> Get()
        {
            var result = await (from inv in this.dataContext.Invoices
                                
                                orderby inv.ID descending
                                select inv).Include(i => i.InvoiceChildren).ToListAsync();
            
            return result;
        }

        public async Task<InvoiceMaster> Get(int id)
        {
            var result = await this.dataContext.Invoices
                                .Where(i => i.ID == id)
                                .Include(i => i.InvoiceChildren.Select(p => p.Item))
                                .FirstOrDefaultAsync();
            
            return result;
        }

        public Task<int> Update(InvoiceMaster item)
        {
            this.dataContext.Entry(item).State = EntityState.Modified;
            return this.dataContext.SaveChangesAsync();
        }
    }
}
