using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelebuHubChat.DbContexts;
using TelebuHubChat.Models;
using Microsoft.AspNetCore.JsonPatch;
using TelebuHubChat.Helpers;
using System.Collections.Generic;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using TelebuHubChat.LogClasses;

namespace TelebuHubChat.Controllers
{
    [Route("accounts")]
    [ApiController]
    public class WidgetsController : ControllerBase
    {
        private readonly TelebuHubChatContext _context;

        public WidgetsController(TelebuHubChatContext context)
        {
            _context = context;
        }

        [HttpGet]
        public string GetWelcomeMsg()
        {
	   LogProperties.info("Application started");
            return "!!!Welcome to TelebuHubChat!!!";
        }
        // GET: accounts/5/widgets/ae5h5fga3fabh8ahjan6agbahvga5gah
        [HttpGet("{account_id}/widgets/{uuid}")]
        public async Task<ActionResult<Widgets>> GetWidgets(int account_id, string uuid)
        {

            var widget = new Widgets();
            widget = await _context.Widgets.Where(w => w.AccountId == account_id && String.Equals(w.UUID, uuid)).FirstOrDefaultAsync();
            var Themes = new WidgetThemes();
            Themes = await _context.WidgetThemes.Where(w => w.Id == widget.ThemeId).FirstOrDefaultAsync();

            widget.WidgetThemes = new WidgetThemes();
            widget.WidgetThemes = Themes;
            //var widgets = await _context.Widgets.Where(w => w.AccountId == account_id && String.Equals(w.UUID, uuid)).Join(_context.WidgetThemes , widList => widList.ThemeId , themes => themes.Id , (widList , themes) => 
            //new { themeId = widList.ThemeId,
            //    theme_Id = themes.Id
            //    } );



            if (widget == null)
            {
                return NotFound(new { Success = false, Message = "Widget Not Found" });
            }

            return Ok(new { Success = true, widgets = widget });
        }

        // GET: accounts/5/widgets
        [HttpGet("{account_id}/widgets")]
        public async Task<ActionResult<IEnumerable<Widgets>>> GetAllWidgets(int account_id)
        {
            var widgets = await _context.Widgets.Where(w => w.AccountId == account_id).ToListAsync();

            if (widgets == null)
            {
                return NotFound(new { Success = false, Message = "No Widgets Found" });
            }

            return Ok(new { Success = true, widgets = widgets });
        }

        //PATCH: accounts/5/widgets/ae5h5fga3fabh8ahjan6agbahvga5gah
        [HttpPatch("{account_id}/widgets/{uuid}")]
        public async Task<IActionResult> PatchWidgets(int account_id, string uuid, [FromBody]JsonPatchDocument<Widgets> widgets)
        {
            if (widgets.Operations.Count != 0)
            {
                var widget = await _context.Widgets.Where(w => w.AccountId == account_id && String.Equals(w.UUID, uuid)).FirstOrDefaultAsync();

                if (widget != null)
                {

                    //var elementsWithPathPurpose = widgets.Operations.Where(o => o.path.Equals("Purpose")).FirstOrDefault();

                    //if (elementsWithPathPurpose != null)
                    //{
                    //    var _purpose = await _context.Purposes.Where(p => string.Equals(elementsWithPathPurpose.value.ToString(), p.Purpose, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();
                    //    if (_purpose != null && string.Equals(elementsWithPathPurpose.value.ToString(), _purpose.Purpose, StringComparison.OrdinalIgnoreCase)==false)
                    //    {
                    //        widgets.Replace(e=>e.PurposeId, _purpose.Id);
                    //    }
                    //}

                    widget.UpdatedTimeUTC = DateTime.UtcNow;

                    widgets.ApplyTo(widget, ModelState);


                    if (!ModelState.IsValid)
                    {
                        return new BadRequestObjectResult(new { Success = false });
                    }
                    _context.Entry(widget).State = EntityState.Modified;
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!WidgetsExists(account_id))
                        {
                            return NotFound(new { Success = false, Message = "Widget with supplied " + account_id + " is not Existed" });
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    return NotFound(new { Success = false, Message = "Widget Not Found" });
                }

                return Ok(new { Success = true, widget = widget });
            }
            else
            {
                return Ok(new { Success = false, Message = "No Operations/Values Specified For Patching" });
            }
        }

