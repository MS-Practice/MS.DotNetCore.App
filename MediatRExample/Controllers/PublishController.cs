using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using MediatRExample.DbProvider;
using MediatRExample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediatRExample.Controllers
{
    //[Produces("application/json")]
    //[Route("api/Publish")]
    public class PublishController : Controller, ICapSubscribe
    {
        [Route("~/publishWithTransactionUsingEF")]
        public async Task<IActionResult> PublishMessageWithTransactionUsingEF([FromServices]AppDbContext dbContext, [FromServices]ICapPublisher publisher)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                await publisher.PublishAsync("ms.services.account.check", new Person { Name = "Marson", Age = 25 });
                trans.Commit();
            }
            return Ok();
        }

        [CapSubscribe("ms.services.account.check")]
        public Task CheckReceivedMessage(Person person)
        {
            Console.WriteLine(person.Name);
            Console.WriteLine(person.Age);
            return Task.CompletedTask;
        }

        [CapSubscribe("ms.services.account.check")]
        public string KafkaTestReceived(Person person)
        {
            Console.WriteLine(person);
            Debug.WriteLine(person);
            return "this is callback message";
        }
    }
}