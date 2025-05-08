using BookEvent.Shared.Models._Common.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEvent.Core.Application.Abstraction.Services.Emails
{
    public interface IEmailService
    {
        public Task SendEmail(Email email);

    }
}