        // POST: accounts/5/widgets
        [HttpPost("{account_id}/widgets")]
        public async Task<ActionResult<Widgets>> PostWidgets(int account_id, Widgets widgets)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data" });
            if (String.Equals(widgets.DomainToLoadIn, "") == true || widgets.DomainToLoadIn == null)
                return BadRequest(new { Success = false, Message = "DomainToLoadIn Can't be Empty or Null" });
            if (String.Equals(widgets.Purpose, "") == true || widgets.Purpose == null)
                return BadRequest(new { Success = false, Message = "Purpose Can't be Empty or Null" });

            //var _purpose =await _context.Purposes.Where(p => string.Equals(widgets.Purpose, p.Purpose, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();

            var widget = new Widgets();

            widget.AccountId = account_id;
            widget.WidgetName = widgets.WidgetName;
            widget.IsActive = widgets.IsActive;
            widget.ExpiryDateUTC = (DateTime.UtcNow).AddMonths(6);
            widget.PopInAfterSeconds = widgets.PopInAfterSeconds;
            widget.ThemeId = widgets.ThemeId;
            widget.WorkFlowId = widgets.WorkFlowId;
            //if (_purpose != null){
            //    widget.PurposeId = _purpose.Id;
            //}else{
            //    var __purpose = new Purposes();
            //    __purpose.Purpose = widgets.Purpose;
            //    _context.Purposes.Add(__purpose);
            //    await _context.SaveChangesAsync();
            //    widget.PurposeId = __purpose.Id;
            //}
            widget.Purpose = widgets.Purpose;
            widget.MetaData = widgets.MetaData;
            widget.MinimizeStateText = widgets.MinimizeStateText;
            widget.UUID = RandomStringGenerator.RandomString(32);
            widget.DomainToLoadIn = widgets.DomainToLoadIn;
            widget.CreatedTimeUTC = DateTime.UtcNow;
            widget.UpdatedTimeUTC = DateTime.UtcNow;
            widget.CustomMetaData = widgets.CustomMetaData;
            widget.AgentAndCustomerWaitTimeRestrictionInSec = widgets.AgentAndCustomerWaitTimeRestrictionInSec;
            widget.TimeToDisplayWelcomeFormToCustomer = widgets.TimeToDisplayWelcomeFormToCustomer;
            widget.AutoCloseTimeForChatInMin = widgets.AutoCloseTimeForChatInMin;
            widget.WhileConnectingToAnAgent = widgets.WhileConnectingToAnAgent;
            widget.CustomerWaitTimeForAgentConnect = widgets.CustomerWaitTimeForAgentConnect;
            widget.BotChatClosure = widgets.BotChatClosure;
            widget.AgentChatClosure = widgets.AgentChatClosure;
            widget.NonBusinessConnect = widgets.NonBusinessConnect;
            widget.TimeSlotId = widgets.TimeSlotId;
            widget.CustomMessageForChatIcon = widgets.CustomMessageForChatIcon;

            _context.Widgets.Add(widget);
            await _context.SaveChangesAsync();

            //var _welcomeFormsTable = new WelcomeFormsTable();
            //_welcomeFormsTable.WidgetId = widget.Id;
            //_welcomeFormsTable.MetaData = widgets.MetaData;

            //_context.WelcomeFormsTables.Add(_welcomeFormsTable);
            //await _context.SaveChangesAsync();

            return Ok(new { Success = true, widget = widget });
        }


        private bool WidgetsExists(int id)
        {
            return _context.Widgets.Any(e => e.Id == id);
        }


        // DELETE: accounts/5/widgets/ae5h5fga3fabh8ahjan6agbahvga5gah
        [HttpDelete("{account_id}/widgets/{uuid}")]
        public async Task<ActionResult<Widgets>> DeleteWidgets(int account_id, string uuid)
        {
            var widgets = _context.Widgets.Where(w => w.AccountId == account_id && String.Equals(w.UUID, uuid))
                .FirstOrDefault();

            if (widgets == null)
            {
                return NotFound(new { Success = false, Message = "Widget Not Found" });
            }

            _context.Widgets.Remove(widgets);

            await _context.SaveChangesAsync();

            return Ok(new { Success = true, Message = "Widget Deleted Successfully" });
        }
    }
}
